// Based on https://stackoverflow.com/a/4779168

using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;
using System.Windows;
using TriggerBase = Microsoft.Xaml.Behaviors.TriggerBase;

namespace Utilities.DotNet.WPF.InteractionExtra
{
    /// <summary>
    /// Represents a collection of <see cref="Behavior"/> objects.
    /// </summary>
    public class Behaviors : List<Behavior>
    {
    }

    /// <summary>
    /// Represents a collection of <see cref="TriggerBase"/> objects.
    /// </summary>
    public class Triggers : List<TriggerBase>
    {
    }

#pragma warning disable S1118 // Utility classes should not have public constructors

    /// <summary>
    /// Helper class to add interaction behaviors and triggers from style setters.
    /// </summary>
    public class InteractionExtra
    {
        /// <summary>
        /// The <see cref="Behaviors"/> attached property.
        /// </summary>
        public static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached( "Behaviors", typeof( Behaviors ), typeof( InteractionExtra ),
                new UIPropertyMetadata( null, OnPropertyBehaviorsChanged ) );

        /// <summary>
        /// Gets the <see cref="Behaviors"/> of a <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="obj">A dependency object.</param>
        /// <returns>The <see cref="Behaviors"/> of the object.</returns>
        public static Behaviors GetBehaviors( DependencyObject obj )
        {
            return (Behaviors) obj.GetValue( BehaviorsProperty );
        }

        /// <summary>
        /// Sets the <see cref="Behaviors"/> of a <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="obj">A dependency object.</param>
        /// <param name="value">The <see cref="Behaviors"/> to set.</param>
        public static void SetBehaviors( DependencyObject obj, Behaviors value )
        {
            obj.SetValue( BehaviorsProperty, value );
        }

        private static void OnPropertyBehaviorsChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var currentBehaviors = Interaction.GetBehaviors( d );
            var newBehaviors = e.NewValue as Behaviors;

            if( newBehaviors != null )
            {
                foreach( var behavior in newBehaviors )
                {
                    currentBehaviors.Add( behavior );
                }
            }
        }

        /// <summary>
        /// The <see cref="Triggers"/> attached property.
        /// </summary>
        public static readonly DependencyProperty TriggersProperty =
            DependencyProperty.RegisterAttached( "Triggers", typeof( Triggers ), typeof( InteractionExtra ),
                new UIPropertyMetadata( null, OnPropertyTriggersChanged ) );

        /// <summary>
        /// Gets the <see cref="Triggers"/> of a <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="obj">A dependency object.</param>
        /// <returns>The <see cref="Triggers"/> of the object.</returns>
        public static Triggers GetTriggers( DependencyObject obj )
        {
            return (Triggers) obj.GetValue( TriggersProperty );
        }

        /// <summary>
        /// Sets the <see cref="Triggers"/> of a <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="obj">A dependency object.</param>
        /// <param name="value">The <see cref="Triggers"/> to set.</param>
        public static void SetTriggers( DependencyObject obj, Triggers value )
        {
            obj.SetValue( TriggersProperty, value );
        }

        private static void OnPropertyTriggersChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var currentTriggers = Interaction.GetTriggers( d );
            var newTriggers = e.NewValue as Triggers;
            if( newTriggers != null )
            {
                foreach( var trigger in newTriggers )
                {
                    currentTriggers.Add( trigger );
                }
            }
        }
    }
#pragma warning restore S1118
}
