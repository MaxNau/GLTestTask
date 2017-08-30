using System.Collections.Generic;

namespace WordSplitter
{
    public interface IWordSplitter
    {
        /// <summary>
        /// Reads file from path synchronously
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns> Returns dictionary where word is a key with they row index locations of the word </returns>
        Dictionary<string, List<int>> ReadFile(string filePath);

        /// <summary>
        /// Writes to file in the path synchronously
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        void Write(string filePath, Dictionary<string, List<int>> words);
    }
}
