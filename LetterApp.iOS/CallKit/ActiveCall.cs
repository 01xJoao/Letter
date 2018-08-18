using System;
using CoreFoundation;
using Foundation;
using LetterApp.Core;
using LetterApp.Core.Helpers;
using LetterApp.Core.Services.Interfaces;
using LetterApp.Core.ViewModels;
using SimpleInjector;
using UIKit;

namespace LetterApp.iOS.CallKit
{
    public class ActiveCall
    {
        private IXNavigationService _navigationService;

        #region Private Variables
        public bool isConnecting;
        public bool isConnected;
        public bool isOnhold;
        #endregion

        #region Computed Properties
        public NSUuid UUID { get; set; }
        public bool isOutgoing { get; set; }
        public string Handle { get; set; }
        public int CallerId { get; set; }
        public DateTime StartedConnectingOn { get; set; }
        public DateTime ConnectedOn { get; set; }
        public DateTime EndedOn { get; set; }

        public bool IsConnecting
        {
            get { return isConnecting; }
            set
            {
                isConnecting = value;
                if (isConnecting) StartedConnectingOn = DateTime.Now;
                RaiseStartingConnectionChanged();
            }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                if (isConnected)
                {
                    ConnectedOn = DateTime.Now;
                }
                else
                {
                    EndedOn = DateTime.Now;
                }
                RaiseConnectedChanged();
            }
        }

        public bool IsOnHold
        {
            get { return isOnhold; }
            set
            {
                isOnhold = value;
            }
        }
        #endregion

        #region Constructors
        public ActiveCall(IXNavigationService navigationService) 
        {
            _navigationService = navigationService;
        }

        public ActiveCall(NSUuid uuid, string callerName, int callerId, bool outgoing)
        {
            this.UUID = uuid;
            this.Handle = callerName;
            this.CallerId = callerId;
            this.isOutgoing = outgoing;
        }
        #endregion

        #region Public Methods
        public void StartCall(ActiveCallbackDelegate completionHandler)
        {

        }

        public void AnswerCall(ActiveCallbackDelegate completionHandler)
        {
            // Simulate the call being answered
            IsConnected = true;
            //completionHandler(true);

            App.StartCall(CallerId);
        }

        public void EndCall(ActiveCallbackDelegate completionHandler)
        {
            // Simulate the call ending
            IsConnected = false;
            //completionHandler(true);
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