using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel.EMPLOYEE
{
    public partial class CheckInForm : Form
    {
        int id_assignment;
        public CheckInForm()
        {
            InitializeComponent();
        }


        WORKING work = new WORKING();
        Assignment AssignmentSQL = new Assignment();
       

        private void CheckInForm_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = work.GetEmployee(DateTime.Now, GlobalVar._GlobalType, GlobalVar._id);
                cbReplace.DataSource = dt;
                cbReplace.DisplayMember = "name";
                cbReplace.ValueMember = "id_employee";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Toang", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCheckin_Click(object sender, EventArgs e)
        {
            try
            {
                int id_employee = Convert.ToInt32(cbReplace.SelectedValue);
                DataTable dt = AssignmentSQL.checkID(id_employee, DateTime.Now);
                if (dt.Rows.Count > 0)
                {
                    int id = (int)(dt.Rows[0][0]);//id của bảng assignment
                    if (work.ExistCheckin(id_employee, id))
                    {
                        MessageBox.Show("Đã Checkin. Vui lòng không checkin lại", "Checkin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    dt = AssignmentSQL.GetDateByID(id);
                    if (AssignmentSQL.AddEmployeesInShiftAndDate(GlobalVar._id, (int)dt.Rows[0][0], (DateTime)dt.Rows[0][1], (DateTime)dt.Rows[0][2], (float)((DateTime.Now - (DateTime)dt.Rows[0][1]).TotalMinutes / 60), 0, GlobalVar._id))//status=0 là làm thay,id
                    {
                        dt = AssignmentSQL.checkID(GlobalVar._id, DateTime.Now);
                        work.AddWorking((int)dt.Rows[0][0], GlobalVar._id);
                        MessageBox.Show("Checkin thành công");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("CheckIn không thành công");
                    }
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            
        }
    }
}
