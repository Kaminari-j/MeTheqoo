using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KSHTool
{
	public class DownloadFile
	{
		protected KSHTool.SERVICE SERVICE_NAME = SERVICE.NONE;
		protected KSHTool.MEDIATYPE MEDIA_TYPE = MEDIATYPE.NONE;
		protected String _grepKeyword { get; set; }
		private String _Content { get; set; }
		private String _url { get; set; }
		public List<string> _DownloadList { get; }
		ListBox lb;
		ToolStripProgressBar tsProgress;

		public DownloadFile(string url, SERVICE SVC, ListBox lstBox, ToolStripProgressBar prgbar)
		{
			this._url = SetTargetUrl(url);
			this.SERVICE_NAME = SVC;
			this.lb = lstBox;
			this.tsProgress = prgbar;

			try
			{
				if (this.GetContentsFromSrc(this._url) == true)
				{
					List<string> fileList = MakeFileList(this._Content);
					if (fileList.Count >= 1)
					{
						DoDownloadFile(fileList);
					}
				}
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
			}
		}

		protected virtual string SetTargetUrl(string url)
		{
			return url;
		}

		protected bool GetContentsFromSrc(string url)
		{
			try
			{
				var webRequest = WebRequest.Create(url);

				using (var response = webRequest.GetResponse())
				using (var content = response.GetResponseStream())
				using (var reader = new StreamReader(content))
				{
					this._Content = reader.ReadToEnd();
				}


				return true;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
				return false;
			}
		}

		protected List<string> MakeFileList(string strWebContent)
		{
			try
			{
				if (String.IsNullOrEmpty(this._Content))
					return null;

				List<string> _filelist = GetFileListFromContent(strWebContent);

				if (_filelist.Count == 0)
					return null;
				else
					return _filelist;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
				return null;
			}
		}

		protected virtual List<string> GetFileListFromContent(string content)
		{
			return null;
		}

		protected bool DoDownloadFile(List<string> lstFiles)
		{
			// Download from web
			try
			{
				tsProgress.Maximum = lstFiles.Count;
				foreach (string imgUrl in lstFiles)
				{
					string targetUrl = this.GetOriginalImageName(imgUrl);

					string fullName = "";
					fullName = MakeUniqueFileName(System.Environment.CurrentDirectory
													+ @"\" + DateTime.Now.ToString("yyyyMMdd")
													+ "_" + this.SERVICE_NAME
													+ "_"
													, imgUrl
															).FullName;

					using (WebClient webClient = new WebClient())
					{
						webClient.DownloadFile(targetUrl, fullName);

						// read image from file, and delete tmp file?
					}

					this.tsProgress.PerformStep();
					this.lb.Items.Add(fullName.ToString());
					//this._DownloadList.Add(fullName);
				}
				return true;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
				return false;
			}
		}

		public FileInfo MakeUniqueFileName(string path, string imgUrl)
		{
			string dir = Path.GetDirectoryName(path);
			string fileName = Path.GetFileNameWithoutExtension(path);
			string fileExt = Path.GetExtension(imgUrl);

			for (int i = 1; ; ++i)
			{
				path = Path.Combine(dir, fileName + i + fileExt);

				if (!File.Exists(path))
					return new FileInfo(path);
			}
		}

		protected virtual string GetOriginalImageName(string imgUrl)
		{
			// orig をつけるかほかの方法

			return imgUrl;
		}

		protected virtual string GetOriginalMovieName(string imgUrl)
		{
			// orig をつけるかほかの方法

			return imgUrl;
		}

		private void ShowExceptionMsgBox(Exception ex)
		{
			MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
		}
	}

	public class DownloadInstagram : DownloadFile
	{
		// https://www.instagram.com/p/BdcnRlSl4Yh
		public DownloadInstagram(String url, ListBox listbox, ToolStripProgressBar tbar) : base(url, SERVICE.instagram, listbox, tbar)
		{
		}

		protected override string SetTargetUrl(string url)
		{
			return System.Text.RegularExpressions.Regex.Matches(url, @"https\:\/\/www\.instagram\.com\/p\/[0-9a-zA-Z]+/")[0].ToString() + "?__a=1";
		}

		protected override List<string> GetFileListFromContent(string content)
		{
			List<string> tmpFileList = new List<string>();
			JObject jobj = JObject.Parse(content);

			string mediaType = jobj["graphql"]["shortcode_media"]["__typename"].ToString();

			if (mediaType == "GraphVideo")
			{
				this.MEDIA_TYPE = MEDIATYPE.video;
				tmpFileList.Add(jobj["graphql"]["shortcode_media"]["video_url"].ToString());
			}
			else if (mediaType == "GraphImage")
			{
				this.MEDIA_TYPE = MEDIATYPE.image;
				tmpFileList.Add(jobj["graphql"]["shortcode_media"]["display_url"].ToString());
			}
			else if (mediaType == "GraphSidecar")
			{
				foreach (JObject file in jobj["graphql"]["shortcode_media"]["edge_sidecar_to_children"]["edges"])
				{
					mediaType = file["node"]["__typename"].ToString();
					if (mediaType == "GraphVideo")
					{
						tmpFileList.Add(file["node"]["video_url"].ToString());
					}
					else if (mediaType == "GraphImage")
					{
						tmpFileList.Add(file["node"]["display_url"].ToString());
					}
				}
			}

			return tmpFileList;
		}
	}

	public class DownloadTwitter : DownloadFile
	{
		public DownloadTwitter(String url, ListBox listbox, ToolStripProgressBar tbar) : base(url, SERVICE.twitter, listbox, tbar)
		{
			this._grepKeyword = @"data-image-url=.*";
		}

		protected override List<string> GetFileListFromContent(string content)
		{
			this.MEDIA_TYPE = MEDIATYPE.image; // Twitterの動画ダウンロードは今後追加するため
			MatchCollection tmp = System.Text.RegularExpressions.Regex.Matches(content, this._grepKeyword);
			List<string> tmpFileList = new List<string>();
			JObject jobj = JObject.Parse(content);
			foreach (Match file in tmp)
			{
				tmpFileList.Add(file.ToString());
			}

			return tmpFileList;
		}

		protected override string GetOriginalImageName(string imgUrl)
		{
			// 後ろに :orig をつける
			// 引数 imgUrl の文字列の例 "data-image-url="https://pbs.twimg.com/media/DDp82xwUMAEDOpz.jpg""

			try
			{
				string[] spltRst = imgUrl.Split('\"');
				if (spltRst.Length == 3)
				{
					string url = spltRst[1];
					url = url + ":orig";

					return url;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace);

				return string.Empty;
			}

			return string.Empty;
		}

		protected override string GetOriginalMovieName(string imgUrl)
		{
			// orig をつけるかほかの方法

			return string.Empty;
		}
	}
}
