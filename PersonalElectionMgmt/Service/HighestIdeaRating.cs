using PersonalElectionMgmt.Models;

namespace PersonalElectionMgmt.Service
{
    public class HighestIdeaRating
    {
        public IdeaHighest GetHighestRating(List<Rating> r, List<Idea> i,List<Citizen> c)
        {
            Console.WriteLine(r);
            int rValue;
            int iId;
            int iValue;
            string cFrom = string.Empty;
            string cTo = string.Empty;
            Rating res = new Rating();
            Idea ide = new Idea();
            foreach (var ra in r)
            {
                int value = int.MinValue;
                if (ra.Value > value)
                {
                    res = ra;
                }
            }
            rValue = res.Value;
            iId = res.IdeaId;

            foreach (var idea in i) 
            {
                if (idea.IdeaId == iId)
                {
                    ide = idea;
                }
            }

            foreach (var ci in c)
            {
                if (ci.Id == res.FromCitizenId)
                {
                    cFrom = ci.Name;
                }
                if (ci.Id == res.ToCitizenId)
                {
                    cTo = ci.Name;
                }
            }
            return new IdeaHighest { IdeaId = ide.IdeaId, IdeaValue = ide.Value, ByCitizen = cFrom, RatingValue = res.Value, toCitizen = cTo } ;
        }
    }
}
