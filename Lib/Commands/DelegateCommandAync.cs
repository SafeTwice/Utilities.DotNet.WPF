/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Utilities.WPF.Net.Commands
{
    /// <summary>
    /// <see cref="ICommand"/> that delegates its execution to an action that is executed asynchronously.
    /// </summary>
    public class DelegateCommandAsync : ICommand
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public bool IsBusy { get; private set; }

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
        /// <param name="execute">Action to execute asynchronously when the command is executed.</param>
        public DelegateCommandAsync( Func<Task> execute ) : this( execute, null )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Action to execute asynchronously when the command is executed.</param>
        /// <param name="canExecute">Function that evaluates if the command can be executed.</param>
        public DelegateCommandAsync( Func<Task> execute, Func<bool>? canExecute )
        {
            m_execute = execute;
            m_canExecute = canExecute;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public bool CanExecute( object? parameter )
        {
            if( IsBusy )
            {
                return false;
            }
            else if( m_canExecute == null )
            {
                return true;
            }
            else
            {
                return m_canExecute();
            }
        }

        public async void Execute( object? parameter )
        {
            try
            {
                IsBusy = true;
                RaiseCanExecuteChanged();

                await m_execute().ConfigureAwait( false );
            }
            finally
            {
                IsBusy = false;
                RaiseCanExecuteChanged();
            }
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
        private readonly Func<Task> m_execute;
    }
}
