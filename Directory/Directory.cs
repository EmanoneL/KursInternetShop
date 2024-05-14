using DB;
using System.Data.Entity;
using System.Windows.Forms;

namespace Directory
{
    public partial class Directory : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        private BindingSource bindingSource;
        protected Right currentUserRight { get; set; }

        public Directory()
        {
            InitializeComponent();
            _context = new();
            InitializeForm();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        }

        protected virtual void InitializeForm()
        {

            // Инициализация элементов управления
            bindingSource = new BindingSource();
            dataGridView1.DataSource = bindingSource;

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

        protected virtual void EditButton_Click(object sender, EventArgs e)
        {

        }


        protected virtual void DeleteButton_Click(object sender, EventArgs e)
        {
        }

        protected virtual void CreateButton_Click(object sender, EventArgs e)
        {
            
        }

        public static void ShowBanks(DB.Right right)
        {
            Banks dir = new Banks(right);
            dir.ShowDialog();
        }

        public static void ShowCats(DB.Right right)
        {
            Cats dir = new(right);
            dir.ShowDialog();
        }

        public static void ShowCitis(DB.Right right)
        {
            Cities dir = new(right);
            dir.ShowDialog();
        }

        public static void ShowStreets(DB.Right right)
        {
            Streets dir = new(right);
            dir.ShowDialog();
        }
        public static void ShowStorages(DB.Right right)
        {
            Storage dir = new(right);
            dir.ShowDialog();
        }
    }
}
