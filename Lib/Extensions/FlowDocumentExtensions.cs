/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Utilities.DotNet.WPF.Extensions
{
    public static class FlowDocumentExtensions
    {
        public static string ToRtf( this FlowDocument document )
        {
            try
            {
                TextRange tr = new TextRange( document.ContentStart, document.ContentEnd );
                using( MemoryStream ms = new MemoryStream() )
                {
                    tr.Save( ms, DataFormats.Rtf ); //Reminder this line of code throws:" Exception thrown: 'System.FormatException' in System.Private.CoreLib.dll "
                    string convertedString = ASCIIEncoding.Default.GetString( ms.ToArray() );
                    return convertedString;
                }
            }
            catch
            {
                throw new InvalidDataException( "Data provided is not in the correct Document format to convert to RTF." );
            }
        }

        public static void LoadFromRtf( this FlowDocument document, string rtf )
        {
            try
            {
                if( String.IsNullOrEmpty( rtf ) )
                {
                    document.Blocks.Clear();
                }
                else
                {
                    TextRange tr = new TextRange( document.ContentStart, document.ContentEnd );
                    using( MemoryStream ms = new MemoryStream( Encoding.ASCII.GetBytes( rtf ) ) )
                    {
                        tr.Load( ms, DataFormats.Rtf );
                    }
                }
            }
            catch
            {
                throw new InvalidDataException( "Data provided is not in the correct RTF format to convert to Document." );
            }
        }

        public static byte[] ToXamlPackage( this FlowDocument document )
        {
            try
            {
                TextRange tr = new TextRange( document.ContentStart, document.ContentEnd );
                using( MemoryStream ms = new MemoryStream() )
                {
                    tr.Save( ms, DataFormats.XamlPackage ); //Reminder this line of code throws:" Exception thrown: 'System.FormatException' in System.Private.CoreLib.dll "
                    return ms.ToArray();
                }
            }
            catch
            {
                throw new InvalidDataException( "Data provided is not in the correct Document format to convert to Xaml Package." );
            }
        }

        public static void LoadFromXamlPackage( this FlowDocument document, byte[] xamlPackage )
        {
            try
            {
                if( xamlPackage == null || xamlPackage.Length == 0 )
                {
                    document.Blocks.Clear();
                }
                else
                {
                    TextRange tr = new TextRange( document.ContentStart, document.ContentEnd );
                    using( MemoryStream ms = new MemoryStream( xamlPackage ) )
                    {
                        tr.Load( ms, DataFormats.XamlPackage );
                    }
                }
            }
            catch
            {
                throw new InvalidDataException( "Data provided is not in the correct Xaml Package format to convert to Document." );
            }
        }
    }
}
