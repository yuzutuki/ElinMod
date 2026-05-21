using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


namespace yz_sss
{
    internal class StYzByside : BaseStance
    {
		public override void Tick()
		{
            if (!this.owner.IsPCFactionOrMinion)
            {
                this.Kill();
                return;
            }
            if (!EClass.pc.IsInCombat)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (this.owner.pos.Distance(EClass.pc.pos) > 1)
                    {
                        this.owner.TryMoveTowards(EClass.pc.pos);
                    }
                }
            }
            /*
            if (this.owner.HasCooldown(1225))
            {
                this.owner.SetCooldown(1225, 0);
            }
            */
			//
		}
        //
    }
}