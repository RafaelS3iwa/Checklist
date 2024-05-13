using Checklist.Classes;
using Checklist.Classes.Abas;
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

namespace Checklist.Panels
{
    public partial class CadastroItem : Form
    {
        public CadastroItem()
        {
            InitializeComponent();
        }   

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(TxtNome.Text) || string.IsNullOrWhiteSpace(CbAbas.Text) || string.IsNullOrWhiteSpace(TxtDescricao.Text))
            {
                MessageBox.Show("Preencha todos os campos para cadastrar o item.", "Aviso", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string connectionString = "Data Source=.;initial catalog=Checklists;integrated security=true;";
                Cliente cliente = SessaoId.IdAtual;
                Guia aba = SessaoAba.AbaAtual;

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string insertQuery = "INSERT INTO Itens(nome_item, tipo, descricao) OUTPUT INSERTED.id_item VALUES(@nome_item, @tipo, @descricao)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@nome_item", TxtNome.Text);
                            cmd.Parameters.AddWithValue("@tipo", aba.getNome());
                            cmd.Parameters.AddWithValue("@descricao", TxtDescricao.Text);

                            int idItem = Convert.ToInt32(cmd.ExecuteScalar());

                            if (idItem > 0)
                            {
                                MessageBox.Show("Item inserido com Sucesso! Clique em Okay para atribuir ao Cliente.", "Aviso", MessageBoxButtons.OK);

                                string insertChecklistQuery = "INSERT INTO Checklist (id_cliente, id_item, id_aba, condicao) VALUES (@id_cliente, @id_item, @id_aba, @condicao)";

                                using (SqlCommand cmdChecklist = new SqlCommand(insertChecklistQuery, conn))
                                {
                                    cmdChecklist.Parameters.AddWithValue("@id_cliente", cliente.getId());
                                    cmdChecklist.Parameters.AddWithValue("@id_item", idItem);
                                    cmdChecklist.Parameters.AddWithValue("@id_aba", aba.getId());
                                    cmdChecklist.Parameters.AddWithValue("@condicao", 0);

                                    int rows = cmdChecklist.ExecuteNonQuery();
                                    if (rows > 0)
                                    {
                                        MessageBox.Show("O Checklist foi atribuído com sucesso!", "Aviso", MessageBoxButtons.OK);
                                        this.Hide();

                                        FormADM formADM = new FormADM();

                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Nenhum registro foi inserido.");
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Error connecting to database: {ex.Message}");
                }
            }       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void CadastroItem_Load(object sender, EventArgs e)
        {
            SessaoAba.LimparAba();
            
            CarregarCategoria();
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
    }
}
