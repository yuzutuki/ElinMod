using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BepInEx;
using HarmonyLib;

namespace yz_ShinySpell
{
    //詠唱処理を移動しようとしてうまくいかなかった残骸的なやつ
    /*
     * たぶんHarmonyパッチ先が特定できていない
     * メソッドの引数をしていすればいけるかも
    [HarmonyPatch(typeof(Chara), nameof(Chara.UseAbility))]
    public class ReadCast : yzExParticle
    {
        public static void Prefix(Act a,Point pos = null)
        {
            Debug.Log("ねねうねうう");
            //Debug.Log($"use:{a.Name},pos:{pos}");
        }
    }
    */
}