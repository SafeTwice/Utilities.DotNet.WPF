/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Utilities.DotNet.WPF.TypeConverters
{
    /// <summary>
    /// Provides a type converter to convert <see cref="Regex"/> objects from other representations.
    /// </summary>
    public class RegexTypeConverter : TypeConverter
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public override bool CanConvertFrom( ITypeDescriptorContext? context, Type sourceType )
        {
            return sourceType == typeof( string );
        }

        /// <inheritdoc/>
        public override object? ConvertFrom( ITypeDescriptorContext? context, CultureInfo? culture, object value )
        {
            return new Regex( (string) value );
        }

        /// <inheritdoc/>
        public override bool CanConvertTo( ITypeDescriptorContext? context, Type? destinationType )
        {
            return false;
        }
    }
}
