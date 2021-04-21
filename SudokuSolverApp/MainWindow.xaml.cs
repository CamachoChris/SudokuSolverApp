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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Sudoku;

namespace SudokuSolverApp
{

    public partial class MainWindow : Window
    {
        const string appName = "SudokuSolver";
        const string version = "0.1.1";
        const string developer = "Grimakar";
        const string timeOfDevelopment = "April 2021";

        SudokuGrid sudokuGrid = new SudokuGrid();
        SudokuPlayingField unsolvedSudoku = new SudokuPlayingField();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            ResultWindow resultWindow = new ResultWindow();
            resultWindow.Owner = this;

            sudokuGrid.GetSudokuField(out unsolvedSudoku);

            //Solving the Sudoku
            SudokuSolver solver = new SudokuSolver(unsolvedSudoku);
            solver.Solve();

            resultWindow.SolvedSudoku = unsolvedSudoku;
            resultWindow.Show();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            sudokuGrid.Clear();
        }


        private void MenuQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, $"{appName}\n{version}\n{timeOfDevelopment} {developer}.\nNo rights reserved...", $"About {appName}");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(sudokuGrid, 1);
            MainGrid.Children.Add(sudokuGrid);
        }
    }
}