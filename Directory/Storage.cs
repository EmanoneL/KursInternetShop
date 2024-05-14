using DB;

namespace Directory
{
    public partial class Storage : Directory
    {
        private readonly DB.HandmadeShopSystemContext _context = new();

        private Right currentUserRight { get; set; }
        public Storage(Right curRight)
        {
            currentUserRight = curRight;
            _context = new();
            InitializeComponent();
        }
        private void LoadStorages()
        {
            var storages = _context.Storages.Select(s => new
            {
                s.IdStorages,
                Storage = $"{s.IdAddressNavigation.CityNavigation.CityName}, " +
                    $"{s.IdAddressNavigation.StreetNavigation.StreetName}, " +
                    $"{s.IdAddressNavigation.HomeNumber}"
            }).ToList();
            dataGridView1.DataSource = storages;

        }

        protected override void InitializeForm()
        {
            base.currentUserRight = currentUserRight;
            base.InitializeForm();
            LoadStorages();
        }

        protected override void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedStorageId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                    var selectedStorage = _context.Storages.Find(selectedStorageId);
                    var editForm = new EditStorageForm(selectedStorage);
                    editForm.ShowDialog();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool isPossibleToDelete(int selectedStorageId)
        {
            var hasProduct = _context.Products.Any(o => o.IdStorages == selectedStorageId);

            if (hasProduct)
            {
                return false;
            }
            return true;
        }
        protected override void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedStorageId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                if (isPossibleToDelete(selectedStorageId))
                {
                    // No referencing records, proceed with deletion
                    var selectedStorage = _context.Storages.Find(selectedStorageId);
                    _context.Storages.Remove(selectedStorage);
                    _context.SaveChanges();
                    LoadStorages();
                }
            }
        }
        protected override void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertStorageForm();
            createForm.ShowDialog();
            LoadStorages();
        }
    }
}
