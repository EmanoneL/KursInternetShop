namespace Reference
{
    public partial class Content : Form
    {
        public Content()
        {
            InitializeComponent();
        }

        public static void ShowForm(DB.Right right)
        {
            Content content = new Content();
            content.ShowDialog();
        }
    }
}
