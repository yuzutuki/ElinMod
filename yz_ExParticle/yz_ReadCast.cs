using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BepInEx;
using HarmonyLib;

namespace yz_ShinySpell
{
    //�r���������ړ����悤�Ƃ��Ă��܂������Ȃ������c�[�I�Ȃ��
    /*
     * ���Ԃ�Harmony�p�b�`�悪����ł��Ă��Ȃ�
     * ���\�b�h�̈��������Ă�����΂����邩��
    [HarmonyPatch(typeof(Chara), nameof(Chara.UseAbility))]
    public class ReadCast : yzExParticle
    {
        public static void Prefix(Act a,Point pos = null)
        {
            Debug.Log("�˂˂��˂���");
            //Debug.Log($"use:{a.Name},pos:{pos}");
        }
    }
    */
}