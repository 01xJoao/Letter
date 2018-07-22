using System;
namespace LetterApp.Core.Models.Generic
{
    public class DescriptionTypeEventModel
    {
        public DescriptionTypeEventModel(string description, bool hasView, EventHandler<CellType> typeEvent, CellType type)
        {
            Description = description;
            HasView = hasView;
            TypeEvent = typeEvent;
            Type = type;
        }

        public string Description { get; set; }
        public bool HasView { get; set; }
        public EventHandler<CellType> TypeEvent { get; set; }
        public CellType Type { get; set; }
    }

    public enum CellType
    {
        ContactUs,
        TermsOfService,
        CreateOrganization,
        SignOut,
        LeaveDivision,
        LeaveOrganization,
        DeleteAccount,
        Password
    }
}
