using BJ;
using Cwl.Helper.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzCoreMelt : Spell
    {
        public override bool Perform()
        {
            //詠唱エフェクトとウェイト
            CastEffect.Call(CC);
            //対象不明なら終了
            if (TC == null || TC.Chara == null)
            {
                return false;
            }
            if (EClass._zone.IsPCFactionOrTent || EClass._zone.IsUserZone)
            {
                CC.SayNothingHappans();
                CC.Say("yz_fail_CoreMelt");
                return false;
            }
            Game.Wait(5.0f);
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
            int pow = YzWrapper.GetPower(CC, this);
            Element ele = YzWrapper.GetElement(this, pow);
            //効果音(発動時)の再生
            CC.PlaySound("missile", 0.6f, true);
            CC.PlaySound("spell_earthquake", 1f, true);
            CC.PlaySound("spell_ball", 0.7f, true);
            CC.PlaySound("explode", 1.0f, true);
            //エフェクト再生
            YzUtil.PlayEffect("CoreMelt", CC, TC);
            //技名表示
            if (CC.IsPC && ModConfig.SpPop.Value)
            {
                string name = this.Name;
                CC.renderer.Say("| " + name + " ", default(Color), 0f);
            }
            //この辺からメイン処理
            List<Point> targetPoints = new List<Point>();
            targetPoints = YzWrapper.GetCirclePoint(CC.pos.Copy(), 18f, false, false);
            targetPoints = YzUtil.DistanceSort(CC.pos, targetPoints);
            int count = 0;
            foreach (Point tp in targetPoints)
            {
                int dir = tp.Distance(CC.pos);
                count++;
                YzUtil.DelayAction(() =>
                {
                    YzUtil.ShowEffectiveArea(tp);
                    //雪解け
                    if (tp.cell.IsSnowTile)
                    {
                        tp.cell.isClearSnow = true;
                    }
                    if (EClass.rnd(20) != 0)
                    {
                        EClass._map.MineBlock(tp);
                        EClass._map.MineFloor(tp);
                        EClass._map.MineObj(tp);
                    }
                    if (EClass.rnd(10) < 8)
                    {
                        tp.ListCards().ForEach(c =>
                        {
                            if (c.trait.CanBeDestroyed && c.rarity < Rarity.Legendary && c.isThing)
                            {
                                c.Destroy();
                            }
                        });
                    }
                    //キャラ取得してダメージ処理
                    var charas = tp.ListCharas().ToArray();
                    foreach(Chara c in charas) 
                    {
                        YzWrapper.DoHostileAction(CC, c, true);
                        if (!YzUtil.AvoidFriendlyFire(CC, c))
                        {
                            float mult = 1f;
                            //爬虫人類絶滅処理
                            if (c.race.id == "dinosaur" || c.race.id == "lizardman")
                            {
                                mult = 2f;
                            }
                            YzWrapper.AddDamage(this, CC, c, pow, ele, damageMult: mult);
                            YzWrapper.PlayVanillaEffect(ele, c.pos, 0f);
                        }
                    }
                    //
                }, dir * 2 + count / 6);
            }
            //CC.DamageHP(CC.hp-1);
            CC.hp = 1;
            CC.DamageHP(CC.MaxHP * 100);
            //
            return true;
        }
    }
}