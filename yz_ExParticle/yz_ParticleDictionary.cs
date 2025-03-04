using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_ShinySpell
{
    //読み込んだアセットをDictionaryにぶちこむ
    //ついでにスクリプトが必要な子へのアタッチ処理
    static class yzParticleDictionary
    {
        static public bool onshot;
        static public Dictionary<string, GameObject> dicObj = new Dictionary<string, GameObject>();
        static public void LoadingAsset()
        {
            //たぶんforeach文とかでもっと最適化できる気がする
            dicObj.Add("testpart1", yzExParticle.ab.LoadAsset<GameObject>("testpart1"));
            dicObj.Add("testpart2", yzExParticle.ab.LoadAsset<GameObject>("testpart2"));
            dicObj.Add("spArrow1", yzExParticle.ab.LoadAsset<GameObject>("spArrow1"));
            dicObj.Add("castBlack", yzExParticle.ab.LoadAsset<GameObject>("castBlack"));
            dicObj.Add("castWhite", yzExParticle.ab.LoadAsset<GameObject>("castWhite"));
            dicObj.Add("castBlue", yzExParticle.ab.LoadAsset<GameObject>("castBlue"));
            dicObj.Add("castSummon", yzExParticle.ab.LoadAsset<GameObject>("castSummon"));
            dicObj.Add("spBall", yzExParticle.ab.LoadAsset<GameObject>("spBall"));
            dicObj.Add("spBolt", yzExParticle.ab.LoadAsset<GameObject>("spBolt"));
            dicObj.Add("spMiasma", yzExParticle.ab.LoadAsset<GameObject>("spMiasma"));
            dicObj.Add("spHand", yzExParticle.ab.LoadAsset<GameObject>("spHand"));
            Debug.Log("___ExParticle_Loading___");
            Debug.Log("modPath..:" + yzExParticle.dir);
            //スクリプトのアタッチ
            //命名規則決めて単純化できそう
            Transform child;
            ////手
            child = dicObj["spHand"].transform.Find("sp1");
            child.gameObject.AddComponent<yz_effObjectManager>();
            child.GetComponent<yz_effObjectManager>().setProp(
                cStr: 0.8f,
               aFix: 1f);
            child = dicObj["spHand"].transform.Find("sp4");
            child.gameObject.AddComponent<yz_effObjectManager>();
            child.GetComponent<yz_effObjectManager>().setProp(
                cStr: 0.6f,
               aFix: 0.8f);
            child = dicObj["spHand"].transform.Find("sp5");
            child.gameObject.AddComponent<yz_effObjectManager>();
            child.GetComponent<yz_effObjectManager>().setProp(
                cStr: 0.2f,
               aFix: 0.8f);
            ////矢
            child = dicObj["spArrow1"].transform.Find("sp1");
            child.gameObject.AddComponent<yz_effObjectManager>();
            child.GetComponent<yz_effObjectManager>().setProp(
                cStr: 0.8f,
               aFix: 1f);
            ////ボール
            child = dicObj["spBall"].transform.Find("sp1");
            child.gameObject.AddComponent<yz_effObjectManager>();
            child.GetComponent<yz_effObjectManager>().setProp(
                rAngle: true,
                cStr: 0.7f,
               aFix: 0.9f);
            child = dicObj["spBall"].transform.Find("sp2");
            child.gameObject.AddComponent<yz_effObjectManager>();
            child.GetComponent<yz_effObjectManager>().setProp(
                rAngle: true,
                cStr: 0.7f,
               aFix: 0.9f);
            child = dicObj["spBall"].transform.Find("sp4");
            child.gameObject.AddComponent<yz_effObjectManager>();
            child.GetComponent<yz_effObjectManager>().setProp(
                cStr: 1f,
               aFix: 1f);
            ////光線
            child = dicObj["spBolt"].transform.Find("sp1");
            child.gameObject.AddComponent<yz_effObjectManager>();
            child.GetComponent<yz_effObjectManager>().setProp(
                cStr: 0.8f,
               aFix: 1f);
            ///
            onshot = true;
        }
        //
    }
}