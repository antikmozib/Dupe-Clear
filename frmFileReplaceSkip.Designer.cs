namespace DupeClear
{
    partial class frmFileReplaceSkip
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
            this.lblFileName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblChosenDir = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.cbDoInFuture = new System.Windows.Forms.CheckBox();
            this.btnKeepBoth = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileName.Location = new System.Drawing.Point(12, 9);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(59, 15);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "FileName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(337, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Another file with the same name exists in the chosen directory.";
            // 
            // lblChosenDir
            // 
            this.lblChosenDir.AutoSize = true;
            this.lblChosenDir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChosenDir.Location = new System.Drawing.Point(12, 65);
            this.lblChosenDir.Name = "lblChosenDir";
            this.lblChosenDir.Size = new System.Drawing.Size(100, 15);
            this.lblChosenDir.TabIndex = 2;
            this.lblChosenDir.Text = "ChosenDirectory";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "What would you like to do?";
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(42, 127);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(98, 29);
            this.btnSkip.TabIndex = 0;
            this.btnSkip.Text = "&Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(146, 127);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(98, 29);
            this.btnReplace.TabIndex = 1;
            this.btnReplace.Text = "&Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // cbDoInFuture
            // 
            this.cbDoInFuture.AutoSize = true;
            this.cbDoInFuture.Location = new System.Drawing.Point(42, 162);
            this.cbDoInFuture.Name = "cbDoInFuture";
            this.cbDoInFuture.Size = new System.Drawing.Size(183, 19);
            this.cbDoInFuture.TabIndex = 3;
            this.cbDoInFuture.Text = "&Do this for all further conflicts";
            this.cbDoInFuture.UseVisualStyleBackColor = true;
            // 
            // btnKeepBoth
            // 
            this.btnKeepBoth.Location = new System.Drawing.Point(250, 127);
            this.btnKeepBoth.Name = "btnKeepBoth";
            this.btnKeepBoth.Size = new System.Drawing.Size(98, 29);
            this.btnKeepBoth.TabIndex = 2;
            this.btnKeepBoth.Text = "&Keep both";
            this.btnKeepBoth.UseVisualStyleBackColor = true;
            this.btnKeepBoth.Click += new System.EventHandler(this.btnKeepBoth_Click);
            // 
            // frmFileReplaceSkip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 190);
            this.Controls.Add(this.btnKeepBoth);
            this.Controls.Add(this.cbDoInFuture);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblChosenDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFileName);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFileReplaceSkip";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Already Exists";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFileReplaceSkip_FormClosing);
            this.Load += new System.EventHandler(this.frmFileReplaceSkip_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblChosenDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.CheckBox cbDoInFuture;
        private System.Windows.Forms.Button btnKeepBoth;
    }
}