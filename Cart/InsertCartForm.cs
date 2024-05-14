using DB;
using Microsoft.EntityFrameworkCore;

namespace Cart
{
    public partial class InsertCartForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected ComboBox productComboBox;
        protected ComboBox cartComboBox;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertCartForm()
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
            LoadProducts();
            LoadCustomers();
        }
        protected void InitializeDynamicControls()
        {
            // Создание и настройка элементов управления
            //nameTextBox = new TextBox { Location = new Point(10, 30), Width = 200 };
            //descriptionTextBox = new TextBox { Location = new Point(10, 80), Width = 200 };

            cartComboBox = new ComboBox { Location = new Point(10, 30), Width = 200 };
            productComboBox = new ComboBox { Location = new Point(10, 80), Width = 200 };

            addButton = new Button { Text = "Add", Location = new Point(10, 130), Width = 200 };
            errorProvider = new ErrorProvider();

            // Labels
            var cartLabel = new Label { Text = "Покупатель:", Location = new Point(10, 10) };
            var productLabel = new Label { Text = "Товар:", Location = new Point(10, 60) };



            // Добавление элементов управления и меток на форму
            Controls.Add(cartLabel);
            Controls.Add(cartComboBox);
            Controls.Add(productLabel);
            Controls.Add(productComboBox);
            Controls.Add(addButton);

            // Привязка события для кнопки добавления
            addButton.Click += addButton_Click;
        }


        private void LoadProducts()
        {
            // Загрузка продуктов в ComboBox
            var products = _context.Products.ToList();
            productComboBox.DataSource = products;
            productComboBox.DisplayMember = "Name";
            productComboBox.ValueMember = "IdProducts";
            productComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadCustomers()
        {
            // Загрузка продуктов в ComboBox
            var customers = _context.Customers.Include(u => u.Carts).ToList();
            cartComboBox.DataSource = customers;
            cartComboBox.DisplayMember = "Name";
            cartComboBox.ValueMember = "IdCustomers";
            cartComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void LoadCarts()
        {
            // Загрузка продуктов в ComboBox
            var products = _context.Carts.ToList();
            cartComboBox.DataSource = products;
            cartComboBox.DisplayMember = "idCarts";
            cartComboBox.ValueMember = "IdCarts";
            cartComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }



        protected void addButton_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                Product selectedProduct = (Product)productComboBox.SelectedItem;
                Customer currentCustomer = ((Customer)cartComboBox.SelectedItem);
                DB.Cart cart = currentCustomer.Carts.First();
                //ICollection<DB.Cart> customerCart = currentCusomer.Carts;



                //// Проверка наличия выбранного продукта в корзине
                if (!_context.CartsHasProsucts.Any(cp => cp.IdProducts == selectedProduct.IdProducts && cp.IdCarts == cart.IdCarts))
                {
                    var newRecord = new CartsHasProsucts
                    {
                        IdCarts = cart.IdCarts,
                        IdProducts = selectedProduct.IdProducts
                    };

                    _context.Set<CartsHasProsucts>().Add(newRecord);
                    _context.SaveChanges();
                    Close();
                    //cart.Products.Add(selectedProduct);
                }
                else
                {
                    MessageBox.Show("Продукт уже добавлен в корзину!");
                }

                //    _context.Set<CartsHasProsuct>().Add(cartProduct);
                //    _context.SaveChanges();

            }




        }




        private bool IsValidInput()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(cartComboBox.Text))
            {
                errorProvider.SetError(cartComboBox, "Cart is required");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(productComboBox.Text))
            {
                errorProvider.SetError(cartComboBox, "Product is required");
                isValid = false;
            }



            return isValid;
        }



        private void ClearForm()
        {
            productComboBox.SelectedIndex = -1;
            cartComboBox.SelectedIndex = -1;
        }
    }



}
