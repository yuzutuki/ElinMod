using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzAbyssal : Spell
    {
        public override bool Perform()
        {
            //詠唱エフェクトとウェイト
            EClass.Wait(0.8f, CC);
            CastEffect.Call(CC);
            //対象不明なら終了
            if (TC == null || TC.Chara == null)
            {
                return false;
            }
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
            int pow = YzWrapper.GetPower(CC, this);
            Element ele = YzWrapper.GetElement(this, pow);
            //効果音(発動時)の再生
            for (int i = 0; i < 4; i++)
            {
                YzUtil.DelayPlaySound("ab_magicsword", CC, 8 + (i * 3), 0.8f - i, false);
            }
            //この辺からメイン処理
            //貫通率
            int aPen = 5;
            //			aPen += EClass.curve(mlv, 10, 5, 75);
            aPen += YzWrapper.Curve(mlv, 10, 5, 75);
            aPen = Mathf.Min(aPen, 100);
            //ダメージ出力
            YzWrapper.AddDamage(this, CC, TC.Chara, pow, Element.Void, AS: AttackSource.Melee, penetration: aPen);
            YzUtil.AddDebuff("ConParalyze", 100 + pow / 10, CC, TC.Chara, false);
            //エフェクト再生
            YzUtil.PlayEffect("Abyssal", TC, TC);
            //
            return true;
        }
    }
}