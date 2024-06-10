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
    /// <see cref="Binding"> that can be passed as a parameter to a <see cref="MarkupExtension"/>.
    /// </summary>
    public class Bind : MarkupExtension
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc cref="Binding.AsyncState"/>
        [DefaultValue( null )]
        public object AsyncState
        {
            get { return m_binding.AsyncState; }
            set { m_binding.AsyncState = value; }
        }

        /// <inheritdoc cref="Binding.BindsDirectlyToSource"/>
        [DefaultValue( false )]
        public bool BindsDirectlyToSource
        {
            get { return m_binding.BindsDirectlyToSource; }
            set { m_binding.BindsDirectlyToSource = value; }
        }

        /// <inheritdoc cref="Binding.Converter"/>
        [DefaultValue( null )]
        public IValueConverter Converter
        {
            get { return m_binding.Converter; }
            set { m_binding.Converter = value; }
        }

        /// <inheritdoc cref="Binding.ConverterCulture"/>
        [TypeConverter( typeof( CultureInfoIetfLanguageTagConverter ) ), DefaultValue( null )]
        public CultureInfo ConverterCulture
        {
            get { return m_binding.ConverterCulture; }
            set { m_binding.ConverterCulture = value; }
        }

        /// <inheritdoc cref="Binding.ConverterParameter"/>
        [DefaultValue( null )]
        public object ConverterParameter
        {
            get { return m_binding.ConverterParameter; }
            set { m_binding.ConverterParameter = value; }
        }

        /// <inheritdoc cref="Binding.ElementName"/>
        [DefaultValue( null )]
        public string ElementName
        {
            get { return m_binding.ElementName; }
            set { m_binding.ElementName = value; }
        }

        /// <inheritdoc cref="Binding.FallbackValue"/>
        [DefaultValue( null )]
        public object FallbackValue
        {
            get { return m_binding.FallbackValue; }
            set { m_binding.FallbackValue = value; }
        }

        /// <inheritdoc cref="Binding.IsAsync"/>
        [DefaultValue( false )]
        public bool IsAsync
        {
            get { return m_binding.IsAsync; }
            set { m_binding.IsAsync = value; }
        }

        /// <inheritdoc cref="Binding.Mode"/>
        [DefaultValue( BindingMode.Default )]
        public BindingMode Mode
        {
            get { return m_binding.Mode; }
            set { m_binding.Mode = value; }
        }

        /// <inheritdoc cref="Binding.NotifyOnSourceUpdated"/>
        [DefaultValue( false )]
        public bool NotifyOnSourceUpdated
        {
            get { return m_binding.NotifyOnSourceUpdated; }
            set { m_binding.NotifyOnSourceUpdated = value; }
        }

        /// <inheritdoc cref="Binding.NotifyOnTargetUpdated"/>
        [DefaultValue( false )]
        public bool NotifyOnTargetUpdated
        {
            get { return m_binding.NotifyOnTargetUpdated; }
            set { m_binding.NotifyOnTargetUpdated = value; }
        }

        /// <inheritdoc cref="Binding.NotifyOnValidationError"/>
        [DefaultValue( false )]
        public bool NotifyOnValidationError
        {
            get { return m_binding.NotifyOnValidationError; }
            set { m_binding.NotifyOnValidationError = value; }
        }

        /// <inheritdoc cref="Binding.Path"/>
        [DefaultValue( null )]
        public PropertyPath Path
        {
            get { return m_binding.Path; }
            set { m_binding.Path = value; }
        }

        /// <inheritdoc cref="Binding.RelativeSource"/>
        [DefaultValue( null )]
        public RelativeSource RelativeSource
        {
            get { return m_binding.RelativeSource; }
            set { m_binding.RelativeSource = value; }
        }

        /// <inheritdoc cref="Binding.Source"/>
        [DefaultValue( null )]
        public object Source
        {
            get { return m_binding.Source; }
            set { m_binding.Source = value; }
        }

        /// <inheritdoc cref="Binding.UpdateSourceExceptionFilter"/>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
        {
            get { return m_binding.UpdateSourceExceptionFilter; }
            set { m_binding.UpdateSourceExceptionFilter = value; }
        }

        /// <inheritdoc cref="Binding.UpdateSourceTrigger"/>
        [DefaultValue( UpdateSourceTrigger.Default )]
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get { return m_binding.UpdateSourceTrigger; }
            set { m_binding.UpdateSourceTrigger = value; }
        }

        /// <inheritdoc cref="Binding.ValidatesOnDataErrors"/>
        [DefaultValue( false )]
        public bool ValidatesOnDataErrors
        {
            get { return m_binding.ValidatesOnDataErrors; }
            set { m_binding.ValidatesOnDataErrors = value; }
        }

        /// <inheritdoc cref="Binding.ValidatesOnExceptions"/>
        [DefaultValue( false )]
        public bool ValidatesOnExceptions
        {
            get { return m_binding.ValidatesOnExceptions; }
            set { m_binding.ValidatesOnExceptions = value; }
        }

        /// <inheritdoc cref="Binding.XPath"/>
        [DefaultValue( null )]
        public string XPath
        {
            get { return m_binding.XPath; }
            set { m_binding.XPath = value; }
        }

        /// <inheritdoc cref="Binding.ValidationRules"/>
        [DefaultValue( null )]
        public Collection<ValidationRule> ValidationRules
        {
            get { return m_binding.ValidationRules; }
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <inheritdoc cref="Binding.Binding()"/>
        public Bind()
        {
            m_binding = new Binding();
        }

        /// <inheritdoc cref="Binding.Binding(string)"/>
        public Bind( string path )
        {
            m_binding = new Binding( path );
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
                    return m_binding.ProvideValue( serviceProvider );
                }
            }

            return m_binding;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private Binding m_binding;
    }
}
