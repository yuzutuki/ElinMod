using BJ;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace yz_sss
{
    internal class ConYzRLL : BaseBuff
    {
        GameObject obj;
        ParticleSystem ps;
        EffectObjectController ctrl;
        //ステータス補正にElementsデータを使用
        public override bool UseElements
        {
            get
            {
                return true;
            }
        }
        //ターン経過処理

        public override void Tick()
        {
            if (castZone != EClass._zone.uid)
            {
                if (target != null)
                {
                    target.Destroy();
                }
                this.Kill();
                return;
            }
            if (target == null)
            {
                GetTarget();
            }
            if (target.isDestroyed)
            {
                this.Kill();
                return;
            }
            CheckOnObject();
            // Debug.Log("tick");
            base.Tick();
        }

        //
        public override void OnRemoved()
        {
            if (target != null)
            {
                target.Destroy();
            }
            if (obj != null)
            {
                ctrl.StopEmitALL();
            }
            this.owner.AddCooldown(720049, 60);
            base.OnRemoved();
        }
        //
        public override void OnStart()
        {
            this.elements.SetBase(1648, 2, 0);
            castZone = EClass._zone.uid;
            if (target == null)
                GetTarget();
            if (obj == null)
                GetObject();
            base.OnStart();
        }
        //Object取得
        public void CheckOnObject()
        {
            if (target != null)
            {
                //
                if (obj == null)
                {
                    GetObject();
                }
                //
                Point cp = this.owner.pos;
                Point tp = this.target.pos;
                if (cp.Distance(tp) < 1)
                {
                    // this.owner.Say("のってる");
                    //var psm = ps.main;
                    //psm.startColor = new Color(1, 1, 1, 1);
                    ctrl.SetStartColor(new Color(1, 1, 0.8f), false);
                    this.elements.SetBase(1648, 2, 0);
                }
                else
                {
                    //this.owner.Say("おりてる");
                    // var psm = ps.main;
                    //psm.startColor = new Color(0.1f, 0.1f, 1, 0.5f);
                    ctrl.SetStartColor(new Color(0.1f, 0.1f, 1), false);
                    this.elements.SetBase(1648, 0, 0);
                }
            }
        }
        public void GetObject()
        {
            if(obj == null)
            {
                obj = GameObject.Find("RLL(Clone)");
            }
            if (obj == null)
            {
                Debug.Log("RLL(clone) not found");
                YzUtil.PlayEffect("RLL", this.owner, TP: target.pos.Copy());
                GetObject();
            }
            else
            {
                Debug.Log("RLL(clone) found");
                ps = obj.GetComponent<ParticleSystem>();
                ctrl = obj.GetComponent<EffectObjectController>();
                Vector3 pv = this.owner.renderer.PositionCenter();
                pv.y -= 0.2f;
                pv.z -= 2;
                ctrl.ReplaceVector(pv);
                //Vector3 tv = this.target.renderer.PositionCenter();
                //ctrl.ReplaceVector(tv);
            }
        }
        //Card取得
        public void GetTarget()
        {
            bool detect = false;
            foreach (Thing thing in EClass._map.things)
            {
                if (thing.id == "YzRLL")
                {
                    //Debug.Log("detected YzRLL");
                    this.target = thing as Card;
                    detect = true;
                }
            }
            if (!detect)
            {
                this.Kill();
            }
        }
        //再配置
        public void Recast(Point OP)
        {
            if (target == null)
                GetTarget();
            Point tp = target.pos.Copy();
            if (!OP.Equals(tp))
            {
                //target.pos = OP.Copy();
                target._Move(OP, Card.MoveType.Force);
                if (obj == null || ctrl == null)
                    GetObject();
                if (ctrl != null)
                {
                    //ctrl.ResetPoint(OP);
                    //Vector3 tv = this.target.renderer.PositionCenter();
                    //Debug.Log($"Target Vector: {this.target.renderer.position}, center: {tv}");
                    //Debug.Log($"PCVector:{EClass.pc.renderer.position}, center: {EClass.pc.renderer.PositionCenter()}");
                    //Debug.Log($"Owner Vector: {this.owner.renderer.position}, center: {pv}");
                    Vector3 pv = this.owner.renderer.PositionCenter();
                    pv.y -=0.2f;
                    pv.z -= 2;
                    ctrl.ReplaceVector(pv);
                }
            }
        }
        //
        //管理用
        [JsonProperty]
        public int castZone;
        [JsonProperty]
        public Card target;

    }
}