namespace PogoVersionMonitor.Services
{
    using System;

    public class VersionChangedEventArgs
    {
        public Version Current { get; }

        public Version Latest { get; }

        public bool IsRevert { get; }

        public VersionChangedEventArgs(Version current, Version latest, bool isRevert)
        {
            Current = current;
            Latest = latest;
            IsRevert = isRevert;
        }
    }
}