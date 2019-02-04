using System;
namespace LetterApp.Core.Models
{
    public class ContactDialogFilter
    {
        public ContactDialogFilter(string title, bool isActive, string description)
        {
            Title = title;
            IsActive = isActive;
            Description = description;
        }

        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}
