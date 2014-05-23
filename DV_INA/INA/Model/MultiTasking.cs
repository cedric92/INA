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
            startTasks();
        }

        public void startTasks()
        {
            // set the formatter to indicate body contains a string
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });

            // define eventhandler for msmq
            queue.ReceiveCompleted += new ReceiveCompletedEventHandler(createDataQuery);


            //enumerable range + max degree of parallelism => define how many threads will be created
            Parallel.ForEach(Enumerable.Range(0, 10), new ParallelOptions { MaxDegreeOfParallelism = 10 }, (i) =>
            {
                // Restart the asynchronous receive operation
                queue.BeginReceive();
            });

            // start listening
           //  queue.BeginReceive();
        }

        private void createDataQuery(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            Message msg = queue.EndReceive(asyncResult.AsyncResult);

            //do work
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });

          //  Console.WriteLine(msg.Body.ToString());
           // string s = msg.Body.ToString();

            //todoooooooooooooooooooooooooooooo//

            _databasemanagement.addDataToTransactionCount(msg.Body.ToString());

            listen(this.queue);
        }

        private void listen(MessageQueue queue)
        {
            try
            {
                queue.BeginReceive();
            }
            catch (MessageQueueException)
            {

            }
        }
    }
}
