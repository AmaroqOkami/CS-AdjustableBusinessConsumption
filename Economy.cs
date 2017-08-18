using System.Reflection;
using System.Collections.Generic;
using ICities;
using ColossalFramework;
using System.Collections;
using UnityEngine;

namespace AdjustableCommercialConsumption
{
    public class GoodsMonitor : ThreadingExtensionBase
    {
        private readonly BuildingManager buildingManager;
        private readonly SimulationManager simulationManager;
        private CoTimer debugTimerIns;

        private static Dictionary<ushort, int> buildingGoodsCount = new Dictionary<ushort, int>();

        private static bool hasStarted = false;
        //private static short GoodsClock = 0;
        private int refTransferAmt = 0;

        public static int GoodsReplenishAmount = 0;
        public static int GoodsOldTotalAmount = 0;
        public static int GoodsTotalAmount = 0;
        public static int GoodsReadAmount = 0;
        public static int GoodsNewAmount = 0;


        public GoodsMonitor()
        {
            buildingManager = Singleton<BuildingManager>.instance;
            simulationManager = Singleton<SimulationManager>.instance;
            debugTimerIns = Singleton<CoTimer>.instance;
        }

        public override void OnAfterSimulationTick()
        {
            if (!hasStarted)
            {
                hasStarted = true;
                debugTimerIns.StartCoroutine("DebugTimer");
            }

            //GoodsClock++;
            //if (GoodsClock > ACC_Options.Instance.UpdateFrequency)
            GoodsCheck();
        }

        public void GoodsCheck()//override void OnAfterSimulationTick()
        {

            if (ACC_Options.Instance.CommercialGoodsMultiplier != 1.0f && (ACC_Options.Instance.pauseRefillEnable || !simulationManager.SimulationPaused))
            {

                Building building;
                BuildingInfo info;
                BuildingAI ai;

                for (var buildingId = (ushort)(simulationManager.m_currentTickIndex % 1000);
                    buildingId < buildingManager.m_buildings.m_buffer.Length;
                    buildingId += 1000)
                {
                    building = buildingManager.m_buildings.m_buffer[buildingId];

                    info = building.Info;
                    if (info == null) continue;

                    ai = info.GetAI() as BuildingAI;
                    if (ai == null) continue;

                    if (ai is CommercialBuildingAI)
                    {
                        //Resupply building based on delta between current tick and last tick.
                        int addAmount = 0;
                        int amount = building.m_customBuffer1;

                        if (!ACC_Options.Instance.refillBuildingsEnable && buildingGoodsCount.TryGetValue(buildingId, out int oldAmount))
                        {
                            if (amount < oldAmount)
                            {
                                int amountDelta = amount - oldAmount;
                                addAmount = System.Math.Abs(amountDelta) + (int)(amountDelta * ACC_Options.Instance.CommercialGoodsMultiplier);

                                ResupplyBuilding(buildingId, ai, TransferManager.TransferReason.Goods, addAmount);

                                int curAmt = buildingManager.m_buildings.m_buffer[buildingId].m_customBuffer1;
                                buildingGoodsCount[buildingId] = curAmt;

                                if (ACC_Options.Instance.DebugConsolePrint)
                                {
                                    GoodsReplenishAmount += addAmount;
                                    GoodsOldTotalAmount += oldAmount;
                                    GoodsTotalAmount += curAmt;
                                    GoodsReadAmount++;
                                }
                            }
                            else
                            {
                                buildingGoodsCount[buildingId] = amount;
                                GoodsReadAmount++;
                            }
                        }
                        else if (ACC_Options.Instance.refillBuildingsEnable)
                        {
                            ResupplyBuilding(buildingId, ai, TransferManager.TransferReason.Goods, 10000);

                            if (ACC_Options.Instance.DebugConsolePrint)
                            {
                                int curAmt = buildingManager.m_buildings.m_buffer[buildingId].m_customBuffer1;

                                GoodsNewAmount++;
                                GoodsReplenishAmount += curAmt - amount;
                                GoodsOldTotalAmount += amount;
                                GoodsTotalAmount += curAmt;
                            }
                        }
                        else
                        {
                            buildingGoodsCount.Add(buildingId, amount);
                            GoodsNewAmount++;
                        }
                    }
                }
            }
        }

        private void ResupplyBuilding(ushort buildingId, BuildingAI buildingAi, TransferManager.TransferReason goodsType, int transferAmount)
        {
            refTransferAmt = transferAmount;
            buildingAi.ModifyMaterialBuffer(buildingId, ref buildingManager.m_buildings.m_buffer[buildingId], goodsType, ref refTransferAmt);
        }
    }

    public class CoTimer : MonoBehaviour
    {
        private IEnumerator DebugTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(2.0f);

                if (ACC_Options.Instance.DebugConsolePrint)
                {
                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, "ACC - Added/Old Total/Total/Reads/New: "
                        + string.Format("{0:n0}", GoodsMonitor.GoodsReplenishAmount) + "/"
                        + string.Format("{0:n0}", GoodsMonitor.GoodsOldTotalAmount) + "/"
                        + string.Format("{0:n0}", GoodsMonitor.GoodsTotalAmount) + "/"
                        + string.Format("{0:n0}", GoodsMonitor.GoodsReadAmount) + "/"
                        + string.Format("{0:n0}", GoodsMonitor.GoodsNewAmount));

                    GoodsMonitor.GoodsReplenishAmount = 0;
                    GoodsMonitor.GoodsOldTotalAmount = 0;
                    GoodsMonitor.GoodsTotalAmount = 0;
                    GoodsMonitor.GoodsReadAmount = 0;
                    GoodsMonitor.GoodsNewAmount = 0;
                }
            }
        }
    }
}
