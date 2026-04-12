//public void DamageHP(long dmg, int ele, int eleP = 100, AttackSource attackSource = AttackSource.None, Card origin = null, bool showEffect = true, Thing weapon = null, Chara originalTarget = null)
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace yz_sss
{
    //遠隔攻撃実行時、処理後。
    //サバーニャエフェクト用
    [HarmonyPatch(typeof(Card), nameof(Card.DamageHP))]
    [HarmonyPatch(new Type[] { typeof(long), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool), typeof(Thing), typeof(Chara) })]
    internal class PatchCardDamageHP
    {
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