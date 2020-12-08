namespace Test.Application
{
	partial class TestForm
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
			this.btnTCPClient = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnTCPClient
			// 
			this.btnTCPClient.Location = new System.Drawing.Point(12, 12);
			this.btnTCPClient.Name = "btnTCPClient";
			this.btnTCPClient.Size = new System.Drawing.Size(130, 23);
			this.btnTCPClient.TabIndex = 0;
			this.btnTCPClient.Text = "Start test form ...";
			this.btnTCPClient.UseVisualStyleBackColor = true;
			this.btnTCPClient.Click += new System.EventHandler(this.btnTCPClient_Click);
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(154, 45);
			this.Controls.Add(this.btnTCPClient);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "TestForm";
			this.Text = "RUDP Test";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnTCPClient;
	}
}