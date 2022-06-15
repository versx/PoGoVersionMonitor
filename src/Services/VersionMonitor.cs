namespace PogoVersionMonitor.Services
{
    using System;

    using PogoVersionMonitor.Diagnostics;
    using PogoVersionMonitor.Utilities;

    public class VersionMonitor
    {
        #region Variables

        private readonly IEventLogger _logger = new EventLogger(Program.OnLogEvent);
        private readonly System.Timers.Timer _timer = new();
        private Version _currentVersion;
        private Version _latestVersion;

        #endregion

        #region Properties

        public Version Latest => _latestVersion ??= GetLatest();

        public ushort CompareIntervalM { get; set; } = 5;

        public bool IsRunning => _timer?.Enabled ?? false;

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
        }

        #endregion

        #region Private Methods

        private void CheckVersionChanged()
        {
            // If current version is null, this is the initial startup, set to latest version
            if (_currentVersion == null)
            {
                _currentVersion = GetLatest();
            }

            _latestVersion = GetLatest();

            // Something happened and versions are 0.0
            if (_currentVersion == new Version(0, 0, 0) || _latestVersion == new Version(0, 0, 0))
                return;

            if (_currentVersion < _latestVersion)
            {
                // New version
                _logger.Debug($"New remote version changed from {_currentVersion} to {_latestVersion}");
                OnVersionChanged(_currentVersion, _latestVersion, false);
                _currentVersion = _latestVersion;
            }
            else if (_currentVersion > _latestVersion)
            {
                // Reverted
                _logger.Debug($"Remote version reverted from {_currentVersion} to {_latestVersion}");
                OnVersionChanged(_currentVersion, _latestVersion, true);
                _currentVersion = _latestVersion;
            }
            else
            {
                // Same version
            }
        }

        private Version GetLatest()
        {
            var version = "0.0.0";
            try
            {
                var data = Utils.GetRequest(RemoteEndPoint);
                if (!string.IsNullOrEmpty(data))
                {
                    version = data;
                    version = Utils.SanitizeString(version);
                }
                return new Version(version[1..]);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new Version(version);
            }
        }

        #endregion
    }
}