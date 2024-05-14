using DB;

namespace Directory
{
    public partial class Cats : Directory
    {
        private readonly DB.HandmadeShopSystemContext _context = new();

        private Right currentUserRight { get; set; }
        public Cats(Right curUs)
        {
            currentUserRight = curUs;
            InitializeComponent();
        }
        private void LoadCats()
        {
            var comments = _context.Categories.Select(c => new { c.IdCategory, c.Name, c.Description }).ToList();
            dataGridView1.DataSource = comments;

        }

        protected override void InitializeForm()
        {
            base.currentUserRight = currentUserRight;
            base.InitializeForm();
            LoadCats();
        }

        protected override void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedCommentId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                    var selectedComment = _context.Categories.Find(selectedCommentId);
                    var editForm = new EditCatForm(selectedComment);
                    editForm.ShowDialog();
                    LoadCats();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool isPossibleToDelete(int selectedCatId)
        {
            var hasProducts = _context.Products.Any(o => o.IdCategory == selectedCatId);


            if (hasProducts)
            {
                return false;
            }
            return true;
        }
        protected override void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedCatId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                if (isPossibleToDelete(selectedCatId))
                {
                    // No referencing records, proceed with deletion
                    var selectedCat = _context.Categories.Find(selectedCatId);
                    _context.Categories.Remove(selectedCat);
                    _context.SaveChanges();
                    LoadCats();
                }
                else
                {
                    MessageBox.Show("Невозможно удалить категорию, так как есть товары, ссылающиеся на нее");
                }
            }
        }
        protected override void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertCatsForm();
            createForm.ShowDialog();
            LoadCats();
        }
    }
}
