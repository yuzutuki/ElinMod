using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzGoblin : Spell
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
            //Debug.Log("1");
            if (YzUtil.CheckFailSelf(this, CC, TC, TP))
            {
                return false;
            }
            //パラメータ取得
            int mlv = YzWrapper.GetLevel(CC, this);
            int pow = YzWrapper.GetPower(CC, this);
            //Element ele = YzWrapper.GetElement(this, pow);
            //この辺からメイン処理
            //ダメージ出力やエフェクトは適宜入れ替える
            //
            //Debug.Log("2");
            //Debug.Log($"CC:{CC?.Name},TC:{TC?.Name},TP:{TP}");
            int aPen = 0;
            float mult = 1f;
            string playEff = "Goblin1";
            string playSe = "Material/meat_dead";
            //緩和条件
            if (TC != CC || (TC==CC && TP.Equals(CC.pos))  )
            {
                //Debug.Log("3");
                if (ModConfig.GpLimit.Value)
                {
                    if (TC.Chara.LV > CC.LV / 2 && TC.Chara.LV < CC.LV * 2)
                    {
                        aPen = 100;
                        mult = 2f;
                        playEff = "Goblin2";
                        playSe = "whip";
                    }
                }
                //原作寄り
                else
                {
                    if (TC.Chara.LV == CC.LV)
                    {
                        aPen = 100;
                        mult = 8f;
                        playEff = "Goblin2";
                        playSe = "whip";
                    }
                }
                //Debug.Log("4");
                YzWrapper.AddDamage(this, CC, TC.Chara, pow, Element.Void, AS: AttackSource.Melee, penetration: aPen, damageMult: mult);
            }
            //Debug.Log("5");
            //エフェクト再生
            YzUtil.PlayEffect(playEff, CC, TP: TP.Copy());
            //
            for (int i = 0; i < 4; i++)
            {
                YzUtil.DelayPlaySound(playSe, CC, 8 * i, 1f);
            }
            //
            return true;
        }
    }
}