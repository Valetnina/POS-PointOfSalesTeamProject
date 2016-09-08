using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS_ViewsLibrary
{/*
    public class ActionCommand : ICommand
    {
        Action _TargetExecuteMethod; 
      Func<bool> _TargetCanExecuteMethod;
		
      public ActionCommand(Action executeMethod) {
         _TargetExecuteMethod = executeMethod; 
      }
		
      public ActionCommand(Action executeMethod, Func<bool> canExecuteMethod){ 
         _TargetExecuteMethod = executeMethod;
         _TargetCanExecuteMethod = canExecuteMethod; 
      }
		
      public void RaiseCanExecuteChanged() { 
         CanExecuteChanged(this, EventArgs.Empty); 
      }
		
      bool ICommand.CanExecute(object parameter) { 
		
         if (_TargetCanExecuteMethod != null) { 
            return _TargetCanExecuteMethod(); 
         } 
			
         if (_TargetExecuteMethod != null) { 
            return true; 
         } 
			
         return false; 
      }
		
     // Beware - should use weak references if command instance lifetime 
      //   is longer than lifetime of UI objects that get hooked up to command 
			
      // Prism commands solve this in their implementation public event 
      EventHandler CanExecuteChanged = delegate { };
		
      void ICommand.Execute(object parameter) { 
         if (_TargetExecuteMethod != null) {
            _TargetExecuteMethod(); 
         } 
      }


    
    }*/
    public class ActionCommand : ICommand
    {

        private readonly Action<Object> action;
        private readonly Predicate<Object> predicate;

        public ActionCommand(Action<Object> action) : this(action, null)
        {
        }

        public ActionCommand(Action<Object> action, Predicate<Object> predicate)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", "You must specify an Action<T>.");
            }

            this.action = action;
            this.predicate = predicate;
        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object parameter)
        {
            if (predicate == null)
            {
                return true;
            }
            return predicate(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            action(parameter);
        }

        public void Execute()
        {
            Execute(null);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}/*
public class ActionCommand<T> : ICommand
    {
        Action<T> _TargetExecuteMethod;
        Func<T, bool> _TargetCanExecuteMethod;

        public ActionCommand(Action<T> executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public ActionCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {

            if (_TargetCanExecuteMethod != null)
            {
                T tparm = (T)parameter;
                return _TargetCanExecuteMethod(tparm);
            }

            if (_TargetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        //  Beware - should use weak references if command instance lifetime is
        // longer than lifetime of UI objects that get hooked up to command

        //Prism commands solve this in their implementation 

        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod((T)parameter);
            }
        }

        #endregion
    }
}
        // Second version
        //private readonly Action<Object> action;
        //private readonly Predicate<Object> predicate;
        //public ActionCommand(Action<Object> action) : this(action, null)
        //{
        //}
        //public ActionCommand(Action<Object> action, Predicate<Object> predicate)
        //{
        //    if (action == null)
        //    {
        //        throw new ArgumentNullException("action", "You must specify an Action<T>");
        //    }

        //    this.action = action;
        //    this.predicate = predicate;

        //}
        //#region ICommand Members

        //public event EventHandler CanExecuteChanged;
        ///*
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }*/


        //public bool CanExecute(object parameter)
        //{
        //    if (predicate == null)
        //    {
        //        return true;
        //    }
        //    return predicate(parameter);
        //}

        //public void Execute(object parameter)
        //{
        //    action(parameter);
        //}
        //public void Execute()
        //{
        //    Execute(null);
        //}
        //#endregion


      
    //}

