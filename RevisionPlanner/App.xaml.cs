﻿using RevisionPlanner.View;

namespace RevisionPlanner;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new MainTabbedView();
	}
}

