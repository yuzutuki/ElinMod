using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzConfiteor : Spell
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
			float powFix = 1f;
			int buffLevel = 0;
			Element ele = YzWrapper.GetElement(this, pow);
			//バフ管理
			ConYzConfiteor con = (CC.AddCondition<ConYzConfiteor>(100, false) as ConYzConfiteor) ?? CC.GetCondition<ConYzConfiteor>();
			if (con != null)
			{
				buffLevel = con.lv;
				con.BuffStack();
			}
			//効果音(発動時)の再生
			CC.PlaySound("atk_eleSound", 0.5f, false);
			//この辺からメイン処理
			//ダメージ出力やエフェクトは適宜入れ替える
			//
			float radius = 2;
            //エフェクト再生
            string msg = this.Name;
            switch (buffLevel)
            {
				case 0:
                    YzUtil.PlayEffect("Confiteor1", CC, TC, TP);
					powFix = 1f;
					YzUtil.DelayPlaySound("ab_swarm", CC, 8, 0.8f, false);
					break;
				case 1:
                    msg = Msg.GetGameText("yz_Confi2");
                    YzUtil.PlayEffect("Confiteor2", CC, TC, TP);
					powFix = 1.52f;
					for (int i=0; i < 6; i++)
					{
						YzUtil.DelayPlaySound("ab_magicsword", CC, 8+(i*3), 0.3f, false);
					}
					break;
				case 2:
                    msg = Msg.GetGameText("yz_Confi3");
                    YzUtil.PlayEffect("Confiteor3", CC, TC, TP);
					powFix = 1.76f;
					radius = 2.5f;
					YzUtil.DelayPlaySound("ab_shred", CC, 11, 1f, false);
					YzUtil.DelayPlaySound("ab_shred", CC, 12, 1f, false);
					YzUtil.DelayPlaySound("ab_shred", CC, 13, 1f, false);
					break;
				case 3:
                    msg = Msg.GetGameText("yz_Confi4");
                    YzUtil.PlayEffect("Confiteor4", CC, TC, TP);
					powFix = 2f;
					radius = 3f;
					YzUtil.DelayPlaySound("ab_shred2", CC, 10, 0.7f, false);
					YzUtil.DelayPlaySound("ab_swarm", CC, 19, 1f, false);
					YzUtil.DelayPlaySound("ab_swarm", CC, 20, 1f, false);
					break;
				default:
					break;
            }
            //技名表示
            if (CC.IsPC && ModConfig.SpPop.Value)
            {
                CC.renderer.Say("| " + msg + " ", default(Color), 0f);
            }
            //
            Debug.Log($"lv:{buffLevel},fix:{powFix},pow:{(int)(pow*powFix)}");
			//範囲とターゲット取得
			List<Point> points = YzWrapper.GetCirclePoint(TP, radius, false, false);
			List<Chara> targets = YzUtil.AvoidFriendlyFire(CC, points);
			foreach (Chara c in targets)
			{
				//ダメージ出力
				YzWrapper.AddDamage(this, CC, c, pow, ele,damageMult:powFix);
				YzWrapper.PlayVanillaEffect(ele, TC.pos, 0.2f);
			}
			//範囲表示
			YzUtil.ShowEffectiveArea(points, false);
			
			//
			return true;
        }
    }
}