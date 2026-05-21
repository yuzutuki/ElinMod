using System;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;
using UnityEngine;
//26/05/18 SSSから移植
namespace yz_ReSS
{
    public static class ModConfig
    {
        //仮データ
        public static void LoadConfig(ConfigFile config)
        {
            /*
            ModConfig.SpPop = config.Bind<bool>("General", "SpPopup", true, "Enable special spell popups");
            ModConfig.SpawnRestrict = config.Bind<bool>("General", "SpawnRestriction", true, "No Spellbook Generation");
            ModConfig.GpLimit = config.Bind<bool>("General", "GoblinPunch", true, "Goblin Punch Limit");
            ModConfig.SelfCastLock = config.Bind<bool>("General", "SelfCastLock", true, "Lock self-cast");
            ModConfig.AutoMapPlus = config.Bind<bool>("General", "AutoMapPlus", true, "AutoMap+SeeInv");
            */
            Debug.Log("-= ReSS:Config Loaded =-");
        }
        /*
         * コンフィグ項目予定リスト：有効無効をわかりやすくする
         * バニラエフェクトを有効化
         * 効果範囲の可視化
         * 詠唱エフェクト
         * 各種魔法エフェクト
         * 属性の色の調整:無効化でバニラのものを使用
         */
        /*
        public static ConfigEntry<bool> SpPop;
        public static ConfigEntry<bool> SpawnRestrict;
        public static ConfigEntry<bool> GpLimit;
        public static ConfigEntry<bool> SelfCastLock;
        public static ConfigEntry<bool> AutoMapPlus;
        */
    }
}