using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace RaterGator
{
    /// <summary>
    /// An improved version of the RaterGator engine that makes use of the power of LINQ
    /// </summary>
    class GatorLinq
    {
        #region Variables

        static string file = "u.data.csv";
        static IEnumerable<string[]> dataset;
        static string[] separator = new string[] { "\t" };

        static IEnumerable<MovieReview> soulmates97;
        static Dictionary<int, int> soulmateScores = new Dictionary<int, int>();

        #endregion

        static void Main(string[] args)
        {
            //var movies = GetUsersMovieReviews(1, 1997);
            soulmates97 = GetUsersSoulmatesMovieReviews(1, 1997);
            var topSoulmates = GetTopSoulmates(10); 

            //foreach (var movie in movies)
            //{
            //    Console.WriteLine(movie);
            //}

            foreach (var sm in topSoulmates)
            {
                Console.WriteLine(sm.Key + " - " + sm.Value + " matches.");
            }

            Console.WriteLine(soulmates97.Count() + " matches found.");

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        #region GetAllMovieReviews()
        /// <summary>
        /// Get all movie reviews available in our dataset
        /// </summary>
        /// <returns></returns>
        static IEnumerable<MovieReview> GetAllMovieReviews()
        {
            return ParseCsv().Select(x => new MovieReview(x));
        } 
        #endregion

        #region GetUsersMovieReviews()
        /// <summary>
        /// Get movie reviews in our dataset by a given userID and in a given year
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        static IEnumerable<MovieReview> GetUsersMovieReviews(int userID, int year)
        {
            return ParseCsv().Where(x => int.Parse(x[0]) == userID && MovieReview.GetYearOfRating(x[3]) == year)
                             .Select(x => new MovieReview(x))
                             .OrderBy(x => x.movieID);
        }
        #endregion

        #region GetUsersSoulmatesMovieReviews()
        /// <summary>
        /// Get soulmate movie reviews in our dataset by a given userID and in a given year
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        static IEnumerable<MovieReview> GetUsersSoulmatesMovieReviews(int userID, int year, bool refresh = false)
        {
            var usersReviews = GetUsersMovieReviews(userID, year);
            List<MovieReview> results = new List<MovieReview>();

            foreach (var review in usersReviews)
            {
                var sm = GetMatchingMovieReviews(userID, review.movieID, review.rating, year);
                results.AddRange(sm);
                AddToSoulmateScore(sm);
                Console.Clear();
                Console.WriteLine(results.Count + " matches found...");
            }

            return results;
        }
        #endregion

        #region GetSoulmatesMovieReviewsInNewYear()
        /// <summary>
        /// Get the movie reviews of your previous year's soulmates, that you have also reviewed in the new year
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="year"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        static IEnumerable<MovieReview> GetSoulmatesMovieReviewsInNewYear(int userID, int year, bool refresh = false)
        {
            // get soulmates from previous year
            var soulmatesReviews = GetUsersSoulmatesMovieReviews(userID, year - 1);
            var soulmates = GetTopSoulmates(0);

            // get movies rated by userID this year
            var movies = GetUsersMovieReviews(userID, year);

            // get ratings by soulmates of the same movies you rated this year


            throw new NotImplementedException();
        } 
        #endregion

        // Helpers

        #region GetTopSoulmates()
        /// <summary>
        /// Get top n number of soulmates scores for the last year that was used
        /// for the lookup of the soulmates movies reviews.
        /// Using 0 (zero) as a wild card you can return all soulmates found.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static IEnumerable<KeyValuePair<int, int>> GetTopSoulmates(int number)
        {
            number = (number == 0 || soulmateScores.Count > number) ? number : soulmateScores.Count;
            return soulmateScores.OrderByDescending(key => key.Value).Take(number);
        } 
        #endregion

        #region AddToSoulmateScore()
        static void AddToSoulmateScore(IEnumerable<MovieReview> matchingMovies)
        {
            foreach (var mm in matchingMovies)
            {
                if (soulmateScores.ContainsKey(mm.userID))
                    soulmateScores[mm.userID] = soulmateScores[mm.userID] + 1;
                else
                    soulmateScores.Add(mm.userID, 1);
            }
        } 
        #endregion

        #region GetMatchingMovieReviews()
        /// <summary>
        /// Get matching movie reviews in our dataset for a given userID and in a given year
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        static IEnumerable<MovieReview> GetMatchingMovieReviews(int userID, int movieID, int rating, int year)
        {
            return ParseCsv().Where(x => int.Parse(x[0]) != userID
                                         && int.Parse(x[1]) == movieID
                                         && int.Parse(x[2]) == rating
                                         && MovieReview.GetYearOfRating(x[3]) == year)
                             .Select(x => new MovieReview(x))
                             .OrderBy(x => x.movieID);
        }
        #endregion
        
        #region ParseCsv()
        static IEnumerable<string[]> ParseCsv()
        {
            if(dataset == null)
                dataset = File.ReadAllLines(file).Select(x => x.Split(separator, StringSplitOptions.None)).ToList();
            return dataset;
        } 
        #endregion        
    }

    #region MovieReview
    class MovieReview
    {
        public int userID;
        public int movieID;
        public int rating;
        public DateTime date;

        public MovieReview(string[] x)
        {
            userID = int.Parse(x[0]);
            movieID = int.Parse(x[1]);
            rating = int.Parse(x[2]);
            date = GetDateFromUnixTimestamp(x[3]);
        }

        public override string ToString()
        {
            return "UserID: " + userID
                    + " - MovieID: " + movieID
                    + " - Rating: " + rating
                    + " - Date: - " + date.ToString("dd-MM-yyyy");
        }

        #region GetDateFromUnixTimestamp()
        public static DateTime GetDateFromUnixTimestamp(string timestamp)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            d = d.AddSeconds(long.Parse(timestamp));
            return d;
        }
        #endregion

        #region GetYearOfRating()
        public static int GetYearOfRating(string timestamp)
        {
            return GetDateFromUnixTimestamp(timestamp).Year;
        }
        #endregion
    } 
    #endregion

    #region SoulMateComparer
    class SoulMateComparer : IEqualityComparer<MovieReview>
    {
        public bool Equals(MovieReview x, MovieReview y)
        {
            return x.movieID.Equals(y.movieID) && x.rating.Equals(y.rating);
        }

        public int GetHashCode(MovieReview x)
        {
            return x.GetHashCode();
        }
    } 
    #endregion

    #region Compare
    public static class Compare
    {
        public static IEnumerable<T> DistinctBy<T, TIdentity>(this IEnumerable<T> source, Func<T, TIdentity> identitySelector)
        {
            return source.Distinct(Compare.By(identitySelector));
        }

        public static IEqualityComparer<TSource> By<TSource, TIdentity>(Func<TSource, TIdentity> identitySelector)
        {
            return new DelegateComparer<TSource, TIdentity>(identitySelector);
        }

        private class DelegateComparer<T, TIdentity> : IEqualityComparer<T>
        {
            private readonly Func<T, TIdentity> identitySelector;

            public DelegateComparer(Func<T, TIdentity> identitySelector)
            {
                this.identitySelector = identitySelector;
            }

            public bool Equals(T x, T y)
            {
                return Equals(identitySelector(x), identitySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return identitySelector(obj).GetHashCode();
            }
        }
    } 
    #endregion
}