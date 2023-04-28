/// @file
/// @copyright  Copyright (c) 2021 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Windows.Input;

namespace Utilities.WPF.Net.Commands
{
    /// <summary>
    /// <see cref="ICommand"/> that delegates its execution to an action which accepts a parameter.
    /// </summary>
    public class DelegateCommand<T> : ICommand
    {
        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        public event EventHandler? CanExecuteChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Action to execute when the command is executed.</param>
        public DelegateCommand( Action<T> execute ) : this( execute, null )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Action to execute when the command is executed.</param>
        /// <param name="canExecute">Function that evaluates if the command can be executed.</param>
        public DelegateCommand( Action<T> execute, Func<bool>? canExecute )
        {
            m_execute = execute;
            m_canExecute = canExecute;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public bool CanExecute( object? parameter )
        {
            if( m_canExecute == null )
            {
                return true;
            }

            return m_canExecute();
        }

        public void Execute( object? parameter )
        {
            m_execute( (T) parameter! );
        }

        /// <summary>
        /// Notify that the value returned by <see cref="CanExecute">CanExecute()</see> has changed.
        /// </summary>
        /// <remarks>
        /// Controls which state depends on the <see cref="CanExecute">CanExecute()</see> result will not update
        /// until this method is called.
        /// </remarks>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke( this, EventArgs.Empty );
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly Func<bool>? m_canExecute;
        private readonly Action<T> m_execute;
    }
}
