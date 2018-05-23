using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataPreprocessing.Data.Semantic
{
    /// <summary>
    /// Matrix of semantic data, is used to store number of occurences of pair (l, r)
    /// in the base text
    /// </summary>
    public class SemanticMatrix : ICollection<string[]>
    {
        /// <summary>
        /// Matrix of fuzzy weights
        /// </summary>
        private double[,] Matrix { get; set; }
        /// <summary>
        /// Map from distance to specific matrix
        /// </summary>
        private Dictionary<int, double[,]> Matrices { get; set; }
        /// <summary>
        /// Dictionary
        /// </summary>
        public string[] Words { get; private set; }
        /// <summary>
        /// Maximal distance between words
        /// </summary>
        private int MaxDistance { get; set; }

        /// <summary>
        /// Returns word from dictionary by its index
        /// </summary>
        /// <param name="index">Index of the word in dictionary</param>
        /// <returns></returns>
        public string this[int index]
        {
            get { return Words[index]; }
        }

        public double this[int row, int column]
        {
            get { return Matrices[1][row, column]; }
        }
        /// <summary>
        /// Length of data source
        /// </summary>
        public int Length
        {
            get { return Matrix.GetLength(0); }
        }
        /// <summary>
        /// Number of items in the matrix
        /// </summary>
        public int Count
        {
            get { return Length; }
        }
        /// <summary>
        /// Is this collection readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SemanticMatrix"/>
        /// </summary>
        /// <param name="splittedText">Set of words in the base text</param>
        /// <param name="dictionary">Dictionary that is used in the base text</param>
        public SemanticMatrix(string[] splittedText, string[] dictionary, int maxDistance)
        {
            Matrices = new Dictionary<int, double[,]>();
            MaxDistance = maxDistance;
            for (int distance = 1; distance <= maxDistance; distance++)
            {
                Matrix = new double[dictionary.Length, dictionary.Length];
                for (int wordIndex = 0; wordIndex < splittedText.Length - 1; wordIndex++)
                {
                    int i = Array.IndexOf(dictionary, splittedText[wordIndex]);
                    int successorIndex = wordIndex + distance >= splittedText.Length ?
                        wordIndex + distance - splittedText.Length :
                        wordIndex + distance;
                    int j = Array.IndexOf(dictionary, splittedText[successorIndex]);
                    Matrix[i, j]++;
                }
                Matrices.Add(distance, Matrix);
            }
            Words = dictionary;
        }

        /// <summary>
        /// Converts vector to a fuzzy format
        /// </summary>
        /// <param name="vector">Vector with occurence data</param>
        private double[] Fuzzyficate(double[] vector)
        {
            double divisor = vector.Sum();
            for (int i = 0; i < vector.GetLength(0); i++)
            {
                if (divisor != 0)
                {
                    vector[i] /= divisor;
                }
            }

            return vector;
        }

        /// <summary>
        /// Returns a fuzzy vector from the semantic matrix
        /// </summary>
        /// <param name="rowIndex">Index of the row, should be null if <paramref name="colIndex"/> is not null</param>
        /// <param name="colIndex">Index of the column, should be null if <paramref name="rowIndex"/> is not null</param>
        /// <returns></returns>
        public double[] GetVector(int key, int? rowIndex = null, int? colIndex = null)
        {
            if (key < 1 || key > MaxDistance)
            {
                throw new ArgumentOutOfRangeException(nameof(key), "Map does not contain this key");
            }

            if (rowIndex.HasValue == colIndex.HasValue)
            {
                throw new ArgumentException("Arguments can't be both null or non-null");
            }

            int index = 0;
            int size = Matrix.GetLength(0);
            List<double> vector = new List<double>(size);

            Func<int> firstIndex;
            Func<int> secondIndex;
            if (rowIndex.HasValue)
            {
                firstIndex = () => { return rowIndex.Value; };
                secondIndex = () => { return index++; };
            }
            else
            {
                firstIndex = () => { return index++; };
                secondIndex = () => { return colIndex.Value; };
            }

            for (int i = 0; i < size; i++)
            {
                vector.Add(Matrices[key][firstIndex(), secondIndex()]);
            }

            return Fuzzyficate(vector.ToArray());
        }

        public void Add(string[] item)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(string[] item)
        {
            throw new InvalidOperationException();
        }

        public void CopyTo(string[][] array, int arrayIndex)
        {
            throw new InvalidOperationException();
        }

        public bool Remove(string[] item)
        {
            throw new InvalidOperationException();
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                List<string> fuzzyContext = new List<string>(Length * 2 + 2)
                {
                    Words[i]
                };
                for (int j = 1; j <= MaxDistance; j++)
                {
                    fuzzyContext.AddRange(GetVector(j, rowIndex: i).Select(item => item.ToString("0.000")));
                    fuzzyContext.Add("|");
                    fuzzyContext.AddRange(GetVector(j, colIndex: i).Select(item => item.ToString("0.000")));
                    fuzzyContext.Add(";");
                }

                yield return fuzzyContext.ToArray();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder matrix = new StringBuilder();
            for (int i = 0; i < Length; i++)
            {
                matrix.AppendLine($"{Words[i]};{string.Join(";", GetVector(i, null).Select(item => item.ToString("0.000")))}");
            }

            return matrix.ToString();
        }
    }
}
