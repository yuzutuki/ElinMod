using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


namespace yz_sss
{
    internal class StYzAutomap : BaseStance
	{
        //Zone.Activateへのパッチで処理
        /*
        以下旧処理
		//ターン経過処理
		public override void Tick()
		{
            if (EClass._zone != zone)
			{
				Debug.Log("ゾーン遷移");
				zone = EClass._zone;
				CC.Say("abMagicMap", CC, null, null);
				CC.PlayEffect("identify", true, 0f, default(Vector3));
				CC.PlaySound("identify", 1f, true);
				Element mm = CC.elements.GetOrCreateElement(8260);
				Debug.Log($"cost:{mm.GetCost(CC)},lv:{mm.Value},Potential:{mm.Potential}");
				//if (CC.elements.GetOrCreateElement(8260).GetCost)
				int pow = YzWrapper.GetPower(CC, 8260);
				Debug.Log($"magicMapPow:{pow}");
				EClass._map.Reveal(CC.pos, pow);
			}
			//
		}
	*/
        //
    }
}