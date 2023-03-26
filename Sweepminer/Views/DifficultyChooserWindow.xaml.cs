using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sweepminer.Models;
using Sweepminer.ViewModels;

namespace Sweepminer.Views;
/// <summary>
/// Логика взаимодействия для DifficultyChooserWindow.xaml
/// </summary>
public partial class DifficultyChooserWindow : Window {
    public DifficultyChooserWindow() {
        InitializeComponent();
    }
    public Difficulty SelectedDifficulty {
        get => (DataContext as DifficultyChooserViewModel).SelectedDifficulty;
    }

    public void CancelClick(object sender, RoutedEventArgs e) {
        DialogResult = false;
        Close();
    }

    public void OkClick(object sender, RoutedEventArgs e) {
        DialogResult = true;
        Close();
    }
}
