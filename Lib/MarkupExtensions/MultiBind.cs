/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Utilities.WPF.Net.MarkupExtensions
{
    /// <summary>
    /// <see cref="MultiBinding"/> that can be passed as a parameter to a <see cref="MarkupExtension"/> and
    /// that accepts as components both <see cref="Bind"/> and <see cref="MultiBind"/> bindings, values provided
    /// by <see cref="BindingMarkupExtensionBase">binding markup extensions</see>, and static (constant) XAML values.
    /// </summary>
    [ContentProperty( nameof( Components ) )]
    public sealed class MultiBind : MultiBindBase, IAddChild
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Collection of components that will be combined by the multi-binding.
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public Collection<object?> Components => base.RawComponentValues;

        /// <inheritdoc cref="BindingBase.BindingGroupName "/>
        [DefaultValue( "" )]
        public string BindingGroupName { get; set; } = string.Empty;

        /// <inheritdoc cref="MultiBinding.Converter"/>
        [DefaultValue( null )]
        public IMultiValueConverter? Converter { get; set; }

        /// <inheritdoc cref="MultiBinding.ConverterCulture"/>
        [DefaultValue( null )]
        [TypeConverter( typeof( CultureInfoIetfLanguageTagConverter ) )]
        public CultureInfo? ConverterCulture { get; set; }

        /// <inheritdoc cref="Binding.ConverterParameter"/>
        [DefaultValue( null )]
        public object? ConverterParameter { get; set; }

        /// <inheritdoc cref="BindingBase.Delay"/>
        [DefaultValue( 0 )]
        public int Delay { get; set; }

        /// <inheritdoc cref="BindingBase.FallbackValue"/>
        [DefaultValue( null )]
        public object? FallbackValue { get; set; }

        /// <inheritdoc cref="Binding.Mode"/>
        [DefaultValue( BindingMode.Default )]
        public BindingMode Mode { get; set; }

        /// <inheritdoc cref="Binding.NotifyOnSourceUpdated"/>
        [DefaultValue( false )]
        public bool NotifyOnSourceUpdated { get; set; }

        /// <inheritdoc cref="Binding.NotifyOnTargetUpdated"/>
        [DefaultValue( false )]
        public bool NotifyOnTargetUpdated { get; set; }

        /// <inheritdoc cref="Binding.NotifyOnValidationError"/>
        [DefaultValue( false )]
        public bool NotifyOnValidationError { get; set; }

        /// <inheritdoc cref="BindingBase.StringFormat"/>
        [DefaultValue( null )]
        public string? StringFormat { get; set; }

        /// <inheritdoc cref="BindingBase.TargetNullValue"/>
        [DefaultValue( null )]
        public object? TargetNullValue { get; set; }

        /// <inheritdoc cref="Binding.UpdateSourceExceptionFilter"/>
        [DefaultValue( null )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public UpdateSourceExceptionFilterCallback? UpdateSourceExceptionFilter { get; set; }

        /// <inheritdoc cref="Binding.UpdateSourceTrigger"/>
        [DefaultValue( UpdateSourceTrigger.Default )]
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

        /// <inheritdoc cref="Binding.ValidatesOnDataErrors"/>
        [DefaultValue( false )]
        public bool ValidatesOnDataErrors { get; set; }

        /// <inheritdoc cref="Binding.ValidatesOnExceptions"/>
        [DefaultValue( false )]
        public bool ValidatesOnExceptions { get; set; }

        /// <inheritdoc cref="Binding.ValidatesOnNotifyDataErrors"/>
        [DefaultValue( false )]
        public bool ValidatesOnNotifyDataErrors { get; set; }

        /// <inheritdoc cref="Binding.ValidationRules"/>
        public Collection<ValidationRule> ValidationRules { get; } = new();

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor for a MultiBind which components are defined as XAML content.
        /// </summary>
        public MultiBind()
        {
        }

        /// <summary>
        /// Constructor for a MultiBind with a single component.
        /// </summary>
        /// <param name="p1">Single component.</param>
        public MultiBind( object? p1 ) : this()
        {
            Components.Add( p1 );
        }

        /// <summary>
        /// Constructor for a MultiBind with two components.
        /// </summary>
        /// <param name="p1">First component.</param>
        /// <param name="p2">Second component.</param>
        public MultiBind( object? p1, object? p2 ) : this( p1 )
        {
            Components.Add( p2 );
        }

        /// <summary>
        /// Constructor for a MultiBind with three components.
        /// </summary>
        /// <param name="p1">First component.</param>
        /// <param name="p2">Second component.</param>
        /// <param name="p3">Third component.</param>
        public MultiBind( object? p1, object? p2, object? p3 ) : this( p1, p2 )
        {
            Components.Add( p3 );
        }

        /// <summary>
        /// Constructor for a MultiBind with four components.
        /// </summary>
        /// <param name="p1">First component.</param>
        /// <param name="p2">Second component.</param>
        /// <param name="p3">Third component.</param>
        /// <param name="p4">Fourth component.</param>
        public MultiBind( object? p1, object? p2, object? p3, object? p4 ) : this( p1, p2, p3 )
        {
            Components.Add( p4 );
        }

        /// <summary>
        /// Constructor for a MultiBind with five components.
        /// </summary>
        /// <param name="p1">First component.</param>
        /// <param name="p2">Second component.</param>
        /// <param name="p3">Third component.</param>
        /// <param name="p4">Fourth component.</param>
        /// <param name="p5">Fifth component.</param>
        public MultiBind( object? p1, object? p2, object? p3, object? p4, object? p5 ) : this( p1, p2, p3, p4 )
        {
            Components.Add( p5 );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        void IAddChild.AddChild( object value )
        {
            base.RawComponentValues.Add( value );
        }

        /// <inheritdoc/>
        void IAddChild.AddText( string text )
        {
            var trimmedText = text.Trim();
            if( trimmedText.Length > 0 )
            {
                base.RawComponentValues.Add( trimmedText );
            }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <inheritdoc/>
        protected override (object? value, CultureInfo? culture) CalculateValue( object?[] componentValues, CultureInfo?[] componentCultures,
                                                                                 Type targetType, CultureInfo targetCulture )
        {
            object? value = null;
            var culture = targetCulture;

            if( Converter != null )
            {
                value = Converter?.Convert( componentValues, targetType, ConverterParameter, targetCulture );

                if( StringFormat != null )
                {
                    value = String.Format( targetCulture, StringFormat, value );
                }
            }
            else
            {
                if( StringFormat != null )
                {
                    value = String.Format( targetCulture, StringFormat, componentValues );
                }
                else
                {
                    value = Binding.DoNothing;
                    culture = null;
                }
            }

            return (value, culture);
        }

        /// <inheritdoc/>
        protected override object?[]? CalculateBackValues( object? targetValue, CultureInfo targetCulture, Type[] sourceTypes, ComponentValue[] currentValues )
        {
            if( Converter != null )
            {
                return Converter.ConvertBack( targetValue, sourceTypes, ConverterParameter, targetCulture );
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        protected override MultiBinding CreateBinding()
        {
            var binding = new MultiBinding
            {
                BindingGroupName = BindingGroupName,
                ConverterCulture = ConverterCulture,
                Delay = Delay,
                FallbackValue = FallbackValue,
                Mode = Mode,
                NotifyOnSourceUpdated = NotifyOnSourceUpdated,
                NotifyOnTargetUpdated = NotifyOnTargetUpdated,
                NotifyOnValidationError = NotifyOnValidationError,
                TargetNullValue = TargetNullValue,
                UpdateSourceExceptionFilter = UpdateSourceExceptionFilter,
                UpdateSourceTrigger = UpdateSourceTrigger,
                ValidatesOnDataErrors = ValidatesOnDataErrors,
                ValidatesOnExceptions = ValidatesOnExceptions,
                ValidatesOnNotifyDataErrors = ValidatesOnNotifyDataErrors,
            };

            foreach( var validationRule in ValidationRules )
            {
                binding.ValidationRules.Add( validationRule );
            }

            return binding;
        }
    }
}
