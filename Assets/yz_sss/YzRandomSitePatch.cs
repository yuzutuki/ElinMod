using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace yz_sss
{
	[HarmonyPatch(typeof(Region),nameof(Region.CheckRandomSites))]
	internal class CheckRandomSitesPatch
	{
		[HarmonyPostfix]
		private static void Postfix(Region __instance)
		{
			bool add = false;
			if (__instance.FindZone("yz_sssMainZone") == null)
			{
				SpatialGen.Create("yz_sssMainZone", __instance, true, -99999, -99999, 0);
				add = true;
			}
			if (add)
			{
				__instance.elomap.objmap.UpdateMeshImmediate();
			}
		}
	}
}