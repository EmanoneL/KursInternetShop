using DB;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Order
{
    public partial class InsertOrderForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected ComboBox deliveryComComboBox;
        protected ComboBox customerComboBox;
        protected ComboBox statusComboBox;
        protected ComboBox addressComboBox;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertOrderForm()
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
            LoadStatus();
            LoadAddress();
            LoadDelivery();
            LoadCustomer();
        }
        protected void InitializeDynamicControls()
        {
            // Создание и настройка элементов управления
            deliveryComComboBox = new ComboBox { Location = new Point(10, 30), Width = 200 };
            customerComboBox = new ComboBox { Location = new Point(10, 80), Width = 200 };
            statusComboBox = new ComboBox { Location = new Point(10, 130), Width = 200 };
            addressComboBox = new ComboBox { Location = new Point(10, 180), Width = 200 };
            addButton = new Button { Text = "Add", Location = new Point(10, 230), Width = 220 };
            errorProvider = new ErrorProvider();

            // Labels
            var delLabel = new Label { Text = "Delivery Company:", Location = new Point(10, 10) };
            var customerLabel = new Label { Text = "Customer:", Location = new Point(10, 60) };
            var statusLabel = new Label { Text = "Status:", Location = new Point(10, 110) };
            var addressLabel = new Label { Text = "Delivery address:", Location = new Point(10, 160) };

            // Добавление элементов управления и меток на форму
            Controls.Add(deliveryComComboBox);
            Controls.Add(customerComboBox);
            Controls.Add(statusComboBox);
            Controls.Add(addressComboBox);
            Controls.Add(delLabel);
            Controls.Add(customerLabel);
            Controls.Add(statusLabel);
            Controls.Add(addressLabel);
            Controls.Add(addButton);

            // Привязка события для кнопки добавления
            addButton.Click += addButton_Click;
        }

        private void LoadStatus()
        {
            var statuses = new List<string> { "in process", "delivered", "lost" };
            statusComboBox.DataSource = statuses;
            statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statusComboBox.SelectedIndex = 0;
            statusComboBox.Enabled = false;



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
        private void LoadDelivery()
        {
            var companies = _context.DeliveryCompanies.ToList();
            deliveryComComboBox.DataSource = companies;
            deliveryComComboBox.DisplayMember = "name";
            deliveryComComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void LoadCustomer()
        {
            var customers = _context.Customers.Include(c => c.Carts).ToList();
            customerComboBox.DataSource = customers;
            customerComboBox.DisplayMember = "name"; // Замените на имя свойства в вашем классе Seller
            customerComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }


        protected void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    Customer customer = (Customer)customerComboBox.SelectedItem;
                    Cart cart = customer.Carts.FirstOrDefault();
                    var products = _context.CartsHasProsucts
                        .Where(chp => chp.IdCarts == cart.IdCarts)
                        .Select(chp => chp.IdProductsNavigation)
                        .ToList();

                    foreach (var product in products)
                    {
                        var order = new DB.Order
                        {
                            IdDeliveryCompany = ((DeliveryCompany)deliveryComComboBox.SelectedItem).IdDeliveryCompany,
                            IdCustomer = customer.IdCustomers,
                            IdProducts = product.IdProducts,
                            ProductCount = (int)cart.Count,
                            TotalCost = (int)cart.Cost,
                            Status = statusComboBox.SelectedItem.ToString(),
                            IdAddress = ((AddressDisplayItem)addressComboBox.SelectedItem).Id,
                            RegistrationDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                        };
                        _context.Orders.Add(order);
                    }
                    _context.SaveChanges();
                    ClearCart(cart);
                    MessageBox.Show("Заказ успешно создан");
                    ClearForm();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении заказа. Проверьте корректность введенных данных");
            }
        }

        private void ClearCart(Cart cart)
        {
            var cartProducts = _context.CartsHasProsucts
            .Where(chp => chp.IdCarts == cart.IdCarts)
            .ToList();

            // Удалить все записи CartHasProducts из контекста данных
            _context.CartsHasProsucts.RemoveRange(cartProducts);

            // Сохранить изменения
            _context.SaveChanges();
        }

        private bool IsValidInput()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (deliveryComComboBox.SelectedItem == null)
            {
                errorProvider.SetError(deliveryComComboBox, "Delivery Company is required");
                return false;
            }
            if (customerComboBox.SelectedItem == null)
            {
                errorProvider.SetError(customerComboBox, "Customer is required");
                return false;
            }
            if (statusComboBox.SelectedItem == null)
            {
                errorProvider.SetError(statusComboBox, "Status is required");
                return false;
            }

            if (addressComboBox.SelectedItem == null)
            {
                errorProvider.SetError(statusComboBox, "Address is required");
                return false;
            }


            if (IsCartEmpty())
            {
                errorProvider.SetError(customerComboBox, "Cart is empty");
                return false;
            }


            return isValid;
        }

        private bool IsCartEmpty()
        {
            Customer customer = (Customer)customerComboBox.SelectedItem;
            Cart cart = customer.Carts.FirstOrDefault();
            var products = _context.CartsHasProsucts
                .Where(chp => chp.IdCarts == cart.IdCarts)
                .Select(chp => chp.IdProductsNavigation)
                .ToList();

            return products.Count == 0;
        }

        private void ClearForm()
        {

            statusComboBox.SelectedIndex = 0;
            deliveryComComboBox.SelectedIndex = -1;
            customerComboBox.SelectedIndex = -1;
            addressComboBox.SelectedIndex = -1;
        }
    }
}

