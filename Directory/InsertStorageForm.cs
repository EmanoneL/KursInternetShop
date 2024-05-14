using DB;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Directory
{
    public partial class InsertStorageForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected ComboBox addressComboBox;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertStorageForm()
        {

            _context = new();
            InitializeDynamicControls();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            LoadAddress();
        }
        private void LoadAddress()
        {
            var addresses = _context.Addresses
                  .Include(a => a.CityNavigation)
                  .Include(a => a.StreetNavigation)
              .ToList();

            var addressDisplayItems = addresses.Select(s => new AddressDisplayItem
            {
                Id = s.IdAddress,
                AddressString = s.CityNavigation != null && s.StreetNavigation != null
                    ? $"{s.CityNavigation.CityName}, {s.StreetNavigation.StreetName}, {s.HomeNumber}"
                    : "Unknown Address"
            }).ToList();

            addressComboBox.DisplayMember = "AddressString";
            addressComboBox.ValueMember = "Id";
            addressComboBox.DataSource = addressDisplayItems;
            addressComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }
        protected void InitializeDynamicControls()
        {
            // Создание и настройка элементов управления
            addressComboBox = new ComboBox { Location = new Point(10, 30), Width = 200 };
            addButton = new Button { Text = "Add", Location = new Point(10, 80), Width = 210 };
            errorProvider = new ErrorProvider();

            // Labels
            var addressLabel = new Label { Text = "Address:", Location = new Point(10, 10) };

            // Добавление элементов управления и меток на форму
            Controls.Add(addressLabel);
            Controls.Add(addressComboBox);

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
                    var storage = new DB.Storage
                    {
                        IdAddress = ((AddressDisplayItem)addressComboBox.SelectedItem).Id,
                    };

                    _context.Storages.Add(storage);
                    _context.SaveChanges();
                    MessageBox.Show("Склад успешно добавлен");
                    ClearForm();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении склада. Проверьте корректность введенных данных");
            }
        }

        private bool IsValidInput()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (addressComboBox.SelectedItem == null)
            {
                errorProvider.SetError(addressComboBox, "Address is required");
                return false;
            }



            return isValid;
        }



        private void ClearForm()
        {
            addressComboBox.SelectedItem = null;
        }
    }
}
