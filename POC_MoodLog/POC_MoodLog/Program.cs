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
        private static string outprob2;
        static ArrayList nrc_emotion = new ArrayList();
        private static string outProb2;

        static void Main(string[] args)
        {
            init();

            String input = POC_MoodLog.Properties.Resources.SampleIn;
            ArrayList ngram = new ArrayList();
                        
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
                foreach (String item in temp2)
                {
                    String[] temp3 = item.Split();
                    foreach (String item2 in temp3)
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
            float[] joy = new float[2];
            float[] sad = new float[2];
            float[] anger = new float[2];
            float[] surprise = new float[2];
            float[] disgust = new float[2];
            float[] fear = new float[2];

            //Enter Freq count here
            joy[0] = 1;
            joy[1] = 1;
            sad[0] = 2;
            sad[1] = 2;

            //Population of other important variables
            float allYes = joy[0] + sad[0] + anger[0] + surprise[0] + disgust[0] + fear[0];
            float allNo = joy[1] + sad[1] + anger[1] + surprise[1] + disgust[1] + fear[1];
            float totalFreq = allYes + allNo;
            Array[] outProb=posteriorProbability(joy, sad, anger, surprise, disgust, fear, allYes,allNo,totalFreq);

            // Display purpose, final arrays finalBoW, emoticonCollection, hashtagCollection
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
            Console.WriteLine(outHash+"\n");

            foreach (float[] item in outProb)
            {
                outProb2 += item[0] + "," + item[1] + "\n";
            }
            Console.WriteLine(outProb2);

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

        public static Array[] posteriorProbability(float[] joy , float[] sad , float[] anger , float[] surprise , float[] disgust, float[] fear , float allYes, float allNo, float totalFreq) 
        {
            int count=0;
            Array[] sixEmotions = {joy,sad,anger,surprise,disgust,fear};
       
            foreach(float[] temp in sixEmotions)
            {
                temp[0] = ((temp[0] / allYes) * (allYes / totalFreq)) / ((temp[0] + temp[1]) / totalFreq);
                temp[1] = 1 - temp[0];
                sixEmotions[count] = temp;
                count++;
            }
            return sixEmotions;
        }

        public static void init()
        {
            bowreference.AddRange(POC_MoodLog.Properties.Resources.bow_algorithm_reference.Split());
            emoticons.AddRange(POC_MoodLog.Properties.Resources.emoticons.Split());
            nrc_emotion.AddRange(POC_MoodLog.Properties.Resources.NRC_emotion.Split());
        }
    }
}
