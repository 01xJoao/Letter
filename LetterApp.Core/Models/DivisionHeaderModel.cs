using System;
using LetterApp.Core.Helpers.Commands;

namespace LetterApp.Core.Models
{
    public class DivisionHeaderModel
    {
        public DivisionHeaderModel(string name, string membersCount, string description, string picture, EventHandler backEvent)
        {
            Name = name;
            MembersCount = membersCount;
            Description = description;
            Picture = picture;
            BackEvent = backEvent;
        }

        public string Name { get; set; }
        public string MembersCount { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public EventHandler BackEvent { get; set; }
    }

    public class OrganizationInfoModel
    {
        public OrganizationInfoModel(string name, string picture, string section, XPCommand openOrganizationCommand)
        {
            Section = section;
            Name = name;
            Picture = picture;
            OpenOrganizationCommand = openOrganizationCommand;
        }

        public string Section { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public XPCommand OpenOrganizationCommand { get; set; }
    }
}
