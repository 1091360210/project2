using System;
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



        [WebMethod(EnableSession = true)]
        public Profile[] getProfiles() {
            DataTable sqlDt = new DataTable("profile");

            string sqlSelect = "select PID, description, company_name, education, profile_pic, UID from Profiles order by PID";

            MySqlConnection con = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, con);

            //gonna use this to fill a data table
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //filling the data table
            sqlDa.Fill(sqlDt);

            //loop through each row in the dataset, creating instances
            //of our container class Account.  Fill each acciount with
            //data from the rows, then dump them in a list.
            List<Profile> profiles = new List<Profile>();
            for (int i = 0; i < sqlDt.Rows.Count; i++)
            {
                profiles.Add(new Profile
                {  
                    pid = Convert.ToInt32(sqlDt.Rows[i]["PID"]),
                    companyName = sqlDt.Rows[i]["company_name"].ToString(),
                    education = sqlDt.Rows[i]["education"].ToString(),
                    picture = sqlDt.Rows[i]["profile_pic"].ToString(),
                    description = sqlDt.Rows[i]["description"].ToString(),
                    uid = Convert.ToInt32(sqlDt.Rows[i]["UID"])
                });
            }
            //convert the list of accounts to an array and return!
            
        

            return profiles.ToArray();
        }

        [WebMethod(EnableSession = true)]
        public Int32 GetUserId(string uName)
        {
            Int32 uid = 0;
            string sqlSelect = "SELECT UID FROM Users Where UserName ='" + uName + "'";
            /*WHERE UserName = '" + uName + "'"*/

            MySqlConnection con = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, con);
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);
            if (sqlDt.Rows.Count > 0)
            {
                uid = Convert.ToInt32(sqlDt.Rows[0]["UID"]);
            }
            return uid;

        }


		[WebMethod(EnableSession = true)]
		public bool updateProfile(string description, string company, string education, string url, string uid) {
            bool success = false;
            string sqlSelect = "SELECT * FROM Profiles WHERE UID = '" + uid + "'";
            MySqlConnection con = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, con);
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);
            if (sqlDt.Rows.Count > 0) {
                //UPDATE t SET lastname = 'Alfred Schmidt', password = '123' WHERE firstname = 'Pramod';
                string sqlUpdate = "UPDATE Profiles SET description='" + description + "', company_name='" + company + "', education='" + 
                                education + "', profile_pic='" + url + "' WHERE UID=@val1;";

                MySqlConnection con3 = new MySqlConnection(getConString());
                MySqlCommand sqlCommand3 = new MySqlCommand(sqlUpdate, con3);
                sqlCommand3.Parameters.AddWithValue("@val1", uid);
                con3.Open();
                sqlCommand3.ExecuteNonQuery();
                con3.Close();
                success = true;
            }
            else {
        
                string sqlInsert = "INSERT INTO Profiles(`PID`,`description`,`company_name`,`education`,`profile_pic`,`UID`) Values(default, '" + description + "', '"
                     + company + "','" + education + "','" + url + "','" + uid + "')";
                MySqlConnection con2 = new MySqlConnection(getConString());
                MySqlCommand sqlCommand2 = new MySqlCommand(sqlInsert, con2);
                con2.Open();
                sqlCommand2.ExecuteNonQuery();
                con2.Close();
                success = true;
            }



            return success;
        }


		[WebMethod(EnableSession = true)]
		public User[] getUsers()
		{
			//check out the return type.  It's an array of Account objects.  You can look at our custom Account class in this solution to see that it's 
			//just a container for public class-level variables.  It's a simple container that asp.net will have no trouble converting into json.  When we return
			//sets of information, it's a good idea to create a custom container class to represent instances (or rows) of that information, and then return an array of those objects.  
			//Keeps everything simple.

			DataTable sqlDt = new DataTable("users");




			string sqlSelect = "select UID, UserName, FirstName, LastName, Email, SuperUser from Users order by UID";


			MySqlConnection con = new MySqlConnection(getConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, con);

			//gonna use this to fill a data table
			MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
			//filling the data table
			sqlDa.Fill(sqlDt);

			//loop through each row in the dataset, creating instances
			//of our container class Account.  Fill each acciount with
			//data from the rows, then dump them in a list.
			List<User> users = new List<User>();
			for (int i = 0; i < sqlDt.Rows.Count; i++)
			{
				users.Add(new User
				{
					uid = Convert.ToInt32(sqlDt.Rows[i]["UID"]),
					userName = sqlDt.Rows[i]["UserName"].ToString(),
					firstName = sqlDt.Rows[i]["FirstName"].ToString(),
					lastName = sqlDt.Rows[i]["LastName"].ToString(),
					email = sqlDt.Rows[i]["Email"].ToString(),
					superUser = Convert.ToInt32(sqlDt.Rows[i]["SuperUser"])
				});
			}
			//convert the list of accounts to an array and return!
			return users.ToArray();
		}

		[WebMethod(EnableSession = true)]
		public Requests[] getRequests() {
			List<Requests> requests = new List<Requests>();
			DataTable sqlDt = new DataTable("request");
			string sqlSelect = "select RID, mentorName, menteeName, status from requests order by RID";
			MySqlConnection con = new MySqlConnection(getConString());
			MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, con);
			MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
			sqlDa.Fill(sqlDt);
			for (int i = 0; i < sqlDt.Rows.Count; i++)
			{
				requests.Add(new Requests
				{
					rid = Convert.ToInt32(sqlDt.Rows[i]["RID"]),
					mentorName = sqlDt.Rows[i]["mentorName"].ToString(),
					menteeName = sqlDt.Rows[i]["menteeName"].ToString(),
					status = Convert.ToInt32(sqlDt.Rows[i]["status"]),

				});
			}




			return requests.ToArray();

		}

		[WebMethod(EnableSession = true)]
		public bool updateRequest(int rid, int status) {
			bool success = false;
			string sqlstatement;
			MySqlConnection con = new MySqlConnection(getConString());
			if (status == 0)
			{
				sqlstatement = "Delete from requests WHERE RID = " + rid;
				MySqlCommand sqlCommand = new MySqlCommand(sqlstatement, con);
				con.Open();
				sqlCommand.ExecuteNonQuery();
				con.Close();
				success = true;
			}
			else {
				sqlstatement = "UPDATE requests SET status ='" + status + "' WHERE RID ='" + rid +"'" ;
				MySqlCommand sqlCommand = new MySqlCommand(sqlstatement, con);
				con.Open();
				sqlCommand.ExecuteNonQuery();
				con.Close();
				success = true;
			}
			
			
			return success;

		}

		[WebMethod(EnableSession = true)]
		public bool makeRequest(string mentor, string mentee) {
			bool success = true;
			MySqlConnection con = new MySqlConnection(getConString());
			string sqlInsert = "INSERT INTO requests(`RID`,`mentorName`,`menteeName`,`status`) Values(default,'" + mentor + "','"
					+ mentee + "','"  + null + "')";
			MySqlCommand sqlCommand = new MySqlCommand(sqlInsert, con);
			con.Open();
			sqlCommand.ExecuteNonQuery();
			con.Close();

			return success;


		}
	}

}
            


        
       
