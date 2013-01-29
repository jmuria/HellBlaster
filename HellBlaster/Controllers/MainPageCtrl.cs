using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HellBlaster.Interfaces;
using HellBlaster.Domain;

namespace HellBlaster.Controllers
{
	public class MainPageCtrl
	{
		public IMainPageView View { protected get; set; }

		public void LoadSolutionFile(string solutionPath)
		{
			foreach (VS10Project project in FindProjects(solutionPath))
				ShowProjectInSolution(project);
		}

		private static List<VS10Project> FindProjects(string solutionPath)
		{
			VS10SolutionReader solReader = new VS10SolutionReader();
			solReader.Read(solutionPath);
			return solReader.FindProjects();			
		}

		private void ShowProjectInSolution(VS10Project project)
		{
			View.AddProject(project.Name);

			foreach (FileReference reference in FindFileReferences(project))
				View.AddFileRefence(reference.Name, reference.Version);
		}

		private static List<FileReference> FindFileReferences(VS10Project project)
		{
			VS10ProjectReader projReader = new VS10ProjectReader();
			projReader.Read(project.FullPath);
			return projReader.FindFileReferences();			
		}

		
	}
}
