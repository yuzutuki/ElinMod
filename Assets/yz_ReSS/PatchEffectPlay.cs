using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using HarmonyLib;

namespace yz_ReSS
{
    [HarmonyPatch(typeof(Effect), nameof(Effect._Play))]
    public class PatchEffectPlay
    {
        public static bool Prefix(Effect __instance,Point from, Vector3 fromV, float fixY = 0f, Point to = null, Sprite sprite = null)
        {
            //Debug.Log($"spriteName:{sprite?.texture?.name},sr:{__instance.sr?.name},{__instance.sr?.sprite?.name},{__instance.sr?.sprite?.texture?.name}");
            if (!ModConfig.VanillaEffect.Value)
            {
                if (__instance?.sr?.sprite?.name == "spell_arrow" || __instance?.sr?.sprite?.name == "spell_moonspear")
                {
                    if (ModConfigCore.CheckEffectTarget(Act.CC))
                    {
                        __instance.Kill();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return true;
        }
    }
}