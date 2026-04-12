using System;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;
using UnityEngine;

namespace yz_sss
{
    public static class ModConfig
    {
        public static void LoadConfig(ConfigFile config)
        {
            ModConfig.SpPop = config.Bind<bool>("General", "SpPopup", true, "Enable special spell popups");
            ModConfig.SpawnRestrict = config.Bind<bool>("General", "SpawnRestriction", true, "No Spellbook Generation");
            ModConfig.GpLimit = config.Bind<bool>("General", "GoblinPunch", true, "Goblin Punch Limit");
            ModConfig.SelfCastLock = config.Bind<bool>("General", "SelfCastLock", true, "Lock self-cast");
            Debug.Log("-= SSS:Config Loaded =-");
            Debug.Log($"-=SSS:flagCheck,{ModConfig.SpPop.Value},{ModConfig.SpawnRestrict.Value},{ModConfig.GpLimit.Value},{ModConfig.SelfCastLock.Value}");
        }
        public static ConfigEntry<bool> SpPop;
        public static ConfigEntry<bool> SpawnRestrict;
        public static ConfigEntry<bool> GpLimit;
        public static ConfigEntry<bool> SelfCastLock;
    }
}