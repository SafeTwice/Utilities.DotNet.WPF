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
    /// </para>Objects that want to activate the trigger should call the <see cref="Activate"/> method.</para>
    /// </remarks>
    public class DelegateTrigger
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public event Action? Activated;

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Activates the trigger.
        /// </summary>
        public void Activate()
        {
            Activated?.Invoke();
        }
    }
}
