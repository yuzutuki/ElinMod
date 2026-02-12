using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using Cwl.Helper.Unity;

namespace yz_sss
{

	[BepInPlugin("yuzutuki.yzSSS", "SSS", "1.0.0.0")]
	public class YzSSS : BaseUnityPlugin
	{
		static public Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();
		static public Dictionary<string, EffData> effDic = new Dictionary<string, EffData>();


		private void Awake()
		{
			var harmony = new Harmony("yz_SSS");
			harmony.PatchAll();
		}
		public void OnStartCore()
		{
			//
			//
			EffectObjectLoader.LoadEffectAsset(Path.GetDirectoryName(Info.Location));
			Debug.Log("=====SSS:onStartCore!=====");
		}
	}
}
/*
 *仕上げToDo:
 *フック三種のテキスト回り
 *OK：マップのブラッシュアップ
 *効果音をちゃんとつける
 *動作確認
 *・ネフィア潜る
 *・マップ端の処理確認
 *・新規セーブでマップ読み込み確認
 *デバッグログのコメントアウト
 *各種メッセージ見直し
 */