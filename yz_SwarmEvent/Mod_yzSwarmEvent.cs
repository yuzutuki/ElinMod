using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//
//
[BepInPlugin("yuzutuki.yzSwarmEvent", "SwarmEvent", "1.0.0.0")]
public class yzSwarmEvent : BaseUnityPlugin
{
    private void Start()
    {
        var harmony = new Harmony("yzSwarmEvent");
        harmony.PatchAll();
	}
	public void OnStartCore()
	{
		var dir = Path.GetDirectoryName(Info.Location);
		var excel = dir + "/SourceCard.xlsx";
		var sources = Core.Instance.sources;
		ModUtil.ImportExcel(excel, "Chara", sources.charas);
//		ModUtil.ImportExcel(excel, "CharaText", sources.charaText);
		//ModUtil.ImportExcel(excel, "ThingV", sources.thingV);
	}
}
//
[HarmonyPatch(typeof(ZoneEventDefenseGame), "NextWave")]
public class yzSE_postSpawn : BaseUnityPlugin
{
    [HarmonyPostfix]
    static void Postfix(ZoneEventDefenseGame __instance)
    {
				if (EClass.rnd(8) == 0)
		{
			EClass.Sound.Play("spell_earthquake");
			__instance.bonus+=2;
			switch (EClass.rnd(2))
			{
				case 0:
				Msg.SayRaw("すごい数の信者が集まってきている！");
				ExSpawn("fanatic", EClass._zone.DangerLv);
					break;
				case 1:
				Msg.SayRaw("戦場にコモドドラゴンを放てっ");
				ExSpawn("komodoensis", EClass._zone.DangerLv);
					break;
				default:
					break;
					/*
				case 2:
					Msg.SayRaw("怒り狂ったニワトリの群れ");
					ExSpawn("chicken", EClass._zone.DangerLv);
					break;
					*/
			}
		}
		//		__instance.Spawn(48);
	}
	static void ExSpawn(string spawnID,int lv)
	{
		Point spawnPoint = EClass._map.bounds.GetRandomEdge(3);
		lv = (lv - 12).ClampMin(5);
		for (int i = 0; i < 28; i++)
		{
			Chara chara = CharaGen.Create(spawnID, lv);
			chara.hostility = (chara.c_originalHostility = Hostility.Enemy);
			chara.SetLv(lv);
			Point nearestPoint = spawnPoint.GetNearestPoint(false, false, true, false);
			EClass._zone.AddCard(chara, nearestPoint);
		}
	}
}
/*
		if (name == "spell_arrow(Clone)")
		{
			string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
			Debug.Log(yzExParticle.yztest);
			Debug.Log("path..." + Application.dataPath);
			GameObject go =yzExParticle.ab.LoadAsset<GameObject>("testpart1");
			GameObject obj = (GameObject)Instantiate(go).gameObject;
			obj.transform.position = __instance.GetComponent<Transform>().position;
			//obj.transform.parent = __instance.transform;
//			Debug.Log(obj.name+"/"+obj.transform.parent.name);
		}
*/
//
//
/*
[HarmonyPatch(typeof(ActEffect))]
[HarmonyPatch(nameof(ActEffect.Proc))]
class yzProcPatch
{
	static void Prefix(ActEffect __instance)
	{
		Debug.Log("zzzHarmoney ActEffect_Prefix");
		WidgetFeed.Instance?.Nerun(Lang.isJP ? ("ねうねう ♪♪") : ("!"));
	}
	static void Postfix(ActEffect __instance)
    {
//		Msg.SayRaw("newnew");
		Debug.Log("zzzHarmoney ActEffect_Postfix");
		WidgetFeed.Instance?.Nerun(Lang.isJP ? ("ねうねう ♪♪") : ("!"));
	}
}
[HarmonyPatch(typeof(ActEffect))]
[HarmonyPatch(nameof(ActEffect.ProcAt))]
class yzProcAtPatch
{
	static void Prefix(ActEffect __instance)
	{
		Debug.Log("zzzHarmoney at_Prefix");
		WidgetFeed.Instance?.Nerun(Lang.isJP ? ("ぷれねうねう ♪♪") : ("!"));
	}
	static void Postfix(ActEffect __instance)
	{
		//		Msg.SayRaw("newnew");
		Debug.Log("zzzHarmoney at_Postfix");
		WidgetFeed.Instance?.Nerun(Lang.isJP ? ("ぽすとねうねう ♪♪") : ("!"));
	}
}
*/
/*
[HarmonyPatch(typeof(Zone))]
[HarmonyPatch(nameof(Zone.Activate))]
class yzZonePatch
{ 
	static void Prefix()
	{
		Debug.Log("zzzHarmoney Prefix");
	}
	static void Postfix(Zone __instance)
	{
		Debug.Log("zzzHarmoney Postfix");
		WidgetFeed.Instance?.Nerun(Lang.isJP ? ("ねうねう ♪♪") : ("!"));
	}
}
*/
/*
 * [HarmonyPatch(typeof(ResourcesPrefabManager), "Load")]
public class ResourcesPrefabManager_Load
{
    [HarmonyPrefix]
    public static void Prefix(ResourcesPrefabManager __instance)
    {
        // コードは、元のメソッドの前に実行されます。これをエントリポイントとして使ったり、何かを変更したりすることができます。
    }

    [HarmonyPostfix]
    public static void Postfix(ResourcesPrefabManager __instance)
    {
        //コードは、元のメソッドの後で実行されます。メソッドの結果に対して何かを行いたい場合に、これを使うことができます。
    }
}
 * */
/*
 * 
class ZonePatch : EClass {
	static void Postfix(Zone __instance) {
		_map.TrySmoothPick(pc.pos, ThingGen.Create("chicken_dagger"), pc);
		_map.TrySmoothPick(pc.pos, ThingGen.Create("chicken_well"), pc);
		_zone.AddCard(CharaGen.Create("chickchicken"), pc.pos.GetNearestPoint(false, false));
		_zone.AddCard(CharaGen.Create("chickchicken2"), pc.pos.GetNearestPoint(false, false));
	}
}
 */


