using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class AbYzAutomap : Ability
    {
        public override bool Perform()
		{
			//ëŒè€ïsñæÇ»ÇÁèIóπ
			if (TC == null || TC.Chara == null)
			{
				return false;
			}
            if (!CC.IsPC)
            {
				CC.SayNothingHappans();
            }
            //
            if (CC.HasCondition<StYzAutomap>())
            {
				//EClass.pc.Say("--OFF--");
				CC.RemoveCondition<StYzAutomap>();
            }
            else
            {
				//EClass.pc.Say("--ON--");
				CC.AddCondition<StYzAutomap>();
            }
			return true;
		}
    }
}