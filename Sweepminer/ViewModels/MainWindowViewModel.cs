using Sweepminer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Sweepminer.Views;

namespace Sweepminer.ViewModels;

internal class MainWindowViewModel : ViewModelBase {
    #region Команды

    public ICommand NewGameCommand { get; }
    public ICommand CloseCommand { get; } = new Command(Application.Current.Shutdown);
    public ICommand HelpCommand { get; }
    public ICommand AboutCommand { get; }

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

    public TimeSpan Timer {
        get {
            var now = TimeSpan.FromTicks(DateTime.Now.Ticks);
            var time = now - _startTime;
            if (time == now) {
                return TimeSpan.Zero;
            }

            return time;
        }
    }

    #endregion

        #region Поля

        private DispatcherTimer _timer = new DispatcherTimer();
    private TimeSpan _startTime;
    private Minesweeper _game;

    #endregion

    public MainWindowViewModel() {
        NewGameCommand = new Command(NewGame);
        HelpCommand = new Command(Help);
        AboutCommand = new Command(About);
        Game = new Minesweeper();
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(0.5);
        _timer.Tick += (sender, args) => { OnPropertyChanged(nameof(Timer)); };
    }

    #region Методы

    private void NewGame() {
        DifficultyChooserWindow window = new DifficultyChooserWindow();
        var result = window.ShowDialog();
        if (result != true) return;
        var difficulty = window.SelectedDifficulty;
        Game = new Minesweeper(difficulty);
        Game.GenerateField();
        _startTime = TimeSpan.FromTicks(DateTime.Now.Ticks);
        _timer.Start();
        Game.Losed += () => _timer.Stop();
        Game.Won += () => _timer.Stop();
        Game.Won += GameOnWon;
    }

    private void GameOnWon() {
        MessageBox.Show($"Ура победа!\nВы зачистили поле за {Timer.ToString("T")}");
    }

    public void Help() {
        MessageBox.Show("Играть довольно просто, " +
                        "начинаем с любой клетки, если в клетке есть число, " +
                        "то в поле 3х3 клетки с числом в центре, будет находиться указанное количество мин",
            icon: MessageBoxImage.Information, caption: "", button: MessageBoxButton.OK);
    }

    public void About() {
        MessageBox.Show("Разработчики:\n" +
                        "\tВоронин Роман Витальевич\n" +
                        "\tКоронденко Илья Вячеславович\n" +
                        "Разработано в рамках самостоятельной работы\n" +
                        "Copyright 2023");
    }

    #endregion
}