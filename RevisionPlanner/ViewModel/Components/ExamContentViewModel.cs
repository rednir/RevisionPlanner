using RevisionPlanner.Model;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class ExamContentViewModel : ViewModelBase
{
    public ICourseContent Content { get; set; }

    public string Name => Content.Name;

    public ICommand RemoveCommand { get; set; }

    public ExamContentViewModel(ICourseContent content, Action<ExamContentViewModel> removeAction)
    {
        Content = content;
        RemoveCommand = new Command(() => removeAction(this));
    }
}
