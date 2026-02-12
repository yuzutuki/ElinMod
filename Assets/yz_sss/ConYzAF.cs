using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


namespace yz_sss
{
    internal class ConYzAF : BaseBuff
	{
		//ターン数表示を強度に上書き
		public override string TextDuration
		{
			get
			{
				return "Lv." + this.lv.ToString();
			}
		}

		//ステータス補正にElementsデータを使用
		public override bool UseElements
		{
			get
			{
				return true;
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
			if (this.lv <= 2)
			{
				this.lv++;
				//
				BuffUpdate();
			}


		}
		//ターン経過処理
		public override void Tick()
		{
			//Debug.Log($"leftTurn:{this.turn}");
			BuffUpdate();
			this.turn--;
			if (this.turn < 0)
			{
				base.Kill(false);
			}
		}
		//
		void BuffUpdate()
		{
			if (this.elements == null)
			{
				Debug.Log("-= ConAF:missingElements =-");
			}
			else
			{
				switch (lv)
				{
					case 1:
						this.elements.SetBase(411, 10, 0);
						break;
					case 2:
						this.elements.SetBase(411, 20, 0);
						break;
					case 3:
						this.elements.SetBase(411, 50, 0);
						break;
					default:
						break;
				}
			}
		}
			//}
			//管理用
			[JsonProperty]
		public int turn;
		[JsonProperty]
		public int lv;
	}
}