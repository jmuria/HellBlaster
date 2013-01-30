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
		List<VS10Project> projects;
	

		public void LoadSolutionFile(string solutionPath)
		{
			projects=FindProjects(solutionPath);
			foreach (VS10Project project in projects)
			{
				ShowProjectInSolution(project);
			}
		}

		private  List<VS10Project> FindProjects(string solutionPath)
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



		public void UpdateFileReference(string referenceName, string referenceVersion)
		{
			foreach (VS10Project project in projects)
			{
				FileReference found = FindReferenceInProject(project, referenceName);
				if (found!=null)
				{
					found.UpdateTo(referenceVersion);
					UpdateReferenceInProject(project, found);
					View.UpdateFileRefence(project.Name, referenceName, referenceVersion);
				}
			}
			
		}

		private static void UpdateReferenceInProject( VS10Project project, FileReference found)
		{
			VS10ProjectWriter projWriter = new VS10ProjectWriter();
			projWriter.Read(project.FullPath);			
			projWriter.UpdateReference(found);
			projWriter.WriteToFile();
		}

		private static FileReference FindReferenceInProject( VS10Project project,string referenceName)
		{
			foreach (FileReference fileRef in FindFileReferences(project))
				if (fileRef.Name == referenceName)
					return fileRef;
			return null;
		}
	}
}
