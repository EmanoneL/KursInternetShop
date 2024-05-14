using DB;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Customer
{
    public partial class InsertCustomerForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected ComboBox bankComboBox;
        protected ComboBox addressComboBox;
        protected TextBox nameTextBox;
        protected TextBox emailTextBox;
        protected TextBox loginTextBox;
        protected TextBox passwordTextBox;
        protected TextBox phoneTextBox;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertCustomerForm()
        {

            _context = new();
            InitializeDynamicControls();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            LoadComboBoxes();
        }

        protected void LoadComboBoxes()
        {
            // Загрузка данных для ComboBox
            LoadBank();
            LoadAddress();
        }
        protected void InitializeDynamicControls()
        {
            // Создание и настройка элементов управления
            nameTextBox = new TextBox { Location = new Point(10, 30), Width = 200 };
            emailTextBox = new TextBox { Location = new Point(10, 80), Width = 200 };
            phoneTextBox = new TextBox { Location = new Point(10, 130), Width = 200 };
            loginTextBox = new TextBox { Location = new Point(10, 180), Width = 200 };
            passwordTextBox = new TextBox { Location = new Point(10, 230), Width = 200 };
            bankComboBox = new ComboBox { Location = new Point(10, 280), Width = 200 };
            addressComboBox = new ComboBox { Location = new Point(10, 330), Width = 200 };
            addButton = new Button { Text = "Add", Location = new Point(10, 380), Width = 210 };
            errorProvider = new ErrorProvider();

            // Labels
            var nameLabel = new Label { Text = "Name:", Location = new Point(10, 10) };
            var emailLabel = new Label { Text = "Email:", Location = new Point(10, 60) };
            var phoneLabel = new Label { Text = "Phone:", Location = new Point(10, 110) };
            var loginLabel = new Label { Text = "Login:", Location = new Point(10, 160) };
            var passwordLabel = new Label { Text = "Password:", Location = new Point(10, 210) };
            var bankLabel = new Label { Text = "Bank:", Location = new Point(10, 260) };
            var addressLabel = new Label { Text = "Address:", Location = new Point(10, 310) };



            // Добавление элементов управления и меток на форму
            Controls.Add(nameLabel);
            Controls.Add(nameTextBox);
            Controls.Add(emailLabel);
            Controls.Add(emailTextBox);
            Controls.Add(phoneLabel);
            Controls.Add(phoneTextBox);
            Controls.Add(loginLabel);
            Controls.Add(loginTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(passwordTextBox);
            Controls.Add(bankLabel);
            Controls.Add(bankComboBox);
            Controls.Add(addressLabel);
            Controls.Add(addressComboBox);
            Controls.Add(addButton);


            // Привязка события для кнопки добавления
            addButton.Click += addButton_Click;
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
        private void LoadBank()
        {
            var banks = _context.Banks.ToList();
            bankComboBox.DataSource = banks;
            bankComboBox.DisplayMember = "name";
            bankComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }



        protected void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    var customer = new DB.Customer
                    {
                        Name = nameTextBox.Text,
                        IdBank = ((Bank)bankComboBox.SelectedItem).IdBank,
                        Email = emailTextBox.Text,
                        PhoneNumber = int.Parse(phoneTextBox.Text),
                        IdAddress = ((AddressDisplayItem)addressComboBox.SelectedItem).Id,
                        Login = loginTextBox.Text,
                        Password = passwordTextBox.Text
                    };

                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                    MessageBox.Show("Покупатель успешно добавлен");
                    ClearForm();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении покупателя. Проверьте корректность введенных данных");
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
            if (bankComboBox.SelectedItem == null)
            {
                errorProvider.SetError(bankComboBox, "Bank is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(emailTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Email is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phoneTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Phone Number is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(loginTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Login is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Password is required");
                return false;
            }
            if (addressComboBox.SelectedItem == null)
            {
                errorProvider.SetError(bankComboBox, "Address is required");
                return false;
            }


            return isValid;
        }



        private void ClearForm()
        {
            nameTextBox.Clear();
            emailTextBox.Clear();
            phoneTextBox.Clear();
            loginTextBox.Clear();
            passwordTextBox.Clear();
            bankComboBox.SelectedIndex = -1;
            addressComboBox.SelectedItem = -1;
        }
    }
}
