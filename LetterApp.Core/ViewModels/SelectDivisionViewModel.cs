using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;
using SharpRaven.Data;

namespace LetterApp.Core.ViewModels
{
    public class SelectDivisionViewModel : XViewModel<int>
    {
        private IDialogService _dialogService;
        private IStatusCodeService _statusCode;
        private IOrganizationService _organizationService;

        private int _organizationId;

        private List<DivisionModel> _divisions;
        public List<DivisionModel> Divisions
        {
            get => _divisions;
            set => SetProperty(ref _divisions, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public SelectDivisionViewModel(IOrganizationService organizationService, IDialogService dialogService, IStatusCodeService statusCode)
        {
            _organizationService = organizationService;
            _dialogService = dialogService;
            _statusCode = statusCode;
        }

        protected override void Prepare(int organizationId)
        {
            _organizationId = organizationId;
        }

        public override async Task InitializeAsync()
        {
            SetL10NResources();

            IsBusy = true;
            IsLoading = true;

            try
            {
                Divisions = await _organizationService.GetDivisions(_organizationId);
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                IsBusy = false;
                IsLoading = false;
            }
        }

        #region Resources

        public string TitleLabel => L10N.Localize("SelectDivision_TitleLabel");

        public Dictionary<string, string> LocationResources = new Dictionary<string, string>();

        private string privateDivisionLabel => L10N.Localize("SelectDivision_PrivateLabel");
        private string publicDivisionLabel => L10N.Localize("SelectOrganization_PublicLabel");
        private string insertDivisionText => L10N.Localize("SelectOrganization_InsertText");
        private string submitButton => L10N.Localize("SelectOrganization_SubmitButton");
        private string leaveOrganizationButton => L10N.Localize("SelectOrganization_LeaveOrganization");

        private void SetL10NResources()
        {
            LocationResources.Add("Private", privateDivisionLabel);
            LocationResources.Add("Public", publicDivisionLabel);
            LocationResources.Add("Insert", insertDivisionText);
            LocationResources.Add("Submit", submitButton);
            LocationResources.Add("Leave", leaveOrganizationButton);
        }

        #endregion
    }
}
