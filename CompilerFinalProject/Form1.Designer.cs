namespace CompilerFinalProject
{
    partial class Form1
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
            this.tfinput = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tftoken = new System.Windows.Forms.RichTextBox();
            this.symboltable = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.RichTextBox();
            this.DG = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SAOutput = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DG)).BeginInit();
            this.SuspendLayout();
            // 
            // tfinput
            // 
            this.tfinput.Location = new System.Drawing.Point(12, 35);
            this.tfinput.Name = "tfinput";
            this.tfinput.Size = new System.Drawing.Size(218, 232);
            this.tfinput.TabIndex = 0;
            this.tfinput.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter Code";
            // 
            // tftoken
            // 
            this.tftoken.Location = new System.Drawing.Point(265, 35);
            this.tftoken.Name = "tftoken";
            this.tftoken.Size = new System.Drawing.Size(266, 101);
            this.tftoken.TabIndex = 2;
            this.tftoken.Text = "";
            // 
            // symboltable
            // 
            this.symboltable.Location = new System.Drawing.Point(265, 170);
            this.symboltable.Name = "symboltable";
            this.symboltable.Size = new System.Drawing.Size(266, 97);
            this.symboltable.TabIndex = 3;
            this.symboltable.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tokens";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(262, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Symbol Table";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 682);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Compile";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(565, 35);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(100, 232);
            this.Output.TabIndex = 7;
            this.Output.Text = "";
            // 
            // DG
            // 
            this.DG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DG.Location = new System.Drawing.Point(12, 292);
            this.DG.Name = "DG";
            this.DG.Size = new System.Drawing.Size(1338, 384);
            this.DG.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(565, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Parsing Stack";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "SLR(1) Table";
            // 
            // SAOutput
            // 
            this.SAOutput.Location = new System.Drawing.Point(672, 35);
            this.SAOutput.Name = "SAOutput";
            this.SAOutput.Size = new System.Drawing.Size(395, 232);
            this.SAOutput.TabIndex = 11;
            this.SAOutput.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(672, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Semantic Analysis";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1354, 733);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SAOutput);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DG);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.symboltable);
            this.Controls.Add(this.tftoken);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tfinput);
            this.Name = "Form1";
            this.Text = "Compiler";
            ((System.ComponentModel.ISupportInitialize)(this.DG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox tfinput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox tftoken;
        private System.Windows.Forms.RichTextBox symboltable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox Output;
        private System.Windows.Forms.DataGridView DG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox SAOutput;
        private System.Windows.Forms.Label label6;

     
        
    }
}

