using System;
using System.Collections.Generic;
using System.Text;
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
        SudokuGrid sudokuGrid = new SudokuGrid();
        public SudokuSolverVM SolverVM = new SudokuSolverVM();

        public ResultWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SolverVM.SudokuSolved += SolverVM_SudokuSolved;
            SolverVM.Solve();
        }

        private void SolverVM_SudokuSolved(object sender, EventArgs e)
        {
            MainGrid.Children.Add(sudokuGrid);
            sudokuGrid.SetSudokuField(SolverVM.PlayingField);
        }
    }
}
