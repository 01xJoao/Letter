using System;
using Foundation;
using SinchSdk;

namespace LetterApp.iOS.CallKit
{
    public class ActiveCall
    {
        #region Private Variables
        public bool isConnecting;
        public bool isConnected;
        public bool isOnhold;

        private bool ended;
        #endregion

        #region Computed Properties

        public NSUuid UUID { get; set; }
        public bool IsOutgoing { get; set; }
        public string Handle { get; set; }
        public int CallerId { get; set; }
        public DateTime StartedConnectingOn { get; set; }
        public DateTime ConnectedOn { get; set; }
        public DateTime EndedOn { get; set; }
        public ISINCall SINCall { get; set; }

        public bool IsConnecting
        {
            get => isConnecting;
            set
            {
                isConnecting = value;

                if (isConnecting) 
                    StartedConnectingOn = DateTime.Now;
                
                RaiseStartingConnectionChanged();
            }
        }

        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;

                if (isConnected)
                    ConnectedOn = DateTime.Now;
                else
                    EndedOn = DateTime.Now;
                
                RaiseConnectedChanged();
            }
        }

        public bool IsOnHold
        {
            get => isOnhold;
            set => isOnhold = value;
        }

        public bool Ended
        {
            get => ended;
            set => ended = value;
        }

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
            isConnecting = false;
            IsConnected = true;
        }

        #endregion

        #region Events

        public delegate void ActiveCallbackDelegate(bool successful);
        public delegate void ActiveCallStateChangedDelegate(ActiveCall call);

        public event ActiveCallStateChangedDelegate StartingConnectionChanged;

        internal void RaiseStartingConnectionChanged()
        {
            if (this.StartingConnectionChanged != null) 
                this.StartingConnectionChanged(this);
        }

        public event ActiveCallStateChangedDelegate ConnectedChanged;

        internal void RaiseConnectedChanged()
        {
            if (this.ConnectedChanged != null) 
                this.ConnectedChanged(this);
        }

        #endregion
    }
}