using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
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
            if (YzUtil.CheckFailSelf(this, CC, TC, TP))
            {
                return false;
            }
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
			//この辺からメイン処理
			//ダメージ出力やエフェクトは適宜入れ替える
			//
			//ダメージ出力
			YzWrapper.AddDamage(this, CC, TC.Chara, pow, ele);
            //効果音
            CC.PlaySound("identify", 1f, true);
            //エフェクト再生
            YzUtil.PlayEffect("PwF", CC, TC);
			//
			YzWrapper.PlayVanillaEffect(ele, TC.pos,0f);
			//
			return true;
        }
    }
}