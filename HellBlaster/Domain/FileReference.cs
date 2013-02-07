using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HellBlaster.Domain
{
	public class FileReference
	{

		public string Name { get; protected set; }
		public string Version { get; protected set; }
		public string Path { get; protected set; }


		public FileReference()
		{
			this.Version = "1.0.0";
		}

		public FileReference(string version)
		{
			this.Version = version;
		}

		public FileReference(string version, string path)
		{
			this.Version = version;
			this.Path = path;
		}

		public FileReference(string name, string version, string path)
		{
			this.Name = name;
			this.Version = version;
			this.Path = path;
		}

		public void UpdateTo(string newVersion)
		{
			if (!String.IsNullOrEmpty(Path) && !String.IsNullOrEmpty(VersionInPath()))
				Path = Path.Replace(VersionInPath(), newVersion);
			
			this.Version = newVersion;
		}

		private string VersionInPath()
		{
			if (Version == null) return null;
			for (int digits = 4; digits > 0; digits--)
			{
				string subVersion = Version.Substring(0, DigitPosition(digits));
				if (Path.IndexOf(subVersion) > 0)
					return subVersion;
			}
			return null;			
		}

		private int DigitPosition(int digits)
		{
			int pos = 0;

			for (int i = 0; i < digits && pos < Version.Length; i++)
				pos = FindNextDigitPosition(pos);

			return pos;
		}

		private int VersionDigits()
		{
			int pos = 0;
			int nDigits = 0;
			while (pos != Version.Length)
			{
				pos = FindNextDigitPosition(pos);
				nDigits++;
			}		
			return nDigits;
		}

		private int FindNextDigitPosition(int pos)
		{
			pos = Version.IndexOf('.', pos + 1);
			if (pos == -1)
				pos = Version.Length;
			return pos;
		}
	}
}
			
			

