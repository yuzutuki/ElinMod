using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yz_effObjectManager : MonoBehaviour
{
    Transform pTra;
    Transform tra;
    SpriteRenderer sr;
    ParticleSystem ps;
    Color pColor;
    public Color colorFix = Color.white;
    public float alphaFix = 1f;
    [SerializeReference,Tooltip("�����_���Ȋp�x��^����H")]
    bool randomAngle = false;
    [SerializeField,Tooltip("�p�[�e�B�N���ɐݒ肳�ꂽ�F�ւ̏�Z")]
    float colorStr = 0.7f;
    bool oneshot = false;
    public void setProp(
        Color cFix = default,//�F�␳
        float aFix = default,//�A���t�@�l�̏㏑��
        bool rAngle = false,//�����_���p�x
        float cStr = 0.7f//�F�̍����x
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
//        Debug.Log("objectManager is load!");
        //
        sr = this?.GetComponent<SpriteRenderer>();
        ps = pTra?.GetComponent<ParticleSystem>();
        if (ps != null && sr != null)
        {
            var psm = ps.main;
            pColor = psm.startColor.color;
            float tempA = sr.color.a;
            Color setColor = sr.color * (pColor * colorStr);
            //            Color setColor = Color.Lerp(sr.color, pColor, colorStr);
            //            Debug.Log($"ColorStat:p:{pColor},sr:{sr.color},str:{colorStr},fixC:{colorFix},fixA:{alphaFix}");
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
    private void Update()
    {
        if (!oneshot)
        {
            fakeStart();
            oneshot = true;
        }
    }
}
