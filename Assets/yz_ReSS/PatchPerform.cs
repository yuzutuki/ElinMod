using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using HarmonyLib;

namespace yz_ReSS
{
    [HarmonyPatch(typeof(Act), nameof(Act.Perform))]
    [HarmonyPatch(new Type[] { })]
    public class PatchPerform
    {
        private static void Postfix(Act __instance)
        {
            //Debug.Log("PostPerform");
            //Debug.Log($"id:{__instance.id},name:{__instance.Name},{__instance._source.alias}");
            if(!ModConfig.ShowCastEffect.Value)
                return;
            if (!ModConfigCore.CheckEffectTarget(Act.CC))
                return;
            //
            SourceElement.Row ele = __instance._source;
            if (ele != null)
            {
                //Debug.Log($"group:{ele.group},id:{ele.id},alias:{ele.alias},category:{ele.category}");
                if (ele.group == "SPELL")
                {
                    string castEffectID = "";
                    //CastEffect
                    switch (ele.categorySub)
                    {
                        case "attack":
                            if(ele.abilityType.Contains("debuff"))
                            {
                                castEffectID = "castSummon";
                            }
                            else
                            {
                                castEffectID = "castBlack";
                            }
                            Debug.Log("attack");
                            break;
                        case "defense":
                            castEffectID = "castWhite";
                            Debug.Log("defense");
                            break;
                        case "util":
                            if (ele.abilityType.Contains("summon") || ele.alias.Contains("Summon"))
                            {
                                castEffectID = "castSummon";
                            }
                            else
                            {
                                castEffectID = "castBlue";
                            }
                            Debug.Log("util");
                            break;
                        default:
                            break;
                    }
                    //Debug.Log($"castEffectID:{castEffectID}");
                    //Debug.Log($"caster:{Act.CC.Name},pos:{Act.CC.pos}");
                    //
                    YzUtil.PlayEffect(castEffectID, Act.CC);
                }
            }
        }
    }
}