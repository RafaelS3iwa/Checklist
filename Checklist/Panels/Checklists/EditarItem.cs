using Checklist.Classes.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist.Panels.Checklists
{
    public partial class EditarItem : Form
    {
        public EditarItem()
        {
            InitializeComponent();
        }

        private void EditarItem_Load(object sender, EventArgs e)
        {
            Item item = SessaoItem.ItemAtual;

            TxtNome.Text = item.getnome();
            CbTipo.Text = item.getTipo();
            TxtDescricao.Text = item.getDescricao();

            LblCriacao.Text = item.getCriacao().ToString("dd/MM/yyyy");
            LblAtualizacao.Text = item.getAtualizacao().ToString("dd/MM/yyyy");

            LblStatus.Text = item.getSituacao().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Item itens = SessaoItem.ItemAtual;

            using (SqlConnection conn = new SqlConnection("Data Source=.;initial catalog=Checklists;integrated security=true;"))
            {
                conn.Open();
                string sql = "UPDATE Itens SET nome_item=@nome_item, tipo=@tipo, descricao=@descricao, update_time=GETDATE() WHERE id_item=@id_item";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_item", itens.getId());
                    cmd.Parameters.AddWithValue("@nome_item", TxtNome.Text);

                    string tipo = CbTipo.SelectedItem.ToString();
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@descricao", TxtDescricao.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("A atualização foi bem sucedida!", "Sucesso", MessageBoxButtons.OK);
                    this.Close();

                    FormADM formADM = new FormADM();
                    formADM.checkBoxView();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
