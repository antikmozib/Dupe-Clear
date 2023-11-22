namespace DupeClear
{
    partial class frmAction
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bwDelete = new System.ComponentModel.BackgroundWorker();
            this.bwCopyMove = new System.ComponentModel.BackgroundWorker();
            this.btnViewErrors = new System.Windows.Forms.Button();
            this.bwDupeFinder = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus1 = new System.Windows.Forms.Label();
            this.lblStatus2 = new System.Windows.Forms.Label();
            this.lblStatus3 = new System.Windows.Forms.Label();
            this.lblStatus4 = new System.Windows.Forms.Label();
            this.lblStatus5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.Window;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnCancel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnCancel.Location = new System.Drawing.Point(504, 96);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnCancel_KeyPress);
            // 
            // bwDelete
            // 
            this.bwDelete.WorkerReportsProgress = true;
            this.bwDelete.WorkerSupportsCancellation = true;
            this.bwDelete.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwDelete_DoWork);
            this.bwDelete.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwDelete_ProgressChanged);
            this.bwDelete.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwDelete_RunWorkerCompleted);
            // 
            // bwCopyMove
            // 
            this.bwCopyMove.WorkerReportsProgress = true;
            this.bwCopyMove.WorkerSupportsCancellation = true;
            this.bwCopyMove.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwCopyMove_DoWork);
            this.bwCopyMove.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwCopyMove_ProgressChanged);
            this.bwCopyMove.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwCopyMove_RunWorkerCompleted);
            // 
            // btnViewErrors
            // 
            this.btnViewErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewErrors.BackColor = System.Drawing.SystemColors.Window;
            this.btnViewErrors.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnViewErrors.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnViewErrors.Location = new System.Drawing.Point(408, 96);
            this.btnViewErrors.Name = "btnViewErrors";
            this.btnViewErrors.Size = new System.Drawing.Size(96, 24);
            this.btnViewErrors.TabIndex = 4;
            this.btnViewErrors.Text = "View &Errors";
            this.btnViewErrors.UseVisualStyleBackColor = false;
            this.btnViewErrors.Visible = false;
            this.btnViewErrors.Click += new System.EventHandler(this.btnViewErrors_Click);
            // 
            // bwDupeFinder
            // 
            this.bwDupeFinder.WorkerReportsProgress = true;
            this.bwDupeFinder.WorkerSupportsCancellation = true;
            this.bwDupeFinder.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwDupeFinder_DoWork);
            this.bwDupeFinder.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwDupeFinder_ProgressChanged);
            this.bwDupeFinder.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwDupeFinder_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(8, 8);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(592, 24);
            this.progressBar1.TabIndex = 8;
            // 
            // lblStatus1
            // 
            this.lblStatus1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus1.AutoEllipsis = true;
            this.lblStatus1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus1.Location = new System.Drawing.Point(8, 40);
            this.lblStatus1.Name = "lblStatus1";
            this.lblStatus1.Size = new System.Drawing.Size(592, 16);
            this.lblStatus1.TabIndex = 9;
            this.lblStatus1.Text = "label1";
            // 
            // lblStatus2
            // 
            this.lblStatus2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus2.AutoEllipsis = true;
            this.lblStatus2.Location = new System.Drawing.Point(8, 56);
            this.lblStatus2.Name = "lblStatus2";
            this.lblStatus2.Size = new System.Drawing.Size(432, 16);
            this.lblStatus2.TabIndex = 10;
            this.lblStatus2.Text = "label2";
            // 
            // lblStatus3
            // 
            this.lblStatus3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus3.AutoEllipsis = true;
            this.lblStatus3.Location = new System.Drawing.Point(8, 72);
            this.lblStatus3.Name = "lblStatus3";
            this.lblStatus3.Size = new System.Drawing.Size(432, 16);
            this.lblStatus3.TabIndex = 11;
            this.lblStatus3.Text = "label3";
            // 
            // lblStatus4
            // 
            this.lblStatus4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus4.AutoEllipsis = true;
            this.lblStatus4.Location = new System.Drawing.Point(440, 56);
            this.lblStatus4.Name = "lblStatus4";
            this.lblStatus4.Size = new System.Drawing.Size(160, 16);
            this.lblStatus4.TabIndex = 12;
            this.lblStatus4.Text = "label4";
            this.lblStatus4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblStatus5
            // 
            this.lblStatus5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus5.AutoEllipsis = true;
            this.lblStatus5.Location = new System.Drawing.Point(440, 72);
            this.lblStatus5.Name = "lblStatus5";
            this.lblStatus5.Size = new System.Drawing.Size(160, 16);
            this.lblStatus5.TabIndex = 13;
            this.lblStatus5.Text = "label5";
            this.lblStatus5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmAction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(608, 125);
            this.ControlBox = false;
            this.Controls.Add(this.lblStatus5);
            this.Controls.Add(this.lblStatus4);
            this.Controls.Add(this.lblStatus3);
            this.Controls.Add(this.lblStatus2);
            this.Controls.Add(this.lblStatus1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnViewErrors);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(624, 164);
            this.Name = "frmAction";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Please wait...";
            this.Load += new System.EventHandler(this.frmAction_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.ComponentModel.BackgroundWorker bwDelete;
        private System.ComponentModel.BackgroundWorker bwCopyMove;
        private System.Windows.Forms.Button btnViewErrors;
        private System.ComponentModel.BackgroundWorker bwDupeFinder;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStatus1;
        private System.Windows.Forms.Label lblStatus2;
        private System.Windows.Forms.Label lblStatus3;
        private System.Windows.Forms.Label lblStatus4;
        private System.Windows.Forms.Label lblStatus5;
    }
}