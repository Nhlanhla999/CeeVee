
namespace CeeVee.Models
{
    public class Project : ObservableObject
    {
        private string _title;
        private string _link;
        private string _description;
        private string _year;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Link
        {
            get => _link;
            set => SetProperty(ref _link, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }
    }
}
