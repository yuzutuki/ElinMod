using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

namespace yz_sss
{
	//何らかの理由でマップの配置ができてなかったときよう。

	[HarmonyPatch(typeof(Region),nameof(Region.CheckRandomSites))]
	internal class CheckRandomSitesPatch
	{
		[HarmonyPostfix]
		private static void Postfix(Region __instance)
		{
			//Debug.Log(__instance.id);
			if (__instance.id == "ntyris")
			{
				bool add = false;
				if (__instance.FindZone("yz_sssMainZone") == null)
				{
					Debug.Log("Missing MainZone in " + __instance.name);

					SpatialGen.Create("yz_sssMainZone", __instance, true, -99999, -99999, 0);
					add = true;
				}
				else
				{
					//Debug.Log("MainZone already exists in " + __instance.id);
					Zone zone = __instance.FindZone("yz_sssMainZone");
					if (zone.Region == null)
					{
						Debug.Log("MainZone has no region, destroying and recreating...");
						zone.Destroy();
						SpatialGen.Create("yz_sssMainZone", __instance, true, -99999, -99999, 0);
						Zone NewZone = __instance.FindZone("yz_sssMainZone");
						Debug.Log($"ZoneID: {NewZone.id},Region: {NewZone?.Region?.id}, Position: {NewZone.mapX}, {NewZone.mapY},{NewZone.x}, {NewZone.y}");
						add = true;
					}
				}
				if (add)
				{
					__instance.elomap.objmap.UpdateMeshImmediate();
				}
			}
		}
	}
	
}