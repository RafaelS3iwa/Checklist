using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Checklist.Panels;
using Checklist.Classes;
using Checklist.Classes.Abas;
using Checklist.Panels.Abas.Cadastro_e_Edição;
using Checklist.Panels.Abas.ADM;
using Checklist.Panels.Abas.User;

namespace Checklist
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                    if (selectedRow.Row["id_cliente"] is int id)
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

        public void CbClientesView()
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

        private void Form1_Load(object sender, EventArgs e)
        {
            CbClientesView();
            CbAbasView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();
            formLogin.ShowDialog(); 

            if (formLogin.DialogResult == DialogResult.OK)
            {
                FormADM formADM = new FormADM();
                formADM.Show();

                this.Hide();
            }
            else
            {
                return;
            }
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void CbAbas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cliente cliente = SessaoId.IdAtual;
            if (CbAbas.SelectedItem is DataRowView selectedRow)
            {
                if (selectedRow.Row["aba"].ToString() == "Selecionar Aba")
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
            AbasUser abasUser = new AbasUser();
            abasUser.TopLevel = false;

            if (panel1.Controls.Count > 0)
                panel1.Controls.Clear();
            panel1.Controls.Add(abasUser);
            abasUser.BringToFront();
            abasUser.Show();
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
    }
}
