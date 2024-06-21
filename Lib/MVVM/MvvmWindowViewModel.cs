/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.WPF.MVVM
{
    /// <summary>
    /// Simple implementation of a viewmodel that is associated to an IMvvmWindow.
    /// </summary>
    public class MvvmWindowViewModel : IMvvmWindowViewModel
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public IMvvmWindow MvvmWindow
        {
            get => m_mvvmWindow ?? throw new InvalidOperationException( "MVVM window not initialized" );
            set => m_mvvmWindow = value;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private IMvvmWindow? m_mvvmWindow;
    }
}
