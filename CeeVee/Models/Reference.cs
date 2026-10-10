
namespace CeeVee.Models
{
    public class Reference : ObservableObject
    {
        private string _name;
        private string _position;
        private string _company;
        private string _phone;
        private string _email;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public string Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
    }
}

