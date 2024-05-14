using DB;
using System.Data;

namespace Order
{
    public partial class OrderManagementForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        private BindingSource bindingSource;
        private Right currentUserRight { get; set; }

        public OrderManagementForm(Right curUr)
        {
            this.currentUserRight = curUr;
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
            LoadOrders();

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


        private void LoadOrders()
        {
            var orders = _context.Orders
                .Select(p => new
                {
                    p.IdOrders,
                    DeliveryCompany = p.IdDeliveryCompanyNavigation.Name,
                    Customer = p.IdCustomerNavigation.Name,
                    Product = p.IdProductsNavigation.Name,
                    p.TotalCost,
                    p.Status,
                    Address = $"{p.IdAddressNavigation.CityNavigation.CityName}, " +
                    $"{p.IdAddressNavigation.StreetNavigation.StreetName}, " +
                    $"{p.IdAddressNavigation.HomeNumber}"
                })
                .ToList();

            bindingSource.DataSource = orders;

            //LoadDataGrid();
        }



        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedOrderId = (int)dataGridView1.SelectedRows[0].Cells["idOrders"].Value;

                    var selectedOrder = _context.Orders.Find(selectedOrderId);
                    var editForm = new EditOrderForm(selectedOrder);
                    editForm.ShowDialog();
                    LoadOrders();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Ошибка редактирвания");
            }
        }


        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedOrderId = (int)dataGridView1.SelectedRows[0].Cells["idOrders"].Value;

                    var selectedOrder = _context.Orders.Find(selectedOrderId);
                    _context.Orders.Remove(selectedOrder);
                    _context.SaveChanges();
                    LoadOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении заказа");
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertOrderForm();
            createForm.ShowDialog();
            LoadOrders();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }


        public static void ShowForm(DB.Right rights)
        {
            OrderManagementForm form = new(rights);
            form.Show();
        }
    }
}
