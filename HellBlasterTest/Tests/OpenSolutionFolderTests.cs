using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HellBlaster.Domain;

namespace HellBlasterTest.Tests
{
	[TestFixture]
	public class OpenSolutionFolderTests
	{
		[Test]
		public void WhenIOpenASolutionFileICanReadTheProjects()
		{
			string path = @"..\..\testdata\solutionsample\Hellblaster\Hellblaster.sln";
			VS10SolutionReader solReader = new VS10SolutionReader();
			solReader.Read(path);
			List<VS10Project> projects=solReader.FindProjects();
			Assert.AreEqual(2, projects.Count);
		}


		[Test]
		public void WhenIOpenASolutionFileICanReadTheProjectsAndTheFileReferences()
		{
			string path = @"..\..\testdata\solutionsample\Hellblaster\Hellblaster.sln";
			VS10SolutionReader solReader = new VS10SolutionReader();
			solReader.Read(path);
			List<VS10Project> projects = solReader.FindProjects();
			Assert.AreEqual(2, projects.Count);
			VS10ProjectReader projReader = new VS10ProjectReader();
			projReader.Read(projects[0].FullPath);
			List<FileReference> refs=projReader.FindReferences();
			Assert.AreEqual(0, refs.Count);

			projReader.Read(projects[1].FullPath);
			refs = projReader.FindReferences();
			Assert.AreEqual(1, refs.Count);

		}
	}
}
