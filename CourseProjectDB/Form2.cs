using Microsoft.Office.Interop.Word;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace CourseProjectDB
{
    public partial class Form2 : Form
    {
        System.Data.DataTable tableTemp;
        private readonly string DocumentPath = @"Documents\";

        public Form2()
        {
            InitializeComponent();
            Program.MainForm = this;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.FormAutorisation.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            UpdateTable();

            switch (Program.UserPrivelege)
            {
                case 2: сотрудникиToolStripMenuItem.Visible = true;
                    button4.Visible = true;
                    просмотретьДанныеToolStripMenuItem.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    button7.Enabled = true;
                    button10.Enabled = true;
                    break;
                case 1:
                    просмотретьДанныеToolStripMenuItem.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    button7.Enabled = true;
                    button10.Enabled = true;
                    break;
            }
        }

        private void открытьПапкуСДокументамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(DocumentPath);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox8.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox6.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            dateTimePicker2.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells[4].Value);
            textBox4.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();

            tableTemp = new System.Data.DataTable();
            SqlCommand cmd = new SqlCommand($"Select fkClient, fkProduct, fkEmployee, fkPrice" +
            $" FROM Operation WHERE idOperation = {dataGridView1.CurrentRow.Cells[0].Value.ToString()}", Program.connection);
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            tableTemp.Load(reader);
            Program.connection.Close();
            DataRow[] Rows = tableTemp.Select();
            Program.ClienidChanged = int.Parse(Rows[0][0].ToString());
            Program.ProductidChanged = int.Parse(Rows[0][1].ToString());
            Program.PriceidChanged = int.Parse(Rows[0][3].ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tableTemp = new System.Data.DataTable();
            int count = 0;
            SqlCommand cmd = new SqlCommand($"SELECT idOperation FROM Operation WHERE OperationType = N'возврат'", Program.connection);
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            tableTemp.Load(reader);
            Program.connection.Close();

            DataRow[] rows = tableTemp.Select();
            for (int i = 0; i < rows.Length; i++)
            {
                SqlCommand cmd2 = new SqlCommand($"EXEC CheckDatePoduct @id = {rows[i][0]}", Program.connection);
                Program.connection.Open();
                count+=cmd2.ExecuteNonQuery();
                Program.connection.Close();
            }

            MessageBox.Show($"Затронуто строк: {count}", "Ломбард", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sender as Button == button1)
            {
                Program.TableName = "Client";
                Program.IsInsert = true;
            }
            else if (sender as Button == button2)
            {
                Program.TableName = "Product";
                Program.IsInsert = true;
            }
            else if (sender as Button == button3)
            {
                Program.TableName = "Price";
                Program.IsInsert = true;
            }
            else if (sender as Button == button12)
            {
                Program.TableName = "Client";
                Program.IsInsert = false;
            }
            else if (sender as Button == button11)
            {
                Program.TableName = "Product";
                Program.IsInsert = false;
            }
            else if (sender as Button == button9)
            {
                Program.TableName = "Price";
                Program.IsInsert = false;
            }
            Program.SecetionForm = new Form3();
            Program.SecetionForm.Show();
        }

        private void UpdateTable ()
        {
            tableTemp = new System.Data.DataTable();
            SqlCommand cmd2 = new SqlCommand($"SELECT * FROM dbo.NormalizeOperation", Program.connection);
            Program.connection.Open();
            cmd2.ExecuteNonQuery();
            SqlDataReader reader = cmd2.ExecuteReader();
            tableTemp.Load(reader);
            Program.connection.Close();
            dataGridView1.DataSource = tableTemp;
            dataGridView1.Columns[0].Visible = false;
            Program.Priceid = -1;
            Program.Productid = -1;
            Program.Clienid = -1;
        }

        //изменение
        private void button5_Click(object sender, EventArgs e)
        {
            if (Program.Priceid != -1 & Program.Productid != -1 & Program.Clienid != -1 & textBox5.Text != "")
            {
                SqlCommand cmd = new SqlCommand($"INSERT INTO Operation (fkClient, fkProduct, fkEmployee, fkPrice, OperationDocLink, OperationDate, OperationType) " +
                    $"VALUES ({Program.Clienid}, {Program.Productid}, {Program.Userid}, {Program.Priceid}, 'none', '{dateTimePicker1.Value.ToString("M/d/yyyy")}', N'{textBox5.Text}')", Program.connection);
                Program.connection.Open();
                cmd.ExecuteNonQuery();
                Program.connection.Close();
                UpdateTable();
            }
            else
            {
                MessageBox.Show("Вы заполнили не все поля для добавления", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //редактирование
        private void button6_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand($"UPDATE Operation SET fkClient = {Program.ClienidChanged}, " +
                $"fkProduct = {Program.ProductidChanged}, fkEmployee = {Program.Userid}, fkPrice = {Program.PriceidChanged}, " +
                $"OperationDate = '{dateTimePicker2.Value.ToString("yyyy-MM-dd")}' , OperationType = N'{textBox4.Text}' " +
                $"WHERE idOperation = {dataGridView1.CurrentRow.Cells[0].Value.ToString()}", Program.connection);
            Program.connection.Open();
            cmd.ExecuteNonQuery();
            Program.connection.Close();
            UpdateTable();
        }

        //удаление
        private void button10_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Подтвердить удаление?","Удаление",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand($"DELETE FROM Operation WHERE idOperation = {dataGridView1.CurrentRow.Cells[0].Value.ToString()}", Program.connection);
                Program.connection.Open();
                cmd.ExecuteNonQuery();
                Program.connection.Close();
                UpdateTable();
            }
        }

        //сотрудники
        private void button4_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }


        //метод вставки переменных в документ
        private void WordDocInsertOnValues(string value, string text, Document document)
        {
            var range = document.Content; range.Find.ClearFormatting();
            range.Find.Execute(FindText: value, ReplaceWith: text);
        }

        //договор
        private void button8_Click(object sender, EventArgs e)
        {
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            wordApp.Visible = false;
            try
            {
                var wordDoc = wordApp.Documents.Open(System.Windows.Forms.Application.StartupPath + @"\" + DocumentPath + @"Pattern.docx");
                WordDocInsertOnValues("{Number}", dataGridView1.CurrentRow.Cells[0].Value.ToString(), wordDoc);
                WordDocInsertOnValues("{ClientName}", dataGridView1.CurrentRow.Cells[1].Value.ToString(), wordDoc);
                WordDocInsertOnValues("{ClientName}", dataGridView1.CurrentRow.Cells[1].Value.ToString(), wordDoc);
                WordDocInsertOnValues("{Price}", dataGridView1.CurrentRow.Cells[3].Value.ToString(), wordDoc);
                WordDocInsertOnValues("{Operation}", dataGridView1.CurrentRow.Cells[6].Value.ToString(), wordDoc);
                WordDocInsertOnValues("{Date}", dataGridView1.CurrentRow.Cells[4].Value.ToString(), wordDoc);
                WordDocInsertOnValues("{Product}", dataGridView1.CurrentRow.Cells[2].Value.ToString(), wordDoc);
                WordDocInsertOnValues("{Price}", dataGridView1.CurrentRow.Cells[3].Value.ToString(), wordDoc);
                wordDoc.SaveAs(System.Windows.Forms.Application.StartupPath + @"\" + DocumentPath + @"Deale"+ dataGridView1.CurrentRow.Cells[0].Value.ToString());
                wordApp.Visible = true;

                SqlCommand cmd = new SqlCommand($"UPDATE Operation SET OperationDocLink = " +
                    $"N'{System.Windows.Forms.Application.StartupPath + @"\" + DocumentPath + @"Deale" + dataGridView1.CurrentRow.Cells[0].Value.ToString()}'" +
                    $" WHERE idOperation = {dataGridView1.CurrentRow.Cells[0].Value.ToString()}", Program.connection);
                Program.connection.Open();
                cmd.ExecuteNonQuery();
                Program.connection.Close();
                UpdateTable();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Проблема с отчетом"); }
        }
    }
}
