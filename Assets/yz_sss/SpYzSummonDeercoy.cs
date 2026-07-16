using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static yz_sss.YzUtil;

namespace yz_sss
{
    internal class SpYzSummonDeercoy : Spell
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
			// ele = YzWrapper.GetElement(this, pow);
			//効果音(発動時)の再生
			CC.PlaySound("identify", 1f, true);
            //この辺からメイン処理
            //召喚座標
            Chara minion;
            Point point = TP.Copy().GetNearestPoint(false, false, true, false, 0);
            //
            if (point == null || !point.IsValid)
            {
                CC.SayNothingHappans();
                return false;
            }
            //
            CardBlueprint.Set(new CardBlueprint());
            if (CC.LV != -1)
            {
                CardBlueprint.current.lv = CC.LV;
            }
            minion = CharaGen.Create("yz_Deercoy", -1);
            //レベル調整
            //minion.interest = 0;
            minion.SetLv(mlv);
            //
            EClass._zone.AddCard(minion, point);
            minion.hostility = Hostility.Neutral;
            minion.AddCondition<ConYzDeercoy>(pow);
            //
            List<Chara> charas = EClass._map.ListCharasInCircle(point, 4);
            charas.RemoveAll(c => c.hostility != Hostility.Enemy || c.HasCondition<ConYzDeercoy>());
            if (charas.Count > 0)
            {
                /*
                minion.Chara.SetEnemy(charas.RandomItem<Chara>());
                minion.Chara.SetAIAggro();
                */
                YzWrapper.DoHostileAction(charas.RandomItem<Chara>(), minion, true);
                foreach (Chara c in charas)
                {
                    YzWrapper.DoHostileAction(minion, c, true);
                }
            }
            //
            YzUtil.PlayEffect("Summon1", minion, minion, TP: point);
            return true;
        }
    }
}