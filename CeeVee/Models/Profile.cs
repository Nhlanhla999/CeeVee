
namespace CeeVee.Models
{
    public class Profile : ObservableObject
    {
        private string _summary;

        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }
    }
}
