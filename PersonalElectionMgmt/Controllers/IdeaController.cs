using Microsoft.AspNetCore.Mvc;
using PersonalElectionMgmt.Models;

namespace PersonalElectionMgmt.Controllers
{
    public class IdeaController
    {
        private ElectionDB db;
        public IdeaController(IConfiguration config)
        {
            db = new ElectionDB(config.GetConnectionString("constr"));
        }
        [HttpGet("Manifesto/{CondenderId}", Name = "GetIdeasForAContender")]
        public IEnumerable<IdeaCitizen> GetContenderManifesto(int CondenderId)
        {
            return from c in db.Citizens
                   join i in db.Ideas on c.Id equals i.ContenderId
                   where i.ContenderId == CondenderId
                   select new IdeaCitizen
                   {
                       Name = c.Name,
                       IdeaValue = i.Value
                   };
        }

    }
}
