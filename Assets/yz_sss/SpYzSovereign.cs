using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzSovereign : Spell
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
            //パラメータ取得
            //パラメータ取得
            Chara cc = CC.Chara;
            Chara tc = TC.Chara;
            int mlv = YzWrapper.GetLevel(CC, this);
            int pow = YzWrapper.GetPower(CC, this);
            Element ele = YzWrapper.GetElement(this, pow);
            RapidData rd = new RapidData(cc);
            float rpd_mult = 100f / (100f + rd.max * 30f);
            //
            ConYzSovereign con = cc.GetCondition<ConYzSovereign>();
            if (con == null)
            {
                con = cc.AddCondition<ConYzSovereign>(100, true) as ConYzSovereign;
                con.castTrun = true;
            }
            else
            {
                Debug.Log("standing by!");
            }

            //
            if (tc.uid == cc.uid || con.castTrun)
            {
                int effCount = 1;
                for (rd.current = 0; rd.current <= rd.max; rd.current++)
                {
                    int delay = rd.RapidDelay();
                    //自己対象の処理
                    //スタック付与
                    if (con != null)
                    {
                        //256:鍛冶
                        int smithLv = YzWrapper.GetLevel(CC, 256);
                        int stackPlus = smithLv + (mlv / 2) + 20;
                        stackPlus = YzWrapper.Curve(stackPlus, 25, 5, 98);
                        stackPlus = YzWrapper.Curve(stackPlus, 100, 5, 95);
                        stackPlus = YzWrapper.Curve(stackPlus, 200, 5, 90);
                        con.BuffStack(stackPlus + 5);
                        effCount = Mathf.Min(con.stack / 20, 100);
                        con.SetCastZone(EClass._zone.uid);
                    }
                    else
                    {
                        EClass.pc.SayNothingHappans();
                    }
                    YzUtil.DelayAction(() =>
                    {
                        cc.PlaySound("build_area", 1f, true);
                    }, (int)(delay*1.5f));
                }
                //
                for (int i = 0; i < effCount; i++)
                {
                    YzUtil.PlayEffect("Sovereign2", CC);
                }

            }
            else
            {
                //他者対象の処理
                cc.PlaySound("ab_shred", 0.6f, true);
                cc.PlaySound("ab_shred2", 0.3f, true);
                //ダメージ出力
                int mult = 1;
                if (cc.HasCondition<ConYzSovereign>())
                {
                    if (con != null)
                    {
                        mult += (con.stack / 100);
                        con.Kill(true);
                    }
                    else
                    {
                        EClass.pc.SayNothingHappans();
                    }
                }
                YzWrapper.AddDamage(this, cc, tc, pow, ele, damageMult: mult);
                //エフェクト再生
                YzUtil.PlayEffect("Sovereign", cc, tc);
            }
            //
            //YzWrapper.PlayVanillaEffect(ele, TC.pos,0f);
            //
            return true;
        }
    }
}