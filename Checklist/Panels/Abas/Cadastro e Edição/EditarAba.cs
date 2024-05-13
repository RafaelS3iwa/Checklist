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

namespace Checklist.Panels.Abas.Cadastro_e_Edição
{
    public partial class EditarAba : Form
    {
        public EditarAba()
        {
            InitializeComponent();
        }

        private void EditarAba_Load(object sender, EventArgs e)
        {
            Guia aba = SessaoAba.AbaAtual;

            TxtAntes.Text = aba.getNome();
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAba.Text))
            {
                MessageBox.Show("A aba não foi atualizada.", "Aviso", MessageBoxButtons.OK);
                this.Close();
            }
            else
            {
                string connectionString = "Data Source=.;initial catalog=Checklists;integrated security=true;";
            
                Guia guia = SessaoAba.AbaAtual;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string insertQuery = "UPDATE Abas SET aba=@aba WHERE id_aba=@id_aba";
                        string updateItem = "UPDATE Itens SET tipo = @tipo WHERE id_item IN(SELECT id_item FROM Checklist WHERE id_aba = @id_aba)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id_aba", guia.getId());
                            cmd.Parameters.AddWithValue("@aba", TxtAba.Text);

                            cmd.ExecuteNonQuery();
  
                        }
                        using (SqlCommand cmd = new SqlCommand(updateItem, conn))
                        {
                            cmd.Parameters.AddWithValue("@id_aba", guia.getId());
                            cmd.Parameters.AddWithValue("@tipo", TxtAba.Text); 

                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("A atualização foi bem sucedida!", "Sucesso", MessageBoxButtons.OK);
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

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            Guia guia = SessaoAba.AbaAtual;

            DialogResult resultado = MessageBox.Show("Tem certeza que deseja excluir esta aba?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=.;initial catalog=Checklists;integrated security=true;"))
                    {
                        conn.Open();
                        string deleteChecklist = "DELETE FROM Checklist WHERE id_aba=@id_aba";
                        string deleteItens = "DELETE FROM Itens WHERE id_item NOT IN (SELECT id_item FROM Checklist)";

                        using (SqlCommand cmd = new SqlCommand(deleteItens, conn))
                        {
                            cmd.Parameters.AddWithValue("@id_aba", guia.getId());
                            cmd.ExecuteNonQuery();
                        }
                        using (SqlCommand cmd = new SqlCommand(deleteChecklist, conn))
                        {
                            cmd.Parameters.AddWithValue("@id_aba", guia.getId());
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error connecting to database: {ex.Message}");
                }
            }
            else
            {
                return;
            }
        }

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
