using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PBTFrontOfficeProject.Controls.DatePicker
{
    /// <summary>
    /// Interaction logic for PosDatePicker.xaml
    /// </summary>
    public partial class PosDatePicker : UserControl
    {
        private const int FormatLengthOfLast = 2;

        private enum Direction : int
        {
            Previous = -1,
            Next = 1
        }

        public PosDatePicker()
        {
            InitializeComponent();
            TimeDisplay.Visibility = System.Windows.Visibility.Collapsed;
            if (TimeDisplay.Value == null)
                TimeDisplay.Value = DateTime.Now;
        }

        #region Properties

        public DateTime? SelectedDate
        {
            get { return Convert.ToDateTime(GetValue(SelectedDateProperty)); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public string DateFormat
        {
            get { return Convert.ToString(GetValue(DateFormatProperty)); }
            set { SetValue(DateFormatProperty, value); }
        }

        public DateTime MinimumDate
        {
            get { return Convert.ToDateTime(GetValue(MinimumDateProperty)); }
            set { SetValue(MinimumDateProperty, value); }
        }

        public DateTime MaximumDate
        {
            get { return Convert.ToDateTime(GetValue(MaximumDateProperty)); }
            set { SetValue(MaximumDateProperty, value); }
        }

        #endregion

        #region DependencyProperties
        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.Register(
            "DateFormat",
            typeof(string),
            typeof(PosDatePicker),
            new FrameworkPropertyMetadata("yyyy-MM-dd HH:mm", OnDateFormatChanged),
                            new ValidateValueCallback(ValidateDateFormat));


        public static DependencyProperty MaximumDateProperty =
          DependencyProperty.Register("MaximumDate",
                                      typeof(DateTime), typeof(PosDatePicker),
                                      new FrameworkPropertyMetadata(DateTime.Parse("2045-01-01 00:00"),
                                    null,
                                      new CoerceValueCallback(CoerceMaxDate)));


        public static DependencyProperty MinimumDateProperty =
          DependencyProperty.Register("MinimumDate",
                                      typeof(DateTime), typeof(PosDatePicker),
                                      new FrameworkPropertyMetadata(DateTime.Parse("1900-01-01 00:00"),
                                      null,
                                      new CoerceValueCallback(CoerceMinDate)));

        public static readonly DependencyProperty SelectedDateProperty =
                               DependencyProperty.Register("SelectedDate",
                               typeof(DateTime?),
                               typeof(PosDatePicker),
                               new FrameworkPropertyMetadata(DateTime.Now,
                               new PropertyChangedCallback(OnSelectedDateChanged),
                               new CoerceValueCallback(CoerceDate)));


        #endregion


        #region Events

        public event RoutedEventHandler DateChanged
        {
            add
            {
                AddHandler(DateChangedEvent, value);
            }
            remove
            {
                RemoveHandler(DateChangedEvent, value);
            }
        }

        public static readonly RoutedEvent DateChangedEvent =
           EventManager.RegisterRoutedEvent(
           "DateChanged",
           RoutingStrategy.Bubble,
           typeof(RoutedEventHandler),
           typeof(PosDatePicker));


        public event RoutedEventHandler DateFormatChanged
        {
            add
            {
                this.AddHandler(DateFormatChangedEvent, value);
            }

            remove
            {
                this.RemoveHandler(DateFormatChangedEvent, value);
            }
        }


        public static readonly RoutedEvent DateFormatChangedEvent =
            EventManager.RegisterRoutedEvent(
            "DateFormatChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PosDatePicker));

        #endregion

        public static bool ValidateDateFormat(object par)
        {
            string s = Convert.ToString(par);
            DateTime result = DateTime.Now;
            return DateTime.TryParse(result.ToString(s), out result);
        }

        private int SelectNextPosition(int selstart)
        {
            return SelectPosition(selstart, Direction.Next);
        }

        private int SelectPreviousPosition(int selstart)
        {
            return SelectPosition(selstart, Direction.Previous);
        }

        private int SelectPosition(int selstart, Direction direction)
        {
            int retval = 0;

            if ((selstart > 0 || direction == Direction.Next) &&
                ((selstart < DateFormat.Length - FormatLengthOfLast) ||
                 direction == Direction.Previous))
            {
                char firstchar = Convert.ToChar(DateFormat.Substring(selstart, 1));
                char nextchar = Convert.ToChar(DateFormat.Substring(selstart + (int)direction, 1));
                bool found = false;

                while ((nextchar == firstchar || !Char.IsLetter(nextchar))
                        && (selstart > 1 || direction > 0)
                        && (selstart < DateFormat.Length - 2 || direction == Direction.Previous))
                {
                    selstart += (int)direction;
                    nextchar = Convert.ToChar(DateFormat.Substring(selstart + (int)direction, 1));
                }

                if (selstart > 1)
                    found = true;
                selstart = DateFormat.IndexOf(nextchar);

                if (found)
                {
                    retval = selstart;
                }
            }
            else
                retval = -1;
            return retval;
        }

        private void FocusOnDatePart(int selstart)
        {
            if (selstart > DateFormat.Length - 1)
                selstart = DateFormat.Length - 1;
            char firstchar = Convert.ToChar(DateFormat.Substring(selstart, 1));

            selstart = DateFormat.IndexOf(firstchar);
            int sellength = Math.Abs((selstart - (DateFormat.LastIndexOf(firstchar) + 1)));
            DateDisplay.Focus();
            DateDisplay.Select(selstart, sellength);
        }

        private DateTime Increase(int selstart, int value)
        {
            DateTime retval = Convert.ToDateTime(DateDisplay.Text);
            try
            {
                switch (DateFormat.Substring(selstart, 1))
                {
                    case "H":
                    case "h":
                        retval = retval.AddHours(value);
                        break;
                    case "y":
                        retval = retval.AddYears(value);
                        break;
                    case "M":
                        retval = retval.AddMonths(value);
                        break;
                    case "m":
                        retval = retval.AddMinutes(value);
                        break;
                    case "d":
                        retval = retval.AddDays(value);
                        break;
                    case "s":
                        retval = retval.AddSeconds(value);
                        break;
                }
            }
            catch (ArgumentException)
            {
                throw new Exception("Catch dates with year over 9999 etc, dont throw");
            }

            return retval;
        }

        private void DateDisplay_GotFocus(object sender, RoutedEventArgs e)
        {
            //DateDisplay.Clear();
            DateDisplay.SelectAll();
        }

        private void CalDisplay_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var time = TimeDisplay.Value.Value.TimeOfDay;
            TimeDisplay.Value = DateTime.Parse(CalDisplay.SelectedDate.GetValueOrDefault().ToString("dd/MM/yyyy") + " " + time.Hours.ToString() + ":" + time.Minutes.ToString());
            PopUpCalendarButton.IsChecked = false;
            SelectedDate = TimeDisplay.Value;
        }

        private void DateDisplay_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //var selstart = DateDisplay.SelectionStart;
            //FocusOnDatePart(selstart);
        }

        private void DateDisplay_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime result;
            while ((!DateTime.TryParse(DateDisplay.Text, out result)
                || (Convert.ToDateTime(DateDisplay.Text) < MinimumDate)
                || (Convert.ToDateTime(DateDisplay.Text) > MaximumDate))
                && DateDisplay.CanUndo)
            {
                DateDisplay.Undo();
            }

            if (DateTime.TryParse(DateDisplay.Text, out result) &&
                SelectedDate != Convert.ToDateTime(DateDisplay.Text))
            {
                SelectedDate = Convert.ToDateTime(DateDisplay.Text);
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Back)
            {
                if (DateDisplay.Text.Length > 0)
                    DateDisplay.Text.Remove(DateDisplay.Text.Length - 1);
                //DateDisplay.Clear();
                return;
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                return;

            DateTime dateResult;
            var selstart = DateDisplay.SelectionStart;
            while (!DateTime.TryParse(DateDisplay.Text, out dateResult))
            {
                DateDisplay.Undo();
            }

            e.Handled = true;
            switch (e.Key)
            {
                case Key.Up:
                    SelectedDate = Increase(selstart, 1);
                    FocusOnDatePart(selstart);
                    break;
                case Key.Down:
                    SelectedDate = Increase(selstart, -1);
                    FocusOnDatePart(selstart);
                    break;
                case Key.Left:
                    selstart = SelectPreviousPosition(selstart);
                    if (selstart > -1)
                        FocusOnDatePart(selstart);
                    else
                        this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                    break;
                case Key.Tab:
                case Key.Right:
                    selstart = SelectNextPosition(selstart);
                    if (selstart > -1)
                        FocusOnDatePart(selstart);
                    else
                        PopUpCalendarButton.Focus();
                    break;
                default:
                    try
                    {
                        if (!Char.IsDigit(Convert.ToChar(e.KeyboardDevice.ToString())))
                        {
                            if (e.Key == Key.D0 ||
                            e.Key == Key.D1 ||
                            e.Key == Key.D2 ||
                            e.Key == Key.D3 ||
                            e.Key == Key.D4 ||
                            e.Key == Key.D5 ||
                            e.Key == Key.D6 ||
                            e.Key == Key.D7 ||
                            e.Key == Key.D8 ||
                            e.Key == Key.D9)
                                e.Handled = false;
                        }
                        break;
                    }
                    catch { break; }

            }
        }

        private static object CoerceDate(DependencyObject d, object value)
        {
            PosDatePicker dtpicker = (PosDatePicker)d;
            DateTime current = Convert.ToDateTime(value);
            if (current < dtpicker.MinimumDate)
                current = dtpicker.MinimumDate;

            if (current > dtpicker.MaximumDate)
                current = dtpicker.MaximumDate;

            return current;
        }

        private static object CoerceMinDate(DependencyObject d, object value)
        {
            PosDatePicker dtpicker = (PosDatePicker)d;
            DateTime current = Convert.ToDateTime(value);
            if (current >= dtpicker.MaximumDate)
                throw new ArgumentException("MinimumDate can not be equal to, or more than maximum date");
            if (current > dtpicker.SelectedDate)
                dtpicker.SelectedDate = current;

            return current;
        }

        private static object CoerceMaxDate(DependencyObject d, object value)
        {
            PosDatePicker dtpicker = (PosDatePicker)d;
            DateTime current = Convert.ToDateTime(value);
            if (current <= dtpicker.MinimumDate)
                throw new ArgumentException("MaximimumDate can not be equal to, or less than MinimumDate");
            if (current < dtpicker.SelectedDate)
                dtpicker.SelectedDate = current;

            return current;
        }

        public static void OnDateFormatChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PosDatePicker dtp = (PosDatePicker)obj;
            if (dtp.DateFormat.Contains("H") || dtp.DateFormat.Contains("h"))
                dtp.TimeDisplay.Visibility = Visibility.Visible;
            dtp.DateDisplay.Text = dtp.SelectedDate.Value.ToString(dtp.DateFormat);
        }

        public static void OnSelectedDateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PosDatePicker dtp = (PosDatePicker)obj;

            if (args.NewValue == null)
            {
                dtp.SelectedDate = Convert.ToDateTime(args.NewValue);
                dtp.DateDisplay.Text = "Tarih Hatalı";
            }
            else
            {
                dtp.DateDisplay.Text = Convert.ToDateTime(args.NewValue).ToString(dtp.DateFormat);
                dtp.TimeDisplay.Value = Convert.ToDateTime(args.NewValue);
                dtp.CalDisplay.SelectedDate = Convert.ToDateTime(args.NewValue);
                dtp.CalDisplay.DisplayDate = Convert.ToDateTime(args.NewValue);

            }
        }

        private string _CleanText
        {
            get
            {
                var str = Regex.Replace(DateDisplay.Text, "[^0-9]", "");
                return str;
                //return str.Replace(".", " ").Replace(" ", "").Replace(":", "");
            }
        }

        private void DateDisplay_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var result = DateTime.ParseExact(_CleanText, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            //DateDisplay.Text = result;
            //DateDisplay.SelectionStart = DateDisplay.Text.Length;
            //return;
            var result = _CleanText;
            if (DateFormat.Contains("H") || DateFormat.Contains("h"))
                result = result.Length > 12 ? result.Substring(0, 12) : result;
            else
                result = result.Length > 8 ? result.Substring(0, 8) : result;

            if (CalendarPopup.IsOpen)
                CalendarPopup.IsOpen = false;
            string separator = ".";
            if (result.Length > 2) result = result.Insert(2, separator);
            if (result.Length > 5) result = result.Insert(5, separator);

            if (DateFormat.Contains("H") || DateFormat.Contains("h"))
            {
                if (result.Length > 10) result = result.Insert(10, " ");
                if (result.Length > 13) result = result.Insert(13, ":");
            }
            DateDisplay.Text = result;
            //var str = DateDisplay.Text;


            //if (str.Length == 2)
            //{
            //    DateDisplay.Text += separator;
            //}
            //else if (str.Length == 5)
            //{
            //    DateDisplay.Text += separator;
            //}

            //if (str.Length >= 10)
            //{
            //    if (DateFormat.Contains("H") || DateFormat.Contains("h"))
            //    {
            //        if (str.Contains(" "))
            //        {
            //            if (DateDisplay.Text.Length == 13)
            //                DateDisplay.Text += ":";
            //        }
            //        else
            //            DateDisplay.Text += " ";
            //    }
            //}

            DateDisplay.SelectionStart = DateDisplay.Text.Length;
        }

        private void CalendarPopup_MouseLeave_1(object sender, MouseEventArgs e)
        {
            SelectedDate = TimeDisplay.Value;
            //CalDisplay_SelectedDatesChanged(null, null);
        }

        private void PopUpCalendarButton_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DateDisplay_LostFocus(null, null);
        }

        private void DateDisplay_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {

        }
    }
}
