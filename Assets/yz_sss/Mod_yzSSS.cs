using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
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
        static public List<int> SpellIDList = new List<int>();
        internal const string ModOptionsGuid = "evilmask.elinplugins.modoptions";
        static public string guid = "yuzutuki.yzSSS";

        private void Awake()
        {
            //Harmony適用
            var harmony = new Harmony("yz_SSS");
            harmony.PatchAll();
            //Config読み込み
            Debug.Log("=====SSS:Config読み込み(Awake)=====");
            ModConfig.LoadConfig(base.Config);
        }
        private void Start()
        {
            //IDリスト生成
            CreateIDList();
            //ModOptions適用
            Debug.Log("=====SSS:Config読み込み(Start)=====");
            ModConfigCore.ModConfigSetting();
            //生成率設定
            ModConfigCore.ResetSpawnChance();
        }
        public void OnStartCore()
        {
            //
            EffectObjectLoader.LoadEffectAsset(Path.GetDirectoryName(Info.Location));
            Debug.Log("=====SSS:アセット読み込み完了！=====");
        }
        public void CreateIDList()
        {
            SpellIDList.Clear();
            //基準値(720012:アナライズ以降)
            int originID = 720012;
            //とりあえず+100まで
            for (int i = 0; i < 100; i++)
            {
                int id = originID + i;
                EClass.sources.elements.map.TryGetValue(id, out SourceElement.Row row);
                if (row != null)
                {
                    if (row.alias.Contains("SpYz") && row.thing.Contains("B"))
                    {
                        SpellIDList.Add(id);
                    }
                }

            }
        }
    }
}