using GTA;
using GTA.Math;
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
                UI.ShowSubtitle($"Position: {Racers.FirstOrDefault(x => x.IsPlayer).RacePosition} - Count: {Racers.Where(x => x.RacePosition == 1).Count()}");
            }
        }

        protected virtual void Tick()
        {
            
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
                foreach (var otherRacer in Racers.Where(x => x != racer && racer.Distance(x) < 50f))
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