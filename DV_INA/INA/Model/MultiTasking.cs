using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Collections.Concurrent;
using System.Threading;

namespace INA.Model
{
    // serializable due to xml formatter
    [Serializable()]
    class MultiTasking:QueueManagement
    {
        DatabaseManagement _databasemanagement = new DatabaseManagement();

        MessageQueue queue = GetStringMessageQueue();

        public MultiTasking()
        {

        }

        public void startTasks()
        {
            // set the formatter to indicate body contains a string
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });

            // define eventhandler for msmq
           // queue.ReceiveCompleted += new ReceiveCompletedEventHandler(createDataQuery);

            
            //enumerable range + max degree of parallelism => define how many threads will be created
            Parallel.ForEach(Enumerable.Range(0, 10), new ParallelOptions { MaxDegreeOfParallelism = 4 }, (i) =>
            {
                // Restart the synchronous receive operation
                process();
   
            });
  
        }

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
                if (_databasemanagement.addDataToTransactionCount(myOrder))
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

    }
}
