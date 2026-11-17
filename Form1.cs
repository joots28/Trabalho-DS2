using SistemaSeguro.DAO;
using SistemaSeguro.Model;
using System;
using System.Windows.Forms;

namespace SistemaSeguro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Evento executado ao abrir o formulário
        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        // Método para atualizar o DataGridView com os dados da base de dados
        private void CarregarGrid()
        {
            try
            {
                ClienteDAO dao = new ClienteDAO();
                dgvClientes.DataSource = dao.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar lista de clientes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento do botão Salvar
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Instanciar o modelo do Cliente com os dados preenchidos na tela
                Cliente novoCliente = new Cliente
                {
                    Nome = txtNome.Text,
                    Endereco = txtEndereco.Text,
                    Telefone = txtTelefone.Text,
                    Email = txtEmail.Text,
                    DataNascimento = dtpNascimento.Value,
                    Profissao = txtProfissao.Text
                };

                // 2. Chamar o método de cadastro da classe DAO
                ClienteDAO dao = new ClienteDAO();
                if (dao.Cadastrar(novoCliente))
                {
                    MessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 3. Limpar os campos e atualizar a tabela
                    LimparCampos();
                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar cliente: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método auxiliar para limpar as caixas de texto após guardar
        private void LimparCampos()
        {
            txtNome.Clear();
            txtEndereco.Clear();
            txtTelefone.Clear();
            txtEmail.Clear();
            txtProfissao.Clear();
            dtpNascimento.Value = DateTime.Now;
            txtNome.Focus();
        }
    }
}