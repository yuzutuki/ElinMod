using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzHomingRock : Spell
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
			int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
			//効果音(発動時)の再生
			//CC.PlaySound("identify", 1f, true);
			//この辺からメイン処理
			List<Point> points = YzWrapper.GetCirclePoint(CC.pos, 6, false, false);
			List<Chara> tempTargets = YzUtil.AvoidFriendlyFire(CC, points);
			//
			if (tempTargets.Count <= 0)
			{
				CC.SayNothingHappans();
				CC.Say("yz_fail_HomingRock");
				return false;
			}
			//
			CC.PlaySound("teleport", 0.5f, false);
			CC.PlaySound("throw", 1f, true);
			//
			YzUtil.DistanceSort(CC.pos, tempTargets);
			Chara target = tempTargets[0];
			//ダメージ出力
			float fDist = Vector2.Distance(new Vector2(CC.pos.x, CC.pos.z), new Vector2(target.pos.x, target.pos.z));
			float powFix = 0.2f + (fDist * 0.7f);
			//Debug.Log($"dist:{CC.Dist(target)},fDist:{fDist},p:{pow},pf:{powFix},rp:{(int)(pow * powFix)},pfed:{powFixed}");
			int dly = (int)(5 * fDist);
			YzUtil.DelayAddDamage(dly, this, CC, target, pow, ele,damageMult:powFix);
			YzUtil.DelayVanilaEffect(dly, ele, target.pos);
			YzUtil.DelayPlaySound("hit_blunt", target,dly);
			YzUtil.DelayPlaySound("Material/rock_dead", target, dly+1,0.5f);
			//エフェクト再生
			YzUtil.PlayEffect("HomingRock", CC, target);
			//
			return true;
        }
    }
}