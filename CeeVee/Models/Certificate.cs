using System;

namespace CeeVee.Models
{
    public class Certificate : ObservableObject
    {
        private string _name;
        private string _institution;
        private DateTime? _year;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Institution
        {
            get => _institution;
            set => SetProperty(ref _institution, value);
        }

        public DateTime? Year
        {
            get => _year;

            set
            {
                DateTime? normalized = NormalizeMonth(value);

                if (!SetProperty(ref _year, normalized))
                {
                    return;
                }

                OnPropertyChanged(nameof(YearText));
            }
        }

        public string YearText
        {
            get
            {
                return Year.HasValue? Year.Value.ToString("MMMM yyyy"): "";
            }
        }

        private DateTime? NormalizeMonth(DateTime? value)
        {
            if (!value.HasValue)
                return null;

            return new DateTime(value.Value.Year, value.Value.Month, 1);
        }
    }
}
