using Checklist.Classes;
using Checklist.Classes.Abas;
using Checklist.Panels;
using Checklist.Panels.Abas.ADM;
using Checklist.Panels.Abas.Cadastro_e_Edição;
using Checklist.Panels.Abas.User;
using Checklist.Panels.Cadastros_e_Login;
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

namespace Checklist
{
    public partial class FormADM : Form
    {
        public FormADM()
        {
            InitializeComponent();
        }

        private void FormADM_Load(object sender, EventArgs e)
        {
            checkBoxView();
            CbAbasView();
        }

        public void checkBoxView()
        {
            string connectionString = "Data Source=.;initial catalog=Checklists;integrated security=true;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Clientes", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    DataRow clienteRow = table.NewRow();
                    clienteRow["nome"] = "Selecionar Cliente";
                    table.Rows.InsertAt(clienteRow, 0);


                    DataRow adicionarClienteRow = table.NewRow();
                    adicionarClienteRow["nome"] = "Adicionar Cliente";
                    table.Rows.InsertAt(adicionarClienteRow, 1);

                    comboBox1.DataSource = table;
                    comboBox1.DisplayMember = "nome";
                    comboBox1.ValueMember = "id_cliente";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error connecting to database: {ex.Message}");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is DataRowView selectedRow)
            {
                if (selectedRow.Row["nome"].ToString() == "Selecionar Cliente")
                {
                    return;
                }
                else
                {
                    DataTable table = (DataTable)comboBox1.DataSource;
                    DataRow[] rowsToDelete = table.Select("nome = 'Selecionar Cliente'");
                    foreach (DataRow rowToDelete in rowsToDelete)
                    {
                        table.Rows.Remove(rowToDelete);
                        comboBox1.SelectedItem = selectedRow;
                    }

                    if (selectedRow.Row["nome"].ToString() == "Adicionar Cliente")
                    {
                        CadastroCliente cadastroCliente = new CadastroCliente();
                        cadastroCliente.Show();
                    }
                    else if (selectedRow.Row["id_cliente"] is int id)
                    {
                        string nome = selectedRow["nome"].ToString();

                        Cliente cliente = new Cliente(id, nome);
                        SessaoId.ArmazenarId(cliente);

                        CbAbasView();
                        FecharAbas();
                    }
                }     
            }
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            Cliente cliente = SessaoId.IdAtual;
            if (cliente != null)
            {
                CadastroItem cadastroItem = new CadastroItem();
                cadastroItem.Show();
            }
            else
            {
                MessageBox.Show("Selecione um cliente para adicionar um item.", "Aviso", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Guia guia = SessaoAba.AbaAtual;

            if(guia != null)
            {
                EditarAba editarAba = new EditarAba();
                editarAba.Show();
            }
            else
            {
                MessageBox.Show("Selecione um cliente e uma aba para editar.", "Aviso", MessageBoxButtons.OK);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cliente cliente = SessaoId.IdAtual;
            if (cliente != null)
            {
                EditarCliente editarCliente = new EditarCliente();
                editarCliente.Show();
            }
            else
            {
                MessageBox.Show("Selecione um cliente para editar.", "Aviso", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void CbAbas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cliente cliente = SessaoId.IdAtual;
            if (CbAbas.SelectedItem is DataRowView selectedRow)
            {
                if(selectedRow.Row["aba"].ToString() == "Selecionar Aba")
                {
                    return;
                }
                else
                {
                    DataTable table = (DataTable)CbAbas.DataSource;
                    DataRow[] rowsToDelete = table.Select("aba = 'Selecionar Aba'");
                    foreach (DataRow rowToDelete in rowsToDelete)
                    {
                        table.Rows.Remove(rowToDelete);
                        CbAbas.SelectedItem = selectedRow;
                    }

                    if (selectedRow.Row["id_aba"] is int id)
                    {
                        string aba = selectedRow["aba"].ToString();
                        Guia guia = new Guia(id, aba);
                        SessaoAba.ArmazenarAba(guia);

                        IniciarAbas();
                    }
                    else if (selectedRow.Row["aba"].ToString() == "Adicionar Aba")
                    {
                        CadastroAba cadastroAba = new CadastroAba();
                        cadastroAba.Show();
                    } 
                }
            }
        }

        public void CbAbasView()
        {
            string connectionString = "Data Source=.;initial catalog=Checklists;integrated security=true;";

            Cliente cliente = SessaoId.IdAtual;

            if (cliente != null)
            {
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

                        DataRow abaRow = table.NewRow();
                        abaRow["aba"] = "Selecionar Aba";
                        table.Rows.InsertAt(abaRow, 0);

                        DataRow adicionarAbaRow = table.NewRow();
                        adicionarAbaRow["aba"] = "Adicionar Aba";
                        table.Rows.InsertAt(adicionarAbaRow, 1);

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
            else
            {
                CbAbas.Items.Add("Selecionar Aba");

                CbAbas.SelectedIndex = 0;
            }
        }

        private void IniciarAbas()
        {
            AbasADM abasADM = new AbasADM();
            abasADM.TopLevel = false;

            if (panel1.Controls.Count > 0)
                panel1.Controls.Clear();
            panel1.Controls.Add(abasADM);
            abasADM.BringToFront();
            abasADM.Show();
        }

        private void FecharAbas()
        {
            AbasADM abasADM = new AbasADM();
            abasADM.TopLevel = false;

            if (panel1.Controls.Count > 0)
                panel1.Controls.Clear();
            panel1.Controls.Add(abasADM);
            abasADM.SendToBack();
            abasADM.Close();
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }
    }
}
