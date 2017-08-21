using System;
using System.IO;
using System.Xml.Serialization;
using ColossalFramework.Plugins;

namespace AdjustableCommercialConsumption
{
    public class ACC_Options
    {
        private const string optionsFileName = "AdjustableComConsumptionOptions.xml";
        private static ACC_Options _instance;

        public static ACC_Options Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateFromFile();
                }

                return _instance;
            }
        }

        public float CommercialGoodsMultiplier;
        public float IndustrialGoodsMultiplier;

        public bool pauseRefillEnable;
        public bool DebugConsolePrint;
        public bool refillBuildingsEnable;

        public ACC_Options()
        {
            CommercialGoodsMultiplier = 1;
            IndustrialGoodsMultiplier = 1;
            pauseRefillEnable = true;
            DebugConsolePrint = false;
            refillBuildingsEnable = false;
        }

        public void Save()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(ACC_Options));
                TextWriter writer = new StreamWriter(optionsFileName);
                ser.Serialize(writer, this);
                writer.Close();
            }
            catch (Exception ex)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "Adjustable Bus. Consumption: " + ex.Message);
            }
        }

        public static ACC_Options CreateFromFile()
        {
            if (!File.Exists(optionsFileName)) return new ACC_Options();

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(ACC_Options));
                TextReader reader = new StreamReader(optionsFileName);
                ACC_Options instance = (ACC_Options)ser.Deserialize(reader);
                reader.Close();

                return instance;
            }
            catch (Exception ex)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "Adjustable Bus. Consumption: " + ex.Message);

                return new ACC_Options();
            }
        }
    }
}