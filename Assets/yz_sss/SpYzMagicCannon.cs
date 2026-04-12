using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzMagicCannon : Spell, IExAction
    {
        public override bool Perform()
        {
			//詠唱エフェクトとウェイト
			EClass.Wait(0.4f, CC);
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
			//CC.PlaySound("identify", 0.1f, true); 
            CC.PlaySound("missile", 0.5f, true);
            //この辺からメイン処理
            //エフェクト再生
            ExActionData actionData = new ExActionData(this,EClass._zone,CC,TP);
            YzUtil.PlayEffect("MagicCannon", CC, TP:TP,actionData:actionData);
			//
			return true;
        }
		//着弾時処理(ObjectControllerから呼び出し)
		public void ExAction(ExActionData data)
		{
			int pow = YzWrapper.GetPower(data.CC, data.SP);
			Element ele = YzWrapper.GetElement(data.SP, pow);
            //効果音
            data.TP.PlaySound("explode", false, 1f, true);
            data.TP.PlaySound("attack_gun_gravity", false, 1f, true);
			//ダメージ出力
			var points = YzWrapper.GetCirclePoint(data.TP,2.5f,true,true);
			foreach (var p in points)
            {
                YzUtil.ShowEffectiveArea(p);
				var charas = p.ListCharas()?.ToArray();
				foreach (var c in charas)
                {
                    if (!YzUtil.AvoidFriendlyFire(data.CC, c))
                    {
                        YzWrapper.AddDamage(data.SP, data.CC, c, pow, ele);
                    }
                }
            }
        }
    }
}