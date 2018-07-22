using System;
using System.Collections.Generic;

namespace LetterApp.Core.Models
{
    public class ProfileOrganizationModel
    {
        public ProfileOrganizationModel(string divisionDescriptionLabel, List<ProfileOrganizationDetails> organizationDivisions)
        {
            DivisionDescriptionLabel = divisionDescriptionLabel;
            OrganizationDivisions = organizationDivisions;
        }

        public string DivisionDescriptionLabel { get; set; }
        public List<ProfileOrganizationDetails> OrganizationDivisions { get; set; }
    }

    public class ProfileOrganizationDetails
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public string MembersCount { get; set; }
    }
}
