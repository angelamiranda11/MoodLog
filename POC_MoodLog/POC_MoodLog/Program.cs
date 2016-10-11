using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace POC_MoodLog
{
    class Program
    {
        static ArrayList bowreference = new ArrayList();
        static ArrayList emoticons = new ArrayList();
        static char[] punctuations= { '.', '!', '?', ';'};
        static ArrayList prepNInter = new ArrayList();
        static ArrayList sentences = new ArrayList();
        static ArrayList ngramCollection = new ArrayList();
        static ArrayList emoticonCollection = new ArrayList();
        static ArrayList hashtagCollection = new ArrayList();
        static ArrayList finalBoW = new ArrayList();
        static int[] emotionInt = { 0, 0, 0, 0, 0, 0 };
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
                    String segmented = doSegment(item);
                    char[] arr = segmented.ToCharArray();

                    arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                                      || char.IsWhiteSpace(c))));
                    segmented = new string(arr);
                    hashtagCollection.Add(segmented);
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

            ArrayList tempa = new ArrayList();
            ArrayList Q1 = new ArrayList();
            ArrayList Q2 = new ArrayList();
            ArrayList Q3 = new ArrayList();
            ArrayList Q4 = new ArrayList();
            tempa.AddRange(POC_MoodLog.Properties.Resources.anewReference.Split('\n'));
            String outQ1 = "";
            String outQ2 = "";
            String outQ3 = "";
            String outQ4 = "";
            foreach (string temp2 in tempa)
            {
                try
                {
                    double x = Convert.ToDouble(temp2.Split(',')[1]);
                    double y = Convert.ToDouble(temp2.Split(',')[2]);
                    if (x < 5 && x >= 0)
                    {
                        if (y < 5 && y >= 0)
                        {
                            Q3.Add(temp2.Split(',')[0]);
                        }
                        else if (y > 5 && y <= 10)
                        {
                            Q2.Add(temp2.Split(',')[0]);
                        }
                    }
                    else if (x > 5 && x <= 10 || x==5)
                    {
                        if (y < 5 && y >= 0 || x == 5)
                        {
                            Q4.Add(temp2.Split(',')[0]);
                        }
                        else if (y > 5 && y <= 10)
                        {
                            Q1.Add(temp2.Split(',')[0]);
                        }
                    }
                }
                catch(IndexOutOfRangeException ie)
                {
                    continue;
                }
                
            }

            Console.Write("\nStrings in Q1 for Happy & Surprise ("+Q1.Count+"): ");
            foreach (var item in Q1)
            {
                outQ1 += item + " ";
            }
            Console.WriteLine(outQ1);

            Console.Write("\nStrings in Q2 for Anger, Fear & Disgust (" + Q2.Count + "): ");
            foreach (var item in Q2)
            {
                outQ2 += item + " ";
            }
            Console.WriteLine(outQ2);

            Console.Write("\nStrings in Q3 for Sadness (" + Q3.Count + "): ");
            foreach (var item in Q3)
            {
                outQ3 += item + " ";
            }
            Console.WriteLine(outQ3);

            Console.Write("\nStrings in Q4 for Neutral (" + Q4.Count + "): ");
            foreach (var item in Q4)
            {
                outQ4 += item + " ";
            }
            Console.WriteLine(outQ4);

            /*Console.Write("\nStrings remaining: ");
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
            */

            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        private static String doSegment(String item)
        {
            try
            {
                var engine = Python.CreateEngine();
                ScriptSource source = engine.CreateScriptSourceFromString(POC_MoodLog.Properties.Resources.wordseg);
                CompiledCode compiledCode = source.Compile();
                ScriptScope scope = engine.CreateScope();
                scope.SetVariable("x", item);
                String result = compiledCode.Execute<String>(scope);
                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

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
