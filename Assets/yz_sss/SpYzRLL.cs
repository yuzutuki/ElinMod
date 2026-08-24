using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzRLL : Spell
    {
        public override bool Perform()
        {
			//詠唱エフェクトとウェイト
			EClass.Wait(0.8f, CC);
			CastEffect.Call(CC);
            //
            if (!CC.IsPC)
            {
                CC.SayNothingHappans();
            }
            Chara cc = CC.Chara;
            //効果音
            CC.PlaySound("remove_hex", 1f, true);
            CC.PlaySound("intonation", 0.3f, true);
            //この辺からメイン処理
            //YzUtil.PlayEffect("RLL", CC,TP:CC.pos.Copy());
            if (cc.HasCondition<ConYzRLL>())
            {
                //再配置
                var con = cc.GetCondition<ConYzRLL>();
                if (con != null)
                {
                    con.Recast(cc.pos.Copy());
                }
            }
            else
            {
                //Thing作成
                Thing t = ThingGen.Create("YzRLL");
                EClass._zone.AddCard(t, CC.pos).Install();
                t.trait.OnInstall(true);
                //
                CC.AddCondition<ConYzRLL>();
            }
            //
            return true;
        }
    }
}