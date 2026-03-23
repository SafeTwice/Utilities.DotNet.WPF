/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Utilities.DotNet.WPF.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="FlowDocument"/>.
    /// </summary>
    public static class FlowDocumentExtensions
    {
        /// <summary>
        /// Gets the contents of a <see cref="FlowDocument"/> encoded in
        /// <see href="https://en.wikipedia.org/wiki/Rich_Text_Format">RTF</see> format.
        /// </summary>
        /// <param name="document">Document to convert.</param>
        /// <returns>String with the document contents encoded in RTF format.</returns>
        public static string ToRtf( this FlowDocument document )
        {
            try
            {
                var tr = new TextRange( document.ContentStart, document.ContentEnd );
                using var ms = new MemoryStream();

                tr.Save( ms, DataFormats.Rtf );

                return Encoding.ASCII.GetString( ms.ToArray() );
            }
            catch
            {
                throw new InvalidDataException( "Data provided is not in the correct Document format to convert to RTF." );
            }
        }

        /// <summary>
        /// Loads the contents of a <see cref="FlowDocument"/> from a string encoded in
        /// <see href="https://en.wikipedia.org/wiki/Rich_Text_Format">RTF</see> format.
        /// </summary>
        /// <param name="document">Document to load its contents.</param>
        /// <param name="rtf">String with the document contents encoded in RTF format.</param>
        /// <exception cref="InvalidDataException">Thrown when <paramref name="rtf"/> contents is not in a valid RTF format.</exception>
        public static void LoadFromRtf( this FlowDocument document, string rtf )
        {
            try
            {
                if( string.IsNullOrEmpty( rtf ) )
                {
                    document.Blocks.Clear();
                }
                else
                {
                    var tr = new TextRange( document.ContentStart, document.ContentEnd );
                    using var ms = new MemoryStream( Encoding.ASCII.GetBytes( rtf ) );

                    tr.Load( ms, DataFormats.Rtf );
                }
            }
            catch
            {
                throw new InvalidDataException( "Data provided is not in the correct RTF format to convert to Document." );
            }
        }

        /// <summary>
        /// Gets the contents of a <see cref="FlowDocument"/> encoded in 
        /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.windows.dataformats.xamlpackage">XamlPackage</see> format.
        /// </summary>
        /// <remarks>
        /// The XamlPackage format is internal to WPF and is not meant to be manipulated. The returned value is only useful for being
        /// stored temporally (e.g., as an runtime snapshot of the document contents) for being later loaded into a <see cref="FlowDocument"/>
        /// using the <see cref="LoadFromXamlPackage(FlowDocument, byte[])"/> method. It is not recommended to use this format for long-term
        /// storage since it is not a standard format and may change in future versions of WPF.
        /// </remarks>
        /// <param name="document">Document to convert.</param>
        /// <returns>Byte array with the document contents encoded in XamlPackage format.</returns>
        public static byte[] ToXamlPackage( this FlowDocument document )
        {
            try
            {
                var tr = new TextRange( document.ContentStart, document.ContentEnd );
                using var ms = new MemoryStream();

                tr.Save( ms, DataFormats.XamlPackage );

                return ms.ToArray();
            }
            catch
            {
                throw new InvalidDataException( "Data provided is not in the correct Document format to convert to Xaml Package." );
            }
        }

        /// <summary>
        /// Loads the contents of a <see cref="FlowDocument"/> from a byte array encoded in
        /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.windows.dataformats.xamlpackage">XamlPackage</see> format.
        /// </summary>
        /// <param name="document">Document to load its contents.</param>
        /// <param name="xamlPackage">Byte array with the document contents encoded in XamlPackage format.</param>
        /// <exception cref="InvalidDataException">Thrown when <paramref name="xamlPackage"/> contents is not in a valid XamlPackage format.</exception>
        public static void LoadFromXamlPackage( this FlowDocument document, byte[] xamlPackage )
        {
            try
            {
                if( xamlPackage.Length == 0 )
                {
                    document.Blocks.Clear();
                }
                else
                {
                    var tr = new TextRange( document.ContentStart, document.ContentEnd );
                    using var ms = new MemoryStream( xamlPackage );

                    tr.Load( ms, DataFormats.XamlPackage );
                }
            }
            catch
            {
                throw new InvalidDataException( "Data provided is not in the correct Xaml Package format to convert to Document." );
            }
        }
    }
}
