using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CourseProjectDB
{
    public partial class Form4 : Form
    {
        DataTable tableTemp;
        public Form4()
        {
            InitializeComponent();
        }

        private void UpdateTable()
        {
            tableTemp = new DataTable();
            SqlCommand cmd2 = new SqlCommand($"SELECT * FROM dbo.Employee ", Program.connection);
            Program.connection.Open();
            cmd2.ExecuteNonQuery();
            SqlDataReader reader = cmd2.ExecuteReader();
            tableTemp.Load(reader);
            Program.connection.Close();
            dataGridView1.DataSource = tableTemp;
            dataGridView1.Columns[0].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand($"INSERT INTO Employee(EmployeeName, [EmployeePrivilege], [EmployeePhone], [EmployeeLogin], [EmployeePassword]) " +
                $"VALUES (N'{textBox2.Text}', {textBox3.Text}, N'{textBox4.Text}', N'{textBox5.Text}', N'{textBox6.Text}')", Program.connection);
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            Program.connection.Close();
            UpdateTable();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand($"DELETE FROM Employee WHERE idEmployee = {dataGridView1.CurrentRow.Cells[0].Value}", Program.connection);
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            Program.connection.Close();
            UpdateTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand($"UPDATE Employee SET EmployeeName = N'{textBox11.Text}', " +
                $"[EmployeePrivilege] = {textBox10.Text}, [EmployeePhone] = N'{textBox9.Text}'," +
                $" [EmployeeLogin] = N'{textBox8.Text}', [EmployeePassword] = N'{textBox7.Text}'" +
                $" WHERE idEmployee = {dataGridView1.CurrentRow.Cells[0].Value}", Program.connection);
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            Program.connection.Close();
            UpdateTable();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox11.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox10.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox9.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        }
    }
}
