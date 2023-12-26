namespace bojan_recipe.Models
{
    public class Gallery
    {
        public int GalleryId { get; set; } // Unique identifier for the Gallery
        public string GalleryName { get; set; } // Name of the Gallery
        public string GalleryDescription { get; set; } // Description of the Gallery
        public string GalleryImageUrl { get; set; } // URL to the Gallery's img
        public string CreatedBy { get; set; }
    }
}
