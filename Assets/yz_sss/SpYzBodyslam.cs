using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzBodyslam : Spell
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

            if (TC == CC && !TP.Equals(CC.pos))
            {
                return false;
            }
                //パラメータ取得
                int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
            //効果音(発動時)の再生

            //hit_blunt Play:kickthrow
            CC.PlaySound("throw", 1f, true);
            YzUtil.DelayPlaySound("kick", CC, 10);
            YzUtil.DelayPlaySound("hit_blunt", CC, 12);
            //この辺からメイン処理
            //ダメージ出力やエフェクトは適宜入れ替える
            //
            //ダメージ出力
            YzWrapper.AddDamage(this, CC, TC.Chara, pow, ele,AS: AttackSource.Melee) ;
            //エフェクト再生
			YzUtil.PlayEffect("Bodyslam", CC, TC);
			//
			return true;
        }
    }
}