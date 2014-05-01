using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Messaging;
using System.Windows;

namespace INA.Model
{
    // serializable wg xml formatter
    [Serializable()]
    class QueueManagement
    {
        #region Members

        // generate new queue
        public Queue<string> queue = null;

        #endregion

        public QueueManagement()
        {
            this.queue = new Queue<string>();
        }

        internal MultiThreading MultiThreading
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        // start 
        public void startMessageQueue(List<KeyValuePair<string, string>> transactions)
        {
            SendStringMessageToQueue(transactions);
        }

        // create queue + queue name
        private static MessageQueue GetStringMessageQueue()
        {
            MessageQueue msgQueue = null;
            string queueName = @".\private$\MyStringQueue";

            if (!MessageQueue.Exists(queueName))
            {
                msgQueue = MessageQueue.Create(queueName);
            }
            else
            {
                msgQueue = new MessageQueue(queueName);
            }
            return msgQueue;
        }

        // send messages to queue
        private static void SendStringMessageToQueue(List<KeyValuePair<string, string>> transactions)
        {

            MessageQueue msgQueue = GetStringMessageQueue();

            // serialize the message while sending
           msgQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });


           string sendtoqueue = null;

            for (int i = 0; i < transactions.Count; i++)
            {
                sendtoqueue = transactions.ElementAt(i).ToString();
                // a transaction type used for Microsoft Transaction Server (MTS) it will be used when sending or receiving the message
                msgQueue.Send(sendtoqueue, MessageQueueTransactionType.Automatic);
            }

            ReadStringMessageFromQueue();

        }

        // read messages from queue
        private static void ReadStringMessageFromQueue()
        {
            // connect to the a queue on the local computer
            MessageQueue msgQueue = GetStringMessageQueue();

            // set the formatter to indicate body contains a string
            msgQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });

            try
            {
                // get messages from queue
                Message[] msgs = msgQueue.GetAllMessages();

                foreach (Message message in msgs)
                {
                    // write messages on console
                    Console.WriteLine(message.Body);
                }
                // clear queue
                msgQueue.Purge();
           }

            catch (MessageQueueException)
            {
                // Handle Message Queuing exceptions.
            }

            // handle invalid serialization format. 
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            // Catch other exceptions as necessary. 

            return;
        }

    }
}
