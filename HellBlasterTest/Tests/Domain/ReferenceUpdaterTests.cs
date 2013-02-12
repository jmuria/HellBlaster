using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HellBlaster.Domain;

namespace HellBlasterTest.Tests
{
	[TestFixture]
	public class ReferenceUpdaterTests
	{
		[Test]
		public void WhenIUpdateWithTheSameVersionThePathRemainsTheSame()
		{
			FileReference fileRef = new FileReference("1.0.0",@"R:\lib\1.0.0\mylib.dll");
			fileRef.UpdateTo("1.0.0");
			Assert.AreEqual(@"R:\lib\1.0.0\mylib.dll", fileRef.Path);
		}


		[Test]
		public void WhenIUpdateWithANewerVersionThePathChanges()
		{
			FileReference fileRef = new FileReference("1.0.0", @"R:\lib\1.0.0\mylib.dll");
			fileRef.UpdateTo("1.5.0");
			Assert.AreEqual(@"R:\lib\1.5.0\mylib.dll", fileRef.Path);
		}

		[Test]
		public void WhenIUpdateWithANewerVersionButWithLessDigitsThePathChanges()
		{
			FileReference fileRef = new FileReference("1.1.0.233", @"R:\lib\1.1.0\mylib.dll");
			fileRef.UpdateTo("1.5.0");
			Assert.AreEqual(@"R:\lib\1.5.0\mylib.dll", fileRef.Path);
		}

		[Test]
		public void WhenIUpdateWithANewerVersionButWithLessDigitsThePathChanges2()
		{
			FileReference fileRef = new FileReference("1.1.233.0", @"R:\lib\1.1\mylib.dll");
			fileRef.UpdateTo("1.2");
			Assert.AreEqual(@"R:\lib\1.2\mylib.dll", fileRef.Path);
		}
	}
}
