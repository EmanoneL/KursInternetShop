using DB;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Products
{
    public partial class ProductManagementForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        private BindingSource bindingSource;
        private Right currentUserRight { get; set; }

        public ProductManagementForm(Right currentUserRight)
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
            LoadProducts();

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


        private void LoadProducts()
        {
            var products = _context.Products
                .Select(p => new
                {
                    p.IdProducts,
                    p.Name,
                    p.Description,
                    Picture = ResizeImageFromByteArray(p.Picture, new Size(100, 100)),
                    p.Cost,
                    p.Status,
                    CategoryName = p.IdCategoryNavigation.Name,
                    Seller = p.IdSellersNavigation.Name,
                    Storage = $"{p.IdStoragesNavigation.IdAddressNavigation.CityNavigation.CityName}, " +
                    $"{p.IdStoragesNavigation.IdAddressNavigation.StreetNavigation.StreetName}, " +
                    $"{p.IdStoragesNavigation.IdAddressNavigation.HomeNumber}"
                })
                .ToList();

            bindingSource.DataSource = products;

            //LoadDataGrid();
        }



        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedProductId = (int)dataGridView1.SelectedRows[0].Cells["idProducts"].Value;

                    var selectedProduct = _context.Products.Find(selectedProductId);
                    var editForm = new EditProductForm(selectedProduct);
                    editForm.ShowDialog();
                    LoadProducts();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsPossibeToDeleteProduct(int selectedProductId)
        {

            // Check for referencing Orders
            var hasOrders = _context.Orders.Any(o => o.IdProducts == selectedProductId);

            // Check for entries in CartsHasProsuct (assuming idProducts is the foreign key)
            var hasCartReferences = _context.CartsHasProsucts.Any(c => c.IdProducts == selectedProductId);

            if (hasOrders || hasCartReferences)
            {
                return false;
            }
            return true;

        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedProductId = (int)dataGridView1.SelectedRows[0].Cells["idProducts"].Value;

                if (!IsPossibeToDeleteProduct(selectedProductId))
                {
                    // Inform the user about referencing records
                    MessageBox.Show("This product cannot be deleted because it has associated orders or cart entries. Delete them first and try again.");
                    return;
                }

                // No referencing records, proceed with deletion
                var selectedProduct = _context.Products.Find(selectedProductId);
                _context.Products.Remove(selectedProduct);
                _context.SaveChanges();
                LoadProducts();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertProductForm();
            createForm.ShowDialog();
            LoadProducts();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }
        private static Image ResizeImageFromByteArray(byte[] imageData, Size size)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (MemoryStream ms = new MemoryStream(imageData))
            {
                Image img = Image.FromStream(ms);
                return new Bitmap(img, size);
            }
        }

        public static void ShowForm(DB.Right rights)
        {
            ProductManagementForm form = new(rights);
            form.Show();
        }
    }
}
