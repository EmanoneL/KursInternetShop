using DB;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Order
{
    public partial class EditOrderForm : InsertOrderForm
    {
        private readonly DB.Order _order;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditOrderForm(DB.Order order) : base()
        {
            InitializeComponent();
            base.LoadComboBoxes();
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {

            int delivertId = _order?.IdDeliveryCompany ?? 0; // Handle null values
            int customerId = _order?.IdCustomer ?? 0; // Handle null values
            int addressId = _order?.IdAddress ?? 0; // Handle null values

            var delcomm = _context.DeliveryCompanies.ToList();
            deliveryComComboBox.DataSource = delcomm;

            var cust = _context.Customers.ToList();
            customerComboBox.DataSource = cust;

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

            addressComboBox.DataSource = addressDisplayItems;


            deliveryComComboBox.SelectedItem = delcomm.FirstOrDefault(c => c.IdDeliveryCompany == delivertId);
            customerComboBox.SelectedItem = cust.FirstOrDefault(c => c.IdCustomers == customerId);
            statusComboBox.SelectedItem = (_order.Status);
            addressComboBox.SelectedItem = addressDisplayItems.FirstOrDefault(c => c.Id == addressId);
            statusComboBox.Enabled = true;


            this.Text = "Edit Order";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных продукта
            _order.IdDeliveryCompany = ((DeliveryCompany)deliveryComComboBox.SelectedItem).IdDeliveryCompany;
            _order.IdCustomer = ((Customer)customerComboBox.SelectedItem).IdCustomers;
            _order.Status = statusComboBox.SelectedItem.ToString();
            _order.IdAddress = ((AddressDisplayItem)addressComboBox.SelectedItem).Id;

            _context.Update(_order);
            _context.SaveChanges();
            MessageBox.Show("Product updated successfully!");
            this.Close();
        }

    }
}
