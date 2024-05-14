namespace Documents
{
    partial class Income
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            button1 = new Button();
            label1 = new Label();
            SellercomboBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(830, 445);
            dataGridView1.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(12, 480);
            button1.Name = "button1";
            button1.Size = new Size(207, 57);
            button1.TabIndex = 5;
            button1.Text = "Экспорт";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(295, 470);
            label1.Name = "label1";
            label1.Size = new Size(79, 20);
            label1.TabIndex = 7;
            label1.Text = "Продавец";
            // 
            // SellercomboBox
            // 
            SellercomboBox.FormattingEnabled = true;
            SellercomboBox.Location = new Point(225, 495);
            SellercomboBox.Name = "SellercomboBox";
            SellercomboBox.Size = new Size(151, 28);
            SellercomboBox.TabIndex = 6;
            SellercomboBox.SelectedIndexChanged += SellercomboBox_SelectedIndexChanged;
            // 
            // Income
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(862, 546);
            Controls.Add(label1);
            Controls.Add(SellercomboBox);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Name = "Income";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button button1;
        private Label label1;
        private ComboBox SellercomboBox;
    }
}
