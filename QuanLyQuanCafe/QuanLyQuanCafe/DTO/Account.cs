using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        public Account(string userName, string displayName, int typeAccount, string password = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.TypeAccount = typeAccount;
            this.Password = password;
        }

        public Account(DataRow row)
        {
            this.UserName = row["username"].ToString();
            this.DisplayName = row["displayname"].ToString();
            this.TypeAccount = (int)row["typeaccount"];
            this.Password = row["pass"].ToString();
        }

        private int typeAccount;

        public int TypeAccount
        {
            get { return typeAccount; }
            set { typeAccount = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
    }
}
