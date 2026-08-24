using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzPwF : Spell
    {
        /*
        public override string GetDetail()
        {
                string detail = this.source.GetDetail();
            detail += "\n\n" + "連射魔法対応";
            return detail;
        }
        */
        public override bool Perform()
        {
			bool enhance = false;
			//詠唱エフェクト
			EClass.Wait(0.5f, CC);
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
            Chara cc = CC.Chara;
            Chara tc = TC.Chara;
            //パワー取得
            int pow = YzWrapper.GetPower(cc, this);
			//エレメント取得
			Element ele = YzWrapper.GetElement(this, pow);
            //連射データ
            RapidData rd = new RapidData(cc);
            float mult = 100f / (100f + rd.max * 20f);
            //
            for (rd.current = 0; rd.current <= rd.max; rd.current++)
            {
                int delay = rd.RapidDelay();
                if(rd.current > 0 && (tc == null || tc.isDead))
                {
                    break ;
                }
                //Debug.Log($"delay:{delay}, current:{rd.current}, max:{rd.max}, mult:{mult}");
                //
                //バフ管理
                ConYzPwF conYzPwF = (cc.AddCondition<ConYzPwF>(100, false) as ConYzPwF) ?? cc.GetCondition<ConYzPwF>();
                if (conYzPwF != null)
                {
                    if (conYzPwF.BuffStack())
                    {
                        enhance = true;
                        //cc.Say("燃えちゃえー");
                        cc.PlaySound("spell_breathe", 1f, true);
                        YzUtil.AddDebuff("ConFaint", 20 + pow / 200, cc, tc, true);
                    }
                }
                //Debug.Log($"pow:{pow}");
                //ダメージ出力
                YzWrapper.AddDamage(this, cc, tc, pow, ele, (enhance ? 100 : 0),damageMult: mult);
                YzWrapper.PlayVanillaEffect(ele, tc.pos, 0.4f);
                //エフェクト再生
                YzUtil.DelayAction(() =>
                {
                    if (enhance)
                        YzUtil.PlayEffect("PwF", cc, tc, replaceAffterEffect: "PwFhit2");
                    else
                        YzUtil.PlayEffect("PwF", cc, tc);
                    cc.PlaySound("spell_arrow", 1f, true);
                    cc.PlaySound("fire_step", 1f, true);
                    Debug.Log($"cc: {cc?.Name}, tc: {tc?.Name},CC: {CC?.Name},TC: {TC?.Name}");
                },delay );
                //
            }
			//
			return true;
        }
    }
}