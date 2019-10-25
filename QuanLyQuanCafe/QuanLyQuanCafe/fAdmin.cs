using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();

        BindingSource accountList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }


        #region method
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromdate.Value, dtpkTodate.Value);
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoCombobox(cbFoodcategory);
            AddFoodBinding();
            AddAccountBinding();
        }

        void AddAccountBinding()
        {
            txtUsername.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayname.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "TypeAccount", true, DataSourceUpdateMode.Never));

        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }


        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromdate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkTodate.Value = dtpkFromdate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource =  BillDAO.Instance.GetBillListDate(checkIn, checkOut);
        }

        void AddFoodBinding()
        {
            txtFoodname.DataBindings.Add(new Binding("Text", dtgvFood.DataSource,"name", true, DataSourceUpdateMode.Never));
            txtFoodid.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "id", true, DataSourceUpdateMode.Never));
            nmFoodprice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "name";
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bạn!");
            }
            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Cập nhật  tài khoản thất bạn!");
            }
            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng không xóa chính bản thân bạn!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa nhật tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Xóa nhật  tài khoản thất bạn!");
            }
            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công!");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bạn!");
            }
        }

        #endregion

        #region events
        private void btnAddaccount_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text;
            string displayName = txtDisplayname.Text;
            int type = (int)nmType.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteaccount_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text;

            DeleteAccount(userName);
        }

        private void btnEditaccount_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text;
            string displayName = txtDisplayname.Text;
            int type = (int)nmType.Value;

            EditAccount(userName, displayName, type);
        }

        private void btnResrtpass_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text;

            ResetPass(userName);
        }

        private void btnShowaccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnSearchfood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txtSearchfood.Text);
        }
        private void txtFoodid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["categoryid"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    cbFoodcategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodcategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbFoodcategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAddfood_Click(object sender, EventArgs e)
        {
            string name = txtFoodname.Text;
            int categoryID = (cbFoodcategory.SelectedItem as Category).ID;
            float price = (float)nmFoodprice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công!");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn!");
            }
        }

        private void btnEidtfood_Click(object sender, EventArgs e)
        {
            string name = txtFoodname.Text;
            int categoryID = (cbFoodcategory.SelectedItem as Category).ID;
            float price = (float)nmFoodprice.Value;
            int id = Convert.ToInt32(txtFoodid.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công!");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn!");
            }
        }

        private void btnDeletefood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtFoodid.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công!");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn!");
            }
        }  
        private void btnViewbill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromdate.Value, dtpkTodate.Value);
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel27_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel22_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel28_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel29_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnShowfood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        #endregion

        
    }
}
