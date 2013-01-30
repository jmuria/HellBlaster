using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using HellBlaster.Interfaces;
using HellBlaster.Controllers;
using System.IO;

namespace HellBlasterTest.Tests
{
	[TestFixture]
	public class MainControllerTests
	{
		private static string TestSolutionPath		{ get {return @"..\..\testdata\solutionsample\Hellblaster\Hellblaster.sln";}}
		private static string TestProjectPath		{ get {return @"..\..\testdata\solutionsample\Hellblaster\HellBlasterTest\HellblasterTest.csproj";}}
		private static string TestProjectPathBackup	{ get {return @"..\..\testdata\solutionsample\Hellblaster\HellBlasterTest\HellblasterTest.csproj_bck";	}}

		MainPageCtrl mainCtrl;
		Mock<IMainPageView> viewMock;

		[SetUp]
		public void SetUp()
		{
			mainCtrl = new MainPageCtrl();
			viewMock = new Mock<IMainPageView>();
			mainCtrl.View = viewMock.Object;
		}

		[Test]
		public void WhenILoadASolutionFileIShowAllTheProjectsAndFileReferences()
		{
			mainCtrl.LoadSolutionFile(TestSolutionPath);

			viewMock.Verify(foo => foo.AddProject(It.IsAny<string>()), Times.Exactly(2));
			viewMock.Verify(foo => foo.AddFileRefence(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
		}

		[Test]
		public void WhenIChangeAReferenceVersionTheInterfaceShowsTheChanges()
		{
			File.Copy(TestProjectPathBackup, TestProjectPath, true);
			
			mainCtrl.LoadSolutionFile(TestSolutionPath);
			mainCtrl.UpdateFileReference("nunit.framework", "2.7");

			viewMock.Verify(foo => foo.UpdateFileRefence("HellBlasterTest", "nunit.framework", "2.7"), Times.Exactly(1));
		}

	}
}
