using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HellBlaster.Domain
{
	public class VS10SolutionReader
	{

		private static string ProjectTag	{get{return "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\")";}}

		public List<VS10Project> FindProjects(string solutionfilecontent)
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
				projects.Add(CreateProject(LineWithProject));				

			return projects;
		}

		private static VS10Project CreateProject(string LineWithProject)
		{
			VS10Project project = new VS10Project();
			project.Name = ProjectName(LineWithProject);
			project.RelativePath = RelativePath(LineWithProject);
			return project;
		}

		private static string RelativePath(string LineWithProject)
		{			
			return LineWithProject.Split(',')[1].Replace('\"', ' ').Trim();				
		}

		private static string ProjectName(string LineWithProject)
		{
			return  LineWithProject.Substring(BeginningOfProjectName(LineWithProject), EndOfProjectName(LineWithProject) - BeginningOfProjectName(LineWithProject));
		}

		private static int EndOfProjectName(string LineWithProject)
		{
			return LineWithProject.IndexOf('\"', BeginningOfProjectName(LineWithProject));
		}

		private static int BeginningOfProjectName(string LineWithProject)
		{
			return LineWithProject.IndexOf("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"") + "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"".Length;
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
