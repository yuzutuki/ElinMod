using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

//
//メモ書き多めかも
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
		ModUtil.ImportExcel(excel, "CharaText", sources.charaText);
	}
}
//
[HarmonyPatch(typeof(ZoneEventDefenseGame), "NextWave")]
public class yzSE_postSpawn : BaseUnityPlugin
{
	static public int eventPoint;
	static public int allyPoint;
	static public string recentEvent;
    [HarmonyPostfix]
    static void Postfix(ZoneEventDefenseGame __instance)
	{
		int popLv = 1;//生成レベル
		//イベント発生ポイント初期設定
        if (eventPoint == 0)
        {
			eventPoint = 1000 - (EClass.rnd(200)+(EClass._zone.DangerLv * 2).ClampMax(500));
		}
		if (allyPoint == 0)
		{
			allyPoint = EClass.rnd(500) + 500;
		}
		//
		eventPoint = ((int)(eventPoint * 0.9f) - (EClass.rnd(400 + (EClass._zone.DangerLv*2)) + EClass._zone.DangerLv));
		//eventPoint = -1;//!!デバッグ用!!
		//Debug.Log($"eventpoint:{eventPoint}");
		if (eventPoint <= 0)
		{
			//群れデータ処理
			eventPoint = 1000 - (EClass._zone.DangerLv*2).ClampMax(500);
			swarmSpawn(yz_SwarmDictionary.getEvent());
			//援軍処理
			allyPoint = allyPoint - (EClass.rnd(800) + 200);
			if (allyPoint <= 0)
			{
				//Debug.Log("--DicCheck?--");
				//yz_SwarmDictionary.getEvent("Ally");
				allyPoint = 1000;
				swarmSpawn(yz_SwarmDictionary.getEvent("Ally"));
            }
		}
		//
		//群れ生成処理
		void swarmSpawn(yz_SeStruct eventData)
		{//効果音とついでに報酬増加処理
			//ダミーデータを拾ってしまった時用
			if(eventData.name == "dummy")
			{
				Msg.SetColor(Msg.colors.MutateGood);
				Msg.SayRaw("？　何か起こる気がしたが、気のせいだった");
				Debug.Log("===重みデータ取得エラー？===");
				return ;
			}
			//援軍生成時に直近のイベントが特定のものだったら強制上書き
			if (eventData.type == yz_ssType.ally)
			{
                switch (recentEvent)
                {
					case("cyakuza"):
						eventData = yz_SwarmDictionary.eventDic["a_njslyr"];
						break;
					case ("komodoensis"):
					case ("list_drush"):
						eventData = yz_SwarmDictionary.eventDic["a_d51"];
						break;
					case ("sister"):
					case ("fanatic"):
						eventData = yz_SwarmDictionary.eventDic["a_sis"];
						break;
                }
				//
				__instance.bonus += 1;
				EClass.Sound.Play("good");
				Msg.SetColor(Msg.colors.MutateGood);
			}
			else
			{
				__instance.bonus += 3;
				EClass.Sound.Play("spell_earthquake");
				Msg.SetColor(Msg.colors.Ding);
			}
			//メッセージ出力
			Msg.SayRaw(eventData.mes);
			recentEvent = eventData.name;
			//レベルの設定、倍率0ならPCレベルor危険度
			if (eventData.lvFixM != 0)
			{
				popLv = (int)Mathf.Max(((float)EClass._zone.DangerLv * eventData.lvFixM) + eventData.lvFixP, 5);
			}
			else
			{
				popLv = (int)Mathf.Max(EClass.pc.LV + eventData.lvFixP, EClass._zone.DangerLv);
			}
			string popName = eventData.name;
			Point spawnPoint = EClass._map.bounds.GetRandomEdge(3);//マップ端のランダムなpointを取得(距離？)
			if (eventData.type == yz_ssType.ally)//pop座標をプレイヤー近辺に上書き
				spawnPoint = EClass.pc.pos.GetRandomNeighbor();
			//
			for (int i = 0; i < eventData.count; i++)
			{
				//スキャッターならマップ端座標を再取得
				if (eventData.type == yz_ssType.scatter)
					spawnPoint = EClass._map.bounds.GetRandomEdge(3);
                //混成編成の取得
                if (eventData.name.StartsWith("list_"))
					popName = yz_SwarmDictionary.mixSpawnList[eventData.name].RandomItem<string>();
				//PT系の取得
				if(eventData.name.StartsWith("pt_"))
					popName = yz_SwarmDictionary.allyPT[eventData.name][i];
				//キャラクターの情報のなにがし
				Chara chara = CharaGen.Create(popName, popLv);
				chara.hostility = (chara.c_originalHostility = Hostility.Enemy);
				//援軍系の処理、関係性の上書きとか
				if (eventData.type == yz_ssType.ally)
				{
					chara.hostility = (chara.c_originalHostility = Hostility.Ally);
					chara.c_minionType = MinionType.Default;
					chara.c_uidMaster = EClass.pc.uid;
					chara.master = EClass.pc;
					chara.homeZone = EClass._zone;
					chara.qualityTier = 1;
					chara.ChangeRarity(Rarity.Superior);
				}
				//生成前その他固有の処理
				switch (eventData.name)
				{
					case ("doppel"):
						__instance.bonus += 4;
						chara.qualityTier = 2;
						chara.ChangeRarity(Rarity.Legendary);
						chara.ChangeRace(EClass.pc.race.name);
						break;

				}
				//キャラクターの生成
				chara.SetLv(popLv);
				Point nearestPoint = spawnPoint.GetNearestPoint(false, false, true, false);
				EClass._zone.AddCard(chara, nearestPoint);
				//生成後の調整
				if (eventData.type == yz_ssType.ally)
				{
					chara.qualityTier = 1;
				}
				//生成後その他固有の処理
				switch (eventData.name)
				{
					case ("doppel"):
						chara.SetLv(EClass.pc.LV);
						chara = statCopy(chara);
						break;
					case ("sheep"):
					case ("chicken"):
						chara.ability.Add(6450, 80, false);//突進
						break;
					case ("sister")://妹援軍のリネーム
						if (eventData.type == yz_ssType.ally)
							chara.c_altName = $"{EClass.pc.c_altName}の狂信者";
						break;
				}
			}

		}
		//
		//以下ドッペル用の処理
		Chara statCopy(Chara chara)
        {
			Chara c = chara;
			//EClass.pc.elements.CopyTo(chara.elements);
			foreach (KeyValuePair<int, Element> keyValuePair in EClass.pc.elements.dict)
			{
				Element element = c.elements.GetElement(keyValuePair.Key);
				if (element != null)
				{
					element.vBase = keyValuePair.Value.ValueWithoutLink - element.vSource;
				}
			}
			c.c_altName = $"{EClass.pc.Aka}「{EClass.pc.c_altName}」";
			c.bio = EClass.pc.bio;
			c._hobbies = EClass.pc._hobbies;
			c._works = EClass.pc._works;
			c._job = EClass.pc.job;
			c.SetFaith(EClass.pc.faith);
			c.elements.SetTo(85, 1);//信仰ポイントを１に
			c._tactics = EClass.pc.tactics;
			c.hp = chara.MaxHP;
			c.isCopy = true;
			//
			c.ability.Add(6622, 20, false);//変異まなざし習得
			//戦術からスキル設定
			switch (c.tactics.source.id)
            {
				case ("archer"):
				case ("gunner"):
					c.ability.Add(5003, 80, false);//射撃？
					break;
				case ("warrior"):
				case ("tank"):
				case ("paladin"):
				case ("thief"):
				case ("predator"):
					c.ability.Add(6450, 80, false);//突進
					break;
				case ("wizard"):
				case ("warmage"):
				case ("hexr"):
				case ("priest"):
				case ("summoner"):
				//得意領域をみて魔法習得
					foreach (int i in EClass.player.domains)
					{
						foreach(string s in Element.Get(i).tag)
						{
							Debug.Log($"{Element.Get(i).name} in tag {s}");
                            switch (s)
                            {
								case ("hand"):
									c.ability.Add(50000 + (i - 910) +400, 90, false);
									break;
								case ("arrow"):
									c.ability.Add(50000 + (i - 910) + 500, 60, false);
									break;
								case ("bolt"):
									c.ability.Add(50000 + (i - 910) + 300, 40, false);
									break;
								case ("ball"):
									c.ability.Add(50000 + (i - 910) + 100, 10, false);
									break;
								case ("funnel"):
									c.ability.Add(50000 + (i - 910) + 600, 5, false);
									break;
								case ("miasma"):
									c.ability.Add(50000 + (i - 910) + 700, 5, false);
									break;
								default:
									break;
							}
                        }
					}
					//
					break;
			}
			//装備の生成
			dpEq(ref c);
			return (c);
        }
		//
		//関数分ける必要あんまりなかったかも
		//.Equipで無理やり装備させてるけどEQ_CATとかEQ_IDあたりのほうがよかったかもしれない
		//一括で装備を生成する処理もどこかにありそうな気もする
		void dpEq(ref Chara c)
		{
			//メインハンド武器の生成
			c.body.Equip(CreFavWp(c), c.body.slotMainHand, false);
			//それっぽい防具の生成
			switch (EClass.pc.GetFavArmorSkill().id)
			{
				case (120):
					Debug.Log("軽装備");
					c.body.Equip(ThingGen.Create(new List<string> { "hat_wizard", "hat_feather" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "cloak_light", "cloak" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "armor_coat", "robe_pope" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "gloves_light", "gloves" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "girdle", "girdle_composite" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "shoes", "boots_" }.RandomItem(), -1, c.LV), null, false);
					break;
				case (121):
					Debug.Log("中装備？（未使用）");
					break;
				case (122):
					Debug.Log("重装備");
					c.body.Equip(ThingGen.Create(new List<string> { "helm_heavy", "helm_knight" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "cloak_foreign", "cloak_armored" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "armor_chain", "armor_foreign" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "gloves_plate", "gloves_composite" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "girdle_plate", "girdle_composite" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "boots_armored", "boots_heavy" }.RandomItem(), -1, c.LV), null, false);
					break;
				default:
					Debug.Log("得意防具不明");
					break;
			}
            //オフハンドの生成したりしなかったり
            switch (EClass.pc.GetFavAttackStyle())
            {
				case (AttackStyle.TwoWield):
					c.body.Equip(CreFavWp(c), c.body.slotOffHand, false);
					break;
				case (AttackStyle.Shield):
					c.body.Equip(ThingGen.Create(new List<string> { "shield_knight", "shield_composite" }.RandomItem(), -1, c.LV), c.body.slotOffHand, false);
					break;
				case (AttackStyle.TwoHand):
				case (AttackStyle.Default):
				default:
					break;
            }
			//遠隔武器の生成 EQ_CATも使ってみる
			int favRangedId = EClass.pc.elements.ListElements((Element e) => e.id == 104 || e.id == 105 || e.id == 108 || e.id == 109 , null).FindMax((Element a) => a.Value).id;
            switch (favRangedId)
            {
				case (104)://弓
					c.EQ_CAT("bow");
					c.AddThing(ThingGen.Create("arrow").SetNum(20), true);
					break;
				case (105)://銃
					c.EQ_CAT("gun");
					c.AddThing(ThingGen.Create("bullet").SetNum(20), true);
					break;
				case (108)://投擲
					c.AddThing("boomerang",c.LV);
					break;
				case (109)://クロスボウ
					c.EQ_CAT("crossbow");
					c.AddThing(ThingGen.Create("quarrel").SetNum(20), true);
					break;
				default:
					Debug.Log("得意遠隔不明");
					break;

			}
			//
			Thing CreFavWp(Chara c)
            {
				Thing weapon = ThingGen.Create("dagger", -1, c.LV);
				int favWeapon = EClass.pc.GetFavWeaponSkill().id;
				//Debug.Log(favWeapon);//GetFavWeaponSkillは遠隔スキルを取得しない　たぶん
				switch (favWeapon)
				{
					case (100)://格闘
						weapon = ThingGen.Create(new List<string> {"martial_claw", "martial_glove" }.RandomItem(), -1, c.LV);
						break;
					case (101)://長剣
						weapon = ThingGen.Create(new List<string> {"sword", "sword_katana", "sword_claymore" }.RandomItem(), -1, c.LV);
						break; 
					case (102)://斧
						weapon = ThingGen.Create(new List<string> {"axe_bardish", "axe_battle", "axe_hand", "axe_machine" }.RandomItem(), -1, c.LV);
						break; 
					case (103)://杖
						weapon = ThingGen.Create(new List<string> {"staff_long", "staff"}.RandomItem(), -1, c.LV);
						break;
					case (106)://槍
						weapon = ThingGen.Create(new List<string> {"pole_halberd", "pole_trident", "spear"}.RandomItem(), -1, c.LV);
						break;
					case (104)://x弓　遠隔が選ばれたら短剣を持たせとく
					case (105)://x銃
					case (108)://投擲
					case (109)://x弩
					case (107)://短剣
						weapon = ThingGen.Create(new List<string> { "dagger_hocho", "dagger_ninto", "dagger_pirate", "dagger" }.RandomItem(), -1, c.LV);
						break; 
					case (110)://鎌
						weapon = ThingGen.Create(new List<string> { "scythe", "scythe_sickle"}.RandomItem(), -1, c.LV);
						break; 
					case (111)://鈍器
						weapon = ThingGen.Create(new List<string> {"blunt_hammer", "blunt_club", "blunt_mace"}.RandomItem(), -1, c.LV);
						break; 
					default:
						//Debug.Log("得意武器不明？");
						break;
				}
				return (weapon);
            }
		}
	}
}

