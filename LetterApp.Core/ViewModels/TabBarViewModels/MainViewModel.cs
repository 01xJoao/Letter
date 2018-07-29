﻿using System;
using System.Threading.Tasks;
using LetterApp.Core.Helpers.Commands;
using LetterApp.Core.Localization;
using LetterApp.Core.ViewModels.Abstractions;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class MainViewModel : XViewModel
    {
        public int TabIndex => AppSettings.TabIndex;

        private XPCommand _setDefaultTabCommand;
        public XPCommand SetDefaultTabCommand => _setDefaultTabCommand ?? (_setDefaultTabCommand = new XPCommand(() => AppSettings.TabIndex = 0));

        public MainViewModel() {}

        #region resources

        public string ChatTab => L10N.Localize("MainViewModel_ChatTab");
        public string CallTab => L10N.Localize("MainViewModel_CallTab");
        public string ContactTab => L10N.Localize("MainViewModel_ContactTab");
        public string ProfileTab => L10N.Localize("MainViewModel_ProfileTab");

        #endregion
    }
}
