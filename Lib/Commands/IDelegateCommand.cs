/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Windows.Input;

namespace Utilities.DotNet.WPF.Commands
{
    /// <summary>
    /// <see cref="ICommand"/> that delegates its execution.
    /// </summary>
    public interface IDelegateCommand : ICommand
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Notify that the value returned by <see cref="ICommand.CanExecute">CanExecute()</see> has changed.
        /// </summary>
        /// <remarks>
        /// Controls which state depends on the <see cref="ICommand.CanExecute">CanExecute()</see> result will not update
        /// until this method is called.
        /// </remarks>
        public void RaiseCanExecuteChanged();
    }
}
