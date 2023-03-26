using System.Collections.ObjectModel;
using Sweepminer.Models;

namespace Sweepminer.ViewModels;

public class DifficultyChooserViewModel : ViewModelBase {
    private ObservableCollection<Difficulty> _difficulties;
    private Difficulty _selectedDifficulty;

    public DifficultyChooserViewModel() {
        _difficulties = new ObservableCollection<Difficulty>();
        _difficulties.Add(new Difficulty(9, 9, 10) {
            Name = "Новичок"
        });
        _difficulties.Add(new Difficulty(16,16,40) {
            Name = "Любитель"
        });
        _difficulties.Add(new Difficulty(16, 30, 99) {
            Name = "Профессионал"
        });
    }

    public ObservableCollection<Difficulty> Difficulties {
        get => _difficulties;
        set => SetPropertyIfChanged(ref _difficulties, value);
    }

    public Difficulty SelectedDifficulty {
        get => _selectedDifficulty; 
        set => SetPropertyIfChanged(ref _selectedDifficulty, value);
    }
    
    
}