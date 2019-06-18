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
    public class AbstractViewModel : PropertyChangeObject
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
