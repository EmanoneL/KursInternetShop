using DB;

namespace Directory
{
    public partial class Banks : Directory
    {
        private readonly DB.HandmadeShopSystemContext _context = new();
        private Right currentUserRight { get; set; }
        public Banks(Right curUr)
        {
            currentUserRight = curUr;
            InitializeComponent();
        }
        private void LoadBanks()
        {
            var banks = _context.Banks.Select(p=> new
            {
                p.IdBank,
                p.Name,
                p.PhoneNumer
            }
            ).ToList();
            dataGridView1.DataSource = banks;

        }

        protected override void InitializeForm()
        {
            base.currentUserRight = currentUserRight;
            base.InitializeForm();
            LoadBanks();
        }

        protected override void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedBankId = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

                    var selectedBank = _context.Banks.Find(selectedBankId);
                    var editForm = new EditBankForm(selectedBank);
                    editForm.ShowDialog();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadBanks();
        }

        private bool isPossibleToDelete(int selectedBankId)
        {
            var hasCustomers = _context.Customers.Any(o => o.IdBank == selectedBankId);

            var hasSellers = _context.Sellers.Any(c => c.IdBank == selectedBankId);

            if (hasCustomers || hasSellers)
            {
                return false;
            }
            return true;
        }
        protected override void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedBankId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                if (isPossibleToDelete(selectedBankId))
                {
                    // No referencing records, proceed with deletion
                    var selectedBank = _context.Banks.Find(selectedBankId);
                    _context.Banks.Remove(selectedBank);
                    _context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Невозможно удалить банк, так как есть покупатели или продавцы, которые его используют");
                }
                LoadBanks();

            }
        }
        protected override void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertBankForm();
            createForm.ShowDialog();
            LoadBanks();
        }
    }
}
