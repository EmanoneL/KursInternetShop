using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
namespace Category
{
    public partial class Form : System.Windows.Forms.Form
    {
        //private MyDbContext dbContext = new MyDbContext();
      

        public Form()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
           // dataGridView1.DataSource = dbContext.MyTables.ToList();
        }

    }

}
