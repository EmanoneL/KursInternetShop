using DB;

namespace Directory
{
    public partial class Cities : Directory
    {
        private readonly DB.HandmadeShopSystemContext _context = new();

        private Right currentUserRight { get; set; }
        public Cities(Right curUr)
        {
            currentUserRight = curUr;
            InitializeComponent();
        }
        private void LoadCities()
        {
            var city = _context.Cities.Select(p => new{ p.Id, p.CityName}).ToList();
            dataGridView1.DataSource = city;

        }

        protected override void InitializeForm()
        {
            base.currentUserRight = currentUserRight;
            base.InitializeForm();
            LoadCities();
        }

        protected override void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedCityId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                    var selectedCity = _context.Cities.Find(selectedCityId);
                    var editForm = new EditCityForm(selectedCity);
                    editForm.ShowDialog();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool isPossibleToDelete(int selectedCityId)
        {
            var hasAddress = _context.Addresses.Any(o => o.City == selectedCityId);


            if (hasAddress)
            {
                return false;
            }
            return true;
        }
        protected override void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedCityId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                if (isPossibleToDelete(selectedCityId))
                {
                    // No referencing records, proceed with deletion
                    var selectedCity = _context.Cities.Find(selectedCityId);
                    _context.Cities.Remove(selectedCity);
                    _context.SaveChanges();
                    LoadCities();
                } else
                {
                    MessageBox.Show("Невозможно удалить город, так как есть адреса, ссылающиеся на него");
                }
            }
        }
        protected override void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertCityForm();
            createForm.ShowDialog();
            LoadCities();
        }
    }
}

