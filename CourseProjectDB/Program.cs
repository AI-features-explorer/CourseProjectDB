using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CourseProjectDB
{
    static class Program
    {
        /// Пользовательские данные
        public static string Username { get; set; }
        public static byte UserPrivelege { get; set; }

        /// Данные приложения
        internal static SqlConnection connection = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Database=Pawnshop;Trusted_Connection=True");
        public static Form1 FormAutorisation { get; set;}
        public static Form2 MainForm { get; set;}
        public static Form3 SecetionForm { get; set; }

        /// Операнды
        public static string TableName { get; set; }
        public static bool IsInsert { get; set; }

        /// Данные для таблицы Операция
        public static int Userid { get; set; }      /// на форме 1
        public static int Clienid { get; set; }     /// на форме 3
        public static int Priceid { get; set; }     /// на форме 3
        public static int Productid { get; set; }   /// на форме 3

        public static int ClienidChanged { get; set; }     /// на форме 3
        public static int PriceidChanged { get; set; }     /// на форме 3
        public static int ProductidChanged { get; set; }   /// на форме 3

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
