using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.View;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel.Setup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class ExamContentViewModel : ViewModelBase
{
    public CourseContent Content { get; set; }

    public string Name => Content.Name;

    public ICommand RemoveCommand { get; set; }

    public ExamContentViewModel(CourseContent content, Action<ExamContentViewModel> removeAction)
    {
        Content = content;
        RemoveCommand = new Command(() => removeAction(this));
    }
}
