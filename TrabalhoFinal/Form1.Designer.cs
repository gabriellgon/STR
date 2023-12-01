namespace TrabalhoFinal
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			groupBox1 = new GroupBox();
			Parar = new Button();
			textboxCurto = new TextBox();
			label2 = new Label();
			label1 = new Label();
			Iniciar = new Button();
			textBoxRecepcaoConexao = new TextBox();
			groupBox2 = new GroupBox();
			textBoxAvisos = new TextBox();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(Parar);
			groupBox1.Controls.Add(textboxCurto);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(label1);
			groupBox1.Controls.Add(Iniciar);
			groupBox1.Controls.Add(textBoxRecepcaoConexao);
			groupBox1.Location = new Point(12, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(356, 612);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			// 
			// Parar
			// 
			Parar.Location = new Point(248, 22);
			Parar.Name = "Parar";
			Parar.Size = new Size(99, 29);
			Parar.TabIndex = 32;
			Parar.Text = "Parar";
			Parar.UseVisualStyleBackColor = true;
			Parar.Click += Parar_Click;
			// 
			// textboxCurto
			// 
			textboxCurto.BackColor = SystemColors.ControlDark;
			textboxCurto.Cursor = Cursors.IBeam;
			textboxCurto.Location = new Point(6, 368);
			textboxCurto.Multiline = true;
			textboxCurto.Name = "textboxCurto";
			textboxCurto.ScrollBars = ScrollBars.Vertical;
			textboxCurto.Size = new Size(341, 237);
			textboxCurto.TabIndex = 30;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(6, 350);
			label2.Name = "label2";
			label2.Size = new Size(84, 15);
			label2.TabIndex = 31;
			label2.Text = "Esta em curto?";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(6, 67);
			label1.Name = "label1";
			label1.Size = new Size(102, 15);
			label1.TabIndex = 1;
			label1.Text = "Pacotes recebidos";
			// 
			// Iniciar
			// 
			Iniciar.Location = new Point(9, 22);
			Iniciar.Name = "Iniciar";
			Iniciar.Size = new Size(99, 29);
			Iniciar.TabIndex = 1;
			Iniciar.Text = "Iniciar";
			Iniciar.UseVisualStyleBackColor = true;
			Iniciar.Click += Iniciar_Click;
			// 
			// textBoxRecepcaoConexao
			// 
			textBoxRecepcaoConexao.BackColor = SystemColors.ControlDark;
			textBoxRecepcaoConexao.Cursor = Cursors.IBeam;
			textBoxRecepcaoConexao.Location = new Point(6, 85);
			textBoxRecepcaoConexao.Multiline = true;
			textBoxRecepcaoConexao.Name = "textBoxRecepcaoConexao";
			textBoxRecepcaoConexao.ScrollBars = ScrollBars.Vertical;
			textBoxRecepcaoConexao.Size = new Size(341, 239);
			textBoxRecepcaoConexao.TabIndex = 29;
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(textBoxAvisos);
			groupBox2.Location = new Point(374, 12);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new Size(356, 612);
			groupBox2.TabIndex = 1;
			groupBox2.TabStop = false;
			groupBox2.Text = "Avisos";
			// 
			// textBoxAvisos
			// 
			textBoxAvisos.BackColor = SystemColors.ControlDark;
			textBoxAvisos.Cursor = Cursors.IBeam;
			textBoxAvisos.Location = new Point(6, 22);
			textBoxAvisos.Multiline = true;
			textBoxAvisos.Name = "textBoxAvisos";
			textBoxAvisos.ScrollBars = ScrollBars.Vertical;
			textBoxAvisos.Size = new Size(341, 583);
			textBoxAvisos.TabIndex = 30;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(746, 629);
			Controls.Add(groupBox2);
			Controls.Add(groupBox1);
			Name = "Form1";
			Text = "Form1";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private GroupBox groupBox1;
		private TextBox textBoxRecepcaoConexao;
		private Label label2;
		private TextBox textboxCurto;
		private Label label1;
		private Button Iniciar;
		private Button Parar;
		private GroupBox groupBox2;
		private TextBox textBoxAvisos;
	}
}