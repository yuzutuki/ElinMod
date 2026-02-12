using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzSummonSword : Spell
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
			// ele = YzWrapper.GetElement(this, pow);
			//効果音(発動時)の再生
			CC.PlaySound("identify", 1f, true);
            //この辺からメイン処理
            if (!YzUtil.SummonMinion("yz_SwordFamiliar", CC, TP.Copy(), 1, YzUtil.LevelAdjust.Average, pow,true))
            {
				return false;
            }
			//
			//YzWrapper.PlayVanillaEffect(ele, TC.pos,1f);
			//
			return true;
        }
    }
}