using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LetterApp.Core.Services.Interfaces;
using Realms;

namespace LetterApp.Core.ViewModels.Abstractions
{
    public abstract class XViewModel : IXViewModel, INotifyPropertyChanged
    {
        protected Realm Realm;

        private static IXNavigationService _navigationService;
        protected static IXNavigationService NavigationService = _navigationService ?? (_navigationService = App.Container.GetInstance<IXNavigationService>());

        public event PropertyChangedEventHandler PropertyChanged;
        private static readonly PropertyChangedEventArgs AllPropertiesChanged = new PropertyChangedEventArgs(string.Empty);

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(nameof(IsBusy)); }
        }

        public void InitializeViewModel()
        {
            Realm = Realm.GetInstance();
            InitializeAsync();
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual Task Appearing() => Task.CompletedTask;

        public virtual Task Disappearing() => Task.CompletedTask;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) 
                return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        public void RaisePropertyChanged(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public virtual void Prepare() {}

        public virtual void Prepare(object dataObject) {}

    }
    public abstract class XViewModel<TObject> : XViewModel
    {
        public override void Prepare(object dataObject) => Prepare((TObject)dataObject);

        protected abstract void Prepare(TObject data);
    }
}
