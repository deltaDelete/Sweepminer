using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sweepminer.Models;

public class Cell : INotifyPropertyChanged {
    public ICommand FlagCommand { get; }
    public ICommand OpenCommand { get; }
    #region Свойства
    public Guid Id { get; } = Guid.NewGuid();
    public bool IsMine {
        get => _isMine;
        set => SetProperty(ref _isMine, value);
    }
    public bool IsOpened {
        get => _isOpened;
        set => SetProperty(ref _isOpened, value);
    }
    public bool IsFlagged {
        get => _isFlagged;
        set => SetProperty(ref _isFlagged, value);
    }
    public int Number {
        get => _number;
        set => SetProperty(ref _number, value);
    }
    public ImageSource Image {
        get => _image;
        set => SetProperty(ref _image, value);
    }
    #endregion
    #region Поля
    private static readonly BitmapImage _flag = new BitmapImage(new Uri("pack://application:,,,/Assets/flag.png"));
    private static readonly BitmapImage _easteregg = new BitmapImage(new Uri("pack://application:,,,/Assets/easteregg.png"));
    private static readonly BitmapImage _mine = new BitmapImage(new Uri("pack://application:,,,/Assets/mine.png"));

    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action<Cell> Opened;
    public event Action<Cell> Flagged;

    private ImageSource _image;
    private bool _isOpened = false;
    private bool _isFlagged = false;
    private bool _isMine;
    private int _number;
    #endregion

    public Cell() {
        FlagCommand = new Command(Flag);
        OpenCommand = new Command(Open, () => !IsFlagged && !IsOpened);
    }

    #region Методы
    
    public void Open() {
        if(IsOpened) return;
        IsOpened = true;
        
        if (IsMine) {
            Image = _mine;
        }

        Opened?.Invoke(this);
        //
    }

    public List<Cell> GetNeighbors(ref ObservableCollection<Cell> field, int cols, int rows) {
        List<Cell> neighbors = new List<Cell>();
        int[] dx = {
            -1, 0, 1,
            -1, 1, -1,
            0, 1
        };
        int[] dy = {
            -1, -1, -1,
            0, 0, 1,
            1, 1
        };

        var index = field.IndexOf(this);
        var (x,y) = Index1DTo2D(index, cols);
        
        for (int i = 0; i < dx.Length; i++) {
            int newX = x + dx[i];
            int newY = y + dy[i];
            if (newX >= 0 && newX < cols && newY >= 0 && newY < rows) {
                neighbors.Add(field[Index2DTo1D(newX, newY, cols)]);
            }
        }

        return neighbors;
    }
    
    public int Index2DTo1D(int x, int y, int cols) {
        return y * cols + x;
    }

    public (int x, int y) Index1DTo2D(int index, int cols) {
        return (index % cols, index / cols);
    }

    public void Flag() {
        IsFlagged = !IsFlagged;
        Flagged?.Invoke(this);
        if (IsFlagged && !IsOpened) {
            var rnd = Random.Shared.Next(0, 100);
            Image = _flag;
            if (rnd == 18) {
                Image = _easteregg;
            }
        }
        else if (!IsOpened) {
            Image = null;
        }
    }
    public static Cell RandomCell() {
        var cell = new Cell() {
            IsMine = Random.Shared.Next(0, 2) switch {
                0 => false,
                1 => true,
                _ => throw new NotImplementedException()
            },
        };
        cell.Number = cell.IsMine ? Random.Shared.Next(1, 8) : 0;
        return cell;
    }
    public void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "") {
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public static Cell CreateMine() {
        var cell = new Cell() {
            IsMine = true,
            Number = -1
        };
        return cell;
    }
    #endregion
}
