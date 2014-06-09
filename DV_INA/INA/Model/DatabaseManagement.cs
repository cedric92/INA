using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace INA.Model
{
    class DatabaseManagement : QueueManagement
    {
        #region Members
        SqlConnection _sqlconnection = null;
        LogFile _LogFile;
        string conString = @"Server=WINJ5GTVAPSLQX\SQLEXPRESS;Database=INA;Trusted_Connection=True;";
        #endregion

        #region Constructor

        public DatabaseManagement(LogFile f)
        {
            // start new connection with constring
            _sqlconnection = new SqlConnection(conString);
            this._LogFile = f;
        }

        #endregion

        #region Methods

        //returns an array with 3 entries
        //each array represents a single message from the file
        // array[0] => file-id
        // array[1] => Header, Footer or Accountno
        // array[2] => sum or Amount 

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

        //evaluate each message
        //header => back to queue
        //footer => check if all message are in database, otherwise send it back to queue
        //message => write to database
        public bool evaluateMessageLine(string value)
        {
            bool success = true;
            //string value => single message from file

            string[] record = splitString(value);

            // record[0] => file-id
            // record[1] => Header, Footer or Accountno
            // record[2] => sum or Amount 

            switch (record[1])
            {
                //message is a header
                case "Header": // do nothing
                    break;
                //message is a footer
                case "Footer":
                    //success = evaluateFooter(record);
                    break;
                //message is no footer/header
                default:
                   // success = evaluateMessage(record);
                    break;
            }
            return success;

        }

        private bool evaluateFooter(string[] record)
        {
            SqlTransaction trans;
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
                    if (i != Convert.ToInt32(record[2]))
                    {
                       
                        return false;
                    }
                    //everything worked => return true
                    this._LogFile.writeToFile("Footer check ok with ID = " + record[0]);
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        private bool evaluateMessage(string[] record)
        {
            SqlTransaction trans;
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

                    //everything ok: return true
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        #endregion
    }
}
