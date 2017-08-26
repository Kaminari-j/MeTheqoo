using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeTheqoo
{
	public class DownFileInfo
	{
		// Download Info
		string OrigSrc { get; set; }
		// File Info
		string FileType { get; set; }
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
