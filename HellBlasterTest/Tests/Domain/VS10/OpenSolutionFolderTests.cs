using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HellBlaster.VS10;
using System.IO;
using HellBlaster.Domain;

namespace HellBlasterTest.Tests
{
	[TestFixture]
	public class OpenSolutionFolderTests
	{

		VS10SolutionReader solReader;


		private static List<FileReference> ReferencesInProject(VS10Project project)
		{
			List<FileReference> refs;
			VS10ProjectReader projReader;
			projReader = new VS10ProjectReader();
			projReader.Read(project.FullPath);
			refs = projReader.FindFileReferences();
			return refs;
		}

		private List<VS10Project> ProjectsInTestSolution()
		{
			solReader.Read(TestSolutionPath());
			List<VS10Project> projects = solReader.FindProjects();
			return projects;
		}

		private static string TestSolutionPath()
		{
			string path = @"..\..\testdata\solutionsample\Hellblaster\Hellblaster.sln";
			return path;
		}





		[SetUp]
		public void SetUp()
		{
			solReader = new VS10SolutionReader();
		}


		[Test]
		public void WhenIOpenASolutionFileICanReadTheProjects()
		{			
			Assert.AreEqual(2, ProjectsInTestSolution().Count);
		}


		[Test]
		public void WhenIOpenASolutionFileICanReadTheProjectsAndTheFileReferences()
		{
			List<VS10Project> projects=ProjectsInTestSolution();
			Assert.AreEqual(2, ProjectsInTestSolution().Count);
			Assert.AreEqual(0, ReferencesInProject(projects[0]).Count);
			Assert.AreEqual(1, ReferencesInProject(projects[1]).Count);
		}



		[Test]
		public void ICanUpdateARefereneceAndSaveTheChanges()
		{
			File.Copy(@"..\..\testdata\solutionsample\Hellblaster\HellBlasterTest\HellblasterTest.csproj_bck", @"..\..\testdata\solutionsample\Hellblaster\HellBlasterTest\HellblasterTest.csproj",true);

			List<VS10Project> projects=ProjectsInTestSolution();
			VS10ProjectReader projReader= new VS10ProjectReader();
			projReader.Read(projects[1].FullPath);
			List<FileReference> refs = projReader.FindFileReferences();
			Assert.AreEqual("2.6.1.12217", refs[0].Version);
			refs[0].UpdateTo("2.7");

			VS10ProjectWriter projWriter = new VS10ProjectWriter();
			projWriter.Read(projects[1].FullPath);
			projWriter.UpdateReference(refs[0]);
			projWriter.WriteToFile();
			projReader = new VS10ProjectReader(); ;
			projReader.Read(projects[1].FullPath);
			refs = projReader.FindFileReferences();
			Assert.AreEqual("2.7", refs[0].Version);			
		}
	}
}
