using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace WordSplitter
{
    public class WordSplitter : IWordSplitter
    {
        private Regex pattern;

        public WordSplitter()
        {
            pattern = new Regex(@"[a-zA-Z0-9]{2,}",
             RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        }

        public Dictionary<string, List<int>> ReadFile(string filePath)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            //List<Word> words = new List<Word>();
            var words = new Dictionary<string, List<int>>();
            int lineCount = 1;

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        using (StreamReader sr = new StreamReader(bs))
                        {
                            string nextLine;
                            while ((nextLine = sr.ReadLine()) != null)
                            {
                                AddWordFromLineToWordList(SplitLineIntoWords(nextLine), words, lineCount);
                                lineCount++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);

            return words;
        }

        /// <summary>
        /// Adds words and its row index occurrences into the word list
        /// </summary>
        /// <param name="wordsLine"></param>
        /// <param name="words"></param>
        /// <param name="lineIndex"></param>
        private void AddWordFromLineToWordList(List<string> wordsLine, Dictionary<string, List<int>> words, int lineIndex)
        {
            int size = wordsLine.Count;
            for (int i = 0; i < size; i++)
            {
                if (words.TryGetValue(wordsLine[i], out List<int> ret))
                {
                    words[wordsLine[i]].Add(lineIndex);
                }
                else
                {
                    words[wordsLine[i]] = new List<int> { lineIndex };
                }
            }
        }

        /// <summary>
        /// Splits line into words
        /// </summary>
        /// <param name="lineText"></param>
        /// <returns> List of words in line </returns>
        public List<string> SplitLineIntoWords(string lineText)
        {
            List<string> lineWords = new List<string>();


            foreach (Match m in pattern.Matches(lineText))
            {
                lineWords.Add(m.Groups[0].Value.ToLower());
            }

            return lineWords;
        }

        public void Write(string path, Dictionary<string, List<int>> words)
        {
            DeleteFile(path);
            CreateFile(path);
            var sortedWords = new SortedDictionary<string, List<int>>(words);
            foreach (var word in sortedWords)
            {
                WriteWordsAndLineNumbersAsync(path, word);
            }
        }

        /// <summary>
        /// Writes words and its row index occurrences in the input file to the output file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private void WriteWordsAndLineNumbersAsync(string path, KeyValuePair<string, List<int>> word)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Append,
                    FileAccess.Write, FileShare.Write, 4096))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(word.Key + " ");

                        int size = word.Value.Count;
                        for (int i = 0; i < size; i++)
                        {
                            if (i < size - 1)
                            {
                                sw.Write(word.Value[i] + ", ");
                            }
                            else
                            {
                                sw.Write(word.Value[i].ToString());
                            }
                        }
                        sw.Write(Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Creates file
        /// </summary>
        /// <param name="path"></param>
        private void CreateFile(string path)
        {
            try
            {
                using (FileStream fs = File.Create(path))
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Removes file if it exists
        /// </summary>
        /// <param name="path"></param>
        private void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
