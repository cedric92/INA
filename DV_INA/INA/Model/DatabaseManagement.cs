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

        public bool addDataToTransactionCount(string value)
        {
            string[] record = splitString(value);
            
            SqlTransaction trans = null;

            // record[0] => fileid
            // record[1] => Header, Footer or Accountno
            // record[2] => sum or Amount 

            switch (record[1])
            {
                case "Header": // do nothing
                    break;
                case "Footer": // count auf db (spalte fileid)
     
                    // connect to database
                    using (_sqlconnection = new SqlConnection(conString))
                    {
                        try
                        {
                            // open connection
                            _sqlconnection.Open();
                            trans = _sqlconnection.BeginTransaction();


                            SqlCommand command = _sqlconnection.CreateCommand();
                            command.Connection = _sqlconnection;
                            command.Transaction = trans;

                            // begin transaction

                            command.CommandText = "SELECT * FROM AccMgmt WHERE Fileid=" + record[0] + ")";
                            int i = command.ExecuteNonQuery();
                            trans.Commit();

                            // alles da?
                            if(i != Convert.ToInt32(record[2])) {
                                return false;
                            }
   
                        }
                        catch (Exception)
                        {
                            return false;
                        }

                    }
                    break;

                default: // record with fileid, accno and amount

                    // connect to database
                    using (_sqlconnection = new SqlConnection(conString))
                    {
                        try
                        {
                            // open connection
                            _sqlconnection.Open();
                            trans = _sqlconnection.BeginTransaction();


                            SqlCommand command = _sqlconnection.CreateCommand();
                            command.Connection = _sqlconnection;
                            command.Transaction = trans;

                            // begin transaction

                            command.CommandText = "INSERT INTO AccMgmt (Account, Amount, Fileid) VALUES (" + record[1] + "," + record[2] + "," + record[0] + ")";
                            command.ExecuteNonQuery();

                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            return false;
                        }

                    }
                    break;
            }
            
            return true;

        }

        #endregion
    }
}
