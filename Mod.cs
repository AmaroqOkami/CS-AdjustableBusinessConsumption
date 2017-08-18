using ICities;
using ColossalFramework.UI;
//using System.Reflection;
using UnityEngine;

namespace AdjustableCommercialConsumption
{
    public class Mod : IUserMod
    {
        public string Name
        {
            get { return "Adjustable Commercial Consumption"; }
        }

        public string Description
        {
            get { return "Allows you to adjust the consumption of goods for commercial buildings."; }
        }


        #region Options UI

        private UISlider comRateSlider;
        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group = helper.AddGroup("General Options");
            UICheckBox checkbox;

            comRateSlider = (UISlider)group.AddSlider("Goods consumption rate", 0f, 2f, 0.05f, ACC_Options.Instance.CommercialGoodsMultiplier, CommercialGoodsMultiplierOnSlider);
            comRateSlider.tooltip = "Sets the percentage of goods consumed by citizens when\n visiting a commercial building, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", ACC_Options.Instance.CommercialGoodsMultiplier * 100) +"%";

            group = helper.AddGroup("Performance Options");

            checkbox = (UICheckBox)group.AddCheckbox("Refill while paused", ACC_Options.Instance.pauseRefillEnable, EnablePauseRefill);
            checkbox.tooltip = "Allows you to choose if you want the building consumption cycling to run while the simulation is paused.\n May very slightly reduce performance while paused, in exchange for a more consistent effect overall."; ;
            /*
            dropDown = (UIDropDown)group.AddDropdown(
                "Update frequency:",
                ACC_Options.UpdateFrequencyOptStr,
                ACC_Options.Instance.GetUpdateFrequencyOptIndex(),
                UpdateFrequencyOnSelected
                );
            dropDown.tooltip = "Sets how many times per second the mod updates. More updates means the mod effect\n is more potent and spreads more quickly, at the (potential) cost of performance.";
            */

            group = helper.AddGroup("Debug/Reset Options");

            checkbox = (UICheckBox) group.AddCheckbox("Enable debug console information", ACC_Options.Instance.DebugConsolePrint, DebugConsoleEnable);
            checkbox.tooltip = "Displays debug info to the console every two seconds, mostly only useful to the developer.";

            checkbox = (UICheckBox) group.AddCheckbox("Fully refill commercial buildings", ACC_Options.Instance.refillBuildingsEnable, EnableRefillCheat);
            checkbox.tooltip = "When enabled, will fully refill commercial buildings completely with the max amount of goods. Happens over time.";
        }

        private void CommercialGoodsMultiplierOnSlider(float slid)
        {
            ACC_Options.Instance.CommercialGoodsMultiplier = slid;
            ACC_Options.Instance.Save();

            comRateSlider.tooltip = "Sets the percentage of goods consumed by citizens when\n visiting a commercial building, from the base amount.\n\nCurrent Rate: " + string.Format("{0:n0}", slid * 100) + "%";
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

        /*
        private void UpdateFrequencyOnSelected(int sel)
        {
            ACC_Options.Instance.UpdateFrequency = ACC_Options.UpdateFrequencyOpt[sel];
            ACC_Options.Instance.Save();
        }
        */

        #endregion
    }
}
