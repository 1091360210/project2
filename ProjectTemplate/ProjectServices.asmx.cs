﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;


namespace ProjectTemplate
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]


    public class ProjectServices : System.Web.Services.WebService
    {
        ////////////////////////////////////////////////////////////////////////
        ///replace the values of these variables with your database credentials
        ////////////////////////////////////////////////////////////////////////
        private string dbID = "group5-2";
        private string dbPass = "!!Group5";
        private string dbName = "group5-2";

        public string sessionUsername = "";

        ///call this method anywhere that you need the connection string!
        private string getConString()
        {
            return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + dbID + "; PASSWORD=" + dbPass + "; Convert Zero Datetime=True;";
        }
        ////////////////////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////////////////////
        //don't forget to include this decoration above each method that you want
        //to be exposed as a web service!
        [WebMethod(EnableSession = true)]
        /////////////////////////////////////////////////////////////////////////
        public string TestConnection()
        {
            try
            {
                string testQuery = "select * from test";

                ////////////////////////////////////////////////////////////////////////
                ///here's an example of using the getConString method!
                ////////////////////////////////////////////////////////////////////////
                MySqlConnection con = new MySqlConnection(getConString());
                ////////////////////////////////////////////////////////////////////////

                MySqlCommand cmd = new MySqlCommand(testQuery, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return "Success!";
            }
            catch (Exception e)
            {
                return "Something went wrong, please check your credentials and db name and try again.  Error: " + e.Message;
            }
        }



        //EXAMPLE OF A SIMPLE SELECT QUERY (PARAMETERS PASSED IN FROM CLIENT)
        [WebMethod(EnableSession = true)]
        public bool LogOn(string uid, string pass)
        {

            bool success = false;

            string sqlSelect = "SELECT UserName, Password FROM Users WHERE UserName = '" + uid + "' AND Password = '" + pass + "'";

            MySqlConnection con = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, con);

            //tell our command to replace the @parameters with real values
            //we decode them because they came to us via the web so they were encoded
            //for transmission (funky characters escaped, mostly)

            //a data adapter acts like a bridge between our command object and 
            //the data we are trying to get back and put in a table object
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //here's the table we want to fill with the results from our query
            DataTable sqlDt = new DataTable();
            //here we go filling it!
            sqlDa.Fill(sqlDt);
            //check to see if any rows were returned.  If they were, it means it's 
            //a legit account
            if (sqlDt.Rows.Count > 0)
            {
                //flip our flag to true so we return a value that lets them know they're logged in
                success = true;
                sessionUsername = uid;
            }
            //return the result!
            return success;
        }


        [WebMethod(EnableSession = true)]
        public bool NAccount(string fName, string lName, string email, string uName, string pwd, string supUse)
        {
            bool success = false;

            string sqlSelect = "SELECT UserName, Password FROM Users WHERE UserName = '" + uName + "'";

            MySqlConnection con = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, con);

            //tell our command to replace the @parameters with real values
            //we decode them because they came to us via the web so they were encoded
            //for transmission (funky characters escaped, mostly)

            //a data adapter acts like a bridge between our command object and 
            //the data we are trying to get back and put in a table object
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //here's the table we want to fill with the results from our query
            DataTable sqlDt = new DataTable();
            //here we go filling it!
            sqlDa.Fill(sqlDt);
            //check to see if any rows were returned.  If they were, it means it's 
            //a legit account
            if (sqlDt.Rows.Count > 0)
            {
                //flip our flag to true so we return a value that lets them know they're logged in
                success = false;
            }
            //return the result!


            else
            {
                string sqlInsert = "INSERT INTO Users(`UID`,`FirstName`,`LastName`,`Email`,`UserName`,`Password`,`SuperUser`) Values(default,'" + fName + "','"
                    + lName + "','" + email + "','" + uName + " ','" + pwd + " ','" + supUse +  "')";

                MySqlConnection con2 = new MySqlConnection(getConString());
                MySqlCommand sqlCommand2 = new MySqlCommand(sqlInsert, con2);
                //a data adapter acts like a bridge between our command object and 
                //the data we are trying to get back and put in a table object
                con2.Open();
                sqlCommand2.ExecuteNonQuery();
                con2.Close();
                success = true;

            }
            //return the result!
            return success;
        }

       




    }
}
            


        
       
