using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エフェクトオブジェクトのスクリプト
public class yz_objFollw : MonoBehaviour
{
    ParticleSystem ps;//パーティクル管理
    Transform traT;//ターゲット
    Transform tra;//自分の
    Vector2 pos;//自分の座標
    Vector3 parentPos;//親の座標
    Vector2 pos2;//移動予定先
    Vector2 posT;//ターゲットの座標
    Color color;//色の補正
//    float alpha = 1;//パーティクル透明度の補正
    int lifetime;//生存時間
    float speed;//速度
    Type moveType;//移動タイプ
    bool lookTarget;
    bool stopEmit;//パーティクルの放出を停止
    bool oneShot=false;
    public enum Type
    {
        follw,
        arrow,
        bolt,
        point
    }
    public void setTarget(GameObject o)
    {
        traT = o.GetComponent<Transform>();
    }
    public void setProp(
        Type type = Type.point,
        Vector2 targetPos = default,
        Vector3 pPos = default,
        GameObject targetO = null,
        int time = -1,
        float sp = 0,
        Color setColor = default,
        float setAlpha = default,
        bool look = false)
    {
        moveType = type;
        if (targetO != null)
            traT = targetO.GetComponent<Transform>();
        posT = targetPos;
        parentPos = pPos;
        speed = sp;
        lifetime = time;
        color = setColor;
        if (setAlpha != default||setAlpha!=0)
            color.a *= setAlpha;
        lookTarget = look;
//        Debug.Log("setProp!");
//        Debug.Log($"{moveType}{posT}{traT}{parentPos}{sp}");
    }
    //
    void Start()
    {
        tra = this.GetComponent<Transform>();
        ps = this.GetComponent<ParticleSystem>();
        var psm = ps.main;
        pos = tra.position;
        pos2 = pos;
        //
        if ((Vector4)color != Vector4.zero)
            psm.startColor= color;
        //

    }
    void FixedUpdate()
    {
        if (tra != null)
        {
            if (!stopEmit)
            {
                pos = tra.position;
                //ターゲットのほうへ向ける(最初だけ)
                if (lookTarget&&!oneShot)
                {
                    oneShot = true;
                    if (posT != default && posT != null)
                    {
                        Vector2 direction = posT - pos2;
                        float r = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        tra.rotation = Quaternion.Euler(0, 0, r);
                    }
                }
                switch (moveType)
                {
                    case (Type.arrow)://アロー移動処理
                        if (pos != posT)
                        {
                            pos2 = Vector2.MoveTowards(pos, posT, speed);
                        }
                        else
                        {
                            stopEmit = true;
                        }
                        break;
                    case (Type.follw)://追従型移動処理
                        if (traT != null)
                        {
                            posT = traT.position;
                            pos2 = Vector2.Lerp(pos, posT, 0.9f);
                        }
                        else
                        {
                            stopEmit = true;
                        }
                        break;
                    case (Type.point)://定点処理
                        pos2 = pos;
                        break;
                    case (Type.bolt)://方向指定処理
                        pos2 = pos;
                        /*
                        if (!oneShot)
                        {
                            oneShot = true;
                            if (posT == default || posT == null)
                                break;
                            Vector2 direction = posT - pos2;
                            float r = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                            tra.rotation = Quaternion.Euler(0, 0, r);
                        }
                        */
                        break;
                }
                if (stopEmit && ps.isPlaying)
                {
                    ps.Stop();
                }

            }
            //座標の適用とZの補正
            tra.position = new Vector3(pos2.x, pos2.y, parentPos.z - 20);
        }
    }
}
