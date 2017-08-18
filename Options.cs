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
        /*
        public static int[] UpdateFrequencyOpt = new int[] { 0, 1, 2, 3, 5 };
        public static string[] UpdateFrequencyOptStr = new string[] { "60/sec", "30/sec", "20/sec", "15/sec", "10/sec" };
        public int UpdateFrequency;
        */

        public bool pauseRefillEnable;
        public bool DebugConsolePrint;
        public bool refillBuildingsEnable;

        public ACC_Options()
        {
            CommercialGoodsMultiplier = 1;
            //UpdateFrequency = 0;
            pauseRefillEnable = true;
            DebugConsolePrint = false;
            refillBuildingsEnable = false;
        }

        /*
        public int GetUpdateFrequencyOptIndex()
        {
            int index = Array.IndexOf(UpdateFrequencyOpt, UpdateFrequency);

            if (index == -1) return Array.IndexOf(UpdateFrequencyOpt, 1);

            return index;
        }
        */

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
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "Adjustable Com. Consumption: " + ex.Message);
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
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "Adjustable Com. Consumption: " + ex.Message);

                return new ACC_Options();
            }
        }
    }
}