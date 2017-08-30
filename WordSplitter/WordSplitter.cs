using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordSplitter
{
    public class WordSplitter : IWordSplitter
    {
        public List<Word> ReadFileAsync(string filePath)
        {
            List<Word> words = new List<Word>();
            int lineCount = 1;

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string nextLine;
                    while ((nextLine = sr.ReadLine()) != null)
                    {
                        AddWordFromLineToWordList(SplitLineIntoWords(nextLine), words, lineCount);
                        lineCount++;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return words;
        }

        /// <summary>
        /// Adds words and its row index occurrences into the word list
        /// </summary>
        /// <param name="wordsLine"></param>
        /// <param name="words"></param>
        /// <param name="lineIndex"></param>
        private void AddWordFromLineToWordList(List<string> wordsLine, List<Word> words, int lineIndex)
        {
            foreach (string word in wordsLine)
            {
                if (!words.Exists(w => w.Text == word))
                {
                    Word wor = new Word()
                    {
                        Text = word
                    };
                    wor.LineNumbers.Add(lineIndex);
                    words.Add(wor);
                }
                else
                {
                    words.SingleOrDefault(w => w.Text == word).LineNumbers.Add(lineIndex);
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

            var pattern = new Regex(
                @"( [A-Za-z0-9]
                ([A-Za-z0-9])*
                [A-Za-z0-9])",
                RegexOptions.IgnorePatternWhitespace);
            // # starting with a letter # followed by #   more letters or # and finishing with a letter

            foreach (Match m in pattern.Matches(lineText))
            {
                lineWords.Add(m.Groups[1].Value.ToLower());
            }

            return lineWords;
        }

        public async Task WriteAsync(string path, List<Word> words)
        {
            DeleteFile(path);
            CreateFile(path);

            foreach (Word word in words)
            {
                await WriteWordsAndLineNumbersAsync(path, word);
            }
        }

        /// <summary>
        /// Writes words and its row index occurrences in the input file to the output file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private async Task WriteWordsAndLineNumbersAsync(string path, Word word)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Append,
                    FileAccess.Write, FileShare.Write, 4096, useAsync: true))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        await sw.WriteAsync(word.Text + " ");

                        int size = word.LineNumbers.Count;
                        for (int i = 0; i < size; i++)
                        {
                            if (i < size - 1)
                            {
                                await sw.WriteAsync(word.LineNumbers[i] + ", ");
                            }
                            else
                            {
                                await sw.WriteAsync(word.LineNumbers[i].ToString());
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
