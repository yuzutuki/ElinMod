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
			if(TC == null || TC.Chara == null)
            {
				return false;
            }
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
			//
			if (TC.Chara.uid == CC.uid || !CC.HasCondition<ConYzSovereign>())
			{
                //自己対象の処理
                CC.PlaySound("build_area", 1f, true);
                //スタック付与
                int effCount = 1;
                ConYzSovereign con = CC.GetCondition<ConYzSovereign>() ?? (CC.AddCondition<ConYzSovereign>(100, true) as ConYzSovereign);
                if (con != null)
                {
                    //256:鍛冶
                    int smithLv = YzWrapper.GetLevel(CC, 256);
                    int stackPlus = smithLv +(mlv / 2)+20;
                    stackPlus = YzWrapper.Curve(stackPlus, 25, 5, 98);
                    stackPlus = YzWrapper.Curve(stackPlus, 100, 5, 95);
                    stackPlus = YzWrapper.Curve(stackPlus, 200, 5, 90);
                    con.BuffStack(stackPlus+5);
                    effCount = Mathf.Min(con.stack / 20, 100);
                    con.SetCastZone(EClass._zone.uid);
                }
                //
                for(int i = 0; i < effCount; i++)
                {
                    YzUtil.PlayEffect("Sovereign2", CC);
                }
            }
            else
			{
                //他者対象の処理
                CC.PlaySound("ab_shred", 0.8f, true);
                CC.PlaySound("ab_shred2", 0.4f, true);
                //ダメージ出力
                int mult = 1;
                if(CC.HasCondition<ConYzSovereign>())
                {
                    ConYzSovereign con = CC.GetCondition<ConYzSovereign>();
                    if (con != null)
                    {
                        mult += (con.stack/100);
                        CC.CureCondition<ConYzSovereign>();
                    }
                }
                YzWrapper.AddDamage(this, CC, TC.Chara, pow, ele,damageMult:mult);
                //エフェクト再生
                YzUtil.PlayEffect("Sovereign", CC, TC);
            }
			//
			//YzWrapper.PlayVanillaEffect(ele, TC.pos,0f);
			//
			return true;
        }
    }
}