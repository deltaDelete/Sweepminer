using Sweepminer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Sweepminer.Views;

namespace Sweepminer.ViewModels;

internal class MainWindowViewModel : ViewModelBase {

    #region Команды
    public ICommand NewGameCommand { get; }
    #endregion
    #region Свойства

    public Minesweeper Game {
        get => _game;
        set {
            if (Equals(value, _game)) return;
            _game = value;
            OnPropertyChanged(nameof(Game));
        }
    }

    #endregion
    #region Поля

    private Minesweeper _game;
    
    #endregion
    public MainWindowViewModel() {
        NewGameCommand = new Command(NewGame);
        Game = new Minesweeper();
    }
    #region Методы
    private void NewGame() {
        DifficultyChooserWindow window = new DifficultyChooserWindow();
        var result = window.ShowDialog();
        if (result != true) return;
        var difficulty = window.SelectedDifficulty;
        Game = new Minesweeper(difficulty);
        Game.GenerateField();
    }

    #endregion
}
