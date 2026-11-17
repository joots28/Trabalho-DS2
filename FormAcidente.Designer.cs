namespace SistemaSeguro
{
    partial class FormAcidente
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtpData = new System.Windows.Forms.DateTimePicker();
            this.txtHora = new System.Windows.Forms.TextBox();
            this.txtEndereco = new System.Windows.Forms.TextBox();
            this.txtMotivo = new System.Windows.Forms.TextBox();
            this.txtEnvolvidos = new System.Windows.Forms.TextBox();
            this.txtDanos = new System.Windows.Forms.TextBox();
            this.cmbAutomovel = new System.Windows.Forms.ComboBox();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.dgvAcidentes = new System.Windows.Forms.DataGridView();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.panelFormCard = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblFormSubtitle = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.lblHora = new System.Windows.Forms.Label();
            this.lblEndereco = new System.Windows.Forms.Label();
            this.lblMotivo = new System.Windows.Forms.Label();
            this.lblEnvolvidos = new System.Windows.Forms.Label();
            this.lblDanos = new System.Windows.Forms.Label();
            this.lblAutomovel = new System.Windows.Forms.Label();
            this.panelGridCard = new System.Windows.Forms.Panel();
            this.lblGridTitle = new System.Windows.Forms.Label();
            this.lblGridSubtitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAcidentes)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelFormCard.SuspendLayout();
            this.panelGridCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.panelHeader.Controls.Add(this.lblHeaderTitle);
            this.panelHeader.Controls.Add(this.lblHeaderSubtitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(32, 0, 32, 0);
            this.panelHeader.Size = new System.Drawing.Size(1024, 85);
            this.panelHeader.TabIndex = 100;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblHeaderTitle.Location = new System.Drawing.Point(32, 16);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(288, 37);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "Registro de Acidentes";
            // 
            // lblHeaderSubtitle
            // 
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(34, 50);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(293, 20);
            this.lblHeaderSubtitle.TabIndex = 1;
            this.lblHeaderSubtitle.Text = "Registro e consulta de acidentes e sinistros.";
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelBody.Controls.Add(this.panelFormCard);
            this.panelBody.Controls.Add(this.panelGridCard);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 85);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.panelBody.Size = new System.Drawing.Size(1024, 556);
            this.panelBody.TabIndex = 101;
            // 
            // panelFormCard
            // 
            this.panelFormCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelFormCard.BackColor = System.Drawing.Color.White;
            this.panelFormCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFormCard.Controls.Add(this.lblFormTitle);
            this.panelFormCard.Controls.Add(this.lblFormSubtitle);
            this.panelFormCard.Controls.Add(this.lblData);
            this.panelFormCard.Controls.Add(this.dtpData);
            this.panelFormCard.Controls.Add(this.lblHora);
            this.panelFormCard.Controls.Add(this.txtHora);
            this.panelFormCard.Controls.Add(this.lblEndereco);
            this.panelFormCard.Controls.Add(this.txtEndereco);
            this.panelFormCard.Controls.Add(this.lblMotivo);
            this.panelFormCard.Controls.Add(this.txtMotivo);
            this.panelFormCard.Controls.Add(this.lblEnvolvidos);
            this.panelFormCard.Controls.Add(this.txtEnvolvidos);
            this.panelFormCard.Controls.Add(this.lblDanos);
            this.panelFormCard.Controls.Add(this.txtDanos);
            this.panelFormCard.Controls.Add(this.lblAutomovel);
            this.panelFormCard.Controls.Add(this.cmbAutomovel);
            this.panelFormCard.Controls.Add(this.btnSalvar);
            this.panelFormCard.Location = new System.Drawing.Point(24, 20);
            this.panelFormCard.Name = "panelFormCard";
            this.panelFormCard.Padding = new System.Windows.Forms.Padding(20);
            this.panelFormCard.Size = new System.Drawing.Size(380, 516);
            this.panelFormCard.TabIndex = 0;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblFormTitle.Location = new System.Drawing.Point(20, 20);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(189, 28);
            this.lblFormTitle.TabIndex = 20;
            this.lblFormTitle.Text = "Dados do Acidente";
            // 
            // lblFormSubtitle
            // 
            this.lblFormSubtitle.AutoSize = true;
            this.lblFormSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFormSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblFormSubtitle.Location = new System.Drawing.Point(20, 44);
            this.lblFormSubtitle.Name = "lblFormSubtitle";
            this.lblFormSubtitle.Size = new System.Drawing.Size(256, 20);
            this.lblFormSubtitle.TabIndex = 21;
            this.lblFormSubtitle.Text = "Preencha as informações do sinistro:";
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblData.Location = new System.Drawing.Point(20, 74);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(46, 20);
            this.lblData.TabIndex = 22;
            this.lblData.Text = "Data:";
            // 
            // dtpData
            // 
            this.dtpData.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpData.Location = new System.Drawing.Point(20, 92);
            this.dtpData.Name = "dtpData";
            this.dtpData.Size = new System.Drawing.Size(320, 29);
            this.dtpData.TabIndex = 0;
            // 
            // lblHora
            // 
            this.lblHora.AutoSize = true;
            this.lblHora.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblHora.Location = new System.Drawing.Point(20, 124);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(48, 20);
            this.lblHora.TabIndex = 23;
            this.lblHora.Text = "Hora:";
            // 
            // txtHora
            // 
            this.txtHora.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtHora.Location = new System.Drawing.Point(20, 142);
            this.txtHora.Name = "txtHora";
            this.txtHora.Size = new System.Drawing.Size(320, 29);
            this.txtHora.TabIndex = 1;
            this.txtHora.Tag = "";
            this.txtHora.Text = "Hora";
            // 
            // lblEndereco
            // 
            this.lblEndereco.AutoSize = true;
            this.lblEndereco.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEndereco.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblEndereco.Location = new System.Drawing.Point(20, 174);
            this.lblEndereco.Name = "lblEndereco";
            this.lblEndereco.Size = new System.Drawing.Size(77, 20);
            this.lblEndereco.TabIndex = 24;
            this.lblEndereco.Text = "Endereço:";
            // 
            // txtEndereco
            // 
            this.txtEndereco.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtEndereco.Location = new System.Drawing.Point(20, 192);
            this.txtEndereco.Name = "txtEndereco";
            this.txtEndereco.Size = new System.Drawing.Size(320, 29);
            this.txtEndereco.TabIndex = 2;
            this.txtEndereco.Text = "Endereço";
            // 
            // lblMotivo
            // 
            this.lblMotivo.AutoSize = true;
            this.lblMotivo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMotivo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblMotivo.Location = new System.Drawing.Point(20, 224);
            this.lblMotivo.Name = "lblMotivo";
            this.lblMotivo.Size = new System.Drawing.Size(63, 20);
            this.lblMotivo.TabIndex = 25;
            this.lblMotivo.Text = "Motivo:";
            // 
            // txtMotivo
            // 
            this.txtMotivo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtMotivo.Location = new System.Drawing.Point(20, 242);
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.Size = new System.Drawing.Size(320, 29);
            this.txtMotivo.TabIndex = 3;
            this.txtMotivo.Text = "Motivo";
            // 
            // lblEnvolvidos
            // 
            this.lblEnvolvidos.AutoSize = true;
            this.lblEnvolvidos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEnvolvidos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblEnvolvidos.Location = new System.Drawing.Point(20, 274);
            this.lblEnvolvidos.Name = "lblEnvolvidos";
            this.lblEnvolvidos.Size = new System.Drawing.Size(89, 20);
            this.lblEnvolvidos.TabIndex = 26;
            this.lblEnvolvidos.Text = "Envolvidos:";
            // 
            // txtEnvolvidos
            // 
            this.txtEnvolvidos.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtEnvolvidos.Location = new System.Drawing.Point(20, 292);
            this.txtEnvolvidos.Name = "txtEnvolvidos";
            this.txtEnvolvidos.Size = new System.Drawing.Size(320, 29);
            this.txtEnvolvidos.TabIndex = 4;
            this.txtEnvolvidos.Text = "Envolvidos";
            // 
            // lblDanos
            // 
            this.lblDanos.AutoSize = true;
            this.lblDanos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDanos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblDanos.Location = new System.Drawing.Point(20, 324);
            this.lblDanos.Name = "lblDanos";
            this.lblDanos.Size = new System.Drawing.Size(57, 20);
            this.lblDanos.TabIndex = 27;
            this.lblDanos.Text = "Danos:";
            // 
            // txtDanos
            // 
            this.txtDanos.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDanos.Location = new System.Drawing.Point(20, 342);
            this.txtDanos.Name = "txtDanos";
            this.txtDanos.Size = new System.Drawing.Size(320, 29);
            this.txtDanos.TabIndex = 5;
            this.txtDanos.Text = "Danos";
            // 
            // lblAutomovel
            // 
            this.lblAutomovel.AutoSize = true;
            this.lblAutomovel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAutomovel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblAutomovel.Location = new System.Drawing.Point(20, 374);
            this.lblAutomovel.Name = "lblAutomovel";
            this.lblAutomovel.Size = new System.Drawing.Size(90, 20);
            this.lblAutomovel.TabIndex = 28;
            this.lblAutomovel.Text = "Automóvel:";
            // 
            // cmbAutomovel
            // 
            this.cmbAutomovel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbAutomovel.FormattingEnabled = true;
            this.cmbAutomovel.Location = new System.Drawing.Point(20, 392);
            this.cmbAutomovel.Name = "cmbAutomovel";
            this.cmbAutomovel.Size = new System.Drawing.Size(320, 29);
            this.cmbAutomovel.TabIndex = 6;
            // 
            // btnSalvar
            // 
            this.btnSalvar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalvar.FlatAppearance.BorderSize = 0;
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(20, 434);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(320, 42);
            this.btnSalvar.TabIndex = 7;
            this.btnSalvar.Text = "Salvar Acidente";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // panelGridCard
            // 
            this.panelGridCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGridCard.BackColor = System.Drawing.Color.White;
            this.panelGridCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGridCard.Controls.Add(this.lblGridTitle);
            this.panelGridCard.Controls.Add(this.lblGridSubtitle);
            this.panelGridCard.Controls.Add(this.dgvAcidentes);
            this.panelGridCard.Location = new System.Drawing.Point(420, 20);
            this.panelGridCard.Name = "panelGridCard";
            this.panelGridCard.Padding = new System.Windows.Forms.Padding(20);
            this.panelGridCard.Size = new System.Drawing.Size(580, 516);
            this.panelGridCard.TabIndex = 1;
            // 
            // lblGridTitle
            // 
            this.lblGridTitle.AutoSize = true;
            this.lblGridTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblGridTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblGridTitle.Location = new System.Drawing.Point(20, 20);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new System.Drawing.Size(223, 28);
            this.lblGridTitle.TabIndex = 30;
            this.lblGridTitle.Text = "Acidentes Registrados";
            // 
            // lblGridSubtitle
            // 
            this.lblGridSubtitle.AutoSize = true;
            this.lblGridSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblGridSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblGridSubtitle.Location = new System.Drawing.Point(20, 44);
            this.lblGridSubtitle.Name = "lblGridSubtitle";
            this.lblGridSubtitle.Size = new System.Drawing.Size(164, 20);
            this.lblGridSubtitle.TabIndex = 31;
            this.lblGridSubtitle.Text = "Registros cadastrados:";
            // 
            // dgvAcidentes
            // 
            this.dgvAcidentes.AllowUserToAddRows = false;
            this.dgvAcidentes.AllowUserToDeleteRows = false;
            this.dgvAcidentes.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dgvAcidentes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAcidentes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAcidentes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAcidentes.BackgroundColor = System.Drawing.Color.White;
            this.dgvAcidentes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAcidentes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvAcidentes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAcidentes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAcidentes.ColumnHeadersHeight = 38;
            this.dgvAcidentes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAcidentes.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvAcidentes.EnableHeadersVisualStyles = false;
            this.dgvAcidentes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvAcidentes.Location = new System.Drawing.Point(20, 70);
            this.dgvAcidentes.Name = "dgvAcidentes";
            this.dgvAcidentes.ReadOnly = true;
            this.dgvAcidentes.RowHeadersVisible = false;
            this.dgvAcidentes.RowTemplate.Height = 32;
            this.dgvAcidentes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAcidentes.Size = new System.Drawing.Size(538, 424);
            this.dgvAcidentes.TabIndex = 8;
            // 
            // FormAcidente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1024, 641);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.MinimumSize = new System.Drawing.Size(960, 600);
            this.Name = "FormAcidente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registro de Acidentes";
            this.Load += new System.EventHandler(this.FormAcidente_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAcidentes)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelBody.ResumeLayout(false);
            this.panelFormCard.ResumeLayout(false);
            this.panelFormCard.PerformLayout();
            this.panelGridCard.ResumeLayout(false);
            this.panelGridCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpData;
        private System.Windows.Forms.TextBox txtHora;
        private System.Windows.Forms.TextBox txtEndereco;
        private System.Windows.Forms.TextBox txtMotivo;
        private System.Windows.Forms.TextBox txtEnvolvidos;
        private System.Windows.Forms.TextBox txtDanos;
        private System.Windows.Forms.ComboBox cmbAutomovel;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.DataGridView dgvAcidentes;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Panel panelFormCard;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblFormSubtitle;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblHora;
        private System.Windows.Forms.Label lblEndereco;
        private System.Windows.Forms.Label lblMotivo;
        private System.Windows.Forms.Label lblEnvolvidos;
        private System.Windows.Forms.Label lblDanos;
        private System.Windows.Forms.Label lblAutomovel;
        private System.Windows.Forms.Panel panelGridCard;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.Label lblGridSubtitle;
    }
}