using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CourseProjectDB
{
    public partial class Form1 : Form
    {
        public DataTable TableTemp { get; set; }
        public DataRow[] Rows { get; set; }

        public Form1()
        {
            InitializeComponent();
            Program.FormAutorisation = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.Priceid = -1;
            Program.Productid = -1;
            Program.Userid = -1;
            Program.Clienid = -1;

            //команда для получения данных таблицы
            SqlCommand cmd = new SqlCommand($"SELECT * FROM dbo.Employee", Program.connection);

            TableTemp = new DataTable();
            Program.connection.Open();
            //выполнение команды
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            TableTemp.Load(reader);

            //контейнер данных таблицы
            Rows = TableTemp.Select();
            Program.connection.Close();

            //подгрузка результатов
            comboBox1.Items.Clear();
            for (int i = 0; i < Rows.Length; i++)
            {
                comboBox1.Items.Add(Rows[i][4]);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // авторизация
        private void button1_Click(object sender, EventArgs e)
        {
            int userDataPosition=0;

            for (int i = 0; i < Rows.Length; i++)
            {
                if (Rows[i][4].ToString() == comboBox1.Text)
                {
                    userDataPosition = i;
                    break;
                }
            }

            string passwordcheck = Rows[userDataPosition]["EmployeePassword"].ToString();

            if (passwordcheck == textBox1.Text)
            {
                Program.Userid = int.Parse(Rows[userDataPosition]["idEmployee"].ToString());
                Program.Username = Rows[userDataPosition]["EmployeeName"].ToString();
                Program.UserPrivelege = byte.Parse(Rows[userDataPosition]["EmployeePrivilege"].ToString());
                string Privelege = "";

                switch (Program.UserPrivelege)
                {
                    case 1: Privelege = "Администратор";
                            break;
                    case 0: Privelege = "Пользователь";
                            break;
                    case 2: Privelege = "Владелец";
                        break;

                }
                MessageBox.Show($"Вы успешно авторизовались под пользователем {Program.Username} - {Privelege}",
                    "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                Program.MainForm = new Form2();
                Program.MainForm.Show();
            }
            else
            {
                textBox1.Text = "";
                MessageBox.Show($"Ошибка авторизации. Неверно указан логин или пароль",
                   "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
