namespace Documents
{
    partial class StorageIncome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            StorageComboBox = new ComboBox();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(459, 472);
            label1.Name = "label1";
            label1.Size = new Size(49, 20);
            label1.TabIndex = 11;
            label1.Text = "Склад";
            // 
            // StorageComboBox
            // 
            StorageComboBox.FormattingEnabled = true;
            StorageComboBox.Location = new Point(225, 495);
            StorageComboBox.Name = "StorageComboBox";
            StorageComboBox.Size = new Size(283, 28);
            StorageComboBox.TabIndex = 10;
            // 
            // button1
            // 
            button1.Location = new Point(12, 480);
            button1.Name = "button1";
            button1.Size = new Size(207, 57);
            button1.TabIndex = 9;
            button1.Text = "Экспорт";
            button1.UseVisualStyleBackColor = true;
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
            dataGridView1.TabIndex = 8;
            // 
            // StorageIncome
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(866, 545);
            Controls.Add(label1);
            Controls.Add(StorageComboBox);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Name = "StorageIncome";
            Text = "StorageIncome";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox StorageComboBox;
        private Button button1;
        private DataGridView dataGridView1;
    }
}