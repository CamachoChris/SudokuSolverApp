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
    public class SudokuCell
    {
        private int _number;
        private bool _isFixNumber;

        private readonly bool[] _potentialNumbers;
        private int _potentialsCount;

        public SudokuCell()
        {
            _potentialNumbers = new bool[9];
            for (int i = 0; i < 9; i++)
                _potentialNumbers[i] = true;

            _potentialsCount = 9;
            _number = 0;
            _isFixNumber = false;            
        }

        public void SetNumber(int value)
        {
            _number = value;
            RemoveAllPotentials();            
        }

        public int GetNumber()
        {
            return _number;
        }

        public void SetFixNumber(int value)
        {
            SetNumber(value);
            _isFixNumber = true;
        }

        public bool IsFixNumber()
        {
            return _isFixNumber;
        }

        public int GetPotentialsCount()
        {
            return _potentialsCount;
        }

        /// <summary>
        /// Counts the Potentials of a Cell.
        /// </summary>
        /// <returns>Returns the result.</returns>
        public int PotentialsCounter()
        {
            int counter = 0;
            for (int i = 0; i < 9; i++)
            {
                if (_potentialNumbers[i])
                    counter++;
            }
            return counter;
        }

        /// <summary>
        /// Check if requested value is a Potential. 
        /// </summary>
        /// <param name="value">Value has to be from 1 to 9.</param>
        /// <returns>Returns true, if potential, false if not.</returns>
        public bool IsPotential(int value)
        {
            return _potentialNumbers[value - 1];
        }

        /// <summary>
        /// Removes a Potential and refreshes the PotentialsCount. 
        /// </summary>
        /// <param name="value">Value has to be from 1 to 9.</param>
        public void RemovePotential(int value)
        {
            _potentialNumbers[value - 1] = false;
            _potentialsCount = PotentialsCounter();
        }

        /// <summary>
        /// Removes all Potentials and sets PotentialCount to 0. Needed, if a Number is set.
        /// </summary>
        public void RemoveAllPotentials()
        {
            for (int i = 0; i < 9; i++)
                _potentialNumbers[i] = false;
            _potentialsCount = 0;
        }

        /// <summary>
        /// Copy method for the Cell that works value by value.
        /// </summary>
        /// <param name="copyToThisCell"></param>
        public void CopyCellTo(SudokuCell copyToThisCell)
        {
            copyToThisCell._number = this._number;
            copyToThisCell._isFixNumber = this._isFixNumber;

            for (int i = 0; i < 9; i++)
                copyToThisCell._potentialNumbers[i] = this._potentialNumbers[i];
        }

        public void ShowPotentials()
        {
            for (int i = 0; i < 9; i++)
                if (_potentialNumbers[i])
                Debug.Write($"{i + 1} ");
            Debug.WriteLine("");
        }
    }
}
