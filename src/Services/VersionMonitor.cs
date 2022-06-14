namespace PogoVersionMonitor.Services
{
    using System;

    using PogoVersionMonitor.Utilities;

    public class VersionMonitor
    {
        #region Variables

        private readonly System.Timers.Timer _timer = new();
        private Version _currentVersion;
        private Version _latestVersion;

        #endregion

        #region Properties

        public Version Latest
        {
            get
            {
                if (_latestVersion == null)
                {
                    _latestVersion = GetLatest();
                }

                return _latestVersion;
            }
        }

        public ushort CompareIntervalM { get; set; } = 5;

        public bool IsRunning => _timer?.Enabled ?? false;

        #endregion

        #region Events

        public event EventHandler<VersionChangedEventArgs> VersionChanged;

        private void OnVersionChanged(Version current, Version latest, bool isRevert)
        {
            VersionChanged?.Invoke(this, new VersionChangedEventArgs(current, latest, isRevert));
        }

        #endregion

        #region Constructor

        public VersionMonitor()
        {
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            // Skip starting timer if already started
            if (_timer.Enabled)
                return;

            _timer.Elapsed += (sender, e) => CheckVersionChanged();
            _timer.Interval = 15 * 1000 * CompareIntervalM;
            _timer.Start();
        }

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
                OnVersionChanged(_currentVersion, _latestVersion, false);
                _currentVersion = _latestVersion;
            }
            else if (_currentVersion > _latestVersion)
            {
                // Reverted
                OnVersionChanged(_currentVersion, _latestVersion, true);
                _currentVersion = _latestVersion;
            }
            else
            {
                // Same version
            }
        }

        #endregion

        #region Static Methods

        private static Version GetLatest()
        {
            var version = "0.0.0";
            try
            {
                var data = Utils.GetRequest(Strings.VersionEndPoint);
                if (!string.IsNullOrEmpty(data))
                {
                    version = data;
                    version = Utils.SanitizeString(version);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Version(version);
            }

            return new Version(version.Substring(1, version.Length - 1));
        }

        #endregion
    }
}