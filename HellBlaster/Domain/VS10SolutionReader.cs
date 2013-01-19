using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HellBlaster.Domain
{
	public class VS10SolutionReader
	{

		private static string ProjectTag	{get{return "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\")";}}
		private string FileContent;
		private static string SolutionPath;
		public string LastError { get; protected set; }

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
			if (!String.IsNullOrEmpty(SolutionPath))
			{
				string solutionFolder = SolutionPath.Substring(0, SolutionPath.LastIndexOf('\\'));
				project.FullPath=Path.Combine(solutionFolder,project.RelativePath);
			}
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



		public void Read(string solutionPath)
		{
			try
			{
				FileContent = new FileInfo(solutionPath).OpenText().ReadToEnd();
				SolutionPath = solutionPath;
			}
			catch (Exception e)
			{
				LastError = e.Message;
			}
		}

		public List<VS10Project> FindProjects()
		{
			return FindProjects(FileContent);
		}
	}
}
