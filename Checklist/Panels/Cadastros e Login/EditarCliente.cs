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

namespace Checklist.Panels.Cadastros_e_Login
{
    public partial class EditarCliente : Form
    {
        public EditarCliente()
        {
            InitializeComponent();

            Cliente cliente = SessaoId.IdAtual;

            TxtNome.Text = cliente.getNome();
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            Cliente cliente = SessaoId.IdAtual;

            if (TxtNome.Text == cliente.getNome() || string.IsNullOrWhiteSpace(TxtNome.Text))
            {
                MessageBox.Show("O cliente não foi alterado.", "Aviso", MessageBoxButtons.OK);
                this.Close();
            }
            else
            {
                using (SqlConnection conn = new SqlConnection("Data Source=.;initial catalog=Checklists;integrated security=true;"))
                {
                    conn.Open();

                    string sql = "UPDATE Clientes SET nome=@nome WHERE id_cliente=@id_cliente";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_cliente", cliente.getId());
                        cmd.Parameters.AddWithValue("@nome", TxtNome.Text);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("A atualização foi bem sucedida!", "Sucesso", MessageBoxButtons.OK);
                        this.Close();

                        FormADM formADM = new FormADM();
                        formADM.checkBoxView();
                    }
                }
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            Cliente cliente = SessaoId.IdAtual;

            DialogResult resultado = MessageBox.Show("Tem certeza que deseja excluir este cliente?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if(resultado == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=.;initial catalog=Checklists;integrated security=true;"))
                    {
                        conn.Open();

                        string deleteItem = "DELETE FROM Itens WHERE id_item IN (SELECT id_item FROM Checklist WHERE id_cliente=@id_cliente)";

                        string deleteChecklist = "DELETE FROM Checklist WHERE id_cliente=@id_cliente";

                        string deleteCliente = "DELETE FROM Clientes WHERE id_cliente=@id_cliente";

                        using (SqlCommand cmd = new SqlCommand(deleteItem, conn))
                        {
                            cmd.Parameters.AddWithValue("@id_cliente", cliente.getId());

                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(deleteChecklist, conn))
                        {
                            cmd.Parameters.AddWithValue("@id_cliente", cliente.getId());

                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(deleteCliente, conn))
                        {
                            cmd.Parameters.AddWithValue("@id_cliente", cliente.getId());

                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show($"O cliente foi excluído com sucesso.");
                        this.Close();

                        FormADM formADM = new FormADM();
                        formADM.checkBoxView();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error connecting to database: {ex.Message}");
                }
            }     
        }
    }
}
