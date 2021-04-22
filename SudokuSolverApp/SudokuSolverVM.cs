using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class SudokuSolverVM
    {
        public event EventHandler SudokuSolved;

        private SudokuPlayingField playingField;
        public SudokuPlayingField PlayingField
        {
            get { return playingField; }
            set { playingField = value; }
        }

        public void Solve()
        {
            SudokuSolver solver = new SudokuSolver(playingField);
            solver.Solve();
            if (SudokuSolved != null)
                SudokuSolved(this, EventArgs.Empty);
        }
    }
}
