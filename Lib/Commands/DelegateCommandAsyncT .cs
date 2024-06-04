/// @file
/// @copyright  Copyright (c) 2021-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Utilities.WPF.Net.Commands
{
    /// <summary>
    /// <see cref="ICommand"/> that delegates its execution to an action which accepts a parameter that is executed asynchronously.
    /// </summary>
    public class DelegateCommandAsync<T> : ICommand
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Indicates if the command is busy executing an asynchronous task.
        /// </summary>
        /// <remarks>
        /// If multiple executions are allowed, this value is always <c>false</c>.
        /// </remarks>
        public bool IsBusy { get; private set; }

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Action to execute asynchronously when the command is executed.</param>
        public DelegateCommandAsync( Func<T, Task> execute, bool allowMultipleExecutions = false ) : this( execute, () => true, allowMultipleExecutions )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Action to execute asynchronously when the command is executed.</param>
        /// <param name="canExecute">Function that evaluates if the command can be executed.</param>
        public DelegateCommandAsync( Func<T, Task> execute, Func<bool> canExecute, bool allowMultipleExecutions = false )
        {
            m_execute = execute;
            m_canExecute = canExecute;
            m_blockMultipleExecutions = !allowMultipleExecutions;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool CanExecute( object? parameter )
        {
            if( m_blockMultipleExecutions && IsBusy )
            {
                return false;
            }
            else
            {
                return m_canExecute();
            }
        }

        /// <inheritdoc/>
        public async void Execute( object? parameter )
        {
            try
            {
                if( m_blockMultipleExecutions )
                {
                    IsBusy = true;
                    RaiseCanExecuteChanged();
                }

                await m_execute( (T) parameter! ).ConfigureAwait( true );
            }
            finally
            {
                if( m_blockMultipleExecutions )
                {
                    IsBusy = false;
                    RaiseCanExecuteChanged();
                }
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

        private readonly Func<T, Task> m_execute;
        private readonly Func<bool> m_canExecute;
        private readonly bool m_blockMultipleExecutions;
    }
}
