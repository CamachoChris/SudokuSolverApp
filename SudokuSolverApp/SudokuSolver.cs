using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sudoku
{
    public class SudokuSolver
    {
        private readonly SudokuPlayingField _playingField;
        private bool _givenUp;
        private int _instanceCount;

        public SudokuSolver(SudokuPlayingField playingField)
        {
            _playingField = playingField;
            _givenUp = false;
            _instanceCount = 0;
        }

        /// <summary>
        /// By using recursion, this method produces instances of the Field and tries solving.
        /// </summary>
        /// <param name="currentField"></param>
        private void BruteForce(SudokuPlayingField currentField)
        {
            if (_playingField.IsSolved() || _givenUp) return;

            if (!currentField.IsSolved())
            {
                _instanceCount++;
                if (_instanceCount == 300)
                    _givenUp = true;

                int counter = 0;
                do
                {
                    currentField.TryPotential(counter);
                    if (currentField.IsSolved())
                    {
                        currentField.CopyPlayingFieldTo(_playingField);
                        return;
                    }
                    else
                    {
                        SudokuPlayingField nextPlayingField = new SudokuPlayingField();
                        currentField.CopyPlayingFieldTo(nextPlayingField);
                        BruteForce(nextPlayingField);
                        counter++;
                    }
                } while (counter < 9);
            }
        }

        public bool Solve()
        {
            int counter = 0;
            bool isFieldSolved;
            do
            {
                BruteForce(_playingField);
                isFieldSolved = _playingField.IsSolved();
                _givenUp = false;
                _instanceCount = 0;
                counter++;
            }
            while (counter < 10 && !isFieldSolved);
            return _playingField.IsSolved();
        }
    }
}
