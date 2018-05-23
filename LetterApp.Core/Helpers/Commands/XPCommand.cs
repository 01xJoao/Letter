using System;
namespace LetterApp.Core.Helpers.Commands
{
    public class XPCommand
    {
        private readonly Action _executeMethod;
        private readonly Func<bool> _canExecuteMethod;

        protected static bool DefaultCanExecuteMethod() => true;

        public XPCommand(Action executeMethod, Func<bool> canExecuteMethod = null)
        {
            _executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod), "XPCommand Delegates Cannot Be Null");
            _canExecuteMethod = canExecuteMethod ?? DefaultCanExecuteMethod;
        }

        public void Execute() => _executeMethod();
        public bool CanExecute() => _canExecuteMethod();
    }
}
