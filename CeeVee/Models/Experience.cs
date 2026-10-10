using System;

namespace CeeVee.Models
{
    public class Experience : ObservableObject
    {
        private string _jobTitle;
        private string _company;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private bool _isPresent;
        private string _description;

        // =========================================================
        // JOB TITLE
        // =========================================================

        public string JobTitle
        {
            get => _jobTitle;
            set => SetProperty(ref _jobTitle, value);
        }

        // =========================================================
        // COMPANY
        // =========================================================

        public string Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }

        // =========================================================
        // START DATE
        // =========================================================

        public DateTime? StartDate
        {
            get => _startDate;

            set
            {
                if (!SetProperty(ref _startDate, NormalizeMonth(value)))
                {
                    return;
                }

                OnPropertyChanged(nameof(StartDateText));
            }
        }

        // =========================================================
        // END DATE
        // =========================================================

        public DateTime? EndDate
        {
            get => _endDate;

            set
            {
                if (!SetProperty(ref _endDate, NormalizeMonth(value)))
                {
                    return;
                }

                OnPropertyChanged(nameof(EndDateText));
            }
        }

        // =========================================================
        // PRESENT
        // =========================================================

        public bool IsPresent
        {
            get => _isPresent;

            set
            {
                if (!SetProperty(ref _isPresent, value))
                {
                    return;
                }

                OnPropertyChanged(nameof(EndDateText));
            }
        }

        // =========================================================
        // DESCRIPTION
        // =========================================================

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        // =========================================================
        // DISPLAY START DATE
        // =========================================================

        public string StartDateText
        {
            get
            {
                return StartDate.HasValue? StartDate.Value.ToString("MMMM yyyy"): "";
            }
        }

        // =========================================================
        // DISPLAY END DATE
        // =========================================================

        public string EndDateText
        {
            get
            {
                if (IsPresent)
                    return "Present";

                return EndDate.HasValue? EndDate.Value.ToString("MMMM yyyy") : "";
            }
        }

        // =========================================================
        // NORMALIZE MONTH
        // =========================================================

        private DateTime? NormalizeMonth(DateTime? value)
        {
            if (!value.HasValue)
                return null;

            return new DateTime(value.Value.Year, value.Value.Month, 1);
        }
    }
}
