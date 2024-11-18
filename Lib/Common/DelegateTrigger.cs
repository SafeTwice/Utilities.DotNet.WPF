/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.WPF.Common
{
    /// <summary>
    /// This class is used to activate a trigger.
    /// </summary>
    /// <remarks>
    /// <para>Objects that want to be notified when the trigger is activated should subscribe to the <see cref="Activated"/> event.</para>
    /// <para>Objects that want to activate the trigger should call the <see cref="Activate"/> method.</para>
    /// <para>If no event listeners are registered when the trigger is activated, the trigger is delayed until an event listener is registered. </para>
    /// </remarks>
    public class DelegateTrigger
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Event that is triggered when the trigger is activated.
        /// </summary>
        public event Action Activated
        {
            add
            {
                lock( objectLock )
                {
                    m_eventHandler += value;

                    if( m_activationPending )
                    {
                        m_activationPending = false;
                        value.Invoke();
                    }
                }
            }

            remove
            {
                lock( objectLock )
                {
                    m_eventHandler -= value;
                }
            }
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Activates the trigger.
        /// </summary>
        public void Activate()
        {
            lock( objectLock )
            {
                if( m_eventHandler != null )
                {
                    m_eventHandler.Invoke();
                }
                else
                {
                    m_activationPending = true;
                }
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly object objectLock = new Object();

        private Action? m_eventHandler;

        private bool m_activationPending = false;
    }
}
