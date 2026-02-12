using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
	//クラス名を変えること！
    internal class SpYzSummonUne : Spell
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
			//Element ele = YzWrapper.GetElement(this, pow);
			//効果音(発動時)の再生
			CC.PlaySound("identify", 1f, true);
			//この辺からメイン処理
			int count = Mathf.RoundToInt(mlv/10) +2;
			for (int i = 0; i < count; i++)
			{
				int radius = Mathf.CeilToInt(i / 4);
				if (!YzUtil.SummonMinion("yz_une", CC, TP.Copy(), radius, YzUtil.LevelAdjust.Power, pow, false,20))
				{
					return false;
				}
			}
			//
			return true;
        }
    }
}