using Checklist.Classes.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist.Panels
{
    public partial class DetalheChecklist : Form
    {
        public DetalheChecklist()
        {
            InitializeComponent();

            Item item = SessaoItem.ItemAtual;

            LblNome.Text = item.getnome();
            LblTipo.Text = item.getTipo();
            TxtDescricao.Text = item.getDescricao();

            LblCriacao.Text = item.getCriacao().ToString("dd/MM/yyyy");
            LblAtualizacao.Text = item.getAtualizacao().ToString("dd/MM/yyyy");

            LblStatus.Text = item.getSituacao().ToString();
        }

        private void DetalheChecklist_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
