/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Base class for multi-bindings and binding markup extensions that combine multiple components (values).
    /// </summary>
    public abstract class MultiBindBase : MarkupExtension
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public override object? ProvideValue( IServiceProvider? serviceProvider )
        {
            if( serviceProvider == null )
            {
                return this;
            }

            var valueProvider = (IProvideValueTarget?) serviceProvider.GetService( typeof( IProvideValueTarget ) );
            if( valueProvider == null )
            {
                return this;
            }

            var targetObject = valueProvider.TargetObject;
            var targetProperty = valueProvider.TargetProperty;

            var forceDynamic = IsTargetCultureRelevant( targetProperty );

            var binding = PrepareBinding( forceDynamic );

            if( binding != null )
            {
                return ProvideDynamicValue( serviceProvider, binding, targetObject, targetProperty );
            }
            else
            {
                return ProvideStaticValue( targetObject, targetProperty );
            }
        }

        private bool IsTargetCultureRelevant( object targetProperty )
        {
            if( targetProperty is DependencyProperty targetDependencyProperty )
            {
                var targetPropertyType = targetDependencyProperty.PropertyType;
                if( targetPropertyType == typeof( string ) )
                {
                    return true;
                }
            }

            return false;
        }

        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MultiBindBase()
        {
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Calculates the effective value of the binding from the effective (calculated) values of its components.
        /// </summary>
        /// <remarks>
        /// <para>Must be implemented by derived classes to perform the actual calculation.</para>
        /// <para>The target type is necessary for the implementation of <see cref="MultiBind"/>, it can be ignored by
        ///       other markup extensions because conversion to the target type will be performed by the caller.</para>
        /// </remarks>
        /// <param name="componentValues">Array with the effective values of the components.</param>
        /// <param name="componentCultures">Array with the cultures of the components.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetCulture">Culture of the target.</param>
        /// <returns>Value of the binding and its associated culture.</returns>
        protected abstract (object? value, CultureInfo? culture) CalculateValue( object?[] componentValues, CultureInfo?[] componentCultures,
                                                                                 Type targetType, CultureInfo targetCulture );

        /// <summary>
        /// Calculates the source values for the components from the effective value of the binding target.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="targetValue">Effective value of the binding target.</param>
        /// <param name="targetCulture">Culture of the binding target.</param>
        /// <param name="sourceTypes">Types of the component sources.</param>
        /// <param name="currentValues">Current values of the components.</param>
        /// <returns>An array with the source values for the components, or <c>null</c> if the calculation cannot be performed or is not feasible.</returns>
        protected abstract object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, ComponentValue[] currentValues );

        /// <summary>
        /// Creates the internal binding.
        /// </summary>
        /// <returns>Internal binding.</returns>
        protected virtual MultiBinding CreateBinding()
        {
            var mode = GetBindingMode();
            if( mode == BindingMode.Default )
            {
                mode = BindingMode.OneWay;
            }

            var trigger = GetUpdateSourceTrigger();

            return new MultiBinding
            {
                Mode = mode,
                UpdateSourceTrigger = trigger,
            };
        }

        //===========================================================================
        //                          PROTECTED NESTED TYPES
        //===========================================================================

        /// <summary>
        /// Represents the effective value of a component in a multi-binding.
        /// </summary>
        protected class ComponentValue
        {
            /// <summary>
            /// Effective value of the component.
            /// </summary>
            public object? Value { get; internal set; }

            /// <summary>
            /// Culture of the component (set if relevant).
            /// </summary>
            public CultureInfo? Culture { get; }

            /// <summary>
            /// Indicates whether the component is reversible (i.e., it's source is a binding or has nested bindings
            /// that back-propagate values to the binding source).
            /// </summary>
            public bool IsReversible { get; }

            public ComponentValue( object? value, CultureInfo? culture, bool isReversible )
            {
                Value = value;
                Culture = culture;
                IsReversible = isReversible;
            }
        }

        //===========================================================================
        //                          PRIVATE NESTED TYPES
        //===========================================================================

        /// <summary>
        /// Represents the effective value of a multi-bind.
        /// </summary>
        private class MultiBindValue : ComponentValue
        {
            /// <summary>
            /// Effective values of the components.
            /// </summary>
            public ComponentValue[] ComponentsValues { get; }

            public MultiBindValue( object? value, CultureInfo? culture, bool isReversible, ComponentValue[] componentRecords ) : base( value, culture, isReversible )
            {
                ComponentsValues = componentRecords;
            }
        }

        private class InternalConverter : IMultiValueConverter
        {
            public MultiBindingExpression? BindingExpression { get; set; }

            public InternalConverter( MultiBindBase binding )
            {
                m_binding = binding;
            }

            public object? Convert( object?[] bindingValues, Type targetType, object? parameter, CultureInfo culture )
            {
                Debug.Assert( BindingExpression != null );

                var bindingExpressions = BindingExpression!.BindingExpressions;
                Debug.Assert( bindingValues.Length == bindingExpressions.Count );

                var mode = BindingExpression.ParentMultiBinding.Mode;

                if( mode == BindingMode.OneWayToSource )
                {
                    return DependencyProperty.UnsetValue;
                }

                var calculatedValue = m_binding.CalculateEffectiveValue( bindingValues.GetEnumerator(), bindingExpressions.GetEnumerator(), targetType, culture );

                if( mode == BindingMode.TwoWay )
                {
                    m_lastValue = calculatedValue;
                }

                return Helper.TryConvertValue( calculatedValue.Value, targetType, culture );
            }

            public object?[]? ConvertBack( object? value, Type[] targetTypes, object? parameter, CultureInfo culture )
            {
                Debug.Assert( BindingExpression != null );

                var bindingExpressions = BindingExpression!.BindingExpressions;

                var targetTypesEnumerator = ( (IEnumerable<Type>) targetTypes ).GetEnumerator();

                var bindingValues = new List<object?>();

                if( m_lastValue == null )
                {
                    // This should only happen when binding mode is OneWayToSource.
                    m_lastValue = m_binding.PrepareMultiBindValue( bindingExpressions, typeof( object ), culture );
                }

                if( m_binding.CalculateBackBindingValues( value, culture, targetTypesEnumerator, m_lastValue, bindingValues ) )
                {
                    var result = bindingValues.ToArray();

                    Debug.Assert( result.Length == targetTypes.Length );

                    return result;
                }
                else
                {
                    return null;
                }
            }

            private readonly MultiBindBase m_binding;
            private MultiBindValue? m_lastValue;
        }


        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private MultiBinding? PrepareBinding( bool forceCreation )
        {
            MultiBinding? multiBinding = null;

            var bindings = GetBindings();

            if( forceCreation || bindings.Any() )
            {
                var converter = new InternalConverter( this );

                multiBinding = CreateBinding();

                multiBinding.Converter = converter;

                foreach( var binding in bindings )
                {
                    multiBinding.Bindings.Add( binding.InternalBinding );
                }
            }

            return multiBinding;
        }

        private IEnumerable<Bind> GetBindings()
        {
            foreach( var value in RawComponentValues )
            {
                if( value is Bind binding )
                {
                    yield return binding;
                }
                else if( value is MultiBindBase customBinding )
                {
                    foreach( var nestedBinding in customBinding.GetBindings() )
                    {
                        yield return nestedBinding;
                    }
                }
            }
        }

        private object ProvideDynamicValue( IServiceProvider serviceProvider, MultiBinding multiBinding, object? targetObject, object? targetProperty )
        {
            if( ( targetObject == null ) || ( targetObject is MultiBindBase ) )
            {
                return this;
            }

            if( ( targetObject is DependencyObject ) && ( targetProperty is DependencyProperty ) )
            {
                var providedValue = multiBinding.ProvideValue( serviceProvider );

                // Store the binding expression to obtain the nested bindings expressions once the binding is
                // attached and the nested binding expressions are available.
                ( (InternalConverter) multiBinding.Converter ).BindingExpression = (MultiBindingExpression) providedValue;

                return providedValue;
            }
            else
            {
                return this;
            }
        }

        private object? ProvideStaticValue( object? targetObject, object? targetProperty )
        {
            Type targetType;

            if( targetProperty is DependencyProperty targetDependencyProperty )
            {
                if( !( targetObject is DependencyObject ) )
                {
                    // This is the case when instantiated inside a template.
                    return this;
                }

                targetType = targetDependencyProperty.PropertyType;
            }
            else if( targetProperty is PropertyInfo targetPropertyInfo )
            {
                targetType = targetPropertyInfo.PropertyType;
            }
            else
            {
                targetType = typeof( object );
            }

            var targetCulture = GetTargetCulture( targetObject );

            var staticValue = GetStaticValue( targetType, targetCulture );

            return Helper.TryConvertValue( staticValue, targetType, targetCulture );
        }

        public object? GetStaticValue( Type targetType, CultureInfo targetCulture )
        {
            var emptyBindingExpressions = new List<BindingExpression>();
            var calculatedValue = CalculateEffectiveValue( Array.Empty<object>().GetEnumerator(), emptyBindingExpressions.GetEnumerator(),
                                                           targetType, targetCulture );
            return calculatedValue.Value;
        }

        private static CultureInfo GetTargetCulture( object? targetObject )
        {
            CultureInfo? culture = null;

            if( targetObject is FrameworkElement frameworkElement )
            {
                var language = frameworkElement.GetValue( FrameworkElement.LanguageProperty ) as XmlLanguage;
                if( language != null )
                {
                    culture = language.GetSpecificCulture();
                }
            }

            return culture ?? CultureInfo.InvariantCulture;
        }

        private MultiBindValue PrepareMultiBindValue( ReadOnlyCollection<BindingExpressionBase> bindingExpressions, Type targetType, CultureInfo targetCulture )
        {
            var bindingValues = new object[ bindingExpressions.Count ];

            for( int i = 0; i < bindingValues.Length; i++ )
            {
                bindingValues[ i ] = DependencyProperty.UnsetValue;
            }

            return CalculateEffectiveValue( bindingValues.GetEnumerator(), bindingExpressions.GetEnumerator(), targetType, targetCulture );
        }

        /// <summary>
        /// Calculates the effective value of the binding from the effective values of its components.
        /// </summary>
        /// <remarks>
        /// <para>The effective value of static (constant) components is intrinsically the component raw value.</para>
        /// <para>The effective value of nested MultiBindBase-derived components is calculated recursively.</para>
        /// <para>The effective value of binding components is obtained from the binding source, which is passed in <paramref name="bindingValues"/>.</para>
        /// </remarks>
        /// <param name="bindingValues">Enumerator for binding values.</param>
        /// <param name="bindingExpressions">Enumerator for binding expressions.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetCulture">Culture of the target.</param>
        /// <returns>Effective value of the binding and its associated culture.</returns>
        private MultiBindValue CalculateEffectiveValue( IEnumerator bindingValues, IEnumerator<BindingExpressionBase> bindingExpressions,
                                                        Type targetType, CultureInfo targetCulture )
        {
            var componentValues = new ComponentValue[ RawComponentValues.Count ];

            var isReversible = false;

            object? errorValue = null;

            // Initialize the component types is necessary (e.g, MultiBind does not initialize them).
            while( ComponentTypes.Count < RawComponentValues.Count )
            {
                ComponentTypes.Add( typeof( object ) );
            }

            for( int i = 0; i < RawComponentValues.Count; i++ )
            {
                var rawComponentValue = RawComponentValues[ i ];
                var componentType = ComponentTypes[ i ];

                ComponentValue componentValue;

                if( rawComponentValue is MultiBindBase markup )
                {
                    componentValue = markup.CalculateEffectiveValue( bindingValues, bindingExpressions, componentType, targetCulture );
                }
                else if( rawComponentValue is Bind binding )
                {
                    bindingValues.MoveNext();
                    bindingExpressions.MoveNext();

                    var bindingExpression = (BindingExpression) bindingExpressions.Current;

                    var bindingValue = bindingValues.Current;
                    var bindingCulture = binding.ConverterCulture ?? GetBindingSourceCulture( bindingExpression );

                    var bindingMode = bindingExpression?.ParentBinding.Mode;
                    var isBindingReversible = ( ( bindingMode == BindingMode.TwoWay ) || ( bindingMode == BindingMode.OneWayToSource ) );

                    componentValue = new ComponentValue( bindingValue, bindingCulture, isBindingReversible );

                    OnComponentBindingValueUpdated( i, bindingValue, bindingExpression! );
                }
                else
                {
                    componentValue = new ComponentValue( rawComponentValue, CultureInfo.InvariantCulture, false );
                }

                isReversible |= componentValue.IsReversible;

                if( componentValue.Value == Binding.DoNothing )
                {
                    errorValue = Binding.DoNothing;
                }
                else if( componentValue.Value == DependencyProperty.UnsetValue )
                {
                    componentValue.Value = GetDefaultValue( componentType );
                }
                else
                {
                    // Convert the component value to its expected type (if necessary).
                    // Note: Markup extensions can declare components with type Object in order to convert the value
                    // later in the calculation process when CalculateValue() is called.
                    componentValue.Value = Helper.ConvertValue( componentValue.Value, componentType, componentValue.Culture );
                }

                componentValues[ i ] = componentValue;
            }

            if( errorValue != null )
            {
                return new MultiBindValue( errorValue, null, isReversible, componentValues );
            }

            var effectiveComponentValues = componentValues.Select( c => c.Value ).ToArray();
            var componentCultures = componentValues.Select( c => c.Culture ).ToArray();

            var calculatedValue = CalculateValue( effectiveComponentValues, componentCultures, targetType, targetCulture );

            // Note: The calculated value is not converted to the target type because it may already have be converted when calculating the
            // value or it may be converted later by the caller.

            return new MultiBindValue( calculatedValue.value, calculatedValue.culture, isReversible, componentValues );
        }

        protected virtual void OnComponentBindingValueUpdated( int componentIndex, object? newValue, BindingExpression bindingExpression )
        {
        }

        /// <summary>
        /// Calculates the values of the binding component from the value of the binding target and with the help of the last value records.
        /// </summary>
        /// <param name="targetValue">Value of the binding target.</param>
        /// <param name="targetCulture">Culture of the binding target.</param>
        /// <param name="bindingSourceTypes">Enumerator for the binding source types.</param>
        /// <param name="currentValue">Current value.</param>
        /// <param name="bindingValues">[Output] Collection to which binding values are added.</param>
        /// <returns><c>true</c> if the calculation was successful; otherwise, <c>false</c>.</returns>
        private bool CalculateBackBindingValues( object? targetValue, CultureInfo targetCulture, IEnumerator<Type> bindingSourceTypes,
                                                 MultiBindValue currentValue, ICollection<object?> bindingValues )
        {
            var sourceTypes = new Type[ RawComponentValues.Count ];
            var currentComponentValues = currentValue.ComponentsValues;

            Debug.Assert( sourceTypes.Length == currentComponentValues.Length );

            // Get source types
            for( int i = 0; i < RawComponentValues.Count; i++ )
            {
                var rawComponentValue = RawComponentValues[ i ];
                Type sourceType;

                if( rawComponentValue is Bind binding )
                {
                    bindingSourceTypes.MoveNext();

                    sourceType = bindingSourceTypes.Current;
                }
                else
                {
                    sourceType = typeof( object );
                }

                sourceTypes[ i ] = sourceType;
            }

            object?[]? componentBackValues;

            if( ( targetValue == DependencyProperty.UnsetValue ) ||
                ( targetValue == Binding.DoNothing ) )
            {
                componentBackValues = null;
            }
            else
            {
                componentBackValues = CalculateBackValues( targetValue, targetCulture, sourceTypes, currentComponentValues );

                if( componentBackValues == null )
                {
                    return false;
                }

                if( componentBackValues.Length != RawComponentValues.Count )
                {
                    throw new Exception( $"CalculateBackValues returned a wrong number of values (expected {RawComponentValues.Count}, actual={componentBackValues.Length})" );
                }
            }

            for( int i = 0; i < RawComponentValues.Count; i++ )
            {
                var rawComponentValue = RawComponentValues[ i ];
                var currentComponentValue = currentComponentValues[ i ];
                var componentBackValue = componentBackValues?[ i ] ?? targetValue;

                if( rawComponentValue is MultiBindBase markup )
                {
                    if( !markup.CalculateBackBindingValues( componentBackValue, targetCulture, bindingSourceTypes,
                                                            (MultiBindValue) currentComponentValue, bindingValues ) )
                    {
                        return false;
                    }
                }
                else if( rawComponentValue is Bind binding )
                {
                    object? finalBackValue;

                    if( ( componentBackValue == DependencyProperty.UnsetValue ) ||
                        ( componentBackValue == Binding.DoNothing ) )
                    {
                        finalBackValue = componentBackValue;
                    }
                    else
                    {
                        var sourceCulture = currentComponentValues[ i ].Culture;
                        finalBackValue = Helper.TryConvertValue( componentBackValue, sourceTypes[ i ], sourceCulture );
                    }

                    bindingValues.Add( finalBackValue );
                }
                else
                {
                    if( ( componentBackValue != DependencyProperty.UnsetValue ) &&
                        ( componentBackValue != Binding.DoNothing ) &&
                        !Object.Equals( componentBackValue, currentComponentValue.Value ) )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static CultureInfo? GetBindingSourceCulture( BindingExpression bindingExpression )
        {
            CultureInfo? sourceCulture = null;

            var sourceObject = bindingExpression.ResolvedSource;
            if( sourceObject is FrameworkElement frameworkElement )
            {
                var sourcePropertyName = bindingExpression.ResolvedSourcePropertyName;

                var sourceType = frameworkElement.GetType();
                var descriptor = DependencyPropertyDescriptor.FromName( bindingExpression.ResolvedSourcePropertyName, sourceType, sourceType );

                if( descriptor != null )
                {
                    var sourcePropertyType = descriptor.DependencyProperty.PropertyType;
                    if( sourcePropertyType == typeof( string ) )
                    {
                        var sourceLanguage = frameworkElement.Language;
                        sourceCulture = sourceLanguage?.GetSpecificCulture();
                    }
                }
            }

            return sourceCulture;
        }

        /// <summary>
        /// Gets the binding mode for the multi-bind.
        /// </summary>
        /// <returns>Binding mode calculated from the nested bindings' modes.</returns>
        private BindingMode GetBindingMode()
        {
            var mode = BindingMode.Default;

            foreach( var component in RawComponentValues )
            {
                BindingMode componentMode;

                if( component is MultiBindBase customBinding )
                {
                    componentMode = customBinding.GetBindingMode();
                }
                else if( component is Bind binding )
                {
                    componentMode = binding.Mode;
                }
                else
                {
                    componentMode = BindingMode.Default;
                }

                if( ( componentMode == BindingMode.OneWayToSource ) && ( mode != BindingMode.OneWayToSource ) )
                {
                    throw new InvalidOperationException( "Bindings with OneWayToSource mode cannot be mixed with bindings with other modes" );
                }

                if( componentMode < mode )
                {
                    mode = componentMode;
                }
            }

            return mode;
        }

        /// <summary>
        /// Gets the update source trigger for the multi-bind.
        /// </summary>
        /// <returns>Update source trigger calculated from the nested bindings' triggers.</returns>
        private UpdateSourceTrigger GetUpdateSourceTrigger()
        {
            var trigger = UpdateSourceTrigger.Default;

            foreach( var component in RawComponentValues )
            {
                UpdateSourceTrigger componentTrigger;

                if( component is MultiBindBase customBinding )
                {
                    componentTrigger = customBinding.GetUpdateSourceTrigger();
                }
                else if( component is Bind binding )
                {
                    componentTrigger = binding.UpdateSourceTrigger;
                }
                else
                {
                    componentTrigger = UpdateSourceTrigger.Default;
                }

                if( componentTrigger > trigger )
                {
                    trigger = componentTrigger;
                }
            }

            return trigger;
        }

        private static object? GetDefaultValue( Type type )
        {
            if( type.IsValueType )
            {
                return Activator.CreateInstance( type );
            }
            else
            {
                return null;
            }
        }

        //===========================================================================
        //                          PROTECTED PROPERTIES
        //===========================================================================

        protected private Collection<object?> RawComponentValues { get; } = new();

        protected private Collection<Type> ComponentTypes { get; } = new();
    }
}
