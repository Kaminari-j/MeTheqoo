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

namespace KSHTool
{
	public enum SERVICE { NONE, twitter, instagram };
	public enum MEDIATYPE { NONE, image, video };

	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();

			// initialize controls
			this.statusLabel_ServiceName.Text = "";
		}

		#region -- Methods --

		private KSHTool.SERVICE GetServiceName(string url)
		{
			if (url.Contains("twitter.com"))
				return KSHTool.SERVICE.twitter;
			if (url.Contains("instagram.com"))
				return KSHTool.SERVICE.instagram;
			else
				return KSHTool.SERVICE.NONE;
		}

		private void onButtonclick()
		{
			string url = tbUrl.Text;
			SERVICE svc = GetServiceName(url);
			this.statusLabel_ServiceName.Text = svc.ToString();
			switch (svc)
			{
				case SERVICE.twitter:
					DownloadTwitter dt = new DownloadTwitter(url);
					break;
				case SERVICE.instagram:
					DownloadInstagram di = new DownloadInstagram(url);
					break;
				case SERVICE.NONE:
				default:
					MessageBox.Show("해당 서비스는 아직 지원되지 않습니다.");
					break;
			}
		}

		#endregion

		#region -- HandleEvent --

		private void tbUrl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				this.onButtonclick();
			}
		}

		private void btnDownload_Click(object sender, EventArgs e)
		{
			this.onButtonclick();
		}

		#endregion
	}
}
