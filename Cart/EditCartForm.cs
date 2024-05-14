using Microsoft.EntityFrameworkCore;

namespace Cart
{
    public partial class EditCartForm : InsertCartForm
    {
        private readonly DB.CartsHasProsucts _cartprod;
        private readonly DB.HandmadeShopSystemContext _context;
        public EditCartForm(DB.CartsHasProsucts cartprod) : base()
        {
            InitializeComponent();
            base.LoadComboBoxes();
            _cartprod = cartprod ?? throw new ArgumentNullException(nameof(cartprod));
            _context = new();
            _context.CartsHasProsucts.Remove(cartprod);
            _context.SaveChanges();
            InitializeForm();
        }

        private void InitializeForm()
        {
            int customerId = _cartprod?.IdCartsNavigation?.IdCustomers ?? 0; // Handle null values
            int productId = _cartprod?.IdProductsNavigation?.IdProducts ?? 0; // Handle null values

            var products = _context.Products.ToList();
            productComboBox.DataSource = products;

            var customers = _context.Customers.Include(u => u.Carts).ToList();
            cartComboBox.DataSource = customers;


            var selectedCustomer = customers.FirstOrDefault(c => c.IdCustomers == customerId);
            var selectedProduct = products.FirstOrDefault(p => p.IdProducts == productId);

            cartComboBox.SelectedItem = selectedCustomer;
            productComboBox.SelectedItem = selectedProduct;



            this.Text = "Edit Cart";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            addButton_Click(sender, e);


            MessageBox.Show("Cart updated successfully!");
            this.Close();
        }
    }
}
