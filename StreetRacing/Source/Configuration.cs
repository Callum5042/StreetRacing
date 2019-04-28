using GTA;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

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

        public bool Active { get; protected set; } = true;

        public int SpawnCount { get; protected set; } = 5;

        public bool MaxMods { get; protected set; } = true;

        public Keys StartNearbyKey { get; protected set; } = Keys.E;

        public Keys StartSpawnKey { get; protected set; } = Keys.T;
    }
}