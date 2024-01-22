using BepInEx.Logging;
using DunGen;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dead_By_Doorlight
{
    [HarmonyPatch(typeof(HUDManager))]
    [HarmonyPatch(typeof(TerminalAccessibleObject))]



    internal class Patches
    {

        public static List<TerminalAccessibleObject> objects = new List<TerminalAccessibleObject>();
        private static List<string> doorCodes = new List<string>();
        public static ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource("DeadByDoorlight.LCMod");


        public static List<String> getDoorCodes()
        {
            mls.LogDebug("The Action Has Been Successfully Called");
            TerminalAccessibleObject[] array = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
            if (array == null || array.Length <= 0)
                return null;
            objects = array.ToList();
            mls.LogInfo(objects.ToString());

            List<TerminalAccessibleObject> OpenableDoors = new List<TerminalAccessibleObject>();

            foreach (var obj in objects)
            {
                if (obj.gameObject.name.Contains("BigDoor"))
                {
                    OpenableDoors.Add(obj);
                }
            }

            objects = OpenableDoors;

            foreach (var door in objects)
            {
                doorCodes.Add(door.objectCode);
                mls.Log(LogLevel.Info, door.objectCode);

            }
            return doorCodes;
        }
        private static string recentMessage = "";

        [HarmonyPatch(typeof(HUDManager), "AddPlayerChatMessageClientRpc")]
        [HarmonyPostfix]

        public static void AddPlayerChatMessageClientRpcPostfix(HUDManager __instance, string chatMessage, int playerId)
        {



            recentMessage = chatMessage;
            mls.Log(LogLevel.Info, recentMessage);

            doorCodes = getDoorCodes();
            mls.LogInfo(doorCodes);

            bool isdead = HUDManager.Instance.playersManager.allPlayerScripts[playerId].isPlayerDead;

            if (isdead)
            {
                foreach (var doorcode in doorCodes)
                {
                    foreach (var obj in objects)
                    {
                        if (recentMessage.Equals("open"))
                        {
                            if (obj.objectCode.Equals(doorcode))
                            {
                                obj.SetDoorOpen(true);
                                mls.LogInfo("door " + doorcode + " has been affected");
                            }

                        }
                        else if (recentMessage.Equals("close"))
                        {
                            if (obj.objectCode.Equals(doorcode))
                            {
                                obj.SetDoorOpen(false);
                                mls.LogInfo("door " + doorcode + " has been affected");
                            }
                        }
                        
                    }

                }

            }
            else
            {
                mls.LogInfo("Currently Player is not currently dead");
            }
        }
    }
}
