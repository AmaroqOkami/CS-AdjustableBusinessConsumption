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
        private UISlider highComRateSlider;
        private UISlider indRateSlider;
        private UISlider delayTimerSlider;
        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group = helper.AddGroup("Consumption Amounts");
            UICheckBox checkbox;

            comRateSlider = (UISlider)group.AddSlider("Commercial", 0f, 2f, 0.05f, ACC_Options.Instance.CommercialGoodsMultiplier, CommercialGoodsMultiplierOnSlider);
            comRateSlider.tooltip = "Sets the percentage of goods consumed by citizens when\nvisiting a commercial building, from the base amount.\nAffects all non-high-density commercial buildings.\n\nCurrent Rate: " + string.Format("{0:n0}", ACC_Options.Instance.CommercialGoodsMultiplier * 100) +"%";

            highComRateSlider = (UISlider)group.AddSlider("High-Density Commercial", 0f, 2f, 0.05f, ACC_Options.Instance.HighCommercialGoodsMultiplier, HighCommercialGoodsMultiplierOnSlider);
            highComRateSlider.tooltip = "Sets the percentage of goods consumed by citizens when visiting\na high-density commercial building, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", ACC_Options.Instance.HighCommercialGoodsMultiplier * 100) + "%";

            indRateSlider = (UISlider)group.AddSlider("Industrial", 0f, 2f, 0.05f, ACC_Options.Instance.IndustrialGoodsMultiplier, IndustrialGoodsMultiplierOnSlider);
            indRateSlider.tooltip = "Sets the percentage of goods consumed by industrial\nbuildings during production, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", ACC_Options.Instance.IndustrialGoodsMultiplier * 100) + "%";


            group = helper.AddGroup("Performance Options");

            delayTimerSlider = (UISlider)group.AddSlider("Start delay", 1f, 10f, 1f, ACC_Options.Instance.delayTimer, delayTimerOnSlider);
            delayTimerSlider.tooltip = "Sets the delay timer for when the mod starts up, in seconds.\nUseful for balancing on-save-load effectiveness and mod stability.\n\nCurrent Delay: " + string.Format("{0:n0}", ACC_Options.Instance.delayTimer) + " seconds";

            checkbox = (UICheckBox)group.AddCheckbox("Refill while paused", ACC_Options.Instance.pauseRefillEnable, EnablePauseRefill);
            checkbox.tooltip = "Allows you to choose if you want the building consumption cycling to run while the simulation is paused.\nMay very slightly reduce performance while paused, in exchange for a more consistent effect overall.";


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

        private void HighCommercialGoodsMultiplierOnSlider(float slid)
        {
            ACC_Options.Instance.HighCommercialGoodsMultiplier = slid;
            ACC_Options.Instance.Save();

            highComRateSlider.tooltip = "Sets the percentage of goods consumed by citizens when\n visiting a high-density commercial building, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", slid * 100) + "%";
        }

        private void IndustrialGoodsMultiplierOnSlider(float slid)
        {
            ACC_Options.Instance.IndustrialGoodsMultiplier = slid;
            ACC_Options.Instance.Save();

            indRateSlider.tooltip = "Sets the percentage of goods consumed by industrial\n buildings during production, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", ACC_Options.Instance.IndustrialGoodsMultiplier * 100) + "%";
        }

        private void delayTimerOnSlider(float slid)
        {
            ACC_Options.Instance.delayTimer = slid;
            ACC_Options.Instance.Save();

            delayTimerSlider.tooltip = "Sets the initialization delay timer when the mod starts up, in seconds.\nUseful to balance starting effectiveness and mod stability..\n\nCurrent Delay: " + string.Format("{0:n0}", ACC_Options.Instance.delayTimer) + " seconds";
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
