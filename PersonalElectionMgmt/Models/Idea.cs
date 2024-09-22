namespace PersonalElectionMgmt.Models
{
    public class Idea
    {
        public int IdeaId { get; set; }
        public string Value { get; set; }

        public int ContenderId { get; set; }
    }


    public class IdeaInput
    {
        public string[] Ideas { get; set; }
        public int ContenderId { get; set; }
    }


    public class  IdeaCitizen
    {
        public string Name { get; set; }
        public string IdeaValue { get; set; }


    }

    public class IdeaHighest
    {
        public int IdeaId { get; set; }
        public string IdeaValue { get; set; }
        public int RatingValue { get; set; }
        public string ByCitizen { get; set; }
        public string toCitizen { get; set; }
    }
}
