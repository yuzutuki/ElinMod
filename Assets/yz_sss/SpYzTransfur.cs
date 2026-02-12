using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace yz_sss
{
    internal class SpYzTransfur : Spell
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
			//int mlv = YzWrapper.GetLevel(CC, this);
			//int pow = YzWrapper.GetPower(CC, this);
			//Element ele = YzWrapper.GetElement(this, pow);
			//効果音(発動時)の再生
			CC.PlaySound("identify", 1f, true);
            //この辺からメイン処理
            if (!TC.isChara)
            {
				CC.SayNothingHappans();
				return (false);
            }
			//
			//もともとのしっぽチェック
			bool isTail = false;
			if (EClass.sources.charas.GetRow(TC.id).elements.Contains(1209) ||
				EClass.sources.races.GetRow(TC.Chara.race.id).elements.Contains(1209))
			{
				//EClass.pc.Say("しっぽあるよ！");
				isTail = true;
			}
			//変化後かどうかをフィートの有無でチェック
			bool checkVal = TC.HasElement(721001);
			//	Debug.Log($"checkFeet:{checkVal}");
			if (!checkVal)
			{
				TC.Chara.elements.SetBase(721001, 1, TC.LV);
				if (!isTail)
				{
					TC.Chara.elements.SetBase(1209, 1, TC.LV);
				}
				EClass.pc.Say("yz_TransfurOn",TC);
			}
			else
			{
				TC.Chara.elements.SetBase(721001, 0, 0);
				if (!isTail)
				{
					TC.Chara.elements.SetBase(1209, 0, 0);
				}
				EClass.pc.Say("yz_TransfurOff",TC);
			}
			
			TC.Chara.Refresh();
			//
			//CC.PlaySound("identify", 1f, true);
			YzUtil.PlayEffect("Polymorph", CC, TC);
			YzWrapper.PlayVanillaEffect("mutation", TC.pos);
			//
			return true;
        }
    }
}