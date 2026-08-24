using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzAbyssal : Spell
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
            Point tp = TC.pos.Copy();
            Chara cc = CC.Chara;
            Chara tc = TC.Chara;
            int mlv = YzWrapper.GetLevel(cc, this);
            int pow = YzWrapper.GetPower(cc, this);
            Element ele = YzWrapper.GetElement(this, pow);
            //連射データ
            RapidData rd = new RapidData(cc);
            float mult = 100f / (100f + rd.max * 25f);
            for (rd.current = 0; rd.current <= rd.max; rd.current++)
            {
                int delay = rd.RapidDelay();
                if (rd.current > 0 && (tc == null || tc.isDead))
                {
                    break;
                }
                //この辺からメイン処理
                //貫通率
                int aPen = 5;
                //			aPen += EClass.curve(mlv, 10, 5, 75);
                aPen += YzWrapper.Curve(mlv, 10, 5, 75);
                aPen = Mathf.Min(aPen, 100);
                //ダメージ出力
                YzWrapper.AddDamage(this, cc, tc, pow,ele, AS: AttackSource.Melee, penetration: aPen, damageMult: mult);
                YzUtil.AddDebuff("ConParalyze", 100 + pow / 10, cc, tc, false);
                YzUtil.DelayAction(() =>
                {
                    //効果音(発動時)の再生
                    for (int i = 0; i < 4; i++)
                    {
                        YzUtil.DelayPlaySound("ab_magicsword", cc, 8 + (i * 3), 0.8f - i, false);
                    }
                    //エフェクト再生
                    YzUtil.PlayEffect("Abyssal", tc, tc);
                },delay);
                //

            }
            return true;
        }
    }
}