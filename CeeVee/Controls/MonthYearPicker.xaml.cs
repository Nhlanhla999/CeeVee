using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CeeVee.Controls
{
    public partial class MonthYearPicker : UserControl
    {
        private DateTime _displayYear;
        public event EventHandler SelectionChanged;

        public MonthYearPicker()
        {
            InitializeComponent();

            _displayYear = new DateTime(DateTime.Today.Year, 1, 1);

            BuildMonths();

            UpdateDisplay();
        }


        // ============================================================
        // SELECTED MONTH
        // ============================================================

        public static readonly DependencyProperty SelectedMonthProperty =
            DependencyProperty.Register(
                nameof(SelectedMonth),
                typeof(DateTime?),
                typeof(MonthYearPicker),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectedMonthChanged));


        public DateTime? SelectedMonth
        {
            get => (DateTime?)GetValue(SelectedMonthProperty);

            set => SetValue(SelectedMonthProperty, value);
        }


        // ============================================================
        // MINIMUM MONTH
        // ============================================================

        public static readonly DependencyProperty MinimumMonthProperty =
            DependencyProperty.Register(
                nameof(MinimumMonth),
                typeof(DateTime?),
                typeof(MonthYearPicker),
                new PropertyMetadata(
                    null,
                    MinimumMonthChanged));


        public DateTime? MinimumMonth
        {
            get => (DateTime?)GetValue(MinimumMonthProperty);

            set => SetValue(MinimumMonthProperty, value);
        }


        // ============================================================
        // MAXIMUM MONTH
        // ============================================================

        public static readonly DependencyProperty MaximumMonthProperty =
            DependencyProperty.Register(
                nameof(MaximumMonth),
                typeof(DateTime?),
                typeof(MonthYearPicker),
                new PropertyMetadata(
                    null,
                    MaximumMonthChanged));


        public DateTime? MaximumMonth
        {
            get => (DateTime?)GetValue(MaximumMonthProperty);

            set => SetValue(MaximumMonthProperty, value);
        }


        // ============================================================
        // SELECTED MONTH CHANGED
        // ============================================================

        private static void SelectedMonthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MonthYearPicker picker = d as MonthYearPicker;

            if (picker == null)
                return;

            picker.NormalizeSelectedMonth();
            picker.UpdateDisplay();
            picker.BuildMonths();
        }


        private static void MinimumMonthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MonthYearPicker picker = d as MonthYearPicker;

            if (picker == null)
                return;

            picker.NormalizeSelectedMonth();
            picker.BuildMonths();
        }


        private static void MaximumMonthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MonthYearPicker picker = d as MonthYearPicker;

            if (picker == null)
                return;

            picker.NormalizeSelectedMonth();
            picker.BuildMonths();
        }


        // ============================================================
        // NORMALIZE MONTH
        // ============================================================

        private void NormalizeSelectedMonth()
        {
            if (!SelectedMonth.HasValue)
                return;

            DateTime month = new DateTime(SelectedMonth.Value.Year, SelectedMonth.Value.Month, 1);

            DateTime? minimum = Normalize(MinimumMonth);

            DateTime? maximum = Normalize(MaximumMonth);

            if (minimum.HasValue &&
                month < minimum.Value)
            {
                month = minimum.Value;
            }

            if (maximum.HasValue &&
                month > maximum.Value)
            {
                month = maximum.Value;
            }

            if (SelectedMonth.Value != month)
            {
                SetCurrentValue(SelectedMonthProperty, month);
            }
        }


        // ============================================================
        // PICKER BUTTON
        // ============================================================

        private void PickerButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMonth.HasValue)
            {
                _displayYear = new DateTime(SelectedMonth.Value.Year, 1, 1);
            }
            else
            {
                _displayYear = new DateTime(DateTime.Today.Year, 1, 1);
            }

            UpdateDisplay();
            BuildMonths();

            CalendarPopup.IsOpen = true;
        }


        // ============================================================
        // PREVIOUS YEAR
        // ============================================================

        private void PreviousYear_Click(object sender, RoutedEventArgs e)
        {
            int year = _displayYear.Year - 1;

            DateTime candidate = new DateTime(year, 1, 1);

            DateTime? minimum = Normalize(MinimumMonth);

            if (minimum.HasValue && candidate.Year < minimum.Value.Year)
            {
                return;
            }

            _displayYear = candidate;

            UpdateDisplay();
            BuildMonths();
        }


        // ============================================================
        // NEXT YEAR
        // ============================================================

        private void NextYear_Click(object sender, RoutedEventArgs e)
        {
            int year = _displayYear.Year + 1;

            DateTime candidate = new DateTime(year, 1, 1);

            DateTime? maximum = Normalize(MaximumMonth);

            if (maximum.HasValue && candidate.Year > maximum.Value.Year)
            {
                return;
            }

            _displayYear = candidate;

            UpdateDisplay();
            BuildMonths();
        }


        // ============================================================
        // BUILD MONTHS
        // ============================================================

        private void BuildMonths()
        {
            if (MonthsGrid == null)
                return;

            MonthsGrid.Children.Clear();

            for (int month = 1; month <= 12; month++)
            {
                DateTime currentMonth = new DateTime(_displayYear.Year, month, 1);

                Button button =
                    new Button
                    {
                        Content =
                            currentMonth.ToString("MMM"),

                        Tag = currentMonth,

                        Style =
                            (Style)FindResource(
                                "MonthButtonStyle")
                    };

                bool enabled = true;


                // ====================================================
                // CURRENT MONTH LIMIT
                // ====================================================

                DateTime todayMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                if (currentMonth > todayMonth)
                {
                    enabled = false;
                }


                // ====================================================
                // MINIMUM LIMIT
                // ====================================================

                DateTime? minimum = Normalize(MinimumMonth);

                if (minimum.HasValue && currentMonth < minimum.Value)
                {
                    enabled = false;
                }


                // ====================================================
                // MAXIMUM LIMIT
                // ====================================================

                DateTime? maximum = Normalize(MaximumMonth);

                if (maximum.HasValue &&
                    currentMonth > maximum.Value)
                {
                    enabled = false;
                }


                button.IsEnabled = enabled;

                if (SelectedMonth.HasValue && Normalize(SelectedMonth).Value == currentMonth)
                {
                    button.Background =
                        new SolidColorBrush(
                            Color.FromRgb(
                                243,
                                240,
                                234));

                    button.BorderBrush =
                        new SolidColorBrush(
                            Color.FromRgb(
                                168,
                                139,
                                90));

                    button.FontWeight =
                        FontWeights.SemiBold;
                }


                button.Click +=
                    MonthButton_Click;

                MonthsGrid.Children.Add(button);
            }


            DateTime today =
                DateTime.Today;

            if (_displayYear.Year == today.Year)
            {
                FooterText.Text =
                    "Future months are disabled";
            }
            else
            {
                FooterText.Text =
                    "Select a month";
            }
        }


        // ============================================================
        // MONTH SELECTED
        // ============================================================

        private void MonthButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            if (!(button.Tag is DateTime))
                return;

            DateTime month = (DateTime)button.Tag;

            SelectedMonth = month;

            SelectionChanged?.Invoke(this, EventArgs.Empty);

            CalendarPopup.IsOpen = false;
        }


        // ============================================================
        // UPDATE DISPLAY
        // ============================================================

        private void UpdateDisplay()
        {
            if (SelectedText == null)
                return;

            if (SelectedMonth.HasValue)
            {
                SelectedText.Text = SelectedMonth.Value.ToString("MMMM yyyy");
            }
            else
            {
                SelectedText.Text = "Select month and year";
            }

            if (YearText != null)
            {
                YearText.Text =_displayYear.Year.ToString();
            }
        }


        // ============================================================
        // NORMALIZE
        // ============================================================

        private DateTime? Normalize(DateTime? value)
        {
            if (!value.HasValue)
                return null;

            return new DateTime(value.Value.Year, value.Value.Month, 1);
        }
    }
}
