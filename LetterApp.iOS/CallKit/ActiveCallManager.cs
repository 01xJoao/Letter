using System;
using System.Collections.Generic;
using CallKit;
using Foundation;

namespace LetterApp.iOS.CallKit
{
    public class ActiveCallManager
    {
        #region Private Variables
        private CXCallController CallController = new CXCallController();
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
            var newCall = new ActiveCall(new NSUuid(), name, id, true, null);
            Calls.Add(newCall);

            var handle = new CXHandle(CXHandleType.Generic, name);
            var startCallAction = new CXStartCallAction(newCall.UUID, handle);
            var transaction = new CXTransaction(startCallAction);
            SendTransactionRequest(transaction);

            return newCall;
        }

        public void AnswerCall(ActiveCall call)
        {
            if (call != null)
            {
                var answerCallAction = new CXAnswerCallAction(call.UUID);
                var transaction = new CXTransaction(answerCallAction);
                SendTransactionRequest(transaction);
            }
        }

        public void EndCall(ActiveCall call)
        {
            if (call != null)
            {
                var endCallAction = new CXEndCallAction(call.UUID);
                var transaction = new CXTransaction(endCallAction);
                SendTransactionRequest(transaction);
            }
        }

        public void PlaceCallOnHold(ActiveCall call)
        {
            if (call != null)
            {
                var holdCallAction = new CXSetHeldCallAction(call.UUID, true);
                var transaction = new CXTransaction(holdCallAction);
                SendTransactionRequest(transaction);
            }
        }

        public void RemoveCallFromOnHold(ActiveCall call)
        {
            if (call != null)
            {
                var holdCallAction = new CXSetHeldCallAction(call.UUID, false);
                var transaction = new CXTransaction(holdCallAction);
                SendTransactionRequest(transaction);
            }
        }

        public void MuteCall(ActiveCall call, bool mute)
        {
            if (call != null)
            {
                var holdCallAction = new CXSetMutedCallAction(call.UUID, mute);
                var transaction = new CXTransaction(holdCallAction);
                SendTransactionRequest(transaction);
            }
        }

        #endregion
    }
}