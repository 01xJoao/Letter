﻿using System;
using System.Collections.Generic;
using System.Linq;
using CallKit;
using Foundation;
using SinchSdk;
using UIKit;

namespace LetterApp.iOS.CallKit
{
    public class ActiveCallManager
    {
        #region Private Variables
        private CXCallController CallController = new CXCallController();

        private ISINClient Client
        {
            get
            {
                var appDelgate = (AppDelegate)UIApplication.SharedApplication.WeakDelegate;
                return appDelgate.Client;
            }
        }
        #endregion

        #region Computed Properties

        public List<ActiveCall> Calls { get; set; }

        #endregion

        #region Constructors

        public ActiveCallManager()
        {
            this.Calls = new List<ActiveCall>();
        }

        #endregion

        #region Private Methods
        private void SendTransactionRequest(CXTransaction transaction)
        {
            CallController.RequestTransaction(transaction, (error) => 
            {
                if (error == null)
                {
                    Console.WriteLine("Transaction request sent successfully.");
                }
                else
                {
                    Console.WriteLine("Error requesting transaction: {0}", error);
                }
            });
        }
        #endregion

        #region Public Methods

        public ActiveCall FindCall(NSUuid uuid)
        {
            foreach (ActiveCall call in Calls)
            {
                if (call.UUID.ToString() == uuid.ToString()) 
                    return call;
            }
            return null;
        }

        public ActiveCall StartCall(string name, int id)
        {
            var call = Client.CallClient.CallUserWithId(id.ToString());
            var newCall = new ActiveCall(new NSUuid(), name, id, true, call);
            Calls.Add(newCall);

            var handle = new CXHandle(CXHandleType.Generic, name);
            var startCallAction = new CXStartCallAction(newCall.UUID, handle);
            var transaction = new CXTransaction(startCallAction);
            SendTransactionRequest(transaction);
            return newCall;
        }

        public void AnswerCall(ActiveCall call)
        {
            var answerCallAction = new CXAnswerCallAction(call.UUID);
            var transaction = new CXTransaction(answerCallAction);
            SendTransactionRequest(transaction);
        }

        public void EndCall(ActiveCall call)
        {
            if (call == null || call == default(ActiveCall))
                call = Calls.LastOrDefault();

            if (call != null)
            {
                var endCallAction = new CXEndCallAction(call.UUID);
                var transaction = new CXTransaction(endCallAction);
                SendTransactionRequest(transaction);
            }
        }

        public void PlaceCallOnHold(ActiveCall call)
        {
            var holdCallAction = new CXSetHeldCallAction(call.UUID, true);
            var transaction = new CXTransaction(holdCallAction);
            SendTransactionRequest(transaction);
        }

        public void RemoveCallFromOnHold(ActiveCall call)
        {
            var holdCallAction = new CXSetHeldCallAction(call.UUID, false);
            var transaction = new CXTransaction(holdCallAction);
            SendTransactionRequest(transaction);
        }

        public void MuteCall(ActiveCall call, bool mute)
        {
            var holdCallAction = new CXSetMutedCallAction(call.UUID, mute);
            var transaction = new CXTransaction(holdCallAction);
            SendTransactionRequest(transaction);
        }

        #endregion
    }
}