/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Utilities.DotNet.WPF.MarkupExtensions
{
    /// <summary>
    /// Base for markup extensions that check emptiness of collections.
    /// </summary>
    [MarkupExtensionReturnType( typeof( bool ) )]
    public abstract class IsEmptyBase : UnaryOperationBase<IEnumerable?, bool>
    {
        //===========================================================================
        //                          PROTECTED CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected IsEmptyBase() : base( true )
        {
        }

        /// <summary>
        /// Constructor that initializes the operand.
        /// </summary>
        /// <param name="i">Operand.</param>
        protected IsEmptyBase( object i ) : this()
        {
            I = i;
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected abstract bool TransformResult( bool result );

        protected override bool CalculateValue( IEnumerable? operandValue )
        {
            bool result = true;

            if( operandValue != null )
            {
                result = !operandValue.GetEnumerator().MoveNext();
            }

            return TransformResult( result );
        }

        protected override IEnumerable? CalculateBackValue( bool targetValue, ComponentValue operandValue )
        {
            return null;
        }

        protected override void OnComponentBindingValueUpdated( int componentIndex, object? newValue, BindingExpression bindingExpression )
        {
            if( componentIndex != VALUE_INDEX )
            {
                return;
            }

            var newCollection = newValue as INotifyCollectionChanged;

            m_attachedBindingExpressions.TryGetValue( bindingExpression, out var oldCollection );

            if( oldCollection != newCollection )
            {
                if( oldCollection != null )
                {
                    oldCollection.CollectionChanged -= OnCollectionChangedEvent;
                    m_attachedBindingExpressions.Remove( bindingExpression );
                }

                if( newCollection != null )
                {
                    newCollection.CollectionChanged += OnCollectionChangedEvent;
                    m_attachedBindingExpressions.Add( bindingExpression, newCollection );
                }
            }
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void OnCollectionChangedEvent( object? sender, NotifyCollectionChangedEventArgs e )
        {
            var collection = sender as INotifyCollectionChanged;

            if( collection == null )
            {
                return;
            }

            var bindingExpressions = GetBindingExpressionsFor( collection ).ToArray();

            foreach( var bindingExpression in bindingExpressions )
            {
                bindingExpression.UpdateTarget();
            }
        }

        private IEnumerable<BindingExpression> GetBindingExpressionsFor( INotifyCollectionChanged collection )
        {
            return m_attachedBindingExpressions.Where( kvp => kvp.Value == collection ).Select( kvp => kvp.Key );
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly Dictionary<BindingExpression, INotifyCollectionChanged> m_attachedBindingExpressions = new();
    }
}
