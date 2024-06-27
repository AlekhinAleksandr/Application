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
    public partial class Vacation : Form
    {
        DataSet ds;
        SqlConnection my_conn;
        SqlDataAdapter my_data;
        SqlCommand my_command;

        Form1 form1 = new Form1();

        string sls1 = "";
        public Vacation(string di, string FIO_employee)
        {
            this.di = di;
            this.FIO_employee = FIO_employee;

            InitializeComponent();

            Loading();
        }
        string di, FIO_employee;
        public void Loading()
        {
            id.Text = di;
            FIO.Text = FIO_employee;
            // Настройки таблицы

            table.ColumnHeadersDefaultCellStyle.Font = new Font(table.ColumnHeadersDefaultCellStyle.Font.FontFamily, 11, FontStyle.Bold); // Шрифт и размер названий столбцов
            table.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // выравнивание названий столбцов по центр

            table.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// выравнивание строк по центр
            table.DefaultCellStyle.Font = new Font("Times New Roman", 11); // Шрифт и размер строк

            ds = new DataSet(); //Создаем объект класса DataSet

            my_conn = new SqlConnection(form1.connectionString); //Создаем соеденение

            string sql = "Select p.[Код отпуска], u.ФИО as [ФИО сотрудника], u.[Дата рождения], y.[Вид отпуска], p.[Дата начала], p.[Дата окончания] " +
                "FROM Отпуск as p JOIN Сотрудник as u ON u.[Код сотрудника] = p.[Код сотрудника] JOIN [Вид отпуска] as y ON y.[Код вида отпуска] = p.[Код вида отпуска] ORDER BY p.[Код отпуска] ASC"; //Sql запрос (достать все из таблицы ...)

            my_data = new SqlDataAdapter(sql, my_conn);//Создаем объект класса DataAdapter (тут мы передаем наш запрос и получаем ответ)

            my_data.Fill(ds, "Отпуск");//Заполняем DataSet cодержимым DataAdapter'a

            table.DataSource = ds.Tables[0].DefaultView;//Заполняем созданный на форме dataGridView1
        }

        private void Vacation_Load(object sender, EventArgs e)
        {
            table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // Автоматическая высота столбцов
            table.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Настройка выпадающего списка должности
            my_data = new SqlDataAdapter("select * from [Вид отпуска]", form1.connectionString);

            DataTable tbl = new DataTable();

            my_data.Fill(tbl);

            vacations.DataSource = tbl;
            vacations.DisplayMember = "Вид отпуска";// столбец для отображения
            vacations.ValueMember = "Код вида отпуска";//столбец с id
        }
        // Кнопка назад
        private void back_Click(object sender, EventArgs e)
        {
            Loading();
        }
        // Кнопка удалить
        private void delete_Click(object sender, EventArgs e)
        {
            int a = table.CurrentRow.Index; // Выделенная строка в таблице

            string id_table = Convert.ToString(table.Rows[a].Cells[0].Value); // номер строки для удаления

            string sql_delete = string.Format("DELETE FROM Отпуск WHERE ([Код отпуска]) = {0}", id_table); // запрос на удаление в БД

            my_command = new SqlCommand(sql_delete, my_conn);

            my_conn.Open(); // Открытие соединения с базой данных 

            my_command.ExecuteNonQuery(); // sql возвращает сколько строк обработано

            my_conn.Close();

            Loading();

            MessageBox.Show("Операция выполнена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Вывод сообщения об удалении
        }
        // Кнопка сохранить изменения
        private void save_Click(object sender, EventArgs e)
        {
            int a = table.CurrentRow.Index; // Выделенная строка в таблице

            string id_table = Convert.ToString(table.Rows[a].Cells[0].Value);

            string strQuery = string.Format("UPDATE Отпуск SET [Дата начала] = @param1, [Дата окончания] = @param2 WHERE [Код отпуска] = {0}", id_table); // Строка передачи данных
            my_command = new SqlCommand(strQuery, my_conn);

            my_conn.Open();

            // Обновление соответствующих столбцов
            my_command.Parameters.AddWithValue("@param1", table.Rows[a].Cells["Дата начала"].Value);
            my_command.Parameters.AddWithValue("@param2", table.Rows[a].Cells["Дата окончания"].Value);

            my_command.ExecuteNonQuery();

            my_conn.Close();

            Loading();

            MessageBox.Show("Операция выполнена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Вывод сообщения об обновлении
        }
        // Показать на дату начала отпуска
        private void show_Click(object sender, EventArgs e)
        {
            ds = new DataSet(); //Создаем объект класса DataSet

            my_conn = new SqlConnection(form1.connectionString); //Создаем соеденение

            string sql = String.Format("Select p.[Код отпуска], u.ФИО as [ФИО сотрудника], u.[Дата рождения], y.[Вид отпуска], p.[Дата начала], p.[Дата окончания] " +
                "FROM Отпуск as p JOIN Сотрудник as u ON u.[Код сотрудника] = p.[Код сотрудника] JOIN [Вид отпуска] as y ON y.[Код вида отпуска] = p.[Код вида отпуска]  WHERE p.[Дата начала] = '{0:yyyy.MM.dd}' ORDER BY p.[Код отпуска] ASC", dateTimePicker1.Value);

            my_data = new SqlDataAdapter(sql, my_conn);//Создаем объект класса DataAdapter (тут мы передаем наш запрос и получаем ответ)

            my_data.Fill(ds, "Отпуск");//Заполняем DataSet cодержимым DataAdapter'a

            table.DataSource = ds.Tables[0].DefaultView;//Заполняем созданный на форме dataGridView1
        }
        
        // Кнопка добавить отпуск
        private void add_Click(object sender, EventArgs e)
        {
            if (FIO.Text == "" || id.Text == "") // Проверка правильности введенных исходных данных
            {
                MessageBox.Show("Проверьте правильность заполнения полей!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Вывод сообщения о ошибке
            }
            else
            {
                my_conn.Open(); // Открытие соединения с базой данных

                my_command = my_conn.CreateCommand();

                my_command.CommandText = "Select * from [Вид отпуска] where [Вид отпуска]  ='" + vacations.Text + "';";

                SqlDataReader sdr = my_command.ExecuteReader();

                while (sdr.Read())
                {
                    sls1 = sdr["Код вида отпуска"].ToString();
                }

                my_conn.Close();

                string commandText = string.Format("INSERT INTO Отпуск ([Код сотрудника], [Код вида отпуска], [Дата начала], [Дата окончания]) VALUES ('{0}', '{1}', '{2:yyyy.MM.dd}', '{3:yyyy.MM.dd}')", id.Text, sls1, dateTimePicker1.Value, dateTimePicker2.Value); // Cтрока передачи данных

                my_conn = new SqlConnection(form1.connectionString); //Создаем соеденение

                my_command = new SqlCommand(commandText, my_conn);

                my_conn.Open(); // Открытие соединения с базой данных

                my_command.ExecuteNonQuery(); // sql возвращает сколько строк обработано

                MessageBox.Show("Операция выполнена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Вывод сообщения о добавлении
                
                my_conn.Close();

                Loading();
            }
        }
    }
}
