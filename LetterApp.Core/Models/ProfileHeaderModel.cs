using System;
namespace LetterApp.Core.Models
{
    public class ProfileHeaderModel
    {
        public ProfileHeaderModel(string name, string description, string picture,EventHandler openGalery, EventHandler<string> updateDescriptionEvent, EventHandler openSettingsEvent)
        {
            Name = name;
            Description = description;
            Picture = picture;
            OpenGalery = openGalery;
            UpdateDescriptionEvent = updateDescriptionEvent;
            OpenSettingsEvent = openSettingsEvent;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public EventHandler OpenGalery { get; set; }
        public EventHandler<string> UpdateDescriptionEvent { get; set; }
        public EventHandler OpenSettingsEvent { get; set; }
    }
}
