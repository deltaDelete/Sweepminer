using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Sweepminer;

internal class MainWindowViewModel : ViewModelBase {
    #region Команды
    public ICommand NewGameCommand { get; }
    #endregion
    #region Свойства
    public ObservableCollection<Cell> Cells { get; set; } 
    public int Rows { get; set; }
    public int Columns { get; set; }
    #endregion
    #region Поля

    #endregion
    public MainWindowViewModel() {
        NewGameCommand = new Command(NewGame);
        Cells = new (Enumerable.Range(0, Rows * Columns).Select(x => Cell.RandomCell()));
    }
    #region Методы
    private void NewGame() {
        //Cells.Clear();
        //Rows = 10;
        //Columns = 10;
        //var cellsToAdd = Enumerable.Range(0, Rows * Columns).Select(x => Cell.RandomCell());
        //foreach (var item in cellsToAdd) {
        //    Cells.Add(item);
        //}
        Rows = 6;
        Columns = 6;
        int minesAmount = 5;

        var cellsToAdd = Enumerable.Range(0, Rows * Columns)
            .Select(x => new Cell()).ToList();
        for (int i = 0; i < minesAmount; i++) {
            int randomCellIndex = Random.Shared.Next(0, cellsToAdd.Count());
            cellsToAdd[randomCellIndex] = Cell.CreateMine();
        }
        for (int i = 0; i < cellsToAdd.Count(); i++) {
            var currentCell = cellsToAdd[i];
            if (currentCell.IsMine) continue;

            // не раб
            Cell[,] cells = new Cell[3,3] {
                { cellsToAdd[i-(Columns+1)], cellsToAdd[i-Columns], cellsToAdd[i-(Columns-1)] },
                { cellsToAdd[i-1],           cellsToAdd[i],         cellsToAdd[i+1] },
                { cellsToAdd[i+Columns-1],   cellsToAdd[i+Columns], cellsToAdd[i+Columns+1] }
            };
        }
    }
    #endregion
}