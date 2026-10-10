
namespace CeeVee.Models
{
    public class Skill : ObservableObject
    {
        private string _name;

        public string Name
        {
            get => _name;

            set => SetProperty(ref _name, value);
        }
    }
}
