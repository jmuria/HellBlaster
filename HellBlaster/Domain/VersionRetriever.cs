using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HellBlaster.Interfaces;
using System.Reflection;

namespace HellBlaster.Domain
{
	public class VersionRetriever:IVersionRetriever
	{
		public string FileVersion(string path)
		{
			try
			{
				return Assembly.LoadFile(path).GetName().Version.ToString();
			}
			catch (Exception e)
			{
				return null;
			}			
		}
	}
}
