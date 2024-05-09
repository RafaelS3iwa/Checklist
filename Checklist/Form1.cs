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
                if (selectedRow.Row["nome"].ToString() == "CLIENTE")
                {
                    SessaoId.LimparId();
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

        private void Form1_Load(object sender, EventArgs e)
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
                // O login foi cancelado ou falhou
                // Você pode tratar isso de acordo com sua lógica de negócios
            }
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {

        }

        private void IniciarPreflight()
        {
            panelPreflight.BorderStyle = BorderStyle.Fixed3D;
            panelProof.BorderStyle = BorderStyle.None;
            panelLiberacao.BorderStyle = BorderStyle.None;

            Preflight preflight = new Preflight();
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

            Proof proof = new Proof();
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

            Liberacao liberacao = new Liberacao();
            liberacao.TopLevel = false;
            if (panel1.Controls.Count > 0)
                panel1.Controls.Clear();
            panel1.Controls.Add(liberacao);
            liberacao.BringToFront();
            liberacao.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
}
