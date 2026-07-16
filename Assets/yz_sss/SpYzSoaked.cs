using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzSoaked : Spell
    {
        public override bool Perform()
        {
            //詠唱エフェクト
            EClass.Wait(1f, CC);
            CastEffect.Call(CC);
            //
            List<Point> ep = YzWrapper.GetCirclePoint(TP, 2.5f, true, true);
            if (ep.Count == 0)
            {
                CC.SayNothingHappans();
                return true;
            }
            foreach (Point p in ep)
            {
                //雪解け
                if (p.cell.IsSnowTile)
                {
                    p.cell.isClearSnow = true;
                }
                //濡らし
                p.cell.isWatered = true;
                //水たまり
                if (!p.cell.blocked)
                {
                    if (EClass.rnd(3) == 0)
                    {
                        EClass._map.SetLiquid(p.x, p.z, 1, 1);
                    }
                }
                //火消し
                if (p.cell.HasFire)
                {
                    p.ModFire(-50, true);
                }
                //キャラクタ対象
                foreach (Chara chara in p.Charas)
                {
                    /*
                    if (chara.HasCondition<ConBurning>())
                    {
                        chara.Talk("thanks", null, null, false);
                    }
                    */
                    chara.AddCondition<ConWet>(100, false);
                }
            }
            //
            CC.Say("yz_UseSoaked", CC);
            YzUtil.ShowEffectiveArea(ep);
            YzUtil.PlayEffect("Soaked", CC, TC, TP: TP);
            CC.PlaySound("identify", 1f, true);
            ActEffect.TryDelay(delegate {
                CC.PlaySound("water_farm", 1f, true);
            });
            EClass.Wait(1f, CC);
            return true;
        }
    }
}