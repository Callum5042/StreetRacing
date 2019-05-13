using NativeUI;
using System;

namespace StreetRacing.Source.Gui
{
    public class SprintGui
    {
        private TimerBarPool timerBarPool;
        private TextTimerBar timeBar;
        private TextTimerBar positionBar;

        public SprintGui()
        {
            timerBarPool = new TimerBarPool();
            timeBar = new TextTimerBar("Time", "0:00");
            timerBarPool.Add(timeBar);

            positionBar = new TextTimerBar("Position", "");
            timerBarPool.Add(positionBar);
        }

        public void Draw(int? position, TimeSpan time)
        {
            positionBar.Text = position?.ToString();
            timeBar.Text = time.ToString(@"mm\:ss\:fff");
            timerBarPool.Draw();
        }
    }
}