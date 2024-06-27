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
    public partial class Admin : Form
    {
        DataSet ds, ds1, ds2;
        SqlConnection my_conn;
        SqlDataAdapter my_data;
        SqlCommand my_command;

        Form1 form1 = new Form1();

        public int n;
        public string di, FIO_employee;

        public Admin()
        {
            InitializeComponent();

            Loading();

            Loading_1();

            Loading_2();
        }
        //////////////////////////////////////////////////// ТАБЛИЦА ДАННЫЕ О СОТРУДНИКЕ///////////////////////////////////////////////////////////////////////////////////////////////// 
        public void Loading()
        {
            // Настройки таблицы

            table.ColumnHeadersDefaultCellStyle.Font = new Font(table.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold); // Шрифт и размер названий столбцов
            table.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // выравнивание названий столбцов по центру

            table.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// выравнивание строк по центру
            table.DefaultCellStyle.Font = new Font("Times New Roman", 12); // Шрифт и размер строк

            ds = new DataSet(); //Создаем объект класса DataSet

            my_conn = new SqlConnection(form1.connectionString); //Создаем соеденение

            string sql = "Select p.[Код сотрудника], p.ФИО as [ФИО сотрудника], p.[Дата рождения], p.Адрес, p.[Номер телефона], u.Должность, y.[Семейное положение], p.[Серия номер паспорта], p.[Выдача паспорта], p.[Дата выдачи] " +
                "FROM Сотрудник as p JOIN Должность as u ON u.[Код должности] = p.[Код должности] JOIN [Семейное положение] as y ON y.[Код положения] = p.[Код положения] ORDER BY [Код сотрудника] ASC"; //Sql запрос (достать все из таблицы ...)

            my_data = new SqlDataAdapter(sql, my_conn);//Создаем объект класса DataAdapter (тут мы передаем наш запрос и получаем ответ)

            my_data.Fill(ds, "Сотрудник");//Заполняем DataSet cодержимым DataAdapter'a

            table.DataSource = ds.Tables[0].DefaultView;//Заполняем созданный на форме dataGridView1
        }

        //////////////////////////////////////////////////// ТАБЛИЦА ОБРАЗОВАНИЕ /////////////////////////////////////////////////////////////////////////////////////////////////
        public void Loading_1()
        {
            // Настройки таблицы

            table_1.ColumnHeadersDefaultCellStyle.Font = new Font(table_1.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold); // Шрифт и размер названий столбцов
            table_1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // выравнивание названий столбцов по центру

            table_1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// выравнивание строк по центру
            table_1.DefaultCellStyle.Font = new Font("Times New Roman", 12); // Шрифт и размер строк

            ds1 = new DataSet(); //Создаем объект класса DataSet

            my_conn = new SqlConnection(form1.connectionString); //Создаем соеденение

            string sql = "Select u.ФИО as [ФИО сотрудника], p.[Учебное заведение], p.Диплом, p.[Год окончания], p.Квалификация " +
                "FROM Образование as p JOIN Сотрудник as u ON u.[Код сотрудника] = p.[Код сотрудника] ORDER BY u.[Код сотрудника] ASC"; //Sql запрос (достать все из таблицы ...)

            my_data = new SqlDataAdapter(sql, my_conn);//Создаем объект класса DataAdapter (тут мы передаем наш запрос и получаем ответ)

            my_data.Fill(ds1, "Образование");//Заполняем DataSet cодержимым DataAdapter'a

            table_1.DataSource = ds1.Tables[0].DefaultView;//Заполняем созданный на форме dataGridView1
        }

        //////////////////////////////////////////////////// ТАБЛИЦА СЕМЬЯ /////////////////////////////////////////////////////////////////////////////////////////////////
        public void Loading_2()
        {
            // Настройки таблицы

            table_2.ColumnHeadersDefaultCellStyle.Font = new Font(table_2.ColumnHeadersDefaultCellStyle.Font.FontFamily, 12, FontStyle.Bold); // Шрифт и размер названий столбцов
            table_2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // выравнивание названий столбцов по центру

            table_2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;// выравнивание строк по центру
            table_2.DefaultCellStyle.Font = new Font("Times New Roman", 12); // Шрифт и размер строк

            ds2 = new DataSet(); //Создаем объект класса DataSet

            my_conn = new SqlConnection(form1.connectionString); //Создаем соеденение

            string sql = "Select u.ФИО as [ФИО сотрудника], p.ФИО as [ФИО супруга/супруги], p.[Дата рождения], p.[Количество детей] " +
                "FROM Семья as p JOIN Сотрудник as u ON u.[Код сотрудника] = p.[Код сотрудника] ORDER BY u.[Код сотрудника] ASC"; //Sql запрос (достать все из таблицы ...)

            my_data = new SqlDataAdapter(sql, my_conn);//Создаем объект класса DataAdapter (тут мы передаем наш запрос и получаем ответ)

            my_data.Fill(ds2, "Семья");//Заполняем DataSet cодержимым DataAdapter'a

            table_2.DataSource = ds2.Tables[0].DefaultView;//Заполняем созданный на форме dataGridView1
        }
        // Открыть форму с добавлением сотрудников
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            n = 0;

            di = "";
            Employee employee = new Employee(n, di); // Открытие новой формы
            employee.Owner = this;
            employee.ShowDialog();
        }
        // Автоматическая настройка таблиц по ширине
        private void Admin_Load(object sender, EventArgs e)
        {
            table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // Автоматическая высота столбцов
            table.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            table_1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // Автоматическая высота столбцов
            table_1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            table_2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // Автоматическая высота столбцов
            table_2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }
        // Завершение работы программы
        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        // Удалить сотрудника из базы данных (в дальнейшей разработке)
        // Поиск сотрудника по фамилии
        private void search_TextChanged(object sender, EventArgs e)
        {
            ds.Tables[0].DefaultView.RowFilter = "[ФИО сотрудника] LIKE '" + search.Text + "%'"; // Критерий поиска

            ds1.Tables[0].DefaultView.RowFilter = "[ФИО сотрудника] LIKE '" + search.Text + "%'"; // Критерий поиска

            ds2.Tables[0].DefaultView.RowFilter = "[ФИО сотрудника] LIKE '" + search.Text + "%'"; // Критерий поиска
        }
        // Открыть форму вакансии
        private void вакансииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Job_openings job_openings = new Job_openings(); // Открытие новой формы
            job_openings.Owner = this;
            job_openings.ShowDialog();
        }
       
       

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа предназначена для кадрового учета транспортной компании!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Вывод сообщения
        }
        // Открыть личную карточку (В дальнейшей разработке)
        // Открытие формы отпуск
        private void отпускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int a = table.CurrentRow.Index; // Выделенная строка в табли

            di = Convert.ToString(table.Rows[a].Cells[0].Value);
            FIO_employee = Convert.ToString(table.Rows[a].Cells[1].Value);

            Vacation vacation = new Vacation(di, FIO_employee); // Открытие новой формы
            vacation.Owner = this;
            vacation.ShowDialog();
        }

        // Открыть окно с редактированием
        private void РедактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            n = 1;

            int a = table.CurrentRow.Index; // Выделенная строка в таблице

            di = Convert.ToString(table.Rows[a].Cells[0].Value);

            Employee employee = new Employee(n, di); // Открытие новой формы
            employee.Owner = this;
            employee.ShowDialog();
        }
        // Кнопка обновить
        private void update_Click(object sender, EventArgs e)
        {
            Loading();

            Loading_1();

            Loading_2();
        }
    }
}
