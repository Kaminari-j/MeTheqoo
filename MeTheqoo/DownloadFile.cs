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
using WebApiContrib.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MeTheqoo
{
	public class DownloadFile
	{
		protected MeTheqoo.SERVICE SERVICE_NAME = SERVICE.NONE;
		protected String _grepKeyword { get; set; }
		private String _Content { get; set; }
		private String _url { get; set; }

		public DownloadFile(string url, SERVICE SVC)
		{
			this._url = url;
			if (url.Contains("taken-by"))
			{
				string[] tmpUrl = url.Split('?');
				this._url = tmpUrl[0] + "?__a=1";
			}

			this.SERVICE_NAME = SVC;

			switch (this.SERVICE_NAME)
			{
				case SERVICE.twitter:
					this._grepKeyword = @"data-image-url=.*";
					break;
				case SERVICE.instagram:
					//this._grepKeyword = @"display_resources.*]";
					this._grepKeyword = null; // JSONで取得するため
					break;
				case SERVICE.NONE:
				default:
					break;
			}

			if (this.GetContentsFromSrc(this._url) == true)
			{
				List<string> fileList = GrepFilesFromContents(this._Content, this._grepKeyword);
				if (fileList.Count >= 1)
				{
					DoDownloadFile(fileList);
				}
			}
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

		protected List<string> GrepFilesFromContents(string strWebContent, string strGrepKwd)
		{
			if (String.IsNullOrEmpty(this._Content))
			{
				return null;
			}
			else
			{
				try
				{
					List<string> tmpFileList = new List<string>();

					if (this.SERVICE_NAME == SERVICE.twitter)
					{
						MatchCollection tmp = System.Text.RegularExpressions.Regex.Matches(strWebContent, strGrepKwd);
						foreach (Match file in tmp)
						{
							tmpFileList.Add(file.ToString());
						}
					}
					else if (this.SERVICE_NAME == SERVICE.instagram)
					{
						JObject o = JObject.Parse(strWebContent);
						foreach (JObject file in o["graphql"]["shortcode_media"]["edge_sidecar_to_children"]["edges"])
						{
							tmpFileList.Add(file["node"]["display_url"].ToString());
						}
					}

					if (tmpFileList.Count == 0)
						return null;
					else
						return tmpFileList;
				}
				catch (Exception ex)
				{
					ShowExceptionMsgBox(ex);
					return null;
				}
			}
		}

		protected bool DoDownloadFile(List<string> lstFiles)
		{
			// Download from web
			try
			{
				foreach (string imgUrl in lstFiles)
				{
					string targetUrl = this.GetOriginalImageName(imgUrl);
					string fullName = MakeUniqueFileName(System.Environment.CurrentDirectory
															+ @"\" + DateTime.Now.ToString("yyyyMMdd")
															+ "_" + this.SERVICE_NAME
															+ "_.jpg"
															).FullName;

					using (WebClient webClient = new WebClient())
					{
						webClient.DownloadFile(targetUrl, fullName);

						// read image from file, and delete tmp file?
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				ShowExceptionMsgBox(ex);
				return false;
			}
		}

		public FileInfo MakeUniqueFileName(string path)
		{
			string dir = Path.GetDirectoryName(path);
			string fileName = Path.GetFileNameWithoutExtension(path);
			string fileExt = Path.GetExtension(path);

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

			try { } catch (Exception) { }

			return string.Empty;
		}

		protected virtual string GetOriginalMovieName(string imgUrl)
		{
			// orig をつけるかほかの方法

			try { } catch (Exception) { }

			return string.Empty;
		}

		private void ShowExceptionMsgBox(Exception ex)
		{
			MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
		}
	}

	public class DownloadInstagram : DownloadFile
	{
		public DownloadInstagram(String url) : base(url, SERVICE.instagram) { }

		protected override string GetOriginalImageName(string imgUrl)
		{
			return imgUrl;
		}

		protected override string GetOriginalMovieName(string imgUrl)
		{
			// orig をつけるかほかの方法

			return imgUrl;
		}
	}

	public class DownloadTwitter : DownloadFile
	{
		public DownloadTwitter(String url) : base(url, SERVICE.twitter) { }

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
