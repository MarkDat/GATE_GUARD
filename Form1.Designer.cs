namespace GATE_GUARD2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtPlate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plateImg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineOutIn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Accept = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtPlates = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateSend = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 188);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 198);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.name,
            this.txtPlates,
            this.dateSend});
            this.dataGridView2.Location = new System.Drawing.Point(390, 32);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(529, 354);
            this.dataGridView2.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.line,
            this.txtPlate,
            this.plateImg,
            this.lineOutIn,
            this.Accept});
            this.dataGridView1.Location = new System.Drawing.Point(12, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(372, 139);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // line
            // 
            this.line.DataPropertyName = "line";
            this.line.HeaderText = "Line";
            this.line.Name = "line";
            this.line.ReadOnly = true;
            // 
            // txtPlate
            // 
            this.txtPlate.DataPropertyName = "txtPlate";
            this.txtPlate.HeaderText = "Plate";
            this.txtPlate.Name = "txtPlate";
            this.txtPlate.ReadOnly = true;
            // 
            // plateImg
            // 
            this.plateImg.DataPropertyName = "plateImg";
            this.plateImg.HeaderText = "IMG";
            this.plateImg.Name = "plateImg";
            this.plateImg.ReadOnly = true;
            // 
            // lineOutIn
            // 
            this.lineOutIn.DataPropertyName = "lineOutIn";
            this.lineOutIn.HeaderText = "lineOutIn";
            this.lineOutIn.Name = "lineOutIn";
            this.lineOutIn.ReadOnly = true;
            // 
            // Accept
            // 
            this.Accept.DataPropertyName = "Accept";
            this.Accept.HeaderText = "Accept";
            this.Accept.Name = "Accept";
            this.Accept.ReadOnly = true;
            this.Accept.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Accept.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Accept.Text = "Accept";
            this.Accept.UseColumnTextForButtonValue = true;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "IDS";
            this.ID.FillWeight = 71.06599F;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            // 
            // name
            // 
            this.name.DataPropertyName = "Name";
            this.name.FillWeight = 109.6447F;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            // 
            // txtPlates
            // 
            this.txtPlates.DataPropertyName = "TxtPlate";
            this.txtPlates.FillWeight = 109.6447F;
            this.txtPlates.HeaderText = "Plate";
            this.txtPlates.Name = "txtPlates";
            // 
            // dateSend
            // 
            this.dateSend.DataPropertyName = "DateIn";
            this.dateSend.FillWeight = 109.6447F;
            this.dateSend.HeaderText = "DateSend";
            this.dateSend.Name = "dateSend";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 401);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn line;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtPlate;
        private System.Windows.Forms.DataGridViewTextBoxColumn plateImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineOutIn;
        private System.Windows.Forms.DataGridViewButtonColumn Accept;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtPlates;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateSend;
    }
}

