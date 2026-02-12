using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace yz_sss
{
	public class ConYzLockon : BaseBuff
	{
		//É^Å[Éìåoâﬂèàóù
		public override void Tick()
		{
			if (owner.enemy != null)
			{
				//Debug.Log($"{Owner.Name}:[{owner.enemy}]");
				//if (!owner.enemy.isDead)
				//YzUtil.PlayEffect("Lockon", owner.enemy, owner.enemy);
			}
            else
            {
				//Debug.Log("noTarget");
            }
		}
	}
}