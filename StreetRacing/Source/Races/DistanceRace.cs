using GTA;
using GTA.Math;
using GTA.Native;
using StreetRacing.Source.Racers;
using StreetRacing.Source.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreetRacing.Source.Races
{
    public abstract class DistanceRace : IStreetRace
    {
        protected readonly IConfiguration configuration;

        public DistanceRace(IConfiguration configuration)
        {
            this.configuration = configuration;

            if (Main.Race?.IsRacing == true)
            {
                throw new InvalidOperationException("Already in a race");
            }
        }
        
        public IList<IRacingDriver> Racers { get; protected set; } = new List<IRacingDriver>();

        public bool IsRacing { get; protected set; } = true;

        public virtual void Finish()
        {
            IsRacing = false;
        }

        public virtual void OnTick(object sender, EventArgs e)
        {
            if (IsRacing)
            {
                CalculateDriversPosition();

                foreach (var racer in Racers.ToList())
                {
                    racer.Tick();
                    if (!racer.InRace)
                    {
                        Racers.Remove(racer);
                        
                        foreach (var racerBehind in Racers.Where(x => x.RacePosition > racer.RacePosition))
                        {
                            racerBehind.RacePosition--;
                        }
                    }
                }
                
                Tick();
                DeployPolice();

                UI.ShowSubtitle($"Position: {Racers.FirstOrDefault(x => x.IsPlayer)?.RacePosition} - Count: {Racers.Where(x => x.RacePosition == 1).Count()}");
            }
        }

        protected void DeployPolice()
        {
            if (configuration.PolicePursuit)
            {
                var vehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 100f).Where(x => x.Driver.IsInPoliceVehicle);
                if (vehicles.Any(x => x.IsOnScreen))
                {
                    Game.Player.WantedLevel = 1;
                }
            }
        }

        protected virtual void Tick()
        {
            foreach (var driver in Racers.ToList())
            {
                if (driver.RacePosition != 1)
                {
                    var first = Racers.FirstOrDefault(x => x.RacePosition == 1);
                    if (first != null)
                    {
                        if (driver.DistanceTo(first) > configuration.WinDistance)
                        {
                            UI.Notify($"{driver.ToString()} lose");

                            driver.Lost();
                            Racers.Remove(driver);

                            if (driver.IsPlayer)
                            {
                                IsRacing = false;
                            }
                        }
                    }
                }

                if (Racers.Count == 1 || !IsRacing)
                {
                    IsRacing = false;
                    if (Racers.FirstOrDefault(x => x.RacePosition == 1)?.IsPlayer == true)
                    {
                        UI.Notify($"You win");
                        Game.Player.Money += configuration.Money;
                    }
                    else
                    {
                        UI.Notify($"You lose");
                        Game.Player.Money -= configuration.Money;
                    }
                }
            }
        }

        protected void CalculateStartPositions()
        {
            foreach (var driver in Racers)
            {
                driver.RacePosition = Racers.Count;
                foreach (var otherDriver in Racers)
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

        protected void CalculateDriversPosition()
        {
            foreach (var racer in Racers)
            {
                foreach (var otherRacer in Racers.Where(x => x != racer && racer.DistanceTo(x) < 50f))
                {
                    if (racer.InFront(otherRacer))
                    {
                        if (racer.RacePosition > otherRacer.RacePosition)
                        {
                            int tmp = racer.RacePosition;
                            racer.RacePosition = otherRacer.RacePosition;
                            otherRacer.RacePosition = tmp;
                        }
                    }
                }
            }
        }
    }
}