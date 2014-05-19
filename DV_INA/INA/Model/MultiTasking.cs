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
        string queueName = @".\private$\MyStringQueue";
        MessageQueue queue;
        

        public MultiTasking()
        {
            queue = new MessageQueue(queueName);
            startTasks();
        }

        //Todo: while(true) läuft immer weiter => abbruch

        public void startTasks()
        {
            
            // set the formatter to indicate body contains a string
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });

            queue.ReceiveCompleted += new ReceiveCompletedEventHandler(createDataQuery);

            Parallel.ForEach(Enumerable.Range(0, 10), (i) =>
            {
                listen(queue);
            });
        }
        private void listen(MessageQueue queue)
        {
            try
            { 
                queue.BeginReceive();
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

        private void createDataQuery(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            string queueName = @".\private$\MyStringQueue";
            MessageQueue queue = new MessageQueue(queueName);

            Message msg = queue.EndReceive(asyncResult.AsyncResult);

            //do work

            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });
            Console.WriteLine(msg.Body.ToString());

            listen(this.queue);
        }
    }
}
