using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    //繰り返し使いそうな本体処理を呼び出す用クラス
    public class YzWrapper
    {
        //カーブ関数
        static public int Curve(long L, int start, int step, int rate = 75)
            => EClass.curve(L, start, step, rate);
        //円形
        static public List<Point> GetCirclePoint(Point center, float radius, bool mustBeWalkable = true, bool los = true)
            => EClass._map.ListPointsInCircle(center, radius, mustBeWalkable, los);
        //直線
        static public List<Point> GetLinePoint(Point center, Point to, int radius)
            => EClass._map.ListPointsInLine(center, to, radius);
        //放射状
        static public List<Point> GetArcPoint(Point center, Point to, int radius, int angle)
            => EClass._map.ListPointsInArc(center, to, radius, angle);
        //キャラ移動
        static public void MoveByForce(Chara target, Point newPoint, Card c = null, bool checkWall = false)
            => target.MoveByForce(newPoint, c, checkWall);
        //パワー取得
        static public int GetPower(Chara CC, Act act)
        {
            int id = act.source.id;
            return GetPower(CC, id);
        }
        static public int GetPower(Chara CC, int id)
        {
            int power = CC.elements.GetOrCreateElement(id).GetPower(CC);
            //Debug.Log($"power:{power}");
            return power;
        }
        //レベル取得
        static public int GetLevel(Chara CC, Act act)
        {
            int id = act.source.id;
            return GetLevel(CC, id);
        }
        static public int GetLevel(Chara CC, int id)
        {
            int level = CC.elements?.Value(id) ?? 0;
            //Debug.Log($"{id} Level:{level}");
            return level;
        }
        //属性値取得
        static public Element GetElement(Act act, int pow)
        {
            if (act.source.aliasRef ==""|| act.source.aliasRef == null)
            {
                return Element.Create(926, pow / 10);
            }
            else
            {
                return Element.Create(act.source.aliasRef, pow / 10);
            }
        }
        static public Element GetElement(string eleAlias, int pow)
        {
            Element ele = Element.Create(eleAlias, pow / 10);
            return ele;
        }
        //敵対行動判定
        static public void DoHostileAction(Card caster, Card target, bool immediate)
        {
            caster.Chara.DoHostileAction(target, immediate);
        }
        //バニラのエフェクト簡易再生
        static public void PlayVanillaEffect(Element EL, Point TP, float delay = 0f)
        {
            string id;
            Point tp = TP.Copy();
            if (EL.source.alias.Contains("ele"))
            {
                id = "Element/ball_" + EL.source.alias.Remove(0, 3);
            }
            else
            {
                Debug.Log($"-=-=-PlayVanillaEffect:指定された属性が不正:{EL.Name},{EL.id},{EL.source.alias}-=-=-");
                id = "Element/ball_Holy";
            }
            PlayVanillaEffect(id, tp, delay);
        }
        static public void PlayVanillaEffect(string ID, Point TP, float delay = 0f)
        {
            Effect eff = new Effect();
            eff = Effect.Get(ID);
            eff.Play(delay, TP, to: TP);
        }
        //ダメージ処理
        static public void AddDamage(Spell SP, Chara CC, Chara TC, int power, Element EL, int eleMod = 0, AttackSource AS = AttackSource.None, float damageMult = 1, bool HostileAction = true, int penetration = 0,bool noFF = false,bool isFlat = false)
        {
            if (TC == null)
            {
                return;
            }
            //例外
            if (TC.isDead || TC.isDestroyed)
            {
                return;
            }
            //同士討ち回避
            if (noFF && YzUtil.AvoidFriendlyFire(CC, TC))
            {
                //自傷許可?
                if (!CC.IsPC||CC.uid!= TC.uid || ModConfig.SelfCastLock.Value)
                {
                    return;
                }
            }
            //名前取得
            string eAlias = SP.source.alias;
            //計算式取得
            string calcId = EClass.sources.calc.map.ContainsKey(eAlias) ? eAlias : "arrow_";
            //ダイスの作成とロール
            long baseDamage = 0;
            int eleP = (SP != null) ? SP.ElementPowerMod : 50;
            //isFlatがtrueの場合、ダイスロールなしでpowerをそのままダメージにする
            if (!isFlat)
            {
                //Act(Spell)を渡した場合、powerは渡した値ではなくElementから再取得されることに注意?
                Dice dice = Dice.Create(calcId, power, CC, SP);
                baseDamage = dice.Roll();
                //属性値の取得と補正?
                eleP += eleMod;
            }
            else
            {
                baseDamage = power;
            }
            /*
            Debug.Log($"====eAli:{eAlias},calID:{calcId}");
            Debug.Log($"=sourceID:{SP.act.id},{SP.id}");
            Debug.Log($"=pow:{power}");
            Debug.Log($"=Dice:{dice.num},{dice.sides},{dice.bonus},{dice.RollMax()}");
            Debug.Log($"=baseDamage:{baseDamage}");
            */
            //物理ダメージ用処理
            if (AS == AttackSource.Melee)
            {
                baseDamage = PhysicsDamage(TC, baseDamage, penetration);
            }
            //ダメージ倍率適用
            baseDamage = (long)(baseDamage * damageMult);
            //ダメージ出力
            TC.DamageHP((int)baseDamage, EL.id, power * eleP / 100, AS, CC, true, null, null);
            //敵対行動処理
            if (!TC.isDead)
            {
                YzWrapper.DoHostileAction(CC, TC, true);
            }

        }
        //シンプルにダメージだけ出力版
        static public void AddDamage(string calcID, Chara TC, int power, int eleID = 0, AttackSource AS = AttackSource.None, float damageMult = 1, bool HostileAction = true, int penetration = 0)
        {
            if (TC == null)
            {
                return;
            }
            //例外
            if (TC.isDead || TC.isDestroyed)
            {
                return;
            }
            //
            long baseDamage = 0;
            //計算式(calcID)nullの場合はpowerをそのままダメージにする
            if (calcID == null || calcID == "")
            {
                baseDamage = power;
            }
            else
            {
                //ダイスの作成とロール
                Dice dice = Dice.Create(calcID, power);
                baseDamage = dice.Roll();
            }
            Debug.Log($"=baseDamage:{baseDamage}");
            //物理ダメージ用処理
            if (AS == AttackSource.Melee)
            {
                baseDamage = PhysicsDamage(TC, baseDamage, penetration);
            }
            //ダメージ倍率適用
            baseDamage = (long)(baseDamage * damageMult);
            //ダメージ出力
            TC.DamageHP(baseDamage, eleID);
        }
        //座標にダメージ出力
        static public void AddPointDamage(Point tp, Spell SP, Chara CC, int power, Element EL, int eleMod = 0, AttackSource AS = AttackSource.None, float damageMult = 1, bool HostileAction = true, int penetration = 0, bool noFF = true)
        {
            var charas = tp.ListCharas().ToArray();
            foreach (Chara c in charas)
            {
                if (!noFF || (noFF && !YzUtil.AvoidFriendlyFire(CC, c)))
                {
                    AddDamage(SP, CC, c, power, EL, eleMod, AS, damageMult, HostileAction, penetration);
                }
            }
        }
        //座標にシンプルダメージ
        static public void AddPointDamage(Point tp, string calcID, int power, int eleID = 0, AttackSource AS = AttackSource.None, float damageMult = 1, bool HostileAction = true, int penetration = 0, bool noFF = true)
        {
            var charas = tp.ListCharas().ToArray();
            foreach (Chara c in charas)
            {
                if (!noFF || (noFF && !YzUtil.AvoidFriendlyFire(null, c)))
                {
                    AddDamage(calcID, c, power, eleID, AS, damageMult, HostileAction, penetration);
                }
            }

        }
        //物理ダメージ(軽減と貫通)処理
        static public long PhysicsDamage(Chara TC, long baseDamage, int penetration, Chara CC = null)
        {
            //軽減処理
            long penDmg = baseDamage * (long)penetration / 100L;
            baseDamage -= penDmg;
            baseDamage = TC.ApplyProtection(baseDamage, 100) + penDmg;
            //
            //Debug.Log($"base:{baseDamage},pen:{penetration}%,{penDmg}");
            //
            if (CC != null)
            {
                if (CC.IsPC)
                    CC.Say("attackMelee", CC, TC, Lang.GetList("attackSlash")[0], null);
                else
                    CC.Say("attackMelee", CC, TC, Lang.GetList("attackSlash")[4], null);
            }
            //
            return baseDamage;
        }
    }
}