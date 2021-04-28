using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using Sudoku;

namespace SudokuSolverApp
{
    public partial class ResultWindow : Window
    {
        private SudokuGrid _sudokuGrid = new SudokuGrid();
        public SudokuSolverVM SolverVM = new SudokuSolverVM();

        public ResultWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => { SolverVM.Solve(); });
            MainGrid.Children.Add(_sudokuGrid);
            _sudokuGrid.SetSudokuField(SolverVM.PlayingField);
        }
    }
}
