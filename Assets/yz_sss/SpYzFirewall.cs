using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzFirewall : Spell
    {
        public override bool Perform()
        {
            //詠唱エフェクト
            EClass.Wait(1f, CC);
            CastEffect.Call(CC);
            List<Point> ep = YzWrapper.GetCirclePoint(TP,2.5f, true, true);
            if(ep.Count == 0)
            {
                CC.SayNothingHappans();
                return true;
            }
                foreach(Point p in ep)
            {
                //雪解け
                if (p.cell.IsSnowTile)
                {
                    p.cell.isClearSnow = true;
                }
                //着火
                p.ModFire(5, false);
                }

            //
            CC.Say("yz_UseFirewall",CC);
            List<Point> tps = YzUtil.DistanceSort(CC.pos,ep);
            YzUtil.ShowEffectiveArea(tps);
            YzUtil.PlayEffect("Firewall", CC, TC,TP: TP);
            ActEffect.TryDelay(delegate{
                CC.PlaySound("identify", 1f, true);
            });
            EClass.Wait(1f,CC);
            return true;
        }
    }
}