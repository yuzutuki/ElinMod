
using BepInEx;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace yz_sss
{
    public class ModConfigCore : BaseUnityPlugin
    {
        static public void ModConfigSetting()
        {
            foreach (var obj in ModManager.ListPluginObject)
            {
                var plugin = obj as BaseUnityPlugin;
                if (plugin.Info.Metadata.GUID == YzSSS.ModOptionsGuid)
                {
                    ModOptionController controller = ModOptionController.Register(YzSSS.guid, "Tooltip", new object[] { });
                    if (controller == null)
                        break;
                    //翻訳テキスト
                    controller.SetTranslation(YzSSS.guid, "StrangeSpellShop", "不思議な魔法のお店", "StrangeSpellShop");
                    controller.SetTranslation("Tooltip", "StrangeSpellShop Config", "不思議な魔法のお店の設定", "StrangeSpellShop Config");
                    controller.SetTranslation("c1", "SpPopup", "大技使用時のポップアップ", "Special spell popups");
                    controller.SetTranslation("c1tip", "Enable special spell popups", "大技使用時のポップアップを有効にします", "启用释放强力技能时的弹出提示");
                    controller.SetTranslation("c2", "No Spellbook Random Generation", "魔法書の生成制限", "No Spellbook Random Generation");
                    controller.SetTranslation("c2tip", "Prevents spellbooks from this mod from generating in regular magic shops and dungeons,aud under similar conditions", "通常の魔法屋やダンジョンなどでこのMODの魔法書が生成されないようにします。", "防止本MOD的魔法书在普通的魔法商店、地下城等情况下生成");
                    controller.SetTranslation("c3", "Goblin Punch Requirement Relaxation", "ゴブリンパンチ強化条件緩和", "Goblin Punch Requirement Relaxation");
                    controller.SetTranslation("c3tip", "When disabled, the requirements become closer to the original behavior and its power increases.", "ゴブリンパンチの強化条件を緩和します。無効にすると元ネタに近い条件になり、威力が強化されます。", "放宽哥布林拳的强化条件。禁用时将更接近原始机制，威力会提升。");
                    controller.SetTranslation("c4", "Self-Target Prevention", "自傷防止", "Self-Target Prevention");
                    controller.SetTranslation("c4tip", "Causes certain offensive spells to automatically fail when the caster targets themselves", "一部の攻撃魔法で詠唱者自身を対象にしたときに、自動失敗するようになります。", "当施法者将自己作为目标时，某些攻击性法术会自动失败。");
                    //UI構築
                    controller.OnBuildUI += delegate (OptionUIBuilder build)
                    {
                        OptVLayout root = build.Root;
                        VerticalLayoutGroup vlg = (root != null) ? root.Base : null;
                        build.Root.AddToggle(controller.Tr("c1"), ModConfig.SpPop.Value, 16, controller.Tr("c1tip")).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.SpPop.Value = v;

                        };
                        //
                        build.Root.AddToggle(controller.Tr("c2"), ModConfig.SpawnRestrict.Value, 16, controller.Tr("c2tip")).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.SpawnRestrict.Value = v;
                            ResetSpawnChance();
                        };
                        //
                        build.Root.AddToggle(controller.Tr("c3"), ModConfig.GpLimit.Value, 16, controller.Tr("c3tip")).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.GpLimit.Value = v;
                        };
                        //
                        build.Root.AddToggle(controller.Tr("c4"), ModConfig.SelfCastLock.Value, 16, controller.Tr("c4tip")).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.SelfCastLock.Value = v;
                        };
                    };
                    Debug.Log("-= SSS:ModOptions適用完了? =-");
                }
            }
            
        }
        //
        //生成率のリセット
        static public void ResetSpawnChance()
        {
            //基準値(720012:アナライズ以降)
            int originID = 720012;
            //とりあえず+100まで
            for (int i = 0; i < 100; i++)
            {
                int id = originID + i;
                EClass.sources.elements.map.TryGetValue(id, out SourceElement.Row row);
                if (row != null)
                {
                    if (row.alias.Contains("SpYz") && row.thing.Contains("B"))
                    {

                        if (!ModConfig.SpawnRestrict.Value)
                        {
                            row.chance = 800;
                            Debug.Log($"SetChance:{id},{row.alias},{row.chance}");
                        }
                        else
                        {
                            row.chance = 0;
                            Debug.Log($"ResetChance:{id},{row.alias},{row.chance}");
                        }
                    }
                    else
                    {
                        Debug.Log($"Skip:{id},{row.alias},{row.name}");
                    }
                }
                else
                {
                    //Debug.LogError($"MissingElement:{id}");
                }
            }

        }
        //
    }
}