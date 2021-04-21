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
using Sudoku;
using SudokuStandardPattern;

namespace SudokuSolverApp
{
    public partial class ResultWindow : Window
    {
        public SudokuPlayingField SolvedSudoku;

        public ResultWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SudokuPattern.Init(resultGrid);
            SudokuPattern.LoadFromSudokuField(resultGrid, SolvedSudoku);
        }
    }
}
