using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using Cwl.Helper.Unity;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;
using UnityEngine.UI;

namespace yz_ReSS
{

    [BepInPlugin("yuzutuki.yzReSS", "ReSS", "1.0.0.0")]
    public class YzReSS : BaseUnityPlugin
    {
        static public Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();
        static public Dictionary<string, EffData> effDic = new Dictionary<string, EffData>();
        static public List<int> SpellIDList = new List<int>();
        internal const string ModOptionsGuid = "evilmask.elinplugins.modoptions";
        static public string guid = "yuzutuki.yzReSS";

        private void Awake()
        {
            //Harmony適用
            var harmony = new Harmony("yz_ReSS");
            harmony.PatchAll();
            //Config読み込み
            Debug.Log("=====ReSS:Config読み込み(Awake)=====");
            ModConfig.LoadConfig(base.Config);
        }
        private void Start()
        {
            //ModOptions適用
            Debug.Log("=====ReSS:Config読み込み(Start)=====");
            ModConfigCore.ModConfigSetting();
        }
        public void OnStartCore()
        {
            //
            EffectObjectLoader.LoadEffectAsset(Path.GetDirectoryName(Info.Location));
            Debug.Log("=====ReSS:アセット読み込み完了！=====");
        }
    }
}