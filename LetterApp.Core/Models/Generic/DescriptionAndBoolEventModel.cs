using System;
namespace LetterApp.Core.Models.Generic
{
    public class DescriptionAndBoolEventModel
    {
        public DescriptionAndBoolEventModel(string description, EventHandler<bool> booleanEvent)
        {
            Description = description;
            BooleanEvent = booleanEvent;
        }

        public string Description { get; set; }
        public EventHandler<bool> BooleanEvent { get; set; }
    }
}
