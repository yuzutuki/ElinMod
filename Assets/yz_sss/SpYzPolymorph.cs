using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzPolymorph : Spell
    {
        public override bool Perform()
        {
            //詠唱エフェクト
            EClass.Wait(0.8f, CC);
            CastEffect.Call(CC);
            //処理部分
            if (TC.Chara == null)
            {
                CC.SayNothingHappans();
                return true;

            }
            CC.Say("yz_PolymorphText");

            int power = YzWrapper.GetPower(CC,this);
            Debug.Log($"base.source.id:{base.source.id},power:{power}");
            if (!YzUtil.AddDebuff("ConYzPolymorph", power, CC, TC.Chara))
            {
                return true;
            }

            //プチ変容などを参考にして書く予定
            CC.PlaySound("identify", 1f, true);
            CC.Say("yz_UsePolymorph", CC, TC, null, null);
            YzUtil.PlayEffect("Polymorph", CC, TC);
            //
            return true;
        }

    }
    //

}
