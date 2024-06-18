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
        /// Creates a binding.
        /// </summary>
        /// <returns>Multi-binding.</returns>
        protected virtual MultiBinding CreateBinding()
        {
            return new MultiBinding
            {
                Mode = BindingMode.OneWay,
            };
        }

        //===========================================================================
        //                          PRIVATE NESTED TYPES
        //===========================================================================

        private class Converter : IMultiValueConverter
        {
            public object? Convert( object[] bindingValues, Type targetType, object? parameter, CultureInfo culture )
            {
                var bindingExpressions = BindingExpression!.BindingExpressions;
                Debug.Assert( bindingValues.Length == bindingExpressions.Count );

                var calculatedValue = Binding!.CalculateEffectiveValue( bindingValues.GetEnumerator(), bindingExpressions.GetEnumerator(),
                                                                        targetType, culture );
                return ConvertTargetValue( calculatedValue.value, targetType, culture );
            }

            public object[]? ConvertBack( object value, Type[] targetTypes, object? parameter, CultureInfo culture )
            {
                return null;
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

            return ConvertTargetValue( calculatedValue.value, targetType, targetCulture );
        }

        private static object? ConvertTargetValue( object? value, Type targetType, CultureInfo culture )
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

        private (object? value, CultureInfo culture) CalculateEffectiveValue( IEnumerator bindingValues, IEnumerator<BindingExpressionBase> bindingExpressions,
                                                                              Type targetType, CultureInfo targetCulture )
        {
            var effectiveComponentValues = new object?[ Components.Count ];
            var componentCultures = new CultureInfo[ Components.Count ];

            for( int i = 0; i < Components.Count; i++ )
            {
                var componentValue = Components[ i ];
                (object? value, CultureInfo culture) effectiveComponentValue;

                if( componentValue is MultiBindBase customBinding )
                {
                    effectiveComponentValue = customBinding.CalculateEffectiveValue( bindingValues, bindingExpressions, targetType, targetCulture );
                }
                else if( componentValue is Bind binding )
                {
                    bindingValues.MoveNext();
                    effectiveComponentValue.value = bindingValues.Current;

                    bindingExpressions.MoveNext();
                    var bindingExpression = bindingExpressions.Current as BindingExpression;
                    effectiveComponentValue.culture = binding.ConverterCulture ?? GetBindingSourceCulture( bindingExpression );
                }
                else
                {
                    effectiveComponentValue.value = componentValue;
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

        //===========================================================================
        //                          PROTECTED PROPERTIES
        //===========================================================================

        protected private Collection<object?> Components { get; } = new();
    }
}
