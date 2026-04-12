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

namespace yz_sss
{

    [BepInPlugin("yuzutuki.yzSSS", "SSS", "1.0.0.0")]
    public class YzSSS : BaseUnityPlugin
    {
        static public Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();
        static public Dictionary<string, EffData> effDic = new Dictionary<string, EffData>();
        internal const string ModOptionsGuid = "evilmask.elinplugins.modoptions";
        static public string guid = "yuzutuki.yzSSS";

        private void Awake()
        {
            //Harmony適用
            var harmony = new Harmony("yz_SSS");
            harmony.PatchAll();
            //Config読み込み
            ModConfig.LoadConfig(base.Config);
        }
        private void Start()
        {
            //ModOptions適用
            ModConfigCore.ModConfigSetting();
            //生成率設定
            ModConfigCore.ResetSpawnChance();
        }
        public void OnStartCore()
        {
            //
            //
            EffectObjectLoader.LoadEffectAsset(Path.GetDirectoryName(Info.Location));
            Debug.Log("=====SSS:アセット読み込み完了！=====");
        }
    }
}