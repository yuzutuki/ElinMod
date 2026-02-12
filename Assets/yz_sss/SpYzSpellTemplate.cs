using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
	//クラス名を変えること！
    internal class SpYzSpellTemplate : Spell
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
			//効果音(発動時)の再生
			CC.PlaySound("identify", 1f, true);
			//この辺からメイン処理
			//ダメージ出力やエフェクトは適宜入れ替える
			//
			//ダメージ出力
			YzWrapper.AddDamage(this, CC, TC.Chara, pow, ele) ;
            //エフェクト再生
			YzUtil.PlayEffect("PwF", CC, TC);
			//
			YzWrapper.PlayVanillaEffect(ele, TC.pos,0f);
			//
			return true;
        }
    }
}