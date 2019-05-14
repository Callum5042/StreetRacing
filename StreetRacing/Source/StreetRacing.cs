using GTA;
using GTA.Math;
using StreetRacing.Source.Races;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace StreetRacing.Source
{
    public class StreetRacing : Script
    {
        private readonly IConfiguration configuration = new ConfigurationMenu();
        private readonly IList<RaceStart> raceStartPoints = new List<RaceStart>();

        public static IRace CurrentRace { get; protected set; }

        public static IList<Checkpoint> RecordedCheckpoints { get; set; } = new List<Checkpoint>();

        public StreetRacing()
        {
            Tick += OnTick;
            KeyUp += OnKeyUp;

            var configMenu = configuration as ConfigurationMenu;
            Tick += configMenu.OnTick;
            KeyUp += configMenu.OnKeyUp;

            LoadRaces();
            StartMessage();
        }

        protected override void Dispose(bool A_0)
        {
            UI.Notify("StreetRacing aborted");
            CurrentRace?.Dispose();
            ClearStartBlips();

            foreach (var blip in RecordedCheckpoints.Select(x => x.Blip))
            {
                blip.Remove();
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (CurrentRace?.IsRacing == true)
            {
                CurrentRace.Tick();
            }
            else if (CurrentRace?.IsRacing == false)
            {
                CurrentRace.Dispose();
                LoadStartBlips();
                CurrentRace = null;
            }
            else
            {
                if (CanStart())
                {
                    foreach (var raceStartPoint in raceStartPoints)
                    {
                        if (Game.Player.Character.Position.DistanceTo(raceStartPoint.Position) < 20f)
                        {
                            UI.ShowSubtitle("Start race");
                        }
                    }
                }
            }

            // Record Track
            RecordTrack();
        }

        private void RecordTrack()
        {
            if (configuration.RecordTrack)
            {
                if (!RecordedCheckpoints.Any())
                {
                    var position = Game.Player.Character.Position;
                    var blip = World.CreateBlip(position);
                    blip.ShowNumber(1);

                    RecordedCheckpoints.Add(new Checkpoint()
                    {
                        Position = position,
                        Blip = blip,
                        Number = 1
                    });
                }
                else
                {
                    var lastCheckpoint = RecordedCheckpoints.Last();
                    var position = Game.Player.Character.Position;

                    if (lastCheckpoint.Position.DistanceTo(position) > 50f)
                    {
                        var number = lastCheckpoint.Number + 1;
                        var blip = World.CreateBlip(position);
                        blip.ShowNumber(number);

                        RecordedCheckpoints.Add(new Checkpoint()
                        {
                            Position = position,
                            Blip = blip,
                            Number = number
                        });
                    }
                }
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (configuration.Active)
            {
                foreach (var raceStartPoint in raceStartPoints)
                {
                    if (Game.Player.Character.Position.DistanceTo(raceStartPoint.Position) < 20f)
                    {
                        if (e.KeyCode == configuration.StartNearbyKey && CanStart())
                        {
                            ClearStartBlips();
                            CurrentRace = new SprintRace(configuration, raceStartPoint);
                        }
                    }
                }

                if (e.KeyCode == configuration.StartNearbyKey && CanStart())
                {
                    try
                    {
                        CurrentRace = new NearbyRace(configuration);
                        ClearStartBlips();
                    }
                    catch (InvalidOperationException)
                    {
                        CurrentRace.Dispose();
                        CurrentRace = null;
                    }
                }

                if (e.KeyCode == configuration.StartSpawnKey && CanStart())
                {
                    ClearStartBlips();
                    CurrentRace = new DistanceRace(configuration);
                }
            }
        }

        private void StartMessage()
        {
            var assembly = Assembly.GetExecutingAssembly().GetName();
            UI.Notify($"{assembly.Name} has loaded: v{assembly.Version.Major}.{assembly.Version.Minor}.{assembly.Version.Build}");
        }

        private void LoadRaces()
        {
            try
            {
                var document = XDocument.Load("scripts/streetracing/checkpoints.xml");
                foreach (XElement raceElement in document.Descendants().Where(x => x.Name == "race"))
                {
                    var firstCheckpoint = raceElement.Element("checkpoint");
                    var xCoord = float.Parse(firstCheckpoint.Attributes().FirstOrDefault(x => x.Name == "X").Value);
                    var yCoord = float.Parse(firstCheckpoint.Attributes().FirstOrDefault(x => x.Name == "Y").Value);
                    var zCoord = float.Parse(firstCheckpoint.Attributes().FirstOrDefault(x => x.Name == "Z").Value);

                    var position = new Vector3(xCoord, yCoord, zCoord);
                    var blip = World.CreateBlip(position);
                    blip.Sprite = BlipSprite.RaceCar;
                    blip.Name = "Street Race";
                    blip.IsShortRange = true;

                    raceStartPoints.Add(new RaceStart()
                    {
                        Name = raceElement.Attributes().FirstOrDefault(x => x.Name == "name").Value,
                        Position = position,
                        Blip = blip
                    });
                }
            }
            catch (FileNotFoundException ex)
            {
                UI.Notify("Could not find file: " + ex.FileName);
            }
        }

        private void ClearStartBlips()
        {
            foreach (var blip in raceStartPoints.Select(x => x.Blip))
            {
                blip.Remove();
            }
        }

        private void LoadStartBlips()
        {
            foreach (var raceStart in raceStartPoints)
            {
                raceStart.Blip = World.CreateBlip(raceStart.Position);
                raceStart.Blip.Sprite = BlipSprite.RaceCar;
                raceStart.Blip.Name = "Street Race";
                raceStart.Blip.IsShortRange = true;
            }
        }

        private bool CanStart()
        {
            return (CurrentRace == null && Game.Player.Character.IsInVehicle());
        }
    }
}