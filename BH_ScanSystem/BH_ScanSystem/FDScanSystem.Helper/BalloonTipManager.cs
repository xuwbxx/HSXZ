

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace FDScanSystem.Helper
{
    public class BalloonTipMessage
    {
        private string _title;

        public string TipTitle
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _text;

        public string TipText
        {
            get { return _text; }
            set
            {
                if (value == null || value.Trim() == string.Empty)
                {
                    throw new ArgumentException(null, "TipText");
                }
                _text = value;
            }
        }

        private ToolTipIcon _icon;

        public ToolTipIcon TipIcon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public BalloonTipMessage(string tipTitle, string tipText, ToolTipIcon tipIcon)
        {
            TipText = tipText;
            TipTitle = tipTitle;
            TipIcon = tipIcon;
        }

        public BalloonTipMessage(string tipText, ToolTipIcon tipIcon)
            : this(null, tipText, tipIcon)
        {
        }

        public BalloonTipMessage(string tipText)
            : this(null, tipText, ToolTipIcon.None)
        {
        }
    }

    public class BalloonTipManager : IDisposable
    {
        private NotifyIcon _notifyIcon;
        private Queue<BalloonTipMessage> _msgQueue;
        private System.Threading.Timer _checkTimer = null;
        private int _timeoutSeconds;
        private const int MaxQueueLength = 1000;
        private const int DefaultBalloonTipTimeoutSeconds = 10;

        public BalloonTipManager(NotifyIcon notifyIcon)
            : this(notifyIcon, DefaultBalloonTipTimeoutSeconds)
        {
        }

        public BalloonTipManager(NotifyIcon notifyIcon, int timeoutSeconds)
        {
            _notifyIcon = notifyIcon;
            _timeoutSeconds = timeoutSeconds;
            _msgQueue = new Queue<BalloonTipMessage>();
            _checkTimer = new System.Threading.Timer(new TimerCallback(TimerCallback), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public void ShowInfo(BalloonTipMessage msg)
        {
            lock (_msgQueue)
            {
                _msgQueue.Enqueue(msg);
                if (_msgQueue.Count > MaxQueueLength)
                    _msgQueue.Dequeue();
            }
        }

        #region IDisposable ³ÉÔ±

        public void Dispose()
        {
            if (_checkTimer != null)
            {
                _checkTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _checkTimer.Dispose();
                _checkTimer = null;
            }
        }

        #endregion

        ~BalloonTipManager()
        {
            Dispose();
        }

        private void TimerCallback(object state)
        {
            _checkTimer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                BalloonTipMessage msg = null;
                lock (_msgQueue)
                {
                    if (_msgQueue.Count > 0)
                        msg = _msgQueue.Dequeue();
                }
                if (msg != null)
                {
                    _notifyIcon.ShowBalloonTip(_timeoutSeconds, msg.TipTitle, msg.TipText, msg.TipIcon);
                }
            }
            finally
            {
                _checkTimer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            }
        }
    }
}
