using System;
using System.Collections.Generic;

namespace LetterApp.Core.Models
{
    public class ProfileDetailsModel
    {
        public ProfileDetailsModel(List<ProfileDetail> details)
        {
            Details = details;
        }

        public List<ProfileDetail> Details { get; set; }
    }

    public class ProfileDetail 
    {
        public string Description { get; set; }
        public string Value { get; set; }
    }
}
