using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


namespace yz_sss
{
    internal class ConYzUB : BaseBuff
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
		public void BuffStack()
		{
			//持続ターン
			this.turn = 15;
			if (this.lv <= 2)
			{
				this.lv++;
			}
		}
		//ターン経過処理
		public override void Tick()
		{
			//
			int recoverMana = Mathf.Max((int)Mathf.Round(CC.mana.max * 0.01f),1);
			switch (lv)
			{
				case 1:
					CC.mana.Mod(recoverMana*1);
					break;
				case 2:
					CC.mana.Mod(recoverMana*2);
					break;
				case 3:
					CC.mana.Mod(recoverMana*5);
					break;
				default:
					break;
			}
			//Debug.Log($"leftTurn:{this.turn}");
			this.turn--;
			if (this.turn < 0)
			{
				base.Kill(false);
			}
		}
        //
		/*
        public override void OnRemoved()
        {
            base.OnRemoved();
			//this.owner.elements.ModBase(411, -50*lv);
		}
		*/
        //管理用
        [JsonProperty]
		public int turn;
		[JsonProperty]
		public int lv;
	}
}