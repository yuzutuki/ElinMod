using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エフェクトのオブジェクトのさらに子オブジェクト用のスクリプト
//主にスプライトの制御用
public class yz_effObjectManager : MonoBehaviour
{
    Transform pTra;
    Transform tra;
    SpriteRenderer sr;
    ParticleSystem ps;
    Color pColor;
    public Color colorFix = Color.white;
    public float alphaFix = 1f;
    [SerializeReference,Tooltip("ランダムな角度を与える？")]
    bool randomAngle = false;
    [SerializeField,Tooltip("パーティクルに設定された色への乗算")]
    float colorStr = 0.7f;
    bool oneshot = false;
    public void setProp(
        Color cFix = default,//色補正
        float aFix = default,//アルファ値の上書き
        bool rAngle = false,//ランダム角度
        float cStr = 0.7f//色の合成度
        )
    {
//        Debug.Log($"setprop:cFix:{cFix},aFix:{aFix}");
        if (cFix != default)
            colorFix = cFix;
        else
            colorFix = Color.white;
        if(aFix!=default)
            alphaFix = aFix;
        randomAngle = rAngle;
        colorStr = cStr;
    }
    void fakeStart()
    {
        tra = this.transform;
        pTra = tra.parent.transform;
        sr = this?.GetComponent<SpriteRenderer>();
        ps = pTra?.GetComponent<ParticleSystem>();
        if (ps != null && sr != null)
        {
            var psm = ps.main;
            pColor = psm.startColor.color;
            float tempA = sr.color.a;
            Color setColor = sr.color * (pColor * colorStr);
            setColor.SetAlpha(tempA);
            setColor *= colorFix;
            setColor.SetAlpha(setColor.a * alphaFix);
            sr.color = setColor;
//            Debug.Log($"ColorResult:{setColor}");
        }
        else
        {
            Debug.Log("missing SRorPS");
        }
        //
        if (randomAngle)
        {
            Vector3 ang = new Vector3(0, 0, Random.Range(-180f, 180f));
            tra.eulerAngles = ang;
        }
    }
    //
    //処理順の都合でStart()じゃなくてUpdate()で初期化処理
    private void Update()
    {
        if (!oneshot)
        {
            fakeStart();
            oneshot = true;
        }
    }
}
