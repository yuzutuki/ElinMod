using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzTetraSpirit : Spell
    {
        public override bool Perform()
        {
			//変数定義はこの辺
			//
			//詠唱エフェクトとウェイト
			EClass.Wait(0.8f, CC);
			CastEffect.Call(CC);
			//対象不明なら終了
			if(TC == null || TC.Chara == null)
            {
				return false;
			}
			//パラメータ取得
			Chara cc = CC.Chara;
			int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
            RapidData rd = new RapidData(cc);
            float mult = 100f / (100f + rd.max * 5f);
            //効果音(発動時)の再生
            CC.PlaySound("atk_eleNether", 0f, true);
			//この辺からメイン処理
			//吸血鬼チェック
			float dmgFix = 1;
			if (YzUtil.CheckVamp(CC))
			{
				dmgFix = 1.2f;
			}
			List<Point> points = YzWrapper.GetCirclePoint(CC.pos, 6, true, true);
			List<Chara> tempTargets = YzUtil.AvoidFriendlyFire(CC, points);
			//
			if (tempTargets.Count <= 0)
			{
				CC.SayNothingHappans();
				return false;
			}
			//
			List<Chara> targets = new List<Chara>();
			for(int i = 0; i < 4+rd.max; i++)
            {
				int index = Random.Range(0, tempTargets.Count);
				targets.Add(tempTargets[index]);
            }
			int count = 0;
			List<Point> targetPoints = new List<Point>();
			//
			foreach (Chara c in targets)
			{
				//ダメージ出力
				YzWrapper.AddDamage(this, CC, c, pow, ele,damageMult:dmgFix * mult);
				//エフェクト再生
				YzUtil.DelayPlaySound("attack_cane", CC, (count * 6), 1f, true);
				YzUtil.DelayPlayEffect(count * 6, "TetraSpirit", CC, c, TP:c.pos);
				YzWrapper.PlayVanillaEffect(ele, c.pos,0.2f);
				//
				if(!targetPoints.Contains(c.pos))
					targetPoints.Add(c.pos);
				count++;
			}
			//効果範囲表示処理
			YzUtil.ShowEffectiveArea(targetPoints);
			return true;
        }
    }
}