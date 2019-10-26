using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }
        private AccountDAO() { }
        public bool Login(string userName, string passWord)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";
            foreach (byte item in hasData)
            {
                hasPass += item;
            }

            // var list = hasData.ToString();
            // list.Reverse();

            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExcuteQurey(query, new object[] { userName, hasPass });
            return result.Rows.Count > 0;
        }

        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExcuteNonQurey("exec USP_UpdateAccount @userName , @displayName , @password , @newPassword", new object[]{userName, displayName, pass,newPass});

            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExcuteQurey("select username, displayname, typeaccount from account");
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExcuteQurey("select * from account where username = '" + userName + "'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("insert account (username, displayname, typeaccount, pass ) values (N'{0}', N'{1}', {2}, N'{3}')", name, displayName, type, "20720532132149213101239102231223249249135100218");
            int result = DataProvider.Instance.ExcuteNonQurey(query);

            return result > 0;
        }

        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("update account set displayname = N'{1}', typeaccount = {2} where username = N'{0}'", name, displayName, type);
            int result = DataProvider.Instance.ExcuteNonQurey(query);

            return result > 0;
        }

        public bool DeleteAccount(string userName)
        {

            string query = string.Format("delete account where username = N'{0}'", userName);
            int result = DataProvider.Instance.ExcuteNonQurey(query);

            return result > 0;
        }

        public bool ResetPassword(string userName)
        {
            string query = string.Format("update account set pass = N'20720532132149213101239102231223249249135100218' where username = N'{0}'", userName);
            int result = DataProvider.Instance.ExcuteNonQurey(query);

            return result > 0;
        }
    }
}
