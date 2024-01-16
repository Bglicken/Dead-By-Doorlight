using DunGen;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dead_By_Doorlight
{
    public class Patches
    {
        public static List<TerminalAccessibleObject> objects = new List<TerminalAccessibleObject>();
        private static List<string> doorCodes = new List<string>();


        static void postfix(ref Terminal __instance)
        {
            if (!StartOfRound.Instance.shipHasLanded)
            {
                TerminalAccessibleObject[] array = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
                if (array == null || array.Length <= 0)
                    return;
                objects = array.ToList();

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
                }

                

            }
        }
        private static string recentMessage = "";

        [HarmonyPatch(typeof(HUDManager), "AddPlayerChatMessageClientRpc")]
        [HarmonyPostfix]
        
        public static void AddPlayerChatMessageClientRpcPostfix(HUDManager __instance, string chatMessage, ref bool ___isPlayerDead)
        {
            
            recentMessage = chatMessage;
            foreach (var doorcode in doorCodes)
            {
                if (doorcode.Equals(recentMessage))
                {
                    foreach (var obj in objects)
                    {
                        if (obj.objectCode.Equals(doorcode))
                            obj.SetDoorToggleLocalClient();
                        Debug.Log("door " + doorcode + "Has been affected");
                    }
                }
            }
         
        }
    }
}
