using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzFlask : Spell, IExAction
    {
        public override bool Perform()
        {
            //詠唱エフェクトとウェイト
            EClass.Wait(0.8f, CC);
            CastEffect.Call(CC);
            //対象不明なら終了
            if (TC == null || TC.Chara == null)
            {
                return false;
            }
            if (YzUtil.CheckFailSelf(this, CC, TC, TP))
            {
                return false;
            }
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
            int pow = YzWrapper.GetPower(CC, this);
            Element ele = YzWrapper.GetElement(this, pow);
            //効果音(発動時)の再生
            CC.PlaySound("throw", 1f, true);
            CC.PlaySound("Material/glass_drop", 1f, true);
            //この辺からメイン処理
            //ダメージ出力やエフェクトは適宜入れ替える
            //
            //エフェクト再生
            ExActionData actionData = new ExActionData(this, EClass._zone, CC, TP, TC.Chara,subFloat:3f);
            YzUtil.PlayEffect("Flask", CC, TC, TP: TP.Copy(),fromP:CC.pos.Copy(), actionData: actionData);
            //
            //
            return true;
        }
        //着弾時処理(ObjectControllerから呼び出し)
        public void ExAction(ExActionData data)
        {
            int count = (int)data.SubFloat;
            Point tp = data.TP;
            if (data.TC != null)
            {
                tp = data.TC.pos.Copy();
            }
            int pow = YzWrapper.GetPower(data.CC, data.SP);
            Element ele = YzWrapper.GetElement(data.SP, pow);
            //効果音
            CC.PlaySound("Material/glass_dead", 1f, true);
            //
            //ダメージ出力
            var charas = tp.ListCharas()?.ToArray();
            if (charas != null && charas.Length > 0)
            {
                foreach (var c in charas)
                {
                    if (!YzUtil.AvoidFriendlyFire(data.CC, c))
                    {
                        YzWrapper.AddDamage(data.SP, data.CC, c, pow, ele);
                    }
                }
            }
            YzWrapper.PlayVanillaEffect(ele, tp, 0f);
            YzUtil.PlayEffect("Flask2", data.CC, TP:tp.Copy());
            //跳ね
            if (count > 0)
            {
                //
                data.CC.PlaySound("throw", 1f, true);
                data.CC.PlaySound("Material/glass_drop", 1f, true);
                //次のターゲット索敵
                List<Point> tempPoints = YzWrapper.GetCirclePoint(tp, 4, false, true);
                List<Chara> tempTargets = YzUtil.AvoidFriendlyFire(data.CC, tempPoints);
                if(tempTargets.Count > 0)
                {
                    var nextTarget = tempTargets.RandomItem();
                    var nextPoint = nextTarget.pos.Copy();
                    //
                    ExActionData actionData = new ExActionData(this, EClass._zone, data.CC, nextPoint.Copy(), nextTarget, subFloat: count - 1);
                    YzUtil.PlayEffect("Flask", data.CC, nextTarget, TP: nextPoint.Copy(), fromP: tp.Copy(), actionData: actionData);
                }
                /*
                List<Chara> targets = new List<Chara>();
                targets.Add(TC.Chara);
                //ターゲット決定
                for (int i = 0; i < 4; i++)
                {
                    Point tempPoint = targets[i].pos.Copy();
                    //同一ターゲット回避
                    tempTargets.RemoveAll(t => targets.Contains(t));
                    if (tempTargets.Count <= 0)
                    {
                        Debug.Log("-ChainLightnig:no tagert-");
                        break;
                    }
                    targets.Add(YzUtil.GetFarthestChara(tempPoint, tempTargets));
                }
                */
            }
        }
    }
}