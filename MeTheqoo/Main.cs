using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeTheqoo
{
	public enum SERVICE { twitter };

	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DownloadTwitter dt = new DownloadTwitter(textBox1.Text);
		}
	}
}
