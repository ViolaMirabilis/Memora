using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Memora.Core
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        /// <summary>
        /// Whenever a new RelayCommand is created, the predicate and action are passed in and set as the backing fields.
        /// </summary>
        /// <param name="canExecute"></param>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter!);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter!);
        }
    }
}
