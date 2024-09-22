namespace PersonalElectionMgmt.Models
{
    public class Citizen
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Contender { get; set; }

       public Citizen(bool Contender = false)
        {
            this.Contender = Contender;
        }
    }

    public class  CitizenParams
    {
        public string Name { get; set; }
        public bool isContender { get; set; }
    }
}
