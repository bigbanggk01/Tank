using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        static ServerSocket server;

        public Form1()
        {
            InitializeComponent();
            server = new ServerSocket();
            server.Start();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadData();
        }
        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-AB6F94G;Initial Catalog=TankDB;Integrated Security=True");
        private void AddCard_Click(object sender, EventArgs e)
        {
            if (CardNum.Text.Equals("") == false)
            {
                int id;
                string query = "Select * from card";
                using (SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon))
                {
                    DataTable dtbl = new DataTable();
                    sda.Fill(dtbl);
                    id = dtbl.Rows.Count + 1;
                }
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (SqlCommand command = new SqlCommand("insert into card(cardid,cardnum, status)" +
                        "values(@cardid,@cardnum,@status)", sqlcon))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@cardid", id);
                            command.Parameters.AddWithValue("@cardnum", CardNum.Text);
                            command.Parameters.AddWithValue("@status", 0);
                            sda.InsertCommand = command;
                            sqlcon.Open();
                            sda.InsertCommand.ExecuteNonQuery();
                            sqlcon.Close();
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
                CardNum.Text = "";
                LoadData();
                return;
            }
        }
        void LoadData()
        {
            dataGridView1.DataSource = Table();
        }
        DataTable Table()
        {
            DataTable dtbl = new DataTable();
            string query = "select * from card";
            sqlcon.Open();

            using (SqlDataAdapter adapter = new SqlDataAdapter(query, sqlcon))
            {

                adapter.Fill(dtbl);
            }
            sqlcon.Close();
            return dtbl;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
