using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HellBlaster.Interfaces
{
	public interface IVersionRetriever
	{
		string  FileVersion(string path);
	}
}
