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
            ModConfig.VanillaEffect = config.Bind<bool>("General", "VanillaEffect", true, "Use vanilla spell effects");
            ModConfig.ShowEffectiveArea = config.Bind<bool>("General", "ShowEffectiveArea", true, "Show effective area");
            ModConfig.ShowOutFovEffect = config.Bind<bool>("General", "ShowOutFovEffect", true, "Show out of fov effect");
            ModConfig.ShowCastEffect = config.Bind<bool>("General", "ShowCastEffect", true, "Show cast effect");
            ModConfig.MoonExEffect = config.Bind<bool>("General", "MoonExEffect", true, "Show moon spear extra effect");
            ModConfig.EffectTarget = config.Bind<int>("General", "EffectTarget", 0, "Effect target");
            ModConfig.MoreEffect = config.Bind<int>("General", "MoreEffect", 2, "More effect");
            ModConfig.ReplaceColor = config.Bind<bool>("General", "ReplaceColor", true, "Replace color");
            ModConfig.EffectAlpha = config.Bind<float>("General", "EffectAlpha", 0.8f, "alpha");
            ModConfig.SatMult = config.Bind<float>("General", "SatMult", 0.8f, "Saturation multiplier");
            ModConfig.ValMult = config.Bind<float>("General", "ValMult", 1f, "Value multiplier");
            ModConfig.ShowArrow = config.Bind<bool>("General", "ShowArrow", true, "Show arrow effect");
            ModConfig.ShowMoonArrow = config.Bind<bool>("General", "ShowMoonArrow", true, "Show moon arrow effect");
            ModConfig.ShowMoonSpear = config.Bind<bool>("General", "ShowMoonSpear", true, "Show moon spear effect");
            ModConfig.ShowBolt = config.Bind<bool>("General", "ShowBolt", true, "Show bolt effect");
            ModConfig.ShowBall = config.Bind<bool>("General", "ShowBall", true, "Show ball effect");
            ModConfig.ShowSword = config.Bind<bool>("General", "ShowSword", true, "Show sword effect");
            ModConfig.ShowHand = config.Bind<bool>("General", "ShowHand", true, "Show hand effect");
            ModConfig.ShowHeal = config.Bind<bool>("General", "ShowHeal", true, "Show heal effect");
            ModConfig.ShowBuff = config.Bind<bool>("General", "ShowBuff", true, "Show buff effects");
            ModConfig.ShowDebuff = config.Bind<bool>("General", "ShowDebuff", true, "Show debuff effects");
            ModConfig.ShowExBuff = config.Bind<bool>("General", "ShowExBuff", true, "Show extra buff effects");
            ModConfig.ShowExDebuff = config.Bind<bool>("General", "ShowExDebuff", true, "Show extra debuff effects");
            ModConfig.ShowMiasma = config.Bind<bool>("General", "ShowMiasma", true, "Show miasma effects");
            ModConfig.ShowMagicMap = config.Bind<bool>("General", "ShowMagicMap", true, "Show magic map effects");
            ModConfig.ShowBreathe = config.Bind<bool>("General", "ShowBreathe", true, "Show breathe effects");
            ModConfig.ShowQuake = config.Bind<bool>("General", "ShowQuake", true, "Show earthquake effects");
            ModConfig.ShowFlare = config.Bind<bool>("General", "ShowFlare", true, "Show flare effects");
            ModConfig.ShowFunnel = config.Bind<bool>("General", "ShowFunnel", true, "Show funnel effects");

            Debug.Log("-= ReSS:Config Loaded =-");
        }
        public static ConfigEntry<bool> VanillaEffect;
        public static ConfigEntry<bool> ShowEffectiveArea;
        public static ConfigEntry<bool> ShowOutFovEffect;
        public static ConfigEntry<bool> ShowCastEffect;
        public static ConfigEntry<bool> MoonExEffect;
        public static ConfigEntry<int> EffectTarget;
        public static ConfigEntry<int> MoreEffect;
        public static ConfigEntry<bool> ReplaceColor;
        public static ConfigEntry<float> SatMult;
        public static ConfigEntry<float> ValMult;
        public static ConfigEntry<float> EffectAlpha;
        public static ConfigEntry<bool> ShowArrow;
        public static ConfigEntry<bool> ShowMoonArrow;
        public static ConfigEntry<bool> ShowMoonSpear;
        public static ConfigEntry<bool> ShowBolt;
        public static ConfigEntry<bool> ShowBall;
        public static ConfigEntry<bool> ShowSword;
        public static ConfigEntry<bool> ShowHand;
        public static ConfigEntry<bool> ShowHeal;
        public static ConfigEntry<bool> ShowBuff;
        public static ConfigEntry<bool> ShowDebuff;
        public static ConfigEntry<bool> ShowExBuff;
        public static ConfigEntry<bool> ShowExDebuff;
        public static ConfigEntry<bool> ShowMiasma;
        public static ConfigEntry<bool> ShowBreathe;
        public static ConfigEntry<bool> ShowMagicMap;
        public static ConfigEntry<bool> ShowQuake;
        public static ConfigEntry<bool> ShowFlare;
        public static ConfigEntry<bool> ShowFunnel;
    }
}