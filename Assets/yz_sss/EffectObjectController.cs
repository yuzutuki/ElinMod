using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    //メインオブジェクトにアタッチするスクリプトなど
    //objFollwに相当
    //子の処理(effObjectManager)も統合する
    public class EffectObjectController : MonoBehaviour
    {
        //使ってない変数もいくつか
        public ExActionData actionData;//Actデータ
        public EffData ed;//エフェクトデータ
        public Card tc;//ターゲットのカード情報
        public Card cc;//キャスターカード
        public Point tp;//ターゲット位置
        public Point fromP;//起点上書き
        ParticleSystem ps;//パーティクル管理
        Animation anim;//アニメ管理
        SpriteRenderer sr;
        Transform tra;//自分の
        Vector3 pos;//自分の座標
        Vector3 pos2;//移動予定先
        Vector3 cPos;//
        Vector3 tPos;//
        Vector3 cPosF;//エフェクト読み込み時の座標
        Vector3 tPosF;//
        Color color;//色の補正
        Zone loadingZone;
        //    float alpha = 1;//パーティクル透明度の補正
        int lifetime = -1;//生存時間
        float speed;//速度
        float lerpCount = 0;
        int frameCount = 0;
        EffType moveType;//移動タイプ
        bool lookTarget;
        Vector2 deviation;
        Vector2 moveVelocity;//固定移動量
        Vector2 fixVelocity;//移動量の補正
        bool moveDone;
        bool isEmitStop;//パーティクルの放出を停止
        //
        //
        void Start()
        {
            //各種初期値取得設定
            //カード取得状況の確認
            if (cc == null)
            {
                //Debug.Log("--CC取得失敗--");
            }
            if (fromP == null)
            {
                cPos = YzUtil.CharaCenter(cc);
            }
            else
            {
                cPos = YzUtil.CharaCenter(fromP);
            }
            if (tc == null)
            {
                tc = cc;
                //Debug.Log("--TC取得失敗--");
            }
            if (tp == null)
            {
                tp = tc.pos.Copy();
            }
            //EffDataからのデータ取得
            moveType = ed.type;
            lookTarget = ed.lookTarget;
            color = ed.colorFix;
            color.a *= ed.alphaFix;
            speed = ed.speedFix;
            if (ed.lifeTime > 0)
            {
                lifetime = ed.lifeTime;
            }
            //コンポーネント取得
            tra = this.GetComponent<Transform>();
            ps = this.GetComponent<ParticleSystem>();
            anim = this.GetComponent<Animation>();
            sr = this.GetComponent<SpriteRenderer>();
            //各種コンポーネント
            if (ps != null)
            {
                var psm = ps.main;
                color = psm.startColor.color * color;
                psm.startColor = color;
                //Debug.Log("--isParticleSystem--");
            }
            if (sr != null)
            {
                sr.color = color;
                //Debug.Log("--isSpriteRender--");
            }
            if (anim != null)
            {
                //Debug.Log("--isAnimation--");

            }
            //読み込みゾーン取得
            loadingZone = EClass._zone;
            //
            //初期座標設定
            //Debug.Log($"{this.name}:EffType:{moveType}");
            switch (moveType)
            {
                case (EffType.point):
                    tPos = YzUtil.CharaCenter(tp);
                    pos = tPos;
                    //Debug.Log($"TP:[{tp}],tPos:[{tPos}]");
                    break;
                case (EffType.follow):
                    tPos = YzUtil.CharaCenter(tc);
                    pos = tPos;
                    break;
                case (EffType.hook):
                    if (tp != tc.pos.Copy())
                    {
                        tPos = YzUtil.CharaCenter(tp);
                    }
                    else
                    {
                        tPos = YzUtil.CharaCenter(tc);
                    }
                    pos = tPos;
                    break;
                case (EffType.bolt):
                case (EffType.accelerate):
                case (EffType.stretch):
                    pos = cPos;
                    tPos = YzUtil.CharaCenter(tp);
                    break;
                case (EffType.accR):
                    pos = YzUtil.CharaCenter(tp);
                    tPos = YzUtil.CharaCenter(cc);
                    break;
                case (EffType.arrow):
                case (EffType.self):
                default:
                    pos = cPos;
                    if (tp != null)
                    {
                        tPos = YzUtil.CharaCenter(tp);
                    }
                    else
                    {
                        tPos = YzUtil.CharaCenter(tc);
                    }
                    break;
            }
            tra.position = pos;
            pos2 = pos;
            //初期座標
            cPosF = cPos;
            tPosF = tPos;
            //ブレ処理
            deviation = new Vector2();
            if (ed.deviation)
            {
                deviation.x = Random.Range(-0.15f, 0.15f);
                deviation.y = Random.Range(-0.15f, 0.15f);
                tPos += (Vector3)deviation;
                if(moveType == EffType.point)
                {
                    pos += (Vector3)deviation;
                    tra.position = pos;
                }
            }
            //引き伸ばし処理
            if (moveType == EffType.stretch)
            {
                float distance = Vector2.Distance(cPos, tPos);
                if (ps != null)
                {
                    var psm = ps.main;
                    psm.startSizeXMultiplier = distance;
                }
                else
                {
                    tra.SetLocalScaleX(distance);
                }
                lookTarget = true;
            }
            //放物線計算用、固定移動量
            if (moveType == EffType.parabola)
            {
                float distance = Vector2.Distance(cPos, tPos);
                lifetime = Mathf.CeilToInt(distance / (0.8f * speed));
                //Debug.Log($"lifetime:{lifetime}");
                moveVelocity = (tPos - cPos) / lifetime;
            }
            //固定移動量(flat用)
            if (moveType == EffType.flat)
            {
                if (lifetime <= 0)
                {
                    lifetime = 60;
                }
                //lifetime = (int)speed;
                //moveVelocity = (tPos - cPos) / lifetime;
            }
            //向き変え処理
            if (lookTarget)
            {
                tra.rotation = YzUtil.LookTarget(cPos, tPos);
            }
            //ed.CheckEffectData();

        }
        void FixedUpdate()
        {
            //シーン遷移で強制破棄
            if (EClass.scene.mode != Scene.Mode.Zone)
            {
                Destroy(gameObject);
                return;
            }
            //破棄処理
            if (DestroyCheck())
            {
                return;
            }
            //
            if (tra == null)
            {
                Debug.Log($"Transform取得エラー:{this.gameObject.name}");
                return;
            }
            //現在座標取得
            pos = tra.position;
            switch (moveType)
            {
                //発動者定点処理
                case (EffType.self):
                    break;
                //アロー型(等速)移動処理
                case (EffType.arrow):
                    if (((Vector2)pos - (Vector2)tPos).sqrMagnitude >= 0.01f)
                    {
                        pos2 = Vector2.MoveTowards(pos, tPos, 0.8f * speed);
                    }
                    else
                    {
                        if (ed.arrowHold == false)
                        {
                            MoveDone();
                            StopEmit();
                        }
                    }
                    break;
                //放物線移動処理(ライフタイム依存、固定移動量)
                case (EffType.parabola):
                    fixVelocity.y = -(frameCount - lifetime) * 0.015f * speed;
                    //Debug.Log($"fixVector:{fixVelocity},frameCount:{frameCount}");
                    pos2 += (Vector3)(moveVelocity + fixVelocity);
                    break;
                //フラット移動処理(時間固定移動)
                case (EffType.flat):
                    if (tc != null && tc != cc)
                    {
                        if(!tc.Chara.isDead && !tc.isDestroyed)
                        {
                            tp= tc.pos.Copy();
                            tPos = YzUtil.CharaCenter(tp);
                        }
                    }
                    //pos2 += (Vector3)(moveVelocity);
                    pos2 = Vector2.Lerp(cPosF, tPos, Mathf.Clamp01(frameCount / (float)ed.lifeTime));
                    break;
                //追従型移動処理
                case (EffType.follow):
                    if (tc != null && tc.renderer != null)
                    {
                        tPos = YzUtil.CharaCenter(tc);
                        pos2 = Vector2.Lerp(pos, tPos, 0.2f * speed);
                    }
                    else
                    {
                        //Debug.Log($"{this.gameObject.name}:MissingTarget,stopEmitter");
                        StopEmit();
                    }
                    break;
                //加速接近処理
                case (EffType.accelerate):
                    tPos = tp.Copy().PositionCenter() + (Vector3)deviation;
                    pos2 = Vector2.Lerp(pos, tPos, lerpCount);
                    //lerpCount += 0.05f;
                    lerpCount = lerpCount + Mathf.Clamp(0.05f * lerpCount, 0.01f, 0.1f * speed);
                    lerpCount = Mathf.Min(lerpCount, 1);
                    //Debug.Log(lerpCount);
                    if (lerpCount >= 1)
                    {
                        StopEmit();
                    }
                    break;
                case (EffType.accR):
                    cPos = YzUtil.CharaCenter(cc);
                    pos2 = Vector2.Lerp(pos, cPos, lerpCount);
                    lerpCount = lerpCount + Mathf.Clamp(0.05f * lerpCount, 0.01f, 0.1f * speed);
                    lerpCount = Mathf.Min(lerpCount, 1);
                    //Debug.Log(lerpCount);
                    if (lerpCount >= 1)
                    {
                        StopEmit();
                    }
                    break;
                //ホーミング(位置更新＋加速矢)処理
                case (EffType.homing):
                    if (tc != null)
                    {
                        tPos = tc.pos.Copy().PositionCenter() + (Vector3)deviation;
                    }
                    if (((Vector2)pos - (Vector2)tPos).sqrMagnitude >= 0.01f)
                    {
                        pos2 = Vector2.MoveTowards(pos2, tPos, (speed * 0.05f * frameCount * frameCount) * Time.deltaTime);
                    }
                    else
                    {
                        if (ed.arrowHold == false)
                        {
                            MoveDone();
                            StopEmit();
                        }
                    }
                    break;
                //対象者定点処理
                case (EffType.point):
                    pos2 = pos;
                    break;
                //方向指定処理
                case (EffType.bolt):
                    pos2 = pos;
                    break;
                //フック系処理
                case (EffType.hook):
                    cPos = YzUtil.CharaCenter(cc);
                    pos2 = Vector2.Lerp(tPosF, cPos, lerpCount);
                    lerpCount = Mathf.Min(lerpCount + (0.1f * speed), 1);
                    if (lerpCount >= 1)
                    {
                        StopEmit();
                    }
                    break;
            }
            //座標の適用とZの補正
            tra.position = new Vector3(pos2.x, pos2.y, tra.position.z);
            //経過フレームカウント
            frameCount++;
        }
        //
        //オブジェクト破棄処理
        bool DestroyCheck()
        {
            //パーティクル処理
            if (ps != null)
            {
                //停止チェック
                if (!ps.isPlaying && !isEmitStop)
                {
                    StopEmit();
                }
                //パーティクル消滅チェック
                if (isEmitStop && !ps.IsAlive(true))
                {
                    //Debug.Log("Destroy:Particle");
                    Destroy(gameObject);
                    return (true);
                }
            }
            //生存時間処理
            if (lifetime > 0)
            {
                //Debug.Log($"lifeTime:{lifetime}");
                lifetime--;
                if (lifetime == 0)
                {
                    //Debug.Log("Destroy:lifetime");
                    Destroy(gameObject);
                    return true;
                }
            }
            //アニメ停止チェック
            if (anim != null)
            {
                if (!anim.isPlaying)
                {
                    //Debug.Log("Destroy:anime");
                    Destroy(gameObject);
                    return true;
                }
            }
            //ゾーン遷移チェック
            if (loadingZone == null || loadingZone != EClass._zone)
            {
                //Debug.Log("Destroy:zone");
                Destroy(gameObject);
                return true;
            }
            return false;
        }
        //
        //パーティクル放出停止処理
        void StopEmit()
        {
            if (isEmitStop) return;
            if (ps == null) return;
            isEmitStop = true;
            if (ps.isPlaying)
            {
                ps.Stop();
                if (lifetime == -1)
                {
                    lifetime = 30;
                }
                return;
            }
        }
        //
        //破棄時の自動呼び出し
        private void OnDestroy()
        {
            if (!moveDone)
            {
                MoveDone();
            }
        }
        //移動完了処理
        void MoveDone()
        {
            if (!moveDone)
            {
                moveDone = true;
                //スプライト透明化
                SpriteAlphaZero();
                //Debug.Log($"afterEffect:{ed.afterEffect}");
                //追加エフェクト呼び出し
                callAfterEffect();
                //着弾時特殊処理
                CallExAction();
            }
        }
        //追加アクション呼び出し
        private void CallExAction()
        {
            if (ed.exAction)
            {
                if (actionData.SP == null)
                {
                    Debug.Log("ExAction:missing");
                    return;
                }
                if (actionData.CastZone != EClass._zone)
                {
                    Debug.Log("ExAction:castZone mismatch");
                    return;
                }
                if (actionData.CC == null || actionData.CC.isDead)
                {
                    Debug.Log("ExAction:CC missing or dead");
                    return;
                }
                if (loadingZone == null || loadingZone != EClass._zone)
                {
                    Debug.Log("ExAction:loadingZone mismatch");
                    return;
                }
                //Debug.Log("ExAction実行");
                string objectName = this.gameObject.name.Replace("(Clone)", "").Trim();
                //Debug.Log($"objectNameA:{objectName}");
                /*
                objectName = this.gameObject.name;
                objectName = objectName.Replace("(Clone)", "");
                objectName = objectName.Trim();
                Debug.Log($"objectNameB:{objectName}");
                */
                //Debug.Log($"ExAction type:{actionData.a.GetType()}");
                //Debug.Log($"{actionData.a.ID},{actionData.a.Name},{actionData.a.FullName}{actionData.a.ShortName},{actionData.a.id}");
                //インターフェイスから実行
                if (actionData.SP is IExAction ie)
                    ie.ExAction(actionData);
                else
                    Debug.Log("ExAction type mismatch");
            }
        }

        //アフターエフェクト呼び出し
        private void callAfterEffect()
        {
            if (!string.IsNullOrEmpty(ed.afterEffect))
            {
                //Debug.Log($"afterEffect:{ed.afterEffect}");
                switch (moveType)
                {
                    case (EffType.accR):
                        YzUtil.PlayEffect(ed.afterEffect, cc, cc, cc.pos.Copy());
                        break;
                    default:
                        YzUtil.PlayEffect(ed.afterEffect, cc, TP: tp);
                        break;
                }
            }
        }
        //子も含めたスプライトの透明化
        void SpriteAlphaZero()
        {
            SpriteRenderer[] sprites = this.GetComponentsInChildren<SpriteRenderer>();
            foreach (var s in sprites)
            {
                //Debug.Log(s.gameObject.name);
                s.color.SetAlpha(0);
                s.enabled = false;
            }

        }

    }

}
//