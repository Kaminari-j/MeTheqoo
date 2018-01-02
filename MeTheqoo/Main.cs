using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeTheqoo
{
	public enum SERVICE { NONE, twitter, instagram };
	public enum MEDIATYPE { NONE, image, video };

	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();

			this.statusLabel_ServiceName.Text = "";
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string url = textBox1.Text;
			switch (GetServiceName(url))
			{
				case SERVICE.twitter:
					DownloadTwitter dt = new DownloadTwitter(url);
					break;
				case SERVICE.instagram:
					DownloadInstagram di = new DownloadInstagram(url);
					break;
				default:
					break;
			}
		}

		private MeTheqoo.SERVICE GetServiceName(string url)
		{
			if (url.Contains("twitter.com"))
				return MeTheqoo.SERVICE.twitter;
			if (url.Contains("instagram.com"))
				return MeTheqoo.SERVICE.instagram;

			return MeTheqoo.SERVICE.NONE;
		}
	}
}
