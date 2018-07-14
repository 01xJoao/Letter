using System;
using System.Collections.Generic;

namespace LetterApp.Core.Models
{
    public class ProfileDivisionModel
    {
        public ProfileDivisionModel(List<ProfileDivision> divisions, EventHandler<int> divisionPressedEvent, EventHandler<int> addDivisionEvent)
        {
            Divisions = divisions;
            DivisionPressedEvent = divisionPressedEvent;
            AddDivisionEvent = addDivisionEvent;
        }

        public List<ProfileDivision> Divisions { get; set; }
        public EventHandler<int> DivisionPressedEvent { get; set; }
        public EventHandler<int> AddDivisionEvent { get; set; }
    }

    public class ProfileDivision
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool AddButtonImage { get; set; }
    }
}
