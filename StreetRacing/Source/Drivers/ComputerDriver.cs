﻿using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;

namespace StreetRacing.Source.Drivers
{
    public class ComputerDriver : DriverBase, ITask
    {
        private readonly IConfiguration configuration;
        private const int drivingStyle = 262204;

        public ComputerDriver(IConfiguration configuration, Vector3 position)
        {
            this.configuration = configuration;

            Vehicle = World.CreateVehicle(SpawnVehicle(), position, Game.Player.Character.Heading);
            Start(Vehicle);
        }

        protected ComputerDriver(IConfiguration configuration) => this.configuration = configuration;

        private VehicleHash SpawnVehicle()
        {
            var list = new List<VehicleHash>
            {
                VehicleHash.Pfister811,
                VehicleHash.Adder,
                VehicleHash.Dominator,
                VehicleHash.Dominator2,
                VehicleHash.Dominator3,
                VehicleHash.Dominator4,
                VehicleHash.Dominator5,
                VehicleHash.HotringSabre,
                VehicleHash.SabreGT,
                VehicleHash.SabreGT2,
                VehicleHash.Deveste,
                VehicleHash.EntityXF,
                VehicleHash.EntityXXR,
                VehicleHash.Cyclone,
                VehicleHash.ItaliGTB,
                VehicleHash.ItaliGTB2,
                VehicleHash.ItaliGTO,
                VehicleHash.Nero,
                VehicleHash.Nero2,
                VehicleHash.Tyrus,
                VehicleHash.Pfister811,
                VehicleHash.Banshee,
                VehicleHash.Banshee2,
                VehicleHash.Reaper,
                VehicleHash.SultanRS,
                VehicleHash.Sultan,
                VehicleHash.FlashGT,
                VehicleHash.Neon,
                VehicleHash.Nero,
                VehicleHash.Nero2,
                VehicleHash.Comet2,
                VehicleHash.Comet3,
                VehicleHash.Comet4,
                VehicleHash.Comet5,
                VehicleHash.Elegy,
                VehicleHash.Elegy2,
                VehicleHash.Specter,
                VehicleHash.Specter2,
                VehicleHash.Seven70,
                VehicleHash.Lynx,
                VehicleHash.Omnis
            };

            Random random = new Random();
            int index = random.Next(list.Count);
            return list[index];
        }

        protected void Start(Vehicle vehicle)
        {
            var ped = vehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
            ped.DrivingStyle = (DrivingStyle)drivingStyle;
            ped.AlwaysKeepTask = true;
            ped.DrivingSpeed = 200f;

            vehicle.AddBlip();
            vehicle.CurrentBlip.Color = BlipColor.Blue;
            vehicle.CurrentBlip.IsFlashing = false;

            if (configuration.MaxMods)
            {
                Vehicle.InstallModKit();
                foreach (VehicleMod value in Enum.GetValues(typeof(VehicleMod)))
                {
                    int modCount = Vehicle.GetModCount(value);
                    Vehicle.SetMod(value, modCount - 1, variations: true);
                }
            }
        }

        public override string ToString()
        {
            return Vehicle.FriendlyName;
        }

        public override bool IsPlayer => false;
        
        public DriverTask DriverTask { get; protected set; } = DriverTask.None;

        public override void Dispose()
        {
            Vehicle.Driver.Delete();
            Vehicle.Delete();
            Vehicle.CurrentBlip.Remove();
            InRace = false;
        }

        public void DriveTo(Vector3 position)
        {
            Vehicle.Driver.Task.DriveTo(Vehicle, position, 20f, 200f, drivingStyle);
        }

        public void Cruise()
        {
            if (DriverTask != DriverTask.Cruise)
            {
                DriverTask = DriverTask.Cruise;
                Vehicle.Driver.Task.CruiseWithVehicle(Vehicle, 200f, drivingStyle);
            }
        }

        public override void Finish()
        {
            Dispose();
        }

        public override void UpdateBlip()
        {
            Vehicle.CurrentBlip.ShowNumber(RacePosition);
        }

        public void Chase(IDriver driver)
        {
            if (DriverTask != DriverTask.Chase)
            {
                DriverTask = DriverTask.Chase;
                Vehicle.Driver.Task.VehicleChase(driver.Vehicle.Driver);
            }
        }
    }
}