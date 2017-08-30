using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordSplitter
{
    public interface IWordSplitter
    {
        /// <summary>
        /// Reads file from path asynchronously
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns> Returns list of words with they row index location </returns>
        List<Word> ReadFileAsync(string filePath);

        /// <summary>
        /// Writes to file in the path asynchronously
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        Task WriteAsync(string filePath, List<Word> words);
    }
}
