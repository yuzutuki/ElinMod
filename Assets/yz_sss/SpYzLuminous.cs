using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    //古いものなのでDelayActではなくTweenUtil.Delayを使用。整合性が取れなくなりそうだったら修正。
    internal class SpYzLuminous : Spell
    {
        public override bool Perform()
        {
            Chara cc = CC;
            Card tc = TC;
            //詠唱エフェクト
            EClass.Wait(0.5f, CC);
            CastEffect.Call(cc);
            //対象不明なら終了
            if (TP == null)
            {
                if (TC == null)
                    TP = TC.pos.Copy();
                else
                    return false;
            }
            if (YzUtil.CheckFailSelf(this, CC, TC, TP))
            {
                return false;
            }
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
            int pow = YzWrapper.GetPower(CC, this);
            Element ele = YzWrapper.GetElement(this, pow);
            RapidData rd = new RapidData(cc);
            //射程と回数の設定
            List<int> rapidBorder = new List<int> { 10, 20, 30, 60, 120, 240, 300 };
            int rapid = 1;
            foreach (int b in rapidBorder)
            {
                if (mlv >= b)
                {
                    rapid++;
                }
            }
            rapid += rd.max;
            List<int> rangeBorder = new List<int> { 5, 10, 20, 50, 100, 150, 200, 300 };
            int range = 1;
            foreach (int b in rangeBorder)
            {
                if (mlv >= b)
                {
                    range++;
                }
            }
            //
            for (int c = 0; c < rapid; c++)
            {
                List<Point> targetPoints = YzUtil.LinePoints(CC.pos, TP, range);
                if (targetPoints.Count == 0)
                {
                    cc.SayNothingHappans();
                    return false;
                }
                Point lastPoint = new Point();
                //ヒット処理
                foreach (Point p in targetPoints)
                {
                    lastPoint = p.Copy();
                    if (!p.IsValid)
                        continue;
                    //掘削
                    if (p.HasBlock)
                    {
                        EClass._map.MineBlock(p);
                        //ヒットスパーク
                        TweenUtil.Delay((float)c * 0.05f, delegate
                        {
                            YzUtil.PlayEffect("LuminousHS", cc, cc, TP: p);
                        });
                        break;
                    }
                    //移動不可オブジェだけ掘削
                    if (p.HasObj)
                    {
                        if (p.IsBlocked)
                        {
                            EClass._map.MineObj(p);
                            TweenUtil.Delay((float)c * 0.05f, delegate
                            {
                                YzUtil.PlayEffect("LuminousHS", cc, cc, TP: p);
                            });
                            //breakはしない。オブジェクト貫通。
                        }
                    }
                    //ダメージ
                    var hitCharas = p.ListCharas()?.ToArray();
                    if(hitCharas.Length > 0) {
                        foreach (Chara hitChara in hitCharas)
                        {
                            if (hitChara != null && !hitChara.isDestroyed)
                            {
                                if (!YzUtil.AvoidFriendlyFire(cc, hitChara))
                                {
                                    YzWrapper.DoHostileAction(cc, hitChara, true);
                                    YzWrapper.AddDamage(this, cc, hitChara, pow, ele);
                                    //ヒットスパーク
                                    TweenUtil.Delay((float)c * 0.05f, delegate
                                    {
                                        YzUtil.PlayEffect("LuminousHS", cc, hitChara);
                                    });
                                }
                            }
                        }
                    }
                }
                //エフェクト再生
                TweenUtil.Delay((float)c * 0.05f, delegate
                {
                    CC.PlaySound("spell_arrow", 0.6f, true);
                    YzUtil.PlayEffect("Luminous", cc,TP: lastPoint);
                });
                //
            }
            //
            return true;
        }
    }
}