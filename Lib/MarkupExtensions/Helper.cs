/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// Helper class.
    /// </summary>
    internal static class Helper
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Converts the value to the target type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="targetType">Type to convert the value to.</param>
        /// <param name="culture">Culture to use for conversion.</param>
        /// <returns>Converted value.</returns>
        public static object? ConvertValue( object? value, Type targetType, CultureInfo culture )
        {
            if( ( value != null ) &&
                ( targetType != typeof( object ) ) &&
                !targetType.IsAssignableFrom( value.GetType() ) )
            {
                return Convert.ChangeType( value, targetType, culture );
            }
            else
            {
                return value;
            }
        }
    }
}
