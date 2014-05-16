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

        public MultiTasking()
        {
            startTasks();
        }

        //Todo: while(true) läuft immer weiter => abbruch

        public void startTasks()
        {
            string queueName = @".\private$\MyStringQueue";

            while (true)
            {
                int msgCount = 0;

                Parallel.ForEach(Enumerable.Range(0, 20), (i) =>
                {
                    MessageQueue queue = new MessageQueue(queueName);
                    // set the formatter to indicate body contains a string
                    queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(String) });
                    Message msg = new Message();
                    try
                    {
                        msg = queue.Receive(TimeSpan.Zero);

                        // do work
                        Console.WriteLine(msg.Body.ToString());

                        Interlocked.Increment(ref msgCount);
                    }
                    catch (MessageQueueException mqex)
                    {
                        if (mqex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                        {
                            return; // nothing in queue
                        }
                        else throw;
                    }
                });

            }
        }
    }
}