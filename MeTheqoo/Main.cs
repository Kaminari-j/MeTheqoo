using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

	delegate void DoAddListBoxValueCallback(string TextValue);
	delegate void DoPerformProgressBarStepCallback();
	delegate void DoResetProgressBarCallback();

	public interface IControlInterface
	{
		#region -- ProgressBar
		void DoSetProgressBarMaxValue(int MaxValue);
		void DoPerformProgressBarStep();
		void DoResetProgressBar();
		#endregion

		#region -- ListBox
		void DoSetListBoxData(string[] Data);
		void DoAddListBoxValue(string TextValue);
		#endregion
	}

	public partial class Main : Form, IControlInterface
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
					DownloadTwitter dt = new DownloadTwitter(url, this as IControlInterface);
					dt.StartDownload();
					break;
				case SERVICE.instagram:
					DownloadInstagram di = new DownloadInstagram(url, this as IControlInterface);
					di.StartDownload();
					break;
				case SERVICE.NONE:
				default:
					MessageBox.Show("해당 서비스는 지원되지 않습니다.");
					break;
			}
		}

		#region IContrlInterface
		public void DoAddListBoxValue(string TextValue)
		{
			if (this.listBox_Download.InvokeRequired)
			{
				DoAddListBoxValueCallback d = new DoAddListBoxValueCallback(DoAddListBoxValue);
				this.Invoke(d, new object[] { TextValue });
			}
			else
			{
				this.listBox_Download.Items.Add(TextValue);
			}
		}

		public void DoPerformProgressBarStep()
		{
			if (this.statusStrip1.InvokeRequired)
			{
				DoPerformProgressBarStepCallback d = new DoPerformProgressBarStepCallback(DoPerformProgressBarStep);
				this.Invoke(d, new object[] { });
			}
			else
			{
				this.toolStripProgressBar1.PerformStep();

				if (this.toolStripProgressBar1.Value == this.toolStripProgressBar1.Maximum)
				{
					MessageBox.Show("다운로드 완료!");
				}
			}
		}

		public void DoSetProgressBarMaxValue(int MaxValue)
		{
			this.toolStripProgressBar1.Maximum = MaxValue;
			this.toolStripProgressBar1.Step = 1;
		}

		public void DoSetListBoxData(string[] Data)
		{
			this.listBox_Download.DataSource = Data;
		}

		public void DoResetProgressBar()
		{
			if (this.statusStrip1.InvokeRequired)
			{
				DoResetProgressBarCallback d = new DoResetProgressBarCallback(DoResetProgressBar);
				this.Invoke(d, new object[] { });
			}
			else
			{
				this.toolStripProgressBar1.Value = 0;
			}
		}
		#endregion

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

		private void listBox_Download_DoubleClick(object sender, EventArgs e)
		{
			if (((ListBox)sender).SelectedItem != null)
			{
				Process.Start(((ListBox)sender).SelectedItem.ToString());
			}
		}
		#endregion

		private void listBox_Download_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ListBox)sender).SelectedItem != null)
			{
				this.pbPictureSelected.Image = Image.FromFile(((ListBox)sender).SelectedItem.ToString());
				
			}
		}
	}
}
