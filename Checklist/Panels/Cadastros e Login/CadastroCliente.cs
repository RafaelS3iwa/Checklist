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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Checklist.Panels
{
    public partial class CadastroCliente : Form
    {
        public CadastroCliente()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNome.Text))
            {
                MessageBox.Show("Preencha o campo 'Nome' para realizar o cadastro.", "Aviso", MessageBoxButtons.OK);
                return;
            }
            else
            {
                string connectionString = "Data Source=.;initial catalog=Checklists;integrated security=true;";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string insertQuery = "INSERT INTO Clientes (nome) VALUES (@nome)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@nome", TxtNome.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cliente inserido com sucesso!");

                                FormADM formADM = new FormADM();
                                formADM.checkBoxView();

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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
