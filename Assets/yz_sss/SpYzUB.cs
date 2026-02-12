using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzUB : Spell
    {
        public override bool Perform()
        {
			//詠唱エフェクトとウェイト
			EClass.Wait(1.0f, CC);
			CastEffect.Call(CC);
			//対象不明なら終了
			if(TC == null || TC.Chara == null)
            {
				return false;
			}
			//パラメータ取得
			TP = TC.pos.Copy();
			int mlv = YzWrapper.GetLevel(CC, this);
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
			//効果音(発動時)の再生
			CC.PlaySound("atk_eleCold", 1f, true);
			//この辺からメイン処理
			//エフェクト再生
			YzUtil.PlayEffect("UB", CC, TC, TP);
			//範囲とターゲット取得
			List<Point> points = YzWrapper.GetCirclePoint(TP,2,false,false);
			List<Chara> targets = YzUtil.AvoidFriendlyFire(CC, points);
			//バフ付与
			if (targets.Count > 0)
			{
				CC.CureCondition<ConYzAF>();
				ConYzUB con = CC.GetCondition<ConYzUB>() ?? (CC.AddCondition<ConYzUB>(100, false) as ConYzUB);
				if (con != null)
				{
					con.BuffStack();
				}
			}
			foreach (Chara c in targets)
			{
				//ダメージ出力
				YzWrapper.AddDamage(this, CC, c, pow, ele);
			}
			//範囲表示
			YzUtil.ShowEffectiveArea(points, false);
			//
			return true;
        }
    }
}