/*装備スキルIDメモ
100	martial	格闘
101	weaponSword	長剣
102	weaponAxe	斧
103	weaponStaff	杖
104	weaponBow	弓
105	weaponGun	銃
106	weaponPolearm	槍
107	weaponDagger	短剣
108	throwing	投擲
109	weaponCrossbow	クロスボウ
110	weaponScythe	鎌
111	weaponBlunt	鈍器
120	armorLight	軽装備
122	armorHeavy	重装備
 */
/*
* 属性IDメモ
*910	eleFire
*911	eleCold
*912	eleLightning
*913	eleDarkness
*914	eleMind
*915	elePoison
*916	eleNether
*917	eleSound
*918	eleNerve
*919	eleHoly
*920	eleChaos
*921	eleMagic
*922	eleEther
*923	eleAcid
*924	eleCut
*925	eleImpact
* 属性魔法のIDのメモ
* 50000が基準
* 1，2桁目が属性、上記属性リストから910引いた値(0炎、1氷,2雷…)
* 3桁目が種類、1球、2吐息、3光線、4手、5矢、6具象、7瘴気、8唄、9海
* 4桁目は未使用、種類が追加されたときとかに使いそう？
* 混沌の光線なら10と300で50310になるといった感じ
* 衝撃属性(eleImpact)はやや用途が特殊なため、使わないほうが無難かも
*/