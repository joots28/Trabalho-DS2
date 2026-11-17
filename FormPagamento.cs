using System;
using System.Data;
using System.Windows.Forms;
using SistemaSeguro.DAO;
using SistemaSeguro.Model;

namespace SistemaSeguro
{
    public partial class FormPagamento : Form
    {
        public FormPagamento()
        {
            InitializeComponent();
        }

        private void FormPagamento_Load(object sender, EventArgs e)
        {
            CarregarClientesCombo();
            CarregarAcidentesCombo();
            CarregarGrid();
        }

        private void CarregarClientesCombo()
        {
            try
            {
                ClienteDAO dao = new ClienteDAO();
                cmbCliente.DataSource = dao.Listar();
                cmbCliente.DisplayMember = "Nome";
                cmbCliente.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar clientes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarAcidentesCombo()
        {
            try
            {
                AcidenteDAO dao = new AcidenteDAO();
                cmbAcidente.DataSource = dao.Listar();
                cmbAcidente.DisplayMember = "ID"; // Exibe o ID do Sinistro
                cmbAcidente.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar acidentes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarGrid()
        {
            try
            {
                PagamentoSeguroDAO dao = new PagamentoSeguroDAO();
                dgvPagamentos.DataSource = dao.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar pagamentos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                PagamentoSeguro pgto = new PagamentoSeguro
                {
                    Valor = decimal.Parse(txtValor.Text),
                    DataPagamento = dtpData.Value,
                    IdCliente = Convert.ToInt32(cmbCliente.SelectedValue),
                    IdAcidente = Convert.ToInt32(cmbAcidente.SelectedValue)
                };

                PagamentoSeguroDAO dao = new PagamentoSeguroDAO();
                if (dao.Cadastrar(pgto))
                {
                    MessageBox.Show("Pagamento registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar pagamento: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            txtValor.Clear();
            dtpData.Value = DateTime.Now;
            txtValor.Focus();
        }
    }
}