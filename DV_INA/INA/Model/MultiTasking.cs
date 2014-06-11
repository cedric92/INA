using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;

namespace INA.Model
{
    // serializable due to xml formatter
    [Serializable()]
    class MultiTasking : QueueManagement
    {
        #region Member
        DatabaseManagement _databasemanagement;

        MessageQueue queue = GetStringMessageQueue();
        LogFile _Logfile;
        #endregion


        public MultiTasking(LogFile f, DatabaseManagement db)
        {
            this._Logfile = f;
            _databasemanagement = db;
        }
        #region Methods
        public void startTasks()
        {
            // set the formatter to indicate body contains a string
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });

            // define eventhandler for msmq
            // queue.ReceiveCompleted += new ReceiveCompletedEventHandler(createDataQuery);

            Task.Factory.StartNew(() =>

                 Parallel.ForEach(Enumerable.Range(0, 10), new ParallelOptions { MaxDegreeOfParallelism = 4 }, (i) =>
           {
               // Restart the synchronous receive operation
               process();

           })
                );
            //enumerable range + max degree of parallelism => define how many threads will be created

            
            _Logfile.writeToFile("Successfully importet into DB!! Everything completed.");
        }

        //used by each task
        //handle a single line, after completing the handling, execute next iteration(recursive) and handle next line
        private void process()
        {
            // Create a transaction.
            MessageQueueTransaction myTransaction = new MessageQueueTransaction();

            try
            {
                // Begin the transaction.
                myTransaction.Begin();

                // Receive the message. 
                Message myMessage = queue.Receive(myTransaction);
                String myOrder = (String)myMessage.Body;

                // Display message information.
                if (_databasemanagement.evaluateMessageLine(myOrder))
                {
                    // Commit the transaction.
                    myTransaction.Commit();
                }
                else
                {
                    // Abort the transaction.
                    myTransaction.Abort();
                }
            }

            catch (MessageQueueException e)
            {
                // Handle nontransactional queues. 
                if (e.MessageQueueErrorCode ==
                    MessageQueueErrorCode.TransactionUsage)
                {
                    Console.WriteLine("Queue is not transactional.");
                }

                // Roll back the transaction.
                myTransaction.Abort();
            }
            // rekursiv
            process();

        }

        #endregion


    }
}
