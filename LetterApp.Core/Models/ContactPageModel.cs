using System;
using System.Collections.Generic;
using LetterApp.Models.DTO.ReceivedModels;

namespace LetterApp.Core.Models
{
    public class ContactListsModel
    {
        public List<List<GetUsersInDivisionModel>> Contacts { get; set; }
    }

    public class ContactTabModel 
    {
        public ContactTabModel(bool isSelected, string divisionName, int divisionId, int divisionIndex, EventHandler<int> divisionEvent)
        {
            IsSelected = isSelected;
            DivisionName = divisionName;
            DivisionId = divisionId;
            DivisionIndex = divisionIndex;
            DivisionEvent = divisionEvent;
        }

        public bool IsSelected { get; set; }
        public string DivisionName { get; set; }
        public int DivisionId { get; set; }
        public int DivisionIndex { get; set; }
        public EventHandler<int> DivisionEvent { get; set; }
    }
}
