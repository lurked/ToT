using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace ToT
{
    public class LogEntry
    {
        public float Duration;
        public float TimeAdded;
        public string Text;
        public Color TextColor;
        public Font TextFont;

        public LogEntry()
        {
            TimeAdded = ScreenManager.TotalTime;
            Duration = 10f;
            Text = "No text set.";
            TextFont = Font.menuItem03;
            TextColor = Color.White;
        }

        public LogEntry(string text, float duration = 10f, Font textFont = Font.menuItem03)
        {
            Text = text;
            Duration = duration;
            TextFont = textFont;
            TimeAdded = ScreenManager.TotalTime;
            TextColor = Color.White;
        }

        public float Expiration()
        {
            return TimeAdded + Duration;
        }
    }
}
