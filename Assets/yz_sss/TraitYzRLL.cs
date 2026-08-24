using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using NPOI.SS.Formula.Functions;

namespace yz_sss
{
    public class TraitYzRLL : Trait
    {
        //持ち上げ不可
        public override bool CanBeHeld
        {
            get
            {
                return false;
            }
        }
        //アップデート(ターン経過)有効
        public override bool HaveUpdate
        {
            get
            {
                return true;
            }
        }
        public override void Update()
        {
            if(!EClass.pc.HasCondition<ConYzRLL>())
            {
                this.owner.Destroy();
            }
        }
        public override void OnStepped(Chara c)
        {
            if (c.IsPC)
            {
                ConYzRLL con = c.GetCondition<ConYzRLL>();
                if(con != null)
                {
                    con.CheckOnObject();
                }
            }
        }
        /*
        public override void OnEnterScreen()
        {
            EClass.pc.Say("[OnEnterScreen]");
            CheckEnd();
            base.OnEnterScreen();
        }
        public void CheckEnd()
        {
            bool check = false;
            foreach (Chara c in EClass._map.charas)
            {
                if(c.HasCondition<ConYzRLL>())
                {
                    check = true;
                }
            }
            if (!check)
            {
                del = true;
            }
            if (del)
            {
                if (!this.owner.isDestroyed)
                {
                    this.owner.Destroy();
                }
            }
        }
        */

    }
}