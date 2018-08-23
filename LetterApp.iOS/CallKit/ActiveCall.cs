using System;
using Foundation;
using SinchSdk;

namespace LetterApp.iOS.CallKit
{
    public class ActiveCall
    {
        #region Computed Properties

        public NSUuid UUID { get; set; }
        public bool IsOutgoing { get; set; }
        public string Handle { get; set; }
        public int CallerId { get; set; }
        public ISINCall SINCall { get; set; }
        public bool IsConnecting { get; set; }
        public bool IsConnected { get; set; }
        public bool IsOnHold { get; set; }
        public bool Ended { get; set; }

        #endregion

        #region Constructors

        public ActiveCall(NSUuid uuid, string callerName, int callerId, bool outgoing, ISINCall call)
        {
            this.UUID = uuid;
            this.Handle = callerName;
            this.CallerId = callerId;
            this.IsOutgoing = outgoing;
            this.SINCall = call;
        }

        #endregion

        #region Public Methods

        public void StartCall() 
        {
            IsConnecting = true;
        }

        public void AnswerCall()
        {
            IsConnected = true;
        }

        public void EndCall()
        {
            Ended = true;
        }

        #endregion
    }
}