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
			int aPen = 5;
			aPen += EClass.curve(mlv, 25, 5, 90);
			aPen = Mathf.Min(aPen,100);
			//ダメージ出力
			YzWrapper.AddDamage(this, CC, TC.Chara, pow, Element.Void,AS:AttackSource.Melee,penetration:aPen) ;
            //エフェクト再生
			YzUtil.PlayEffect("Abyssal", TC, TC);
			//
			//YzWrapper.PlayVanillaEffect(ele, TC.pos,0f);
			//
			return true;
        }
    }
}