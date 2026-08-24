using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzAF : Spell
    {
        public override bool Perform()
        {
            //詠唱エフェクトとウェイト
            EClass.Wait(1.0f, CC);
            CastEffect.Call(CC);
            //対象不明なら終了
            if (TC == null || TC.Chara == null)
            {
                return false;
            }
            //パラメータ取得
            Point tp = TC.pos.Copy();
            Chara cc = CC.Chara;
            int mlv = YzWrapper.GetLevel(cc, this);
            int pow = YzWrapper.GetPower(cc, this);
            Element ele = YzWrapper.GetElement(this, pow);
            RapidData rd = new RapidData(cc);
            float mult = 100f / (100f + rd.max * 60f);
            //この辺からメイン処理
            //範囲とターゲット取得
            List<Point> points = YzWrapper.GetCirclePoint(tp, 2, false, false);
            List<Chara> targets = YzUtil.AvoidFriendlyFire(cc, points);
            //
            for (rd.current = 0; rd.current <= rd.max; rd.current++)
            {
                int delay = rd.RapidDelay();
                //バフ付与
                if (targets.Count > 0)
                {
                    cc.CureCondition<ConYzUB>();
                    ConYzAF con = cc.GetCondition<ConYzAF>() ?? (cc.AddCondition<ConYzAF>(100, false) as ConYzAF);
                    if (con != null)
                    {
                        con.BuffStack();
                    }
                }
                foreach (Chara c in targets)
                {
                    //ダメージ出力
                    YzWrapper.AddDamage(this, cc, c, pow, ele, damageMult: mult);
                }
                YzUtil.DelayAction(() =>
                {
                    //エフェクト再生
                    YzUtil.PlayEffect("AF", cc, TP: tp);
                    //効果音
                    cc.PlaySound("atk_eleFire", 1f, true);
                    //範囲表示
                    YzUtil.ShowEffectiveArea(points, false);
                }, delay);
                //
            }
            return true;
        }
    }
}