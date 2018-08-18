using System;
using System.Linq;
using LetterApp.Core.Localization;
using LetterApp.Core.Models.DTO.ReceivedModels;
using LetterApp.Models.DTO.ReceivedModels;
using Realms;

namespace LetterApp.Core.Helpers
{
    public static class RealmUtils
    {
        public static UserModel UpdateUser(Realm realm, CheckUserModel userCheck)
        {
            var user = new UserModel
            {
                UserID = userCheck.UserID,
                Email = userCheck.Email,
                FirstName = userCheck.FirstName,
                LastName = userCheck.LastName,
                Position = userCheck.Position,
                Picture = userCheck.Picture,
                Description = userCheck.Description,
                ContactNumber = userCheck.ContactNumber,
                ShowContactNumber = userCheck.ShowContactNumber,
                OrganizationID = userCheck.OrganizationID,
                LastUpdateTime = userCheck.LastUpdateTime.Ticks
            };

            foreach (var divion in userCheck?.Divisions)
                user.Divisions.Add(divion);

            realm.Write(() =>
            {
                realm.Add(user, true);
            });

            return user;
        }


        public static OrganizationModel UpdateOrganization(Realm realm, UserModel user, OrganizationReceivedModel result)
        {
            var organization = new OrganizationModel
            {
                OrganizationID = result.OrganizationID,
                Name = result.Name,
                Description = result.Description,
                Picture = result.Picture,
                ContactNumber = result.ContactNumber,
                Address = result.Address,
                Email = result.Email,
                LastUpdateTicks = result.LastUpdateTicks
            };

            foreach (var divion in result?.Divisions)
            {
                var userInDivision = user.Divisions.Where(x => x.DivisionID == divion.DivisionID).FirstOrDefault();

                if (userInDivision != null)
                {
                    divion.IsUserInDivisionActive = userInDivision.IsUserInDivisionActive;
                    divion.IsUnderReview = userInDivision.IsUnderReview;
                }

                organization.Divisions.Add(divion);
            }

            realm.Write(() =>
            {
                realm.Add(organization, true);
            });

            return organization;
        }

        public static string GetCallerName(string callerId)
        {
            var realm = Realm.GetInstance(new RealmConfiguration { ShouldDeleteIfMigrationNeeded = true });
            var members = realm.All<GetUsersInDivisionModel>();
            var caller = members.Where(x => x.UserId == Int32.Parse(callerId)).First();

            string fullname = string.Empty;

            if (caller != null)
                fullname = $"{caller?.FirstName} {caller?.LastName}";
            else
                fullname = L10N.Localize("Call_Anonym");

            return fullname;
        }
    }
}
