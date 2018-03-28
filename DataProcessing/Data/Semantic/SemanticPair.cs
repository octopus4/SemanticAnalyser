using System;

namespace DataProcessing.Data.Semantic
{
    /// <summary>
    /// Pair of word id in the dictionary and frequence
    /// </summary>
    public class SemanticPair : IComparable<SemanticPair>
    {
        /// <summary>
        /// Number of the word in the dictionary
        /// </summary>
        public int WordId { get; set; }
        /// <summary>
        /// Probability to meet this word in a context of the specified word
        /// </summary>
        public double Frequence { get; set; }

        /// <summary>
        /// The way to compare two semantic pairs
        /// </summary>
        public static Comparison<SemanticPair> Comparison
        {
            get { return (left, right) => { return right.Frequence.CompareTo(left.Frequence); }; }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SemanticPair"/>
        /// </summary>
        /// <param name="wordId">Id (e.g. number) of the word in the dictionary</param>
        /// <param name="frequence">Probability to meet this word in a context of the specified word</param>
        public SemanticPair(int wordId, double frequence)
        {
            WordId = wordId;
            Frequence = frequence;
        }

        /// <summary>
        /// Returns a text representation of this instance of <see cref="SemanticPair"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"<{WordId};{Frequence}>";
        }

        /// <summary>
        /// Returns -1, if <see langword="this"/> <see cref="SemanticPair"/> is bigger than <paramref name="other"/>,
        /// 0 if they are equal and 1, if the <paramref name="other"/> is bigger than <see langword="this"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(SemanticPair other)
        {
            return other.Frequence.CompareTo(Frequence);
        }
    }
}
