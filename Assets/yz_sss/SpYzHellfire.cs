using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace yz_sss
{
    internal class SpYzHellfire : Spell
    {
        public override bool Perform()
        {
			//詠唱エフェクト
			//
			EClass.Wait(0.8f, CC);
			CastEffect.Call(CC);
			//対象不明なら終了
			if(TC == null || TC.Chara == null)
			{
				CC.SayNothingHappans();
				return false;
            }
            if (YzUtil.CheckFailSelf(this, CC, TC, TP))
            {
                return false;
            }
            //Debug.Log($"CCpos:{CC.pos},TP:{TP},dist:{TP.Distance(CC.pos)}");
            if (TP.Distance(CC.pos)==0)
			{
				CC.SayNothingHappans();
				return false;
			}
			//パラメータ取得
			Chara cc = CC.Chara;
			Point tp = TP.Copy();
            int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
			Element eleSub = YzWrapper.GetElement("eleNether",pow);
            RapidData rd = new RapidData(cc);
            float mult = 100f / (100f + rd.max * 25f);
            //この辺からメイン処理

            //吸血鬼チェック
            float dmgFix = 1;
			if (YzUtil.CheckVamp(CC))
			{
				dmgFix = 1.2f;
			}
			//
			Vector2 dir = (new Vector2(TP.x, TP.z) - new Vector2(CC.pos.x, CC.pos.z));
			dir.Normalize();
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			if (angle < 0) angle += 360f;
			//丸め
			float snap = Mathf.Round(angle / 45f) * 45f;
			List<Point> fixPoint = new List<Point>();
			fixPoint.Add(new Point(0, 0));
            switch ((int)snap % 360)
            {
				case 0://右
					fixPoint.Add(new Point(0, 1));
					fixPoint.Add(new Point(0, -1));
					break;
				case 45:
					fixPoint.Add(new Point(0, 1));
					fixPoint.Add(new Point(1, 0));
					break;
				case 90://上
					fixPoint.Add(new Point(1, 0));
					fixPoint.Add(new Point(-1, 0));
					break;
				case 135:
					fixPoint.Add(new Point(0, 1));
					fixPoint.Add(new Point(-1, 0));
					break;
				case 180://左
					fixPoint.Add(new Point(0, 1));
					fixPoint.Add(new Point(0, -1));
					break;
				case 225:
					fixPoint.Add(new Point(0, -1));
					fixPoint.Add(new Point(-1, 0));
					break;
				case 270://下
					fixPoint.Add(new Point(1, 0));
					fixPoint.Add(new Point(-1, 0));
					break;
				case 315:
					fixPoint.Add(new Point(0, -1));
					fixPoint.Add(new Point(1, 0));
					break;
				default:
					fixPoint.Add(new Point(0, 1));
					fixPoint.Add(new Point(0, -1));
					break;
			}
			//直線取得
			List<Point> targetPoints = new List<Point>();
			foreach(var fix in fixPoint)
            {
				Point t = new Point(TP.x + fix.x, TP.z + fix.z);
				Point o = new Point(CC.pos.x + fix.x, CC.pos.z + fix.z);
				var line = YzWrapper.GetLinePoint(o, t, 8);
				targetPoints = targetPoints.Union(line).ToList();
			}
			targetPoints = YzUtil.DistanceSort(CC.pos, targetPoints);
			//ターゲット取得
			List<Chara> tcList = YzUtil.AvoidFriendlyFire(cc, targetPoints);
			//
			for (rd.current = 0; rd.current <= rd.max; rd.current++)
			{
				int delay = rd.RapidDelay();

                YzUtil.DelayAction(() =>
				{
					//効果音
					YzUtil.DelayPlaySound("fire_step", cc, 0);
					YzUtil.DelayPlaySound("fire_step", cc, 10);
					YzUtil.DelayPlaySound("fire_step", cc, 20);
					//エフェクト再生
					YzUtil.PlayEffect("Hellfire", cc, TP: tp.Copy(), fromP: cc.pos.Copy());
					YzUtil.ShowEffectiveArea(targetPoints, true);
				}, delay);
            
            //
            //ダメージ出力
            foreach (Chara c in tcList)
				{
					YzWrapper.AddDamage(this, CC, c, pow, ele, damageMult: dmgFix * mult);
					YzWrapper.AddDamage(this, CC, c, pow, eleSub, damageMult: dmgFix * mult);
					YzWrapper.PlayVanillaEffect(ele, c.pos, 0.2f);
				}
			}
			//
			return true;
        }
    }
}