using Checklist.Classes;
using Checklist.Classes.Item;
using Checklist.Panels.Checklists;
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

namespace Checklist.Panels.Abas.ADM
{
    public partial class ProofADM : Form
    {
        public ProofADM()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        int DistanceUnity = 1;
        private void ProofADM_Load(object sender, EventArgs e)
        {
            Cliente cliente = SessaoId.IdAtual;

            if(cliente != null)
            {
                SqlConnection conn = new SqlConnection("Data Source=.;initial catalog=Checklists;integrated security=true;");
                SqlCommand cmd = new SqlCommand("SELECT Checklist.*, Itens.* FROM Checklist INNER JOIN Itens ON Checklist.id_item = Itens.id_item WHERE Checklist.id_cliente = @id_cliente AND Itens.tipo LIKE '%Proof%'", conn);

                cmd.Parameters.AddWithValue("@id_cliente", cliente.getId());

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0) // Se não houver linhas retornadas pela consulta
                {
                    Label lblEmpty = new Label();
                    this.Controls.Add(lblEmpty);
                    lblEmpty.Text = "Ainda vazio";
                    lblEmpty.AutoSize = true;
                    lblEmpty.Font = new Font("Arial", 15.25F, FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));
                    lblEmpty.ForeColor = Color.Blue;
                    lblEmpty.Location = new Point(135, 100); // Posição do Label
                    return;
                }

                int initialTop = 50; // Posição inicial no eixo Y
                int labelHeight = 30; // Altura dos rótulos
                int verticalSpacing = 10; // Espaçamento vertical entre os rótulos
                int labelWidth = 300; // Largura do rótulo
                int buttonWidth = 75; // Largura do botão

                foreach (DataRow row in dt.Rows)
                {
                    int itemId = Convert.ToInt32(row["id_item"]);
                    int situacao = Convert.ToInt32(row["condicao"]);

                    CheckBox checkBox = new CheckBox();
                    this.Controls.Add(checkBox);
                    checkBox.Top = initialTop + (DistanceUnity - 1) * (labelHeight + verticalSpacing);
                    checkBox.Tag = itemId;
                    checkBox.Left = 25; // Ajuste a posição conforme necessário
                    checkBox.Tag = itemId;
                    checkBox.Checked = (situacao == 1);
                    checkBox.CheckedChanged += CheckBox_CheckedChanged;

                    // Criação do Label
                    Label lbl = new Label();
                    this.Controls.Add(lbl);
                    int currentTop = initialTop + (DistanceUnity - 1) * (labelHeight + verticalSpacing);
                    lbl.Top = currentTop;
                    lbl.Left = 100; // Posição fixa no eixo X
                    lbl.Width = labelWidth;
                    lbl.BringToFront();
                    DistanceUnity++;

                    // Botão Detalhe
                    Button button1 = new Button();
                    this.Controls.Add(button1);
                    button1.Top = currentTop; // Posiciona o botão na mesma posição Y do rótulo
                    button1.Left = lbl.Right - 158; // Posicionamento ao lado direito do Label, com um pequeno espaço
                    button1.Width = buttonWidth; // Largura do botão
                    button1.Text = "Detalhes"; // Texto do botão
                    button1.Font = new Font("Arial", 10.25F, FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));
                    button1.Tag = itemId;
                    button1.Click += BtnDetalhes_Click; // Evento de clique do botão
                    button1.BringToFront();

                    // Botão Excluir
                    Button button2 = new Button();
                    this.Controls.Add(button2);
                    button2.Top = currentTop; // Posiciona o botão na mesma posição Y do rótulo
                    button2.Left = button1.Right + 10; // Posicionamento ao lado direito do primeiro Button, com um espaço
                    button2.Width = buttonWidth; // Largura do botão
                    button2.Text = "Excluir"; // Texto do botão
                    button2.Font = new Font("Arial", 10.25F, FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));
                    button2.Tag = itemId;
                    button2.Click += BtnExcluir_Click; // Evento de clique do botão
                    button2.BringToFront();

                    // Estilizando o Label
                    lbl.ForeColor = Color.Black;
                    lbl.Font = new Font("Arial", 12.25F, FontStyle.Underline | FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));
                    lbl.AutoSize = false;
                    lbl.Text = row["nome_item"].ToString();

                    if (lbl.Text.Length > 10)
                    {
                        lbl.Text = lbl.Text.Substring(0, 10) + "...";
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um cliente para visualizar os Checklist.", "Aviso", MessageBoxButtons.OK);
                return;
            }
          
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            int itemId = Convert.ToInt32(check.Tag);

            using (SqlConnection conn = new SqlConnection("Data Source=.;initial catalog=Checklists;integrated security=true;"))
            {
                conn.Open();

                string sql = "UPDATE Checklist INNER JOIN Itens ON Checklist.id_item = Itens.id_item SET condicao=@condicao WHERE id_item=@id_item"; 

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_item", itemId);

                    if (check.Checked)
                    {
                        cmd.Parameters.AddWithValue("@condicao", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@condicao", 0); 
                    }   

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            int itemId = Convert.ToInt32(btn.Tag);

            using (SqlConnection conn = new SqlConnection("Data Source=.;initial catalog=Checklists;integrated security=true;"))
            {
                conn.Open();

                string sql = "DELETE FROM Itens WHERE id_item = @id_item";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_item", itemId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Item apagado com sucesso!");

                    FormADM formADM = new FormADM();
                    formADM.Hide();
                    formADM.Show();
                }
            }
        }

        private void BtnDetalhes_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int itemId = Convert.ToInt32(btn.Tag);

            using (SqlConnection conn = new SqlConnection("Data Source=.;initial catalog=Checklists;integrated security=true;"))
            {
                conn.Open();

                string sql = "SELECT Checklist.*, Itens.* FROM Checklist INNER JOIN Itens ON Checklist.id_item = Itens.id_item WHERE Itens.id_item = @id_item";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_item", itemId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nomeItem = reader["nome_item"].ToString();
                            string tipo = reader["tipo"].ToString();
                            string descricao = reader["descricao"].ToString();

                            DateTime criacao;
                            DateTime.TryParse(reader["created_time"].ToString(), out criacao);

                            DateTime atualizacao;
                            DateTime.TryParse(reader["update_time"].ToString(), out atualizacao);

                            int situacao = Convert.ToInt32(reader["condicao"].ToString());

                            Item item = new Item(itemId, nomeItem, tipo, descricao, criacao, atualizacao, situacao);
                            SessaoItem.ArmazenarItem(item);

                            DetalheChecklistADM detalheChecklistADM = new DetalheChecklistADM();
                            detalheChecklistADM.Show();
                        }
                    }
                }
            }
        }
    }
}