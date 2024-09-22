using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PersonalElectionMgmt.Models;
using PersonalElectionMgmt.Service;

namespace PersonalElectionMgmt.Controllers
{
    public class CitizenController : ControllerBase
    {
        private ElectionDB db;
        public CitizenController(IConfiguration config)
        {
            db = new ElectionDB(config.GetConnectionString("constr"));
        }

        [HttpPost("Citizen", Name = "PostCitizens")]

        public void Post(CitizenParams citizen)
        {
            db.Citizens.Add(new Citizen(citizen.isContender) { Name = citizen.Name });
            db.SaveChanges();
        }

        [HttpGet("Citizen", Name = "GetAllCitizens")]

        public IEnumerable<Citizen> GetAll()
        {
            return from c in db.Citizens
                   select c;
        }

        [HttpGet("Contenders", Name = "GetContenders")]
        public IEnumerable<Citizen> GetContenders()
        {
            return from c in db.Citizens
                   where c.Contender == true
                   select c;
        }

        [HttpGet("NonContenders", Name = "GetNonContenders")]
        public IEnumerable<Citizen> GetNonContenders()
        {
            return from c in db.Citizens
                   where c.Contender == false
                   select c;
        }

        [HttpPost("Contender/Ideas", Name = "PostIdeas")]

        public void PostIdea(IdeaInput idea)
        {
            var count = (from c in db.Ideas
                         where c.ContenderId == idea.ContenderId
                         select c).Count();
            if (count < 3)
            {
                foreach (var i in idea.Ideas)
                {
                    if (count == 3)
                    {
                        break;
                    }
                    db.Ideas.Add(new Idea { Value = i, ContenderId = idea.ContenderId });
                    count++;
                }
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Contender can have only 3 ideas");
            }
        }

        [HttpGet("Contender/Rating/{ContenderId}", Name = "GetRatingForAContender")]

        public IEnumerable<Rating> GetRating(int ContenderId)
        {
            return from r in db.Ratings
                   where r.ToCitizenId == ContenderId
                   select r;
        }


        [HttpPost("Contender/Rating", Name = "RatingIdeas")]

        public void PostRating(RatingInput rating)
        {
            if (rating.Value < 0 || rating.Value > 10)
            {
                throw new Exception("Rating should be between 1 and 10");
            }
            db.Ratings.Add(new Rating {Value = rating.Value, IdeaId= rating.IdeaId, FromCitizenId=rating.FromCitizenId, ToCitizenId=rating.ToCitizenId });
            db.SaveChanges();
        }

        [HttpDelete("Contender/Rating/{FromContenderId}/{IdeaId}", Name = "DeleteRating")]
        public void Delete(int FromContenderId, int IdeaId)
        {
            var res = (from r in db.Ratings
                       where r.FromCitizenId == FromContenderId && r.IdeaId == IdeaId
                       select r).FirstOrDefault();
            if (res != null)
            {
                db.Ratings.Remove(res);
                db.SaveChanges();
            } else
            {
                throw new Exception("Rating not found");
            }
        }

        [HttpDelete("Contender", Name = "ContenderRemoval")]
        public void DeleteContender()
        {
            var contendersToRemove = (from c in db.Citizens
                                      join idea in db.Ideas on c.Id equals idea.ContenderId
                                      join rating in db.Ratings on c.Id equals rating.ToCitizenId
                                      where rating.Value < 5
                                      group rating by new { idea.ContenderId, idea.IdeaId } into ratingGroup
                                      where ratingGroup.Count() > 3
                                      select ratingGroup.Key.ContenderId).Distinct();
            if (contendersToRemove != null)
            {
                foreach (var c in contendersToRemove)
                {
                    var res = (from citizen in db.Citizens
                               where citizen.Id == c
                               select citizen).FirstOrDefault();
                    if (res != null)
                        db.Citizens.Remove(res);
                    db.SaveChanges();
                }
            }
        }

        [HttpGet("HighestIdearatingUsingCCode", Name = "GetHighestRatedIdea")]
        public IdeaHighest GetHighestRatedIdea()
        {
           List<Rating> ra = (from r in db.Ratings
                             select r).ToList();
           List<Idea> id = (from i in db.Ideas
                      select i).ToList();
            List<Citizen> ci = (from c in db.Citizens
                      select c).ToList();
            HighestIdeaRating high = new HighestIdeaRating();
            IdeaHighest hId = new IdeaHighest();
            hId=high.GetHighestRating(ra, id, ci);
            return hId;

        }

        [HttpGet("HighestIdearatingUsingLinqQuery", Name = "GetHighestRatedIdeaUsingLinq")]
        public IdeaHighest GetHighestRatedIdeaLinq()
        {
            Rating? rate = db.Ratings.OrderByDescending(r => r.Value).FirstOrDefault();

            Idea? idea = (from i in db.Ideas
                         where i.IdeaId == rate.IdeaId
                         select i).FirstOrDefault();
            string? fromcitizen = (from ci in db.Citizens
                                where ci.Id == rate.FromCitizenId
                                select ci.Name).FirstOrDefault()?.ToString();
            string? tocitizen = (from ci in db.Citizens
                                   where ci.Id == rate.ToCitizenId
                                   select ci.Name).FirstOrDefault()?.ToString();
            return new IdeaHighest { IdeaId = idea.IdeaId, IdeaValue = idea.Value, ByCitizen = fromcitizen, RatingValue = rate.Value, toCitizen = tocitizen };

        } 


        /*[HttpGet("Contender/Winner", Name = "GetWinner")]
        public IEnumerable<Citizen> GetWinner()
        {
            var contenderRatings = from idea in db.Ideas
                                   join rating in db.Ratings on idea.IdeaId equals rating.IdeaId
                                   group rating by idea.ContenderId into contenderGroup
                                   select new Citizen
                                   {
                                       ContenderId = contenderGroup.Key,
                                       AverageRating = contenderGroup.Average(r => r.Value)
                                   };

            var winner = (from cr in contenderRatings
                          orderby cr.AverageRating descending
                          select cr.ContenderId).FirstOrDefault();

        } */
    }
}
