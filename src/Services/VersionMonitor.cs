namespace PogoVersionMonitor.Services
{
    using System;

    using PogoVersionMonitor.Diagnostics;
    using PogoVersionMonitor.Utilities;

    public class VersionMonitor
    {
        private static readonly Version DefaultVersion = new Version(0, 0, 0);

        #region Variables

        private readonly IEventLogger _logger = new EventLogger(Program.OnLogEvent);
        private readonly System.Timers.Timer _timer = new();
        private Version _currentVersion;
        private Version _latestVersion;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the latest version from the remote source.
        /// </summary>
        public Version Latest => _latestVersion ??= GetLatest();

        /// <summary>
        /// Gets the current version to compare against the latest
        /// remote source version.
        /// </summary>
        public Version Current => _currentVersion;

        /// <summary>
        /// Gets or sets a value for comparing versions in minutes
        /// at an interval.
        /// </summary>
        public ushort CompareIntervalM { get; set; } = 5;

        /// <summary>
        /// Gets a value determining whether the comparison timer is
        /// currently running or not.
        /// </summary>
        public bool IsRunning => _timer?.Enabled ?? false;

        /// <summary>
        /// Gets or sets the remote endpoint source to fetch and
        /// compare versions with.
        /// </summary>
        public string RemoteEndPoint { get; set; }

        #endregion

        #region Events

        public event EventHandler<VersionChangedEventArgs> VersionChanged;

        private void OnVersionChanged(Version current, Version latest, bool isRevert)
        {
            VersionChanged?.Invoke(this, new VersionChangedEventArgs(current, latest, isRevert));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiates a new <see cref="VersionMonitor"/> object.
        /// </summary>
        /// <param name="remoteEndPoint">Remote endpoint source to fetch and compare versions with.</param>
        public VersionMonitor(string remoteEndPoint)
        {
            RemoteEndPoint = remoteEndPoint;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start monitoring for version changes from remote source.
        /// </summary>
        public void Start()
        {
            // Skip starting timer if already started
            if (_timer.Enabled)
                return;

            _timer.Elapsed += (sender, e) => CheckVersionChanged();
            _timer.Interval = 60 * 1000 * CompareIntervalM;
            _timer.Start();

            _logger.Debug($"Version check timer started, interval set to {CompareIntervalM} minutes...");
        }

        /// <summary>
        /// Stop monitoring for version changes from remote source.
        /// </summary>
        public void Stop()
        {
            // Skip stopping timer if already stopped
            if (!_timer.Enabled)
                return;

            _timer.Stop();

            _logger.Debug($"Version check timer stopped...");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Version comparison callback method against remote and local versions.
        /// </summary>
        private void CheckVersionChanged()
        {
            // If current version is null, this is the initial startup, set to latest version
            if (_currentVersion == null)
            {
                _currentVersion = GetLatest();
            }

            // Fetch latest version from remote endpoint source
            _latestVersion = GetLatest();

            // Something happened and versions are 0.0.0
            if (_currentVersion == DefaultVersion || _latestVersion == DefaultVersion)
                return;

            // Compare current and latest remote versions against each other
            if (_currentVersion < _latestVersion)
            {
                // New version
                _logger.Debug($"Remote version changed from {_currentVersion} to {_latestVersion}");
                OnVersionChanged(_currentVersion, _latestVersion, false);
                _currentVersion = _latestVersion;
            }
            else if (_currentVersion > _latestVersion)
            {
                // Reverted version
                _logger.Debug($"Remote version reverted from {_currentVersion} to {_latestVersion}");
                OnVersionChanged(_currentVersion, _latestVersion, true);
                _currentVersion = _latestVersion;
            }
            else
            {
                // Same version
            }
        }

        /// <summary>
        /// Fetches latest version from remote endpoint source.
        /// </summary>
        /// <returns>Returns version from remote endpoint.</returns>
        private Version GetLatest()
        {
            // Default version placeholder incase of failure
            var version = "0.0.0";
            try
            {
                // Get version from remote endpoint source
                var data = Utils.GetRequest(RemoteEndPoint);
                // Ensure fetched data is not null or empty
                if (!string.IsNullOrEmpty(data))
                {
                    // Assign remote version and sanitize value to remove
                    // any unicode or unexpected characters
                    version = Utils.SanitizeString(data);
                    //version = version[1..];
                }
            }
            catch (Exception ex)
            {
                // Error occurred while fetching and preparing remote
                // endpoint version
                _logger.Error(ex);
            }
            // Wrap version string in System.Version object
            return new Version(version);
        }

        #endregion
    }
}