using NativeUI;
using System;
using System.Drawing;

namespace StreetRacing.Source.Gui
{
    public class DistanceRaceGui
    {
        private TimerBarPool timerBarPool;
        private TextTimerBar timeBar;
        private TextTimerBar positionBar;
        private BarTimerBar distanceBar;

        public DistanceRaceGui()
        {
            timerBarPool = new TimerBarPool();
            timeBar = new TextTimerBar("Time", "0:00");
            timerBarPool.Add(timeBar);

            positionBar = new TextTimerBar("Position", "");
            timerBarPool.Add(positionBar);

            distanceBar = new BarTimerBar("Distance");
            distanceBar.BackgroundColor = Color.Black;
            distanceBar.ForegroundColor = Color.White;
            timerBarPool.Add(distanceBar);
        }

        public void Draw(int? position, TimeSpan time, float percentage)
        {
            distanceBar.Percentage = percentage;
            positionBar.Text = position?.ToString();
            timeBar.Text = time.ToString(@"mm\:ss\:fff");
            timerBarPool.Draw();
        }
    }
}