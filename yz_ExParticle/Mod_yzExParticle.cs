using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
//
namespace yz_ShinySpell
{
    //
    [BepInPlugin("yuzutuki.yzExParticle", "ExParticle", "1.0.0.0")]
    public class yzExParticle : BaseUnityPlugin
    {
        static public AssetBundle ab;
        static public string dir;
        public void OnStartCore()
        {
            //Debug.Log("=====onStartCore!=====");
            //Mod���ۑ�����Ă���t�H���_�̎擾�ƃA�Z�b�g�̓ǂݍ��݂Ƃ�
            dir = Path.GetDirectoryName(Info.Location);
            ab = AssetBundle.LoadFromFile(Path.Combine(dir, "ParticleAsset", "testparticle2"));
            if (yzParticleDictionary.dicObj.Count == 0)
                yzParticleDictionary.LoadingAsset();
        }
        private void Awake()
        {
            var harmony = new Harmony("yz_ShinySpell");
            harmony.PatchAll();
        }
        
        //�I�u�W�F�N�g�̐����p
        static public yz_objFollw CreateIns(string name, Vector3 setPos)
        {
            Debug.Log($"callCI:{name}");
            GameObject obj = (GameObject)Instantiate(yzParticleDictionary.dicObj[name]);
            //        Debug.Log($"{name},pos:{setPos}");
            obj.transform.position = setPos;
            obj.AddComponent<yz_objFollw>();
            yz_objFollw sc = obj.GetComponent<yz_objFollw>();
            return (sc);
        }
    }
    //
    //
    //�r���G�t�F�N�g
    /*
    [HarmonyPatch(typeof(Chara), nameof(Chara.UseAbility))]
    public class yz_readCast : yzExParticle
    {
        //    public static void UseAbilityPrefix(Act a, Card tc = null, Point pos = null, bool pt = false)
        [HarmonyPrefix]
        public static void UseAbilityPrefix()
        {
            //
            Debug.Log("neww");
    //        Debug.Log($"use:{a.Name},pos:{pos}");
         }
    }
        [HarmonyPatch(typeof(Chara), nameof(Chara.UseAbility)), HarmonyPrefix]
        public void UseAbilityPrefix()
        {
            //
            Debug.Log("calling!");
            //        Debug.Log($"use:{a.Name},pos:{pos}");
        }
    */
    //
    //
    //[HarmonyPatch(typeof(ActEffect), nameof(ActEffect.ProcAt))]
    /*
    [HarmonyPatch]
    public class yz_readAction : yzExParticle
    {
        //    [HarmonyPrefix]
        //
    [HarmonyPatch(typeof(ActEffect), nameof(ActEffect.ProcAt)),HarmonyPrefix]
        public static void ProcAtPrefix(ActEffect __instance, EffectId id,Card cc,Point tp, ActRef actRef = default(ActRef))
        {
            //id:���(Arrow,Ball..)
            //AliasEle:����(eleFire,eleIce...)
            //        ElementRef er = EClass.setting.elements[e.source.alias];
            //�r���G�t�F�N�g�̏����Ɖ�����
            string particleID;
            particleID = null;
            switch (id)
            {
                case EffectId.Arrow:
                case EffectId.Ball:
                case EffectId.Bolt:
                case EffectId.Hand:
                case EffectId.Meteor:
                case EffectId.Earthquake:
                    particleID = "castBlack";
                    break;
                case EffectId.Heal:
                case EffectId.Buff:
                case EffectId.BuffStats:
                case EffectId.Uncurse:
                case EffectId.UncurseEQ:
                case EffectId.Revive:
                case EffectId.RestoreBody:
                case EffectId.RestoreMind:
                case EffectId.RemoveHex:
                case EffectId.RemoveHexAll:
                    particleID = "castWhite";
                    break;
                case EffectId.Summon:
                case EffectId.Funnel:
                case EffectId.Debuff:
                case EffectId.DebuffStats:
                    particleID = "castSummon";
                    break;
                case EffectId.MistOfDarkness:
                case EffectId.Web:
                case EffectId.Gravity:
                case EffectId.Identify:
                case EffectId.MagicMap:
                case EffectId.Teleport:
                case EffectId.TeleportShort:
                case EffectId.Evac:
                case EffectId.Return:
                case EffectId.Mutation:
                    particleID = "castBlue";
                    break;
            }
            if (particleID != null)
            {
                Color elementColor = Color.white;
                string aEle = actRef.aliasEle;
                if (id == EffectId.DebuffStats || id== EffectId.BuffStats ||
                    aEle == null || aEle == "")
                {
                    aEle = "_void";
    //                Debug.Log($"missingElement?");
                }
                elementColor = EClass.setting.elements[aEle].colorSprite;
    //                Debug.Log($"EffectID:{id},aliasEle:{aEle},eColor:{elementColor}");
                elementColor.r = Mathf.Max(elementColor.r,0.2f);
                elementColor.g = Mathf.Max(elementColor.g, 0.2f);
                elementColor.b = Mathf.Max(elementColor.b, 0.2f);
                //
                Vector3 setV = cc.pos.PositionCenter();
                setV.x += 0.05f;
                setV.y += 0.3f;
                setV.z -= 80f;
                Vector3 targetV = tp.PositionCenter();
    //            targetV.x += 0.05f;
                targetV.y += 0.3f;
                targetV.z -= 80f;
                yz_objFollw sc = CreateIns(particleID, setV);
                sc.setProp(
                    type: yz_objFollw.Type.point,
                    pPos: setV);
                Debug.Log("=-=-cast!-=-=");
                Debug.Log($"cc.pos.position:{setV}");
                //�U���G�t�F�N�g�̏���
                switch (id)
                {
                    case EffectId.Hand:
                        sc = CreateIns("spHand", setV); ;
                        sc.setProp(
                            type: yz_objFollw.Type.bolt,
                            pPos: setV,
                            targetPos: targetV,
                            setColor: elementColor,
                            look: true);
                        break;
                    case EffectId.Arrow:
                        //Debug.Log("���̕ӂɖ��");
                        sc = CreateIns("spArrow1", setV);
                        sc.setProp(
                            type: yz_objFollw.Type.arrow,
                            targetPos:targetV,
                            pPos: setV,
                            sp: 0.5f,
                            setColor: elementColor,
                            look: true);
                        break;
                    case EffectId.Ball:
    //                    Debug.Log("���̕ӂɃ{�[������");
                        sc = CreateIns("spBall", setV); ;
                        sc.setProp(
                            type: yz_objFollw.Type.point,
                            pPos: setV,
                            setColor:elementColor);
                        break;
                    case EffectId.Bolt:
    //                    Debug.Log("���̕ӂɌ�������");
                        sc = CreateIns("spBolt", setV); ;
                        sc.setProp(
                            type: yz_objFollw.Type.bolt,
                            pPos: setV,
                            targetPos:targetV,
                            setColor: elementColor,
                            look:true);
                        break;
                    case EffectId.Debuff:
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
                                sc = CreateIns("spMiasma", targetV); ;
                                sc.setProp(
                                    type: yz_objFollw.Type.point,
                                    pPos: targetV,
                                    setColor: (elementColor * 0.7f) + new Color(0.1f, 0.1f, 0.1f),
                                    setAlpha:0.6f);
                                break;
                        }
                        break;
                    case EffectId.Buff:
    //                    Debug.Log("���̕ӂɃo�t����");
                        break;
                }
            }
    //        Debug.Log($"ElementData:{e.id},{e.vBase},{e._source}");
        }
    }
    */
    //
}