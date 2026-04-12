using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using Cwl.Helper.Unity;
using Cwl.Helper.Extensions;

namespace yz_sss
{
    public class TraitYzBomb : Trait
    {
        //処理重複防止フラグ
        bool onCountdown;
        bool onExprode;
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
        //設置済み検出時にカウントダウン開始
        //マップ遷移処理や何らかの要因でカウントが止まったとき用
        public override void Update()
        {
            if (this.owner.IsInstalled)
            {
                if (!onCountdown && !onExprode)
                {
                    YzCountdown();
                }
            }
        }
        //設置時にカウントダウン開始
        //魔法処理の方(アイテム生成時)からも呼び出している
        public override void OnInstall(bool byPlayer)
        {
            if (!onCountdown && !onExprode)
            {
                YzCountdown();
            }
        }
        //起爆処理
        public void YzCountdown()
        {
            onCountdown = true;
            Zone calledZone = EClass._zone;
            CoroutineHelper.Deferred(
                () =>
                {

                    if (this.owner == null) return;
                    if (this.owner.isDestroyed) return;
                    if (calledZone != EClass._zone || !this.owner.IsInstalled)
                    {
                        onCountdown = false;
                        return;
                    }
                        //起爆
                        YzExplode();
                    },
                180
                );

        }
        //爆破処理
        private void YzExplode()
        {
            if (!onExprode)
            {
                onExprode = true;
                //Debug.Log($"pow:{this.owner.c_fixedValue}");
                //
                List<Point> targetPoints = new List<Point>();
                List<Point> endPoint = new List<Point>();
                int pow = this.owner.c_fixedValue;
                int range = Mathf.Clamp(pow / 150, 1, 8);
                Point origin = this.owner.pos.Copy();
                targetPoints.Add(origin);
                EClass._map.MineObj(origin);
                //座標取得
                Point[] directions =
                {
                        new Point(1,0),
                        new Point(-1,0),
                        new Point(0,1),
                        new Point(0,-1)
                    };
                foreach (var d in directions)
                {
                    Point p = origin.Copy();
                    for (int i = 0; i < range; i++)
                    {
                        bool mined = false;
                        bool isHardness = false;
                        p.x += d.x;
                        p.z += d.z;
                        //マップ端判定
                        if (!p.IsValid)
                        {
                            isHardness = true;
                        }
                        else
                        {

                            //硬度40(大理石)以上は破壊不可
                            //オブジェチェック
                            //Debug.Log($"p.hasBlock:{p.HasBlock},p.isBlock:{p.IsBlocked},cell.block:{p.cell.blocked},cell.isBlock{p.cell.IsBlocked}");
                            if (p.HasObj)
                            {
                                if (p.cell.matObj.hardness > 40)
                                {
                                    isHardness = true;
                                }
                                else
                                {
                                    //通過不可オブジェのときだけ採掘フラグ
                                    if (p.IsBlocked)
                                    {
                                        mined = true;
                                    }
                                    EClass._map.MineObj(p);
                                }
                            }
                            //壁チェック
                            if (p.HasBlock)
                                if (p.matBlock.hardness > 40)
                                {
                                    isHardness = true;
                                }
                                else
                                {
                                    mined = true;
                                    EClass._map.MineBlock(p);
                                }
                        }
                        //処理座標追加
                        //硬い壁検出で停止
                        if (isHardness)
                        {
                            if (i != 0)
                            {
                                endPoint.Add(targetPoints[^1]);
                            }
                            break;
                        }
                        //座標追加
                        targetPoints.Add(p.Copy());
                        //採掘したら停止
                        if (mined)
                        {
                            endPoint.Add(p.Copy());
                            break;
                        }
                        //最後までfor回したら追加
                        if (i == range - 1)
                        {
                            endPoint.Add(p.Copy());
                        }
                    }
                }
                //
                //ダメージとエフェクト処理
                List<Chara> targets = new List<Chara>();
                for (int i = 0; i < range; i++)
                {
                    YzUtil.DelayPlaySound("spell_ball", this.owner, i * 3, 1 - (i * 0.10f));
                }
                //マスごと処理
                foreach (Point tp in targetPoints)
                {
                    //Debug.Log(tp);
                    YzUtil.ShowEffectiveArea(tp);
                    YzWrapper.PlayVanillaEffect("Element/ball_Fire", tp);
                    targets = targets.Union(tp.Charas).ToList();
                    TraitYzBomb bomb = tp.FindThing<TraitYzBomb>();
                    if (bomb != null && bomb.owner != this.owner && !bomb.onExprode)
                    {
                        bomb.YzExplode();
                    }
                }
                //エフェクト
                foreach (Point ep in endPoint)
                {
                    YzUtil.PlayEffect("Bombfire", this.owner, TP: ep);
                }
                //ダメージ
                foreach (Chara c in targets)
                {
                    YzWrapper.AddDamage("SpYzBomb", c, pow, 910);
                    if (!c.isDead)
                    {
                        YzWrapper.DoHostileAction(EClass.pc, c, true);
                    }
                }
                //
                this.owner.Destroy();
            }
        }

    }
}