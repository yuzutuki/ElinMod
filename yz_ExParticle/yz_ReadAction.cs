using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using HarmonyLib;

namespace yz_ShinySpell
{

    [HarmonyPatch(typeof(ActEffect), nameof(ActEffect.ProcAt))]
    public class ReadAction
    {
        public static void Prefix(ActEffect __instance, EffectId id, Card cc, Point tp, ActRef actRef = default(ActRef))
        {
            //id:���(Arrow,Ball..)
            //AliasEle:����(eleFire,eleIce...)
            //        ElementRef er = EClass.setting.elements[e.source.alias];
            //�r���G�t�F�N�g�̏����Ɖ�����
            string particleID;
            particleID = null;
            switch (id.ToString())
            {
                case "Arrow":
                case "Ball":
                case "Bolt":
                case "Hand":
                case "Meteor":
                case "Earthquake":
                    particleID = "castBlack";
                    break;
                case "Heal":
                case "Buff":
                case "BuffStats":
                case "Uncurse":
                case "UncurseEQ":
                case "Revive":
                case "RestoreBody":
                case "RestoreMind":
                case "RemoveHex":
                case "RemoveHexAll":
                    particleID = "castWhite";
                    break;
                case "Summon":
                case "Funnel":
                case "Debuff":
                case "DebuffStats":
                    particleID = "castSummon";
                    break;
                case "MistOfDarkness":
                case "Web":
                case "Gravity":
                case "Identify":
                case "MagicMap":
                case "Teleport":
                case "TeleportShort":
                case "Evac":
                case "Return":
                case "Mutation":
                    particleID = "castBlue";
                    break;
                default:
                    break;
            }
            //
            if (particleID != null)
            {
                Color elementColor = Color.white;
                string aEle = actRef.aliasEle;
                //�F���擾�ł��Ȃ������ȂƂ��̏����Ƃ�
                    if (id.ToString() == "DebuffStats" || id.ToString() == "BuffStats" ||
                        aEle == null || aEle == "")
                    {
                    aEle = "_void";
                    Debug.Log($"missingElement?,{id}");
                }
                elementColor = EClass.setting.elements[aEle].colorSprite;
                if(elementColor == null || elementColor == Color.black)
                    elementColor = Color.gray;
                //Debug.Log($"EffectID:{id},aliasEle:{aEle},eColor:{elementColor}");
                //�F����(��Z�œ��������Ȃ��悤��)
                elementColor.r = Mathf.Max(elementColor.r, 0.2f);
                elementColor.g = Mathf.Max(elementColor.g, 0.2f);
                elementColor.b = Mathf.Max(elementColor.b, 0.2f);
                //�\���ʒu�̎擾�ƒ���
                Vector3 setV = cc.pos.PositionCenter();
                setV.x += 0.05f;
                setV.y += 0.3f;
                setV.z -= 80f;
                Vector3 targetV = tp.PositionCenter();
                //targetV.x += 0.05f;
                targetV.y += 0.3f;
                targetV.z -= 80f;
                //�G�t�F�N�g����
                yz_objFollw sc = yzExParticle.CreateIns(particleID, setV);
                sc.setProp(
                    type: yz_objFollw.Type.point,
                    pPos: setV);
                //                Debug.Log("=-=-cast!-=-=");
                //                Debug.Log($"cc.pos.position:{setV}");
                //�U���G�t�F�N�g�̏���
                Debug.Log($"castID :{id},{id.ToString()}");
                switch (id.ToString())
                {
                    case "Hand":
                        sc = yzExParticle.CreateIns("spHand", setV); ;
                        sc.setProp(
                            type: yz_objFollw.Type.bolt,
                            pPos: setV,
                            targetPos: targetV,
                            setColor: elementColor,
                            look: true);
                        break;
                    case "Arrow":
                        //Debug.Log("���̕ӂɖ��");
                        sc = yzExParticle.CreateIns("spArrow1", setV);
                        sc.setProp(
                            type: yz_objFollw.Type.arrow,
                            targetPos: targetV,
                            pPos: setV,
                            sp: 0.5f,
                            setColor: elementColor,
                            look: true);
                        break;
                    case "Ball":
                        //Debug.Log("���̕ӂɃ{�[������");
                        sc = yzExParticle.CreateIns("spBall", setV); ;
                        sc.setProp(
                            type: yz_objFollw.Type.point,
                            pPos: setV,
                            setColor: elementColor);
                        break;
                    case "Bolt":
                        //Debug.Log("���̕ӂɌ�������");
                        sc = yzExParticle.CreateIns("spBolt", setV); ;
                        sc.setProp(
                            type: yz_objFollw.Type.bolt,
                            pPos: setV,
                            targetPos: targetV,
                            setColor: elementColor,
                            look: true);
                        break;
                    case "Debuff":
                        //�G�������g����ᏋC�������o
                        switch (aEle)
                        {
                            case ("eleFire"):
                            case ("eleCold"):
                            case ("eleLightning"):
                            case ("eleDarkness"):
                            case ("eleMind"):
                            case ("elePoison"):
                            case ("eleNether"):
                            case ("eleSound"):
                            case ("eleNerve"):
                            case ("eleHoly"):
                            case ("eleChaos"):
                            case ("eleMagic"):
                            case ("eleEther"):
                            case ("eleAcid"):
                            case ("eleCut"):
                                sc = yzExParticle.CreateIns("spMiasma", targetV); ;
                                sc.setProp(
                                    type: yz_objFollw.Type.point,
                                    pPos: targetV,
                                    setColor: (elementColor * 0.7f) + new Color(0.1f, 0.1f, 0.1f),
                                    setAlpha: 0.6f);
                                break;
                        }
                        break;
                    case "Buff"://case EffectId.Buff:
                        //                    Debug.Log("���̕ӂɃo�t����");
                        break;
                    default:
                        break;
                }
            }
            //        Debug.Log($"ElementData:{e.id},{e.vBase},{e._source}");
        }
    }
}