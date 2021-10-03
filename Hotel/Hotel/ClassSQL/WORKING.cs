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
        public bool ExistCheckin(int id_employee, int id_assignment)//kiểm tra đã checkin chưa
        {
            string query = "select * from working where id_employee=@id_employee and id=@id_assignment";//status=-1 là chưa checkin
            Mydb.openConnection();
            try
            {
                SqlCommand command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@id_employee", SqlDbType.Int).Value = id_employee;
                command.Parameters.Add("@id_assignment", SqlDbType.Int).Value = id_assignment;
                DataTable dt = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter(command);
                Adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Mydb.closeConnection();
                    return true;
                }
                else
                {
                    Mydb.closeConnection();
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Assignment SQL");
                Mydb.closeConnection();
                return false;
            }
        }

        public bool ExistCheckout(int id_employee, int id_assignment)//kiểm tra đã checkout chưa
        {
            string query = "select* from working where id_employee = @id_employee and id = @id_assignment";//status=-1 là chưa checkin
            Mydb.openConnection();
            try
            {
                SqlCommand command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@id_employee", SqlDbType.Int).Value = id_employee;
                command.Parameters.Add("@id_assignment", SqlDbType.Int).Value = id_assignment;
                DataTable dt = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter(command);
                Adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Mydb.closeConnection();
                    return false;
                }
                else
                {
                    Mydb.closeConnection();
                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Assignment SQL");
                Mydb.closeConnection();
                return false;
            }
        }

        public bool DelWorking(int id)
        {
            string query = "delete from working  where id=@id";
            Mydb.openConnection();
            try
            {
                SqlCommand command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                if (command.ExecuteNonQuery() > 0)
                {
                    Mydb.closeConnection();
                    return true;
                }
                else
                {
                    Mydb.closeConnection();
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Checkin SQL");
                Mydb.closeConnection();
                return false;
            }
        }
        public bool UpdateCheckin(int id, DateTime checkin)
        {
            string query = "update assignment set delay=CAST(DATEDIFF(MINUTE, checkin,@checkin) AS FLOAT(32))/60 ,status=1 where id=@id";
            Mydb.openConnection();
            try
            {
                SqlCommand command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@checkin", SqlDbType.DateTime).Value = checkin;

                if (command.ExecuteNonQuery() > 0)
                {
                    Mydb.closeConnection();
                    return true;
                }
                else
                {
                    Mydb.closeConnection();
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Checkin SQL");
                Mydb.closeConnection();
                return false;
            }
        }

        public bool UpdateCheckout(int id, DateTime checkout)
        {
            string query = "update assignment set delay=(case when status=0 then 0 else delay end)," +
                "  soon=(case when status=0 then 0 else case when checkout<@checkout1  " +
                "then 0 else CAST(DATEDIFF(MINUTE, @checkout2,checkout) AS FLOAT(32))/60 end end) where id=@id";
            Mydb.openConnection();
            try
            {
                SqlCommand command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@checkout1", SqlDbType.DateTime).Value = checkout;
                command.Parameters.Add("@checkout2", SqlDbType.DateTime).Value = checkout;

                if (command.ExecuteNonQuery() > 0)
                {
                    Mydb.closeConnection();
                    return true;
                }
                else
                {
                    Mydb.closeConnection();
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Checkout SQL");
                Mydb.closeConnection();
                return false;
            }
        }
        public DataTable GetIDWorking(int id_employee)
        {

            string query;
            Mydb.openConnection();
            SqlCommand command;
            DataTable data = new DataTable();
            try
            {
                query = "select id from working where id_employee=@id_employee";

                command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@id_employee", SqlDbType.Int).Value = id_employee;
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

        public bool AddWorking(int id, int id_employee)
        {
            string query = "insert into working (id,id_employee) values (@id,@id_employee)";
            Mydb.openConnection();
            try
            {
                SqlCommand command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@id_employee", SqlDbType.Int).Value = id_employee;

                if (command.ExecuteNonQuery() > 0)
                {
                    Mydb.closeConnection();
                    return true;
                }
                else
                {
                    Mydb.closeConnection();
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Checkout SQL");
                Mydb.closeConnection();
                return false;
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
        
        public bool CheckIn(int id)
        {
            SqlCommand command = new SqlCommand("UPDATE Working Set status = @ostatus where id_employee = @oid", Mydb.getConnection);
            command.Parameters.Add("@oid", SqlDbType.Int).Value = id;
            command.Parameters.Add("@ostatus", SqlDbType.Int).Value = 1;
            if (command.ExecuteNonQuery() > 0)
            {
                Mydb.closeConnection();
                return true;
            }
            else
            {
                Mydb.closeConnection();
                return false;
            }
        }
        public bool ThayCa(int id, int eid)
        {
            SqlCommand command = new SqlCommand("UPDATE Working Set status = @ostatus, id2=@eid  where id_employee = @oid", Mydb.getConnection);
            command.Parameters.Add("@oid", SqlDbType.Int).Value = id;
            command.Parameters.Add("@ostatus", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@eid", SqlDbType.Int).Value = eid;
            if (command.ExecuteNonQuery() > 0)
            {
                Mydb.closeConnection();
                return true;
            }
            else
            {
                Mydb.closeConnection();
                return false;
            }
        }
        public DataTable getWorking()
        {
            SqlCommand command = new SqlCommand("select working.id_employee, name, shift from working inner join Employees on id_employee = id", Mydb.getConnection);
            Mydb.openConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
    }
}

