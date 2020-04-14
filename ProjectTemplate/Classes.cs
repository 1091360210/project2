using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate
{
    public class Profile
    {
        public int pid;
        public string companyName;
        public string education;
        public string picture;
        public string description;
        public int uid;
    }

	public class User
	{
		//this is just a container for all info related
		//to an account.  We'll simply create public class-level
		//variables representing each piece of information!
		public int uid;
		public string firstName;
		public string lastName;
		public string email;
		public string userName;
		public string password;
		public int superUser;
	}

	public class Requests {
		public int rid;
		public string mentorName;
		public string menteeName;
		public int status;
	}
}