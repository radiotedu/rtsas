namespace IcecastStreamMonitor.Installer;

partial class InstallerForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.pnlSide = new System.Windows.Forms.Panel();
        this.lblStep4 = new System.Windows.Forms.Label();
        this.lblStep3 = new System.Windows.Forms.Label();
        this.lblStep2 = new System.Windows.Forms.Label();
        this.lblStep1 = new System.Windows.Forms.Label();
        this.lblSideTitle = new System.Windows.Forms.Label();
        this.pnlBottom = new System.Windows.Forms.Panel();
        this.btnBack = new System.Windows.Forms.Button();
        this.btnNext = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.pnlContainer = new System.Windows.Forms.Panel();
        this.pnlSide.SuspendLayout();
        this.pnlBottom.SuspendLayout();
        this.SuspendLayout();
        // 
        // pnlSide
        // 
        this.pnlSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
        this.pnlSide.Controls.Add(this.lblStep4);
        this.pnlSide.Controls.Add(this.lblStep3);
        this.pnlSide.Controls.Add(this.lblStep2);
        this.pnlSide.Controls.Add(this.lblStep1);
        this.pnlSide.Controls.Add(this.lblSideTitle);
        this.pnlSide.Dock = System.Windows.Forms.DockStyle.Left;
        this.pnlSide.Location = new System.Drawing.Point(0, 0);
        this.pnlSide.Name = "pnlSide";
        this.pnlSide.Padding = new System.Windows.Forms.Padding(16);
        this.pnlSide.Size = new System.Drawing.Size(180, 411);
        this.pnlSide.TabIndex = 0;
        // 
        // lblStep4
        // 
        this.lblStep4.AutoSize = true;
        this.lblStep4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblStep4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
        this.lblStep4.Location = new System.Drawing.Point(16, 195);
        this.lblStep4.Name = "lblStep4";
        this.lblStep4.Size = new System.Drawing.Size(89, 17);
        this.lblStep4.TabIndex = 4;
        this.lblStep4.Text = "4. Install Ready";
        // 
        // lblStep3
        // 
        this.lblStep3.AutoSize = true;
        this.lblStep3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblStep3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
        this.lblStep3.Location = new System.Drawing.Point(16, 155);
        this.lblStep3.Name = "lblStep3";
        this.lblStep3.Size = new System.Drawing.Size(117, 17);
        this.lblStep3.TabIndex = 3;
        this.lblStep3.Text = "3. Shortcuts && Start";
        // 
        // lblStep2
        // 
        this.lblStep2.AutoSize = true;
        this.lblStep2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.lblStep2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
        this.lblStep2.Location = new System.Drawing.Point(16, 115);
        this.lblStep2.Name = "lblStep2";
        this.lblStep2.Size = new System.Drawing.Size(120, 17);
        this.lblStep2.TabIndex = 2;
        this.lblStep2.Text = "2. Stream Server IP";
        // 
        // lblStep1
        // 
        this.lblStep1.AutoSize = true;
        this.lblStep1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblStep1.ForeColor = System.Drawing.Color.White;
        this.lblStep1.Location = new System.Drawing.Point(16, 75);
        this.lblStep1.Name = "lblStep1";
        this.lblStep1.Size = new System.Drawing.Size(115, 17);
        this.lblStep1.TabIndex = 1;
        this.lblStep1.Text = "1. Welcome && Path";
        // 
        // lblSideTitle
        // 
        this.lblSideTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblSideTitle.ForeColor = System.Drawing.Color.White;
        this.lblSideTitle.Location = new System.Drawing.Point(16, 16);
        this.lblSideTitle.Name = "lblSideTitle";
        this.lblSideTitle.Size = new System.Drawing.Size(148, 48);
        this.lblSideTitle.TabIndex = 0;
        this.lblSideTitle.Text = "RADIOTEDU Setup Wizard";
        // 
        // pnlBottom
        // 
        this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
        this.pnlBottom.Controls.Add(this.btnBack);
        this.pnlBottom.Controls.Add(this.btnNext);
        this.pnlBottom.Controls.Add(this.btnCancel);
        this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlBottom.Location = new System.Drawing.Point(180, 363);
        this.pnlBottom.Name = "pnlBottom";
        this.pnlBottom.Size = new System.Drawing.Size(484, 48);
        this.pnlBottom.TabIndex = 1;
        // 
        // btnBack
        // 
        this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnBack.Enabled = false;
        this.btnBack.Location = new System.Drawing.Point(220, 10);
        this.btnBack.Name = "btnBack";
        this.btnBack.Size = new System.Drawing.Size(75, 28);
        this.btnBack.TabIndex = 0;
        this.btnBack.Text = "< Back";
        this.btnBack.UseVisualStyleBackColor = true;
        this.btnBack.Click += new System.EventHandler(this.BtnBack_Click);
        // 
        // btnNext
        // 
        this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnNext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnNext.Location = new System.Drawing.Point(305, 10);
        this.btnNext.Name = "btnNext";
        this.btnNext.Size = new System.Drawing.Size(85, 28);
        this.btnNext.TabIndex = 1;
        this.btnNext.Text = "Next >";
        this.btnNext.UseVisualStyleBackColor = true;
        this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.Location = new System.Drawing.Point(397, 10);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 28);
        this.btnCancel.TabIndex = 2;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
        // 
        // pnlContainer
        // 
        this.pnlContainer.BackColor = System.Drawing.Color.White;
        this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pnlContainer.Location = new System.Drawing.Point(180, 0);
        this.pnlContainer.Name = "pnlContainer";
        this.pnlContainer.Padding = new System.Windows.Forms.Padding(20);
        this.pnlContainer.Size = new System.Drawing.Size(484, 363);
        this.pnlContainer.TabIndex = 2;
        // 
        // InstallerForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(664, 411);
        this.Controls.Add(this.pnlContainer);
        this.Controls.Add(this.pnlBottom);
        this.Controls.Add(this.pnlSide);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "InstallerForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "RADIOTEDU Stream Audio Server — Setup Wizard";
        this.Load += new System.EventHandler(this.InstallerForm_Load);
        this.pnlSide.ResumeLayout(false);
        this.pnlSide.PerformLayout();
        this.pnlBottom.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel pnlSide;
    private System.Windows.Forms.Label lblSideTitle;
    private System.Windows.Forms.Label lblStep4;
    private System.Windows.Forms.Label lblStep3;
    private System.Windows.Forms.Label lblStep2;
    private System.Windows.Forms.Label lblStep1;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnBack;
    private System.Windows.Forms.Button btnNext;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Panel pnlContainer;
}
