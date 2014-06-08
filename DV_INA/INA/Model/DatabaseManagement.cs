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
        #region Members
            SqlConnection _sqlconnection = null;
            string conString = @"Server=WINJ5GTVAPSLQX\SQLEXPRESS;Database=INA;Trusted_Connection=True;";

            //foreach file a key value pair exists. the first int defines the file id, the second int 
            //counts the already transacted datasets => compare to footer-counter
            List<KeyValuePair<int, int>> transactionCount = new List<KeyValuePair<int, int>>();
            // list with a transaction for each fileid
            List<KeyValuePair<int, SqlTransaction>> Transactions = new List<KeyValuePair<int, SqlTransaction>>();

        #endregion

        #region Constructor

            public DatabaseManagement()
            {
                // start new connection with constring
                _sqlconnection = new SqlConnection(conString);
            }
        
        #endregion

        #region Methods

            public string[] splitString(string value)
            {
                // split received string from queue
                string tmp = value;
                tmp = tmp.Remove(0, 1);
                tmp = tmp.Remove(tmp.Length - 1, 1);
                int ind = tmp.IndexOf(',');
                tmp = tmp.Remove(ind, 1);
                string[] s = tmp.Split(' ');

                return s;
            }

            public void addDataToTransactionCount(string value)
            {
                string[] record = splitString(value);

                switch (record[1])
                {
                    case "Header": // do nothing
                        break;
                    case "Footer": // compare transacted lines with number of lines
                        int transValue = 0;
                        // search correct listindex by delivered fileid, get counted lines
                        // transactioncount: file id | counted lines
                        foreach (var item in transactionCount)
                        {
                            if (item.Key == Convert.ToInt32(record[0]))
                            {
                                transValue = item.Value;
                            }
                        }
                        // compare counted lines with number of lines from footer
                        if (transValue == Convert.ToInt32(record[2]))
                        {
                            foreach (var item in Transactions)
                            {
                                if (item.Key == Convert.ToInt32(record[0]))
                                {
                                    // everythings ok - commit this file
                                    item.Value.Commit();
                                    Console.WriteLine("Commit file with id " + record[0]);
                                }
                                else
                                {
                                    // return footer to queue
                                    SendStringMessageToQueue(value);
                                }
                            }
                        }

                        break;

                    default: // record with fileid, accno and amount
                        // generate transaction if necessary, count record
                        generateTransaction(Convert.ToInt32(record[0]));

                        // connect to database
                        using (_sqlconnection=new SqlConnection(conString))
                        {
                            // open connection
                            _sqlconnection.Open();

                            // lookup transaction for this fileid
                            foreach (var item in Transactions)
                            {
                                if (item.Key == Convert.ToInt32(record[0]))
                                {
                                    SqlTransaction trans = item.Value;
                                    trans = _sqlconnection.BeginTransaction();

                                    SqlCommand command = _sqlconnection.CreateCommand();
                                    command.Connection = _sqlconnection;
                                    command.Transaction = trans;

                                    // begin transaction
                                    try
                                    {
                                        command.CommandText = "INSERT INTO AccMgmt (Account, Amount) VALUES (" + record[1] + "," + record[0] + ")";
                                        command.ExecuteNonQuery();      
                                    }
                                    catch (Exception)
                                    {                     
                                        //todo: transaction rollback definieren
                                        // wenn hier rollback alle offenen transaktionen des files gelöscht
                                        // was wenn noch weitere kommen?? unterbinden!
                                        //trans.Rollback();

                                        throw;
                                    }

                                    // overwrite old transaction in list
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
     
        #endregion
    }
}
