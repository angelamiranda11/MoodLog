using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace wpf_moodlog
{
    public class Program
    {
        ArrayList bowreference = new ArrayList();
        ArrayList emoticons = new ArrayList();
        char[] punctuations = { '.', '!', '?', ';' };
        ArrayList prepNInter = new ArrayList();
        ArrayList sentences = new ArrayList();
        ArrayList ngramCollection = new ArrayList();
        ArrayList emoticonCollection = new ArrayList();
        ArrayList hashtagCollection = new ArrayList();
        ArrayList finalBoW = new ArrayList();
        int[] emotionInt = { 0, 0, 0, 0, 0, 0 };
        ArrayList nrc_emotion = new ArrayList();
        ArrayList memWord = new ArrayList();
        ArrayList wordCommaEmotion = new ArrayList();
        ArrayList prepEmotion = new ArrayList();
        ArrayList hashSegmented = new ArrayList();
        float[] joy = new float[2];
        float[] sad = new float[2];
        float[] anger = new float[2];
        float[] surprise = new float[2];
        float[] disgust = new float[2];
        float[] fear = new float[2];
        ArrayList tempa = new ArrayList();
        ArrayList prep = new ArrayList();
        ArrayList ngram = new ArrayList();

        public float[] processText(string text)
        {
            init();
            String input = text;
            input = input.ToLower();
            var inSplit = input.Split();
            foreach (String item in inSplit)
            {
                if (item.IndexOf('#') == 0 && item != "")
                {
                    String segmented = doSegment(item);
                    char[] arr = segmented.ToCharArray();

                    arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                                      || char.IsWhiteSpace(c))));
                    segmented = new string(arr);
                    hashtagCollection.Add(segmented);
                    input = input.Remove(input.IndexOf(item[0]), item.Length);
                }
                else if (emoticons.Contains(item) && item != "")
                {
                    emoticonCollection.Add(item);
                    input = input.Remove(input.IndexOf(item[0]), item.Length);
                }
            }

            foreach (String seg in hashtagCollection)
            {
                String[] hashtags = seg.Split();
                foreach(String segs in hashtags)
                {
                    //improvement: Considering prepositions in hashtag processing
                    Debug.WriteLine("Processing " + segs);
                    if (bowreference.Contains(segs))
                    {
                        Debug.WriteLine("Adding " + segs);
                        memWord.Add(segs.ToLower());
                        finalBoW.Add(segs.ToLower());
                    }
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
                        if (bowreference.Contains(item2.ToLower()) || prepNInter.Contains(item2.ToLower()))
                        {
                            if ((bowreference.Contains(item2) && Array.IndexOf(temp3, item2) == 1) ||
                                (prepNInter.Contains(item2) && Array.IndexOf(temp3, item2) == 0) || bowreference.Contains(item2))
                            {
                                if ((bowreference.Contains(item2) && Array.IndexOf(temp3, item2) == 1) || bowreference.Contains(item2))
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
                    double x = Convert.ToSingle(temp2.Split(',')[1]);
                    double y = Convert.ToSingle(temp2.Split(',')[2]);
                    String word = temp2.Split(',')[0];
                    if (word == words)
                    {
                        if (x < 5 && x >= 0)
                        {
                            if (y < 5 && y >= 0)
                            {
                                x = Math.Abs(x - 5);
                                y = Math.Abs(y - 5);
                                Debug.WriteLine("Entered Quadrant 3; Angle: " + (180+Math.Atan2(y, x) * (180 / Math.PI)));
                                if ((180 + Math.Atan2(y, x) * (180 / Math.PI)) >= 180 && (180 + Math.Atan2(y, x) * (180 / Math.PI)) < 270)
                                {
                                    wordCommaEmotion.Add(word + "," + "sad");
                                    sad[0] += 1;
                                }
                            }
                            else if (y > 5 && y <= 10)
                            {
                                x = x - 5;
                                y = y - 5;
                                Debug.WriteLine("Entered Quadrant 2; Angle: "+ (Math.Atan2(y, x) * (180 / Math.PI)));
                                if (Math.Abs((Math.Atan2(y, x) * (180 / Math.PI))) >= 90 && (Math.Abs(Math.Atan2(y, x) * (180 / Math.PI))) < 120)
                                {
                                    wordCommaEmotion.Add(word + "," + "disgust");
                                    disgust[0] += 1;
                                }
                                else if (Math.Abs((Math.Atan2(y, x) * (180 / Math.PI))) >= 120 && (Math.Abs(Math.Atan2(y, x) * (180 / Math.PI)))< 150)
                                {
                                    wordCommaEmotion.Add(word + "," + "anger");
                                    anger[0] += 1;
                                }
                                else if (Math.Abs((Math.Atan2(y, x) * (180 / Math.PI)))>= 150 && (Math.Abs(Math.Atan2(y, x) * (180 / Math.PI))) < 180)
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
                                x = Math.Abs(x - 5);
                                y = Math.Abs(y - 5);
                                Debug.WriteLine("Entered Quadrant 4; Angle: " + (360+(Math.Atan2(y, x) * (180 / Math.PI))));
                                wordCommaEmotion.Add(word + "," + "neutral");
                            }
                            else if (y > 5 && y <= 10)
                            {
                                x = x - 5;
                                y = y - 5;
                                Debug.WriteLine("Entered Quadrant 1; Angle: " + (Math.Atan2(y, x) * (180 / Math.PI)));
                                if ((Math.Atan2(y, x) * (180 / Math.PI)) >= 0 && (Math.Atan2(y, x) * (180 / Math.PI)) < 45)
                                {
                                    wordCommaEmotion.Add(word + "," + "joy");
                                    joy[0] += 1;
                                }
                                else if ((Math.Atan2(y, x) * (180 / Math.PI)) >= 45 && (Math.Atan2(y, x) * (180 / Math.PI)) < 90)
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
                String[] runtemp = wpf_moodlog.Properties.Resources.PrepRef.Split();
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

            //frequency of each word, yes is [0] and no is [1]
            Debug.WriteLine("Joy: " + joy[0] + " " + joy[1]);
            Debug.WriteLine("Sad: " + sad[0] + " " + sad[1]);
            Debug.WriteLine("Anger: " + anger[0] + " " + anger[1]);
            Debug.WriteLine("Surprise: " + surprise[0] + " " + surprise[1]);
            Debug.WriteLine("Disgust: " + disgust[0] + " " + disgust[1]);
            Debug.WriteLine("Fear: " + fear[0] + " " + fear[1]);

            //Population of other important variables
            float allYes = joy[0] + sad[0] + anger[0] + surprise[0] + disgust[0] + fear[0];
            float allNo = joy[1] + sad[1] + anger[1] + surprise[1] + disgust[1] + fear[1];
            float totalFreq = allYes + allNo;
            String[] sequence = { "Joy", "Sad", "Anger", "Surprise", "Disgust", "Fear" };
            Array[] outProb = posteriorProbability(joy, sad, anger, surprise, disgust, fear, allYes, allNo, totalFreq);
            int k = 0;
            float[] outYesProb = new float[6];
            foreach (float[] item in outProb)
            {
                if (float.IsNaN(item[0])) { item[0] = 0; }
                if (float.IsNaN(item[1])) { item[1] = 0; }
                outYesProb[k] = item[0];
                k++;
            }
            return outYesProb;
        }

        private String doSegment(String item)
        {
            try
            {
                var engine = Python.CreateEngine();
                ScriptSource source = engine.CreateScriptSourceFromString(wpf_moodlog.Properties.Resources.wordseg);
                CompiledCode compiledCode = source.Compile();
                ScriptScope scope = engine.CreateScope();
                scope.SetVariable("x", item);
                String result = compiledCode.Execute<String>(scope);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }

        }


        public IEnumerable<string> makeBigrams(string text)
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

        public Array[] posteriorProbability(float[] joy, float[] sad, float[] anger, float[] surprise, float[] disgust, float[] fear, float allYes, float allNo, float totalFreq)
        {
            int count = 0;
            Array[] sixEmotions = { joy, sad, anger, surprise, disgust, fear };

            foreach (float[] temp in sixEmotions)
            {
                temp[0] = ((temp[0] / allYes) * (allYes / totalFreq)) / ((temp[0] + temp[1]) / totalFreq);
                temp[1] = 1 - temp[0];
                sixEmotions[count] = temp;
                count++;
            }
            return sixEmotions;
        }

        public String getEmotion(String intext, ArrayList ar)
        {
            String emotion = null;
            foreach (String entry in ar)
            {
                String[] entryAr = entry.Split(',');
                if (intext == entryAr[0])
                {
                    emotion = entryAr[1];
                }
            }
            return emotion;
        }

        public void init()
        {
            bowreference.AddRange(wpf_moodlog.Properties.Resources.bow_algorithm_reference.Split());
            emoticons.AddRange(wpf_moodlog.Properties.Resources.emoticons.Split());
            nrc_emotion.AddRange(wpf_moodlog.Properties.Resources.NRC_emotion_lexicon_wordlevel_alphabetized_v0_92__1_.Split());
            prepNInter.AddRange(wpf_moodlog.Properties.Resources.ngrams1.Split());
            tempa.AddRange(wpf_moodlog.Properties.Resources.NRC_emotion_lexicon_wordlevel_alphabetized_v0_92__1_.Split('\n'));
            prep.AddRange(wpf_moodlog.Properties.Resources.PrepRef.Split('\n'));
        }
    }
}
