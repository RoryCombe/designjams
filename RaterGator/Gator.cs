using System;
using System.IO;
using System.Collections.Generic;


class Gator
{
    static void Main(string[] args)
    {
        var mainUser = "1";
		for (int i = 1; i < 400; i++)
		{
		    mainUser = i.ToString();
		    Console.WriteLine ("User ID: " + mainUser);
		    try
		    {
		        
		            
		            var file = File.ReadLines("u.data.csv");
		            string[] sep = new string[]{"\t"};
		            
		            
		            
		            var userOnesRatings = new Dictionary<string, string>();
		            var matchingRatings = new Dictionary<string, string>();
		            var likelyPartners = new Dictionary<string, string>();
		            
		            var userOnesRatings98 = new Dictionary<string, string>();
		            
		            foreach (var line in file)
		            {
		                
		                var arr = line.Split(sep, StringSplitOptions.None);
		                
		                
		                System.DateTime d = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		                // den Timestamp addieren           
		                d = d.AddSeconds(long.Parse(arr[3]));
		            
		                if(d.Year == 1997 && arr[0] == mainUser)
		                {
		                    userOnesRatings.Add(arr[1], arr[2]);
		                }
		            }
		            
		            file = File.ReadLines("u.data.csv");
		            
		            foreach (var line in file)
		            {
		                var arr = line.Split(sep, StringSplitOptions.None);
		                
		                System.DateTime d = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		                // den Timestamp addieren           
		                d = d.AddSeconds(long.Parse(arr[3]));
		                
		                if(d.Year == 1997 && arr[0] != mainUser)
		                {
		                    var itemid = arr[1];
		                    var userOnesValue = "";
		                    if(userOnesRatings.ContainsKey(itemid))
		                    {
		                        userOnesValue = userOnesRatings[itemid];
		                        var otherUsersValue = arr[2];
		                        
		                        if(userOnesValue.Equals(otherUsersValue))
		                        {
		                            //Console.WriteLine ("Found match: " + itemid + " - " + userOnesValue + " - " + otherUsersValue);
		                            if(matchingRatings.ContainsKey(itemid))
		                            {
		                                matchingRatings[itemid] = (int.Parse(matchingRatings[itemid]) + 1).ToString();    
		                                if(likelyPartners.ContainsKey(arr[0]))
		                                {
		                                    likelyPartners[arr[0]] = (int.Parse(likelyPartners[arr[0]]) + 1).ToString();
		                                }
		                                else
		                                {
		                                    likelyPartners.Add(arr[0], "1");
		                                }
		                            }
		                            else
		                            {
		                                matchingRatings.Add(itemid, "1");
		                                if(likelyPartners.ContainsKey(arr[0]))
		                                {
		                                    likelyPartners[arr[0]] = (int.Parse(likelyPartners[arr[0]]) + 1).ToString();
		                                }
		                                else
		                                {
		                                    likelyPartners.Add(arr[0], "1");
		                                }
		                                
		                            }
		                        }
		                    }
		                }
		            }
		            
		            List<KeyValuePair<string, string>> myList = likelyPartners.ToList();
		            
		                myList.Sort(
		                    delegate(KeyValuePair<string, string> firstPair,
		                    KeyValuePair<string, string> nextPair)
		                    {
		                        return int.Parse(firstPair.Value).CompareTo(int.Parse(nextPair.Value));
		                    }
		                );
		            
		            //foreach (var element in myList)
		            //{
		            //    Console.WriteLine ("ID: "+ element.Key + " - Matched " + element.Value + " time(s)");
		            //}
		            //
		            //foreach (var element in matchingRatings)
		            //{
		            //    Console.WriteLine ("User ones rating: " + userOnesRatings[element.Key] + " - Item ID: " + element.Key + " - Rating Matched " + element.Value + " time(s)");
		            //}
		            
		            file = File.ReadLines("u.data.csv");
		            
		            foreach (var line in file)
		            {
		                
		                var arr = line.Split(sep, StringSplitOptions.None);
		                
		                
		                System.DateTime d = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		                // den Timestamp addieren           
		                d = d.AddSeconds(long.Parse(arr[3]));
		            
		                if(d.Year == 1998 && arr[0] == mainUser)
		                {
		                    userOnesRatings98.Add(arr[1], arr[2]);
		                }
		            }
		            
		            //foreach (var element in userOnesRatings98)
		            //{
		            //    Console.WriteLine ("Item ID: "+ element.Key + " - Rating " + element.Value);
		            //}
		            
		            file = File.ReadLines("u.data.csv");
		            
		            var aggregatedPerItem = new Dictionary<string, Dictionary<string, string>>();
		            
		            foreach (var line in file)
		            {
		                
		                var arr = line.Split(sep, StringSplitOptions.None);
		                
		                
		                System.DateTime d = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		                // den Timestamp addieren           
		                d = d.AddSeconds(long.Parse(arr[3]));
		            
		                if(d.Year == 1997 && arr[0] != mainUser && likelyPartners.ContainsKey(arr[0]) && userOnesRatings98.ContainsKey(arr[1]))
		                {
		                    var userid = arr[0];
		                    var itemid = arr[1];
		                    var rating = arr[2];
		                    
		                    var soulMateScore = int.Parse(likelyPartners[userid]);
		            
		                    if(aggregatedPerItem.ContainsKey(itemid))
		                    {
		                        Dictionary<string, string> couples = aggregatedPerItem[itemid];
		                        couples.Add(userid, rating);
		                    }
		                    else
		                    {
		                        var dict = new Dictionary<string, string>();
		                        dict.Add(userid, rating);
		                        aggregatedPerItem.Add(itemid, dict);
		                    }
		            //        
		            //        var weightedScore = rating * soulMateScore / soulMateScore;
		            //        
		            //        Console.WriteLine (userid + " - " + itemid + " - " + rating + " - Weighted score: " + weightedScore);
		                    //userOnesRatings98.Add(arr[1], arr[2]);
		                }
		            }
		            
		            var score_actual98 = new Dictionary<string, float>();
		            var score_amazon = new Dictionary<string, float>();
		            var score_gator = new Dictionary<string, float>();
		            
		            
		            foreach (var element in aggregatedPerItem)
		            {
		                var itemid = element.Key;
		                var couples = element.Value;
		                
		                //Console.WriteLine ("Item ID: " + itemid);
		                
		                float topScore = 0;
		                float bottomScore = 0;
		                
		                foreach (var elem in couples)
		                {
		                    var soulmateScore = likelyPartners[elem.Key]; 
		                    var soulmateID = elem.Key;
		                    var soulmateProductRating = elem.Value;
		                    
		                    topScore += float.Parse(soulmateScore) * float.Parse(soulmateProductRating);
		                }
		                
		                foreach (var elem in couples)
		                {
		                    var soulmateScore = likelyPartners[elem.Key]; 
		                    var soulmateID = elem.Key;
		                    var soulmateProductRating = elem.Value;
		                    
		                    bottomScore += float.Parse(soulmateScore);
		                }
		                
		                score_gator.Add(itemid, topScore/bottomScore);
		                
		                //Console.WriteLine ("Item id : " + itemid + " has weighted score: " + topScore/bottomScore);
		            }
		            
		            file = File.ReadLines("u.data.csv");
		            
		            var totalRatingItem = new Dictionary<string, Dictionary<string, string>>();
		            
		            foreach (var line in file)
		            {
		                
		                var arr = line.Split(sep, StringSplitOptions.None);
		                
		                
		                System.DateTime d = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		                // den Timestamp addieren           
		                d = d.AddSeconds(long.Parse(arr[3]));
		            
		                if(d.Year == 1997 && arr[0] != mainUser && userOnesRatings98.ContainsKey(arr[1]))
		                {
		                    var userid = arr[0];
		                    var itemid = arr[1];
		                    var rating = arr[2];
		                    
		                    
		                    if(totalRatingItem.ContainsKey(itemid))
		                    {
		                        Dictionary<string, string> couples = totalRatingItem[itemid];
		                        couples.Add(userid, rating);
		                    }
		                    else
		                    {
		                        var dict = new Dictionary<string, string>();
		                        dict.Add(userid, rating);
		                        totalRatingItem.Add(itemid, dict);
		                    }
		            //        
		            //        var weightedScore = rating * soulMateScore / soulMateScore;
		            //        
		            //        Console.WriteLine (userid + " - " + itemid + " - " + rating + " - Weighted score: " + weightedScore);
		                    //userOnesRatings98.Add(arr[1], arr[2]);
		                }
		            }
		            
		            //Console.WriteLine ("---------------");
		            
		            foreach (var element in totalRatingItem)
		            {
		                var itemid = element.Key;
		                var couples = element.Value;
		                
		                float topScore = 0;
		                int count = 0;
		                
		                foreach (var elem in couples)
		                {
		                    var user = elem.Key;
		                    var rating = elem.Value;
		                    
		                    topScore += float.Parse(rating);
		                    count++;
		                }
		                
		                score_amazon.Add(itemid, topScore/count);    
		                    
		                //Console.WriteLine ("Item id : " + itemid + " has total score: " + topScore/count);
		            }
		            
		            
		            
		            foreach (var element in userOnesRatings98)
		            {
		                score_actual98.Add(element.Key, float.Parse(element.Value));
		                
		                //Console.WriteLine ("Item id : " + element.Key + " - rating: " + element.Value);
		            }
		            
		            int wins = 0;
		            
		            foreach (var element in score_actual98)
		            {
		                float act = element.Value;
		                float ama = score_amazon[element.Key];
		                float gat = score_gator[element.Key];
		                
		                float AmaDeltaAct = (act - ama);
		                float GatDeltaAct = (act - gat);
		                
		                var normAma = Math.Abs(AmaDeltaAct);
		                var normGat = Math.Abs(GatDeltaAct);
		                
		                var win = normGat < normAma ? "We win!!!" : "\tWe lose :("; 
		                
		                if(normGat < normAma)
		                    wins += 1;
		                else
		                    wins =- 1;
		                
		                Console.WriteLine(win + " - Item: " + element.Key + " accuracy Amazon " + AmaDeltaAct + " vs. accuracy Gator " + GatDeltaAct);
		            }
		            
		            Console.WriteLine ("Wins: " + wins);
		            
		            Console.WriteLine ("---------------");
		    
		    }
		    catch
		    {
		        continue;
		    }
		}

    }
}