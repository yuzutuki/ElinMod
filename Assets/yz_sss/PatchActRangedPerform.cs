using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace yz_sss
{
    //遠隔攻撃実行時、処理後。
    //サバーニャエフェクト用
    [HarmonyPatch(typeof(ActRanged), nameof(ActRanged.Perform))]
    [HarmonyPatch(new Type[] { })]
    internal class PatchActRangedPerform
    {
        [HarmonyPostfix]
        private static void Postfix(ActRanged __instance)
        {
            //Debug.Log("PerformRange");
            if (Act.TC != null && Act.CC.HasCondition<ConYzLockon>())
                YzUtil.PlayEffect("Lockon", Act.CC, Act.TC);

        }
    }
}