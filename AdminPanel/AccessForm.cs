using MenuItemConstruction;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AdminPanel
{
    public partial class AccessForm : Form
    {
        List<bool> checkboxStates = new List<bool>();
        string selectedUserID;
        string edtColomn;

        Dictionary<string, string> translationDictionary = new Dictionary<string, string>
        {
            { "Чтение", "rd" },
            { "Запись", "write" },
            { "Редактирование", "edit" },
            { "Удаление", "del" }
        };
        public AccessForm()
        {
            InitializeComponent();

        }

        private void LoadDataGrid()
        {
            MenuItemConstruction.MenuItemSettings.fillDataGrid(dataGridView1, @"SELECT id, login as Пользователь, item_name as Пункт_меню,
                                                                                CASE 
                                                                                    WHEN rd = 0 THEN 'нет'
                                                                                    ELSE 'да'
                                                                                END AS Чтение,
                                                                                CASE 
                                                                                    WHEN write = 0 THEN 'нет'
                                                                                    ELSE 'да'
                                                                                END AS Запись,
                                                                                CASE 
                                                                                    WHEN edit = 0 THEN 'нет'
                                                                                    ELSE 'да'
                                                                                END AS Редактирование,
                                                                                CASE 
                                                                                    WHEN del = 0 THEN 'нет'
                                                                                    ELSE 'да'
                                                                                END AS Удаление
                                                                            FROM rights 
                                                                            JOIN users ON user_id = users.id
                                                                            JOIN menu ON rights.menu_item_id = menu.menu_item_id
                                                                            WHERE item_name='Товары';
                                                                ");
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
        }

        private void LoadComboBox()
        {
            DB.DB db = new DB.DB();
            SqliteDataReader reader = db.SELECT(values: "item_name", FROM: "menu");
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["item_name"].ToString());
            }
            comboBox1.SelectedIndex = 0;
            db.Close();
        }
        private void AccessForm_Load(object sender, EventArgs e)
        {

            LoadDataGrid();
            LoadComboBox();

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0) // Убеждаемся, что это не заголовок столбца или строки
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Проверяем значение ячейки и устанавливаем цвет фона
                if (cell.Value != null && cell.Value.ToString() == "нет")
                {
                    cell.Style.BackColor = Color.IndianRed; // Устанавливаем красный цвет для значения "нет"
                    cell.Style.ForeColor = Color.White; // Устанавливаем белый цвет текста для контраста
                }
                else if (cell.Value != null && cell.Value.ToString() == "да")
                {
                    cell.Style.BackColor = Color.LightGreen; // Устанавливаем белый цвет фона для других значений
                    cell.Style.ForeColor = Color.Black; // Устанавливаем черный цвет текста
                }
            }
        }

        private void freshDataGrid()
        {
            string sqlex = @"SELECT id, login as Пользователь,
                            CASE 
                                WHEN rd = 0 THEN 'нет'
                                ELSE 'да'
                            END AS Чтение,
                            CASE 
                                WHEN write = 0 THEN 'нет'
                                ELSE 'да'
                            END AS Запись,
                            CASE 
                                WHEN edit = 0 THEN 'нет'
                                ELSE 'да'
                            END AS Редактирование,
                            CASE 
                                WHEN del = 0 THEN 'нет'
                                ELSE 'да'
                            END AS Удаление
                        FROM rights 
                        JOIN users ON user_id = users.id
                        JOIN menu ON rights.menu_item_id = menu.menu_item_id
                        WHERE item_name='" + comboBox1.Text + "';";

            dataGridView1.DataSource = null;
            MenuItemSettings.fillDataGrid(dataGridView1, sqlex);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            freshDataGrid();

        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                // Получение данных из выбранной строки
                selectedUserID = selectedRow.Cells["id"].Value.ToString();
                edtColomn = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText;

            }
        }

        private void changeAccess(string givenAccess)
        {
            try
            {
                string changingColomn = translationDictionary[edtColomn];
                if (edtColomn != null && selectedUserID != null)
                {
                    DB.DB db = new DB.DB();


                    string menuItemID = Convert.ToString(comboBox1.SelectedIndex + 1);

                    db.UPDATE("rights", $"{changingColomn} = {givenAccess}", $"user_id={selectedUserID} AND menu_item_id = {menuItemID}");
                    db.Close();
                }
            }
            catch
            {
                MessageBox.Show("Неверно выбранные данные для изменений");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            changeAccess("1");
            freshDataGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            changeAccess("0");
            freshDataGrid();
        }
    }
}
