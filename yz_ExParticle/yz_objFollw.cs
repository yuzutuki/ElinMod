using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G�t�F�N�g�I�u�W�F�N�g�̃X�N���v�g
public class yz_objFollw : MonoBehaviour
{
    ParticleSystem ps;//�p�[�e�B�N���Ǘ�
    Transform traT;//�^�[�Q�b�g
    Transform tra;//������
    Vector2 pos;//�����̍��W
    Vector3 parentPos;//�e�̍��W
    Vector2 pos2;//�ړ��\���
    Vector2 posT;//�^�[�Q�b�g�̍��W
    Color color;//�F�̕␳
//    float alpha = 1;//�p�[�e�B�N�������x�̕␳
    int lifetime;//��������
    float speed;//���x
    Type moveType;//�ړ��^�C�v
    bool lookTarget;
    bool stopEmit;//�p�[�e�B�N���̕��o���~
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
                //�^�[�Q�b�g�̂ق��֌�����(�ŏ�����)
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
                    case (Type.arrow)://�A���[�ړ�����
                        if (pos != posT)
                        {
                            pos2 = Vector2.MoveTowards(pos, posT, speed);
                        }
                        else
                        {
                            stopEmit = true;
                        }
                        break;
                    case (Type.follw)://�Ǐ]�^�ړ�����
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
                    case (Type.point)://��_����
                        pos2 = pos;
                        break;
                    case (Type.bolt)://�����w�菈��
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
            //���W�̓K�p��Z�̕␳
            tra.position = new Vector3(pos2.x, pos2.y, parentPos.z - 20);
        }
    }
}
