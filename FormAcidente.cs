using System;
using System.Data;
using System.Windows.Forms;
using SistemaSeguro.DAO;
using SistemaSeguro.Model;

namespace SistemaSeguro
{
    public partial class FormAcidente : Form
    {
        public FormAcidente()
        {
            InitializeComponent();
        }

        private void FormAcidente_Load(object sender, EventArgs e)
        {
            CarregarAutomoveisCombo();
            CarregarGrid();
        }

        private void CarregarAutomoveisCombo()
        {
            try
            {
                AutomovelDAO dao = new AutomovelDAO();
                DataTable tabela = dao.Listar();

                cmbAutomovel.DataSource = tabela;
                cmbAutomovel.DisplayMember = "Placa"; // Exibe a Placa do Veículo no ComboBox
                cmbAutomovel.ValueMember = "ID";      // Guarda o ID do Automóvel (FK)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar veículos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarGrid()
        {
            try
            {
                AcidenteDAO dao = new AcidenteDAO();
                dgvAcidentes.DataSource = dao.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar lista de acidentes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Acidente acidente = new Acidente
                {
                    DataAcidente = dtpData.Value,
                    HoraAcidente = TimeSpan.Parse(txtHora.Text),
                    EnderecoAcidente = txtEndereco.Text,
                    MotivoAcidente = txtMotivo.Text,
                    AutomoveisEnvolvidos = txtEnvolvidos.Text,
                    DanosTerceiros = txtDanos.Text,
                    IdAutomovel = Convert.ToInt32(cmbAutomovel.SelectedValue)
                };

                AcidenteDAO dao = new AcidenteDAO();
                if (dao.Cadastrar(acidente))
                {
                    MessageBox.Show("Acidente registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar acidente: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            txtHora.Clear();
            txtEndereco.Clear();
            txtMotivo.Clear();
            txtEnvolvidos.Clear();
            txtDanos.Clear();
            dtpData.Value = DateTime.Now;
            txtHora.Focus();
        }
    }
}