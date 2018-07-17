using System;
namespace LetterApp.Core.Models.Generic
{
    public class DescriptionTypeEventModel
    {
        public DescriptionTypeEventModel(string description, EventHandler<Type> typeEvent)
        {
            Description = description;
            TypeEvent = typeEvent;
        }

        public string Description { get; set; }
        public EventHandler<Type> TypeEvent { get; set; }
    }

    public enum Type 
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
