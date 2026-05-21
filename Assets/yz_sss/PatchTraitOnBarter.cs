using HarmonyLib;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;


namespace yz_sss
{
    //店員さんへの商品設定
    [HarmonyPatch(typeof(Trait), nameof(Trait.OnBarter))]
    [HarmonyPatch(new Type[] { typeof(bool) })]
    internal class PatchTraitOnBarter
    {
        [HarmonyPostfix]
        private static void Postfix(Trait __instance)
        {
            //Debug.Log("OnBarter called");
            if (__instance.owner.id == "yz_nagisa")
            {
                //Debug.Log("Setting up shop inventory for yz_nagisa");
                if (__instance.owner.isRestocking)
                {
                    //Debug.Log("Restocking shop inventory");
                    //商人カバン取得
                    var inv = __instance.owner.things.Find("chest_merchant");
                    if (inv is null)
                    {
                        inv = ThingGen.Create("chest_merchant");
                        __instance.owner.AddThing(inv);
                    }
                    //在庫を空に
                    inv.RemoveThings();
                    //商品を追加
                    //固定商品
                    //var thing = ThingGen.Create("snow");
                    inv.AddThing(ThingGen.Create("YzLearnAutomap"));
                    //魔法書
                    List<int> idList = new List<int>(YzSSS.SpellIDList);
                    foreach (int id in idList)
                    {
                        inv.AddThing(ThingGen.CreateSpellbook(id, 1, 3 + EClass.rnd(5)));
                    }
                    //ランダム追加
                    for (int i = 0; i < 32; i++)
                    {
                        inv.AddThing(ThingGen.CreateSpellbook(idList.RandomItem(), 1, 2 + EClass.rnd(5)));
                    }
                    //新しめのやつおおめに
                    if (idList.Count > 16)
                    {
                        idList.RemoveRange(0, idList.Count - 15);
                        for (int i = 0; i < 8; i++)
                        {
                            inv.AddThing(ThingGen.CreateSpellbook(idList.RandomItem(), 1, 3 + EClass.rnd(4)));
                        }
                    }
                    if (idList.Count > 6)
                    {
                        idList.RemoveRange(0, idList.Count - 5);
                        for (int i = 0; i < 4; i++)
                        {
                            inv.AddThing(ThingGen.CreateSpellbook(idList.RandomItem(), 1, 3 + EClass.rnd(4)));
                        }
                    }
                    if (idList.Count > 6)
                    {
                        idList.RemoveRange(0, idList.Count - 5);
                        for (int i = 0; i < 4; i++)
                        {
                            inv.AddThing(ThingGen.CreateSpellbook(idList.RandomItem(), 1, 4 + EClass.rnd(3)));
                        }
                    }
                    //コンテナサイズの修正
                    inv.things.ChangeSize(8, 12);
                    //魔法書全鑑定と呪い祝福の解除。通常インベントリ内も鑑定されてしまうけどヨシ！

                    __instance.owner.things.Foreach((t) =>
                    {
                        //Debug.Log($"Thing: {t.id}, Category: {t.category.id}, IsIdentified: {t.IsIdentified}");
                        if (t.category.id == "spellbook")
                        {
                            if (!t.IsIdentified)
                            {
                                t.Identify(false);
                            }
                            if (t.blessedState != BlessedState.Normal)
                            {
                                t.blessedState = BlessedState.Normal;
                            }
                        }
                    });

                }
            }
        }
    }
}