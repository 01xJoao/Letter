using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterApp.Core.Exceptions;
using LetterApp.Core.Localization;
using LetterApp.Core.Models;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels.Abstractions;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.ViewModels.TabBarViewModels
{
    public class UserProfileViewModel : XViewModel
    {
        private IAuthenticationService _autheticationService;

        private UserModel _user;

        private bool _updateView;
        public bool UpdateView
        {
            get => _updateView;
            set => SetProperty(ref _updateView, value);
        }

        public ProfileHeaderModel ProfileHeader { get; private set; }
        public ProfileDetailsModel ProfileDetails { get; private set; }
        public ProfileDivisionModel ProfileDivision { get; private set; }

        public UserProfileViewModel(IAuthenticationService autheticationService)
        {
            _autheticationService = autheticationService;
        }

        public override async Task InitializeAsync()
        {
            SetupModels();

            try
            {
                var userCheck = await _autheticationService.CheckUser();

                if (userCheck.StatusCode == 200)
                {
                    if (userCheck.LastUpdateTime > _user.LastUpdateTime)
                        _updateView = true;

                    var user = new UserModel();

                    user.UserID = userCheck.UserID;
                    user.Email = userCheck.Email;
                    user.FirstName = userCheck.FirstName;
                    user.LastName = userCheck.LastName;
                    user.Position = userCheck.Position;
                    user.Picture = userCheck.Picture;
                    user.Description = userCheck.Description;
                    user.ContactNumber = userCheck.ContactNumber;
                    user.ShowContactNumber = userCheck.ShowContactNumber;
                    user.OrganizationID = userCheck.OrganizationID;
                    foreach (var divion in userCheck.Divisions)
                        user.Divisions.Add(divion);
                    user.LastUpdateTime = userCheck.LastUpdateTime;

                    Realm.Write(() => {
                        Realm.Add(user, true);
                    }); 
                }
            }
            catch (Exception ex)
            {
                Ui.Handle(ex as dynamic);
            }
            finally
            {
                if(_updateView)
                {
                    SetupModels();
                    UpdateView = true;
                }
            }
        }

        private void SetupModels()
        {
            _user = Realm.Find<UserModel>(AppSettings.UserId);

            ProfileHeader = new ProfileHeaderModel($"{_user.FirstName} {_user.LastName}", _user.Description, _user.Picture, UpdateDescription, OpenSettings);

            var divisions = new List<ProfileDivision>();

            if(_user.Divisions != null)
            {
                foreach(var division in _user.Divisions)
                {
                    var div = new ProfileDivision();
                    div.Id = division.DivisionID;
                    div.Name = division.Name;
                    div.Picture = division.Picture;
                    divisions.Add(div);
                }
            }

            ProfileDivision = new ProfileDivisionModel(divisions, OpenDivision, AddDivision);

            var profileDetails = new List<ProfileDetail>();

            var profileDetail1 = new ProfileDetail();
            profileDetail1.Description = Role;
            profileDetail1.Value = _user.Position;

            var profileDetail2 = new ProfileDetail();
            profileDetail2.Description = Email;
            profileDetail2.Value = _user.Email;

            var profileDetail3 = new ProfileDetail();
            profileDetail3.Description = Mobile;
            profileDetail3.Value = _user.ContactNumber;

            var details = new[] { profileDetail1, profileDetail2, profileDetail3 };
            profileDetails.AddRange(details);

            ProfileDetails = new ProfileDetailsModel(profileDetails);
        }

        private void OpenDivision(object sender, int e)
        {

        }

        private void AddDivision(object sender, int e)
        {
           
        }

        private void OpenSettings(object sender, EventArgs e)
        {

        }

        private void UpdateDescription(object sender, string description)
        {

        }

        #region Resources

        private string Role     => L10N.Localize("UserProfile_Role");
        private string Email    => L10N.Localize("UserProfile_Email");
        private string Mobile   => L10N.Localize("UserProfile_Mobile");

        #endregion
    }
}
