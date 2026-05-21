using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace yz_sss
{
    public class ConYzSovereign : BaseBuff
    {
        //ターン数表示を強度に上書き
        public override string TextDuration
        {
            get
            {
                return (this.stack + 100).ToString() + "%";
            }
        }


        public override bool CanStack(Condition c)
        {
            return true;
        }
        //スタックの追加処理
        public bool BuffStack(int stackPlus)
        {
            int smithLv = YzWrapper.GetLevel(this.owner, 256);
            this.stack += stackPlus;
            this.stack = YzWrapper.Curve(this.stack, 100 + smithLv / 2, 50 + smithLv / 10, 95);
            this.stack = YzWrapper.Curve(this.stack, 500 + smithLv, 50 + smithLv / 20, 90);
            this.stack += 10;
            //9999%ならfalse返す
            if (this.stack >= 9999)
            {
                this.stack = 9999;
                return false;
            }
            return true;
        }
        /*
        public override void OnStart()
        {
            this.castZone = EClass._zone.uid;
            //Debug.Log($"zone: {this.castZone}");
        }
        */
        public void SetCastZone(int zoneUid)
        {
            this.castZone = zoneUid;
        }
        //ターン経過処理
        public override void Tick()
        {
            int currentZone = EClass._zone.uid;
            if (currentZone != this.castZone)
            {
                //Debug.Log($"Zone changed: {currentZone} != {this.castZone}");
                base.Kill(false);
            }
        }
        //管理用
        [JsonProperty]
        public int castZone;
        [JsonProperty]
        public int stack;
    }
}