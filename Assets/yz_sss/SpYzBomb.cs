using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzBomb : Spell
    {
        public override bool Perform()
        {
            //詠唱エフェクトとウェイト
            EClass.Wait(0.2f, CC);
            CastEffect.Call(CC);
            //重ねて置くの防止
            TraitYzBomb check = CC.pos.FindThing<TraitYzBomb>();
            if (check != null)
            {
                //Debug.Log("-=( ) detectBomb");
                CC.SayNothingHappans();
                return false;
            }
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
            int pow = YzWrapper.GetPower(CC, this);
            Element ele = YzWrapper.GetElement(this, pow);
            //効果音(発動時)の再生
            CC.PlaySound("Material/rock_drop", 1f, true);
            CC.PlaySound("Material/wood_drop", 0.3f, true);
            //この辺からメイン処理
            //Thing作成
            Thing t = ThingGen.Create("YzBomb");
            t.LV = 5;
            t.c_fixedValue = pow;
            EClass._zone.AddCard(t, CC.pos).Install();
            t.trait.OnInstall(true);
            //
            return true;
        }
    }
}