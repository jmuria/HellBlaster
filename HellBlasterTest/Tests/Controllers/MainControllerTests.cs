using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using HellBlaster.Interfaces;
using HellBlaster.Controllers;
using System.IO;
using HellBlaster.VS10;
using HellBlaster.Domain;

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
		public void WhenILoadASolutionFileThePreviouslyLoadedReferencesAreCleaned()
		{
			mainCtrl.LoadSolutionFile(TestSolutionPath);

			viewMock.Verify(foo => foo.CleanReferences(), Times.Exactly(1));			
		}

		[Test]
		public void WhenIChangeAReferenceVersionTheInterfaceShowsTheChanges()
		{
			File.Copy(TestProjectPathBackup, TestProjectPath, true);
			
			mainCtrl.LoadSolutionFile(TestSolutionPath);
			mainCtrl.UpdateFileReference("nunit.framework", "2.7");

			viewMock.Verify(foo => foo.UpdateFileRefence("HellBlasterTest", "nunit.framework", "2.7"), Times.Exactly(1));
		}


		[Test]
		public void WhenIStartTypingAnEmptyReferenceNameTheSystemDoesNotSuggest()
		{
			AddFileReference("nunit.framework");
			AddFileReference("core");
			AddFileReference("FIMS.DataTypes");
			Assert.IsFalse(mainCtrl.ReferenceSuggested(String.Empty));			
			
			viewMock.Verify(foo => foo.SuggestFileRefence(It.IsAny<string>()), Times.Never());
		}


		[Test]
		public void WhenIStartTypingAReferenceNameAndThereIsAnyReferenceTheSystemDoesNotSuggest()
		{
			Assert.IsFalse(mainCtrl.ReferenceSuggested("nun"));

			viewMock.Verify(foo => foo.SuggestFileRefence(It.IsAny<string>()), Times.Never());
		}

		[Test]
		public void WhenIStartTypingAReferenceNameAndTheReferenceExistsTheSystemSuggestsIt()
		{
			mainCtrl.ReferenceList = new List<FileReference>();
			AddFileReference("nunit.framework");
			AddFileReference("core");
			Assert.IsTrue(mainCtrl.ReferenceSuggested("nun"));

			viewMock.Verify(foo => foo.SuggestFileRefence("nunit.framework"), Times.Once());
		}


		[Test]
		public void WhenIStartTypingAReferenceNameAndTheSystemFindsMoreThanOneReferenceTheSystemDoesNotSuggest()
		{
			mainCtrl.ReferenceList = new List<FileReference>();
			AddFileReference("nunit.framework");
			AddFileReference("nunit.framework2");
			AddFileReference("nunit.framework3");
			AddFileReference("core");
			Assert.IsFalse(mainCtrl.ReferenceSuggested("nun"));

			viewMock.Verify(foo => foo.SuggestFileRefence(It.IsAny<string>()), Times.Never());
		}


		[Test]
		public void WhenIStartTypingAReferenceNameAndTheReferenceIsMoreThanOnceTheSystemSuggestsIt()
		{
			mainCtrl.ReferenceList = new List<FileReference>();
			AddFileReference("nunit.framework");
			AddFileReference("nunit.framework");			
			AddFileReference("core");
			Assert.IsTrue(mainCtrl.ReferenceSuggested("nun"));

			viewMock.Verify(foo => foo.SuggestFileRefence("nunit.framework"), Times.Once());
		}

		[Test]
		public void WhenIStartTypingAReferenceNameAndTheReferenceExistsTheSystemSuggestsIt2()
		{
			mainCtrl.ReferenceList = new List<FileReference>();
			AddFileReference("nunit.framework");
			AddFileReference("core");
			Assert.IsTrue(mainCtrl.ReferenceSuggested("c"));

			viewMock.Verify(foo => foo.SuggestFileRefence("core"), Times.Once());
		}


		[Test]
		public void WhenIStartTypingAReferenceNameAndTheReferenceIsNotFoundTheSystemDoesNotSuggest()
		{
			mainCtrl.ReferenceList = new List<FileReference>();
			AddFileReference("nunit.framework");
			AddFileReference("core");
			Assert.IsFalse(mainCtrl.ReferenceSuggested("j"));

			viewMock.Verify(foo => foo.SuggestFileRefence(It.IsAny<string>()), Times.Never());
		}

		private void AddFileReference(string name)
		{
			FileReference refer = new FileReference(name, String.Empty, String.Empty);
			mainCtrl.ReferenceList.Add(refer);
		}


		[Test]
		public void WhenILoadASolutionFileAndIStartTypingAReferenceIShowAllTheSystemSuggestsIt()
		{
			mainCtrl.LoadSolutionFile(TestSolutionPath);

			mainCtrl.ReferenceSuggested("nun");

			viewMock.Verify(foo => foo.SuggestFileRefence("nunit.framework"), Times.Once());
		}


		[Test]
		public void WhenThereIsADiscrepancyInTheSolutionItIsMarkedInTheView()
		{
			mainCtrl.ReferenceList = new List<FileReference>();
			mainCtrl.ReferenceList.Add(new FileReference("nunit.framework", "2.4", ""));
			mainCtrl.ReferenceList.Add(new FileReference("core", "1.0", ""));
			mainCtrl.ReferenceList.Add(new FileReference("nunit.framework", "2.6", ""));
			mainCtrl.CheckDiscrepancies();
			viewMock.Verify(foo => foo.ShowDiscrepancy("nunit.framework", "2.4"), Times.Once());
			viewMock.Verify(foo => foo.ShowDiscrepancy("nunit.framework", "2.6"), Times.Once());
		}

	}
}
