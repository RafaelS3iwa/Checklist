using Checklist.Classes;
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

            Cliente cliente = SessaoId.IdAtual;

            label4.Text = cliente.getId().ToString();
        }   

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(TxtNome.Text) || string.IsNullOrEmpty(CbTipo.Text) || string.IsNullOrEmpty(TxtDescricao.Text))
            {
                MessageBox.Show("Preencha todos os campos para cadastrar o item.", "Aviso", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string connectionString = "Data Source=.;initial catalog=Checklists;integrated security=true;";
                Cliente cliente = SessaoId.IdAtual;

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string insertQuery = "INSERT INTO Itens(nome_item, tipo, descricao) OUTPUT INSERTED.id_item VALUES(@nome_item, @tipo, @descricao)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@nome_item", TxtNome.Text);

                            string tipo = CbTipo.SelectedItem.ToString();
                            cmd.Parameters.AddWithValue("@tipo", tipo);

                            cmd.Parameters.AddWithValue("@descricao", TxtDescricao.Text);

                            int idItem = Convert.ToInt32(cmd.ExecuteScalar());

                            if (idItem > 0)
                            {
                                MessageBox.Show("Item inserido com Sucesso! Clique em Okay para atribuir ao Cliente.", "Aviso", MessageBoxButtons.OK);

                                string insertChecklistQuery = "INSERT INTO Checklist (id_cliente, id_item, condicao) VALUES (@id_cliente, @id_item, @condicao)";

                                using (SqlCommand cmdChecklist = new SqlCommand(insertChecklistQuery, conn))
                                {
                                    cmdChecklist.Parameters.AddWithValue("@id_cliente", cliente.getId());
                                    cmdChecklist.Parameters.AddWithValue("@id_item", idItem);
                                    cmdChecklist.Parameters.AddWithValue("@condicao", 0);

                                    int rows = cmdChecklist.ExecuteNonQuery();
                                    if (rows > 0)
                                    {
                                        MessageBox.Show("O Checklist foi atribuído com sucesso!", "Aviso", MessageBoxButtons.OK);
                                        this.Hide();

                                        FormADM formADM = new FormADM();
                                        formADM.checkBoxView();
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
    }
}
