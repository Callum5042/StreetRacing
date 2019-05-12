using GTA;
using GTA.Math;
using NativeUI;
using StreetRacing.Source.New.Drivers;
using StreetRacing.Source.Races;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StreetRacing.Source.New.Races
{
    public class SprintGUI
    {
        private TimerBarPool timerBarPool;
        private TextTimerBar timeBar;
        private TextTimerBar positionBar;

        public SprintGUI()
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

    public class SprintRace : IRace
    {
        private readonly IConfiguration configuration;
        private readonly SprintGUI sprintGUI = new SprintGUI();
        private readonly DateTime startTime = DateTime.Now;
        private TimeSpan time;

        public IList<Checkpoint> Checkpoints { get; protected set; } = new List<Checkpoint>();

        public IList<IDriver> Drivers { get; protected set; } = new List<IDriver>();

        public SprintRace(IConfiguration configuration, RaceStart raceStart)
        {
            this.configuration = configuration;

            LoadRaceFromFile(raceStart);
            LoadDrivers(configuration);
            CalculateStartPositions();
            UI.Notify($"Race Started: {raceStart.Name}");
        }

        private void LoadDrivers(IConfiguration configuration)
        {
            Drivers.Add(new PlayerDriver(configuration));

            var position = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * 6.0f);
            Drivers.Add(new ComputerDriver(configuration, position));
        }

        private void LoadRaceFromFile(RaceStart raceStart)
        {
            var document = XDocument.Load("scripts/streetracing/checkpoints.xml");
            foreach (XElement item in document.Descendants().Where(x => x.Name == "checkpoint" && x.Parent.Attributes().FirstOrDefault(p => p.Name == "name").Value == raceStart.Name))
            {
                var xCoord = float.Parse(item.Attributes().FirstOrDefault(x => x.Name == "X").Value);
                var yCoord = float.Parse(item.Attributes().FirstOrDefault(x => x.Name == "Y").Value);
                var zCoord = float.Parse(item.Attributes().FirstOrDefault(x => x.Name == "Z").Value);

                var number = Checkpoints.Count + 1;
                var racePosition = new Vector3(xCoord, yCoord, zCoord);
                var blip = World.CreateBlip(racePosition);
                blip.ShowNumber(number);

                Checkpoints.Add(new Checkpoint()
                {
                    Number = number,
                    Position = racePosition,
                    Blip = blip
                });
            }
        }

        public bool IsRacing { get; protected set; } = true;

        public void Tick()
        {
            time = startTime.Subtract(DateTime.Now);

            foreach (var driver in Drivers)
            {
                driver.UpdateBlip();
            }

            CalculatePositions();
            ComputerAI();
            CheckCheckpointsBlips();
            sprintGUI.Draw(Drivers.FirstOrDefault(x => x.IsPlayer)?.RacePosition, time);
        }

        protected void CalculateStartPositions()
        {
            foreach (var driver in Drivers)
            {
                driver.RacePosition = Drivers.Count;
                foreach (var otherDriver in Drivers)
                {
                    if (driver != otherDriver)
                    {
                        if (driver.InFront(otherDriver))
                        {
                            driver.RacePosition--;
                        }
                    }
                }
            }
        }

        private void CalculatePositions()
        {
            // First get position based on checkpoints


            // Then get position between cars on the who has is on the same checkpoint

            foreach (var driver in Drivers)
            {
                foreach (var otherRacer in Drivers.Where(x => x != driver && driver.DistanceTo(x.Position) < 50f))
                {
                    if (driver.InFront(otherRacer))
                    {
                        if (driver.RacePosition > otherRacer.RacePosition)
                        {
                            int tmp = driver.RacePosition;
                            driver.RacePosition = otherRacer.RacePosition;
                            otherRacer.RacePosition = tmp;
                        }
                    }
                }
            }
        }

        private void ComputerAI()
        {
            foreach (var driver in Drivers.Where(x => !x.IsPlayer && x.InRace))
            {
                if (driver is ITask task)
                {
                    var nextCheckpoint = Checkpoints.FirstOrDefault(x => x.Number == driver.Checkpoint + 1);
                    if (driver.DistanceTo(nextCheckpoint.Position) < 20f)
                    {
                        driver.Checkpoint = nextCheckpoint.Number;

                        var driveToCheckpoint = Checkpoints.FirstOrDefault(x => x.Number == driver.Checkpoint + 1);
                        if (driveToCheckpoint != null)
                        {
                            task.DriveTo(driveToCheckpoint.Position);
                        }
                        else
                        {
                            task.Cruise();
                            driver.Finish();
                        }
                    }
                }
            }
        }

        private void CheckCheckpointsBlips()
        {
            var player = Drivers.FirstOrDefault(x => x.IsPlayer);
            if (player != null)
            {
                var nextCheckpoint = Checkpoints.FirstOrDefault(x => x.Number == player.Checkpoint + 1);
                if (nextCheckpoint != null)
                {
                    if (player.DistanceTo(nextCheckpoint.Position) < 20f)
                    {
                        nextCheckpoint.Blip.Remove();
                        player.Checkpoint = nextCheckpoint.Number;
                    }
                }

                UI.ShowSubtitle("Checkpoint: " + player.Checkpoint);
                if (player.Checkpoint == Checkpoints.Max(x => x.Number))
                {
                    player.Finish();
                    Finish();
                }
            }
        }

        public void Finish()
        {
            IsRacing = false;
            var player = Drivers.FirstOrDefault(x => x.IsPlayer);
            BigMessageThread.MessageInstance.ShowRankupMessage("Finish", time.ToString(@"mm\:ss\:fff"), player.RacePosition);
        }
        
        public void Dispose()
        {
            foreach (var blip in Checkpoints.Select(x => x.Blip))
            {
                blip.Remove();
            }

            foreach (var driver in Drivers.Where(x => !x.IsPlayer))
            {
                driver.Dispose();
            }
        }
    }
}