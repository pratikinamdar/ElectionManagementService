namespace PersonalElectionMgmt.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int Value { get; set; }

        public int IdeaId { get; set; }

        public int FromCitizenId { get; set; }

        public int ToCitizenId { get; set;}
    }

    public class RatingInput
    {
        public int Value { get; set; }
        public int IdeaId { get; set; }
        public int FromCitizenId { get; set; }
        public int ToCitizenId { get; set; }
    }
}
