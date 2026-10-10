using System;

namespace CeeVee.Models
{
    public class Achievement : ObservableObject
    {
        private string _title;
        private string _description;
        private DateTime? _year;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
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
                return Year.HasValue ? Year.Value.ToString("MMMM yyyy") : "";
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

