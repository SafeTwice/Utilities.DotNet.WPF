﻿/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Base class for multi-bindings and operations that combine multiple values.
    /// </summary>
    public abstract class MultiBindBase : MarkupExtension
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public override object? ProvideValue( IServiceProvider serviceProvider )
        {
            PrepareBinding();

            if( ( m_internalBinding != null ) && ( m_internalBinding.Bindings.Count > 0 ) )
            {
                return ProvideDynamicValue( serviceProvider, m_internalBinding );
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
        /// <returns>Value of the binding.</returns>
        protected abstract object? CalculateValue( object?[] componentValues, Type targetType, CultureInfo culture );

        //===========================================================================
        //                          PRIVATE NESTED TYPES
        //===========================================================================

        private class Converter : IMultiValueConverter
        {
            public object? Convert( object[] bindingValues, Type targetType, object? parameter, CultureInfo culture )
            {
                var bindingBase = (MultiBindBase) parameter!;

                var calculatedValue = bindingBase.CalculateEffectiveValue( bindingValues.GetEnumerator(), targetType, culture );
                return ConvertValue( calculatedValue, targetType );
            }

            public object[]? ConvertBack( object value, Type[] targetTypes, object? parameter, CultureInfo culture )
            {
                return null;
            }
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void PrepareBinding()
        {
            var bindings = GetBindings();

            foreach( var binding in bindings )
            {
                InternalBinding.Bindings.Add( binding );
            }
        }

        private IEnumerable<BindingBase> GetBindings()
        {
            foreach( var value in Components )
            {
                if( value is Bind binding )
                {
                    yield return binding.InternalBinding;
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

        private object? CalculateEffectiveValue( IEnumerator bindingValues, Type targetType, CultureInfo culture )
        {
            var effectiveOperandValues = new object?[ Components.Count ];

            for( int i = 0; i < Components.Count; i++ )
            {
                var operandValue = Components[ i ];

                if( operandValue is MultiBindBase customBinding )
                {
                    effectiveOperandValues[ i ] = customBinding.CalculateEffectiveValue( bindingValues, targetType, culture );
                }
                else if( operandValue is Bind binding )
                {
                    bindingValues.MoveNext();
                    effectiveOperandValues[ i ] = bindingValues.Current;
                }
                else
                {
                    effectiveOperandValues[ i ] = operandValue;
                }
            }

            return CalculateValue( effectiveOperandValues, targetType, culture );
        }

        private object ProvideDynamicValue( IServiceProvider serviceProvider, BindingBase binding )
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
                return binding.ProvideValue( serviceProvider );
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

            var calculatedValue = CalculateEffectiveValue( Array.Empty<object>().GetEnumerator(), targetType, CultureInfo.InvariantCulture );

            if( ( calculatedValue != null ) && ( targetType != typeof( object ) ) )
            {
                calculatedValue = ConvertValue( calculatedValue, targetType );
            }

            return calculatedValue;
        }

        private static object? ConvertValue( object? value, Type targetType )
        {
            if( ( value != null ) && !targetType.IsAssignableFrom( value.GetType() ) )
            {
                try
                {
                    return Convert.ChangeType( value, targetType );
                }
                catch
                {
                }
            }

            return value;
        }

        //===========================================================================
        //                          PROTECTED PROPERTIES
        //===========================================================================

        protected private Collection<object?> Components { get; } = new();

        protected private MultiBinding InternalBinding
        {
            get
            {
                m_internalBinding ??= new MultiBinding()
                {
                    Mode = BindingMode.OneWay,
                    Converter = new Converter(),
                    ConverterParameter = this,
                };
                return m_internalBinding;
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private MultiBinding? m_internalBinding;
    }
}