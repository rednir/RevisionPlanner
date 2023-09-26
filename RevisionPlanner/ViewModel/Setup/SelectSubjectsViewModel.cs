﻿using System.ComponentModel;
using System.Windows.Input;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectSubjectsViewModel : ViewModelBase
{
    public ICommand NextCommand { get; private set; }

    public IEnumerable<PresetSubjectGrouping> PresetSubjectGroupings { get; set; } = new List<PresetSubjectGrouping>()
    {
        new PresetSubjectGrouping()
        {
            Name = "Grouping 1",
            Subjects = new List<PresetSubject>()
            {
                new PresetSubject
                {
                    Name = "Subject 1",
                },
                new PresetSubject
                {
                    Name = "Subject 2",
                },
                new PresetSubject
                {
                    Name = "Subject 3",
                },
            },
        },
        new PresetSubjectGrouping()
        {
            Name = "Grouping 2",
            Subjects = new List<PresetSubject>()
            {
                new PresetSubject
                {
                    Name = "Subject 1",
                },
            },
        },
    };

    public SelectSubjectsViewModel(Action next)
    {
        NextCommand = new Command(next);
    }
}

