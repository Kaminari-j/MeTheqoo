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

namespace MeTheqoo
{
	public class DownloadFile
	{
		protected MeTheqoo.SERVICE SERVICE_NAME;
		protected String ImgFindKwd { get; set; }
		private String _Content { get; set; }
		protected List<string> _FileList = new List<string>();

		public DownloadFile() { }

		protected void GetContentsFromSrc(string url)
		{
			var webRequest = WebRequest.Create(url);

			using (var response = webRequest.GetResponse())
			using (var content = response.GetResponseStream())
			using (var reader = new StreamReader(content))
			{
				var strContent = reader.ReadToEnd();
				this._Content = strContent.ToString();
			}

			if (!String.IsNullOrEmpty(this._Content))
			{
				MatchCollection tmp = System.Text.RegularExpressions.Regex.Matches(this._Content, this.ImgFindKwd);
				foreach (Match file in tmp)
				{
					this._FileList.Add(file.ToString());
				}
			}
		}

		protected bool DoDownloadFile()
		{
			// Download from web

			foreach (string imgUrl in _FileList)
			{
				string targetUrl = this.GetOriginalImageName(imgUrl);

				using (WebClient webClient = new WebClient())
				{
					//have to add filename phrase
					string fileFullName = MakeUniqueFileName(System.Environment.CurrentDirectory
															+ @"\" + DateTime.Now.ToString("yyyyMMdd") 
															+ "_.jpg"
															).FullName;

					webClient.DownloadFile(targetUrl, fileFullName);

					// read image from file, and delete tmp file?
				}
			}
			return true;
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
	}

	public class DownloadTwitter : DownloadFile
	{
		public DownloadTwitter(String url) : base()
		{
			this.SERVICE_NAME = MeTheqoo.SERVICE.twitter;
			this.ImgFindKwd = @"data-image-url=.*";

			DoDownloadFile();
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
