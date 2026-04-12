using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzStaticGrab : Spell
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
            if (YzUtil.CheckFailSelf(this, CC, TC, TP))
            {
                return false;
            }
            //パラメータ取得
            int pow = YzWrapper.GetPower(CC, this);
			Element ele = Element.Create(base.source.aliasRef, pow / 10);
            //エフェクト再生
			YzUtil.PlayEffect("Grab", CC, TC);
			YzUtil.ShowEffectiveArea(TC.pos);
            CC.PlaySound("throw", 1f, true);
            CC.PlaySound("kick", 1f, true);
			CC.PlaySound("elevator", 0.3f, true);
			//移動処理
			if (CC.Dist(TC) > 1)
			{
				Point point = YzUtil.GetStepTowards(CC.pos, TC.pos);
				point = point.GetNearestPoint(false, true, true, false, 0);
				CC.Say("abDraw", CC, TC, null, null);
				YzWrapper.MoveByForce(TC.Chara, point);
				//押しのけ
				YzUtil.PushChara(TC.Chara, point);
			}
			//ダメージ出力
			if (!TC.IsPCFactionOrMinion)
            {
                YzWrapper.PlayVanillaEffect(ele, TC.pos, 0.1f);
                YzUtil.AddDebuff("ConParalyze", 20 + pow / 150, CC, TC.Chara, false);
				YzWrapper.DoHostileAction(CC, TC, true);
				YzWrapper.AddDamage(this, CC, TC.Chara, pow, ele);
			}
			//
			return true;
        }
    }
}