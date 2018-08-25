using System;
namespace LetterApp.Core.Models
{
    public class CallHistoryModel
    {
        public int CallerId { get; set; }
        public string CallerInfo { get; set; }
        public string CallerPicture { get; set; }
        public DateTime CallDate { get; set; }
        public string CallDateText { get; set; }
        public string CallType { get; set; }
        public bool HasSuccess { get; set; }
        public int NumberOfCalls { get; set; }
        public bool IsNew { get; set; }
    }
}
