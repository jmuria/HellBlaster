using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HellBlaster.Interfaces;
using HellBlaster.Controllers;
using HellBlaster.View;
using System.Collections.ObjectModel;

namespace HellBlaster
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window,IMainPageView
	{
		MainPageCtrl ctrl;
		public ObservableCollection<Reference> References { get; set; }
		string lastProjectName;

		public MainWindow()
		{
			ctrl = new MainPageCtrl();
			ctrl.View = this;
			References = new ObservableCollection<Reference>();			
			
			InitializeComponent();
			
			Projects.DataContext = References;
		}



		public void AddProject(string projectName)
		{				
			lastProjectName = projectName;			
		}

		public void AddFileRefence(string name, string version)
		{
			Reference newRef = new Reference();
			newRef.projectName = lastProjectName;
			newRef.referenceName = name;
			newRef.version = version;
			References.Add(newRef);
		}

		
		

		public void UpdateFileRefence(string projectName, string assemblyName, string assemblyVersion)
		{
			foreach (Reference refItem in References)
			{
				if (refItem.referenceName ==assemblyName)
				{
					refItem.version=assemblyVersion;
				}
			}
		}

		private void Select_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.DefaultExt = ".sln";
			dlg.Filter = "Visual Studio Solution files (.sln)|*.sln";

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				string filename = dlg.FileName;
				SolutionPath.Text = filename;
			}
		}

		private void Analyze_Click(object sender, RoutedEventArgs e)
		{
			ctrl.LoadSolutionFile(SolutionPath.Text);
		}

		private void Update_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(AssemblyName.Text) && !String.IsNullOrEmpty(NewVersion.Text))
			{
				ctrl.UpdateFileReference(AssemblyName.Text, NewVersion.Text);
			}
		}

		

		private void AssemblyName_KeyDown(object sender, KeyEventArgs e)
		{
			ctrl.WritingReference(AssemblyName.Text);
		}


		public void SuggestFileRefence(string assemblyName)
		{
			AssemblyName.Text = assemblyName;
		}

		

		
	}
}
