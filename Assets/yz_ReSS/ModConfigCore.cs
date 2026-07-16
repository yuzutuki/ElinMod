using BepInEx;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;
using HarmonyLib;
using Steamworks;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//26/05/18 SSSから移植

namespace yz_ReSS
{
    public class ModConfigCore : BaseUnityPlugin
    {
        static public void ModConfigSetting(string path)
        {
            Debug.Log(" -= ReSS Config 読み込み… =-");
            foreach (var obj in ModManager.ListPluginObject)
            {
                var plugin = obj as BaseUnityPlugin;
                if (plugin.Info.Metadata.GUID == YzReSS.ModOptionsGuid)
                {
                    ModOptionController controller = ModOptionController.Register(YzReSS.guid, "ModTip", new object[] { });
                    if (controller == null)
                        break;
                    //翻訳テキスト読み込み
                    controller.SetTranslation(YzReSS.guid, "ReShinySpell", "ReShinySpell", "ReShinySpell");
                    ModConfigTranslation.Regist(controller, path);

                    //UI構築
                    controller.OnBuildUI += delegate (OptionUIBuilder build)
                    {
                        OptVLayout root = build.Root;
                        VerticalLayoutGroup vlg = (root != null) ? root.Base : null;
                        //
                        build.Root.AddTopic(controller.Tr("Headline1"));

                        build.Root.AddText(controller.Tr("EffectTargetTip"));
                        //
                        //0すべて有効,1PC勢力のみ有効、2PCのみ有効、3すべて無効
                        build.Root.AddDropdown(new List<string> { controller.Tr("EffectTarget0"), controller.Tr("EffectTarget1"), controller.Tr("EffectTarget2"), controller.Tr("EffectTarget3") }, ModConfig.EffectTarget.Value).OnValueChanged += delegate (int v)
                        {
                            ModConfig.EffectTarget.Value = v;
                        };
                        /*
                        build.Root.AddToggle(controller.Tr("ShowEffectiveArea"), ModConfig.ShowEffectiveArea.Value,tooltip:controller.Tr("ShowEffectiveAreaTip")).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowEffectiveArea.Value = v;
                        };
                        */
                        build.Root.AddToggle(controller.Tr("ShowOutFovEffect"), ModConfig.ShowOutFovEffect.Value, tooltip: controller.Tr("ShowOutFovEffectTip")).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowOutFovEffect.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ReplaceColor"), ModConfig.ReplaceColor.Value, tooltip: controller.Tr("ReplaceColorTip")).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ReplaceColor.Value = v;
                        };
                        build.Root.AddText(controller.Tr("MoreEffectTip"));
                        build.Root.AddDropdown(new List<string> { controller.Tr("MoreEffect0"), controller.Tr("MoreEffect1"), controller.Tr("MoreEffect2"), controller.Tr("MoreEffect3") }, ModConfig.MoreEffect.Value).OnValueChanged += delegate (int v)
                        {
                            ModConfig.MoreEffect.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("VanillaEffect"), ModConfig.VanillaEffect.Value, tooltip: controller.Tr("VanillaEffectTip")).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.VanillaEffect.Value = v;
                        };
                        //
                        build.Root.AddText(" ");
                        build.Root.AddTopic(controller.Tr("Headline3"));
                        build.Root.AddText(controller.Tr("Headline3tip"));
                        //
                        var sliderS = build.Root.AddSlider(controller.Tr("SatMult") + ":" + (ModConfig.SatMult.Value * 100).ToString() + "%", false, 0f, 1.5f, ModConfig.SatMult.Value);
                        sliderS.OnValueChanged += delegate (float v)
                        {
                            v = Mathf.Round(v * 100f) / 100f;//0.01刻み
                            ModConfig.SatMult.Value = v;
                            sliderS.Title = controller.Tr("SatMult") + ":" + (v * 100).ToString() + "%";
                        };
                        var sliderV = build.Root.AddSlider(controller.Tr("ValMult") + ":" + (ModConfig.ValMult.Value * 100).ToString() + "%", false, 0.05f, 1.5f, ModConfig.ValMult.Value);
                        sliderV.OnValueChanged += delegate (float v)
                        {
                            v = Mathf.Round(v * 100f) / 100f;//0.01刻み
                            ModConfig.ValMult.Value = v;
                            sliderV.Title = controller.Tr("ValMult") + ":" + (v * 100).ToString() + "%";
                        };
                        var sliderA = build.Root.AddSlider(controller.Tr("EffectAlpha") + ":" + (ModConfig.EffectAlpha.Value * 100).ToString() + "%", false, 0f, 1.5f, ModConfig.EffectAlpha.Value);
                        sliderA.OnValueChanged += delegate (float v)
                        {
                            v = Mathf.Round(v * 100f) / 100f;//0.01刻み
                            ModConfig.EffectAlpha.Value = v;
                            sliderA.Title = controller.Tr("EffectAlpha") + ":" + (v * 100).ToString() + "%";
                        };
                        //
                        build.Root.AddText(" ");
                        build.Root.AddTopic(controller.Tr("Headline2"));
                        //
                        build.Root.AddToggle(controller.Tr("ShowCastEffect"), ModConfig.ShowCastEffect.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowCastEffect.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowArrow"), ModConfig.ShowArrow.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowArrow.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowMoonArrow"), ModConfig.ShowMoonArrow.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowMoonArrow.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowMoonSpear"), ModConfig.ShowMoonSpear.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowMoonSpear.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("MoonExEffect"), ModConfig.MoonExEffect.Value, tooltip: controller.Tr("MoonExEffectTip")).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.MoonExEffect.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowBolt"), ModConfig.ShowBolt.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowBolt.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowBall"), ModConfig.ShowBall.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowBall.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowSword"), ModConfig.ShowSword.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowSword.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowHand"), ModConfig.ShowHand.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowHand.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowMiasma"), ModConfig.ShowMiasma.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowMiasma.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowFunnel"), ModConfig.ShowFunnel.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowFunnel.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowBreathe"), ModConfig.ShowBreathe.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowBreathe.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowFlare"), ModConfig.ShowFlare.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowFlare.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowQuake"), ModConfig.ShowQuake.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowQuake.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowHeal"), ModConfig.ShowHeal.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowHeal.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowMagicMap"), ModConfig.ShowMagicMap.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowMagicMap.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowBuff"), ModConfig.ShowBuff.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowBuff.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowDebuff"), ModConfig.ShowDebuff.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowDebuff.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowExBuff"), ModConfig.ShowExBuff.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowExBuff.Value = v;
                        };
                        build.Root.AddToggle(controller.Tr("ShowExDebuff"), ModConfig.ShowExDebuff.Value).OnValueChanged += delegate (bool v)
                        {
                            ModConfig.ShowExDebuff.Value = v;
                        };

                        build.Root.AddText(" \n ");
                    };
                    Debug.Log("-= ReSS:ModOptions適用完了? =-");
                }
            }
            
        }
        //
        static public bool CheckEffectTarget(Card CC)
        {
            int targetType = ModConfig.EffectTarget.Value;
            if (targetType == 0)
                return true;
            else if (targetType == 1)
                return CC.IsPCFactionOrMinion;
            else if (targetType == 2)
                return CC.IsPC;
            else if (targetType == 3)
                return false;
            return true;
        }
    }
}