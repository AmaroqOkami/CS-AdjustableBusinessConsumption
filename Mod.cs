using ICities;
using ColossalFramework.UI;
using UnityEngine;

namespace AdjustableCommercialConsumption
{
    public class Mod : IUserMod
    {
        public string Name
        {
            get { return "Adjustable Business Consumption"; }
        }

        public string Description
        {
            get { return "Allows you to adjust the consumption of goods for commercial and industrial buildings."; }
        }


        #region Options UI

        private UISlider comRateSlider;
        private UISlider indRateSlider;
        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group = helper.AddGroup("General Options");
            UICheckBox checkbox;

            comRateSlider = (UISlider)group.AddSlider("Commercial consumption", 0f, 2f, 0.05f, ACC_Options.Instance.CommercialGoodsMultiplier, CommercialGoodsMultiplierOnSlider);
            comRateSlider.tooltip = "Sets the percentage of goods consumed by citizens when\n visiting a commercial building, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", ACC_Options.Instance.CommercialGoodsMultiplier * 100) +"%";

            indRateSlider = (UISlider)group.AddSlider("Industrial consumption", 0f, 2f, 0.05f, ACC_Options.Instance.IndustrialGoodsMultiplier, IndustrialGoodsMultiplierOnSlider);
            indRateSlider.tooltip = "Sets the percentage of goods consumed by industrial\n buildings during production, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", ACC_Options.Instance.IndustrialGoodsMultiplier * 100) + "%";


            group = helper.AddGroup("Performance Options");

            checkbox = (UICheckBox)group.AddCheckbox("Refill while paused", ACC_Options.Instance.pauseRefillEnable, EnablePauseRefill);
            checkbox.tooltip = "Allows you to choose if you want the building consumption cycling to run while the simulation is paused.\n May very slightly reduce performance while paused, in exchange for a more consistent effect overall.";


            group = helper.AddGroup("Debug/Reset Options");

            checkbox = (UICheckBox) group.AddCheckbox("Enable debug console information", ACC_Options.Instance.DebugConsolePrint, DebugConsoleEnable);
            checkbox.tooltip = "Displays debug info to the console every two seconds, mostly only useful to the developer.";

            checkbox = (UICheckBox) group.AddCheckbox("Fully refill business buildings", ACC_Options.Instance.refillBuildingsEnable, EnableRefillCheat);
            checkbox.tooltip = "When enabled, will fully refill industrial and commercial buildings completely with the max amount of goods. Happens over time.";
        }

        private void CommercialGoodsMultiplierOnSlider(float slid)
        {
            ACC_Options.Instance.CommercialGoodsMultiplier = slid;
            ACC_Options.Instance.Save();

            comRateSlider.tooltip = "Sets the percentage of goods consumed by citizens when\n visiting a commercial building, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", slid * 100) + "%";
        }

        private void IndustrialGoodsMultiplierOnSlider(float slid)
        {
            ACC_Options.Instance.IndustrialGoodsMultiplier = slid;
            ACC_Options.Instance.Save();

            indRateSlider.tooltip = "Sets the percentage of goods consumed by industrial\n buildings during production, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", ACC_Options.Instance.IndustrialGoodsMultiplier * 100) + "%";
        }

        private void EnablePauseRefill(bool enabled)
        {
            ACC_Options.Instance.pauseRefillEnable = enabled;
            ACC_Options.Instance.Save();
        }

        private void DebugConsoleEnable(bool enabled)
        {
            ACC_Options.Instance.DebugConsolePrint = enabled;
            ACC_Options.Instance.Save();
        }

        private void EnableRefillCheat(bool enabled)
        {
            ACC_Options.Instance.refillBuildingsEnable = enabled;
            ACC_Options.Instance.Save();
        }

        #endregion
    }
}
