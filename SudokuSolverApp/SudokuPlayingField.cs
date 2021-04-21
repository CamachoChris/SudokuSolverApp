using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sudoku
{
    /*  TERMINOLOGY 
     *  -----------
     *  Field refers to the 9x9 playing field that consists of cells.
     *  Cell refers to the subelements of a Field, that consists of numbers and potentials.
     *  Numbers are int values that the player sees on the playing field.
     *  Potentials are int values that remember which numbers could be possible for a Cell.
     *  A Mandatory is the last Potential left for a Cell, so to speak the intermediate stage between Potential and Number.
     */
    public class SudokuPlayingField
    {
        public SudokuCell[,] playingField;
        private readonly Random _random;

        public SudokuPlayingField()
        {
            _random = new Random();
            playingField = new SudokuCell[9,9];
            for (int row = 0; row < 9; row++)
                for (int column = 0; column < 9; column++)
                    playingField[column, row] = new SudokuCell();
        }

        /// <summary>
        /// Sets the Number at a position and synchronizes the Field, so that it is always up to date.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="number"></param>
        public void SetFixNumber(int column, int row, int number)
        {
            playingField[column, row].SetFixNumber(number);
            SyncPotentials(column, row);
        }

        /// <summary>
        /// Sets the Number at a position and synchronizes the Field, so that it is always up to date.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="number"></param>
        public void SetNumber(int column, int row, int number)
        {
            playingField[column, row].SetNumber(number);
            SyncPotentials(column, row);
        }

        /// <summary>
        /// Searches the Field for unsolved Cells.
        /// </summary>
        /// <returns>Returns true, if no unsolved fields are left.</returns>
        public bool IsSolved()
        {
            for (int column = 0; column < 9; column++)
                for (int row = 0; row < 9; row++)
                {
                    if (playingField[column, row].GetNumber() == 0)
                        return false;
                }
            return true;
        }

        /// <summary>
        /// According to the Number at the position the Potentials become synchronized (column, row and square).
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>        
        private void SyncPotentials(int entryColumn, int entryRow)
        {
            //Only synchronize Cells with numbers, otherwise exception.
            if (playingField[entryColumn, entryRow].GetNumber() == 0)
                return;

            int startColumnForSquare = StartColumnForSquare(entryColumn);
            int startRowForSquare = StartRowForSquare(entryRow);

            for (int i = 0; i < 9; i++)
            {
                //Synchronize column
                if (i != entryRow)
                    playingField[entryColumn, i].RemovePotential(playingField[entryColumn, entryRow].GetNumber());

                //Synchronize row
                if (i != entryColumn)
                    playingField[i, entryRow].RemovePotential(playingField[entryColumn, entryRow].GetNumber());

                //Synchronize square
                int parseColumn = LineToSquareColumn(i, 3) + startColumnForSquare;
                int parseRow = LineToSquareRow(i, 3) + startRowForSquare;
                if (!(parseColumn == entryColumn && parseRow == entryRow))
                    playingField[parseColumn, parseRow].RemovePotential(playingField[entryColumn, entryRow].GetNumber());
            }
        }

        /// <summary>
        /// Converts the Potential (value) as Number in a Cell.
        /// </summary>
        /// <param name="value">Value has to be from 1 to 9.</param>
        public void SetPotentialAsNumber(int column, int row, int value)
        {
            if (playingField[column, row].IsPotential(value))
                SetNumber(column, row, value);
        }

        /// <summary>
        /// Set a random Potential as Number.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public void SetRandomPotentialAsNumber(int column, int row)
        {
            int rndPosition = _random.Next(10);
            for (int i = 1; i <= 9; i++)
            {
                if (playingField[column, row].IsPotential(i) && rndPosition == i)
                {
                    SetPotentialAsNumber(column, row, i);
                    return;
                }
            }
        }

        /// <summary>
        /// Finds the last remaining Potential (Mandatory) and makes it the official Number of the Cell.
        /// </summary>
        public void SetMandatoryAsNumber(int column, int row)
        {
            if (playingField[column, row].PotentialsCounter() == 1)
                for (int i = 1; i <= 9; i++)
                    if (playingField[column, row].IsPotential(i))
                    {
                        SetPotentialAsNumber(column, row, i);
                        return;
                    }
        }

        /// <summary>
        /// Counts the amount of Mandatories.
        /// </summary>
        /// <returns>Returns the amount Mandatories. 0 means no are found.</returns>
        public int GetMandatoriesCount()
        {
            int counter = 0;
            for (int row = 0; row < 9; row++)
                for (int column = 0; column < 9; column++)
                    if (playingField[column, row].PotentialsCounter() == 1)
                        counter++;
            return counter;
        }

        /// <summary>
        /// Converts all Mandatories on a Field to Numbers and synchronizes them.
        /// </summary>
        public void ConvertAllMandatoriesToNumbers()
        {
            for (int row = 0; row < 9; row++)
                for (int column = 0; column < 9; column++)
                    if (playingField[column, row].PotentialsCounter() == 1)
                    {
                        SetMandatoryAsNumber(column, row);
                    }
        }

        /// <summary>
        /// As long as there are Mandatories, they are converted and synchronized.
        /// </summary>
        public void ScoopMandatories()
        {
            while (GetMandatoriesCount() > 0)
            {
                ConvertAllMandatoriesToNumbers();
            }            
        }

        /// <summary>
        /// If no Mandatories are left, but Field is still unsolved, try Potentials with least count.
        /// </summary>
        /// <param name="position">If more with same PotentialCount, take position (first, second, third...). 0 is first position</param>
        public void TryPotential(int position)
        {
            int counter = 0;
            for (int i = 2; i <= 9; i++)
                for (int row = 0; row < 9; row++)
                    for (int column = 0; column < 9; column++)
                    {
                        int potentialCount = playingField[column, row].PotentialsCounter();
                        if (potentialCount == i)
                        {
                            if (position == counter)
                            {
                                SetRandomPotentialAsNumber(column, row);
                                ScoopMandatories();
                                return;
                            }
                            counter++;
                        }        
                    }
        }

        private static int StartColumnForSquare(int column)
        {
            return (column / 3) * 3;
        }

        private static int StartRowForSquare(int row)
        {
            return (row / 3) * 3;
        }

        private static int LineToSquareColumn(int value, int squaresize)
        {
            return (value % squaresize);
        }

        private static int LineToSquareRow(int value, int squaresize)
        {
            return (value / squaresize);
        }

        private SudokuCell GetCell(int column, int row)
        {
            return playingField[column, row];
        }

        /// <summary>
        /// Copy method for the Field that works value by value.
        /// </summary>
        /// <param name="copyToThisField"></param>
        public void CopyPlayingFieldTo(SudokuPlayingField copyToThisField)
        {
            for (int column = 0; column < 9; column++)
                for (int row = 0; row < 9; row++)
                {
                     this.GetCell(column, row).CopyCellTo(copyToThisField.playingField[column, row]);
                }            
        }

        public void Show(string title)
        {
            Debug.WriteLine(title);
            for (int row = 0; row < 9; row++) 
            {
                for (int entry = 0; entry < 9; entry++)
                {
                    Debug.Write($"{playingField[entry, row].GetNumber()} ");
                    if (entry % 3 == 2)
                        Debug.Write("| ");
                }
                Debug.WriteLine("");
                if (row % 3 == 2)
                    Debug.WriteLine("---------------------");
            }
        }

        public void ShowAllPotentials(string title)
        {
            Debug.WriteLine(title);
            for (int row = 0; row < 9; row++)
            {
                for (int entry = 0; entry < 9; entry++)
                {
                    Debug.Write($"{playingField[entry, row].PotentialsCounter()} ");
                    if (entry % 3 == 2)
                        Debug.Write("| ");
                }
                Debug.WriteLine("");
                if (row % 3 == 2)
                    Debug.WriteLine("---------------------");
            }
        }
    }
}
