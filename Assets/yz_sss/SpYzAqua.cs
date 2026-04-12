using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzAqua : Spell
    {
        public override bool Perform()
        {
            //詠唱エフェクトとウェイト
            EClass.Wait(0.8f, CC);
            CastEffect.Call(CC);
            //対象不明なら終了
            if (TC == null || TC.Chara == null)
            {
                return false;
            }
            if (YzUtil.CheckFailSelf(this,CC,TC,TP))
            {
                return false;
            }
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
            int pow = YzWrapper.GetPower(CC, this);
            Element ele = YzWrapper.GetElement(this, pow);
            //効果音(発動時)の再生
            //CC.PlaySound("identify", 1f, true);
            CC.PlaySound("bubble", 1f, true);
            for(int i=0; i < 6; i++)
            {
                YzUtil.DelayAction(() => {
                    CC.PlaySound("bubble", 0.7f, true);
                }, i*7);
            }
            //この辺からメイン処理
            //ダメージ出力やエフェクトは適宜入れ替える
            List<Point> points = YzWrapper.GetArcPoint(CC.pos, TP, 7, 35);
            points = YzUtil.DistanceSort(CC.pos, points);
            int count = 0;
            foreach (Point p in points)
            {
                count++;
                //YzWrapper.PlayVanillaEffect(ele, p,count*2);
                YzUtil.DelayAction(() => {
                    YzUtil.ShowEffectiveArea(p.Copy());
                    YzUtil.PlayEffect("Aqua", CC, TP: p.Copy());
                    //YzWrapper.AddDamage(p.Copy(), this, CC, pow, ele);
                    List<Chara> charas = p.ListCharas();
                    foreach (Chara c in charas)
                    {
                        c.AddCondition<ConWet>(100, false);
                        if (!YzUtil.AvoidFriendlyFire(CC, c))
                        {
                            YzWrapper.AddDamage(this, CC, c, pow, ele);
                        }
                    }
                }, count);
            }
            //
            return true;
        }
    }
}