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
    class CARD
    {
        MyDB Mydb = new MyDB();
       
        public bool EditStatusCard(string room, int status)
        {
            string query = "update card set status= @status where id_room= @room";
            Mydb.openConnection();
            try
            {
                SqlCommand command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@status", SqlDbType.Int).Value = status;
                command.Parameters.Add("@room", SqlDbType.NVarChar).Value = room;

                if (command.ExecuteNonQuery() > 0)
                {
                    Mydb.closeConnection();
                    return true;
                }
                Mydb.closeConnection();
                return false;
            }
            catch
            {
                Mydb.closeConnection();
                return false;
            }
        }

        public DataTable GetStatusCard(string room)
        {
            string query = "select status from card where id_room=@id_room";
            Mydb.openConnection();
            DataTable data = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand(query, Mydb.getConnection);
                command.Parameters.Add("@id_room", SqlDbType.NVarChar).Value = room;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(data);
                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Mydb.closeConnection();
                return data;
            }
        }

    }
}
