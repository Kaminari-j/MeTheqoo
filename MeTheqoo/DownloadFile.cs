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
<<<<<<< HEAD
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
=======
>>>>>>> parent of 166bcbb... json

namespace MeTheqoo
{
	public class DownloadFile
	{
		protected MeTheqoo.SERVICE SERVICE_NAME;
		protected String ImgFindKwd { get; set; }
		private String _Content { get; set; }
		protected List<string> _FileList = new List<string>();

		public DownloadFile(string url) { }

		protected bool GetContentsFromSrc(string url)
		{
			try
			{
				var webRequest = WebRequest.Create(url);

				using (var response = webRequest.GetResponse())
				using (var content = response.GetResponseStream())
				using (var reader = new StreamReader(content))
				{
<<<<<<< HEAD
					//string _Content = reader.ReadToEnd();
					JToken jt = JObject.Parse(reader.ReadToEnd());
=======
					var strContent = reader.ReadToEnd();
					this._Content = strContent.ToString();
>>>>>>> parent of 166bcbb... json
				}

				if (!String.IsNullOrEmpty(this._Content))
				{
					MatchCollection tmp = System.Text.RegularExpressions.Regex.Matches(this._Content, this.ImgFindKwd);
					foreach (Match file in tmp)
					{
						this._FileList.Add(file.ToString());
					}

					if (this._FileList.Count == 0)
						return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace);
				return false;
			}
		}

		protected bool DoDownloadFile()
		{
			// Download from web

			foreach (string imgUrl in _FileList)
			{
				string targetUrl = this.GetOriginalImageName(imgUrl);
				string fullName = MakeUniqueFileName(System.Environment.CurrentDirectory
														+ @"\" + DateTime.Now.ToString("yyyyMMdd")
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
		public DownloadTwitter(String url) : base(url)
		{
			this.SERVICE_NAME = MeTheqoo.SERVICE.twitter;
			this.ImgFindKwd = @"data-image-url=.*";

			if (this.GetContentsFromSrc(url) == true)
			{
				DoDownloadFile();
			}
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
