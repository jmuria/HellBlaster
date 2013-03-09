using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HellBlaster.Interfaces
{
	public interface IMainPageView
	{
		void AddProject(string projectName);
		void AddFileRefence(string name, string version);
		void UpdateFileRefence(string projectName, string assemblyName, string assemblyVersion);
		void CleanReferences();
		void SuggestFileRefence(string assemblyName);
		void ShowDiscrepancy(string assemblyName,string version);
	}
}
