using System;
using System.Data;
using System.Windows.Forms;
using SistemaSeguro.DAO;
using SistemaSeguro.Model;

namespace SistemaSeguro
{
    public partial class FormAutomovel : Form
    {
        public FormAutomovel()
        {
            InitializeComponent();
        }

        // Executado ao abrir a tela
        private void FormAutomovel_Load(object sender, EventArgs e)
        {
            CarregarClientesCombo();
            CarregarGrid();
        }

        // Preenche o ComboBox com os clientes vindos do banco de dados
        private void CarregarClientesCombo()
        {
            try
            {
                ClienteDAO dao = new ClienteDAO();
                DataTable tabela = dao.Listar();

                cmbCliente.DataSource = tabela;
                cmbCliente.DisplayMember = "Nome"; // Texto visível no ComboBox
                cmbCliente.ValueMember = "ID";      // ID real armazenado (FK)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar clientes no seletor: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Atualiza a tabela com a lista de automóveis
        private void CarregarGrid()
        {
            try
            {
                AutomovelDAO dao = new AutomovelDAO();
                dgvAutomoveis.DataSource = dao.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar automóveis: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento do botão Salvar
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                // Instancia o automóvel pegando o ID do cliente selecionado no ComboBox
                Automovel auto = new Automovel
                {
                    Placa = txtPlaca.Text,
                    Modelo = txtModelo.Text,
                    Marca = txtMarca.Text,
                    Ano = int.Parse(txtAno.Text),
                    IdCliente = Convert.ToInt32(cmbCliente.SelectedValue)
                };

                AutomovelDAO dao = new AutomovelDAO();
                if (dao.Cadastrar(auto))
                {
                    MessageBox.Show("Automóvel cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar automóvel: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            txtPlaca.Clear();
            txtModelo.Clear();
            txtMarca.Clear();
            txtAno.Clear();
            txtPlaca.Focus();
        }
    }
}