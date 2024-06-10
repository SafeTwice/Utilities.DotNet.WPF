/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Base class for operations that combine multiple values.
    /// </summary>
    public abstract class OperationBase : MarkupExtension
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
                if( serviceProvider == null )
                {
                    return this;
                }

                var valueProvider = serviceProvider.GetService( typeof( IProvideValueTarget ) ) as IProvideValueTarget;

                var targetObject = valueProvider?.TargetObject;

                if( ( targetObject == null ) || ( targetObject is OperationBase ) )
                {
                    return this;
                }

                var targetProperty = valueProvider?.TargetProperty;

                if( ( targetObject is DependencyObject ) && ( targetProperty is DependencyProperty ) )
                {
                    return m_internalBinding.ProvideValue( serviceProvider );
                }
                else
                {
                    return this;
                }
            }
            else
            {
                return CalculateEffectiveValue( Array.Empty<object>().GetEnumerator() );
            }
        }

        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor that initializes the number of operands.
        /// </summary>
        /// <param name="numOperands">Number of operands of the operation.</param>
        protected OperationBase( int numOperands )
        {
            m_values = new object[ numOperands ];
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Gets the value of the specified operand.
        /// </summary>
        /// <remarks>
        /// The operand identifier must be in the range from 0 to the number of operands (as specified in the constructor) minus one.
        /// </remarks>
        /// <param name="operandId">Identifier of the operand.</param>
        /// <returns>Value of the operand.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the operand identifier is out of range.</exception>
        protected object? GetOperandValue( int operandId )
        {
            if( operandId < 0 || operandId >= m_values.Length )
            {
                throw new ArgumentOutOfRangeException( nameof( operandId ) );
            }

            return m_values[ operandId ];
        }

        /// <summary>
        /// Sets the value of the specified operand.
        /// </summary>
        /// <remarks>
        /// The operand identifier must be in the range from 0 to the number of operands (as specified in the constructor) minus one.
        /// </remarks>
        /// <param name="operandId">Identifier of the operand.</param>
        /// <param name="operandValue">Value of the operand.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the operand identifier is out of range.</exception>
        protected void SetOperandValue( int operandId, object? operandValue )
        {
            if( operandId < 0 || operandId >= m_values.Length )
            {
                throw new ArgumentOutOfRangeException( nameof( operandId ) );
            }

            m_values[ operandId ] = operandValue;
        }

        /// <summary>
        /// Calculates the value of the operation from the values of the operands.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="operandValues">Array with the values of the operands.</param>
        /// <returns>Value of the operation.</returns>
        protected abstract object? CalculateValue( object?[] operandValues );

        //===========================================================================
        //                          PRIVATE NESTED TYPES
        //===========================================================================

        private class Converter : IMultiValueConverter
        {
            public object? Convert( object[] bindingValues, Type targetType, object parameter, CultureInfo culture )
            {
                return m_bindingBase.CalculateEffectiveValue( bindingValues.GetEnumerator() );
            }

            public object[]? ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
            {
                return null;
            }

            public Converter( OperationBase bindingBase )
            {
                m_bindingBase = bindingBase;
            }

            OperationBase m_bindingBase;
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
            foreach( var value in m_values )
            {
                if( value is Binding binding )
                {
                    yield return binding;
                }
                else if( value is OperationBase customBinding )
                {
                    foreach( var nestedBinding in customBinding.GetBindings() )
                    {
                        yield return nestedBinding;
                    }
                }
            }
        }

        private object? CalculateEffectiveValue( IEnumerator bindingValues )
        {
            var effectiveOperandValues = new object?[ m_values.Length ];

            for( int i = 0; i < m_values.Length; i++ )
            {
                var operandValue = m_values[ i ];

                if( operandValue is OperationBase customBinding )
                {
                    effectiveOperandValues[ i ] = customBinding.CalculateEffectiveValue( bindingValues );
                }
                else if( operandValue is Binding binding )
                {
                    bindingValues.MoveNext();
                    effectiveOperandValues[ i ] = bindingValues.Current;
                }
                else
                {
                    effectiveOperandValues[ i ] = operandValue;
                }
            }

            return CalculateValue( effectiveOperandValues );
        }

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private MultiBinding InternalBinding
        {
            get
            {
                m_internalBinding ??= new MultiBinding()
                {
                    Mode = BindingMode.OneWay,
                    Converter = new Converter( this ),
                };
                return m_internalBinding;
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private MultiBinding? m_internalBinding;

        private object?[] m_values;
    }
}
