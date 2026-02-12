using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzAnalyze : Spell
    {
        public override bool Perform()
        {
			//詠唱エフェクト
			EClass.Wait(1f, CC);
			CastEffect.Call(CC);
			//対象不明なら終了
			if(TC == null || TC.Chara == null)
            {
				return true;
            }
			Chara tcc = TC.Chara;
			EClass.pc.Say("yz_UseAnalyze", CC, TC, null, null);
			YzUtil.PlayEffect("Analyze", CC, TC);
			CC.PlaySound("identify", 1f, true);
			//EClass.Wait(1f, CC);
			TweenUtil.Delay(0.5f, delegate
			{
				EClass.ui.AddLayer<LayerChara>().SetChara(tcc);
			});
			//
			return true;
        }
    }
}