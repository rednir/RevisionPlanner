﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RevisionPlanner.ViewModel;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

	public void OnPropertyChanged([CallerMemberName]string name = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

