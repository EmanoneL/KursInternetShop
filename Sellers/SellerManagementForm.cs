using DB;
using System.Data;
using System.Data.Entity;

namespace Sellers
{
    public partial class SellerManagementForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        private BindingSource bindingSource;
        private Right currentUserRight { get; set; }

        public SellerManagementForm(Right curUR)
        {
            this.currentUserRight = curUR;
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
            LoadSellers();

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


        private void LoadSellers()
        {
            var products = _context.Sellers
                .Select(s => new
                {
                    s.IdSellers,
                    s.Name,
                    Logo = ResizeImageFromByteArray(s.Logo, new Size(100, 100)),
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

            bindingSource.DataSource = products;

            //LoadDataGrid();
        }



        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedSellerId = (int)dataGridView1.SelectedRows[0].Cells["idSellers"].Value;

                    var selectedSeller = _context.Sellers.Include(s => s.IdAddressNavigation).FirstOrDefault(c => c.IdSellers == selectedSellerId);
                    var editForm = new EditSellerForm(selectedSeller);
                    editForm.ShowDialog();
                    LoadSellers();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsPossibeToDeleteSeller(int selectedSellerId)
        {

            // Check for referencing Products
            var hasProducts = _context.Products.Any(o => o.IdSellers == selectedSellerId);

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
                var selectedSellerId = (int)dataGridView1.SelectedRows[0].Cells["idSellers"].Value;

                if (!IsPossibeToDeleteSeller(selectedSellerId))
                {
                    // Inform the user about referencing records
                    MessageBox.Show("This seller cannot be deleted because it has associated product entries. Delete them first and try again.");
                    return;
                }

                // No referencing records, proceed with deletion
                var selectedSeller = _context.Sellers.Find(selectedSellerId);
                _context.Sellers.Remove(selectedSeller);
                _context.SaveChanges();
                LoadSellers();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertSellerForm();
            createForm.ShowDialog();
            LoadSellers();
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

        public static void ShowForm(DB.Right right)
        {
            SellerManagementForm form = new(right);
            form.Show();
        }
    }
}
