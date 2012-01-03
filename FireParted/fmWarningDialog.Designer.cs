namespace FireParted
{
    partial class fmWarningDialog
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
            this.chkSuppressWarning = new System.Windows.Forms.CheckBox();
            this.btnOkay = new System.Windows.Forms.Button();
            this.txtWarningText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // chkSuppressWarning
            // 
            this.chkSuppressWarning.AutoSize = true;
            this.chkSuppressWarning.Location = new System.Drawing.Point(13, 194);
            this.chkSuppressWarning.Name = "chkSuppressWarning";
            this.chkSuppressWarning.Size = new System.Drawing.Size(158, 17);
            this.chkSuppressWarning.TabIndex = 1;
            this.chkSuppressWarning.Text = "Don\'t show this dialog again";
            this.chkSuppressWarning.UseVisualStyleBackColor = true;
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(156, 217);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(117, 36);
            this.btnOkay.TabIndex = 2;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // txtWarningText
            // 
            this.txtWarningText.BackColor = System.Drawing.SystemColors.Control;
            this.txtWarningText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtWarningText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWarningText.Location = new System.Drawing.Point(13, 13);
            this.txtWarningText.Name = "txtWarningText";
            this.txtWarningText.ReadOnly = true;
            this.txtWarningText.Size = new System.Drawing.Size(400, 175);
            this.txtWarningText.TabIndex = 3;
            this.txtWarningText.Text = "";
            // 
            // fmWarningDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 262);
            this.Controls.Add(this.txtWarningText);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.chkSuppressWarning);
            this.Name = "fmWarningDialog";
            this.Text = "Important!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSuppressWarning;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.RichTextBox txtWarningText;
    }
}