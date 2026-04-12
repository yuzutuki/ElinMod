using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;


namespace yz_sss
{
    internal class ConYzDeercoy : BaseBuff
    {
        int tempHP;
        bool onExplosion = false;
        //ターン数表示を強度に上書き
        public override string TextDuration
        {
            get
            {
                return "Pow." + this.power;
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
        public override void OnStart()
        {
            tempHP = this.owner.hp;
            base.OnStart();
        }
        public override void OnRemoved()
        {
            Explosion(this.owner.pos.Copy());
            base.OnRemoved();
        }
        //ターン経過処理
        public override void Tick()
        {
            //Debug.Log($"Deercoy Tick, tempHP:{tempHP},ownerHP:{this.owner.hp}");
            if (this.owner.hp < tempHP || this.owner.hp < this.owner.MaxHP)
            {
                Explosion(this.owner.pos.Copy());
            }
        }
        //
        public void Explosion(Point tp)
        {
            if (!onExplosion)
            {
                onExplosion = true;
                if (tp == null)
                {
                    Debug.Log("-= explosion failed, tp is null");
                    return;
                }
                //Debug.Log($"owner:{this.owner.Name},CC:{CC.Name}");
                //Debug.Log($"Deercoy Explosion!,pow:{power},lv:{this.owner.LV}");
                //
                this.owner.PlaySound("explode", 1f, true);
                YzUtil.PlayEffect("noitaExp", this.owner, TP: tp);
                //この辺からメイン処理
                int pow = this.power;
                float radius = Mathf.Clamp(EClass.curve(owner.LV,10,5,25)*0.12f+1, 1.5f, 4f);
                List<Point> points = YzWrapper.GetCirclePoint(tp, radius, false, false);
                foreach (var p in points)
                {
                    //破壊処理
                    //オブジェクトチェック
                    if (p.HasObj)
                        if (p.cell.matObj.hardness < 40)
                        {
                            EClass._map.MineObj(p);
                            //YzWrapper.PlayVanillaEffect("Element/ball_Impact", p);
                        }
                    //壁チェック
                    if (p.HasBlock)
                        if (p.matBlock.hardness < 40)
                        {
                            EClass._map.MineBlock(p);
                            //YzWrapper.PlayVanillaEffect("Element/ball_Impact", p);
                        }
                    //エフェクト
                    YzUtil.ShowEffectiveArea(p);
                    //
                    var targets = p.ListCharas()?.ToArray();
                    if (targets != null)
                    {
                        foreach (var c in targets)
                        {
                            if (c != null && !c.isDead && !c.isDestroyed)
                            {
                                YzUtil.DelayAction(() =>
                                {
                                    YzWrapper.AddDamage("SpYzSummonDeercoy", c, pow, 925);
                                    YzWrapper.PlayVanillaEffect("Element/ball_Impact", p);
                                },6);
                            }
                        }
                    }
                }
                this.owner.hp = 0;
                this.owner.Die();

            }
        }
        //
    }
}