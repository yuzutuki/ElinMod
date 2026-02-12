using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
	internal class SpYzFrostbomb : Spell
	{
		public override bool Perform()
		{
			//詠唱エフェクト
			CastEffect.Call(CC);
			//対象不明なら終了
			if (TC == null || TC.Chara == null)
			{
				return true;
            }//
            //todo:ActRefまわりを調べる。成形する
            string aliE = base.source.aliasRef.IsEmpty(Act.CC.MainElement.source.alias);
            Debug.Log($"EleAlias:{aliE}");
            ActRef ar = new ActRef
            {
                n1 = base.source.proc.TryGet(1, true),
                aliasEle = aliE,
                act = act
            };
            Debug.Log($"actRef_n1:{ar.n1}");
            CoroutineRunner.Instance.StartCoroutine(DotTest(ar));
            //
            //
            CC.PlaySound("identify", 1f, true);
			//
			return true;
        }        //
        //コルーチンテスト用
        IEnumerator DotTest(ActRef AR)
        {
            float interval = 1f;
            Point center = TP.Copy();
            List<Point> tp = new List<Point>();
            tp = YzWrapper.GetCirclePoint(center, 2);
            //Element e = Element.Create("eleCold", 100 / 10);
            Element e = Element.Create(AR.aliasEle.IsEmpty("eleFire"), 1);
            EffectId eid = EffectId.Ball;
            debug.Log($"eid:{eid}");
            for (int i = 1; i < 10; i++)
            {
                if (i == 3 || i == 6)
                {
                    interval *= 0.5f;
                }
                Debug.Log($"tick,tack:{i}");
                CC.PlaySound("trap", 0.7f, true);
                ActEffect.DamageEle(CC, eid, 1, e, tp, AR);
                yield return new WaitForSeconds(interval);
            }
            Debug.Log($"boomb");
            CC.PlaySound("spell_ball", 1f, true);
            tp = YzWrapper.GetCirclePoint(center, 4);
            ActEffect.DamageEle(CC, eid, 100, e, tp, AR);
        }
    }
    //
}