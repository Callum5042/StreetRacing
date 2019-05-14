using GTA;
using GTA.Math;
using NativeUI;
using StreetRacing.Source.Drivers;
using StreetRacing.Source.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StreetRacing.Source.Races
{
    public class SprintRace : RaceBase
    {
        private readonly IConfiguration configuration;
        private readonly SprintGui sprintGUI = new SprintGui();

        public IList<Checkpoint> Checkpoints { get; protected set; } = new List<Checkpoint>();

        public SprintRace(IConfiguration configuration, RaceStart raceStart) : base(configuration)
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
            for (int i = 1; i <= configuration.SpawnCount; i++)
            {
                var position = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * (6.0f * i));
                Drivers.Add(new ComputerDriver(configuration, position));
            }
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
        
        public override void Tick()
        {
            base.Tick();

            CheckCheckpointsBlips();
            sprintGUI.Draw(Drivers.FirstOrDefault(x => x.IsPlayer)?.RacePosition, time);
        }

        protected override void CalculatePositions()
        {
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

        protected override void ComputerAI()
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
        
        public override void Dispose()
        {
            base.Dispose();
            foreach (var blip in Checkpoints.Select(x => x.Blip))
            {
                blip.Remove();
            }
        }
    }
}