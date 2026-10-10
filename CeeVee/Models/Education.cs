using System;

namespace CeeVee.Models
{
    public class Education : ObservableObject
    {
        private string _degree;
        private string _institution;
        private DateTime? _year;
        private bool _isPresent;

        public string Degree
        {
            get => _degree;
            set => SetProperty(ref _degree, value);
        }

        public string Institution
        {
            get => _institution;
            set => SetProperty(ref _institution, value);
        }

        /// <summary>
        /// Stores only the selected month and year.
        /// The day is always normalized to 1.
        /// </summary>
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

        public bool IsPresent
        {
            get => _isPresent;

            set
            {
                if (!SetProperty(ref _isPresent, value))
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
                if (IsPresent)
                    return "Present";

                if (!Year.HasValue)
                    return "";

                return Year.Value.ToString("MMMM yyyy");
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
