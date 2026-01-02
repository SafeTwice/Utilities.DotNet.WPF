/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;

#pragma warning disable IDE0130

namespace Utilities.DotNet.WPF.AttachedProperties
{
    /// <summary>
    /// Container for rich text in XamlPackage format.
    /// </summary>
    public sealed class XamlPackageContainer
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Content in XamlPackage format.
        /// </summary>
        public byte[] EncodedData { get; }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="encodedData">Contents in XamlPackage format.</param>
        public XamlPackageContainer( byte[] encodedData )
        {
            EncodedData = encodedData;
            m_packageInfo = GetXamlPackageInfo( encodedData );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc />
        public override bool Equals( object? obj )
        {
            return ( obj is XamlPackageContainer other ) && m_packageInfo.Equals( other.m_packageInfo );
        }

        /// <inheritdoc />
        public override int GetHashCode() => m_packageInfo.GetHashCode();

        //===========================================================================
        //                          PRIVATE NESTED TYPES
        //===========================================================================

        private struct XamlPartInfo
        {
            public Uri Uri { get; }
            public string ContentType { get; }
            public byte[] Hash { get; }

            public XamlPartInfo( Uri uri, string contentType, byte[] hash )
            {
                Uri = uri;
                ContentType = contentType;
                Hash = hash;
            }

            public override bool Equals( object? obj )
            {
                if( obj is XamlPartInfo other )
                {
                    return Uri == other.Uri &&
                           ContentType == other.ContentType &&
                           Hash.SequenceEqual( other.Hash );
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode() => Uri.GetHashCode() ^ ContentType.GetHashCode() ^
                                                 ( (IStructuralEquatable) Hash ).GetHashCode( EqualityComparer<byte>.Default );
        }

        private struct XamlPackageInfo
        {
            public XamlPartInfo[] Parts { get; }

            public XamlPackageInfo( XamlPartInfo[] parts )
            {
                Parts = parts;
            }

            public override bool Equals( object? obj )
            {
                if( ( obj is XamlPackageInfo other ) && ( Parts.Length == other.Parts.Length ) )
                {
                    for( int i = 0; i < Parts.Length; i++ )
                    {
                        if( !Parts[ i ].Equals( other.Parts[ i ] ) )
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode() => ( (IStructuralEquatable) this.Parts ).GetHashCode( EqualityComparer<XamlPartInfo>.Default );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static XamlPackageInfo GetXamlPackageInfo( byte[] xamlPackage )
        {

            using var stream = new MemoryStream( xamlPackage );
            using var package = Package.Open( stream, FileMode.Open, FileAccess.Read );

            var partsInfo = package.GetParts()
                .Where( part => part.ContentType != "application/vnd.openxmlformats-package.relationships+xml" )
                .Select( part =>
                {
                    using var partStream = part.GetStream();

                    var partHash = Hasher.ComputeHash( partStream );

                    return new XamlPartInfo( part.Uri, part.ContentType, partHash );
                } ).ToArray();

            var xamlPackageInfo = new XamlPackageInfo( partsInfo );
            return xamlPackageInfo;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private static readonly SHA256 Hasher = SHA256.Create();

        private XamlPackageInfo m_packageInfo;
    }
}
