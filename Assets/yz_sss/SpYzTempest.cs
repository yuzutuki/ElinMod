using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzTempest : Spell
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
			if (CC.mana.max == 0)
			{
				CC.SayNothingHappans();
				return false;
			}
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
			//効果音(発動時)の再生
			CC.PlaySound("identify", 0.5f, true);
            CC.PlaySound("atk_eleLightning", 1f, true);
            //
			int bonusCast = Mathf.Max(YzWrapper.Curve(CC.mana.value,100,100,50) /100, 0);
            float ratio = (CC.mana.max==0)?0f:(float)CC.mana.value/CC.mana.max;
			int castCount = Mathf.Max(Mathf.FloorToInt(ratio * 10f)+1,1) + bonusCast;
			ConYzLightning con = CC.GetCondition<ConYzLightning>() ?? (CC.AddCondition<ConYzLightning>(100, true) as ConYzLightning);
			con.AddStack(castCount);
			CC.mana.value = Mathf.Min(CC.mana.value, 0);
            //エフェクト再生
			int delay = 30/castCount;
            for (int i = 0; i < castCount; i++)
			{
				YzUtil.DelayPlayEffect(i*delay,"Tempest1",CC,CC,TP:CC.pos.Copy());
			}
			//
			//
			return true;
        }
    }
}