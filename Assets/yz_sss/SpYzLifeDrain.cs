using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzLifeDrain : Spell
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
            Chara cc = CC.Chara;
            Chara tc = TC.Chara;
            int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
            RapidData rd = new RapidData(cc);
            float mult = 100f / (100f + rd.max * 20f);
			//この辺からメイン処理
			//ダメージ出力やエフェクトは適宜入れ替える
			//吸血鬼チェック
			float dmgFix = 1;
            if (YzUtil.CheckVamp(CC))
            {
				dmgFix = 1.2f;
            }
            //ダメージ出力
            //
            for (rd.current = 0; rd.current <= rd.max; rd.current++)
            {
                int delay = rd.RapidDelay();
                if (rd.current > 0 && (tc == null || tc.isDead))
                {
                    break;
                }
                int tempDmg = 0;
                tempDmg = tc?.hp ?? 0;
                YzWrapper.AddDamage(this, cc, tc, pow, ele, damageMult: dmgFix * mult);
                tempDmg = tempDmg - (tc?.hp ?? 0);
                Debug.Log("Life Drain:tempDmg: " + tempDmg);
                if (tempDmg > 0)
                {
                    int heal = Mathf.Max(1, Mathf.Min(tempDmg, tc.MaxHP) / 4);
                    Debug.Log("Life Drain:tempDmg: " + tempDmg + ", Heal: " + heal);
                    cc.HealHP(heal, HealSource.Magic);
                }
                YzUtil.DelayAction(() =>
                {
                    //エフェクト再生
                    YzUtil.PlayEffect("LifeDrain", cc, tc, TP: tc.pos.Copy());
                    //効果音
                    CC.PlaySound("attack_cane", 0f, true);
                    CC.PlaySound("atk_eleNether", 1f, true);
                    //
                    YzWrapper.PlayVanillaEffect(ele, tc.pos.Copy(), 0.2f);
                }, delay);
                //
            }
			return true;
        }
    }
}