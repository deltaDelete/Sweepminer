using Sweepminer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sweepminer.ViewModels;

internal class MainWindowViewModel : ViewModelBase {
    #region Команды
    public ICommand NewGameCommand { get; }
    #endregion
    #region Свойства
    public ObservableCollection<Cell> Cells { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
    public int Mines { get; set; }
    #endregion
    #region Поля

    #endregion
    public MainWindowViewModel() {
        NewGameCommand = new Command(NewGame);
        Cells = new(Enumerable.Range(0, Rows * Columns).Select(x => Cell.RandomCell()));
    }
    #region Методы
    private void NewGame() {
        DifficultyChooserWindow window = new DifficultyChooserWindow();
        RegenerateCells();
    }

    private void RegenerateCells(int rows, int columns, int mines) {
        Cells.Clear();
        Rows = rows;
        Columns = columns;
        Mines = mines;

        Cell[,] cells = new Cell[Rows, Columns];
        for (int i = 0; i < Rows; i++) {
            for (int j = 0; j < Columns; j++) {
                cells[i, j] = new Cell();
            }
        }

        for (int i = 0; i < mines; i++) {
            int randomCellX = Random.Shared.Next(0, Rows);
            int randomCellY = Random.Shared.Next(0, Columns);
            cells[randomCellX, randomCellY] = Cell.CreateMine();
        }
        for (int i = 0; i < Columns; i++) {
            for (int j = 0; j < Rows; j++) {
                if (cells[i, j].IsMine) continue;

                int num = 0;
                for (int k = -1; k < 2; k++) {
                    for (int l = -1; l < 2; l++) {
                        try {
                            num += cells[i + k, j + l].IsMine ? 1 : 0;
                        }
                        catch (IndexOutOfRangeException) {
                            // MessageBox.Show($"Индекс [{i+k}, {j+l}] вне границ");
                        }
                    }
                }

                cells[i, j].Number = num;
            };
        }

        for (int i = 0; i < Columns; i++) {
            for (int j = 0; j < Rows; j++) {
                Cells.Add(cells[i, j]);
            }
        }
    }
    #endregion
}
