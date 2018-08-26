using System;
using System.Collections.Generic;

namespace LetterApp.Core.Models
{
    public class CallHistoryModel
    {
        public int CallerId { get; set; }
        public string CallerInfo { get; set; }
        public string CallerPicture { get; set; }
        public DateTime CallDate { get; set; }
        public string CallDateText { get; set; }
        public CallType CallType { get; set; }
        public string CallCountAndType { get; set; }
        public bool HasSuccess { get; set; }
        public List<int> CallStack = new List<int>();
        public bool ShouldAlert { get; set; }
    }
}
