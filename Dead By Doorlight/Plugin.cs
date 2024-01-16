using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dead_By_Doorlight
{
    [BepInPlugin(modGUID, modName, modVersion)]

    public class ModBase : BaseUnityPlugin
    {
        private const string modGUID = "DeadByDoorlight.LCMod";
        private const string modName = "Dead By Doorlight";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static ModBase Instance;

        internal ManualLogSource mls;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("Balls");

            harmony.PatchAll(typeof(ModBase));        }
    }

    
}
