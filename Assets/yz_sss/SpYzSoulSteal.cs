using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzSoulSteal : Spell
    {
        public override bool Perform()
        {
			EClass.Wait(1f, CC);
			//詠唱エフェクト
			CastEffect.Call(CC);
			//対象不明なら終了
			if(TC == null || TC.Chara == null)
            {
				return false;
			}
			//パラメータ取得
			int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
            //技名表示
            if (CC.IsPC && ModConfig.SpPop.Value)
            {
                string name = this.Name;
                CC.renderer.Say("| " + name + " ", default(Color), 0f);
            }
            //効果音(発動時)の再生
            CC.PlaySound("ab_swarm", 0f, true);
			//この辺からメイン処理
			//吸血鬼チェック
			float dmgFix = 1;
			if (YzUtil.CheckVamp(CC))
			{
				dmgFix = 1.2f;
			}
			
			List<Point> points = YzWrapper.GetCirclePoint(CC.pos,10,false,false);
			List<Chara> targets = YzUtil.AvoidFriendlyFire(CC, points);
            //
            if (targets.Count <= 0)
            {
				CC.SayNothingHappans();
				return false;
            }
			int count = 0;
			int tempDmg;
			foreach(Chara c in targets) {
				tempDmg = c.hp;
			//ダメージ出力
			YzWrapper.AddDamage(this, CC,c, pow, ele,damageMult:dmgFix) ;
				tempDmg = tempDmg - c.hp;
				//Debug.Log($"damage:{tempDmg}");
                if (tempDmg > 0)
                {
					int heal = Mathf.Max(1, Mathf.Min(tempDmg,c.MaxHP) / 10);
					//Debug.Log($"heal:{heal}");
					CC.HealHP(heal, HealSource.Magic);
                }
				//エフェクト再生
				YzUtil.DelayPlaySound("attack_cane", CC, (count * 3) + 2, 0.5f, true);
				YzUtil.DelayPlayEffect(count*3, "SoulSteal", CC, c, c.pos);
				count++;
			}
			//
			return true;
        }
    }
}