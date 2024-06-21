/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Utilities.DotNet.WPF.MarkupExtensions
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
        /// Constructor that initializes the parameters.
        /// </summary>
        /// <param name="parameterTypes">Types of the parameters.</param>
        protected BindingMarkupExtensionBase( IEnumerable<Type> parameterTypes )
        {
            foreach( var type in parameterTypes )
            {
                ComponentTypes.Add( type );
                RawComponentValues.Add( null );
            }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Gets the raw value of the specified parameter.
        /// </summary>
        /// <remarks>
        /// The parameter identifier must be in the range from 0 to the number of parameters (as specified in the constructor) minus one.
        /// </remarks>
        /// <param name="parameterId">Identifier of the parameter.</param>
        /// <returns>Raw value of the parameter.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the parameter identifier is out of range.</exception>
        protected object? GetParameterRawValue( int parameterId )
        {
            if( ( parameterId < 0 ) || ( parameterId >= RawComponentValues.Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( parameterId ) );
            }

            return RawComponentValues[ parameterId ];
        }

        /// <summary>
        /// Sets the raw value of the specified parameter.
        /// </summary>
        /// <remarks>
        /// The parameter identifier must be in the range from 0 to the number of parameters (as specified in the constructor) minus one.
        /// </remarks>
        /// <param name="parameterId">Identifier of the parameter.</param>
        /// <param name="parameterValue">Raw value of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the parameter identifier is out of range.</exception>
        protected void SetParameterRawValue( int parameterId, object? parameterValue )
        {
            if( ( parameterId < 0 ) || ( parameterId >= RawComponentValues.Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( parameterId ) );
            }

            RawComponentValues[ parameterId ] = parameterValue;
        }

        /// <inheritdoc/>
        protected sealed override (object? value, CultureInfo? culture) CalculateValue( object?[] componentValues, CultureInfo?[] componentCultures,
                                                                                        Type targetType, CultureInfo targetCulture )
        {
            // Target type is ignored because markup extensions have their own implicit return type and the conversion
            // to the target type will be performed later by the caller.
            return CalculateValue( componentValues, componentCultures, targetCulture );
        }

        /// <summary>
        /// Calculates the effective value of the markup extension from the effective values of its parameters.
        /// </summary>
        /// <remarks>
        /// Must be implemented by derived classes to perform the actual calculation.
        /// </remarks>
        /// <param name="parameterValues">Array with the effective values of the parameters.</param>
        /// <param name="parameterCultures">Array with the cultures of the parameters.</param>
        /// <param name="targetCulture">Culture of the target.</param>
        /// <returns>Effective value of the markup expression and its associated culture.</returns>
        protected abstract (object? value, CultureInfo? culture) CalculateValue( object?[] parameterValues, CultureInfo?[] parameterCultures, CultureInfo targetCulture );

        /// <inheritdoc/>
        protected override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, ComponentValue[] currentValues )
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
