using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTheqoo
{
	public enum FILETYPE { jpg, png }

	public class DownFileInfo
	{
		// Download Info
		public string URL { get; set; }
		public string DownloadSrc { get; set; }
		// File Info
		public MeTheqoo.FILETYPE FileType { get; set; }
		public List<string> _FileList = new List<string>();

		string FileName { get; set; }
		string FileNameOrg { get; set; }
		Int64 FileSize { get; set; }
		DateTime AddDateTime { get; set; }
		DateTime SrcDateTime { get; set; }

		public DownFileInfo(string src)
		{
			this.OrigSrc = src;
		}

		public DownFileInfo(string src, string FileName, string FileNameOrg)
		{
			this.OrigSrc = src;
			this.FileName = FileName;
			this.FileNameOrg = FileNameOrg;
		}
	}
}
