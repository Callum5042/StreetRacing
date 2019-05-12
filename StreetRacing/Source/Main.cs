using GTA;
using GTA.Math;
using StreetRacing.Source.Races;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace StreetRacing.Source
{
    //public class Main : Script
    //{
    //    private readonly IConfiguration configuration = new ConfigurationMenu();

    //    public static IStreetRace Race { get; protected set; }
        
    //    public Main()
    //    {
    //        Tick += OnTick;
    //        KeyUp += OnKeyUp;
    //        Aborted += (o, e) =>
    //        {
    //            Race.Finish();
    //            UI.Notify("StreetRacing has aborted");
    //        };

    //        var configMenu = configuration as ConfigurationMenu;
    //        Tick += configMenu.OnTick;
    //        KeyUp += configMenu.OnKeyUp;

    //        Start();
    //    }

    //    private void Start()
    //    {
    //        var assembly = Assembly.GetExecutingAssembly().GetName();
    //        UI.Notify($"{assembly.Name} has loaded: v{assembly.Version.Major}.{assembly.Version.Minor}.{assembly.Version.Build}");
    //    }

    //    public IList<(Vector3 position, Blip blip)> Checkpoints = new List<(Vector3, Blip)>();

    //    private void OnTick(object sender, EventArgs e)
    //    {
    //        // Unload
    //        if (Race?.IsRacing == false)
    //        {
    //            Race.Finish();
    //            Tick -= Race.OnTick;
    //            Race = null;
    //        }

    //        // Record Track
    //        if (configuration.RecordTrack)
    //        {
    //            if (!Checkpoints.Any())
    //            {
    //                var blip = World.CreateBlip(Game.Player.Character.Position);
    //                blip.Sprite = BlipSprite.TransformCheckpoint;
    //                blip.ShowNumber(1);
    //                Checkpoints.Add((Game.Player.Character.Position, blip));
    //            }
    //            else
    //            {
    //                var position = Game.Player.Character.Position;
    //                if (position.DistanceTo(Checkpoints.Last().position) > 50f)
    //                {
    //                    var blip = World.CreateBlip(position);
    //                    blip.Sprite = BlipSprite.TransformCheckpoint;
    //                    blip.ShowNumber(Checkpoints.Count + 1);
    //                    Checkpoints.Add((position, blip));
    //                }
    //            }
    //        }
    //    }

    //    private void OnKeyUp(object sender, KeyEventArgs e)
    //    {
    //        //if (e.KeyCode == Keys.Delete)
    //        //{
    //        //    IList<Vector3> checkpoints = new List<Vector3>();
    //        //    foreach (var (position, blip) in Checkpoints)
    //        //    {
    //        //        checkpoints.Add(position);
    //        //        blip.Remove();
    //        //    }

    //        //    configuration.SaveCheckpoints(checkpoints);
    //        //    Checkpoints.Clear();
    //        //}

    //        if (e.KeyCode == Keys.Y)
    //        {
    //            UI.ShowSubtitle("Start sprint");
    //            Race = new SprintRace(configuration);
    //            Tick += Race.OnTick;
    //        }

    //        if (configuration.Active)
    //        {
    //            try
    //            {
    //                if (e.KeyCode == configuration.StartSpawnKey)
    //                {
    //                    Race = new SpawnDistanceRace(configuration);
    //                    Tick += Race.OnTick;
    //                }

    //                if (e.KeyCode == configuration.StartNearbyKey)
    //                {
    //                    Race = new NearbyDistanceRace(configuration);
    //                    Tick += Race.OnTick;
    //                }
    //            }
    //            catch (InvalidOperationException ex)
    //            {
    //                UI.Notify(ex.Message);
    //            }
    //        }
    //    }
    //}
}