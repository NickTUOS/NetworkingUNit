using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test.Application
{
	public partial class TestForm : Form
	{
		[STAThread]
		static public void Main(string[] args)
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			
			System.Windows.Forms.Application.Run(new TestForm());
		}

		public TestForm()
		{
			InitializeComponent();
		}

		private void btnTCPClient_Click(object sender, EventArgs e)
		{
			TestingSocketForm form = new TestingSocketForm();
			form.Show();
		}
	}
}