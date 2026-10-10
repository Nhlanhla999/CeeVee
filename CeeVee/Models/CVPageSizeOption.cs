

namespace CeeVee.Models
{
    public class CVPageSizeOption
    {
        public CVPageSize Type { get; set; }

        public string Name { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public string Description { get; set; }

        public CVPageSizeOption(CVPageSize type, string name, double width, double height, string description)
        {
            Type = type;
            Name = name;
            Width = width;
            Height = height;
            Description = description;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}