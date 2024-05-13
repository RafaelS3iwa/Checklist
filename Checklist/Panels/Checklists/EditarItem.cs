using Checklist.Classes.Abas;
using Checklist.Classes;
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
            TxtDescricao.Text = item.getDescricao();

            LblCriacao.Text = item.getCriacao().ToString("dd/MM/yyyy");
            LblAtualizacao.Text = item.getAtualizacao().ToString("dd/MM/yyyy");

            if (item.getSituacao() == 0)
            {
                LblStatus.Text = "Não realizado";
            }
            else
            {
                LblStatus.Text = "Realizado";
            }

            CarregarCategoria();

            SessaoAba.LimparAba();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DetalheChecklistADM detalheChecklistADM = new DetalheChecklistADM();
            detalheChecklistADM.Close();

            Item itens = SessaoItem.ItemAtual;
            Guia guia = SessaoAba.AbaAtual;

            using (SqlConnection conn = new SqlConnection("Data Source=.;initial catalog=Checklists;integrated security=true;"))
            {
                conn.Open();
                string sql = "UPDATE Itens SET nome_item=@nome_item, tipo=@tipo, descricao=@descricao, update_time=GETDATE() WHERE id_item=@id_item";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_item", itens.getId());
                    cmd.Parameters.AddWithValue("@nome_item", TxtNome.Text);

                    cmd.Parameters.AddWithValue("@tipo", guia.getNome());
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

        private void CbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item item = SessaoItem.ItemAtual;
            if (CbAbas.SelectedItem is DataRowView selectedRow)
            {
                if (selectedRow.Row["id_aba"] is int id)
                {
                    string aba = selectedRow["aba"].ToString();
                    Guia guia = new Guia(id, aba);
                    SessaoAba.ArmazenarAba(guia);
                }
            }
        }

        public void CarregarCategoria()
        {
            string connectionString = "Data Source=.;initial catalog=Checklists;integrated security=true;";
            Cliente cliente = SessaoId.IdAtual;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Abas WHERE id_cliente=@id_cliente", conn);
                    cmd.Parameters.AddWithValue("@id_cliente", cliente.getId());

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    CbAbas.DataSource = table;
                    CbAbas.DisplayMember = "aba";
                    CbAbas.ValueMember = "id_aba";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error connecting to database: {ex.Message}");
            }
        }

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
