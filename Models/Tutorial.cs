namespace bojan_recipe.Models
{
    public enum TutorialCategory
    {
        Beginner,
        Intermediate,
        Expert
    }

    public class Tutorial
    {
        public int TutorialId { get; set; } // Unique identifier for the tutorial
        public string TutorialName { get; set; } // Name of the tutorial
        public string TutorialDescription { get; set; } // Description of the tutorial
        public string TutorialVideoUrl { get; set; } // URL to the tutorial's video
        public TutorialCategory Category { get; set; } // Category
        public string CreatedBy { get; set; }
    }
}
