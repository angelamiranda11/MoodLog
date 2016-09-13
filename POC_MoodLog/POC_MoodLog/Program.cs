using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_MoodLog
{
    class Program
    {
        static ArrayList bowreference = new ArrayList();
        static ArrayList emoticons = new ArrayList();
        static char[] punctuations= { '.', '!', '?', ';' };
        static ArrayList prepNInter = new ArrayList();
        static ArrayList sentences = new ArrayList();
        static ArrayList ngramCollection = new ArrayList();
        static ArrayList emoticonCollection = new ArrayList();
        static ArrayList hashtagCollection = new ArrayList();
        static ArrayList finalBoW = new ArrayList();
        private static string outStr;
        private static string outEmo;
        private static string outHash;

        static void Main(string[] args)
        {
            String input = POC_MoodLog.Properties.Resources.SampleIn;
            ArrayList ngram = new ArrayList();

            bowreference.AddRange(POC_MoodLog.Properties.Resources.bow_algorithm_reference.Split());
            emoticons.AddRange(POC_MoodLog.Properties.Resources.emoticons.Split());
            
            var inSplit = input.Split();
            foreach (String item in inSplit)
            {
                if (item.IndexOf('#')==0)
                {
                    hashtagCollection.Add(item);
                    input = input.Remove(input.IndexOf(item[0]), item.Length);
                }
                else if (emoticons.Contains(item))
                {
                    emoticonCollection.Add(item);
                    input = input.Remove(input.IndexOf(item[0]), item.Length);
                }
            }
            var gcc2 = input.Split(punctuations);
            foreach (var item2 in gcc2)
            {
                if (item2.Trim() != "")
                {
                    sentences.Add(item2);
                }
            }


            foreach (String item in sentences)
            {
                String bigram = String.Join(",", makeBigrams(item));
                ngramCollection.Add(bigram);
            }

            for (int j = 0; j < ngramCollection.Count; j++)
            {
                String temp = Convert.ToString(ngramCollection[j]);
                String[] temp2 = temp.Split(',');
                foreach(String item in temp2)
                {
                    String[] temp3 = item.Split();
                    foreach(String item2 in temp3)
                    {
                        //if item2 is in the BoW ref or has interjection/preposition + BoW Approved 
                        if (bowreference.Contains(item2) || prepNInter.Contains(item2))
                        {
                            if ((bowreference.Contains(item2) && Array.IndexOf(temp3, item2) == 1) ||
                                (prepNInter.Contains(item2) && Array.IndexOf(temp3, item2) == 0))
                            {
                                if (bowreference.Contains(item2) && Array.IndexOf(temp3, item2) == 1)
                                {
                                    finalBoW.Add(item);
                                }
                                else if (prepNInter.Contains(item2) && Array.IndexOf(temp3, item2) == 0 && bowreference.Contains(temp3[1]))
                                {
                                    finalBoW.Add(item);
                                }
                            }
                        }
                    }
                    
                }

            }

            Console.Write("\nStrings remaining: ");
            foreach (var item in finalBoW)
            {
                outStr += item + " ";
            }
            Console.WriteLine(outStr);

            Console.Write("\nEmoticons: ");
            foreach (var item in emoticonCollection)
            {
               outEmo += item+" ";
            }
            Console.WriteLine(outEmo);

            Console.Write("\nHashtags: ");
            foreach (var item in hashtagCollection)
            {
                outHash += item + " ";
            }
            Console.WriteLine(outHash);
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        public static IEnumerable<string> makeBigrams(string text)
        {
            int nGramSize = 2;
            StringBuilder nGram = new StringBuilder();
            Queue<int> wordLengths = new Queue<int>();

            int wordCount = 0;
            int lastWordLen = 0;

            if (text != "" && char.IsLetterOrDigit(text[0]))
            {
                nGram.Append(text[0]);
                lastWordLen++;
            }

            //generate ngrams
            for (int i = 1; i < text.Length - 1; i++)
            {
                char before = text[i - 1];
                char after = text[i + 1];

                if (char.IsLetterOrDigit(text[i])
                        ||
                        (text[i] != ' '
                        && (char.IsSeparator(text[i]) || char.IsPunctuation(text[i]))
                        && (char.IsLetterOrDigit(before) && char.IsLetterOrDigit(after))
                        )
                    )
                {
                    nGram.Append(text[i]);
                    lastWordLen++;
                }
                else
                {
                    if (lastWordLen > 0)
                    {
                        wordLengths.Enqueue(lastWordLen);
                        lastWordLen = 0;
                        wordCount++;

                        if (wordCount >= nGramSize)
                        {
                            yield return nGram.ToString();
                            nGram.Remove(0, wordLengths.Dequeue() + 1);
                            wordCount -= 1;
                        }

                        nGram.Append(" ");
                    }
                }
            }
            nGram.Append(text.Last());
            yield return nGram.ToString();
        }
    }
}
