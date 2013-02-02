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

namespace HellBlaster
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window,IMainPageView
	{
		MainPageCtrl ctrl;
		public MainWindow()
		{
			ctrl = new MainPageCtrl();
			ctrl.View = this;
			InitializeComponent();
		}



		public void AddProject(string projectName)
		{
			TextBlock tb = new TextBlock();
			tb.Text = projectName;
			Projects.Children.Add(tb);
		}

		public void AddFileRefence(string name, string version)
		{
			TextBlock tb = new TextBlock();
			DisplayReference(tb,name, version);
			Projects.Children.Add(tb);
		}

		private static void DisplayReference( TextBlock tb,string name, string version)
		{
			tb.Text = "\t" + ReferenceText(name, version);
		}

		private static string ReferenceText(string name, string version)
		{
			return name + ": " + version;
		}

		public void UpdateFileRefence(string projectName, string assemblyName, string assemblyVersion)
		{
			foreach (TextBlock tb in Projects.Children)
			{
				if (tb.Text.Contains(assemblyName))
				{
					DisplayReference(tb, assemblyName, assemblyVersion);
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
	}
}
