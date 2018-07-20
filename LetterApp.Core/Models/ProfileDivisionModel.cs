using System;
using System.Collections.Generic;

namespace LetterApp.Core.Models
{
    public class ProfileDivisionModel
    {
        public ProfileDivisionModel(List<ProfileDivisionDetails> divisions,string description, EventHandler<int> divisionPressedEvent, EventHandler<int> addDivisionEvent)
        {
            Divisions = divisions;
            DivisionDescriptionLabel = description;
            DivisionPressedEvent = divisionPressedEvent;
            AddDivisionEvent = addDivisionEvent;

        }

        public string DivisionDescriptionLabel { get; set; }
        public List<ProfileDivisionDetails> Divisions { get; set; }
        public EventHandler<int> DivisionPressedEvent { get; set; }
        public EventHandler<int> AddDivisionEvent { get; set; }
    }

    public class ProfileDivisionDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool AddButtonImage { get; set; }
    }
}
