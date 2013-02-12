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
		public List<FileReference> ReferenceList { get; set; }


		public MainPageCtrl()
		{
			ReferenceList = new List<FileReference>();
		}


		public void LoadSolutionFile(string solutionPath)
		{
			projects=FindProjects(solutionPath);
			foreach (VS10Project project in projects)
				ShowProjectInSolution(project);
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
			ShowReferences(FindFileReferencesInProject(project));
		}

		private void ShowReferences(List<FileReference> referencesInProject)
		{
			ReferenceList.AddRange(referencesInProject);
			foreach (FileReference reference in referencesInProject)
				View.AddFileRefence(reference.Name, reference.Version);
		}

		private static List<FileReference> FindFileReferencesInProject(VS10Project project)
		{
			VS10ProjectReader projReader = new VS10ProjectReader();
			projReader.Read(project.FullPath);
			return projReader.FindFileReferences();			
		}



		public void UpdateFileReference(string referenceName, string referenceVersion)
		{
			foreach (VS10Project project in projects)
			{
				FileReference RefFound = FindReferenceInProject(project, referenceName);
				if (RefFound!=null)
				{
					RefFound.UpdateTo(referenceVersion);
					UpdateReferenceInProject(project, RefFound);
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

		private FileReference FindReferenceInProject( VS10Project project,string referenceName)
		{			
			foreach (FileReference fileRef in FindFileReferencesInProject(project))
				if (fileRef.Name == referenceName)
					return fileRef;
			return null;
		}

		

		public void WritingReference(string namePart)
		{
			if (!String.IsNullOrEmpty(namePart))
			{				
				string foundReference = FindReferenceByNamePart(namePart);
				if (!String.IsNullOrEmpty(foundReference))
					View.SuggestFileRefence(foundReference);
			}
		}

		private string FindReferenceByNamePart(string namePart)
		{
			string foundReference = string.Empty;
			foreach (FileReference refer in ReferenceList)
				if (refer.Name.IndexOf(namePart) == 0)
				{
					if (FirstReferenceOrTheSameOne(foundReference, refer))
						foundReference = refer.Name;
					else
						return null;					
				}
			
			return foundReference;
			
		}

		private static bool FirstReferenceOrTheSameOne(string foundReference, FileReference refer)
		{
			return String.IsNullOrEmpty(foundReference) || refer.Name == foundReference;
		}

		
	}
}
