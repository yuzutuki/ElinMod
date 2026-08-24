using HarmonyLib;
using MonoMod.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yz_sss;

//ゾーン遷移前処理
//魔紋削除処理に使う予定だったけど没
namespace yz_sss
{

    /*
    [HarmonyPatch(typeof(Chara), nameof(Chara.MoveZone))]
    [HarmonyPatch(new Type[] { typeof(Zone), typeof(ZoneTransition) })]
    internal class PatchCharaMoveZone
    {
        [HarmonyPrefix]
        private static bool Prefix(Chara __instance, Zone z, ZoneTransition transition)
        {
            Debug.Log("=== OnMoveZone ===");
            if (__instance.IsPC)
            {
            }
            return true;
        }
    }
    */

}
/*
if (EClass.pc.HasCondition<ConYzRLL>())
{
    Zone zone = __instance.currentZone;
    Debug.Log("currentZone: " + zone?.id+","+zone?.name);
    foreach (Thing t in EClass._map.things)
    {
        Debug.Log($"Search:{t?.Name}");
        if (t != null)
        {
            if (t.id == "YzRLL")
            {
                Debug.Log("===FindThing!: " + t?.Name);
                Debug.Log($"isDestroyed?: {t?.isDestroyed}");
                TraitYzRLL trait = t.trait as TraitYzRLL;
                if(trait != null)
                {
                    Debug.Log($"TraitOK");
                    trait.End();
                }
                else
                {
                    Debug.Log($"TraitNG");
                }
                //t.MakeEgg();
                //t.Destroy();
            }
        }
    }
    //EClass._map.FindThing("YzRLL").Destroy();
EClass.pc.RemoveCondition<ConYzRLL>();
                }
            */