using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TIDStation.General;
using TIDStation.Serial;

namespace TIDStation.UI
{
    public class Frequency : Label
    {
        private const double shift = 335.54432;
        public static Frequency Default { get; set; } = null!;
        public static Frequency Current { get; set; } = null!;

        private Brush? foreG = null;

        private bool inputMode = false;
        private string Text
        {
            get => (Content is string s) ? s.Trim() : string.Empty;
            set
            {
                Content = $"{value}         "[..9];
            }
        }

        public Frequency() : base()
        {
        }

        private void EndInput()
        {
            double clampLo = 18.0 + (Comms.ShiftMode ? shift : 0.0);
            double clampHi = Comms.ShiftMode ? 999.99999 : 670.0;
            double fr = (double.TryParse(Text, out double d) ? d : Value + (Comms.ShiftMode ? shift : 0.0)).Clamp(clampLo, clampHi);
            if (Comms.ShiftMode) fr -= shift;
            Value = fr;
            Refresh();
            inputMode = false;
            Default?.Select();
        }

        public void Select()
        {
            if (!IsEditable) return;
            if (Current != null && Current.foreG != null)
                Current.Foreground = Current.foreG;
            foreG ??= Foreground;
            if (!this.Equals(Default))
                Foreground = SelectedBrush;
            Current = this;
        }
        

        public void Refresh()
        {
            double fr = Value;
            if(Comms.ShiftMode) fr += shift;
            Text = fr.ToString("000.00000");
        }

        private long lastKey = -1;
        private bool timing = false;
        private async Task Timer()
        {
            lastKey = DateTime.Now.Ticks;
            if (timing) return;
            timing = true;
            do
            {
                await Task.Delay(20);
                if (timing)
                {
                    long span = (DateTime.Now.Ticks - lastKey) / 10000L;
                    if (span > InputTimeout)
                    {
                        KeyIn(Key.Escape);
                    }
                }
            }
            while (timing);
        }

        private static Key NormalizeNumPad(Key k)
        {
            const int numPadOffset = Key.NumPad0 - Key.D0;
            return k >= Key.NumPad0 && k <= Key.NumPad9 ? k - numPadOffset : k;
        }

        public void KeyIn(Key k)
        {
            if (inputMode)
            {
                k = NormalizeNumPad(k);
                Tasks.Watch = Timer();
                switch (k)
                {
                    case Key.Escape:
                        Text = string.Empty;
                        EndInput();
                        break;
                    case Key.Back:
                    case Key.Delete:
                        Text = Text[..^1];
                        if(Text.EndsWith('.'))
                            Text = Text[..^1];
                        if (Text.Length == 0)
                            EndInput();
                        break;
                    case Key.Enter:
                        EndInput();
                        break;
                    case >= Key.D0 and <= Key.D9:
                        if (Text.Length == 3 && !Text.Contains('.'))
                            Text += '.';
                        Text += k - Key.D0;
                        if (Text.Length >= 9)
                            EndInput();
                        break;
                    case Key.OemPeriod:
                    case Key.OemComma:
                    case Key.Decimal:
                        if (!Text.Contains('.'))
                        {
                            Text += '.';
                            if (Text.Length == 1)
                                Text = "000" + Text;
                            else if (Text.Length == 2)
                                Text = "00" + Text;
                            else if (Text.Length == 3)
                                Text = "0" + Text;
                        }
                        break;
                }
            }
            else
            {
                if (k == Key.Escape || k == Key.Back || k == Key.Delete)
                {
                    EndInput();
                }
                else
                {
                    k = NormalizeNumPad(k);
                    if (k >= Key.D0 && k <= Key.D9)
                    {
                        inputMode = true;
                        Text = string.Empty;
                        KeyIn(k);
                    }
                }
            }
        }



        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }
        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(Frequency), new PropertyMetadata(true, IsEditableChanged));

        private static void IsEditableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is Frequency freq)
            {
                freq.Opacity = ((bool)e.NewValue) ? 1 : 0.35;
            }
        }

        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(Frequency), new PropertyMetadata(new SolidColorBrush(Colors.Yellow)));

        public int InputTimeout
        {
            get { return (int)GetValue(InputTimeoutProperty); }
            set { SetValue(InputTimeoutProperty, value); }
        }
        public static readonly DependencyProperty InputTimeoutProperty =
            DependencyProperty.Register("InputTimeout", typeof(int), typeof(Frequency), new PropertyMetadata(5000));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Frequency), new PropertyMetadata(18.0, ValueChanged));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is Frequency freq && e.NewValue is double dbl)
            {
                if (Comms.ShiftMode) dbl += shift;
                freq.Text = $"{dbl:000.00000}";
            }
        }
    }
}
