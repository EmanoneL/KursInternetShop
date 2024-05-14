using DB;
using System.Data;

namespace Customer
{
    public partial class EditCustomerForm : InsertCustomerForm
    {
        private readonly DB.Customer _customer;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditCustomerForm(DB.Customer customer) : base()
        {
            InitializeComponent();
            base.LoadComboBoxes();
            _customer = customer ?? throw new ArgumentNullException(nameof(customer));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {
            var addressDisplayItems = _context.Addresses.Select(s => new AddressDisplayItem
            {
                Id = s.IdAddress,
                AddressString = s.CityNavigation != null && s.StreetNavigation != null
                    ? $"{s.CityNavigation.CityName}, {s.StreetNavigation.StreetName}, {s.HomeNumber}"
                    : "Unknown Address"
            }).ToList();

            var banks = _context.Banks.ToList();
            bankComboBox.DataSource = banks;

            addressComboBox.DataSource = addressDisplayItems;

            // Загрузка данных выбранного продукта на форму

            nameTextBox.Text = _customer.Name;
            emailTextBox.Text = _customer.Email;
            phoneTextBox.Text = _customer.PhoneNumber.ToString();
            loginTextBox.Text = _customer.Login;
            passwordTextBox.Text = _customer.Login;
            bankComboBox.SelectedItem = banks.FirstOrDefault(c => c.IdBank == _customer.IdBank);

            addressComboBox.SelectedItem = addressDisplayItems.FirstOrDefault(c => c.Id == _customer.IdAddress);



            this.Text = "Edit Customer";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных продукта
            _customer.Name = nameTextBox.Text;
            _customer.Email = emailTextBox.Text;
            _customer.PhoneNumber = long.Parse(phoneTextBox.Text);
            _customer.IdBankNavigation = (Bank)bankComboBox.SelectedItem;
            _customer.IdAddress = ((AddressDisplayItem)addressComboBox.SelectedItem).Id;


            _context.Update(_customer);
            _context.SaveChanges();
            MessageBox.Show("Customer updated successfully!");
            this.Close();
        }
    }
}
