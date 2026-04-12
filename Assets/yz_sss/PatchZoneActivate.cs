using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;


namespace yz_sss
{
    //ゾーン読み込み時
    //オートマッピング用
    [HarmonyPatch(typeof(Zone), nameof(Zone.Activate))]
    internal class PatchZoneActivate
    {
        [HarmonyPostfix]
        private static void Postfix(Zone __instance)
        {
            //オートマッピング
            if (EClass.pc.HasCondition<StYzAutomap>())
            {
                //EClass._zone.FeatureType != ZoneFeatureType.RandomField ||
                Zone z = EClass._zone;
                if (z.IsRegion || z.IsPCFactionOrTent || z.IsTown)
                    return;
                if (z.IsNefia || z.IsInstance || z.AllowCriminal || z.FeatureType == ZoneFeatureType.RandomField)
                {
                    Chara c = EClass.pc;
                    //Debug.Log("ゾーン遷移");
                    //c.Say("abMagicMap", c, null, null);
                    //8260:魔法の地図
                    Element mm = c.elements.GetOrCreateElement(8260);
                    //8770:テレパシー
                    Element tl = c.elements.GetOrCreateElement(8770);
                    if (mm.Potential - 100 < 1 || tl.Potential - 100 < 1)
                    {
                        Msg.Say("noSpellStock");
                        c.RemoveCondition<StYzAutomap>();
                        return;
                    }
                    mm.vPotential -= 1;
                    tl.vPotential -= 1;
                    int cost = mm.GetCost(c).cost + tl.GetCost(c).cost;
                    c.mana.Mod(-cost);
                    Debug.Log($"vExp:{mm.vExp},next:{mm.ExpToNext}");
                    //					Debug.Log($"cost:{mm.GetCost(c).cost},lv:{mm.Value},Potential:{mm.Potential},stack:{mm.Potential - 100}");
                    //					Debug.Log($"cost:{tl.GetCost(c).cost},lv:{tl.Value},Potential:{tl.Potential},stack:{tl.Potential - 100}");
                    int pow = YzWrapper.GetPower(c, 8260);
                    int pow2 = YzWrapper.GetPower(c, 8770);
                    //Debug.Log($"magicMapPow:{pow}");
                    //Debug.Log($"telepathyPow:{pow2}");
                    c.ModExp(mm.id, c.elements.GetSpellExp(c, mm, 80));
                    c.ModExp(tl.id, c.elements.GetSpellExp(c, tl, 80));
                    EClass._map.Reveal(c.pos, pow);
                    c.AddCondition<ConTelepathy>(pow2);
                }
            }
        }
    }
}