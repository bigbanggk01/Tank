﻿namespace WindowsFormsApp1
{
    partial class EndingForm2
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
            this.angainBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buyBtn = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // angainBtn
            // 
            this.angainBtn.Location = new System.Drawing.Point(12, 120);
            this.angainBtn.Name = "angainBtn";
            this.angainBtn.Size = new System.Drawing.Size(180, 28);
            this.angainBtn.TabIndex = 0;
            this.angainBtn.Text = "Play again";
            this.angainBtn.UseVisualStyleBackColor = true;
            this.angainBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(218, 120);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 28);
            this.button2.TabIndex = 1;
            this.button2.Text = "Logout";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Papyrus", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(65, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 76);
            this.label1.TabIndex = 2;
            this.label1.Text = "You lose";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(115, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(202, 22);
            this.textBox1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Card number";
            // 
            // buyBtn
            // 
            this.buyBtn.Location = new System.Drawing.Point(115, 82);
            this.buyBtn.Name = "buyBtn";
            this.buyBtn.Size = new System.Drawing.Size(93, 31);
            this.buyBtn.TabIndex = 6;
            this.buyBtn.Text = "Buy now";
            this.buyBtn.UseVisualStyleBackColor = true;
            this.buyBtn.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(223, 82);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 31);
            this.button3.TabIndex = 7;
            this.button3.Text = "Exit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // EndingForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 167);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buyBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.angainBtn);
            this.Name = "EndingForm2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Top up";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button angainBtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buyBtn;
        private System.Windows.Forms.Button button3;
    }
}