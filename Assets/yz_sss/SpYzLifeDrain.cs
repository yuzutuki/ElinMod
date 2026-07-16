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
            int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
			//効果音(発動時)の再生
			CC.PlaySound("attack_cane", 0f, true);
			CC.PlaySound("atk_eleNether", 1f, true);
			//この辺からメイン処理
			//ダメージ出力やエフェクトは適宜入れ替える
			//吸血鬼チェック
			float dmgFix = 1;
            if (YzUtil.CheckVamp(CC))
            {
				dmgFix = 1.2f;
            }
			//ダメージ出力
			int tempDmg;
			tempDmg = TC.hp;
			YzWrapper.AddDamage(this, CC, TC.Chara, pow, ele,damageMult:dmgFix);
			tempDmg = tempDmg - TC.hp;
			if (tempDmg > 0)
			{
				int heal = Mathf.Max(1, Mathf.Min(tempDmg, TC.MaxHP) / 8);
				CC.HealHP(heal, HealSource.Magic);
			}
			//エフェクト再生
			YzUtil.PlayEffect("LifeDrain", CC, TC,TP: TC.pos);
			//
			YzWrapper.PlayVanillaEffect(ele, TC.pos,0.2f);
			//
			return true;
        }
    }
}