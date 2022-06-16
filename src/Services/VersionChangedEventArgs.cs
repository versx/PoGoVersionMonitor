namespace PogoVersionMonitor.Services
{
    using System;

    public class VersionChangedEventArgs
    {
        /// <summary>
        /// Gets the current version.
        /// </summary>
        public Version Current { get; }

        /// <summary>
        /// Gets the latest version.
        /// </summary>
        public Version Latest { get; }

        /// <summary>
        /// Gets a value determining whether the version was reverted
        /// to a previous version.
        /// </summary>
        public bool IsRevert { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="latest"></param>
        /// <param name="isRevert"></param>
        public VersionChangedEventArgs(Version current, Version latest, bool isRevert)
        {
            Current = current;
            Latest = latest;
            IsRevert = isRevert;
        }
    }
}