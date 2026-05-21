using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzSummonHand : Spell
    {
        public override bool Perform()
        {
            //詠唱エフェクトとウェイト
            EClass.Wait(0.8f, CC);
            CastEffect.Call(CC);
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
            int pow = YzWrapper.GetPower(CC, this);
            // ele = YzWrapper.GetElement(this, pow);
            //効果音(発動時)の再生
            CC.PlaySound("identify", 1f, true);
            //この辺からメイン処理
            YzUtil.SummonMinion("yz_Osty", CC, TP.Copy(), 1, YzUtil.LevelAdjust.Unique, pow, true);
            //
            //YzWrapper.PlayVanillaEffect(ele, TC.pos,1f);
            //
            return true;
        }
    }
}