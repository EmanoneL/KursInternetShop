using DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Products
{
    public partial class ProductManagementForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        private BindingSource bindingSource;

        public ProductManagementForm()
        {
            InitializeComponent();
            _context = new(); 
            InitializeForm();
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
        }

        private void LoadProducts()
        {
            var products = _context.Products
                .Include(p => p.IdCategoryNavigation)
                .Include(p => p.IdSellersNavigation)
                .Include(p => p.IdStoragesNavigation.IdAddressNavigation.CityNavigation)
                .Include(p => p.IdStoragesNavigation.IdAddressNavigation.StreetNavigation)
                .ToList();

            bindingSource.DataSource = products;

            LoadDataGrid();
        }

        public void LoadDataGrid()
        {
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdProducts",
                HeaderText = "idProducts",
                Width = 50
            });

            // Столбец Name
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Столбец Description
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Столбец Picture
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
            {
                DataPropertyName = "Picture",
                HeaderText = "Picture",
                Width = 100,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dataGridView1.Columns.Add(imageColumn);

            // Столбец Cost
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cost",
                HeaderText = "Cost",
                Width = 70
            });

            // Столбец Status
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Status",
                HeaderText = "Status",
                Width = 70
            });

            // Столбец Category
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdCategoryNavigation.Name",
                HeaderText = "Category",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Столбец Seller
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdSellersNavigation.Name",
                HeaderText = "Seller",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Столбец Address
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AddressString",
                HeaderText = "Address",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Настройка высоты строк по высоте изображения
            dataGridView1.RowTemplate.Height = 100; // Высота строки

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var product = (Product)row.DataBoundItem;
                if (product != null && product.Picture != null && product.Picture.Length > 0)
                {
                    row.Height = 100; // Высота строки
                }
            }

        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedProduct = (Product)dataGridView1.SelectedRows[0].DataBoundItem;
                    var editForm = new EditProductForm(selectedProduct);
                    editForm.ShowDialog();
                    LoadProducts();
                }
            } catch (ArgumentNullException ex)
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
    }
}
