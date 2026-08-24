//public void DamageHP(long dmg, int ele, int eleP = 100, AttackSource attackSource = AttackSource.None, Card origin = null, bool showEffect = true, Thing weapon = null, Chara originalTarget = null)
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using MonoMod.Cil;

namespace yz_sss
{

    [HarmonyPatch(typeof(Card), nameof(Card.DamageHP))]
    [HarmonyPatch(new Type[] { typeof(long), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool), typeof(Thing), typeof(Chara), typeof(int) })]
    internal class PatchCardDamageHP
    {
        //オスティかばう処理
        [HarmonyPrefix]
        private static bool Prefix(Card __instance, long dmg, int ele, int eleP = 100, AttackSource attackSource = AttackSource.None, Card origin = null, bool showEffect = true, Thing weapon = null, Chara originalTarget = null, int resistPenetrationLevel = 0)
        {
            if (!__instance.isChara)
                return true;
            if (!__instance.IsPC)
                return true;
            //
            if (__instance.hp < 0)
            {
                return true;
            }
            if (originalTarget == null)
            {
                //Debug.Log("is PC!");
                //if (attackSource != AttackSource.Hunger && attackSource != AttackSource.Finish)
                switch (attackSource)
                {
                    case AttackSource.Hunger:
                    case AttackSource.Finish:
                    case AttackSource.Wrath:
                    case AttackSource.Euthanasia:
                        return true;
                    default:
                        foreach (Chara chara in EClass._map.charas)
                        {
                            if (chara.IsPCFactionMinion && chara.hp > 0 && chara.HasCondition<StYzByside>())
                            {
                                //Debug.Log("osty HP:" + chara.hp);
                                if(__instance.pos.Distance(chara.pos) > 2)
                                {
                                    //Debug.Log("-==Teleport!");
                                    chara.Teleport(__instance.pos.GetRandomNeighbor(),true,true);
                                }
                                chara.DamageHP(dmg, ele, eleP, attackSource, origin, showEffect, weapon, __instance.Chara, resistPenetrationLevel);
                                return false;
                            }
                        }
                        break;
                }
            }
            //
            return true;
        }
        //鹿起爆処理
        [HarmonyPostfix]
        private static void Postfix(Card __instance)
        {
            //Debug.Log("DamageHP");
            if (!__instance.isChara)
                return;
            if (__instance.HasCondition<ConYzDeercoy>())
            {
                ConYzDeercoy con = __instance.GetCondition<ConYzDeercoy>();
                //Debug.Log("Baaaaa");
                con.Explosion(__instance.pos);
            }
        }
    }

}