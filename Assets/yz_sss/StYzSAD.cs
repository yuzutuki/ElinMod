using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


namespace yz_sss
{
    internal class StYzSAD : BaseStance
    {
        public override void Tick()
        {
            if (!this.owner.IsPCFactionOrMinion)
            {
                this.Kill();
                return;
            }
            //
            if(tpCD > 0)
            {
                tpCD--;
            }
            else {
                if (this.owner.IsInCombat)
                {
                    //自身が戦闘中
                    if (this.owner.enemy != null)
                    {
                        Chara e = this.owner.enemy as Chara;
                        if (e != null && !e.isDead)
                        {
                            if (this.owner.Dist(e) > 1)
                            {
                                tpCD = 3;
                                this.owner.Teleport(e.pos.GetRandomNeighbor(),true);
                            }
                        }
                    }
                }
                else if (EClass.pc.IsInCombat)
                {
                    //PCが戦闘中
                    if (EClass.pc.enemy != null && !EClass.pc.enemy.isDead)
                    {
                        Chara e = EClass.pc.enemy as Chara;
                        if (e != null)
                        {
                            if (this.owner.Dist(e) > 1)
                            {
                                tpCD = 3;
                                this.owner.Teleport(e.pos.GetRandomNeighbor(),true);
                                this.owner.SetEnemy(e);
                                this.owner.SetAIAggro();
                            }
                        }
                    }
                }
                else
                {
                    //索敵
                    foreach (Point p in EClass._map.ListPointsInCircle(EClass.pc.pos, 5, false, false))
                    {
                        if (p.Charas.Count > 20) { break; }
                        foreach (Chara c in p.Charas)
                        {
                            if (c.IsHostile(EClass.pc) && EClass.pc.CanSeeLos(c, -1) && !c.isDead)
                            {
                                this.owner.SetEnemy(c);
                            }
                            if (this.owner.enemy != null && !this.owner.enemy.isDead) { break; }
                        }
                        if (this.owner.enemy != null && !this.owner.enemy.isDead) { break; }

                    }
                    if (this.owner.enemy != null && !this.owner.enemy.isDead)
                    {
                        Chara e = this.owner.enemy as Chara;
                        if (e != null && !e.isDead)
                        {
                            tpCD = 3;
                            this.owner.Teleport(e.pos.GetRandomNeighbor(),true);
                            this.owner.SetAIAggro();
                        }
                    }
                    //追従
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (this.owner.pos.Distance(EClass.pc.pos) > 2)
                            {
                                this.owner.TryMoveTowards(EClass.pc.pos);
                            }
                        }
                    }
                }
            }
            //
        }
        //管理用
        [JsonProperty]
        public int tpCD;
        //
    }
}