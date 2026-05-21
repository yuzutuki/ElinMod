using Cwl.Helper;
using Cwl.Helper.Unity;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using UnityEngine;

//
//26/05/18 SSSから移植
//不要なメソッドは削除

namespace yz_ReSS
{
    //エフェクトの移動などのタイプ
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
        homing,
        parabola,
        flat
    }
    static public class YzUtil
    {

        //エフェクトの再生
        public static void PlayEffect(string key, Card CC, Card TC = null, Point fromP = null, Point TP = null, string replaceAffterEffect = null)
        {
            // エフェクトのPrefab取得
            if (!YzReSS.objDic.TryGetValue(key, out GameObject prefab))
            {
                Debug.Log($"--MissingEffect?--: {key}");
                return;
            }

            // エフェクト設定の取得
            if (!YzReSS.effDic.TryGetValue(key, out EffData data))
            {
                Debug.Log("エフェクトデータの取得に失敗");
                return;
            }
            /*
            if (TC == null || TC.isDestroyed)
            {
                Debug.Log($"ターゲットnull or destroy:{key}");
                return;
            }
            */

            // 基準位置を計算
            Vector3 setPos = new Vector3();
            switch (data.type)
            {
                case EffType.hook:
                    setPos = CharaCenter(TC);
                    break;
                case EffType.accR:
                    if (TP != null)
                        setPos = CharaCenter(TP);
                    else
                        setPos = CharaCenter(TC);
                    break;
                case EffType.point:
                    if (TP != null)
                        setPos = CharaCenter(TP);
                    else
                        if (TC != null)
                        {
                            setPos = CharaCenter(TC);
                        }
                        else
                        {
                            setPos = CharaCenter(CC);
                        }
                    break;
                default:
                    if (fromP == null)
                    {
                        setPos = CharaCenter(CC);
                    }
                    else
                    {
                        setPos = CharaCenter(fromP);
                    }
                    break;
            }
            //Debug.Log($"checkVec: c:{CharaCenter(CC)},t:{TP?.PositionCenter()}");
            // インスタンス生成
            GameObject obj = UnityEngine.Object.Instantiate(prefab, setPos, Quaternion.Euler(0, 0, 0));
            // エフェクト制御コンポーネントの設定
            if (obj.TryGetComponent<EffectObjectController>(out var oc))
            {
                oc.cc = CC;
                oc.tc = TC;
                oc.tp = TP?.Copy();
                oc.fromP = fromP?.Copy();
                oc.ed = data.Clone();
            }
            else
            {
                Debug.Log($"制御スクリプト取得失敗？: {obj.name}");
            }
            //追加エフェクト
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
                Debug.Log("--CharaCenter:MissingCard?--");
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
            v3.z -= 50;
            return (v3);
        }
        static public Vector3 CharaCenter(Point p)
        {
            Vector3 v3 = new Vector3();
            if (p != null)
            {
                v3 = p.Copy().PositionCenter();
                v3 += new Vector3(0.05f, 0.35f, -150f);
            }
            return v3;

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
        static public Point GetStepTowards(Point CP, Point TP)
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
            if (Enum.TryParse<EffType>(name, ignoreCase: true, out EffType type))
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
        static public Vector3 PointToVector3(Point o, float z = 0)
        {
            Point p = o.Copy();
            Vector3 v = p.Position(0);
            if (z != 0) v.z = z;
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
        //直線座標取得
        static public List<Point> LinePoints(Point origin, Point target, float range, bool ignoreOrigin = true)
        {
            List<Point> targetPoints = new List<Point>();
            Vector2 ov = PointToVector2(origin.Copy());
            Vector2 tv = PointToVector2(target.Copy());
            Vector2 dir = (tv - ov);
            dir.Normalize();
            float angleDeg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
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
                current += (dir * 0.25f);
                Point add = VectorToPoint(current);
                //Debug.Log($"dir:{dir},current:{current},addPoint:{add}");
                if (!add.IsValid)
                    break;
                if (targetPoints.Contains(add))
                    continue;
                if (ignoreOrigin && target == origin)
                    continue;
                targetPoints.Add(add.Copy());
            }
            return targetPoints;
        }
        //最も遠いキャラ
        static public Chara GetFarthestChara(Point origin, List<Chara> charas)
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
        static public List<Point> DistanceSort(Point origin, List<Point> points)
        {
            Point o = origin.Copy();
            Vector2 ov = new Vector2(o.x, o.z);
            List<Point> result = points;
            result.Sort((a, b) =>
            {
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
            result.Sort((a, b) =>
            {
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
        static public void ShowEffectiveArea(List<Point> points, bool delay = false)
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
                if (delay) count++;
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
        //汎用
        static public void DelayAction(Action action, int delay)
        {
            Zone calledZone = EClass._zone;
            CoroutineHelper.Deferred(
                () =>
                {
                    if (EClass.scene.mode != Scene.Mode.Zone)
                        return;
                    if (EClass._zone == null || EClass._zone != calledZone)
                        return;
                    action.Invoke();
                },
                delay
                );
        }
        //効果音遅延再生
        static public void DelayPlaySound(string id, Card origin, int delay, float v = 1f, bool spatial = true)
        {
            DelayAction(() => origin.PlaySound(id, v, spatial), delay);
        }
        //エフェクト遅延再生
        static public void DelayPlayEffect(int delay, string id, Card CC, Card TC=null, Point TP = null, string AffterEffect = null)
        {
            DelayAction(() => YzUtil.PlayEffect(id, CC, TC, TP.Copy(), replaceAffterEffect: AffterEffect), delay);
        }
        /*
        //バニラのエフェクト遅延再生(デフォのDelayと別)
        static public void DelayVanilaEffect(int delay, Element EL, Point TP)
        {
            DelayAction(() => YzWrapper.PlayVanillaEffect(EL, TP.Copy()), delay);
        }
        static public void DelayVanilaEffect(int delay, String ID, Point TP)
        {
            DelayAction(() => YzWrapper.PlayVanillaEffect(ID, TP.Copy()), delay);
        }
        */
    }
    //
}

