using System;
namespace LetterApp.Core.Models.Generic
{
    public class DescriptionAndBoolEventModel
    {
        public DescriptionAndBoolEventModel(string description, bool isActive, EventHandler<bool> booleanEvent)
        {
            Description = description;
            BooleanEvent = booleanEvent;
            IsActive = isActive;
        }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public EventHandler<bool> BooleanEvent { get; set; }
    }
}
