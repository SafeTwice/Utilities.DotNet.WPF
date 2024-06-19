/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
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
        public override object? ProvideValue( IServiceProvider serviceProvider )
        {
            var binding = PrepareBinding();

            if( binding != null )
            {
                return ProvideDynamicValue( serviceProvider, binding );
            }
            else
            {
                return ProvideStaticValue( serviceProvider );
            }
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
        /// Calculates the value of the binding from the effective (calculated) values of its components.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="componentValues">Array with the effective values of the components.</param>
        /// <param name="componentCultures">Array with the cultures of the components.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetCulture">Culture of the target.</param>
        /// <returns>Value of the binding and its associated culture.</returns>
        protected abstract (object? value, CultureInfo culture) CalculateValue( object?[] componentValues, CultureInfo[] componentCultures,
                                                                                Type targetType, CultureInfo targetCulture );

        /// <summary>
        /// Calculates the values of the components from the value of the binding target.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="targetValue">Value of the binding target.</param>
        /// <param name="targetCulture">Culture of the binding target.</param>
        /// <param name="sourceTypes">Types of the component sources.</param>
        /// <param name="sourceCultures">Cultures of the component sources.</param>
        /// <returns>An array with the values of the components, or <c>null</c> if the calculation cannot be performed or is not feasible.</returns>
        protected abstract object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, CultureInfo[] sourceCultures );

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
        //                          PRIVATE NESTED TYPES
        //===========================================================================

        private class Converter : IMultiValueConverter
        {
            public object? Convert( object?[] bindingValues, Type targetType, object? parameter, CultureInfo culture )
            {
                var bindingExpressions = BindingExpression!.BindingExpressions;
                Debug.Assert( bindingValues.Length == bindingExpressions.Count );

                var calculatedValue = Binding!.CalculateEffectiveValue( bindingValues.GetEnumerator(), bindingExpressions.GetEnumerator(),
                                                                        targetType, culture );

                return ConvertValue( calculatedValue.value, targetType, culture );
            }

            public object?[]? ConvertBack( object? value, Type[] targetTypes, object? parameter, CultureInfo culture )
            {
                var bindingExpressions = BindingExpression!.BindingExpressions;

                var targetTypesEnumerator = ( (IEnumerable<Type>) targetTypes ).GetEnumerator();

                var bindingValues = new List<object?>();

                if( Binding!.CalculateBackBindingValues( value, culture, bindingExpressions.GetEnumerator(), targetTypesEnumerator, bindingValues ) )
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

            public MultiBindBase? Binding { get; set; }
            public MultiBindingExpression? BindingExpression { get; set; }
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private MultiBinding? PrepareBinding()
        {
            MultiBinding? multiBinding = null;

            var bindings = GetBindings();

            if( bindings.Count() > 0 )
            {
                var converter = new Converter();

                multiBinding = CreateBinding();

                multiBinding.Converter = converter;

                converter.Binding = this;
            }

            foreach( var binding in bindings )
            {
                multiBinding!.Bindings.Add( binding.InternalBinding );
            }

            return multiBinding;
        }

        private IEnumerable<Bind> GetBindings()
        {
            foreach( var value in Components )
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

        private object ProvideDynamicValue( IServiceProvider serviceProvider, MultiBinding binding )
        {
            if( serviceProvider == null )
            {
                return this;
            }

            var valueProvider = serviceProvider.GetService( typeof( IProvideValueTarget ) ) as IProvideValueTarget;

            var targetObject = valueProvider?.TargetObject;

            if( ( targetObject == null ) || ( targetObject is MultiBindBase ) )
            {
                return this;
            }

            var targetProperty = valueProvider?.TargetProperty;

            if( ( targetObject is DependencyObject ) && ( targetProperty is DependencyProperty ) )
            {
                var providedValue = binding.ProvideValue( serviceProvider );

                // Store the binding expression to obtain the nested bindings expressions once the binding is
                // attached and the nested binding expressions are available.
                ( (Converter) binding.Converter ).BindingExpression = ( providedValue as MultiBindingExpression );

                return providedValue;
            }
            else
            {
                return this;
            }
        }

        private object? ProvideStaticValue( IServiceProvider serviceProvider )
        {
            var valueProvider = serviceProvider.GetService( typeof( IProvideValueTarget ) ) as IProvideValueTarget;

            var targetObject = valueProvider?.TargetObject;
            var targetProperty = valueProvider?.TargetProperty;

            Type targetType;

            if( targetProperty is DependencyProperty targetDependencyProperty )
            {
                targetType = targetDependencyProperty.PropertyType;
            }
            else if( !( targetObject is MultiBindBase ) && ( targetProperty is PropertyInfo targetPropertyInfo ) )
            {
                targetType = targetPropertyInfo.PropertyType;
            }
            else
            {
                targetType = typeof( object );
            }

            var targetCulture = GetTargetCulture( targetObject );

            var emptyBindingExpressions = new List<BindingExpression>();
            var calculatedValue = CalculateEffectiveValue( Array.Empty<object>().GetEnumerator(), emptyBindingExpressions.GetEnumerator(), targetType, targetCulture );

            return ConvertValue( calculatedValue.value, targetType, targetCulture );
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

        /// <summary>
        /// Calculates the effective value of the binding from the effective values of its components.
        /// </summary>
        /// <remarks>
        /// <para>The effective value of static (constant) components is intrinsically the component value.</para>
        /// <para>The effective value of nested MultiBindBase-derived components is calculated recursively.</para>
        /// <para>The effective value of binding components is obtained from the binding source, which is passed in <paramref name="bindingValues"/>.</para>
        /// </remarks>
        /// <param name="bindingValues">Enumerator for binding values.</param>
        /// <param name="bindingExpressions">Enumerator for binding expressions.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetCulture">Culture of the target.</param>
        /// <returns>Effective value of the binding and its associated culture.</returns>
        private (object? value, CultureInfo culture) CalculateEffectiveValue( IEnumerator bindingValues, IEnumerator<BindingExpressionBase> bindingExpressions,
                                                                              Type targetType, CultureInfo targetCulture )
        {
            var effectiveComponentValues = new object?[ Components.Count ];
            var componentCultures = new CultureInfo[ Components.Count ];

            for( int i = 0; i < Components.Count; i++ )
            {
                var component = Components[ i ];
                (object? value, CultureInfo culture) effectiveComponentValue;

                if( component is MultiBindBase customBinding )
                {
                    effectiveComponentValue = customBinding.CalculateEffectiveValue( bindingValues, bindingExpressions, targetType, targetCulture );
                }
                else if( component is Bind binding )
                {
                    bindingValues.MoveNext();
                    effectiveComponentValue.value = bindingValues.Current;

                    bindingExpressions.MoveNext();
                    var bindingExpression = bindingExpressions.Current as BindingExpression;
                    effectiveComponentValue.culture = binding.ConverterCulture ?? GetBindingSourceCulture( bindingExpression );
                }
                else
                {
                    effectiveComponentValue.value = component;
                    effectiveComponentValue.culture = CultureInfo.InvariantCulture;
                }

                if( ( effectiveComponentValue.value == DependencyProperty.UnsetValue ) ||
                    ( effectiveComponentValue.value == Binding.DoNothing ) )
                {
                    return effectiveComponentValue;
                }

                effectiveComponentValues[ i ] = effectiveComponentValue.value;
                componentCultures[ i ] = effectiveComponentValue.culture;
            }

            return CalculateValue( effectiveComponentValues, componentCultures, targetType, targetCulture );
        }

        /// <summary>
        /// Calculates the values of the binding component from the value of the binding target.
        /// </summary>
        /// <param name="targetValue">Value of the binding target.</param>
        /// <param name="targetCulture">Culture of the binding target.</param>
        /// <param name="bindingExpressions">Enumerator for binding expressions.</param>
        /// <param name="bindingSourceTypes">Enumerator for the binding source types.</param>
        /// <param name="bindingValues">[Output] Collection to which binding values are added.</param>
        /// <returns><c>true</c> if the calculation was successful; otherwise, <c>false</c>.</returns>
        private bool CalculateBackBindingValues( object? targetValue, CultureInfo targetCulture, IEnumerator<BindingExpressionBase> bindingExpressions,
                                                 IEnumerator<Type> bindingSourceTypes, ICollection<object?> bindingValues )
        {
            var sourceCultures = new CultureInfo[ Components.Count ];
            var sourceTypes = new Type[ Components.Count ];

            for( int i = 0; i < Components.Count; i++ )
            {
                var component = Components[ i ];
                CultureInfo sourceCulture;
                Type sourceType;

                if( component is Bind binding )
                {
                    bindingExpressions.MoveNext();
                    var bindingExpression = bindingExpressions.Current as BindingExpression;
                    sourceCulture = binding.ConverterCulture ?? GetBindingSourceCulture( bindingExpression );

                    bindingSourceTypes.MoveNext();
                    sourceType = bindingSourceTypes.Current;
                }
                else
                {
                    sourceCulture = CultureInfo.InvariantCulture;
                    sourceType = typeof( object );
                }

                sourceCultures[ i ] = sourceCulture;
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
                componentBackValues = CalculateBackValues( targetValue, targetCulture, sourceTypes, sourceCultures );

                if( componentBackValues == null )
                {
                    return false;
                }

                Debug.Assert( componentBackValues.Length == Components.Count );
            }

            for( int i = 0; i < Components.Count; i++ )
            {
                var component = Components[ i ];
                var componentBackValue = componentBackValues?[ i ] ?? targetValue;

                if( component is MultiBindBase customBinding )
                {
                    if( !customBinding.CalculateBackBindingValues( componentBackValue, targetCulture, bindingExpressions, bindingSourceTypes, bindingValues ) )
                    {
                        return false;
                    }
                }
                else if( component is Bind binding )
                {
                    object? finalBackValue;

                    if( ( componentBackValue == DependencyProperty.UnsetValue ) ||
                        ( componentBackValue == Binding.DoNothing ) )
                    {
                        finalBackValue = componentBackValue;
                    }
                    else
                    {
                        finalBackValue = ConvertValue( componentBackValue, sourceTypes[ i ]!, sourceCultures[ i ] );
                    }
                    bindingValues.Add( finalBackValue );
                }
                else
                {
                    if( ( componentBackValue != DependencyProperty.UnsetValue ) &&
                        ( componentBackValue != Binding.DoNothing ) &&
                        !Object.Equals( componentBackValue, component ) )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static CultureInfo GetBindingSourceCulture( BindingExpression? bindingExpression )
        {
            CultureInfo? sourceCulture = null;

            var source = bindingExpression?.ResolvedSource;
            if( source is FrameworkElement frameworkElement )
            {
                var sourceLanguage = frameworkElement.Language;
                sourceCulture = sourceLanguage?.GetSpecificCulture();
            }

            return sourceCulture ?? CultureInfo.InvariantCulture;
        }

        private static object? ConvertValue( object? value, Type targetType, CultureInfo culture )
        {
            if( ( value != null ) && ( targetType != typeof( object ) ) && !targetType.IsAssignableFrom( value.GetType() ) )
            {
                try
                {
                    return Convert.ChangeType( value, targetType, culture );
                }
                catch
                {
                }
            }

            return value;
        }

        protected virtual BindingMode GetBindingMode()
        {
            var mode = BindingMode.Default;

            foreach( var component in Components )
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

                if( componentMode == BindingMode.OneWayToSource )
                {
                    if( ( mode == BindingMode.OneWay ) || ( mode == BindingMode.OneTime ) )
                    {
                        mode = BindingMode.TwoWay;
                    }
                }
                else if( ( componentMode == BindingMode.OneWay ) || ( mode == BindingMode.OneTime ) )
                {
                    if( mode == BindingMode.OneWayToSource )
                    {
                        mode = BindingMode.TwoWay;
                    }
                }

                if( componentMode < mode )
                {
                    mode = componentMode;
                }
            }

            return mode;
        }

        protected virtual UpdateSourceTrigger GetUpdateSourceTrigger()
        {
            var trigger = UpdateSourceTrigger.Default;

            foreach( var component in Components )
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

        //===========================================================================
        //                          PROTECTED PROPERTIES
        //===========================================================================

        protected private Collection<object?> Components { get; } = new();
    }
}
