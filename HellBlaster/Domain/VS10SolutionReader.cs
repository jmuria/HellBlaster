using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HellBlaster.Domain
{
	public class VS10SolutionReader
	{

		private static string ProjectTag	{get{return "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\")";}}

		public List<VS10Project> FindReferences(string solutionfilecontent)
		{
			if (!String.IsNullOrEmpty(solutionfilecontent) && solutionfilecontent.Contains("Microsoft Visual Studio Solution File, Format Version 11.00"))
				return FindProjectsInSolutionFile(solutionfilecontent);
			else
				return null;
		}

		private static List<VS10Project> FindProjectsInSolutionFile(string solutionfilecontent)
		{
			List<VS10Project> projects = new List<VS10Project>();

			foreach (string LineWithProject in LinesWithProjectEntries(solutionfilecontent))
				projects.Add(new VS10Project());

			return projects;
		}

		private static IEnumerable<string> LinesWithProjectEntries(string solutionfilecontent)
		{
			string[] lines = solutionfilecontent.Split('\n');
			IEnumerable<string> linesWithProjects = from line in lines
													where line.IndexOf(ProjectTag) == 0
													select line;
			return linesWithProjects;
		}


	}
}
