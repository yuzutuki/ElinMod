using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using BepInEx;

namespace yz_sc
{
    [BepInPlugin("yuzutuki.yzSoundChecker", "yzSoundChecker", "1.0.0.0")]
    public class yzSoundChecker : BaseUnityPlugin
    {
        private void Awake()
        {
            var harmony = new Harmony("yzSoundChecker");
            harmony.PatchAll();
        }
    }

        [HarmonyPatch(typeof(Point), nameof(Point.PlaySound))]
	public static class PlaySoundPatch
	{
		[HarmonyPostfix]
		public static void Postfix(string id, bool synced = true, float v = 1f, bool spatial = true)
		{
			Debug.Log($"Play:{id}");
		}
	}
}