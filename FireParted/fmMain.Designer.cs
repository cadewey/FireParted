/*
    This file is part of FireParted.

    FireParted is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FireParted is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FireParted.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace FireParted
{
    partial class fmMain
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
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.numData = new System.Windows.Forms.NumericUpDown();
            this.numSdcard = new System.Windows.Forms.NumericUpDown();
            this.numCache = new System.Windows.Forms.NumericUpDown();
            this.lblData = new System.Windows.Forms.Label();
            this.lblSdcard = new System.Windows.Forms.Label();
            this.lblCache = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSpaceTitle = new System.Windows.Forms.Label();
            this.lblAvailSpace = new System.Windows.Forms.Label();
            this.btnReadPartitions = new System.Windows.Forms.Button();
            this.btnBackupData = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDataUsage = new System.Windows.Forms.Label();
            this.progDataUsage = new System.Windows.Forms.ProgressBar();
            this.progSdUsage = new System.Windows.Forms.ProgressBar();
            this.lblSdUsage = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnResetValues = new System.Windows.Forms.Button();
            this.btnApplyChanges = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSdcard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCache)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbConsole
            // 
            this.rtbConsole.BackColor = System.Drawing.Color.White;
            this.rtbConsole.Font = new System.Drawing.Font("DejaVu Sans Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConsole.Location = new System.Drawing.Point(214, 11);
            this.rtbConsole.Name = "rtbConsole";
            this.rtbConsole.ReadOnly = true;
            this.rtbConsole.Size = new System.Drawing.Size(609, 328);
            this.rtbConsole.TabIndex = 0;
            this.rtbConsole.Text = "";
            // 
            // numData
            // 
            this.numData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numData.Location = new System.Drawing.Point(18, 59);
            this.numData.Maximum = new decimal(new int[] {
            7600,
            0,
            0,
            0});
            this.numData.Name = "numData";
            this.numData.Size = new System.Drawing.Size(120, 22);
            this.numData.TabIndex = 1;
            this.numData.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numData.ValueChanged += new System.EventHandler(this.numData_ValueChanged);
            this.numData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numData_KeyDown);
            // 
            // numSdcard
            // 
            this.numSdcard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSdcard.Location = new System.Drawing.Point(18, 128);
            this.numSdcard.Maximum = new decimal(new int[] {
            7600,
            0,
            0,
            0});
            this.numSdcard.Name = "numSdcard";
            this.numSdcard.Size = new System.Drawing.Size(120, 22);
            this.numSdcard.TabIndex = 2;
            this.numSdcard.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSdcard.ValueChanged += new System.EventHandler(this.numSdcard_ValueChanged);
            this.numSdcard.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numSdcard_KeyDown);
            // 
            // numCache
            // 
            this.numCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCache.Location = new System.Drawing.Point(18, 205);
            this.numCache.Maximum = new decimal(new int[] {
            7600,
            0,
            0,
            0});
            this.numCache.Name = "numCache";
            this.numCache.Size = new System.Drawing.Size(120, 22);
            this.numCache.TabIndex = 3;
            this.numCache.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numCache.ValueChanged += new System.EventHandler(this.numCache_ValueChanged);
            this.numCache.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numCache_KeyDown);
            // 
            // lblData
            // 
            this.lblData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblData.Location = new System.Drawing.Point(20, 36);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(145, 20);
            this.lblData.TabIndex = 4;
            this.lblData.Text = "Userdata Partition";
            // 
            // lblSdcard
            // 
            this.lblSdcard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSdcard.Location = new System.Drawing.Point(20, 105);
            this.lblSdcard.Name = "lblSdcard";
            this.lblSdcard.Size = new System.Drawing.Size(145, 20);
            this.lblSdcard.TabIndex = 5;
            this.lblSdcard.Text = "SD Card Partition";
            // 
            // lblCache
            // 
            this.lblCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCache.Location = new System.Drawing.Point(20, 182);
            this.lblCache.Name = "lblCache";
            this.lblCache.Size = new System.Drawing.Size(145, 20);
            this.lblCache.TabIndex = 6;
            this.lblCache.Text = "Cache Partition";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(138, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "MB";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(138, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "MB";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(138, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "MB";
            // 
            // lblSpaceTitle
            // 
            this.lblSpaceTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpaceTitle.Location = new System.Drawing.Point(4, 267);
            this.lblSpaceTitle.Name = "lblSpaceTitle";
            this.lblSpaceTitle.Size = new System.Drawing.Size(142, 20);
            this.lblSpaceTitle.TabIndex = 10;
            this.lblSpaceTitle.Text = "Unallocated Space:";
            // 
            // lblAvailSpace
            // 
            this.lblAvailSpace.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailSpace.Location = new System.Drawing.Point(139, 267);
            this.lblAvailSpace.Name = "lblAvailSpace";
            this.lblAvailSpace.Size = new System.Drawing.Size(69, 20);
            this.lblAvailSpace.TabIndex = 11;
            this.lblAvailSpace.Text = "---";
            // 
            // btnReadPartitions
            // 
            this.btnReadPartitions.Location = new System.Drawing.Point(7, 437);
            this.btnReadPartitions.Name = "btnReadPartitions";
            this.btnReadPartitions.Size = new System.Drawing.Size(180, 32);
            this.btnReadPartitions.TabIndex = 12;
            this.btnReadPartitions.Text = "Read Partition Table";
            this.btnReadPartitions.UseVisualStyleBackColor = true;
            this.btnReadPartitions.Click += new System.EventHandler(this.btnReadPartitions_Click);
            // 
            // btnBackupData
            // 
            this.btnBackupData.Location = new System.Drawing.Point(637, 437);
            this.btnBackupData.Name = "btnBackupData";
            this.btnBackupData.Size = new System.Drawing.Size(186, 32);
            this.btnBackupData.TabIndex = 13;
            this.btnBackupData.Text = "Archive /data Partition";
            this.btnBackupData.UseVisualStyleBackColor = true;
            this.btnBackupData.Click += new System.EventHandler(this.btnBackupData_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(211, 351);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 19);
            this.label4.TabIndex = 14;
            this.label4.Text = "Data Usage:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDataUsage
            // 
            this.lblDataUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataUsage.Location = new System.Drawing.Point(293, 351);
            this.lblDataUsage.Name = "lblDataUsage";
            this.lblDataUsage.Size = new System.Drawing.Size(114, 19);
            this.lblDataUsage.TabIndex = 15;
            this.lblDataUsage.Text = "---";
            this.lblDataUsage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progDataUsage
            // 
            this.progDataUsage.BackColor = System.Drawing.Color.White;
            this.progDataUsage.ForeColor = System.Drawing.Color.LimeGreen;
            this.progDataUsage.Location = new System.Drawing.Point(425, 347);
            this.progDataUsage.MarqueeAnimationSpeed = 99999;
            this.progDataUsage.Name = "progDataUsage";
            this.progDataUsage.Size = new System.Drawing.Size(398, 23);
            this.progDataUsage.TabIndex = 16;
            // 
            // progSdUsage
            // 
            this.progSdUsage.BackColor = System.Drawing.Color.White;
            this.progSdUsage.ForeColor = System.Drawing.Color.LimeGreen;
            this.progSdUsage.Location = new System.Drawing.Point(425, 376);
            this.progSdUsage.MarqueeAnimationSpeed = 99999;
            this.progSdUsage.Name = "progSdUsage";
            this.progSdUsage.Size = new System.Drawing.Size(398, 23);
            this.progSdUsage.TabIndex = 19;
            // 
            // lblSdUsage
            // 
            this.lblSdUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSdUsage.Location = new System.Drawing.Point(293, 380);
            this.lblSdUsage.Name = "lblSdUsage";
            this.lblSdUsage.Size = new System.Drawing.Size(114, 19);
            this.lblSdUsage.TabIndex = 18;
            this.lblSdUsage.Text = "---";
            this.lblSdUsage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(211, 380);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 19);
            this.label6.TabIndex = 17;
            this.label6.Text = "SD Usage:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnResetValues
            // 
            this.btnResetValues.Location = new System.Drawing.Point(7, 309);
            this.btnResetValues.Name = "btnResetValues";
            this.btnResetValues.Size = new System.Drawing.Size(180, 30);
            this.btnResetValues.TabIndex = 20;
            this.btnResetValues.Text = "Reset Values";
            this.btnResetValues.UseVisualStyleBackColor = true;
            this.btnResetValues.Click += new System.EventHandler(this.btnResetValues_Click);
            // 
            // btnApplyChanges
            // 
            this.btnApplyChanges.Location = new System.Drawing.Point(205, 437);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new System.Drawing.Size(180, 32);
            this.btnApplyChanges.TabIndex = 21;
            this.btnApplyChanges.Text = "Apply Changes";
            this.btnApplyChanges.UseVisualStyleBackColor = true;
            this.btnApplyChanges.Click += new System.EventHandler(this.btnApplyChanges_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 481);
            this.Controls.Add(this.btnApplyChanges);
            this.Controls.Add(this.btnResetValues);
            this.Controls.Add(this.progSdUsage);
            this.Controls.Add(this.lblSdUsage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progDataUsage);
            this.Controls.Add(this.lblDataUsage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnBackupData);
            this.Controls.Add(this.btnReadPartitions);
            this.Controls.Add(this.lblAvailSpace);
            this.Controls.Add(this.lblSpaceTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCache);
            this.Controls.Add(this.lblSdcard);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.numCache);
            this.Controls.Add(this.numSdcard);
            this.Controls.Add(this.numData);
            this.Controls.Add(this.rtbConsole);
            this.Name = "fmMain";
            this.Text = "FireParted v0.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSdcard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCache)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbConsole;
        private System.Windows.Forms.NumericUpDown numData;
        private System.Windows.Forms.NumericUpDown numSdcard;
        private System.Windows.Forms.NumericUpDown numCache;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblSdcard;
        private System.Windows.Forms.Label lblCache;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSpaceTitle;
        private System.Windows.Forms.Label lblAvailSpace;
        private System.Windows.Forms.Button btnReadPartitions;
        private System.Windows.Forms.Button btnBackupData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDataUsage;
        private System.Windows.Forms.ProgressBar progDataUsage;
        private System.Windows.Forms.ProgressBar progSdUsage;
        private System.Windows.Forms.Label lblSdUsage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnResetValues;
        private System.Windows.Forms.Button btnApplyChanges;
    }
}

