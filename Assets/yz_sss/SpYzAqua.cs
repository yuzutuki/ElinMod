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
            if (YzUtil.CheckFailSelf(this, CC, TC, TP))
            {
                return false;
            }
            //パラメータ取得
            Point tp = TP.Copy();
            Chara cc = CC.Chara;
            Chara tc = TC.Chara;
            int mlv = YzWrapper.GetLevel(cc, this);
            int pow = YzWrapper.GetPower(cc, this);
            Element ele = YzWrapper.GetElement(this, pow);
            //連射データ
            RapidData rd = new RapidData(cc);
            float mult = 100f / (100f + rd.max * 30f);
            for (rd.current = 0; rd.current <= rd.max; rd.current++)
            {
                int delay = rd.RapidDelay();
                //効果音(発動時)の再生
                cc.PlaySound("bubble", 1f, true);
                for (int i = 0; i < 6; i++)
                {
                    YzUtil.DelayAction(() =>
                    {
                        cc.PlaySound("bubble", 0.7f, true);
                    }, i * 7);
                }
                //この辺からメイン処理
                //ダメージ出力やエフェクトは適宜入れ替える
                List<Point> points = YzWrapper.GetArcPoint(cc.pos.Copy(), tp.Copy(), 7, 35);
                points = YzUtil.DistanceSort(cc.pos.Copy(), points);
                int count = 0;
                foreach (Point p in points)
                {
                    List<Chara> charas = p.ListCharas();
                    foreach (Chara c in charas)
                    {
                        c.AddCondition<ConWet>(100, false);
                        if (!YzUtil.AvoidFriendlyFire(cc, c))
                        {
                            YzWrapper.AddDamage(this, cc, c, pow, ele, damageMult: mult);
                        }
                    }
                    //YzWrapper.PlayVanillaEffect(ele, p,count*2);
                    YzUtil.DelayAction(() =>
                    {
                        YzUtil.ShowEffectiveArea(p.Copy());
                        YzUtil.PlayEffect("Aqua", cc, TP: p.Copy(), fromP: cc.pos.Copy());
                        //YzWrapper.AddDamage(p.Copy(), this, cc, pow, ele);
                    }, count / 2 + delay);
                    count++;

                }
            }
            //
            return true;
        }
    }
}