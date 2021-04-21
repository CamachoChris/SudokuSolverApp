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

namespace SudokuStandardPattern
{
    class SudokuPattern
    {
        const int ColumnCount = 9;
        const int RowCount = 9;

        public static void LoadFromSudokuField(Grid sudokuPattern, SudokuPlayingField playingField)
        {
            //Converting 2D SudokuPlayingField into 1D array of SudokuCell
            int i = 0;
            SudokuCell[] fieldArray = new SudokuCell[ColumnCount * RowCount];
            for (int row = 0; row < RowCount; row++)
                for (int column = 0; column < ColumnCount; column++)
                {
                    fieldArray[i] = playingField.playingField[column, row];
                    i++;
                }

            //Writing the 1D array to the textboxes.
            i = 0;
            foreach (var entry in sudokuPattern.Children)
            {
                if (fieldArray[i].GetNumber() != 0)
                {
                    ((entry as TextBox).Text) = string.Format($"{fieldArray[i].GetNumber()}");
                    if (fieldArray[i].IsFixNumber())
                    {
                        (entry as TextBox).Foreground = Brushes.DarkCyan;
                        (entry as TextBox).FontWeight = FontWeights.Bold;
                    }
                }
                i++;
            }
        }

        /// <summary>
        /// Loading the numbers of the Pattern into the SudokuPlayingField with MessageBox inquiry.
        /// </summary>
        /// <param name="sudokuPattern"></param>
        /// <param name="playingField"></param>
        /// <returns>Returns true, if all entries were valid. False, if not.</returns>
        public static bool LoadToSudokuField(Grid sudokuPattern, SudokuPlayingField playingField)
        {
            int[,] sudokuNumbers = new int[ColumnCount, RowCount];

            int i = 0;
            foreach (var entry in sudokuPattern.Children)
            {
                int column = i % ColumnCount;
                int row = i / ColumnCount;

                if ((entry as TextBox).Text != "")
                {
                    if (!int.TryParse((entry as TextBox).Text, out int number) || !((0 < number) && (number <= 9)))
                    {
                        MessageBox.Show("Only numbers from 1 to 9.");
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
                        playingField.SetFixNumber(column, row, currentNumber);
                }
            
            return true;
        }

        public static void ClearPattern(Grid sudokupattern)
        {
            foreach (TextBox entry in sudokupattern.Children)
            {
                entry.Text = "";
            }
        }

        /// <summary>
        /// Make the 9x9 grid and insert textboxes as child for sudokupattern
        /// </summary>
        /// <param name="sudokupattern"></param>
        public static void Init(Grid sudokupattern)
        {
            //Start with 9 columns and 9 rows.
            ColumnDefinition[] columnsArray = new ColumnDefinition[ColumnCount];
            for (int i = 0; i < columnsArray.Length; ++i)
            {
                columnsArray[i] = new ColumnDefinition();
                sudokupattern.ColumnDefinitions.Add(columnsArray[i]);
            }

            RowDefinition[] rowsArray = new RowDefinition[RowCount];
            for (int i = 0; i < rowsArray.Length; ++i)
            {
                rowsArray[i] = new RowDefinition();
                sudokupattern.RowDefinitions.Add(rowsArray[i]);
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
                    sudokupattern.Children.Add(textbox);
                }
        }
    }
}
