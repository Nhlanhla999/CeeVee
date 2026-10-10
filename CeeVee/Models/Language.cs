

namespace CeeVee.Models
{
    public class Language : ObservableObject
    {
        private string _name;

        public string Name
        {
            get => _name;

            set => SetProperty(ref _name, value);
        }
    }
}
