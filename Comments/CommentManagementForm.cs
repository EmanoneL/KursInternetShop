using DB;
using System.Data;

namespace Comments
{
    public partial class CommentManagementForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        private BindingSource bindingSource;
        private Right currentUserRight { get; set; }
        public CommentManagementForm(Right currentUserRight)
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
            LoadComments();

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


        private void LoadComments()
        {


            var comments = _context.Comments
                .Select(p => new
                {
                    p.IdComments,
                    Product = p.IdProductNavigation.Name,
                    Customer = p.IdCustomersNavigation.Name,
                    p.Rating,
                    p.Description,
                    p.Date
                })
                .ToList();


            bindingSource.DataSource = comments;


            LoadDataGrid();
        }

        private void LoadDataGrid() { }



        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedCommentId = (int)dataGridView1.SelectedRows[0].Cells["idComments"].Value;

                    var selectedComment = _context.Comments.Find(selectedCommentId);
                    var editForm = new EditCommentForm(selectedComment);
                    editForm.ShowDialog();
                    LoadComments();
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void DeleteButton_Click(object sender, EventArgs e)
        {


            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedCommentId = (int)dataGridView1.SelectedRows[0].Cells["idComments"].Value;

                // No referencing records, proceed with deletion
                var selectedComment = _context.Comments.Find(selectedCommentId);
                _context.Comments.Remove(selectedComment);
                _context.SaveChanges();
                LoadComments();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var createForm = new InsertCommentForm();
            createForm.ShowDialog();
            LoadComments();
        }



        public static void ShowForm(DB.Right rights)
        {
            CommentManagementForm form = new(rights);
            form.Show();
        }
    }
}
