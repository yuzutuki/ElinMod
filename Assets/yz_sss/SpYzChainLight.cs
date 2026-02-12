using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
	//クラス名を変えること！
    internal class SpYzChainLight : Spell
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
			//この辺からメイン処理
			List<Chara> targets = new List<Chara>();
			targets.Add(TC.Chara);
			//ターゲット決定
			for(int i=0; i < 4; i++)
            {
				Point tempPoint = targets[i].pos.Copy();
				List<Point> tempPoints = YzWrapper.GetCirclePoint(tempPoint, 4, false, true);
				List<Chara> tempTargets = YzUtil.AvoidFriendlyFire(CC, tempPoints);
				//同一ターゲット回避
				tempTargets.RemoveAll(t => targets.Contains(t));
				if (tempTargets.Count <= 0)
				{
					Debug.Log("-ChainLightnig:no tagert-");
					break;
				}
				targets.Add(YzUtil.GetFarthestChara(tempPoint,tempTargets));
			}
			//
			Chara originChara = CC;
			int count = 0;
			foreach (Chara target in targets)
			{
				//ダメージ出力
				YzWrapper.AddDamage(this, CC, target, pow, ele);
				//エフェクト再生
				YzUtil.PlayEffect("ChainLight", originChara, target);
				YzUtil.PlayEffect("ChainLight2", originChara, target);
				YzUtil.PlayEffect("ChainLight3", originChara, target);
				YzUtil.ShowEffectiveArea(target.pos.Copy(),2);
				//
				YzWrapper.PlayVanillaEffect(ele, target.pos, 0.1f*count);
				YzUtil.DelayPlaySound("spell_bolt", CC,count*3,0.2f - (0.2f*count));
				//原点更新
				originChara = target;
				count++;
			}
			//
			return true;
        }
    }
}