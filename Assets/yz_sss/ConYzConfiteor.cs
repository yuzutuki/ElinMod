using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace yz_sss
{
    internal class ConYzConfiteor : BaseBuff
	{
		//ターン数表示を強度に上書き
		public override string TextDuration
		{
			get
			{
				return "Count." + this.lv.ToString();
			}
		}

		public override bool CanStack(Condition c)
		{
			return true;
		}
		//スタックの追加処理
		public void BuffStack()
		{
			//持続ターン
			this.turn = 15;
			if (this.lv < 3)
			{
				this.lv++;
            }
            else
            {
				base.Kill(true);
            }
		}
		//ターン経過処理
		public override void Tick()
		{
			this.turn--;
			if (this.turn < 0)
			{
				base.Kill(true);
			}
		}
		//
        public override void OnRemoved()
        {
			this.owner.AddCooldown(720030, 300);
			base.OnRemoved();
		}
		//管理用
		[JsonProperty]
		public int turn;
		[JsonProperty]
		public int lv;
	}
}