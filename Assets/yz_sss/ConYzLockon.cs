using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace yz_sss
{
	public class ConYzLockon : BaseBuff
	{
        //ロックオンマーカー表示用バフ
        //実際の処理はAct.PerformおよびActRange.Performへのパッチで行う
		//
		//以下旧処理
        //ターン経過処理
        /*
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
		*/
    }
}