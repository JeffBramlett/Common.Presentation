using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.Presentation
{
    /// <summary>
    /// Abstract class for ViewModels
    /// </summary>
    public class AbstractViewModel : INotifyPropertyChanged
    {
        #region Fields
        private IDialogProvider _dialogProvider;
        #endregion

        #region Protected Properties
        /// <summary>
        /// Dialog provider for ViewModels to open Dialogs and get results
        /// </summary>
        protected IDialogProvider DialogProvider
        {
            get
            {
                if (_dialogProvider == null)
                {
                    _dialogProvider = new DialogProvider();
                }

                return _dialogProvider;
            }
            set { _dialogProvider = value; }
        }
        #endregion

        #region Ctors and Dtors
        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="dialogProvider">optional dialog provider (default is DialogProvider)</param>
        public AbstractViewModel(IDialogProvider dialogProvider = null)
        {
            _dialogProvider = dialogProvider;
        }
        #endregion

        #region Protected Command Creation
        /// <summary>
        /// Create a command object
        /// </summary>
        /// <param name="cmdExe">the delegate to execute when the command is raised</param>
        /// <returns>The ICommand object for binding</returns>
        protected ICommand CreateCommand(Action cmdExe)
        {
            ICommand cmd = new DelegateCommand(cmdExe);
            return cmd;
        }

        /// <summary>
        /// Create a command object with delegate to determine if the command can be called
        /// </summary>
        /// <param name="cmdExe">the delegate to execute when the command is raised</param>
        /// <param name="canExecuteCmd">the delegate which returns if the command can be executed</param>
        /// <returns>The ICommand object for binding</returns>
        protected ICommand CreateCommand(Action cmdExe, Func<bool> canExecuteCmd)
        {
            ICommand cmd = new DelegateCommand(cmdExe, canExecuteCmd);
            return cmd;
        }

        /// <summary>
        /// Create command with generic type
        /// </summary>
        /// <typeparam name="M">the type for the command</typeparam>
        /// <param name="cmdExe">the delegate to execute when the command is raised</param>
        /// <returns>The ICommand object for binding</returns>
        protected ICommand CreateCommand<M>(Action<M> cmdExe)
        {
            ICommand cmd = new DelegateCommand<M>(cmdExe);
            return cmd;
        }

        /// <summary>
        /// Create command with generic type for delegate that determines if it can be called
        /// </summary>
        /// <typeparam name="M">the type for the command</typeparam>
        /// <param name="cmdExe">the delegate to execute when the command is raised</param>
        /// <param name="canExecuteCmd">delegate to determine if the command can be executed</param>
        /// <returns>The ICommand object for binding</returns>
        protected ICommand CreateCommand<M>(Action<M> cmdExe, Func<M, bool> canExecuteCmd)
        {
            ICommand cmd = new DelegateCommand<M>(cmdExe, canExecuteCmd);
            return cmd;
        }

        #endregion

        #region Delegates and Events
        /// <summary>
        /// Exception delegate for Exceptions that may occur in ViewModel
        /// </summary>
        /// <param name="ex">the exception that occurred</param>
        public delegate void ViewModelExceptionDelegate(Exception ex);

        /// <summary>
        /// Event for exceptions that occurred in ViewModel
        /// </summary>
        public event ViewModelExceptionDelegate ViewModelException;

        #endregion

        #region Protected INotifyPropertyChanged Implementation

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
        /// <param name="moreNotifications">other notifications is needed (does not set any values)</param>
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
                        var alsoExpr = notifyAlso.Body as MemberExpression;
                        handler(this, new PropertyChangedEventArgs(alsoExpr.Member.Name));
                    }

                }
            }
        }

        /// <summary>
        /// Raise and exception for the event
        /// </summary>
        /// <param name="ex">the exception to raise</param>
        protected void RaiseException(Exception ex)
        {
           Task.Run(() =>
           {
               ViewModelException?.Invoke(ex);
           });
        }
        #endregion

    }
}
