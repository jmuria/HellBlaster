using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using HellBlaster.Interfaces;
using HellBlaster.Controllers;

namespace HellBlasterTest.Tests
{
	[TestFixture]
	public class MainControllerTests
	{
		private static string TestSolutionPath()
		{
			return  @"..\..\testdata\solutionsample\Hellblaster\Hellblaster.sln";			
		}

		[Test]
		public void WhenILoadASolutionFileIShowAllTheProjectsAndFileReferences()
		{
			MainPageCtrl mc = new MainPageCtrl();
			var viewmock= new Mock<IMainPageView>();
			mc.View =viewmock.Object;

			mc.LoadSolutionFile(TestSolutionPath());

			viewmock.Verify(foo => foo.AddProject(It.IsAny<string>()), Times.Exactly(2));
			viewmock.Verify(foo => foo.AddFileRefence(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
		}

	}
}
