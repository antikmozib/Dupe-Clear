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
            this.lblStatus1 = new System.Windows.Forms.Label();
            this.lblStatus2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bwDelete = new System.ComponentModel.BackgroundWorker();
            this.bwCopyMove = new System.ComponentModel.BackgroundWorker();
            this.btnViewErrors = new System.Windows.Forms.Button();
            this.bwDupeFinder = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblStatus3 = new System.Windows.Forms.Label();
            this.lblStatus4 = new System.Windows.Forms.Label();
            this.lblStatus5 = new System.Windows.Forms.Label();
            this.progressBar1 = new ProgressBarEx.ProgressBarEx();
            this.SuspendLayout();
            // 
            // lblStatus1
            // 
            this.lblStatus1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus1.AutoEllipsis = true;
            this.lblStatus1.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblStatus1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblStatus1.Location = new System.Drawing.Point(12, 69);
            this.lblStatus1.Name = "lblStatus1";
            this.lblStatus1.Size = new System.Drawing.Size(591, 20);
            this.lblStatus1.TabIndex = 1;
            this.lblStatus1.Text = "Status1";
            this.lblStatus1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus2
            // 
            this.lblStatus2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus2.AutoEllipsis = true;
            this.lblStatus2.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblStatus2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblStatus2.Location = new System.Drawing.Point(12, 89);
            this.lblStatus2.Name = "lblStatus2";
            this.lblStatus2.Size = new System.Drawing.Size(391, 20);
            this.lblStatus2.TabIndex = 2;
            this.lblStatus2.Text = "Status2";
            this.lblStatus2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.Window;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnCancel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnCancel.Location = new System.Drawing.Point(499, 167);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 28);
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
            this.btnViewErrors.Location = new System.Drawing.Point(392, 167);
            this.btnViewErrors.Name = "btnViewErrors";
            this.btnViewErrors.Size = new System.Drawing.Size(101, 28);
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
            // lblStatus3
            // 
            this.lblStatus3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus3.AutoEllipsis = true;
            this.lblStatus3.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblStatus3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblStatus3.Location = new System.Drawing.Point(12, 109);
            this.lblStatus3.Name = "lblStatus3";
            this.lblStatus3.Size = new System.Drawing.Size(391, 20);
            this.lblStatus3.TabIndex = 5;
            this.lblStatus3.Text = "Status3";
            this.lblStatus3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus4
            // 
            this.lblStatus4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus4.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblStatus4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus4.Location = new System.Drawing.Point(409, 89);
            this.lblStatus4.Name = "lblStatus4";
            this.lblStatus4.Size = new System.Drawing.Size(191, 20);
            this.lblStatus4.TabIndex = 6;
            this.lblStatus4.Text = "Status4";
            this.lblStatus4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatus5
            // 
            this.lblStatus5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus5.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblStatus5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus5.Location = new System.Drawing.Point(409, 109);
            this.lblStatus5.Name = "lblStatus5";
            this.lblStatus5.Size = new System.Drawing.Size(191, 20);
            this.lblStatus5.TabIndex = 7;
            this.lblStatus5.Text = "Status5";
            this.lblStatus5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.BackColor = System.Drawing.Color.Transparent;
            this.progressBar1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.progressBar1.BorderColor = System.Drawing.Color.PaleGreen;
            this.progressBar1.Cursor = System.Windows.Forms.Cursors.Default;
            this.progressBar1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressBar1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.progressBar1.GradiantColor = System.Drawing.SystemColors.Window;
            this.progressBar1.GradiantPosition = ProgressBarEx.ProgressBarEx.GradiantArea.Bottom;
            this.progressBar1.Image = null;
            this.progressBar1.Location = new System.Drawing.Point(12, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RoundedCorners = false;
            this.progressBar1.ShowPercentage = true;
            this.progressBar1.ShowText = true;
            this.progressBar1.Size = new System.Drawing.Size(588, 38);
            // 
            // frmAction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(612, 207);
            this.ControlBox = false;
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblStatus5);
            this.Controls.Add(this.lblStatus4);
            this.Controls.Add(this.lblStatus3);
            this.Controls.Add(this.btnViewErrors);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblStatus2);
            this.Controls.Add(this.lblStatus1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(628, 223);
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
        private System.Windows.Forms.Label lblStatus1;
        private System.Windows.Forms.Label lblStatus2;
        private System.Windows.Forms.Button btnCancel;
        private System.ComponentModel.BackgroundWorker bwDelete;
        private System.ComponentModel.BackgroundWorker bwCopyMove;
        private System.Windows.Forms.Button btnViewErrors;
        private System.ComponentModel.BackgroundWorker bwDupeFinder;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblStatus3;
        private System.Windows.Forms.Label lblStatus4;
        private System.Windows.Forms.Label lblStatus5;
        private ProgressBarEx.ProgressBarEx progressBar1;
    }
}