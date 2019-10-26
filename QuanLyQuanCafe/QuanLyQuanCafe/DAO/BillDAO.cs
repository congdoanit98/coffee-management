using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO();  return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }
        private BillDAO() { }

        /// Thanh cong bill ID
        /// That bai -1

        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExcuteQurey("select * from bill where idtable = "+id+" and statuss = 0");

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }

        public void CheckOut(int id,int discount, float totalPrice)
        {
            string query = "update bill set datecheckout = getdate(), statuss = 1," + " discount = " + discount +", totalPrice = "+ totalPrice + " where id = "+id;
            DataProvider.Instance.ExcuteNonQurey(query);
        }

        public void InsertBill(int id)
        {
            DataProvider.Instance.ExcuteNonQurey("exec USP_InsertBill @idTable", new object[]{id});
        }

        public DataTable GetBillListDate(DateTime checkin, DateTime checkout)
        {
            return DataProvider.Instance.ExcuteQurey("exec USP_GetListBillByDate @checkin , @checkout", new object[] { checkin, checkout });
        }

        public DataTable GetBillListDateAndPage(DateTime checkin, DateTime checkout, int pageNum)
        {
            return DataProvider.Instance.ExcuteQurey("exec USP_GetListBillByDateAndPage @checkin , @checkout , @page", new object[] { checkin, checkout, pageNum});
        }

        public int GetNumBillListDate(DateTime checkin, DateTime checkout)
        {
            return (int)DataProvider.Instance.ExcuteScalar("exec USP_GetNumBillByDate @checkin , @checkout", new object[] { checkin, checkout });
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExcuteScalar("select max(id) from bill");
            }
            catch
            {
                return 1;
            }

        }
    }
}
