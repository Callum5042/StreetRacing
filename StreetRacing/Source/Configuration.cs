using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace StreetRacing.Source
{
    public class Configuration : IConfiguration
    {
        private readonly string configFilePath = "scripts/streetracing-config.xml";

        public void Load()
        {
            try
            {
                using (var document = XmlReader.Create(configFilePath))
                {
                    while (document.Read())
                    {
                        while (document.Read())
                        {
                            if (document.NodeType == XmlNodeType.Element)
                            {
                                foreach (var prop in GetType().GetProperties())
                                {
                                    if (prop.PropertyType == typeof(bool))
                                    {
                                        document.ReadToFollowing(prop.Name);
                                        prop.SetValue(this, document.ReadElementContentAsBoolean());
                                    }
                                    else if (prop.PropertyType == typeof(int))
                                    {
                                        document.ReadToFollowing(prop.Name);
                                        prop.SetValue(this, document.ReadElementContentAsInt());
                                    }
                                    else if (prop.PropertyType == typeof(Keys))
                                    {
                                        document.ReadToFollowing(prop.Name);
                                        var value = document.ReadElementContentAsString();

                                        if (Enum.TryParse(value, out Keys key))
                                        {
                                            prop.SetValue(this, key);
                                        }
                                    }
                                    else
                                    {
                                        document.ReadToFollowing(prop.Name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // UI.Notify("Could not find file: " + ex.FileName);
            }
            catch (InvalidOperationException) { }
        }

        public void Save()
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;

            using (var document = XmlWriter.Create(configFilePath, settings))
            {
                document.WriteStartElement("config");
                foreach (var prop in GetType().GetProperties())
                {
                    if (prop.PropertyType == typeof(bool))
                    {
                        document.WriteElementString(prop.Name, ((bool)prop.GetValue(this)).ToString().ToLower());
                    }
                    else
                    {
                        document.WriteElementString(prop.Name, prop.GetValue(this).ToString());
                    }
                }

                document.WriteEndElement();
            }
        }

        public void SaveCheckpoints(IEnumerable<Vector3> checkpoints)
        {
            XDocument document;
            try
            {
                document = XDocument.Load("scripts/streetracing/checkpoints.xml");
            }
            catch (FileNotFoundException)
            {
                document = new XDocument();
                var root = new XElement("root");
                document.Add(root);
            }

            var race = new XElement("race");
            race.Add(new XAttribute("name", World.GetStreetName(Game.Player.Character.Position)));
            foreach (var checkpoint in checkpoints)
            {
                var element = new XElement("checkpoint");
                element.Add(new XAttribute("X", checkpoint.X));
                element.Add(new XAttribute("Y", checkpoint.Y));
                element.Add(new XAttribute("Z", checkpoint.Z));
                race.Add(element);
            }

            document.Root.Add(race);
            document.Save("scripts/streetracing/checkpoints.xml");

            // Clear checkpoints
            foreach (var blip in StreetRacing.RecordedCheckpoints.Select(x => x.Blip))
            {
                blip.Remove();
            }
        }
        
        public Keys MenuKey { get; protected set; } = Keys.F8;

        public Keys StartNearbyKey { get; protected set; } = Keys.E;

        public Keys StartSpawnKey { get; protected set; } = Keys.T;

        public bool Active { get; protected set; } = true;

        public int SpawnCount { get; protected set; } = 1;

        public bool MaxMods { get; protected set; } = true;

        public float WinDistance { get; protected set; } = 200f;

        public int Money { get; protected set; } = 1000;

        public bool PolicePursuit { get; protected set; } = true;

        public bool RecordTrack { get; protected set; }
    }
}