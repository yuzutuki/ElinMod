using BepInEx;
using HarmonyLib;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_ReSS
{
    [HarmonyPatch(typeof(ActEffect), nameof(ActEffect.ProcAt))]
    public class PatchProcAt
    {
        public static void Postfix(ActEffect __instance, EffectId id, int power, BlessedState state, Card cc, Card tc, Point tp, bool isNeg, ActRef actRef = default(ActRef))
        {
            //
            if (!ModConfigCore.CheckEffectTarget(cc))
                return;
            //Debug.Log("ProcAt called");
            //Debug.Log($"rapid:{ActEffect.RapidCount},*Delay:{ActEffect.RapidDelay}");
            int delay = (int)((float)ActEffect.RapidCount * ActEffect.RapidDelay * 45);
            //Debug.Log($"delay:{delay}");
            //色の準備
            Color elementColor = Color.white;
            string aEle = actRef.aliasEle;
            if (id.ToString() == "DebuffStats" || id.ToString() == "BuffStats" ||
                aEle == null || aEle == "")
            {
                aEle = "_void";
                //Debug.Log($"missingElement?,{id}");
            }
            if (ModConfig.ReplaceColor.Value)
            {
                elementColor = ColorSetting.ElementColor.ContainsKey(aEle) ? ColorSetting.ElementColor[aEle] : Color.gray;
            }
            else
            {
                elementColor = EClass.setting?.elements[aEle]?.colorSprite ?? Color.gray;
            }
            if (elementColor == null || elementColor == Color.black)
                elementColor = Color.gray;
            /*
            //色調整(乗算で透明化しないように)
            elementColor.r = Mathf.Max(elementColor.r, 0.2f);
            elementColor.g = Mathf.Max(elementColor.g, 0.2f);
            elementColor.b = Mathf.Max(elementColor.b, 0.2f);
            */
            //攻撃エフェクトの処理
            //Debug.Log($"castID:{id},Alias:{actRef.aliasEle?.ToString() ?? "null?"},actName:{actRef.act?.Name?.ToString() ?? "null?"},n1:{actRef.n1?.ToString() ?? "null?"}");
            switch (id.ToString())
            {
                //手
                case "Hand":
                    if (ModConfig.ShowHand.Value)
                        YzUtil.DelayPlayEffect(delay, "spHand", cc, tc, TP: tp.Copy(), color: elementColor);
                    break;
                //剣
                case "Sword":
                    if (ModConfig.ShowSword.Value)
                        YzUtil.DelayPlayEffect(delay, "spSlash", cc, tc, TP: tp.Copy(), color: elementColor);
                    break;
                case "Arrow":
                    if (ModConfig.ShowArrow.Value)
                        YzUtil.DelayPlayEffect(delay, "spArrow1", cc, tc, TP: tp.Copy(), color: elementColor);
                    break;
                case "Ball":
                    if (ModConfig.ShowBall.Value)
                        YzUtil.DelayPlayEffect(delay, "spBall", cc, tc, TP: tp.Copy(), color: elementColor);
                    break;
                case "Bolt":
                    if (ModConfig.ShowBolt.Value)
                        YzUtil.DelayPlayEffect(delay, "spBolt", cc, tc, fromP: cc.pos.Copy(), TP: tp.Copy(), color: elementColor);
                    break;
                case "MoonArrow":
                    if (ModConfig.ShowMoonArrow.Value)
                    {
                        YzUtil.DelayPlayEffect(delay, "spArrow1", cc, tc, TP: tp.Copy(), color: new Color(0.3f, 0.8f, 0.5f));
                        if (ModConfig.MoonExEffect.Value)
                        {
                            YzUtil.DelayPlayEffect(delay, "MoonArrow", cc, tc, TP: tp.Copy(), AffterEffect: "MoonArrow2");
                        }
                    }
                    break;
                case "MoonSpear":
                    if (ModConfig.ShowMoonSpear.Value)
                    {
                        YzUtil.DelayPlayEffect(delay, "MoonSpear", cc, tc, TP: tp.Copy(), AffterEffect: (ModConfig.MoonExEffect.Value ? "MoonSpear2" : "MoonSpear2b"));
                        if (ModConfig.MoonExEffect.Value)
                        {
                            YzUtil.DelayPlayEffect(delay, "MoonArrow", cc, tc, TP: tp.Copy());
                        }
                    }
                    break;
                case "Breathe":
                    if (ModConfig.ShowBreathe.Value)
                    {
                        List<Point> list = EClass._map.ListPointsInArc(cc.Chara.pos, tp, 7, 35f);
                        if (list.Count == 0)
                        {
                            list.Add(cc.Chara.pos.Copy());
                        }
                        int count = 0;
                        foreach (Point p in list)
                        {
                            YzUtil.DelayPlayEffect(count, "Breathe", cc, TP: p.Copy(), color: elementColor);
                            count++;
                        }
                    }
                    break;
                case "Earthquake":
                    if (ModConfig.ShowQuake.Value)
                    {
                        YzUtil.DelayPlayEffect(delay, "Quake", cc, tc, TP: tp.Copy());
                    }
                    break;
                case "Flare":
                    if (ModConfig.ShowFlare.Value)
                    {
                        YzUtil.DelayPlayEffect(delay, "Flare", cc, tc, TP: tp.Copy(), color: elementColor);
                    }
                    break;
                case "Funnel":
                    if (ModConfig.ShowFunnel.Value)
                    {
                        YzUtil.DelayPlayEffect(delay, "Funnel", cc, tc, TP: tp.Copy(), color: elementColor);
                    }
                    break;
                //デバフとか処理
                case "Debuff":
                case "DebuffStats":
                    if (ModConfig.ShowDebuff.Value)
                        YzUtil.PlayEffect("spDebuff", cc, tc);
                    switch (actRef.n1)
                    {
                        //瘴気
                        case ("ConMiasma"):
                            if (ModConfig.ShowMiasma.Value)
                                YzUtil.PlayEffect("spMiasma", cc, tc, color: elementColor);
                            break;
                        //鈍足
                        case ("SPD"):
                            if (ModConfig.ShowExDebuff.Value)
                                YzUtil.PlayEffect("spDebuffSpd", cc, tc);
                            break;
                        //脆弱
                        case ("ConWeakness"):
                            if (ModConfig.ShowExDebuff.Value)
                                YzUtil.PlayEffect("spDebuffDef", cc, tc, color: new Color(0.5f, 0.7f, 1f));
                            //setColor: new Color(0.5f, 0.7f, 1f),
                            break;
                        //元素傷跡
                        case ("ConWeakResEle"):
                            if (ModConfig.ShowExDebuff.Value)
                                YzUtil.PlayEffect("spDebuffDef", cc, tc, color: new Color(0.4f, 1.0f, 0.4f));
                            //setColor: new Color(0.4f, 1.0f, 0.4f),
                            break;
                        //ナイトメア
                        case ("ConNightmare"):
                            if (ModConfig.ShowExDebuff.Value)
                                YzUtil.PlayEffect("spDebuffDef", cc, tc, color: new Color(0.7f, 0.3f, 0.7f));
                            //setColor: new Color(0.7f, 0.3f, 0.7f),
                            break;
                            //グラビティ
                        case ("ConGravity"):
                            if (ModConfig.ShowExDebuff.Value)
                                YzUtil.PlayEffect("Gravity", cc, tc);
                            //setColor: new Color(0.7f, 0.3f, 0.7f),
                            break;
                            //災い
                        case ("ConBane"):
                            if (ModConfig.ShowExDebuff.Value)
                                YzUtil.PlayEffect("Bane", cc, tc);
                            //setColor: new Color(0.7f, 0.3f, 0.7f),
                            break;
                        //汎用
                        default:
                            break;
                    }
                    break;
                //この辺にバフ処理
                case "Buff"://case EffectId.Buff:
                case "BuffStats":
                    if (ModConfig.ShowBuff.Value)
                        YzUtil.PlayEffect("spBuff", cc, tc);
                    switch (actRef.n1)
                    {
                        //加速
                        case ("SPD"):
                            if (ModConfig.ShowExBuff.Value)
                                YzUtil.PlayEffect("spBuffSpd", cc, tc);
                            break;
                        //盾
                        case ("ConHolyShield"):
                            if (ModConfig.ShowExBuff.Value)
                                YzUtil.PlayEffect("spBuffShield", cc, tc, color: new Color(0.8f, 0.8f, 1f));
                            // setColor: new Color(0.8f, 0.8f, 1f, 0.6f));
                            break;
                        //元素保護
                        case ("ConResEle"):
                            if (ModConfig.ShowExBuff.Value)
                                YzUtil.PlayEffect("spBuffShield2", cc, tc, color: new Color(0.8f, 1f, 0.8f));
                            // setColor: new Color(0.8f, 1f, 0.8f, 0.4f));
                            break;
                        //猫の目
                        case ("ConNightVision"):
                            if (ModConfig.ShowExBuff.Value)
                                YzUtil.PlayEffect("BuffDEX", cc, tc);
                            break;
                        //英雄
                        case ("ConHero"):
                            if (ModConfig.ShowExBuff.Value)
                                YzUtil.PlayEffect("BuffSTR", cc, tc);
                            break;
                        //知者の加護
                        case ("ConWisdom"):
                            if (ModConfig.ShowExBuff.Value)
                                YzUtil.PlayEffect("BuffINT", cc, tc);
                            break;
                        //浮遊
                        case ("ConLevitate"):
                            if (ModConfig.ShowExBuff.Value)
                                YzUtil.PlayEffect("Levitation", cc, tc);
                            break;
                        //テレパシー
                        case ("ConTelepathy"):
                            if (ModConfig.ShowExBuff.Value)
                                YzUtil.PlayEffect("Telepathy", cc, tc);
                            break;
                        //唄
                        case ("ConWeapon"):
                            if (ModConfig.ShowExBuff.Value)
                                YzUtil.PlayEffect("Song", cc, tc, color: elementColor);
                            break;
                        default:
                            break;
                    }
                    break;
                //魔法の地図
                case "MagicMap":
                    if (ModConfig.ShowMagicMap.Value)
                        YzUtil.PlayEffect("spMap", cc);
                    break;
                case "Heal":
                case "HealComplete":
                    {
                        if (ModConfig.ShowHeal.Value)
                            YzUtil.PlayEffect("Heal1", cc, tc);
                    }
                    break;
                default:
                    break;
            }
            }
        //        Debug.Log($"ElementData:{e.id},{e.vBase},{e._source}");
    }

}