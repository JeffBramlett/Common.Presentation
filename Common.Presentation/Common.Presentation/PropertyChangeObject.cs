﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Presentation
{
    /// <summary>
    /// Base class for Property changes
    /// </summary>
    public abstract class PropertyChangeObject: INotifyPropertyChanged
    {

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Set the property and notifies any listeners that it changed (if it did)
        /// </summary>
        /// <typeparam name="T">the type of the property (can be inferred from arguments)</typeparam>
        /// <param name="field">the backing field variable name</param>
        /// <param name="value">the new value</param>
        /// <param name="memberExpression">the anonymous expression of the property</param>
        /// <param name="moreNotifications">other notifications is needed (does not set any values)</param>
        protected void SetProperty<T>(ref T field, T value, Expression<Func<T>> memberExpression, params Expression<Func<object>>[] moreNotifications)
        {
            // Must have member expression to find property name
            if (memberExpression == null)
            {
                throw new ArgumentNullException();
            }

            var bodyExpr = memberExpression.Body as MemberExpression;

            // Member expression must have a body (a property)
            if (bodyExpr == null)
            {
                throw new ArgumentNullException();
            }

            // don't do anything unless the value changes
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }

            field = value;

            RaisePropertyChanged(memberExpression, moreNotifications);
        }


        /// <summary>
        /// Raise property changed by names only (value needs to be reread)
        /// </summary>
        /// <typeparam name="T">the type of the property (can be inferred from arguments)</typeparam>
        /// <param name="memberExpression">the anonymous expression of the property</param>
        /// <param name="moreNotifications">other notifications as needed (does not set any values)</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> memberExpression, params Expression<Func<object>>[] moreNotifications)
        {
            // Must have member expression to find property name
            if (memberExpression == null)
            {
                throw new ArgumentNullException();
            }

            var bodyExpr = memberExpression.Body as MemberExpression;

            // Member expression must have a body (a property)
            if (bodyExpr == null)
            {
                throw new ArgumentNullException();
            }

            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(bodyExpr.Member.Name));
                foreach (Expression<Func<object>> notifyAlso in moreNotifications)
                {
                    if (notifyAlso != null)
                    {
                        var alsoExpr = notifyAlso.Body as UnaryExpression;
                        if (alsoExpr != null)
                        {
                            var nextExpr = alsoExpr.Operand as MemberExpression;
                            handler(this, new PropertyChangedEventArgs(nextExpr.Member.Name));
                        }
                    }
                }
            }
        }

    }
}
