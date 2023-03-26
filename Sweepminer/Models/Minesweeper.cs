using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Sweepminer.Models;

public class Minesweeper : INotifyPropertyChanged {
    private Difficulty _difficulty;
    private ObservableCollection<Cell> _field;
    private int _flags = 0;
    private bool _isFieldUnlocked = true;
    private bool _isWin = false;

    public Difficulty Difficulty {
        get => _difficulty;
        set => SetField(ref _difficulty, value);
    }

    public ObservableCollection<Cell> Field {
        get => _field;
        set => SetField(ref _field, value);
    }

    public int Flags {
        get => _flags;
        set => SetField(ref _flags, value);
    }

    public bool IsFieldUnlocked {
        get => _isFieldUnlocked;
        set => SetField(ref _isFieldUnlocked, value);
    }

    public bool IsWin {
        get => _isWin;
        set => SetField(ref _isWin, value);
    }

    public Minesweeper(Difficulty difficulty) {
        Difficulty = difficulty;
        Field = new();
    }

    public Minesweeper() : this(new Difficulty(9, 9, 10)) {
    }

    public void GenerateField() {
        IsFieldUnlocked = true;
        IsWin = false;
        
        Cell[,] cells = new Cell[Difficulty.Rows, Difficulty.Columns];

        for (int i = 0; i < Difficulty.Rows; i++) {
            for (int j = 0; j < Difficulty.Columns; j++) {
                cells[i, j] = new Cell();
            }
        }

        for (int i = 0; i < Difficulty.Mines; i++) {
            int randomCellX = Random.Shared.Next(0, Difficulty.Rows);
            int randomCellY = Random.Shared.Next(0, Difficulty.Columns);
            cells[randomCellX, randomCellY] = Cell.CreateMine();
        }

        Flags = Difficulty.Mines;
        for (int i = 0; i < Difficulty.Columns; i++) {
            for (int j = 0; j < Difficulty.Rows; j++) {
                if (cells[i, j].IsMine) {
                    cells[i, j].Opened += Lose;
                    cells[i, j].Flagged += OnFlagged;
                    continue;
                };

                int num = 0;
                for (int k = -1; k < 2; k++) {
                    for (int l = -1; l < 2; l++) {
                        try {
                            num += cells[i + k, j + l].IsMine ? 1 : 0;
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }

                cells[i, j].Number = num;
            }
        }

        for (int i = 0; i < Difficulty.Columns; i++) {
            for (int j = 0; j < Difficulty.Rows; j++) {
                Field.Add(cells[i, j]);
            }
        }

        for (int i = 0; i < Field.Count; i++) {
            if (Field[i].IsMine) continue;

            if (Field[i].Number == 0) {
                Field[i].Opened += OpenAdjacentCells;
            }
        }
    }

    private void OpenAdjacentCells(Cell cell1) {
        foreach (var cell in cell1.GetNeighbors(ref _field, Difficulty.Columns, Difficulty.Rows)) {
            if (cell.Number > -1) {
                cell.Open();
            }
        }
    }

    private void OnFlagged(Cell cell) {
        if (cell.IsFlagged) {
            Flags--;
        }
        else {
            Flags++;
        }

        if (Flags == 0) {
            CheckWin();
        }
    }

    private void Lose(Cell cell) {
        if (!IsFieldUnlocked) return;
        IsFieldUnlocked = false;
        OpenAllCells();
        MessageBox.Show("Поздравляю с поражением, но ничего, игра - это не настоящая жизнь, можешь пробовать сколько хочется!");
    }

    private void CheckWin() {
        if (IsWin) return;
        int totalFlaggedRight = 0;
        foreach (var cell in Field) {
            if (cell.IsMine && cell.IsFlagged) {
                totalFlaggedRight++;
            }
        }

        if (totalFlaggedRight != Difficulty.Mines) return;

        MessageBox.Show("Ура победа!");
        IsFieldUnlocked = false;
        IsWin = true;
        OpenAllCells();
    }

    public void OpenAllCells() {
        foreach (var cell in Field) {
            cell.Open();
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}