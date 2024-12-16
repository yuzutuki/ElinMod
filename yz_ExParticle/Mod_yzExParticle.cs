using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
//
//
[BepInPlugin("yuzutuki.yzExParticle", "ExParticle", "1.0.0.0")]
public class yzExParticle : BaseUnityPlugin
{
//    static public string yztest = "ねう！";
    static public AssetBundle ab;
    static public string dir;
    private void Start()
    {
        //        yztest = "ねうねう！";
        dir = Path.GetDirectoryName(Info.Location);
        ab = AssetBundle.LoadFromFile(Path.Combine(dir, "ParticleAsset", "testparticle2"));
        //
        var harmony = new Harmony("yzExParticle");
        harmony.PatchAll();
        //
        if (yzParticleDictionary.dicObj.Count == 0)
            yzParticleDictionary.LoadingAsset();
    }
    static public yz_objFollw CreateIns(string name,Vector3 setPos)
    {
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
/*
[HarmonyPatch(typeof(Effect), "Activate")]
public class yzPostEffect : yzExParticle
{
static public yz_objFollw CreateIns(string name, GameObject parent, Vector3 setPos)
{
    GameObject obj = (GameObject)Instantiate(yzParticleDictionary.dicObj[name]);
    obj.transform.position = setPos;
        obj.AddComponent<yz_objFollw>();
        yz_objFollw sc = obj.GetComponent<yz_objFollw>();
    return (sc);
}
//
//色の設定
static public Color ColorSeting(Effect ins)
{
    Color color = Color.white;
    if (ins.sr != null)
    {
        color = ins.sr.color;
    }
    color.SetAlpha(224);
    return (color);
}
[HarmonyPostfix]
static void Postfix(Effect __instance)
{
    string name = __instance.GetComponent<Transform>().name;
    string particleID = null;
    yz_objFollw sc;
    switch (name)
    {
        case "spell_arrow(Clone)":
            particleID = "spArrow1";
            sc = CreateIns(particleID,__instance.gameObject,__instance.fromV);
            sc.setProp(
                type: yz_objFollw.Type.arrow,
                targetPos: __instance.destV,
                pPos: __instance.fromV,
                sp: 0.5f,
                setColor: ColorSeting(__instance));
            break;
/*                
        case "cast(Clone)":
            particleID = "castBlue";
            sc = CreateIns(particleID, __instance.gameObject, __instance.fromV);
            sc.setProp(
                type: yz_objFollw.Type.self,
                pPos: __instance.transform.position);
            break;
    }
}
}
*/
//
[HarmonyPatch(typeof(ActEffect),nameof(ActEffect.ProcAt))]
public class yz_readAction : yzExParticle
{
    [HarmonyPrefix]
    public static void Prefix(ActEffect __instance, EffectId id,Card cc,Point tp, ActRef actRef = default(ActRef))
    {
        //id:種類(Arrow,Ball..)
        //AliasEle:属性(eleFire,eleIce...)
        //        ElementRef er = EClass.setting.elements[e.source.alias];
        //詠唱エフェクト
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
            //setV.z
            yz_objFollw sc = CreateIns(particleID, setV); ;
            sc.setProp(
                type: yz_objFollw.Type.self,
                pPos: setV);
//            Debug.Log($"cc.pos.position:{setV}");
            //
            switch (id)
            {
                case EffectId.Arrow:
                    //Debug.Log("この辺に矢処理");
                    sc = CreateIns("spArrow1", setV);
                    sc.setProp(
                        type: yz_objFollw.Type.arrow,
                        targetPos:targetV,
                        pPos: setV,
                        sp: 0.5f,
                        setColor: elementColor);
                    break;
                case EffectId.Ball:
//                    Debug.Log("この辺にボール処理");
//                    Debug.Log($"setV?:{setV}");
                    sc = CreateIns("spBall", setV); ;
                    sc.setProp(
                        type: yz_objFollw.Type.self,
                        pPos: setV,
                        setColor:elementColor);
                    break;
                case EffectId.Bolt:
//                    Debug.Log("この辺に光線処理");
//                    Debug.Log($"setV?:{setV},tV:{targetV}");
                    sc = CreateIns("spBolt", setV); ;
                    sc.setProp(
                        type: yz_objFollw.Type.bolt,
                        pPos: setV,
                        targetPos:targetV,
                        setColor: elementColor);
                    break;
                case EffectId.Buff:
//                    Debug.Log("この辺にバフ処理");
                    break;
            }
        }
//        Debug.Log($"ElementData:{e.id},{e.vBase},{e._source}");
    }
}
//