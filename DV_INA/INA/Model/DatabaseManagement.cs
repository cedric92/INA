using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace INA.Model
{
    class DatabaseManagement : QueueManagement
    {
        SqlConnection _sqlconnection = null;
        string conString = @"Server=WINJ5GTVAPSLQX\SQLEXPRESS;Database=INA;Trusted_Connection=True;";

        //foreach file a key value pair exists. the first int defines the file id, the second int 
        //counts the already datasets => compare to footer-counter
        List<KeyValuePair<int, int>> transactionCount = new List<KeyValuePair<int, int>>();
        List<KeyValuePair<int, SqlTransaction>> Transactions = new List<KeyValuePair<int, SqlTransaction>>();

        public DatabaseManagement()
        {
            _sqlconnection = new SqlConnection(conString);
        }

        public void addToTransaction()
        {

        }

        public void addDataToTransactionCount(string value)
        {
            string tmp = value;
            tmp=tmp.Remove(0, 1);
            tmp = tmp.Remove(tmp.Length-1, 1);
            int ind = tmp.IndexOf(',');
            tmp = tmp.Remove(ind, 1);
            string[] abc = tmp.Split(' ');

            switch (abc[1])
            {
                case "Header": // O.o

                    break;
                case "Footer":
                    int transValue = 0;
                    foreach (var item in transactionCount)
                    {
                        if (item.Key == Convert.ToInt32(abc[0]))
                        {
                            transValue = item.Value;
                        }
                    }

                    if (transValue == Convert.ToInt32(abc[2]))
                    {
                        foreach (var item in Transactions)
                        {
                            if (item.Key == Convert.ToInt32(abc[0]))
                            {
                                item.Value.Commit();
                                Console.WriteLine("Commit file with id " + abc[0]);
                            }
                            else
                            {
                                //
                                SendStringMessageToQueue(value);
                            }
                        }
                    }

                    break;

                default:
                    generateTransaction(Convert.ToInt32(abc[0]));

                    using (_sqlconnection=new SqlConnection(conString))
                    {
                        _sqlconnection.Open();

                        foreach (var item in Transactions)
                        {
                            if (item.Key == Convert.ToInt32(abc[0]))
                            {
                                SqlTransaction trans = item.Value;
                                trans = _sqlconnection.BeginTransaction();


                                SqlCommand command = _sqlconnection.CreateCommand();
                                command.Connection = _sqlconnection;
                                command.Transaction = trans;

                                try
                                {
                                    command.CommandText = "INSERT INTO AccMgmt (Account, Amount) VALUES ("+abc[1]+","+abc[0]+")";
                                    command.ExecuteNonQuery();      
                             
                                    
                                }
                                catch (Exception)
                                {
                                    
                                    //todo: transaction rollback definieren

                                    throw;
                                }

                                var newEntry = new KeyValuePair<int, SqlTransaction>(item.Key, trans);
                                Transactions[item.Key] = newEntry;
                            }
                        }
                    }
                    break;
            }

        }

        private void generateTransaction(int fileID)
        {
            foreach (var item in transactionCount)
            {

                //if file id already exists => increment total number of lines
                if (item.Key == fileID)
                {
                    int oldvalue = item.Value;
                    var newEntry = new KeyValuePair<int,int>(fileID, oldvalue++);                  

                    transactionCount[fileID] = newEntry;
                }
                // if file id doesnt exist => create new entry
                else
                {
                    transactionCount.Add(new KeyValuePair<int, int>(fileID,1));
                    createTransaktion(fileID);
                }
            }
        }

        private void createTransaktion(int fileID)
        {
            Transactions.Add(new KeyValuePair<int, SqlTransaction>(fileID, _sqlconnection.BeginTransaction()));
        }
    }
}
