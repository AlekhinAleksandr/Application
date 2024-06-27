using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Personel_accounting
{
    public partial class Job_openings : Form
    {
        DataSet ds;
        SqlConnection my_conn;
        SqlDataAdapter my_data;
        SqlCommand my_command;

        Form1 form1 = new Form1();

        string sls1 = "";
        public Job_openings()
        {
            InitializeComponent();

            Loading();
        }
        public void Loading()
        {
            // Настройки таблиц

            table.ColumnHeadersDefaultCellStyle.Font = new Font(table.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold); // Шрифт и размер названий столбцо
            table.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // выравнивание названий столбцов по центру

            table.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// выравнивание строк по центру
            table.DefaultCellStyle.Font = new Font("Times New Roman", 12); // Шрифт и размер строк

            ds = new DataSet(); //Создаем объект класса DataSet

            my_conn = new SqlConnection(form1.connectionString); //Создаем соеденение

            string sql = "Select p.[Код вакансии], u.Должность, p.[Дата объявления] " +
                "FROM Вакансия as p JOIN Должность as u ON u.[Код должности] = p.[Код должности] ORDER BY p.[Код вакансии] ASC"; //Sql запрос (достать все из таблицы ...)

            my_data = new SqlDataAdapter(sql, my_conn);//Создаем объект класса DataAdapter (тут мы передаем наш запрос и получаем ответ)

            my_data.Fill(ds, "Вакансия");//Заполняем DataSet cодержимым DataAdapter'a

            table.DataSource = ds.Tables[0].DefaultView;//Заполняем созданный на форме dataGridView1
        }

        private void Job_openings_Load(object sender, EventArgs e)
        {
            table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // Автоматическая высота столбцов
            table.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Настройка выпадающего списка должности
            my_data = new SqlDataAdapter("select * from Должность", form1.connectionString);

            DataTable tbl = new DataTable();

            my_data.Fill(tbl);

            post.DataSource = tbl;
            post.DisplayMember = "Должность";// столбец для отображения
            post.ValueMember = "Код должности";//столбец с id
        }
        // Кнопка добавить вакансию
        private void add_Click(object sender, EventArgs e)
        {
            my_conn.Open(); // Открытие соединения с базой данных

            my_command = my_conn.CreateCommand();

            my_command.CommandText = "Select * from Должность where Должность  ='" + post.Text + "';";

            SqlDataReader sdr = my_command.ExecuteReader();

            while (sdr.Read())
            {
                sls1 = sdr["Код должности"].ToString();
            }

            my_conn.Close();

            string commandText = string.Format("INSERT INTO Вакансия ([Код должности], [Дата объявления]) VALUES ('{0}', '{1:yyyy.MM.dd}')", sls1, dateTimePicker1.Value); // Cтрока передачи данных

            my_conn = new SqlConnection(form1.connectionString); //Создаем соеденение

            my_command = new SqlCommand(commandText, my_conn);

            my_conn.Open(); // Открытие соединения с базой данных

            my_command.ExecuteNonQuery(); // sql возвращает сколько строк обработано

            MessageBox.Show("Операция выполнена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Вывод сообщения о добавлении

            my_conn.Close();

            Loading();
        }
        // Кнопка удалить вакансию
        private void delete_Click(object sender, EventArgs e)
        {
            int a = table.CurrentRow.Index; // Выделенная строка в таблице

            string id_table = Convert.ToString(table.Rows[a].Cells[0].Value); // номер строки для удаления

            string sql_delete = string.Format("DELETE FROM Вакансия WHERE ([Код вакансии]) = {0}", id_table); // запрос на удаление в БД

            my_command = new SqlCommand(sql_delete, my_conn);

            my_conn.Open(); // Открытие соединения с базой данных 

            my_command.ExecuteNonQuery(); // sql возвращает сколько строк обработано

            my_conn.Close();

            Loading();

            MessageBox.Show("Операция выполнена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Вывод сообщения об удалении
        }
    }
}
