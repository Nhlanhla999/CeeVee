

namespace CeeVee.Models
{
    public class PersonalDetails : ObservableObject
    {
        private string _fullName;
        private string _lastName;
        private string _email;
        private string _phone;
        private string _location;
        private string _profession;
        private string _linkedin;
        private string _github;

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        public string Profession
        {
            get => _profession;
            set => SetProperty(ref _profession, value);
        }

        public string LinkedIn
        {
            get => _linkedin;
            set => SetProperty(ref _linkedin, value);
        }

        public string Github
        {
            get => _github;
            set => SetProperty(ref _github, value);
        }


    }
}
