// Based on https://stackoverflow.com/a/4779168

using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;
using System.Windows;
using TriggerBase = Microsoft.Xaml.Behaviors.TriggerBase;

namespace Utilities.WPF.Net.InteractionExtra
{
    public class Behaviors : List<Behavior>
    {
    }

    public class Triggers : List<TriggerBase>
    {
    }

    /// <summary>
    /// Helper class to add interaction behaviors and triggers from style setters.
    /// </summary>
    public class InteractionExtra
    {
        public static Behaviors GetBehaviors( DependencyObject obj )
        {
            return (Behaviors) obj.GetValue( BehaviorsProperty );
        }

        public static void SetBehaviors( DependencyObject obj, Behaviors value )
        {
            obj.SetValue( BehaviorsProperty, value );
        }

        public static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached( "Behaviors", typeof( Behaviors ), typeof( InteractionExtra ),
                new UIPropertyMetadata( null, OnPropertyBehaviorsChanged ) );

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

        public static Triggers GetTriggers( DependencyObject obj )
        {
            return (Triggers) obj.GetValue( TriggersProperty );
        }

        public static void SetTriggers( DependencyObject obj, Triggers value )
        {
            obj.SetValue( TriggersProperty, value );
        }

        public static readonly DependencyProperty TriggersProperty =
            DependencyProperty.RegisterAttached( "Triggers", typeof( Triggers ), typeof( InteractionExtra ),
                new UIPropertyMetadata( null, OnPropertyTriggersChanged ) );

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
}
