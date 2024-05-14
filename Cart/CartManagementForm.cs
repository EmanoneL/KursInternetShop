using DB;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cart
{
    public partial class CartManagementForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        private BindingSource bindingSource;
        private Right currentUserRight { get; set; }
        public CartManagementForm(Right currentUserRight)
        {
            this.currentUserRight = currentUserRight;
            InitializeComponent();
            _context = new();
            InitializeForm();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        }

        private void InitializeForm()
        {

            // Инициализация элементов управления
            bindingSource = new BindingSource();
            dataGridView1.DataSource = bindingSource;
            LoadCarts();

            // Кнопки
            var editButton = new Button { Text = "Edit", Location = new System.Drawing.Point(10, 110) };
            editButton.Click += EditButton_Click;

            var deleteButton = new Button { Text = "Delete", Location = new System.Drawing.Point(10, 160) };
            deleteButton.Click += DeleteButton_Click;

            var createButton = new Button { Text = "Create", Location = new System.Drawing.Point(10, 210) };
            createButton.Click += CreateButton_Click;

            Controls.Add(editButton);
            Controls.Add(deleteButton);
            Controls.Add(createButton);

            if (currentUserRight != null)
            {
                if (currentUserRight.Rd == 0)
                {
                    dataGridView1.Visible = false;
                }

                if (currentUserRight.Write == 0)
                {
                    createButton.Visible = false;
                }

                if (currentUserRight.Edit == 0)
                {
                    editButton.Visible = false;
                }

                if (currentUserRight.Del == 0)
                {
                    deleteButton.Visible = false;
                }
            }
        }


        private void LoadCarts()
        {


            var carts = _context.CartsHasProsucts
                .Select(cp => new
                {
                    Customer = cp.IdCartsNavigation.IdCustomersNavigation.Name,
                    Product = cp.IdProductsNavigation.Name,
                    cp.IdCartsNavigation.Count,
                    cp.IdCartsNavigation.Cost

                }).OrderBy(cart => cart.Customer)
                .ToList();


            bindingSource.DataSource = carts;

        }




        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedProductName = (string)dataGridView1.SelectedRows[0].Cells["Product"].Value;
                    var selectedCustomerName = (string)dataGridView1.SelectedRows[0].Cells["Customer"].Value;
                    var selectedProductId = _context.Products.FirstOrDefault(p => p.Name == selectedProductName).IdProducts;
                    var selectedCartId = _context.Customers.Include(c => c.Carts).FirstOrDefault(c => c.Name == selectedCustomerName);
                    var cart = selectedCartId.Carts.FirstOrDefault().IdCarts;
                    var cartprod = _context.CartsHasProsucts.Find(cart, selectedProductId);
                    var editForm = new EditCartForm(cartprod);
                    editForm.ShowDialog();
                    LoadCarts();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private bool IsPossibeToDeleteProduct(int selectedProductId)
        //{

        //    // Check for referencing Orders
        //    var hasOrders = _context.Orders.Any(o => o.IdProducts == selectedProductId);

        //    // Check for entries in CartsHasProsuct (assuming idProducts is the foreign key)
        //    var hasCartReferences = _context.CartsHasProsucts.Any(c => c.IdProducts == selectedProductId);

        //    if (hasOrders || hasCartReferences)
        //    {
        //        return false;
        //    }
        //    return true;

        //}
        private void DeleteButton_Click(object sender, EventArgs e)
        {


            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedProductName = (string)dataGridView1.SelectedRows[0].Cells["Product"].Value;
                var selectedCustomerName = (string)dataGridView1.SelectedRows[0].Cells["Customer"].Value;

                var selectedProductId = _context.Products.FirstOrDefault(p => p.Name == selectedProductName).IdProducts;
                var selectedCartId = _context.Customers.Include(c => c.Carts).FirstOrDefault(c => c.Name == selectedCustomerName);
                var cart = selectedCartId.Carts.FirstOrDefault().IdCarts;
                var cartprod = _context.CartsHasProsucts.Find(cart, selectedProductId);
                _context.CartsHasProsucts.Remove(cartprod);
                _context.SaveChanges();
                LoadCarts();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertCartForm();
            createForm.ShowDialog();
            LoadCarts();
        }



        public static void ShowForm(DB.Right rights)
        {
            CartManagementForm form = new(rights);
            form.Show();
        }
    }
}

