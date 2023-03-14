/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows;

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
            window.Closed += (sender, e) =>
            {
                fromWindow.Show();
            };

            fromWindow.Hide();
            window.Show();
        }
    }
}
