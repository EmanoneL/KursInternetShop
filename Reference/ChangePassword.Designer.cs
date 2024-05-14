namespace Reference
{
    partial class ChangePassword
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
            label6 = new Label();
            label5 = new Label();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label3 = new Label();
            label8 = new Label();
            label4 = new Label();
            button2 = new Button();
            button1 = new Button();
            label2 = new Label();
            label1 = new Label();
            textBox3 = new TextBox();
            label7 = new Label();
            SuspendLayout();
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(431, 334);
            label6.Name = "label6";
            label6.Size = new Size(0, 20);
            label6.TabIndex = 25;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(-1, 334);
            label5.Name = "label5";
            label5.Size = new Size(78, 20);
            label5.TabIndex = 24;
            label5.Text = "Раскладка";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(218, 169);
            textBox2.Margin = new Padding(3, 4, 3, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(148, 27);
            textBox2.TabIndex = 23;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(218, 124);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(148, 27);
            textBox1.TabIndex = 22;
            // 
            // label3
            // 
            label3.BackColor = SystemColors.Window;
            label3.Font = new Font("Microsoft Sans Serif", 10F);
            label3.Location = new Point(0, 73);
            label3.Name = "label3";
            label3.Size = new Size(554, 36);
            label3.TabIndex = 21;
            label3.Text = "Придумайте новый пароль";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.BackColor = SystemColors.MenuHighlight;
            label8.Font = new Font("Microsoft Sans Serif", 10F);
            label8.Location = new Point(-1, 37);
            label8.Name = "label8";
            label8.Size = new Size(554, 36);
            label8.TabIndex = 20;
            label8.Text = "Версия 1.0.0.0";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.BackColor = SystemColors.Info;
            label4.Font = new Font("Microsoft Sans Serif", 14F);
            label4.Location = new Point(-1, 1);
            label4.Name = "label4";
            label4.Size = new Size(554, 36);
            label4.TabIndex = 19;
            label4.Text = "HAND MADE SHOP";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // button2
            // 
            button2.Location = new Point(390, 262);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(75, 29);
            button2.TabIndex = 18;
            button2.Text = "Отмена";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(65, 262);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(81, 29);
            button1.TabIndex = 17;
            button1.Text = "Сменить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(65, 175);
            label2.Name = "label2";
            label2.Size = new Size(112, 20);
            label2.TabIndex = 16;
            label2.Text = "Новый пароль";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(62, 131);
            label1.Name = "label1";
            label1.Size = new Size(130, 20);
            label1.TabIndex = 15;
            label1.Text = "Прежний пароль";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(218, 213);
            textBox3.Margin = new Padding(3, 4, 3, 4);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(148, 27);
            textBox3.TabIndex = 27;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(59, 217);
            label7.Name = "label7";
            label7.Size = new Size(154, 20);
            label7.TabIndex = 26;
            label7.Text = "Подтвердите пароль";
            // 
            // ChangePassword
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(555, 357);
            Controls.Add(textBox3);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(label8);
            Controls.Add(label4);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ChangePassword";
            Text = "ChangePassword";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label6;
        private Label label5;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label label3;
        private Label label8;
        private Label label4;
        private Button button2;
        private Button button1;
        private Label label2;
        private Label label1;
        private TextBox textBox3;
        private Label label7;
    }
}