using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Live_Cricket_2._0.Model
{
    public class RelayCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            { 
                CommandManager.RequerySuggested -= value; ;
            }
        }

        private Func<object, bool> canExecute;
        private Action<object> execute;
        public RelayCommand(Action<object> i_execute, Func<object, bool> i_canExecute = null)
        {
            this.canExecute = i_canExecute;
            this.execute = i_execute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }


}
