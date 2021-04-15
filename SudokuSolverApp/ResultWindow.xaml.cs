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

namespace SudokuSolverApp
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        const int ColumnCount = 9;
        const int RowCount = 9;

        public ResultWindow()
        {
            InitializeComponent();
        }

        public void InitializeWindow()
        {
            ColumnDefinition[] columnsArray = new ColumnDefinition[ColumnCount];
            for (int i = 0; i < columnsArray.Length; ++i)
            {
                columnsArray[i] = new ColumnDefinition();
                resultGrid.ColumnDefinitions.Add(columnsArray[i]);
            }

            RowDefinition[] rowsArray = new RowDefinition[RowCount];
            for (int i = 0; i < rowsArray.Length; ++i)
            {
                rowsArray[i] = new RowDefinition();
                resultGrid.RowDefinitions.Add(rowsArray[i]);
            }
            for (int row = 0; row < RowCount; row++)
                for (int column = 0; column < ColumnCount; column++)
                {
                    TextBox textbox = new TextBox();
                    textbox.HorizontalContentAlignment = HorizontalAlignment.Center;
                    textbox.VerticalContentAlignment = VerticalAlignment.Center;
                    textbox.FontSize = 15;
                    textbox.IsReadOnly = true;
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
                    textbox.Name = string.Format($"l{column}{row}");
                    Grid.SetColumn(textbox, column);
                    Grid.SetRow(textbox, row);
                    resultGrid.Children.Add(textbox);
                }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeWindow();

        }
    }
}
