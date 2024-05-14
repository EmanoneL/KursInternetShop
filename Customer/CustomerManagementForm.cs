using DB;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Customer
{
    public partial class CustomerManagementForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        private BindingSource bindingSource;
        private Right currentUserRight { get; set; }

        public CustomerManagementForm(Right currentUserRight)
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
            LoadCustomers();

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


        private void LoadCustomers()
        {
            var customers = _context.Customers
                .Select(s => new
                {
                    s.IdCustomers,
                    s.Name,
                    Bank = s.IdBankNavigation.Name,
                    s.Email,
                    s.PhoneNumber,
                    Address = $"{s.IdAddressNavigation.CityNavigation.CityName}, " +
                    $"{s.IdAddressNavigation.StreetNavigation.StreetName}, " +
                    $"{s.IdAddressNavigation.HomeNumber}",
                    s.Login,
                    s.Password
                })
                .ToList();

            bindingSource.DataSource = customers;

            //LoadDataGrid();
        }



        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedCustomerId = (int)dataGridView1.SelectedRows[0].Cells["idCustomers"].Value;

                    var selectedCustomer = _context.Customers.Include(s => s.IdAddressNavigation).FirstOrDefault(c => c.IdCustomers == selectedCustomerId);
                    var editForm = new EditCustomerForm(selectedCustomer);
                    editForm.ShowDialog();
                    LoadCustomers();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsPossibeToDeleteCustomer(int selectedCustomerId)
        {

            // Check for referencing Products
            var hasProducts = _context.Orders.Any(o => o.IdCustomer == selectedCustomerId);

            if (hasProducts)
            {
                return false;
            }
            return true;

        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedCustomerId = (int)dataGridView1.SelectedRows[0].Cells["idCustomers"].Value;

                if (!IsPossibeToDeleteCustomer(selectedCustomerId))
                {
                    // Inform the user about referencing records
                    MessageBox.Show("This customer cannot be deleted because it has associated order entries. Delete them first and try again.");
                    return;
                }

                // No referencing records, proceed with deletion
                var selectedCustomer = _context.Customers.Find(selectedCustomerId);
                _context.Customers.Remove(selectedCustomer);
                _context.SaveChanges();
                LoadCustomers();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertCustomerForm();
            createForm.ShowDialog();
            LoadCustomers();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }


        public static void ShowForm(DB.Right rights)
        {
            CustomerManagementForm form = new(rights);
            form.Show();
        }
    }
}
