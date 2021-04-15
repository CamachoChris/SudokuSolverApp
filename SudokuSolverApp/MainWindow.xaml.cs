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
        const int ColumnCount = 9;
        const int RowCount = 9;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }

        public void InitializeWindow()
        {
            ColumnDefinition[] columnsArray = new ColumnDefinition[ColumnCount];
            for (int i = 0; i < columnsArray.Length; ++i)
            {
                columnsArray[i] = new ColumnDefinition();
                playingFieldGrid.ColumnDefinitions.Add(columnsArray[i]);
            }

            RowDefinition[] rowsArray = new RowDefinition[RowCount];
            for (int i = 0; i < rowsArray.Length; ++i)
            {
                rowsArray[i] = new RowDefinition();
                playingFieldGrid.RowDefinitions.Add(rowsArray[i]);
            }
            for (int row = 0; row < RowCount; row++)
                for (int column = 0; column < ColumnCount; column++)
                {
                    TextBox textbox = new TextBox();
                    textbox.HorizontalContentAlignment = HorizontalAlignment.Center;
                    textbox.VerticalContentAlignment = VerticalAlignment.Center;
                    textbox.FontSize = 15;
                    if (column == 2 || column == 5)
                    {
                        if (row == 2 || row == 5)
                            textbox.Margin = new Thickness(0, 0, 2, 2);
                        else
                            textbox.Margin = new Thickness(0, 0, 2, 0);
                    }
                    if (row == 2 || row == 5)
                    {
                        if (column == 2 || column == 5) { }
                        else
                            textbox.Margin = new Thickness(0, 0, 0, 2);
                    }
                    textbox.Name = string.Format($"tb{column}{row}");
                    Grid.SetColumn(textbox, column);
                    Grid.SetRow(textbox, row);
                    playingFieldGrid.Children.Add(textbox);
                }
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            ResultWindow resultWindow = new ResultWindow();
            resultWindow.Owner = this;
            resultWindow.Show();
            SudokuPlayingField sudokuPlayingField = new SudokuPlayingField();

            int i = 0;
            int column;
            int row;

            foreach (var entry in playingFieldGrid.Children)
            {
                column = i % ColumnCount;
                row = i / ColumnCount;

                if ((entry as TextBox).Text != "")
                {
                    if (!int.TryParse((entry as TextBox).Text, out int number) || !((0 < number) && (number <= 9)))
                    {
                        MessageBox.Show(this, "Only numbers from 1 to 9.");
                        resultWindow.Close();
                        return;
                    }

                    sudokuPlayingField.SetFixNumber(column, row, number);
                }
                i++;
            }

            SudokuSolver solver = new SudokuSolver(sudokuPlayingField);
            solver.Solve();

            i = 0;
            SudokuCell[] fieldArray = new SudokuCell[ColumnCount * RowCount];
            for (row = 0; row < RowCount; row++)
                for (column = 0; column < ColumnCount; column++)
                {
                    fieldArray[i] = sudokuPlayingField.playingField[column, row];
                    i++;
                }

            i = 0;
            foreach (var entry in resultWindow.resultGrid.Children)
            {
                if (fieldArray[i].GetNumber() != 0)
                {
                    ((entry as TextBox).Text) = string.Format($"{fieldArray[i].GetNumber()}");
                    if (fieldArray[i].IsFixNumber())
                        (entry as TextBox).Foreground = Brushes.OrangeRed;
                }
                i++;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox entry in playingFieldGrid.Children)
            {
                entry.Text = "";
            }

        }
    }
}