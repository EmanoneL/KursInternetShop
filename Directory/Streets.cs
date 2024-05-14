using DB;

namespace Directory
{
    public partial class Streets : Directory
    {
        private readonly DB.HandmadeShopSystemContext _context = new();
        private Right currentUserRight { get; set; }
        public Streets(Right currentUserRights)
        {
            currentUserRight = currentUserRights;
            InitializeComponent();
            _context = new DB.HandmadeShopSystemContext();
        }
        private void LoadStreets()
        {
            var streets = _context.Streets.Select(s => new
            {
                s.Id,
                s.StreetName
            }).ToList();
            dataGridView1.DataSource = streets;

        }

        protected override void InitializeForm()
        {
            base.currentUserRight = currentUserRight;
            base.InitializeForm();
            LoadStreets();
        }

        protected override void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedStreetId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                    var selectedStreet = _context.Streets.Find(selectedStreetId);
                    var editForm = new EditStreetForm(selectedStreet);
                    editForm.ShowDialog();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool isPossibleToDelete(int selectedStreetId)
        {
            var hasAddress = _context.Addresses.Any(o => o.Street == selectedStreetId);

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
                var selectedStreetId = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

                if (isPossibleToDelete(selectedStreetId))
                {
                    // No referencing records, proceed with deletion
                    var selectedStreet = _context.Streets.Find(selectedStreetId);
                    _context.Streets.Remove(selectedStreet);
                    _context.SaveChanges();
                    LoadStreets();
                }
                else
                {
                    MessageBox.Show("Нельзя удалить улицу, так как есть адреса, которые на нее ссылаются");
                }
            }
        }
        protected override void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertStreetForm();
            createForm.ShowDialog();
            LoadStreets();
        }
    }
}
