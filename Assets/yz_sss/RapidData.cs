using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace yz_sss
{
	public class RapidData
	{
		public int max;
		public int current;
		public RapidData(Chara CC)
		{
			max = CC.Evalue(1648);
			current = 0;
		}
		//連射ディレイ
		public int RapidDelay()
		{
			int delay = (int)((float)current * ActEffect.RapidDelay * (100/Mathf.Max(max + 1, 1)));
			return delay;
		}
	}
}