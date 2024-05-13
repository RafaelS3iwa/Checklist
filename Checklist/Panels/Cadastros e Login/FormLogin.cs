using Checklist.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checklist
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtUser.Text) || string.IsNullOrWhiteSpace(TxtSenha.Text))
            {
                MessageBox.Show("Por favor, preencha os campos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string user = "Admin";
                string senha = "12345"; 

                if(TxtUser.Text == user && TxtSenha.Text == senha)
                {
                    DialogResult = DialogResult.OK;
                    this.Hide();
                }
                else
                {
                    TxtUser.Text = null; 
                    TxtSenha.Text = null;
                    MessageBox.Show("Usuário ou senha incorretos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
