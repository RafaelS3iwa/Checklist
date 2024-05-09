using Checklist.Classes;
using Checklist.Panels;
using Checklist.Panels.Abas.ADM;
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
                    clienteRow["nome"] = "CLIENTE";
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

        private void panelPreflight_Click(object sender, EventArgs e)
        {
            IniciarPreflight();
        }

        private void panelProof_Click(object sender, EventArgs e)
        {
            IniciarProof();
        }

        private void panelLiberacao_Click(object sender, EventArgs e)
        {
            IniciarLiberacao();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is DataRowView selectedRow)
            {
                if (selectedRow.Row["nome"].ToString() == "CLIENTE")
                {
                    SessaoId.LimparId();
                }
                else if (selectedRow.Row["nome"].ToString() == "Adicionar Cliente")
                {
                    CadastroCliente cadastroCliente = new CadastroCliente();
                    cadastroCliente.Show();
                }
                else if (selectedRow.Row["id_cliente"] is int id)
                {
                    string nome = selectedRow["nome"].ToString();

                    Cliente cliente = new Cliente(id, nome);
                    SessaoId.ArmazenarId(cliente);

                    if (panelPreflight.BorderStyle == BorderStyle.Fixed3D)
                    {
                        IniciarPreflight();
                    }
                    else if(panelProof.BorderStyle == BorderStyle.Fixed3D)
                    {
                        IniciarProof();
                    }
                    else if(panelLiberacao.BorderStyle == BorderStyle.Fixed3D)
                    {
                        IniciarLiberacao();
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
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void IniciarPreflight()
        {
            panelPreflight.BorderStyle = BorderStyle.Fixed3D;
            panelProof.BorderStyle = BorderStyle.None;
            panelLiberacao.BorderStyle = BorderStyle.None;

            PreflightADM preflight = new PreflightADM();
            preflight.TopLevel = false;
            if (panel1.Controls.Count > 0)
                panel1.Controls.Clear();
            panel1.Controls.Add(preflight);
            preflight.BringToFront();
            preflight.Show();
        }

        private void IniciarProof()
        {
            panelPreflight.BorderStyle = BorderStyle.None;
            panelProof.BorderStyle = BorderStyle.Fixed3D;
            panelLiberacao.BorderStyle = BorderStyle.None;

            ProofADM proof = new ProofADM();
            proof.TopLevel = false;
            if (panel1.Controls.Count > 0)
                panel1.Controls.Clear();
            panel1.Controls.Add(proof);
            proof.BringToFront();
            proof.Show();
        }

        private void IniciarLiberacao()
        {
            panelPreflight.BorderStyle = BorderStyle.None;
            panelProof.BorderStyle = BorderStyle.None;
            panelLiberacao.BorderStyle = BorderStyle.Fixed3D;

            LiberacaoADM liberacao = new LiberacaoADM();
            liberacao.TopLevel = false;
            if (panel1.Controls.Count > 0)
                panel1.Controls.Clear();
            panel1.Controls.Add(liberacao);
            liberacao.BringToFront();
            liberacao.Show();
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

        private void FormADM_Activated(object sender, EventArgs e)
        {
            checkBoxView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
}
