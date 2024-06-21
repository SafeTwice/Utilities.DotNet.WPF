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

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// <see cref="Binding"/> that can be passed as a parameter to a <see cref="MarkupExtension"/>.
    /// </summary>
    public sealed class Bind : MarkupExtension
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc cref="Binding.AsyncState"/>
        [DefaultValue( null )]
        public object? AsyncState
        {
            get { return InternalBinding.AsyncState; }
            set { InternalBinding.AsyncState = value; }
        }

        /// <inheritdoc cref="BindingBase.BindingGroupName"/>
        [DefaultValue( "" )]
        public string BindingGroupName
        {
            get { return InternalBinding.BindingGroupName; }
            set { InternalBinding.BindingGroupName = value; }
        }

        /// <inheritdoc cref="Binding.BindsDirectlyToSource"/>
        [DefaultValue( false )]
        public bool BindsDirectlyToSource
        {
            get { return InternalBinding.BindsDirectlyToSource; }
            set { InternalBinding.BindsDirectlyToSource = value; }
        }

        /// <inheritdoc cref="Binding.Converter"/>
        [DefaultValue( null )]
        public IValueConverter? Converter
        {
            get { return InternalBinding.Converter; }
            set { InternalBinding.Converter = value; }
        }

        /// <inheritdoc cref="Binding.ConverterCulture"/>
        [DefaultValue( null )]
        [TypeConverter( typeof( CultureInfoIetfLanguageTagConverter ) )]
        public CultureInfo? ConverterCulture
        {
            get { return InternalBinding.ConverterCulture; }
            set { InternalBinding.ConverterCulture = value; }
        }

        /// <inheritdoc cref="Binding.ConverterParameter"/>
        [DefaultValue( null )]
        public object? ConverterParameter
        {
            get { return InternalBinding.ConverterParameter; }
            set { InternalBinding.ConverterParameter = value; }
        }

        /// <inheritdoc cref="BindingBase.Delay"/>
        [DefaultValue( 0 )]
        public int Delay
        {
            get { return InternalBinding.Delay; }
            set { InternalBinding.Delay = value; }
        }

        /// <inheritdoc cref="Binding.ElementName"/>
        [DefaultValue( null )]
        public string? ElementName
        {
            get { return InternalBinding.ElementName; }
            set { InternalBinding.ElementName = value; }
        }

        /// <inheritdoc cref="BindingBase.FallbackValue"/>
        [DefaultValue( null )]
        public object? FallbackValue
        {
            get { return InternalBinding.FallbackValue; }
            set { InternalBinding.FallbackValue = value; }
        }

        /// <inheritdoc cref="Binding.IsAsync"/>
        [DefaultValue( false )]
        public bool IsAsync
        {
            get { return InternalBinding.IsAsync; }
            set { InternalBinding.IsAsync = value; }
        }

        /// <inheritdoc cref="Binding.Mode"/>
        [DefaultValue( BindingMode.Default )]
        public BindingMode Mode
        {
            get { return InternalBinding.Mode; }
            set { InternalBinding.Mode = value; }
        }

        /// <inheritdoc cref="Binding.NotifyOnSourceUpdated"/>
        [DefaultValue( false )]
        public bool NotifyOnSourceUpdated
        {
            get { return InternalBinding.NotifyOnSourceUpdated; }
            set { InternalBinding.NotifyOnSourceUpdated = value; }
        }

        /// <inheritdoc cref="Binding.NotifyOnTargetUpdated"/>
        [DefaultValue( false )]
        public bool NotifyOnTargetUpdated
        {
            get { return InternalBinding.NotifyOnTargetUpdated; }
            set { InternalBinding.NotifyOnTargetUpdated = value; }
        }

        /// <inheritdoc cref="Binding.NotifyOnValidationError"/>
        [DefaultValue( false )]
        public bool NotifyOnValidationError
        {
            get { return InternalBinding.NotifyOnValidationError; }
            set { InternalBinding.NotifyOnValidationError = value; }
        }

        /// <inheritdoc cref="Binding.Path"/>
        [DefaultValue( null )]
        public PropertyPath? Path
        {
            get { return InternalBinding.Path; }
            set { InternalBinding.Path = value; }
        }

        /// <inheritdoc cref="Binding.RelativeSource"/>
        [DefaultValue( null )]
        public RelativeSource? RelativeSource
        {
            get { return InternalBinding.RelativeSource; }
            set { InternalBinding.RelativeSource = value; }
        }

        /// <inheritdoc cref="Binding.Source"/>
        [DefaultValue( null )]
        public object? Source
        {
            get { return InternalBinding.Source; }
            set { InternalBinding.Source = value; }
        }

        /// <inheritdoc cref="BindingBase.StringFormat"/>
        [DefaultValue( null )]
        public string? StringFormat
        {
            get { return InternalBinding.StringFormat; }
            set { InternalBinding.StringFormat = value; }
        }

        /// <inheritdoc cref="BindingBase.TargetNullValue"/>
        [DefaultValue( null )]
        public object? TargetNullValue
        {
            get { return InternalBinding.TargetNullValue; }
            set { InternalBinding.TargetNullValue = value; }
        }

        /// <inheritdoc cref="Binding.UpdateSourceExceptionFilter"/>
        [DefaultValue( null )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public UpdateSourceExceptionFilterCallback? UpdateSourceExceptionFilter
        {
            get { return InternalBinding.UpdateSourceExceptionFilter; }
            set { InternalBinding.UpdateSourceExceptionFilter = value; }
        }

        /// <inheritdoc cref="Binding.UpdateSourceTrigger"/>
        [DefaultValue( UpdateSourceTrigger.Default )]
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get { return InternalBinding.UpdateSourceTrigger; }
            set { InternalBinding.UpdateSourceTrigger = value; }
        }

        /// <inheritdoc cref="Binding.ValidatesOnDataErrors"/>
        [DefaultValue( false )]
        public bool ValidatesOnDataErrors
        {
            get { return InternalBinding.ValidatesOnDataErrors; }
            set { InternalBinding.ValidatesOnDataErrors = value; }
        }

        /// <inheritdoc cref="Binding.ValidatesOnExceptions"/>
        [DefaultValue( false )]
        public bool ValidatesOnExceptions
        {
            get { return InternalBinding.ValidatesOnExceptions; }
            set { InternalBinding.ValidatesOnExceptions = value; }
        }

        /// <inheritdoc cref="Binding.ValidatesOnNotifyDataErrors"/>
        [DefaultValue( false )]
        public bool ValidatesOnNotifyDataErrors
        {
            get { return InternalBinding.ValidatesOnNotifyDataErrors; }
            set { InternalBinding.ValidatesOnNotifyDataErrors = value; }
        }

        /// <inheritdoc cref="Binding.ValidationRules"/>
        public Collection<ValidationRule> ValidationRules
        {
            get { return InternalBinding.ValidationRules; }
        }

        /// <inheritdoc cref="Binding.XPath"/>
        [DefaultValue( null )]
        public string? XPath
        {
            get { return InternalBinding.XPath; }
            set { InternalBinding.XPath = value; }
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <inheritdoc cref="Binding.Binding()"/>
        public Bind()
        {
            InternalBinding = new Binding();
        }

        /// <inheritdoc cref="Binding.Binding(string)"/>
        public Bind( string path )
        {
            InternalBinding = new Binding( path );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            if( serviceProvider == null )
            {
                return this;
            }

            var provideValueTarget = serviceProvider.GetService( typeof( IProvideValueTarget ) ) as IProvideValueTarget;

            if( provideValueTarget != null )
            {
                if( ( provideValueTarget.TargetObject is DependencyObject ) &&
                    ( provideValueTarget.TargetProperty is DependencyProperty ) )
                {
                    return InternalBinding.ProvideValue( serviceProvider );
                }
            }

            return this;
        }

        //===========================================================================
        //                          INTERNAL PROPERTIES
        //===========================================================================

        internal Binding InternalBinding { get; }
    }
}
