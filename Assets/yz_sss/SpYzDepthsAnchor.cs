using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzDepthsAnchor : Spell
    {
        public override bool Perform()
        {
			//詠唱エフェクト
			EClass.Wait(0.8f, CC);
			CastEffect.Call(CC);
			//対象不明なら終了
			if(TC == null || TC.Chara == null)
            {
				return true;
			}
			//パラメータ取得
			int pow = YzWrapper.GetPower(CC, this);
			Element ele = YzWrapper.GetElement(this, pow);
			//エフェクト再生
			YzUtil.PlayEffect("Anchor", CC, TC);
			YzWrapper.PlayVanillaEffect(ele, TC.pos, 0.1f);
			CC.PlaySound("kick", 1f, true);
			CC.PlaySound("elevator", 1f, true);
			//移動処理
			if (CC.Dist(TC) > 1)
			{
				Point tempCP = CC.pos.Copy();
				Point tempTP = TC.pos.Copy();
				while(true) {
					int dx = tempTP.x - tempCP.x;
					int dy = tempTP.z - tempCP.z;
					int dist = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));
					if (dist <= 1)
						break;
					if (dx != 0)
						tempCP.x += (int)Mathf.Sign(dx);
					if (dy != 0)
						tempCP.z += (int)Mathf.Sign(dy);
					//
					dx = tempTP.x - tempCP.x;
					dy = tempTP.z - tempCP.z;
					dist = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));
					if (dist <= 1)
						break;
					if (dx != 0)
						tempTP.x -= (int)Mathf.Sign(dx);
					if (dy != 0)
						tempTP.z -= (int)Mathf.Sign(dy);
				}
				Debug.Log($"CC:{CC.pos} to {tempCP},TC:{TC.pos} to {tempTP}");
				tempCP = tempCP.GetNearestPoint(false, true, true, false, 0);
				tempTP = tempTP.GetNearestPoint(false, true, true, false, 0);
				CC.Say("abDraw", CC, TC, null, null);
				//TC.Chara.TryMove(tempTP);
				//CC.TryMove(tempCP);
				//
				TC.Chara._Move(tempTP,Card.MoveType.Force);
				CC._Move(tempCP, Card.MoveType.Force);
				//押しのけ
				YzUtil.PushChara(TC.Chara, tempTP);
				YzUtil.PushChara(CC.Chara, tempCP);
			}
			//ダメージ出力
			YzUtil.AddDebuff("ConFaint", 20 + pow / 200, CC, TC.Chara, true);
			YzWrapper.AddDamage(this, CC, TC.Chara, pow, ele);
			//
			return true;
        }
    }
}