using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel
{
    class WORKING
    {
        MyDB Mydb = new MyDB();

        public DataTable GetEmployee(DateTime date)
        {
            string query;
            Mydb.openConnection();
            SqlCommand command;
            DataTable data = new DataTable();
            try
            {
                query = "select E.id as id_employee ,E.name,T.name as type from employees as E inner join" +
                    " type_employee as T on E.type=T.id " +
                    " where not exists (select A.id_employee from assignment as A where  CAST(A.checkin as DATE)=@date and E.id=A.id_employee)";

                command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@date", SqlDbType.Date).Value = date;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(data);

                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Assignment SQL");
                Mydb.closeConnection();
                return data;
            }
        }

        public DataTable GetEmployee(DateTime date, int type,int id)
        {
            string query;
            Mydb.openConnection();
            SqlCommand command;
            DataTable data = new DataTable();
            try
            {
                query = "select distinct E.id as id_employee ,E.name as name,T.name as type from employees as E inner join" +
                    " type_employee as T on E.type=T.id " +
                    " inner join (select A.id_employee from assignment as A where A.checkin<=@date and A.checkout>=@date2 and A.id_employee<>@id) as Work" +
                    " on Work.id_employee=E.id where E.type=@type";

                command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
                command.Parameters.Add("@date2", SqlDbType.DateTime).Value = date;
                command.Parameters.Add("@type", SqlDbType.Int).Value = type;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(data);

                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Assignment SQL 2");
                Mydb.closeConnection();
                return data;
            }
        }

      

        public DataTable LogIn(string username, string password,int type)
        {
            SqlCommand command = new SqlCommand("select log_in.id from log_in inner join Employees on log_in.id=Employees.id where username= @username and password= @password and type= @type", Mydb.getConnection);
            command.Parameters.Add("@username", SqlDbType.NChar).Value = username;
            command.Parameters.Add("@password", SqlDbType.NChar).Value = password;
            command.Parameters.Add("@type", SqlDbType.NChar).Value = type;
            Mydb.openConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        
    }
}

