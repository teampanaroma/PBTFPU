using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace PBTFrontOfficeProject.Service
{
    /// <summary>
    /// Interaction logic for DateTimeControl.xaml
    /// </summary>
    public partial class DateTimeControl : UserControl
    {
        /// <summary>
        /// Contains all times betwenn 00:00 and 23:30
        /// </summary>
        List<string> _times = new List<string>();

        /// <summary>
        /// Contains the ValueProperty dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.RegisterAttached(
                "Value",
                typeof(DateTime?),
                typeof(DateTimeControl), new PropertyMetadata(new PropertyChangedCallback(ValueProperty_PropertyChanged)));

        /// <summary>
        /// Executed allways when the value property changes
        /// </summary>
        /// <param name="obj">the DateTimeControl</param>
        /// <param name="e">the event args</param>
        static void ValueProperty_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            DateTimeControl control = (DateTimeControl)obj;
            control.UpdateValue();
        }

        /// <summary>
        /// Updates the control values
        /// </summary>
        void UpdateValue()
        {
            try
            {
                // we do not want change events from controls during the Value update
                UnregisterEvents();

                // Update the selected date of the DatePicker
                this.date.SelectedDate = this.Value;

                // do we have a time set in the TextBox?
                if (this.Value != null)
                {
                    var value = this.Value.Value;
                    var sTime = string.Format("{0:00}:{1:00}", value.Hour, value.Minute);
                    var item = _times.FirstOrDefault(t => t == sTime);
                    if (item != null)
                    {
                        time.SelectedItem = item;
                    }
                    else
                    {
                        var hour = string.Format("{0:00}", value.Hour);
                        item = _times.FirstOrDefault(t => t.StartsWith(hour));
                        time.SelectedItem = item; // set the selected item to the full hour
                        time.Text = sTime; // set the time
                    }
                }
                else // no time -> use default
                {
                    this.time.Text = "00:00";
                }
            }
            finally
            {
                // start listen to control events again
                RegisterEvents();
            }
        }

        /// <summary>
        /// Gets or sets the date time value
        /// </summary>
        public DateTime? Value
        {
            get
            {
                return (DateTime?)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);

            }
        }

        /// <summary>
        /// Initializes a new instance of DateTimeControl class
        /// </summary>
        public DateTimeControl()
        {
            InitializeComponent();


            for (int i = 0; i < 30 * 2 * 24; i += 30)
            {
                _times.Add(DateTime.Today.AddMinutes(i).ToString("HH:mm"));
            }
            this.time.DataContext = _times;

            RegisterEvents();
        }

        /// <summary>
        /// Registers all control events
        /// </summary>
        void RegisterEvents()
        {
            this.date.SelectedDateChanged += date_SelectedDateChanged;
            this.time.SelectionChanged += time_SelectionChanged;
            this.time.LostFocus += time_LostFocus;
        }

        /// <summary>
        /// Unregisters all control events
        /// </summary>
        void UnregisterEvents()
        {
            this.date.SelectedDateChanged -= date_SelectedDateChanged;
            this.time.SelectionChanged -= time_SelectionChanged;
            this.time.LostFocus -= time_LostFocus;
        }

        /// <summary>
        /// Executed when the combobox textbox loses its focus
        /// </summary>
        /// <param name="sender">the combobox</param>
        /// <param name="e">the event args</param>
        void time_LostFocus(object sender, RoutedEventArgs e)
        {
            var newValue = ParseTimeText();

            if (this.Value != null)
            {
                // we do have a Value until now, so update the Value
                var timeSpan = TimeSpan.Parse(newValue);
                var updateValue = this.Value.Value.Date.Add(timeSpan);
                if (updateValue != this.Value)
                {
                    this.Value = updateValue;
                    return;
                }
            }

            // we do not have a full update. so only override the correct parsed time
            this.time.Text = newValue;
        }

        /// <summary>
        /// Parses the Time TextBlock text
        /// </summary>
        /// <returns>the parsed time</returns>
        string ParseTimeText()
        {
            var value = (string)this.time.Text;
            var newValue = string.Empty;
            if (string.IsNullOrEmpty(value))
            {
                newValue = "00:00";
            }
            else
            {
                string[] values = null;
                if (!value.Contains(":"))
                {

                    if (value.Length > 2)
                    {
                        value = value.Insert(2, ":");
                    }
                    else
                    {
                        value = value += ":";
                    }
                }
                values = value.Split(':');

                var hour = values[0];
                if (hour.Length > 2)
                    hour = hour.Substring(0, 2);

                var minute = values[1];
                if (minute.Length > 2)
                    minute = minute.Substring(0, 2);

                int iHour, iMinute;
                if (!int.TryParse(hour, out iHour))
                    iHour = 0;
                if (!int.TryParse(minute, out iMinute))
                    iMinute = 0;
                iHour = Math.Max(0, iHour);
                iHour = Math.Min(23, iHour);
                iMinute = Math.Max(0, iMinute);
                iMinute = Math.Min(59, iMinute);
                newValue = string.Format("{0:00}:{1:00}", iHour, iMinute);
            }
            return newValue;
        }

        /// <summary>
        /// Updates the time token of the Value property
        /// </summary>
        /// <param name="sender">the time TextBox</param>
        /// <param name="e">the event args</param>
        void time_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Value == null)
            {
                return;
            }

            var value = (string)this.time.SelectedValue;
            if (value != null)
            {
                var timeSpan = TimeSpan.Parse(value);
                this.Value = this.Value.Value.Date.Add(timeSpan);
            }
        }

        /// <summary>
        /// Updates the time token of the Value property
        /// </summary>
        /// <param name="sender">the time TextBox</param>
        /// <param name="e">the event args</param>
        void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var newValue = date.SelectedDate == null ? DateTime.Today : date.SelectedDate.Value.Date;

            // check if the time comobo does have a selected value
            var timeValue = (string)this.time.SelectedValue;
            if (timeValue != null)
            {
                var timeSpan = TimeSpan.Parse(timeValue);
                newValue = newValue.Add(timeSpan);
            }
            else
            {
                // parse the text of the time combo
                var timeText = ParseTimeText();
                var timeSpan = TimeSpan.Parse(timeText);
                newValue = newValue.Add(timeSpan);
            }

            // update the value
            this.Value = newValue;
        }
    }
}
