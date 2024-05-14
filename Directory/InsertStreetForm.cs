using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Directory
{
    public partial class InsertStreetForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected TextBox nameTextBox;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertStreetForm()
        {

            _context = new();
            InitializeDynamicControls();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
        protected void InitializeDynamicControls()
        {
            // Создание и настройка элементов управления
            nameTextBox = new TextBox { Location = new Point(10, 30), Width = 200 };
            addButton = new Button { Text = "Add", Location = new Point(10, 130), Width = 210 };
            errorProvider = new ErrorProvider();

            // Labels
            var nameLabel = new Label { Text = "Name:", Location = new Point(10, 10) };

            // Добавление элементов управления и меток на форму
            Controls.Add(nameLabel);
            Controls.Add(nameTextBox);

            Controls.Add(addButton);

            // Привязка события для кнопки добавления
            addButton.Click += addButton_Click;
        }


        protected void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    var street = new DB.Street
                    {
                        StreetName = nameTextBox.Text,
                    };

                    _context.Streets.Add(street);
                    _context.SaveChanges();
                    MessageBox.Show("Улица успешно добавлена");
                    ClearForm();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении улицы. Проверьте корректность введенных данных");
            }
        }

        private bool IsValidInput()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Name is required");
                return false;
            }

            return isValid;
        }



        private void ClearForm()
        {
            nameTextBox.Clear();
        }
    }
}
