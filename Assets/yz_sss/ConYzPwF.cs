using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace yz_sss
{
	public class ConYzPwF : BaseBuff
	{
		//ターン数表示を強度に上書き
		public override string TextDuration
		{
			get
			{
				return "Lv." + this.lv.ToString();
			}
		}


		public override bool CanStack(Condition c)
		{
			return true;
		}
		//スタックの追加処理
		public bool BuffStack()
        {
			//Debug.Log("Stack++");
			//持続ターン
			this.turn = 50;
			this.lv++;
			//フルスタックならtrue返す
            if (this.lv >= 5)
            {
				this.lv = 0;
				this.turn = 0;
				return true;
			}
			return false;
		}
		//ターン経過処理
		public override void Tick()
		{
			//Debug.Log($"leftTurn:{this.turn}");
			this.turn--;
			if (this.turn < 0)
			{
				base.Kill(false);
			}
		}
		//管理用
		[JsonProperty]
		public int turn;
		[JsonProperty]
		public int lv;
	}
}