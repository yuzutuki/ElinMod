using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzPwF : Spell
    {
        public override bool Perform()
        {
			bool enhance = false;
			//詠唱エフェクト
			EClass.Wait(0.5f, CC);
			CastEffect.Call(CC);
			//対象不明なら終了
			if(TC == null || TC.Chara == null)
            {
				return true;
			}
			//パワー取得
			int pow = YzWrapper.GetPower(CC, this);
			//エレメント取得
			Element ele = YzWrapper.GetElement(this, pow);
			//バフ管理
			ConYzPwF conYzPwF = (EClass.pc.AddCondition<ConYzPwF>(100, false) as ConYzPwF) ?? EClass.pc.GetCondition<ConYzPwF>();
            if (conYzPwF != null)
            {
                if (conYzPwF.BuffStack())
                {
					enhance = true;
					//CC.Say("燃えちゃえー");
					CC.PlaySound("spell_breathe", 1f, true);
					YzUtil.AddDebuff("ConFaint", 20 + pow / 200, CC, TC.Chara, true);
                }
            }
			//Debug.Log($"pow:{pow}");
			//ダメージ出力
			YzWrapper.AddDamage(this, CC, TC.Chara, pow, ele, (enhance ? 100:0)) ;
            //エフェクト再生
            if (enhance)
				YzUtil.PlayEffect("PwF", CC, TC,replaceAffterEffect:"PwFhit2");
			else
				YzUtil.PlayEffect("PwF", CC, TC);
			YzWrapper.PlayVanillaEffect(ele, TC.pos,0.4f);
			CC.PlaySound("spell_arrow", 1f, true);
			CC.PlaySound("fire_step", 1f, true);
			//CC.PlaySound("identify", 1f, true);
			//
			return true;
        }
    }
}