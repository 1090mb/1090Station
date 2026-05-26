using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TIDStation.General;

namespace TIDStation.UI
{
    public class NumEntry : Label
    {
        private bool inputMode = false;

        private string Text
        {
            get => (Content is string s) ? s.Trim() : string.Empty;
            set => Content = value;
        }

        private void EndInput()
        {
            int i = (int.TryParse(Text, out int d) ? d : Value).Clamp(Min, Max);
            Text = $"{i:D3}";
            inputMode = false;
            timing = false;
            Value = i;
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
                    if(span > InputTimeout)
                    {
                        KeyIn(Key.Escape);
                    }
                }
            }
            while (timing);
        }

        private static Key NormalizeNumPad(Key k)
        {
            return k >= Key.NumPad0 && k <= Key.NumPad9 ? k - (Key.NumPad0 - Key.D0) : k;
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
                        if (Text.Length == 0)
                            EndInput();
                        break;
                    case Key.Enter:
                        EndInput();
                        break;
                    case >= Key.D0 and <= Key.D9:
                        Text += k - Key.D0;
                        if (Text.Length >= 3)
                            EndInput();
                        break;
                }
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

        public int InputTimeout
        {
            get { return (int)GetValue(InputTimeoutProperty); }
            set { SetValue(InputTimeoutProperty, value); }
        }
        public static readonly DependencyProperty InputTimeoutProperty =
            DependencyProperty.Register("InputTimeout", typeof(int), typeof(NumEntry), new PropertyMetadata(5000));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumEntry), new PropertyMetadata(0));

        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(int), typeof(NumEntry), new PropertyMetadata(0));

        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(NumEntry), new PropertyMetadata(100));

    }
}
