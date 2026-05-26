using System.Diagnostics;

namespace TIDStation.General
{
    public static class Util
    {
        public static void SyncSignal(this object obj)
        {
            lock (obj) Monitor.PulseAll(obj);
        }

        public static bool Wait(this object obj, int timeOut = 1000)
        {
            return Monitor.Wait(obj, timeOut);
        }

        public static int Clamp(this int val, int min, int max)
        {
            return Math.Clamp(val, min, max);
        }

        public static double Clamp(this double val, double min, double max)
        {
            return Math.Clamp(val, min, max);
        }

        public static void Write16BE(this int ushrt, byte[] array, int offset)
        {
            array[offset] = (byte)((ushrt >> 8) & 0xff);
            array[offset + 1] = (byte)(ushrt & 0xff);
        }

        public static string Truncate(this string str, int length) => str.Length > length ? str[..length] : str;
    }
}
