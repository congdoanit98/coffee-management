using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountFrofie : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccountt(loginAccount); }
        }
        public fAccountFrofie(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }

        void ChangeAccountt(Account acc)
        {
            txtUsername.Text = LoginAccount.UserName;
            txtDisplayname.Text = LoginAccount.DisplayName;

        }

        void UpdateAccountInfo()
        {
            string displayName = txtDisplayname.Text;
            string password = txtPassword.Text;
            string newPass = txtNewpass.Text;
            string reenterPass = txtReEnterpass.Text;
            string userName = txtUsername.Text;

            if (!newPass.Equals(reenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!");
            }
            else{
                if(AccountDAO.Instance.UpdateAccount(userName, displayName,password, newPass)){
                    MessageBox.Show("Cập nhật thành công!");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu!");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }
    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
