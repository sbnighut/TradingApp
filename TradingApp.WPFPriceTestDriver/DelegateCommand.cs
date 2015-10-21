using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Trade.WPFPriceTestDriver
{
    /// <summary>
    /// This class defines concrete command that is flexible enough to change its receiver(_canExecute, _action)
    /// The invoker(can be a button, tabitem etc...) is just going to call Execute method from the ICommand interface
    /// Actual method to execute is registered when the delegateCommand instance is created
    /// </summary>
    public class DelegateCommand : ICommand
    {
        Func<bool> _canExecute;
        Action _action;

        /// <summary>
        /// Defines the constructor of the DelegateCommand initializing both the delegates Func(boolean delegate) as well as Action(void delegate)
        /// </summary>
        /// <param name="canExecute"></param>
        /// <param name="actionToExecute"></param>
        public DelegateCommand(Func<bool> canExecute, Action actionToExecute)
        {
            _canExecute = canExecute;
            _action = actionToExecute;
        }

        /// <summary>
        /// Calls the Func _canExecute which is a boolean function in order to 
        /// decide whether to perform the action or not
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        /// <summary>
        /// Enables or disables the button if the event is raised.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// The CanExecute is again invoked in order to decide if the button should be enabled or not
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Implements the interface virtual method
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action();
        }

        /// <summary>
        /// This method is used in the unit testing phase
        /// </summary>
        public void Execute()
        {
            _action();
        }

    }
}
