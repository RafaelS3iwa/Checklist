namespace Checklist.Panels
{
    partial class Preflight
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.checklistsDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.checklistsDataSetBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // checklistsDataSet
            // 
            // 
            // checklistsDataSetBindingSource
            // 
            this.checklistsDataSetBindingSource.Position = 0;
            // 
            // Preflight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(408, 405);
            this.Name = "Preflight";
            this.Text = "Preflight";
            this.Load += new System.EventHandler(this.Preflight_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checklistsDataSetBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource checklistsDataSetBindingSource;
    }
}