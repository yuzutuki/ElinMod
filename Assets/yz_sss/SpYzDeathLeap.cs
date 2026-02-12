using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzDeathLeap : Spell
    {
        public override bool Perform()
        {
			//詠唱エフェクト
			EClass.Wait(0.8f, CC);
			CastEffect.Call(CC);
			//対象不明なら終了
			if(TC == null || TC.Chara == null)
            {
				return false;
			}
			//パラメータ取得
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
			//エフェクト再生
			YzUtil.PlayEffect("Hook", CC, TC);
			YzWrapper.PlayVanillaEffect(ele, TC.pos, 0.1f);
			CC.PlaySound("rush", 1f, true);
			CC.PlaySound("elevator", 1f, true);
			//移動処理
			if (CC.Dist(TC) > 1)
			{
				Point point = YzUtil.GetStepTowards(TC.pos, CC.pos);
				point = point.GetNearestPoint(false, true, true, false, 0);
				CC.Say("abDraw", CC, TC, null, null);
				YzWrapper.MoveByForce(CC.Chara, point);
				//押しのけ
				YzUtil.PushChara(CC.Chara, point);
			}
			//ダメージ出力
			YzUtil.AddDebuff("ConFear", 40+pow/100, CC, TC.Chara, false);
			YzWrapper.DoHostileAction(CC, TC, true);
			YzWrapper.AddDamage(this, CC, TC.Chara, pow, ele);
			//
			return true;
        }
    }
}