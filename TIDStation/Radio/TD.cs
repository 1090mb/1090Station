using TIDStation.General;
using TIDStation.Serial;

namespace TIDStation.Radio
{
    public static class TD
    {
        private static volatile bool suspend;

        public static void Suspend()
        {
            suspend = true;
        }

        public static void Resume()
        {
            suspend = false;
        }

        public static void Update()
        {
            if (!suspend)
                Tasks.Watch = Comms.Commit();
        }
    }
}
