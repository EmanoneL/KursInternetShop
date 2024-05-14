using DB;

namespace Addreses
{
    public partial class Addresses : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        private BindingSource bindingSource;
        private Right currentUserRight { get; set; }

        public Addresses(Right currentUserRight)
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
            LoadAddresses();

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


        private void LoadAddresses()
        {
            var addresses = _context.Addresses
                .Select(p => new
                {
                    City = p.CityNavigation.CityName,
                    Street = p.StreetNavigation.StreetName,
                    p.HomeNumber
                })
                .ToList();

            bindingSource.DataSource = addresses;

            //LoadDataGrid();
        }



        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedAddressId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                    var selectedAddress = _context.Addresses.Find(selectedAddressId);
                    var editForm = new EditAddressForm(selectedAddress);
                    editForm.ShowDialog();
                    LoadAddresses();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsPossibeToDeleteProduct(int selectedAddressId)
        {

            // Check for referencing Orders
            var hasOrders = _context.Orders.Any(o => o.IdAddress == selectedAddressId);
            var hasCustomers = _context.Customers.Any(o => o.IdAddress == selectedAddressId);
            var hasSellers = _context.Sellers.Any(o => o.IdAddress == selectedAddressId);

            if (hasOrders || hasCustomers || hasSellers)
            {
                return false;
            }
            return true;

        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedAddressId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                if (!IsPossibeToDeleteProduct(selectedAddressId))
                {
                    // Inform the user about referencing records
                    MessageBox.Show("This address cannot be deleted because it has associated entries. Delete them first and try again.");
                    return;
                }

                // No referencing records, proceed with deletion
                var selectedAddress = _context.Addresses.Find(selectedAddressId);
                _context.Addresses.Remove(selectedAddress);
                _context.SaveChanges();
                LoadAddresses();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertAddressForm();
            createForm.ShowDialog();
            LoadAddresses();
        }



        public static void ShowForm(DB.Right rights)
        {
            Addresses form = new(rights);
            form.Show();
        }
    }
}
