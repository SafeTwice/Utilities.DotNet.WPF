/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using System.Windows.Data;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Base class for markup extensions which parameters can be static (constant) XAML values,
    /// other <see cref="BindingMarkupExtensionBase">binding markup extensions</see>, or
    /// <see cref="Bind"/> / <see cref="MultiBind"/> bindings.
    /// </summary>
    public abstract class BindingMarkupExtensionBase : MultiBindBase
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor that initializes the number of parameters.
        /// </summary>
        /// <param name="numParameters">Number of parameters of the markup extension.</param>
        protected BindingMarkupExtensionBase( int numParameters )
        {
            for( int i = 0; i < numParameters; i++ )
            {
                Components.Add( null );
            }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Gets the value of the specified parameter.
        /// </summary>
        /// <remarks>
        /// The parameter identifier must be in the range from 0 to the number of parameters (as specified in the constructor) minus one.
        /// </remarks>
        /// <param name="parameterId">Identifier of the parameter.</param>
        /// <returns>Value of the parameter.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the parameter identifier is out of range.</exception>
        protected object? GetParameterValue( int parameterId )
        {
            if( ( parameterId < 0 ) || ( parameterId >= Components.Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( parameterId ) );
            }

            return Components[ parameterId ];
        }

        /// <summary>
        /// Sets the value of the specified parameter.
        /// </summary>
        /// <remarks>
        /// The parameter identifier must be in the range from 0 to the number of parameters (as specified in the constructor) minus one.
        /// </remarks>
        /// <param name="parameterId">Identifier of the parameter.</param>
        /// <param name="parameterValue">Value of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the parameter identifier is out of range.</exception>
        protected void SetParameterValue( int parameterId, object? parameterValue )
        {
            if( ( parameterId < 0 ) || ( parameterId >= Components.Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( parameterId ) );
            }

            Components[ parameterId ] = parameterValue;
        }

        /// <inheritdoc/>
        protected sealed override (object? value, CultureInfo culture) CalculateValue( object?[] componentValues, CultureInfo[] componentCultures,
                                                                                       Type targetType, CultureInfo targetCulture )
        {
            // Target type is ignored because operations have their own implicit return type.
            return CalculateValue( componentValues, componentCultures, targetCulture );
        }

        /// <summary>
        /// Calculates the value of the markup extension from the values of its parameters.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="parameterValues">Array with the values of the parameters.</param>
        /// <param name="parameterCultures">Array with the cultures of the parameters.</param>
        /// <param name="targetCulture">Culture of the target.</param>
        /// <returns>Value of the markup expression and its associated culture.</returns>
        protected abstract (object? value, CultureInfo culture) CalculateValue( object?[] parameterValues, CultureInfo[] parameterCultures, CultureInfo targetCulture );

        /// <inheritdoc/>
        protected override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, CultureInfo[] sourceCultures )
        {
            return null;
        }

        /// <inheritdoc/>
        protected sealed override MultiBinding CreateBinding()
        {
            return base.CreateBinding();
        }
    }
}
