using System;
using System.Collections.Generic;

namespace LetterApp.Core.Models
{
    public class ProfileOrganizationModel
    {
        public ProfileOrganizationModel(string divisionDescriptionLabel, List<ProfileOrganizationDetails> organizations)
        {
            DivisionDescriptionLabel = divisionDescriptionLabel;
            Organizations = organizations;
        }

        public string DivisionDescriptionLabel { get; set; }
        public List<ProfileOrganizationDetails> Organizations { get; set; }
    }

    public class ProfileOrganizationDetails
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public string MembersCount { get; set; }
    }
}
