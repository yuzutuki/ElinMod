using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Reflection;
using Cwl.Helper.Unity;


namespace yz_sss
{
    public enum EffType
    {
        self,
        follow,
        arrow,
        bolt,
        point,
        hook,
        stretch,
        accelerate,
        accR,
        homing
    }
    static public class YzUtil
    {

        //エフェクトの再生
        public static void PlayEffect(string key, Card CC, Card TC = null ,Point TP = null,string replaceAffterEffect = null)
        {
            // エフェクトのPrefab取得
            if (!YzSSS.objDic.TryGetValue(key, out GameObject prefab))
            {
                Debug.Log($"--MissingEffect?--: {key}");
                return;
            }

            // エフェクト設定の取得
            if (!YzSSS.effDic.TryGetValue(key, out EffData data))
            {
                Debug.Log("エフェクトデータの取得に失敗");
                return;
            }

            // 基準位置を計算
            Vector3 setPos = new Vector3();
            switch (data.type)
            {
                case EffType.hook:
                    setPos = CharaCenter(TC);
                    break;
                case EffType.accR:
                    if(TP != null)
                       setPos = TP.PositionCenter();
                    else
                        setPos = CharaCenter(TC);
                    break;
                case EffType.point:
                    if (TP != null)
                        setPos = TP.PositionCenter();
                    else
                        setPos = CharaCenter(TC);
                    break;
                default:
                    setPos = CharaCenter(CC);
                    break;
            }
            //Debug.Log($"checkVec: c:{CharaCenter(CC)},t:{TP?.PositionCenter()}");
            // インスタンス生成
            GameObject obj = UnityEngine.Object.Instantiate(prefab, setPos, Quaternion.Euler(0,0,0));
            // エフェクト制御コンポーネントの設定
            if (obj.TryGetComponent<EffectObjectController>(out var oc))
            {
                oc.cc = CC;
                oc.tc = TC;
                oc.tp = TP?.Copy();
                oc.ed = data.Clone();
            }
            else
            {
                Debug.Log($"制御スクリプト取得失敗？: {obj.name}");
            }
            if (replaceAffterEffect != null)
            {
                oc.ed.afterEffect = replaceAffterEffect;
            }

            //Debug.Log($"PlayEffect: {obj.name}");
        }
        //基準点取得用
        static public Vector3 CharaCenter(Card c)
        {
            Vector3 v3 = new Vector3();
            //Debug.Log($"chara:{c.Name},{c.isThing}");
            //Debug.Log($"chara:{c?.Chara?.Name}");
            //Debug.Log($"charaSource:{c?.Chara?.source?.idRenderData}");
            if (c == null)
            {
                Debug.Log("--MissingCard?--");
                return (v3);
            }
            if (c.renderer.PositionCenter() == null ||
                c.renderer.PositionCenter() == default ||
                !c.isChara ||
                !c.isSynced)
            {
                v3 = c.pos.PositionCenter() + new Vector3(0.05f, 0.35f, 0);
            }
            else if (c.Chara.source.idRenderData == "@chara" ||
                     c.Chara.source.idRenderData == "@chara_L" ||
                     c.Chara.source.idRenderData == "@chara_LL")
            {
                v3 = c.pos.PositionCenter() + new Vector3(0.05f, 0.35f, 0);
            }
            else if (c.Chara.source.idRenderData == "chara_L" ||
                     c.Chara.source.idRenderData == "chara_LL" ||
                     c.Chara.source.idRenderData == "chara_LW")
            {
                v3 = c.renderer.PositionCenter() + new Vector3(0, -0.8f, 0);
            }
            else
            {
                v3 = c.renderer.PositionCenter();
            }
            v3.z -= 20;
            return (v3);
        }
        //PointAからBへの隣接マス取得
        //隣接マス取得用
        static Point[] angleToPoint =
        {
            new Point(1,0),
            new Point(1,1),
            new Point(0,1),
            new Point(-1,1),
            new Point(-1,0),
            new Point(-1,-1),
            new Point(0,-1),
            new Point(1,-1)
        };
        static public Point GetStepTowards(Point CP,Point TP)
        {
            Vector2 direction = new Vector2(TP.x - CP.x, TP.z - CP.z);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float snap = Mathf.Round(angle / 45f) * 45;
            if (snap < 0) snap += 360f;
            int index = Mathf.RoundToInt(snap / 45f) % 8;
            return new Point(CP.x + angleToPoint[index].x, CP.z + angleToPoint[index].z);
         }
        //
        //effDataからTypeの取得
        static public EffType ToType(string name)
        {
            if (Enum.TryParse<EffType>(name,ignoreCase:true, out EffType type))
            {
                return type;
            }
            else
            {
                Debug.Log($"EffType名が不正:{name}");
                return EffType.self;
            }
        }
        //
        //二点から角度
        static public Quaternion LookTarget(Vector2 o, Vector2 t)
        {
            //念のためnull処理
            if (t == null || o == null)
            {
                Debug.Log($"--角度取得処理に失敗--");
                return (new Quaternion());
            }
            //
            Vector2 direction = t - o;
            float r = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return (Quaternion.Euler(0, 0, r));
        }
        //
        //PointからVector2
        static public Vector2 PointToVector2(Point o)
        {
            Point p = o.Copy();
            //return p.PositionCenter();
            return p.Position(0);
        }
        //
        //PointからVector3
        static public Vector3 PointToVector3(Point o,float z = 0)
        {
            Point p = o.Copy();
            Vector3 v = p.Position(0);
            if(z!=0) v.z = z;
            return v;
        }
        //Vectorからポイント
        static public Point VectorToPoint(Vector3 v)
        {
            Point p = new Point();
            int xx = Mathf.RoundToInt(v.x / Point._screen.tileAlign.x);
            int yy = Mathf.RoundToInt(v.y / Point._screen.tileAlign.y);
            p.x = (yy - xx) / 2 * -1;
            p.z = xx - p.x;
            return p;
        }
        //actRefの確認
        static public void CheckActRef(ActRef ar)
        {
            Debug.Log($"-== actRefの情報表示 ==-");
            Debug.Log($"act.Name:{ar.act?.Name ?? "is null"}");
            Debug.Log($"aliasEle:{ar.aliasEle ?? "is null"}");
            //Debug.Log($"idEle:{ar.idEle}");
            Debug.Log($"n1:{ar.n1 ?? "null"}");
            Debug.Log($"refThing.name:{ar.refThing?.Name ?? "is null"}");
            Debug.Log($"origin.name:{ar.origin?.Name ?? "is null"}");
            Debug.Log($"isPerfume:{ar.isPerfume}");
            Debug.Log($"noFriendlyFire:{ar.noFriendlyFire}");

        }
        //Actの確認
        static public void CheckAct(Act a)
        {

        }
        //ミニオン召喚
        public enum LevelAdjust
        {
            Same,
            Average,
            Power,
            Static
        }
        static public bool SummonMinion(string summonID, Chara caster, Point _tp, int radius = 1,LevelAdjust adjust = LevelAdjust.Same,int power = 100,bool uniqueSummon = false,int lifeTime=-1)
        {
            Point tp = _tp.Copy();
            Chara minion;
            //
            //呼び出し数チェック
            caster.Say("summon_ally", caster, null, null);
            if (EClass._zone.CountMinions(caster) >= caster.MaxSummon || caster.c_uidMaster != 0)
            {
                caster.Say("summon_ally_fail", caster, null, null);
                return false;
            }
            //重複チェック
            if (uniqueSummon)
            {
                foreach (Chara chara in EClass._map.charas)
                {
                    if (chara.IsPCFactionMinion && !chara.IsEscorted())
                    {
                        if (chara.id == summonID)
                        {
                            caster.Say("yz_UniqueSummonFail");
                            return false;
                        }
                    }
                }
            }
            //召喚座標
            Point randomPoint = tp.GetRandomPoint(radius, true, true, false, 100);
            Point point = (randomPoint != null) ? randomPoint.GetNearestPoint(false, false, true, false, 0) : null;
            //
            if (point == null || !point.IsValid)
            {
                caster.SayNothingHappans();
                return false;
            }
            //
            CardBlueprint.Set(new CardBlueprint());
            if (caster.LV != -1)
            {
                CardBlueprint.current.lv = caster.LV;
            }
            minion = CharaGen.Create(summonID, -1);
            //レベル調整
            int minionLv = 1;
            switch (adjust)
            {
                case (LevelAdjust.Same):
                    minionLv = caster.LV;
                    break;
                case (LevelAdjust.Static):
                    minionLv = minion.LV;
                    break;
                case (LevelAdjust.Power):
                    minionLv = power / 10;
                    minionLv.Clamp(1, caster.LV * 2);
                    break;
                case (LevelAdjust.Average):
                    minionLv = ((power / 10)+caster.LV)/2;
                    minionLv.Clamp(1, caster.LV * 4);
                    break;
            }
            //
            minion.interest = 0;
            minion.SetLv(minionLv);
            //
            EClass._zone.AddCard(minion, point);
            minion.MakeMinion(caster, MinionType.Default);
            if (lifeTime != -1)
            {
                minion.SetSummon(lifeTime);
            }
            //
            YzUtil.PlayEffect("Summon1", minion, minion, point);
            //
            return true;
        }
            //
        //デバフ付与判定、失敗でfalse
        static public bool AddDebuff(string conName,int power,Chara CC,Chara TC,bool forced=false)
        {
            if (TC==null || TC.isDead)
            {
                return false;
            }
            CC.DoHostileAction(TC, false);//敵対行動
            bool isPowerful = TC.IsPowerful;//強敵判定
            int power2 = power;
            int resist = TC.WIL * (isPowerful ? 20 : 5);
            //ホーリーヴェイルの有無
            ConHolyVeil HolyVail = TC.GetCondition<ConHolyVeil>();
            if (HolyVail != null)
            {
                resist += HolyVail.power * 5;
            }
            //抵抗処理
            if (!forced && EClass.rnd(power2) < resist && EClass.rnd(10) != 0)
            {
                TC.Say("debuff_resist", TC, null, null);
                CC.DoHostileAction(TC, false);
                return false;
            }
            TC.AddCondition(conName, power2, forced);
            CC.DoHostileAction(TC, false);
            //ウェイト処理
            if (EClass.core.config.game.waitOnDebuff && !CC.IsPC)
            {
                EClass.Wait(0.3f, TC);
                return (true);
            }
            return true;
        }
        //
        //キャラクター押しのけ
        static public void PushChara(Chara main,Point p)
        {
            List<Chara> cl = p.ListCharas();
            if (cl.Count > 1)
            {
                foreach (Chara c in cl)
                {
                    if (c.uid != main.uid)
                    {
                        Point subPoint = p.Copy();
                        subPoint = subPoint.GetNearestPoint(false, false, true, true, 0);
                        YzWrapper.MoveByForce(c, subPoint);
                    }
                }
            }
        }
        //
        //吸血鬼チェック
        static public bool CheckVamp(Chara chara)
        {
            if(chara.race.id == "vampire" ||
                chara.race.id == "succubus" || 
                chara.HasElement(1216) ||
                chara.HasElement(1250))
                return true;
            return false;
        }
        //巻き込みチェック
        //複数座標判定
        static public List<Chara> AvoidFriendlyFire(Card caster,List<Point> tps)
        {
            List<Chara> tcl = new List<Chara>();
            foreach (Point p in tps)
            {
                tcl = tcl.Union(p.ListCharas()).ToList();
            }
            /*
            foreach (Chara c in tcl)
            {
                Debug.Log(c.Name);
            }
            Debug.Log("avoidFF..");
            */
            tcl.RemoveAll(target => YzUtil.AvoidFriendlyFire(caster, target));
            /*
            foreach (Chara c in tcl)
            {
                Debug.Log(c.Name);
            }
            */
            return tcl;

        }
        //単体判定
        static public bool AvoidFriendlyFire(Card caster,Card target)
        {
            Card o = caster;
            Card t = target;
            //例外:ターゲット不明、対象がキャラではない
            if (t == null || !t.isChara || !o.isChara)
            {
                return false;
            }
            //詠唱者除外
            if (o.Chara.uid == t.Chara.uid)
            {
                return true;
            }
            //リトルシスター除外
            if(t.Chara.id == "littleOne")
            {
                return true;
            }
            //軽減対象かどうか
            if (o.Chara.IsFriendOrAbove(t.Chara))
            {
                //302=魔力制御
                int accuracy = o.Evalue(302);
                if (!o.IsPC && o.IsPCFactionOrMinion)
                {
                    accuracy += EClass.pc.Evalue(302);
                }
                //精度フィート確認
                if (o.HasElement(1214, 1))
                {
                    accuracy *= 2;
                }
                //なんやかんやから閾値設定する場所
                //とりあえずは1以上なら(魔力制御持ってるなら)回避成功。
                if (accuracy > 0)
                {
                    return true;
                }
                //
            }
            return false;
        }
        //直線座標取得
        static public List<Point> LinePoints(Point origin,Point target,float range,bool ignoreOrigin = true)
        {
            List<Point> targetPoints = new List<Point>();
            Vector2 ov = PointToVector2(origin.Copy());
            Vector2 tv = PointToVector2(target.Copy());            
            Vector2 dir = (tv - ov);
            dir.Normalize();
            float angleDeg = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
            float a = Mathf.Abs(Mathf.DeltaAngle(angleDeg, 0f));
            if (a > 90f) a = 180 - a;
            float tnorm = a / 90f;
            //
            float scale = Mathf.Lerp(1.2f, 0.6f, tnorm);
            dir = dir * scale;
            //
            int steps = Mathf.CeilToInt(range) * 4;
            Vector2 current = ov;
            for (int i = 0; i < steps; i++)
            {
                current += (dir*0.25f);
                Point add = VectorToPoint(current);
                //Debug.Log($"dir:{dir},current:{current},addPoint:{add}");
                if (!add.IsValid)
                    break;
                if (targetPoints.Contains(add))
                    continue;
                if(ignoreOrigin && target == origin)
                    continue;
                targetPoints.Add(add.Copy());
            }
            return targetPoints;
        }
        //最も遠いキャラ
        static public Chara GetFarthestChara(Point origin,List<Chara> charas)
        {
            return charas.OrderByDescending(c =>
            {
                Point p = c.pos.Copy();
                int dx = p.x - origin.x;
                int dz = p.z - origin.z;
                return dx * dx + dz * dz;
            }).FirstOrDefault();
        }
        //最も近いキャラ
        static public Chara GetNearestChara(Point origin, List<Chara> charas)
        {
            return charas.OrderBy(c =>
            {
                Point p = c.pos.Copy();
                int dx = p.x - origin.x;
                int dz = p.z - origin.z;
                return dx * dx + dz * dz;
            }).FirstOrDefault();
        }
        //float距離ソート
        //int距離はPoint.Distanceを使う
        static public List<Point> DistanceSort(Point origin,List<Point> points)
        {
            Point o = origin.Copy();
            Vector2 ov = new Vector2(o.x, o.z);
            List<Point> result = points;
            result.Sort((a, b) => {
                Vector2 av = new Vector2(a.x, a.z);
                Vector2 bv = new Vector2(b.x, b.z);
                float distA = (av - ov).sqrMagnitude;
                float distB = (bv - ov).sqrMagnitude;
                return distA.CompareTo(distB);
                //(Mathf.Abs(a.x) + Mathf.Abs(a.z)).CompareTo(Mathf.Abs(b.x) + Mathf.Abs(b.z));
            });
            return result;
        }
        static public List<Chara> DistanceSort(Point origin, List<Chara> charas)
        {
            Point o = origin.Copy();
            Vector2 ov = new Vector2(o.x, o.z);
            List<Chara> result = charas;
            result.Sort((a, b) => {
                Vector2 av = new Vector2(a.pos.x, a.pos.z);
                Vector2 bv = new Vector2(b.pos.x, b.pos.z);
                float distA = (av - ov).sqrMagnitude;
                float distB = (bv - ov).sqrMagnitude;
                return distA.CompareTo(distB);
                //(Mathf.Abs(a.x) + Mathf.Abs(a.z)).CompareTo(Mathf.Abs(b.x) + Mathf.Abs(b.z));
            });
            return result;
        }
        //効果範囲表示
        static public void ShowEffectiveArea(List<Point> points,bool delay = false)
        {
            int count = 0;
            foreach (Point p in points)
            {
                CoroutineHelper.Deferred(
                    () =>
                    {
                        Effect vEff = Effect.Get("effectiveArea");
                        vEff.posFix.z += 2;
                        vEff.duration = 0.3f;
                        vEff.Play(p.Copy());
                    },
                    count);
                if(delay) count++;
            }

        }
        //単体Point用
        static public void ShowEffectiveArea(Point point, int delay = 0)
        {
                CoroutineHelper.Deferred(
                    () =>
                    {
                        Effect vEff = Effect.Get("effectiveArea");
                        vEff.posFix.z += 2;
                        vEff.duration = 0.3f;
                        vEff.Play(point.Copy());
                    },
                    delay);
        }
        //CWLのコルーチンを使った遅延再生、Delayはフレーム数
        //効果音遅延再生
        static public void DelayPlaySound(string id, Card origin, int delay, float v = 1f,bool spatial = true)
        {
            Zone calledZone = EClass._zone;
            CoroutineHelper.Deferred(
                () => {
                    if (calledZone != EClass._zone)return;
                    origin.PlaySound(id, v, spatial);
                    },
                delay
                );
        }
        //エフェクト遅延再生
        static public void DelayPlayEffect(int delay, string id, Card CC,Card TC,Point TP=null, string AffterEffect = null)
        {
            Zone calledZone = EClass._zone;
            CoroutineHelper.Deferred(
                () => {
                    if (calledZone != EClass._zone)return;
                    YzUtil.PlayEffect(id, CC, TC, TP.Copy(),AffterEffect);
                },
                delay
                );
        }
        //バニラのエフェクト遅延再生(デフォのDelayと別)
        static public void DelayVanilaEffect(int delay,Element EL,Point TP)
        {
            Zone calledZone = EClass._zone;
            CoroutineHelper.Deferred(
                () => {
                    if (calledZone != EClass._zone) return;
                    YzWrapper.PlayVanillaEffect(EL, TP);
                },
                delay
                );
        }
        static public void DelayVanilaEffect(int delay, String ID, Point TP)
        {
            Zone calledZone = EClass._zone;
            CoroutineHelper.Deferred(
                () => {
                    if (calledZone != EClass._zone) return;
                    YzWrapper.PlayVanillaEffect(ID, TP);
                },
                delay
                );
        }
        //ダメージ遅延発生
        static public void DelayAddDamage(int delay, Spell SP, Chara CC, Chara TC, int power, Element EL, int eleMod = 0, AttackSource AS = AttackSource.None, float damageMult = 1)
        {
            Zone calledZone = EClass._zone;
            CoroutineHelper.Deferred(
                () =>
                {
                    if (calledZone != EClass._zone) return;
                    YzWrapper.AddDamage(SP, CC, TC, power, EL, eleMod, AS,damageMult);
                }, delay
                );
            }
        }
    //テストコルーチン実行管理用
    //CWLの機能を使うので多分未使用
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner ins;
        public static CoroutineRunner Instance
        {
            get
            {
                if (ins == null)
                {
                    var go = new GameObject("CoroutineRunner");
                    ins = go.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(go);
                }
                return ins;
            }
        }
    }
}

