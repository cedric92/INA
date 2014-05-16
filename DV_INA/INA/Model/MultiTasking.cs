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
    class MultiTasking
    {
        MessageQueue queue = QueueManagement.GetStringMessageQueue();

        public MultiTasking()
        {
            startTasks();
        }

        public void startTasks()
        {

            // set the formatter to indicate body contains a string
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });

            // define eventhandler for msmq
            queue.ReceiveCompleted += new ReceiveCompletedEventHandler(myReceiveCompleted);
            // start listening
           queue.BeginReceive();     
        }

        // eventhandler (there are new messages)
        private void myReceiveCompleted(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            try
            {
                // get new messages from queue
                Message msg = queue.EndReceive(asyncResult.AsyncResult);

                // Process each message on a separate thread
                // This will immediately queue all items on the threadpool,
                // so there may be more threads spawned than you really want
                // Change how many items are allowed to process concurrently using ThreadPool.SetMaxThreads()
                System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(processQueueMessage), msg);
              
                // Restart the asynchronous receive operation
                queue.BeginReceive();
            }
            catch (MessageQueueException)
            {
                // Handle sources of MessageQueueException.
            }

        }

        // one thread processes one message
        private void processQueueMessage(object message)
        {
            Message msg = (Message)message;

            try
            {
                Console.WriteLine(msg.Body.ToString());
            }
            catch (MessageQueueException mqex)
            {
                if (mqex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                {
                    return; // nothing in queue
                }
                else throw;
            }
        }


    }
}