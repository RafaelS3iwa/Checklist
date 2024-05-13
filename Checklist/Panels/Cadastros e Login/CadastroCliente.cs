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
            if (string.IsNullOrEmpty(TxtNome.Text) || string.IsNullOrWhiteSpace(TxtAba.Text))
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

                        string insertQuery = "INSERT INTO Clientes (nome) OUTPUT INSERTED.id_cliente VALUES (@nome)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@nome", TxtNome.Text);

                            int idCliente = Convert.ToInt32(cmd.ExecuteScalar());

                            if (idCliente > 0)
                            {
                                string adicionarAba = "INSERT INTO Abas (aba, id_cliente) VALUES (@aba, @id_cliente)"; 

                                using(SqlCommand command = new SqlCommand(adicionarAba, conn))
                                {
                                    command.Parameters.AddWithValue("@aba", TxtAba.Text);
                                    command.Parameters.AddWithValue("@id_cliente", idCliente);

                                    int rows = command.ExecuteNonQuery();
                                    if (rows > 0)
                                    {
                                        MessageBox.Show("Cliente inserido com sucesso!");

                                        FormADM formADM = new FormADM();
                                        formADM.checkBoxView();

                                        this.Close();
                                    }
                                }
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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
