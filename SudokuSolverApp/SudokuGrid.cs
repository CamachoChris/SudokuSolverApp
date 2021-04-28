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
    class SudokuGrid : Grid
    {
        const int ColumnCount = 9;
        const int RowCount = 9;

        public SudokuGrid()
        {
            //Start with 9 columns and 9 rows.
            ColumnDefinition[] columnsArray = new ColumnDefinition[ColumnCount];
            for (int i = 0; i < columnsArray.Length; ++i)
            {
                columnsArray[i] = new ColumnDefinition();
                this.ColumnDefinitions.Add(columnsArray[i]);
            }

            RowDefinition[] rowsArray = new RowDefinition[RowCount];
            for (int i = 0; i < rowsArray.Length; ++i)
            {
                rowsArray[i] = new RowDefinition();
                this.RowDefinitions.Add(rowsArray[i]);
            }

            //Make 9x9 texboxes and make them child of the Grid sudokupattern.
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
                    Grid.SetColumn(textbox, column);
                    Grid.SetRow(textbox, row);
                    this.Children.Add(textbox);
                }
        }

        /// <summary>
        /// Gets the numbers of the SudokuGrid and puts them into the SudokuPlayingField.
        /// </summary>
        /// <param name="sudokuPattern"></param>
        /// <param name="playingField"></param>
        /// <returns>Returns true, if all entries are valid. False, if not, and all numbers set to 0.</returns>
        public bool GetSudokuField(out SudokuPlayingField playingField)
        {
            int[,] sudokuNumbers = new int[ColumnCount, RowCount];
            SudokuPlayingField potentialField = new SudokuPlayingField();

            int i = 0;
            foreach (var entry in this.Children)
            {
                int column = i % ColumnCount;
                int row = i / ColumnCount;

                if ((entry as TextBox).Text != "")
                {
                    if (!int.TryParse((entry as TextBox).Text, out int number) || !((0 < number) && (number <= 9)))
                    {
                        playingField = potentialField;
                        return false;
                    }
                    sudokuNumbers[column, row] = number;
                }
                i++;
            }

            for (int column = 0; column < ColumnCount; column++)
                for (int row = 0; row < RowCount; row++)
                {
                    int currentNumber = sudokuNumbers[column, row];
                    if (currentNumber != 0)
                        potentialField.SetFixNumber(column, row, currentNumber);
                }

            playingField = potentialField;
            return true;
        }

        /// <summary>
        /// Sets the numbers of the SudokuPlayingField into the SudokuGrid.
        /// </summary>
        /// <param name="playingField"></param>
        public void SetSudokuField(SudokuPlayingField playingField)
        {
            //Converting 2D SudokuPlayingField into 1D array of SudokuCell
            int i = 0;
            SudokuCell[] fieldArray = new SudokuCell[ColumnCount * RowCount];
            for (int row = 0; row < RowCount; row++)
                for (int column = 0; column < ColumnCount; column++)
                {
                    fieldArray[i] = playingField.PlayingField[column, row];
                    i++;
                }

            //Writing the 1D array to the textboxes.
            i = 0;
            foreach (var entry in this.Children)
            {
                if (fieldArray[i].Number != 0)
                {
                    ((entry as TextBox).Text) = string.Format($"{fieldArray[i].Number}");
                    if (fieldArray[i].IsFixNumber)
                    {
                        (entry as TextBox).Foreground = Brushes.DarkCyan;
                        (entry as TextBox).FontWeight = FontWeights.Bold;
                    }
                }
                i++;
            }
        }

        public void Clear()
        {
            foreach (TextBox entry in this.Children)
            {
                entry.Text = "";
            }
        }
    }
}
