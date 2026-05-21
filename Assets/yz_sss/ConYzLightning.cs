using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


namespace yz_sss
{
    internal class ConYzLightning : BaseBuff
    {
        //ターン数表示を強度に上書き
        public override string TextDuration
        {
            get
            {
                return this.stack.ToString();
            }
        }

        public override bool CanStack(Condition c)
        {
            return true;
        }
        //スタックの追加処理
        public void AddStack(int s)
        {
            this.stack += s;
        }
        //ターン経過処理
        public override void Tick()
        {
            //
            Cast();
            //Debug.Log($"leftTurn:{this.turn}");
            this.stack--;
            if (this.stack <= 0)
            {
                base.Kill(false);
            }
        }
        public void Cast()
        {
            int count = Mathf.Min(this.stack, 10);
            int pow = YzWrapper.GetPower(this.owner, 720048);
            if (pow == 0) pow = 100;
            Chara tc = null;
            //索敵
            for (int i = 0; i < count; i++)
            {
                float mult = 1f;
                if (i == 0)
                {
                    mult = 2.5f;
                }
                //
                YzUtil.DelayAction(() =>
                {
                    Point cp = this.owner.pos.Copy();
                    List<Point> tempPoints = YzWrapper.GetCirclePoint(cp, 4, false, true);
                    List<Chara> tempTargets = YzUtil.AvoidFriendlyFire(this.owner, tempPoints);
                    if(tempTargets.Count > 0)
                    {
                        tc = tempTargets.RandomItem<Chara>();
                        tc.PlaySound("attack_gun_shotgun", 1f, true);
                        YzWrapper.AddDamage("SpYzTempest", tc, pow, 912, damageMult: mult);//912:雷
                        YzUtil.PlayEffect("Tempest2", tc, tc);
                    }
                }, i * 4);
            }

            /*
             List<Point> tempPoints = YzWrapper.GetCirclePoint(tp, 4, false, true);
                List<Chara> tempTargets = YzUtil.AvoidFriendlyFire(CC, tempPoints);
                if(tempTargets.Count > 0)
                {
                    var nextTarget = tempTargets.RandomItem();
                    var nextPoint = nextTarget.pos.Copy();
                    //
                    ExActionData actionData = new ExActionData(this, EClass._zone, CC, nextPoint.Copy(), nextTarget, subFloat: count - 1);
                    YzUtil.PlayEffect("Flask", CC, nextTarget, TP: nextPoint.Copy(), fromP: tp.Copy(), actionData: actionData);
                }
            */
        }
        //
        //管理用
        [JsonProperty]
        public int stack;
    }
}