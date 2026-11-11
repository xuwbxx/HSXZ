
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace FDScanSystem.Helper {
    public class FormLogHelper : TextWriter {
        private RichTextBox _richTextBox;
        private object[,] _logFonts = new object[2, 6];
        private const int MaxLines = 10000;

        public FormLogHelper(RichTextBox richTextBox) {
            _richTextBox = richTextBox;
            _richTextBox.MaxLength = 0xFFFF;
            _logFonts[0, 0] = "FATAL";
            _logFonts[0, 1] = "ERROR";
            _logFonts[0, 2] = "WARN";
            _logFonts[0, 3] = "INFO";
            _logFonts[0, 4] = "DEBUG";
            _logFonts[0, 5] = "SUCCESS";
            for (int i = 0; i < _logFonts.GetLength(1); i++) {
                string setting = ConfigurationManager.AppSettings[_logFonts[0, i] as string];
                LogFont logfont;
                if (setting == null) {
                    logfont.Color = Color.Black;
                    logfont.Bold = false;
                }
                else {
                    string[] parts = setting.Split(',');
                    logfont.Color = Color.FromName(parts[0]);
                    logfont.Bold = (parts.Length > 1 && parts[1].ToLower().Equals("bold"));
                }
                _logFonts[1, i] = logfont;
            }
        }

        public override void Write(char c) {
            if (_richTextBox.InvokeRequired) {
                _richTextBox.Invoke(new MethodInvoker(delegate() {
                    _richTextBox.AppendText(c.ToString());
                    TrimLines();
                    _richTextBox.ScrollToCaret();
                }));
            }
            else {
                _richTextBox.AppendText(c.ToString());
                TrimLines();
                _richTextBox.ScrollToCaret();
            }
        }

        public override void Write(string str) {
            if (_richTextBox.InvokeRequired) {
                _richTextBox.Invoke(new MethodInvoker(delegate() {
                    Color oldColor = _richTextBox.SelectionColor;
                    Font oldFont = _richTextBox.SelectionFont;
                    for (int i = 0; i < _logFonts.GetLength(1); i++) {
                        if (_logFonts[1, i] != null && str.IndexOf(_logFonts[0, i] as string) >= 0) {
                            _richTextBox.SelectionColor = ((LogFont)_logFonts[1, i]).Color;
                            if (((LogFont)_logFonts[1, i]).Bold) {
                                _richTextBox.SelectionFont = new Font(oldFont, FontStyle.Bold);
                            }
                        }
                    }
                    _richTextBox.AppendText(str);
                    _richTextBox.SelectionColor = oldColor;
                    _richTextBox.SelectionFont = oldFont;
                    TrimLines();
                    _richTextBox.ScrollToCaret();
                }));
            }
            else {
                try
                {
                    Color oldColor = _richTextBox.SelectionColor;
                    Font oldFont = _richTextBox.SelectionFont;
                    for (int i = 0; i < _logFonts.GetLength(1); i++)
                    {
                        if (_logFonts[1, i] != null && str.IndexOf(_logFonts[0, i] as string) >= 0)
                        {
                            _richTextBox.SelectionColor = ((LogFont)_logFonts[1, i]).Color;
                            if (((LogFont)_logFonts[1, i]).Bold)
                            {
                                _richTextBox.SelectionFont = new Font(oldFont, FontStyle.Bold);
                            }
                        }
                    }
                    _richTextBox.AppendText(str);
                    _richTextBox.SelectionColor = oldColor;
                    _richTextBox.SelectionFont = oldFont;
                    TrimLines();
                    _richTextBox.ScrollToCaret();
                }
                catch
                { }
            }
        }

        public override void WriteLine(char c) {
            this.Write(c);
            this.WriteLine();
        }

        public override void WriteLine(string str) {
            this.Write(str);
            this.WriteLine();
        }

        public override Encoding Encoding {
            get { return Encoding.UTF8; }
        }

        private void TrimLines() {
            if (_richTextBox.Lines.Length > MaxLines) {
                int needRemoveLineCount = _richTextBox.Lines.Length - MaxLines;
                int firstKeepCharIndex = _richTextBox.GetFirstCharIndexFromLine(needRemoveLineCount);
                int toBeRemovedLength = firstKeepCharIndex;
                if (toBeRemovedLength > _richTextBox.TextLength) {
                    _richTextBox.Clear();
                    return;
                }
                else if (toBeRemovedLength <= 0) {
                    return;
                }
                _richTextBox.Select(0, toBeRemovedLength);
                if (_richTextBox.ReadOnly) {
                    _richTextBox.ReadOnly = false;
                    _richTextBox.SelectedText = "";
                    _richTextBox.ReadOnly = true;
                }
                else {
                    _richTextBox.SelectedText = "";
                }
            }
            _richTextBox.Select(_richTextBox.TextLength, 0);
        }

        internal struct LogFont {
            internal Color Color;
            internal bool Bold;
        }
    }
}
