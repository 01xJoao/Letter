using System;
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
            // Initialize
            this.Calls = new List<ActiveCall>();
        }
        #endregion

        #region Private Methods
        private void SendTransactionRequest(CXTransaction transaction)
        {
            // Send request to call controller
            CallController.RequestTransaction(transaction, (error) => {
                // Was there an error?
                if (error == null)
                {
                    // No, report success
                    Console.WriteLine("Transaction request sent successfully.");
                }
                else
                {
                    // Yes, report error
                    Console.WriteLine("Error requesting transaction: {0}", error);
                }
            });
        }
        #endregion

        #region Public Methods
        public ActiveCall FindCall(NSUuid uuid)
        {
            // Scan for requested call
            foreach (ActiveCall call in Calls)
            {
                if (call.UUID.ToString() == uuid.ToString()) 
                    return call;
            }

            // Not found
            return null;
        }

        public ActiveCall StartCall(string name, int id)
        {
            var call = Client.CallClient.CallUserWithId(id.ToString());

            // Build call action
            var handle = new CXHandle(CXHandleType.Generic, name);

            var newCall = new ActiveCall(new NSUuid(), name, id, true, call);
            Calls.Add(newCall);

            var startCallAction = new CXStartCallAction(newCall.UUID, handle);

            // Create transaction
            var transaction = new CXTransaction(startCallAction);

            // Inform system of call request
            SendTransactionRequest(transaction);

            return newCall;
        }

        public void EndCall(ActiveCall call)
        {
            //var activeCall = Calls.Find(x => x.IsOnHold == false);

            if (call == null || call == default(ActiveCall))
                call = Calls.LastOrDefault();

            if (call != null)
            {
                // Build action
                var endCallAction = new CXEndCallAction(call.UUID);

                // Create transaction
                var transaction = new CXTransaction(endCallAction);

                // Inform system of call request
                SendTransactionRequest(transaction);
            }
        }

        public void PlaceCallOnHold(ActiveCall call)
        {
            // Build action
            var holdCallAction = new CXSetHeldCallAction(call.UUID, true);

            // Create transaction
            var transaction = new CXTransaction(holdCallAction);

            // Inform system of call request
            SendTransactionRequest(transaction);
        }

        public void RemoveCallFromOnHold(ActiveCall call)
        {
            // Build action
            var holdCallAction = new CXSetHeldCallAction(call.UUID, false);

            // Create transaction
            var transaction = new CXTransaction(holdCallAction);

            // Inform system of call request
            SendTransactionRequest(transaction);
        }
        #endregion
    }
}