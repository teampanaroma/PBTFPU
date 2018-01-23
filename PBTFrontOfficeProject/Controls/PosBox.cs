using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PBTFrontOfficeProject.Controls
{

    public enum TextTypes
    {
        None = 0,
        Numeric = 2,
        Decimal = 4,
        Money = 8,
        Barcode = 16,
        MBSifre = 32,
        IPAddress = 64,
        Password = 128,
    }


    /// <summary>
    /// Özel dizayn textbox
    /// Format verilerek numeric, alfanumeric 
    /// Max min uzunluk verilebilir
    /// 
    /// </summary>
    public class PosBox : TextBox
    {
        public static DependencyProperty TextTypesProperty =
        DependencyProperty.Register(
        "TextType",
        typeof(TextTypes),
        typeof(PosBox),
        new PropertyMetadata(TextTypes.None));
        public TextTypes TextType
        {
            get { return (TextTypes)GetValue(TextTypesProperty); }
            set { SetValue(TextTypesProperty, value); }
        }


        public static DependencyProperty MaximumProperty =
        DependencyProperty.Register(
        "Maximum",
        typeof(decimal),
        typeof(PosBox),
        new PropertyMetadata(decimal.MaxValue));
        public decimal Maximum
        {
            get { return (decimal)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static DependencyProperty MinimumProperty =
        DependencyProperty.Register(
        "Minimum",
        typeof(decimal),
        typeof(PosBox),
        new PropertyMetadata(decimal.MinValue));
        public decimal Minimum
        {
            get { return (decimal)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static DependencyProperty ValueProperty =
        DependencyProperty.Register(
        "Value",
        typeof(object),
        typeof(PosBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty DecimalLenghtProperty =
        DependencyProperty.Register(
        "DecimalLenght",
        typeof(int),
        typeof(PosBox),
        new PropertyMetadata(null));
        public int DecimalLenght
        {
            get { return (int)GetValue(DecimalLenghtProperty); }
            set { SetValue(DecimalLenghtProperty, value); }
        }

        public static DependencyProperty CleanTextProperty =
        DependencyProperty.Register(
        "CleanText",
        typeof(string),
        typeof(PosBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string CleanText
        {
            get { return (string)GetValue(CleanTextProperty); }
            set { SetValue(CleanTextProperty, value); }
        }

        static PosBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PosBox), new FrameworkPropertyMetadata(typeof(PosBox)));
        }

        public PosBox()
        {

        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            if ((TextType & TextTypes.Password) == TextTypes.Password)
            {
                if (string.IsNullOrEmpty(Text))
                    CleanText = Text;

                if (!string.IsNullOrEmpty(CleanText))
                {
                    var tmpSStart = SelectionStart;
                    var tmpSLenght = SelectionLength;
                    Text = CleanText;
                    SelectionStart = tmpSStart;
                    SelectionLength = tmpSLenght;
                }
            }
            e.Handled = true;
            switch (TextType)
            {
                case TextTypes.Barcode:
                case TextTypes.None:
                    e.Handled = false;
                    base.OnTextInput(e);
                    Value = Text;
                    break;
                case TextTypes.Numeric:
                case TextTypes.Numeric | TextTypes.Password:
                    e.Handled = !CheckNumeric(e, true);
                    base.OnTextInput(e);
                    Value = Text;
                    break;
                case TextTypes.Decimal:
                case TextTypes.Money:
                    e.Handled = false;
                    if (CheckNumeric(e) || e.Text == Separator)
                        DecimalInput(e);
                    break;
                case TextTypes.MBSifre:
                    e.Handled = !CheckHex(e);
                    base.OnTextInput(e);
                    break;
                case TextTypes.IPAddress:
                    this.MaxLength = 15;
                    e.Handled = !CheckIpAddress(e);// !CheckNumeric(e, false) && !e.Text.Contains(",");
                    if (!e.Handled && e.Text.Contains(","))
                    {
                        var index = SelectionStart;
                        Text = Text.Insert(SelectionStart, ".");
                        SelectionStart = index + 1;
                    }
                    else
                        base.OnTextInput(e);
                    break;
                default:
                    e.Handled = true;
                    break;
            }

            if ((TextType & TextTypes.Password) == TextTypes.Password)
            {
                CleanText = Text;
                var tmpSStart = SelectionStart;
                Text = string.Empty.PadLeft(CleanText.Length, '•');
                SelectionStart = tmpSStart;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            SelectionStart = Text.Length;
            base.OnGotFocus(e);
        }

        private bool CheckIpAddress(TextCompositionEventArgs e)
        {
            var count = Text.Count(c => c == '.');
            if (e.Text.Contains(",") && count == 3)
                return false;

            return !(!CheckNumeric(e, false) && !e.Text.Contains(","));
        }

        private bool CheckHex(TextCompositionEventArgs e)
        {
            char C = e.Text[0];
            var result = C >= '0' && C <= 'F';
            return result;
        }

        //private int LastSelectionStart;
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if ((TextType & TextTypes.Password) == TextTypes.Password)
            {
                try
                {
                    var change = e.Changes.FirstOrDefault();
                    if (change != null && Text != null && change.RemovedLength > 0 && change.AddedLength == 0)
                        CleanText = CleanText.Remove(change.Offset, change.RemovedLength);
                }
                catch { }
            }
            //Value = Text.Replace(".", "");
            switch (TextType)
            {
                case TextTypes.None:
                    break;
                case TextTypes.Numeric:
                    break;
                case TextTypes.Money:
                case TextTypes.Decimal:
                    if (isPressSeparator)
                        isPressSeparator = Text.Contains(Separator);
                    int separatorIndex = Text.IndexOf(Separator);
                    if (!isPressSeparator)
                        SelectionStart = separatorIndex < 0 ? Text.Length : separatorIndex;
                    else
                        SelectionStart = Text.Length;
                    break;
                default:
                    break;
            }
        }
        bool CheckNumeric(TextCompositionEventArgs e)
        {
            return CheckNumeric(e, true);
        }
        private bool CheckNumeric(TextCompositionEventArgs e, bool decimalControl)
        {
            try
            {
                var result = true;
                result &= char.IsDigit(e.Text[0]);
                if (!decimalControl)
                    return result;
                if (result)
                {
                    decimal num = 0;
                    result &= decimal.TryParse((Text + e.Text), out num);
                    //if (result)
                    //    result &= num <= 99999999999.9999M; //TODO: BURADA KISIT KONULMAMALI!
                    result &= num <= Maximum;
                    result &= num >= Minimum;
                }
                return result;
            }
            catch
            {
                return false;
            }
        }

        bool isPressSeparator = false;
        private readonly string Separator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
        private bool _IsZeroPress;
        private void DecimalInput(TextCompositionEventArgs e)
        {
            if (TextType == TextTypes.Decimal && DecimalLenght==0)
                DecimalLenght = 3;

            if (Maximum == 0)
                Maximum = 999999999.999M;
            if (e.Text.Contains(Separator))
            {
                if (!Text.Contains(Separator))
                    base.OnTextInput(e);
                isPressSeparator = true;
                SelectionStart = Text.Length;
                return;
            }

            if (!string.IsNullOrEmpty(Text))
            {
                if (TextType == TextTypes.Money)
                {
                    Text = Text.Replace(Separator + "00", Separator);
                    DecimalLenght = DecimalLenght == 0 ? 2 : DecimalLenght;
                    if (!e.Text.Contains("0"))
                    {
                        if (!_IsZeroPress)
                        {
                            Text = Text.Replace(Separator + "00", Separator);
                            if (Text.Contains(Separator))
                            {
                                if (Text.Last() == '0')
                                    Text = Text.Substring(0, Text.Length - 1);
                            }
                        }

                        _IsZeroPress = false;
                    }
                    else
                        _IsZeroPress = true;
                }
            }

            var LastText = Text;
            base.OnTextInput(e);
            int separatorIndex = Text.IndexOf(Separator);

            if (!isPressSeparator)
            {
                SelectionStart = separatorIndex < 0 ? Text.Length : separatorIndex;
            }
            else
            {
                SelectionStart = Text.Length;
            }

            decimal DataValue = decimal.Parse(Text);
            decimal decVal = 0;
            if (separatorIndex != -1)
            {
                if (Text.Substring(separatorIndex, Text.Length - separatorIndex) == Separator + "000")
                {
                    Text = LastText;
                    return;
                }
                var decStr = Text.Substring(separatorIndex, Text.Length - separatorIndex);
                if (Text.EndsWith("00"))
                    decStr = Text.Substring(separatorIndex, Text.Length - separatorIndex).Replace(Separator + "00", Separator);
                if (decStr.Length - 1 > DecimalLenght)
                {
                    Text = LastText;
                    return;
                }
                if (TextType == TextTypes.Money && decStr.Replace(Separator, "").Length > 2)
                {
                    Text = LastText;
                    return;
                }

                decVal = decStr.Length > 1 ? decimal.Parse(decStr) : 0;
            }
            decimal intVal = DataValue - decVal;
            if (DataValue > Maximum)
            {
                Text = LastText;
                return;
            }
            Value = DataValue;
            if (TextType == TextTypes.Money)
            {
                if (!isPressSeparator)
                    Text = DataValue.ToString("N2");
                if (decVal != 0)
                    Text = DataValue.ToString("N2");
            }
            //else if(TextType == TextTypes.Decimal)
            //{
            //    var format = string.Format("#.{0}", "".PadRight(DecimalLenght, '0'));
            //    Text = DataValue.ToString(format);
            //}
        }


    }
}
