using GTA;
using GTA.Math;
using NativeUI;
using StreetRacing.Source.Racers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StreetRacing.Source.Races
{
    public class SprintRace : IStreetRace
    {
        protected TimerBarPool timerBarPool;

        private TextTimerBar timeBar;

        private TextTimerBar positionBar;

        public IList<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();

        private readonly DateTime StartTime = DateTime.Now;

        private TimeSpan time;

        private readonly IConfiguration configuration;

        public SprintRace(IConfiguration configuration)
        {
            this.configuration = configuration;

            // Load race
            var document = XDocument.Load("scripts/streetracing/checkpoints.xml");
            foreach (XElement item in document.Descendants().Where(x => x.Name == "checkpoint"))
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

            // Add Racers
            Racers.Add(new PlayerRacingDriver());
            var playerPosition = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * (6.0f * 1));

            var otherRacer = new SpawnRacingDriver(configuration, playerPosition);
            Racers.Add(otherRacer);
            otherRacer.Vehicle.Driver.Task.DriveTo(otherRacer.Vehicle.Driver.CurrentVehicle, Checkpoints.FirstOrDefault().Position, 20f, 200f);

            // GUI
            timerBarPool = new TimerBarPool();
            timeBar = new TextTimerBar("Time", "0:00");
            timerBarPool.Add(timeBar);

            positionBar = new TextTimerBar("Position", "");
            timerBarPool.Add(positionBar);
        }

        public bool IsRacing { get; protected set; } = true;

        public IList<IRacingDriver> Racers { get; protected set; } = new List<IRacingDriver>();

        public void Finish()
        {
            foreach (var checkpoint in Checkpoints)
            {
                checkpoint.Blip.Remove();
            }

            foreach (var driver in Racers.Where(x => !x.IsPlayer))
            {
                driver.Vehicle.CurrentBlip.Remove();
                driver.Driver.Delete();
                driver.Vehicle.Delete();
            }

            var player = Racers.FirstOrDefault(x => x.IsPlayer);
            BigMessageThread.MessageInstance.ShowRankupMessage("Finish", time.ToString(@"mm\:ss\:fff"), player.RacePosition);
        }

        public void OnTick(object sender, EventArgs e)
        {
            foreach (var racer in Racers.Where(x => !x.IsPlayer))
            {
                var driver = racer.Vehicle.Driver;
                var nextCheckpoint = Checkpoints.FirstOrDefault(x => x.Number == racer.Checkpoint + 1);
                if (driver.Position.DistanceTo(nextCheckpoint.Position) < 20f)
                {
                    racer.Checkpoint = nextCheckpoint.Number;

                    var driveToCheckpoint = Checkpoints.FirstOrDefault(x => x.Number == racer.Checkpoint + 1);
                    if (driveToCheckpoint != null)
                    {
                        driver.Task.DriveTo(driver.CurrentVehicle, driveToCheckpoint.Position, 20f, 200f);
                    }
                    else
                    {
                        racer.Lost();
                    }
                }
            }


            var player = Racers.FirstOrDefault(x => x.IsPlayer);
            if (player != null)
            {
                var nextCheckpoint = Checkpoints.FirstOrDefault(x => x.Number == player.Checkpoint + 1);
                if (nextCheckpoint != null)
                {
                    if (player.Driver.Position.DistanceTo(nextCheckpoint.Position) < 20f)
                    {
                        nextCheckpoint.Blip.Remove();
                        player.Checkpoint = nextCheckpoint.Number;
                    }
                }

                if (player.Checkpoint == Checkpoints.Max(x => x.Number))
                {
                    IsRacing = false;
                }
            }

            // Draw
            positionBar.Text = player?.RacePosition.ToString();
            time = StartTime.Subtract(DateTime.Now);
            timeBar.Text = time.ToString(@"mm\:ss\:fff");
            timerBarPool.Draw();
        }
    }
}