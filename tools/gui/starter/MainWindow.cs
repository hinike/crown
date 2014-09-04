﻿using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{
	//--------------------------------------------------------------------------------
	enum Platform
	{
		Linux32 = 0,
		Linux64,
		Windows32,
		Windows64,
		Android
	}

	//--------------------------------------------------------------------------------
	enum BuildMode
	{
		Debug = 0,
		Development,
		Release
	}

	public string project_name = null;
	public string source_path = null;
	public string destination_path = null;

	//--------------------------------------------------------------------------------
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	//--------------------------------------------------------------------------------
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	//--------------------------------------------------------------------------------
	protected void OnProjectButtonClicked (object sender, EventArgs e)
	{
		starter.ProjectDialog pd = new starter.ProjectDialog (this);
		pd.Run ();
		pd.Destroy ();

		project_entry.Text = project_name;
	}

	//--------------------------------------------------------------------------------
	protected void OnRunButtonClicked (object sender, EventArgs e)
	{
		Platform platform = (Platform) platform_combobox.Active;
		BuildMode build = (BuildMode) build_combobox.Active;

		// Do not use MD for compiling because it cannot retrieve env value even though is specified in target environment.
		// Use 'xbuild' command instead.
		string path = Environment.GetEnvironmentVariable("CROWN_INSTALL_DIR") + "/";
		string executable = "";

		switch (platform)
		{
			case Platform.Linux32:
			{
				path += "bin/linux32/";
				switch (build)
				{
				case BuildMode.Debug: executable = "crown-debug-32"; break;
				case BuildMode.Development: executable = "crown-development-32"; break;
				case BuildMode.Release: executable = "crown-release-32"; break;
				}
				break;
			}
			case Platform.Linux64:
			{
				path += "bin/linux64/";
				switch (build)
				{
				case BuildMode.Debug: executable = "crown-debug-64"; break;
				case BuildMode.Development: executable = "crown-development-64"; break;
				case BuildMode.Release: executable = "crown-release-64"; break;
				}
				break;
			}
			case Platform.Windows32:
			{
				path += "bin\\windows32\\";
				switch (build)
				{
				case BuildMode.Debug: executable = "crown-debug-32"; break;
				case BuildMode.Development: executable = "crown-development-32"; break;
				case BuildMode.Release: executable = "crown-release-32"; break;
				}
				break;
			}
			case Platform.Windows64:
			{
				path += "bin\\windows64\\";
				switch (build)
				{
				case BuildMode.Debug: executable = "crown-debug-64"; break;
				case BuildMode.Development: executable = "crown-development-64"; break;
				case BuildMode.Release: executable = "crown-release-64"; break;
				}
				break;
			}
		}

		string args = " --source-dir " + source_path;
		args += " --bundle-dir " + destination_path;
		args += " --compile --continue";

		System.IO.Directory.SetCurrentDirectory (path);
		System.Diagnostics.Process.Start (executable, args);
	}
}