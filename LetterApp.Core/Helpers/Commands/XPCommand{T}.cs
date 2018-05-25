using System;
namespace LetterApp.Core.Helpers.Commands
{
    public class XPCommand<TParameter>
    {
        private readonly Action<TParameter> _executeMethod;
        private readonly Func<TParameter, bool> _canExecuteMethod;

        private bool DefaultCanExecuteMethod(TParameter arg) => true;

        public XPCommand(Action<TParameter> executeMethod, Func<TParameter, bool> canExecuteMethod = null)
        {
            _executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod), "XPCommand Delegates Cannot Be Null");
            _canExecuteMethod = canExecuteMethod ?? DefaultCanExecuteMethod;
        }

        public void Execute(TParameter parameter) => _executeMethod(parameter);
        public bool CanExecute(TParameter parameter) => _canExecuteMethod(parameter);
    }
}
