﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sudoku
{
    public class SudokuSolver
    {
        readonly SudokuPlayingField PlayingField;

        public SudokuSolver(SudokuPlayingField playingField)
        {
            PlayingField = playingField;
        }

        /// <summary>
        /// By using recursion, this method produces several instances of the Field and tries solving.
        /// </summary>
        /// <param name="currentField"></param>
        public void BruteForce(SudokuPlayingField currentField)
        {
            if (PlayingField.IsSolved() || PlayingField.GivenUp) return;

            if (!currentField.IsSolved())
            {
                PlayingField.InstanceCount++;
                if (PlayingField.InstanceCount == 400)
                    PlayingField.GivenUp = true;

                int counter = 0;
                do
                {
                    currentField.TryPotential(counter);
                    if (currentField.IsSolved())
                    {
                        currentField.CopyPlayingFieldTo(PlayingField);
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
                BruteForce(PlayingField);
                isFieldSolved = PlayingField.IsSolved();
                PlayingField.GivenUp = false;
                PlayingField.InstanceCount = 0;
                counter++;
            }
            while (counter < 10 && !isFieldSolved);
            return PlayingField.IsSolved();
        }
    }
}