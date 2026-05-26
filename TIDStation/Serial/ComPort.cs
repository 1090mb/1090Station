using System.Diagnostics;
using System.IO.Ports;

namespace TIDStation.Serial
{
    public class ComPort
    {
        private volatile bool active;
        public bool Active => active;

        private readonly SerialPort port = null!;
        private readonly Task loopTask = null!;
        private readonly Action<byte[]> callBack;

        public ComPort(int number, int baud, Parity parity, int bits, StopBits stopbits, Action<byte[]> callBack)
        {
            this.callBack = callBack;
            try
            {
                port = new($"COM{number}", baud, parity, bits, stopbits);
                port.Open();
                active = true;
                loopTask = Task.Run(Loop);
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ComPort open failed: {ex.Message}");
            }
            Close();
        }

        public void Close()
        {
            active = false;
            try { port.ReadTimeout = 100; } catch { }
            try { port.WriteTimeout = 100; } catch { }
            try { port.Close(); } catch { }
            try { port.Dispose(); } catch { }
            using (loopTask)
                loopTask?.Wait();
        }

        public void Send(byte[] data) => Send(data, 0, data.Length);
        public void Send(byte[] data, int len) => Send(data, 0, len);
        public void Send(byte[] data, int offset, int len)
        {
            try
            {
                port.Write(data, offset, len);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ComPort send failed: {ex.Message}");
            }
        }

        public void Send(byte byt)
        {
            Send([byt], 0, 1);
        }

        private void Loop()
        {
            while (active)
            {
                byte[] bytes = new byte[32768];
                int br;
                try { br = port.Read(bytes, 0, 32768); } catch { br = -1; }
                if (br <= 0)
                    break;
                callBack(bytes[..br]);
            }
        }
    }
}
