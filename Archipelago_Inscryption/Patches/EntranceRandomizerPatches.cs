using Archipelago_Inscryption.Archipelago;
using Archipelago_Inscryption.Helpers;
using Archipelago_Inscryption.Utils;
using DiskCardGame;
using GBC;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Archipelago_Inscryption.Patches
{
    [HarmonyPatch]
    internal class EntranceRandomizerPatches
    {
        [HarmonyPatch(typeof(BrokenBridgeEntrance), "BridgeFixed")]
        [HarmonyPostfix]
        static void FixBridgeOnAnyScrybe(ref bool __result)
        {
            __result = StoryEventsData.EventCompleted(StoryEvent.GBCGrimoraDefeated) || StoryEventsData.EventCompleted(StoryEvent.GBCLeshyDefeated)
             || StoryEventsData.EventCompleted(StoryEvent.GBCPoeDefeated) || StoryEventsData.EventCompleted(StoryEvent.GBCMagnificusDefeated);
        }

        [HarmonyPatch(typeof(ZoneEntrance), "ChangeScene")]
        [HarmonyPrefix]
        static void ChangeMapTransitionDestination(ZoneEntrance __instance)
        {
            switch (__instance.transform.GetPath()) {
                case "Map/NavigationGrid/Nature Temple":
                    __instance.zoneSceneId = "GBC_Temple_Tech";
                    break;
                case "Map/NavigationGrid/Undead Temple":
                    __instance.zoneSceneId = "GBC_Temple_Wizard";
                    break;
                case "Map/NavigationGrid/Tech Elevator":
                    __instance.zoneSceneId = "GBC_Temple_Nature";
                    break;
                case "Map/NavigationGrid/Wizard Temple":
                    __instance.zoneSceneId = "GBC_Temple_Undead";
                    break;
                case "Map/NavigationGrid/Nature Docks":
                    __instance.zoneSceneId = "GBC_Mycologist_Hut";
                    break;
                case "Map/NavigationGrid/MycologistHut":
                    __instance.zoneSceneId = "GBC_Docks";
                    break;
            }
            string objectPath = __instance.transform.GetPath();
            ArchipelagoModPlugin.Log.LogMessage(objectPath);
        }

        [HarmonyPatch(typeof(SceneTransitionVolume), "ChangeScene")]
        [HarmonyPrefix]
        static void ChangeSceneTransitionDestination(SceneTransitionVolume __instance)
        {
            switch (__instance.transform.GetPath()) {
                case "Temple/OutdoorsCentral/ReturnToMapVolume":
                    __instance.sceneId = "GBC_Temple_Undead";
                    break;
            }
            string objectPath = __instance.transform.GetPath();
            ArchipelagoModPlugin.Log.LogMessage(objectPath);
        }

        [HarmonyPatch(typeof(RoomTransitionVolume), "TransitionSequence")]
        [HarmonyPrefix]
        static void ChangeRoomTransitionDestination(RoomTransitionVolume __instance)
        {
            if (SceneLoader.ActiveSceneName == "GBC_Temple_Nature")
            {
                switch (__instance.transform.GetPath()) {
                    case "Temple/OutdoorsCentral/Cabin/ChangeRoomVolume":
                        __instance.destinationRoom.name = "Shop";
                        __instance.exitMarker.position = new Vector2(9.755f, 10f);
                        break;
                    case "Temple/Shop/Floor/ChangeRoomVolume":
                        __instance.destinationRoom.name = "OutdoorsCentral";
                        __instance.exitMarker.position = new Vector2(-0.09f, -0.205f);
                        break;
                }
            }
            else if (SceneLoader.ActiveSceneName == "GBC_Temple_Wizard")
            {
                switch (__instance.transform.GetPath()) {
                    case "Temple/Floor_1/Floor/ChangeRoomVolume_Shop":
                        SaveData.Data.overworldIndoorPosition = Vector2.zero;
                        __instance.destinationRoom.name = "";
                        __instance.exitMarker.position = new Vector2(9.755f, 10f);
                        break;
                }
            }
            string objectPath = __instance.transform.GetPath();
            ArchipelagoModPlugin.Log.LogMessage(objectPath);
        }
    }
}