/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Utilities.WPF.Net.Windows
{
    /// <summary>
    /// Window utilities.
    /// </summary>
    public static class WindowsUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public static void NavigateTo( this Window fromWindow, Window window )
        {
            window.Closed += ( sender, e ) =>
            {
                fromWindow.Show();
            };

            fromWindow.Hide();
            window.Show();
        }

        public static void DisableCloseButton( this Window window )
        {
            IntPtr hwnd = new WindowInteropHelper( window ).EnsureHandle();
            IntPtr hMenu = GetSystemMenu( hwnd, false );

            if( hMenu != IntPtr.Zero )
            {
                EnableMenuItem( hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED );
            }
        }

        public static void EnableCloseButton( this Window window )
        {
            IntPtr hwnd = new WindowInteropHelper( window ).EnsureHandle();
            IntPtr hMenu = GetSystemMenu( hwnd, false );

            if( hMenu != IntPtr.Zero )
            {
                EnableMenuItem( hMenu, SC_CLOSE, MF_ENABLED );
            }
        }

        //===========================================================================
        //                           PRIVATE METHODS
        //===========================================================================

        [DllImport( "user32.dll" )]
        private static extern IntPtr GetSystemMenu( IntPtr hWnd, bool bRevert );

        [DllImport( "user32.dll" )]
        private static extern bool EnableMenuItem( IntPtr hMenu, uint uIDEnableItem, uint uEnable );

        //===========================================================================
        //                           PRIVATE CONSTANTS
        //===========================================================================

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint MF_ENABLED = 0x00000000;

        private const uint SC_CLOSE = 0xF060;
    }
}
