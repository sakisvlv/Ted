using Annytab.Stemmer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Ted.Dal;
using Ted.Model.Auth;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Ted.Model.Ads;
using Ted.Model.DTO;
using Ted.Model.PersonalSkills;
using Ted.Model.Posts;

namespace Ted.Knn
{
    public class KnnService : IKnnService
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public KnnService(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public double GetDistance(Ad ad, User user)
        {
            double distance = 0;
            foreach (var adKnn in ad.AdKnns)
            {
                var skKnn = user.SkillKnns.Where(x => x.GlobalString.Word == adKnn.GlobalString.Word).FirstOrDefault();
                if (skKnn != null)
                {
                    distance = distance + Math.Pow((((skKnn.Count) / skKnn.Weight) - ((adKnn.Count) / adKnn.Weight)), 2);
                }
                else
                {
                    distance = distance + Math.Pow((adKnn.Count / adKnn.Weight), 2);
                }
            }

            foreach (var skillKnn in user.SkillKnns)
            {
                var aKnn = ad.AdKnns.Where(x => x.GlobalString.Word == skillKnn.GlobalString.Word).FirstOrDefault();
                if (aKnn == null)
                {
                    distance = distance + Math.Pow((skillKnn.Count / skillKnn.Weight), 2);
                }
            }
            return Math.Sqrt(distance);
        }

        public double GetDistance(Post post, User user)
        {
            double distance = 0;
            foreach (var pknn in post.Pknns)
            {
                var postKnn = user.PostKnns.Where(x => x.GlobalString.Word == pknn.GlobalString.Word).FirstOrDefault();
                if (postKnn != null)
                {
                    distance = distance + Math.Pow((postKnn.Count) - (pknn.Count), 2);
                }
                else
                {
                    distance = distance + Math.Pow((pknn.Count), 2);
                }
            }

            foreach (var skillKnn in user.PostKnns)
            {
                var aKnn = post.Pknns.Where(x => x.GlobalString.Word == skillKnn.GlobalString.Word).FirstOrDefault();
                if (aKnn == null)
                {
                    distance = distance + Math.Pow((skillKnn.Count), 2);
                }
            }
            return Math.Sqrt(distance);
        }

        public async Task ManageAd(AdDTO adDTO)
        {
            var stemmedTitle = await AddToGlobalStrings(adDTO.Title);
            var stemmedDescription = await AddToGlobalStrings(adDTO.Description);

            var ad = new Ad
            {
                Title = adDTO.Title,
                Description = adDTO.Description,
                Company = adDTO.Company,
                Owner = await _context.Users.Where(x => x.Id == Guid.Parse(adDTO.Owner.Id)).FirstOrDefaultAsync()
            };

            var finalGlobalRelations = await ToAdKnn(stemmedTitle, ad, true);
            var descriptionGlobalRelations = await ToAdKnn(stemmedDescription, ad, false);

            finalGlobalRelations.AddRange(descriptionGlobalRelations);

            await _context.AdKnns.AddRangeAsync(finalGlobalRelations);
            await _context.Ads.AddAsync(ad);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task RemoveAd(Ad ad)
        {
            var relations = await _context.AdKnns
                .Where(x => x.Ad == ad)
                .Include(x => x.GlobalString)
                .ToListAsync();

            var words = RemoveStopwords(ad.Title).Split();
            words = StemmWords(words);

            var wds = RemoveStopwords(ad.Description).Split();
            wds = StemmWords(wds);

            var result = wds.Concat(words);

            foreach (var relation in relations)
            {
                if (result.Contains(relation.GlobalString.Word))
                {
                    relation.Count--;
                    if (relation.Count == 0)
                    {
                        _context.AdKnns.Remove(relation);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ManageSkill(Skill skill, User user)
        {
            var stemmedTitle = await AddToGlobalStrings(skill.Title);
            var stemmedDescription = await AddToGlobalStrings(skill.Description);

            var finalUserRelations = await ToSkillKnn(stemmedTitle, user, true);
            var descriptionGlobalRelations = await ToSkillKnn(stemmedDescription, user, false);

            finalUserRelations.AddRange(descriptionGlobalRelations);

            await _context.SkillKnns.AddRangeAsync(finalUserRelations);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task RemoveSkill(Skill skill, User user)
        {
            var relations = await _context.SkillKnns
                .Where(x => x.User == user)
                .Include(x => x.GlobalString)
                .ToListAsync();

            var words = RemoveStopwords(skill.Title).Split();
            words = StemmWords(words);

            var wds = RemoveStopwords(skill.Description).Split();
            wds = StemmWords(wds);

            var result = wds.Concat(words);

            foreach (var relation in relations)
            {
                if (result.Contains(relation.GlobalString.Word))
                {
                    relation.Count--;
                    if (relation.Count == 0)
                    {
                        _context.SkillKnns.Remove(relation);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ManageHistory(string text, User user)
        {
            var stemmedText = await AddToGlobalStrings(text);

            var postknn = new PostKnn();

            var finalUserRelations = await ToPostKnn(text.Split(), user);

            await _context.PostKnns.AddRangeAsync(finalUserRelations);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task ManagePosts(string text, Post post)
        {
            var stemmedText = await AddToGlobalStrings(text);

            var pknn = new Pknn();

            var finalUserRelations = await ToPknn(text.Split(), post);

            await _context.PKnns.AddRangeAsync(finalUserRelations);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private async Task<string[]> AddToGlobalStrings(string input)
        {
            var words = RemoveStopwords(input).Split();
            words = StemmWords(words);

            var wds = RemoveStopwords(input).Split();
            wds = StemmWords(wds);

            for (int i = 0; i < words.Length; i++)
            {
                var result = await _context.GlobalStrings.Where(x => x.Word == words[i]).CountAsync();
                if (result == 0)
                {
                    var globalString = new GlobalString();
                    globalString.Word = words[i];
                    await _context.GlobalStrings.AddAsync(globalString);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return wds;
        }

        private async Task<List<AdKnn>> ToAdKnn(string[] words, Ad ad, bool isTitle)
        {
            var relations = new List<AdKnn>();
            var listWords = words.ToList();
            while (listWords.Count != 0)
            {
                var relation = new AdKnn();
                relation.Ad = ad;
                relation.Weight = isTitle == true ? 2 : 1;
                var globalWord = await _context.GlobalStrings.Where(x => x.Word == listWords[0]).FirstOrDefaultAsync();
                relation.GlobalString = globalWord;
                relation.Count = listWords.Where(x => x == listWords[0]).Count();
                relations.Add(relation);
                listWords.RemoveAll(x => x == listWords[0]);
            }

            return relations;
        }

        private async Task<List<SkillKnn>> ToSkillKnn(string[] words, User user, bool isTitle)
        {
            var relations = new List<SkillKnn>();
            var listWords = words.ToList();
            while (listWords.Count != 0)
            {
                var relation = new SkillKnn();
                relation.User = user;
                relation.Weight = isTitle == true ? 2 : 1;
                var globalWord = await _context.GlobalStrings.Where(x => x.Word == listWords[0]).FirstOrDefaultAsync();
                relation.GlobalString = globalWord;
                relation.Count = listWords.Where(x => x == listWords[0]).Count();
                relations.Add(relation);
                listWords.RemoveAll(x => x == listWords[0]);
            }

            return relations;
        }

        private async Task<List<PostKnn>> ToPostKnn(string[] words, User user)
        {
            var relations = new List<PostKnn>();
            var listWords = words.ToList();
            while (listWords.Count != 0)
            {
                var relation = new PostKnn();
                relation.User = user;
                var globalWord = await _context.GlobalStrings.Where(x => x.Word == listWords[0]).FirstOrDefaultAsync();
                relation.GlobalString = globalWord;
                relation.Count = listWords.Where(x => x == listWords[0]).Count();
                relations.Add(relation);
                listWords.RemoveAll(x => x == listWords[0]);
            }

            return relations;
        }

        private async Task<List<Pknn>> ToPknn(string[] words, Post post)
        {
            var relations = new List<Pknn>();
            var listWords = words.ToList();
            while (listWords.Count != 0)
            {
                var relation = new Pknn();
                relation.Post = post;
                var globalWord = await _context.GlobalStrings.Where(x => x.Word == listWords[0]).FirstOrDefaultAsync();
                relation.GlobalString = globalWord;
                relation.Count = listWords.Where(x => x == listWords[0]).Count();
                relations.Add(relation);
                listWords.RemoveAll(x => x == listWords[0]);
            }

            return relations;
        }

        private string[] StemmWords(string[] input)
        {
            IStemmer stemmer = new EnglishStemmer();
            var stemmedWords = stemmer.GetSteamWords(input);
            return stemmedWords;
        }


        /// <summary>
        /// Words we want to remove.
        /// </summary>
        private Dictionary<string, bool> _stops = new Dictionary<string, bool>
    {
        { "a", true },
        { "about", true },
        { "above", true },
        { "across", true },
        { "after", true },
        { "afterwards", true },
        { "again", true },
        { "against", true },
        { "all", true },
        { "almost", true },
        { "alone", true },
        { "along", true },
        { "already", true },
        { "also", true },
        { "although", true },
        { "always", true },
        { "am", true },
        { "among", true },
        { "amongst", true },
        { "amount", true },
        { "an", true },
        { "and", true },
        { "another", true },
        { "any", true },
        { "anyhow", true },
        { "anyone", true },
        { "anything", true },
        { "anyway", true },
        { "anywhere", true },
        { "are", true },
        { "around", true },
        { "as", true },
        { "at", true },
        { "back", true },
        { "be", true },
        { "became", true },
        { "because", true },
        { "become", true },
        { "becomes", true },
        { "becoming", true },
        { "been", true },
        { "before", true },
        { "beforehand", true },
        { "behind", true },
        { "being", true },
        { "below", true },
        { "beside", true },
        { "besides", true },
        { "between", true },
        { "beyond", true },
        { "bill", true },
        { "both", true },
        { "bottom", true },
        { "but", true },
        { "by", true },
        { "call", true },
        { "can", true },
        { "cannot", true },
        { "cant", true },
        { "co", true },
        { "computer", true },
        { "con", true },
        { "could", true },
        { "couldnt", true },
        { "cry", true },
        { "de", true },
        { "describe", true },
        { "detail", true },
        { "do", true },
        { "done", true },
        { "down", true },
        { "due", true },
        { "during", true },
        { "each", true },
        { "eg", true },
        { "eight", true },
        { "either", true },
        { "eleven", true },
        { "else", true },
        { "elsewhere", true },
        { "empty", true },
        { "enough", true },
        { "etc", true },
        { "even", true },
        { "ever", true },
        { "every", true },
        { "everyone", true },
        { "everything", true },
        { "everywhere", true },
        { "except", true },
        { "few", true },
        { "fifteen", true },
        { "fify", true },
        { "fill", true },
        { "find", true },
        { "fire", true },
        { "first", true },
        { "five", true },
        { "for", true },
        { "former", true },
        { "formerly", true },
        { "forty", true },
        { "found", true },
        { "four", true },
        { "from", true },
        { "front", true },
        { "full", true },
        { "further", true },
        { "get", true },
        { "give", true },
        { "go", true },
        { "had", true },
        { "has", true },
        { "have", true },
        { "he", true },
        { "hence", true },
        { "her", true },
        { "here", true },
        { "hereafter", true },
        { "hereby", true },
        { "herein", true },
        { "hereupon", true },
        { "hers", true },
        { "herself", true },
        { "him", true },
        { "himself", true },
        { "his", true },
        { "how", true },
        { "however", true },
        { "hundred", true },
        { "i", true },
        { "ie", true },
        { "if", true },
        { "in", true },
        { "inc", true },
        { "indeed", true },
        { "interest", true },
        { "into", true },
        { "is", true },
        { "it", true },
        { "its", true },
        { "itself", true },
        { "keep", true },
        { "last", true },
        { "latter", true },
        { "latterly", true },
        { "least", true },
        { "less", true },
        { "ltd", true },
        { "made", true },
        { "many", true },
        { "may", true },
        { "me", true },
        { "meanwhile", true },
        { "might", true },
        { "mill", true },
        { "mine", true },
        { "more", true },
        { "moreover", true },
        { "most", true },
        { "mostly", true },
        { "move", true },
        { "much", true },
        { "must", true },
        { "my", true },
        { "myself", true },
        { "name", true },
        { "namely", true },
        { "neither", true },
        { "never", true },
        { "nevertheless", true },
        { "next", true },
        { "nine", true },
        { "no", true },
        { "nobody", true },
        { "none", true },
        { "nor", true },
        { "not", true },
        { "nothing", true },
        { "now", true },
        { "nowhere", true },
        { "of", true },
        { "off", true },
        { "often", true },
        { "on", true },
        { "once", true },
        { "one", true },
        { "only", true },
        { "onto", true },
        { "or", true },
        { "other", true },
        { "others", true },
        { "otherwise", true },
        { "our", true },
        { "ours", true },
        { "ourselves", true },
        { "out", true },
        { "over", true },
        { "own", true },
        { "part", true },
        { "per", true },
        { "perhaps", true },
        { "please", true },
        { "put", true },
        { "rather", true },
        { "re", true },
        { "same", true },
        { "see", true },
        { "seem", true },
        { "seemed", true },
        { "seeming", true },
        { "seems", true },
        { "serious", true },
        { "several", true },
        { "she", true },
        { "should", true },
        { "show", true },
        { "side", true },
        { "since", true },
        { "sincere", true },
        { "six", true },
        { "sixty", true },
        { "so", true },
        { "some", true },
        { "somehow", true },
        { "someone", true },
        { "something", true },
        { "sometime", true },
        { "sometimes", true },
        { "somewhere", true },
        { "still", true },
        { "such", true },
        { "system", true },
        { "take", true },
        { "ten", true },
        { "than", true },
        { "that", true },
        { "the", true },
        { "their", true },
        { "them", true },
        { "themselves", true },
        { "then", true },
        { "thence", true },
        { "there", true },
        { "thereafter", true },
        { "thereby", true },
        { "therefore", true },
        { "therein", true },
        { "thereupon", true },
        { "these", true },
        { "they", true },
        { "thick", true },
        { "thin", true },
        { "third", true },
        { "this", true },
        { "those", true },
        { "though", true },
        { "three", true },
        { "through", true },
        { "throughout", true },
        { "thru", true },
        { "thus", true },
        { "to", true },
        { "together", true },
        { "too", true },
        { "top", true },
        { "toward", true },
        { "towards", true },
        { "twelve", true },
        { "twenty", true },
        { "two", true },
        { "un", true },
        { "under", true },
        { "until", true },
        { "up", true },
        { "upon", true },
        { "us", true },
        { "very", true },
        { "via", true },
        { "was", true },
        { "we", true },
        { "well", true },
        { "were", true },
        { "what", true },
        { "whatever", true },
        { "when", true },
        { "whence", true },
        { "whenever", true },
        { "where", true },
        { "whereafter", true },
        { "whereas", true },
        { "whereby", true },
        { "wherein", true },
        { "whereupon", true },
        { "wherever", true },
        { "whether", true },
        { "which", true },
        { "while", true },
        { "whither", true },
        { "who", true },
        { "whoever", true },
        { "whole", true },
        { "whom", true },
        { "whose", true },
        { "why", true },
        { "will", true },
        { "with", true },
        { "within", true },
        { "without", true },
        { "would", true },
        { "yet", true },
        { "you", true },
        { "your", true },
        { "yours", true },
        { "yourself", true },
        { "yourselves", true }
    };

        /// <summary>
        /// Chars that separate words.
        /// </summary>
        private char[] _delimiters = new char[]
        {
        ' ',
        ',',
        ';',
        '.'
        };

        /// <summary>
        /// Remove stopwords from string.
        /// </summary>
        private string RemoveStopwords(string input)
        {
            // 1
            // Split parameter into words
            var words = input.Split(_delimiters,
                StringSplitOptions.RemoveEmptyEntries);
            // 2
            // Allocate new dictionary to store found words
            var found = new Dictionary<string, bool>();
            // 3
            // Store results in this StringBuilder
            StringBuilder builder = new StringBuilder();
            // 4
            // Loop through all words
            foreach (string currentWord in words)
            {
                // 5
                // Convert to lowercase
                string lowerWord = currentWord.ToLower();
                // 6
                // If this is a usable word, add it
                if (!_stops.ContainsKey(lowerWord) &&
                    !found.ContainsKey(lowerWord))
                {
                    builder.Append(currentWord).Append(' ');
                    found.Add(lowerWord, true);
                }
            }
            // 7
            // Return string with words removed
            return builder.ToString().Trim();
        }

    }
}
