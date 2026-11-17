using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaSeguro
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void cilentesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 telaCliente = new Form1();
            telaCliente.ShowDialog();
        }

        private void automoveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAutomovel telaAuto = new FormAutomovel();
            telaAuto.ShowDialog();
        }

        private void acidentesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAcidente telaAcidente = new FormAcidente();
            telaAcidente.ShowDialog();
        }

        private void pagamentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPagamento telaPgto = new FormPagamento();
            telaPgto.ShowDialog();
        }
    }
}
