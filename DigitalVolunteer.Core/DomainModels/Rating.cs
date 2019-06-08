namespace DigitalVolunteer.Core.DomainModels
{
    public class Rating
    {
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        public double AverageBall => 4.55;
    }
}