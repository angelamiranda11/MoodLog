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
        static ArrayList nrc_emotion = new ArrayList();
        private static string outProb2;
        static ArrayList memWord = new ArrayList();
        static ArrayList wordCommaEmotion = new ArrayList();
        static ArrayList prepEmotion = new ArrayList();
        static ArrayList hashSegmented = new ArrayList();
        static float[] joy = new float[2];
        static float[] sad = new float[2];
        static float[] anger = new float[2];
        static float[] surprise = new float[2];
        static float[] disgust = new float[2];
        static float[] fear = new float[2];
        static ArrayList tempa = new ArrayList();
        static ArrayList prep = new ArrayList();
        static String input = POC_MoodLog.Properties.Resources.SampleIn;
        static ArrayList ngram = new ArrayList();
        static void Main(string[] args)
        {
            init();
            var inSplit = input.Split();
            foreach (String item in inSplit)
            {
                if (item.IndexOf('#')==0 && item!="")
                {
                    String segmented = doSegment(item);
                    char[] arr = segmented.ToCharArray();

                    arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                                      || char.IsWhiteSpace(c))));
                    segmented = new string(arr);
                    hashtagCollection.Add(segmented);
                    input = input.Remove(input.IndexOf(item[0]), item.Length);
                }
                else if (emoticons.Contains(item) && item!="")
                {
                    emoticonCollection.Add(item);
                    input = input.Remove(input.IndexOf(item[0]), item.Length);
                }
            }

            foreach(String seg in hashtagCollection)
            {
                String bigram = String.Join(",", makeBigrams(seg));
                ngramCollection.Add(bigram);
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
                        if (bowreference.Contains(item2.ToLower()) || prepNInter.Contains(item2.ToLower()))
                        {
                            if ((bowreference.Contains(item2) && Array.IndexOf(temp3, item2) == 1) ||
                                (prepNInter.Contains(item2) && Array.IndexOf(temp3, item2) == 0) || bowreference.Contains(item2))
                            {
                                if ((bowreference.Contains(item2) && Array.IndexOf(temp3, item2) == 1)||bowreference.Contains(item2))
                                {
                                    if (!finalBoW.Contains(item))
                                    {
                                        memWord.Add(item2.ToLower());
                                        Debug.WriteLine("Adding " + item2);
                                        finalBoW.Add(item.ToLower());
                                    }  
                                }
                                else if (prepNInter.Contains(item2) && Array.IndexOf(temp3, item2) == 0 && bowreference.Contains(temp3[1]))
                                {
                                    if (!finalBoW.Contains(item))
                                    {
                                        memWord.Add(temp3[1].ToLower());
                                        Debug.WriteLine("Adding " + temp3[1]);
                                        finalBoW.Add(item.ToLower());
                                    }
                                }
                            }
                        }
                    }

                }
            }
            foreach (String words in memWord)
            {
                foreach (string temp2 in tempa)
                {
                    if (temp2 == "") break;
                    Double x = Convert.ToDouble(temp2.Split(',')[1]);
                    Double y = Convert.ToDouble(temp2.Split(',')[2]);
                    String word = temp2.Split(',')[0];
                    if(word == words)
                    {
                        if (x < 5 && x >= 0)
                        {
                            if (y < 5 && y >= 0)
                            {
                                if ((Math.Atan2(y, x) * (180 / Math.PI) + 180) >= 180 && (Math.Atan2(y, x) * (180 / Math.PI) + 180) <= 270)
                                {
                                    wordCommaEmotion.Add(word + "," + "sad");
                                    sad[0] += 1;
                                }
                            }
                            else if (y > 5 && y <= 10)
                            {
                                if ((Math.Atan2(y, x) * (180 / Math.PI) + 90) >= 90 && (Math.Atan2(y, x) * (180 / Math.PI) + 90) <= 120)
                                {
                                    wordCommaEmotion.Add(word + "," + "disgust");
                                    disgust[0] += 1;
                                }
                                if ((Math.Atan2(y, x) * (180 / Math.PI) + 90) >= 120 && (Math.Atan2(y, x) * (180 / Math.PI) + 90) <= 150)
                                {
                                    wordCommaEmotion.Add(word + "," + "anger");
                                    anger[0] += 1;
                                }
                                if ((Math.Atan2(y, x) * (180 / Math.PI) + 90) >= 150 && (Math.Atan2(y, x) * (180 / Math.PI) + 90) <= 180)
                                {
                                    wordCommaEmotion.Add(word + "," + "fear");
                                    fear[0] += 1;
                                }
                            }
                        }
                        else if (x > 5 && x <= 10 || x == 5)
                        {
                            if (y < 5 && y >= 0 || x == 5)
                            {
                                wordCommaEmotion.Add(word + "," + "neutral");
                            }
                            else if (y > 5 && y <= 10)
                            {
                                if ((Math.Atan2(y, x) * (180 / Math.PI) + 0) >= 30 && (Math.Atan2(y, x) * (180 / Math.PI) + 0) <= 60)
                                {
                                    wordCommaEmotion.Add(word + "," + "joy");
                                    joy[0] += 1;
                                }
                                if ((Math.Atan2(y, x) * (180 / Math.PI) + 0) >= 60 && (Math.Atan2(y, x) * (180 / Math.PI) + 0) <= 90)
                                {
                                    wordCommaEmotion.Add(word + "," + "surprise");
                                    surprise[0] += 1;
                                }
                            }
                        }
                    }
                }
            }
            
            foreach (String temp in finalBoW)
            {
                String[] runtemp = POC_MoodLog.Properties.Resources.PrepRef.Split();
                String prepositionPlace = temp.Split()[0];
                float effectValue = 0;
                foreach (String i in runtemp)
                {
                    if (i != "")
                    {
                        String checker = i.Split(',')[0];
                        char effect = ' ';
                        if (prepositionPlace.Trim().ToLower() == checker.Trim())
                        {
                            effect = Convert.ToChar(i.Split(',')[1]);
                            effectValue = Convert.ToSingle(i.Split(',')[2]);
                            String emotion = getEmotion(temp.Split()[1], wordCommaEmotion);
                            switch (emotion)
                            {
                                case "joy":
                                    if (effect == '+')
                                    {
                                        joy[0] += effectValue;
                                    }
                                    else if (effect == '-')
                                    {
                                        joy[0] -= 1;
                                        joy[1] += effectValue;
                                    }
                                    break;
                                case "surprise":
                                    if (effect == '+')
                                    {
                                        surprise[0] += effectValue;
                                    }
                                    else if (effect == '-')
                                    {
                                        surprise[0] -= 1;
                                        surprise[1] += effectValue;
                                    }
                                    break;
                                case "fear":
                                    if (effect == '+')
                                    {
                                        fear[0] += effectValue;
                                    }
                                    else if (effect == '-')
                                    {
                                        fear[0] -= 1;
                                        fear[1] += effectValue;
                                    }
                                    break;
                                case "anger":
                                    if (effect == '+')
                                    {
                                        anger[0] += effectValue;
                                    }
                                    else if (effect == '-')
                                    {
                                        anger[0] -= 1;
                                        anger[1] += effectValue;
                                    }
                                    break;
                                case "disgust":
                                    if (effect == '+')
                                    {
                                        disgust[0] += effectValue;
                                    }
                                    else if (effect == '-')
                                    {
                                        disgust[0] -= 1;
                                        disgust[1] += effectValue;
                                    }
                                    break;
                                case "sad":
                                    if (effect == '+')
                                    {
                                        sad[0] += effectValue;
                                    }
                                    else if (effect == '-')
                                    {
                                        sad[0] -= 1;
                                        sad[1] += effectValue;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Joy: " + joy[0] + " " + joy[1]);
            Console.WriteLine("Sad: " + sad[0] + " " + sad[1]);
            Console.WriteLine("Anger: " + anger[0] + " " + anger[1]);
            Console.WriteLine("Surprise: " + surprise[0] + " " + surprise[1]);
            Console.WriteLine("Disgust: " + disgust[0] + " " + disgust[1]);
            Console.WriteLine("Fear: " + fear[0] + " " + fear[1]);

            //Population of other important variables
            float allYes = joy[0] + sad[0] + anger[0] + surprise[0] + disgust[0] + fear[0];
            float allNo = joy[1] + sad[1] + anger[1] + surprise[1] + disgust[1] + fear[1];
            float totalFreq = allYes + allNo;
            String[] sequence = {"Joy", "Sad" , "Anger" , "Surprise" , "Disgust" , "Fear" };
            Array[] outProb=posteriorProbability(joy, sad, anger, surprise, disgust, fear, allYes,allNo,totalFreq);
            Console.WriteLine("\nResults: ");
            int k = 0;
            foreach (float[] item in outProb)
            {
                if (float.IsNaN(item[0])){ item[0] = 0; }
                if (float.IsNaN(item[1])) { item[1] = 0; }
                outProb2 += sequence[k]+": "+(item[0])*100 + "%," + (item[1]) * 100 + "%\n";
                k++;
            }
            //outProb is the arraylist consisting of float arrays
            //it is the list of final probabilities in specific sequence
            // "Joy", "Sad" , "Anger" , "Surprise" , "Disgust" , "Fear" 

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

        public static String getEmotion(String intext, ArrayList ar)
        {
            String emotion = null;
            foreach(String entry in ar)
            {
                String[] entryAr = entry.Split(',');
                if (intext == entryAr[0])
                {
                    emotion = entryAr[1];
                }
            }
            return emotion;
        }

        public static void init()
        {
            bowreference.AddRange(POC_MoodLog.Properties.Resources.bow_algorithm_reference.Split());
            emoticons.AddRange(POC_MoodLog.Properties.Resources.emoticons.Split());
            nrc_emotion.AddRange(POC_MoodLog.Properties.Resources.NRC_emotion.Split());
            prepNInter.AddRange(POC_MoodLog.Properties.Resources.ngrams1.Split());
            tempa.AddRange(POC_MoodLog.Properties.Resources.NRC_emotion.Split('\n'));
            prep.AddRange(POC_MoodLog.Properties.Resources.PrepRef.Split('\n'));
            input = input.ToLower();
        }
    }
}