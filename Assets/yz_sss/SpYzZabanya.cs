using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cwl.Helper.Unity;

namespace yz_sss
{
    internal class SpYzZabanya : Spell
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
			//Element ele = YzWrapper.GetElement(this, pow);
			//効果音(発動時)の再生
			CC.PlaySound("reboot", 1f, true);
            CC.PlaySound("boost", 1f, true);
            //この辺からメイン処理
            //初期目標の取得
            List<Point> points = YzWrapper.GetCirclePoint(CC.pos, 13, false, false);
            List<Chara> targets = YzUtil.AvoidFriendlyFire(CC, points);
            //専用の召喚処理
            //召喚座標リスト
            Point[] summonPoints =
            {
                new Point(1,1),
                new Point(1,-1),
                new Point(-1,-1),
                new Point(-1,1),
                new Point(2,0),
                new Point(-2,0),
            };
            //
            for (int i = 0; i < 6; i++)
            {
                //召喚メイン処理

                Chara minion = new Chara();
                Point summonPoint = TP.Copy();
                summonPoint.x += summonPoints[i].x;
                summonPoint.z += summonPoints[i].z;
                //召喚座標
                summonPoint = summonPoint.GetNearestPoint(false, true, true, false, 0);
                if (summonPoint != null && summonPoint.IsValid)
                {
                    CardBlueprint.Set(new CardBlueprint());
                    if (CC.LV != -1)
                    {
                        CardBlueprint.current.lv = CC.LV;
                    }
                    if (i < 2)
                    {
                        minion = CharaGen.Create("yz_zabanya1", -1);
                    }
                    else if (i < 4)
                    {
                        minion = CharaGen.Create("yz_zabanya3", -1);
                    }
                    else
                    {
                        minion = CharaGen.Create("yz_zabanya2", -1);
                    }
                    //興味
                    minion.interest = 0;
                    //レベル
                    minion.SetLv(CC.LV);
                    //
                    EClass._zone.AddCard(minion, summonPoint);
                    minion.MakeMinion(CC, MinionType.Default);
                    minion.SetSummon(28);
                    //ステータス上書き
                    //79:SPD
                    minion.elements.SetBase(79, (int)(CC.Speed * 2.5f));
                    //72:DEX 73:PER 76:MAG
                    minion.elements.SetTo(72, (int)Mathf.Max(CC.Evalue(72)*0.95f, minion.Evalue(72)));
                    minion.elements.SetTo(73, (int)Mathf.Max(CC.Evalue(73)*0.95f, minion.Evalue(73)));
                    minion.elements.SetTo(76, (int)Mathf.Max(CC.Evalue(76) * 0.9f, minion.Evalue(76)));
                    //105:銃 133:射撃 304:詠唱
                    minion.elements.SetTo(105, (int)Mathf.Max(CC.Evalue(105) * 0.95f, minion.Evalue(105)));
                    minion.elements.SetTo(133, (int)Mathf.Max(CC.Evalue(133) * 0.95f, minion.Evalue(133)));
                    minion.elements.SetTo(304, (int)Mathf.Max(CC.Evalue(304) * 0.95f, minion.Evalue(304)));
                    //
                    YzUtil.PlayEffect("Summon3", minion, minion, summonPoint);
                    YzUtil.PlayEffect("Summon2", CC, minion);
                    //敵視設定
                    minion.AddCondition<ConYzLockon>();
                    minion.enemy = targets[Random.Range(0, targets.Count)];
                    //minion.FindNearestNewEnemy();
                    minion.SetAIAggro();
                }

            }
            //
            return true;
        }
    }
}