using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CourseProjectDB
{
    public partial class Form3 : Form
    {
        DataTable tableTemp;
        string tableName;

        bool isNotView = true;
        public Form3()
        {
            InitializeComponent();
            Program.SecetionForm = this;
        }

        //загрузка выбранной таблицы на форму
        private void Form3_Load(object sender, EventArgs e)
        { 
            tableName = Program.TableName;
            button5.Visible = false;
            checkBox1.Visible = false;
            UpdateTable();

            if (Program.UserPrivelege == 0)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }

            switch (tableName)
            {
                case "Client":
                    label1.Text = "Список клиентов";
                    label3.Visible = true;
                    label4.Visible = true;
                    label4.Visible = true;
                    label8.Visible = true;
                    textBox2.Visible = true;
                    textBox7.Visible = true;
                    textBox3.Visible = true;
                    textBox6.Visible = true;
                    label2.Text = dataGridView1.Columns[1].Name.ToString();
                    label6.Text = dataGridView1.Columns[1].Name.ToString();
                    label3.Text = dataGridView1.Columns[2].Name.ToString();
                    label7.Text = dataGridView1.Columns[2].Name.ToString();
                    label4.Text = dataGridView1.Columns[3].Name.ToString();
                    label8.Text = dataGridView1.Columns[3].Name.ToString();
                    break;
                case "Product":
                    label1.Text = "Список товаров";
                    button5.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    textBox2.Visible = true;
                    textBox7.Visible = true;
                    label2.Text = dataGridView1.Columns[1].Name.ToString();
                    label6.Text = dataGridView1.Columns[1].Name.ToString();
                    label3.Text = dataGridView1.Columns[2].Name.ToString();
                    label7.Text = dataGridView1.Columns[2].Name.ToString();
                    label4.Visible = false;
                    label8.Visible = false;
                    textBox3.Visible = false;
                    textBox6.Visible = false;
                    break;
                case "Price":
                    label1.Text = "Список цен";
                    checkBox1.Visible = true;
                    label2.Text = dataGridView1.Columns[1].Name.ToString();
                    label6.Text = dataGridView1.Columns[1].Name.ToString();
                    label3.Visible = false;
                    label4.Visible = false;
                    label7.Visible = false;
                    label8.Visible = false;
                    textBox2.Visible = false;
                    textBox7.Visible = false;
                    textBox3.Visible = false;
                    textBox6.Visible = false;
                    break;
            }
        }

        //обновляет строки таблицы
        private void button6_Click(object sender, EventArgs e)
        {
            //флаг для предсавления
            isNotView = true;
            UpdateTable();
        }

        private void UpdateTable ()
        {
            tableTemp = new DataTable();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM dbo.{tableName}", Program.connection);
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            tableTemp.Load(reader);
            Program.connection.Close();
            dataGridView1.DataSource = tableTemp;
            dataGridView1.Columns[0].Visible = false;
        }

        // подгрузка данных строк для редактирования Operation
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            switch (tableName)
            {
                case "Client":
                    textBox8.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    textBox7.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    textBox6.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    break;
                case "Product":
                    if (isNotView == true) 
                    { 
                        textBox8.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                        textBox7.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    }
                    break;
                case "Price":
                    textBox8.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    break;
            }
        }

        //подгрузка представления
        private void button5_Click(object sender, EventArgs e)
        {
            //флаг для представления
            isNotView = false;

            tableTemp = new DataTable();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM dbo.ViewOwnedProuduct", Program.connection);
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            tableTemp.Load(reader);
            Program.connection.Close();
            dataGridView1.DataSource = tableTemp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            switch (tableName)
            {
                case "Client": cmd = new SqlCommand($"Insert into Client(ClientName, ClientAddress, ClientPhone) " +
                    $"VALUES (N'{textBox1.Text}', N'{textBox2.Text}', N'{textBox3.Text}')", Program.connection); break;

                case "Product": cmd = new SqlCommand($"Insert into Product (ProductName, ProductOwner) " +
                    $"VALUES (N'{textBox1.Text}', N'{textBox2.Text}')", Program.connection); break;

                case "Price": cmd = new SqlCommand($"Insert into Price (PriceValue, PriceHeveCommission) " +
                    $"VALUES ({textBox1.Text}, '{checkBox1.Checked}')", Program.connection); break;
            }
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            Program.connection.Close();
            UpdateTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand($"DELETE FROM {tableName} WHERE id{tableName} = {dataGridView1.CurrentRow.Cells[0].Value.ToString()}",Program.connection);
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            Program.connection.Close();
            UpdateTable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            switch (tableName)
            {
                case "Client":
                    cmd = new SqlCommand($"Update Client SET ClientName = N'{textBox8.Text}', " +
                        $"ClientAddress = N'{textBox7.Text}', ClientPhone = N'{textBox6.Text}' " +
                        $"WHERE idClient = {dataGridView1.CurrentRow.Cells[0].Value.ToString()}", Program.connection); break;

                case "Product":
                    cmd = new SqlCommand($"Update Product  SET ProductName = N'{textBox8.Text}', ProductOwner = N'{textBox7.Text}' " +
                        $"WHERE idProduct = {dataGridView1.CurrentRow.Cells[0].Value.ToString()}", 
                        Program.connection); break;

                case "Price":
                    string Source = textBox8.Text.Replace(',', '.');
                    cmd = new SqlCommand($"Update Price SET PriceValue = {Source} , PriceHeveCommission = '{checkBox1.Checked}' " +
                        $"WHERE idPrice = {dataGridView1.CurrentRow.Cells[0].Value.ToString()}",
                        Program.connection); break;
            }
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            Program.connection.Close();
            UpdateTable();
        }

        //выбор записи
        private void button4_Click(object sender, EventArgs e)
        {
            if (isNotView)
            {
                if (Program.IsInsert)
                {
                    switch (tableName)
                    {
                        case "Client":
                            Program.Clienid = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                            Program.MainForm.textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                            break;
                        case "Product":
                            Program.Productid = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                            Program.MainForm.textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                            break;
                        case "Price":
                            Program.Priceid = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                            Program.MainForm.textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                            break;
                    }
                }
                else
                {
                    switch (tableName)
                    {
                        case "Client":
                            Program.ClienidChanged = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                            Program.MainForm.textBox8.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                            break;
                        case "Product":
                            Program.ProductidChanged = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                            Program.MainForm.textBox7.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                            break;
                        case "Price":
                            Program.PriceidChanged = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                            Program.MainForm.textBox6.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                            break;
                    }
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Вы не можете выбрать запись из представления, обновите таблицу",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
