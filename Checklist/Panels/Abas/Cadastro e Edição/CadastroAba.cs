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

namespace Checklist.Panels.Abas.Cadastro_e_Edição
{
    public partial class CadastroAba : Form
    {
        public CadastroAba()
        {
            InitializeComponent();
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAba.Text))
            {
                MessageBox.Show("Preencha o campo 'Aba' para realizar o cadastro.", "Aviso", MessageBoxButtons.OK);
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

                        string insertQuery = "INSERT INTO Abas (aba, id_cliente) VALUES (@aba, @id_cliente)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@id_cliente", cliente.getId()); 
                            cmd.Parameters.AddWithValue("@aba", TxtAba.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Aba inserida com sucesso!");

                                FormADM formADM = new FormADM();
                                formADM.CbAbasView();

                                this.Hide();
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
                    MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}");
                }
            }
        }

        private void CadastroAba_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
