using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace yz_sss
{

    //攻撃実行時、処理後。
    //サバーニャエフェクト用
    [HarmonyPatch(typeof(Act), nameof(Act.Perform))]
    [HarmonyPatch(new Type[] { })]
    internal class PatchActPerform
    {
        [HarmonyPostfix]
        private static void Postfix(Act __instance)
        {
            //Debug.Log("Perform");
            if (Act.TC != null && Act.CC.HasCondition<ConYzLockon>())
                YzUtil.PlayEffect("Lockon", Act.CC, Act.TC);
        }
    }
}