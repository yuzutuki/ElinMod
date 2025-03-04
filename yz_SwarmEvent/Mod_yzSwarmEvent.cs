using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

//
//�����������߂���
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
		int popLv = 1;//�������x��
		//�C�x���g�����|�C���g�����ݒ�
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
		//eventPoint = -1;//!!�f�o�b�O�p!!
		//Debug.Log($"eventpoint:{eventPoint}");
		if (eventPoint <= 0)
		{
			//�Q��f�[�^����
			eventPoint = 1000 - (EClass._zone.DangerLv*2).ClampMax(500);
			swarmSpawn(yz_SwarmDictionary.getEvent());
			//���R����
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
		//�Q�ꐶ������
		void swarmSpawn(yz_SeStruct eventData)
		{//���ʉ��Ƃ��łɕ�V��������
			//�_�~�[�f�[�^���E���Ă��܂������p
			if(eventData.name == "dummy")
			{
				Msg.SetColor(Msg.colors.MutateGood);
				Msg.SayRaw("�H�@�����N����C���������A�C�̂���������");
				Debug.Log("===�d�݃f�[�^�擾�G���[�H===");
				return ;
			}
			//���R�������ɒ��߂̃C�x���g������̂��̂������狭���㏑��
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
			//���b�Z�[�W�o��
			Msg.SayRaw(eventData.mes);
			recentEvent = eventData.name;
			//���x���̐ݒ�A�{��0�Ȃ�PC���x��or�댯�x
			if (eventData.lvFixM != 0)
			{
				popLv = (int)Mathf.Max(((float)EClass._zone.DangerLv * eventData.lvFixM) + eventData.lvFixP, 5);
			}
			else
			{
				popLv = (int)Mathf.Max(EClass.pc.LV + eventData.lvFixP, EClass._zone.DangerLv);
			}
			string popName = eventData.name;
			Point spawnPoint = EClass._map.bounds.GetRandomEdge(3);//�}�b�v�[�̃����_����point���擾(�����H)
			if (eventData.type == yz_ssType.ally)//pop���W���v���C���[�ߕӂɏ㏑��
				spawnPoint = EClass.pc.pos.GetRandomNeighbor();
			//
			for (int i = 0; i < eventData.count; i++)
			{
				//�X�L���b�^�[�Ȃ�}�b�v�[���W���Ď擾
				if (eventData.type == yz_ssType.scatter)
					spawnPoint = EClass._map.bounds.GetRandomEdge(3);
                //�����Ґ��̎擾
                if (eventData.name.StartsWith("list_"))
					popName = yz_SwarmDictionary.mixSpawnList[eventData.name].RandomItem<string>();
				//PT�n�̎擾
				if(eventData.name.StartsWith("pt_"))
					popName = yz_SwarmDictionary.allyPT[eventData.name][i];
				//�L�����N�^�[�̏��̂Ȃɂ���
				Chara chara = CharaGen.Create(popName, popLv);
				chara.hostility = (chara.c_originalHostility = Hostility.Enemy);
				//���R�n�̏����A�֌W���̏㏑���Ƃ�
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
				//�����O���̑��ŗL�̏���
				switch (eventData.name)
				{
					case ("doppel"):
						__instance.bonus += 4;
						chara.qualityTier = 2;
						chara.ChangeRarity(Rarity.Legendary);
						chara.ChangeRace(EClass.pc.race.name);
						break;

				}
				//�L�����N�^�[�̐���
				chara.SetLv(popLv);
				Point nearestPoint = spawnPoint.GetNearestPoint(false, false, true, false);
				EClass._zone.AddCard(chara, nearestPoint);
				//������̒���
				if (eventData.type == yz_ssType.ally)
				{
					chara.qualityTier = 1;
				}
				//�����セ�̑��ŗL�̏���
				switch (eventData.name)
				{
					case ("doppel"):
						chara.SetLv(EClass.pc.LV);
						chara = statCopy(chara);
						break;
					case ("sheep"):
					case ("chicken"):
						chara.ability.Add(6450, 80, false);//�ːi
						break;
					case ("sister")://�����R�̃��l�[��
						if (eventData.type == yz_ssType.ally)
							chara.c_altName = $"{EClass.pc.c_altName}�̋��M��";
						break;
				}
			}

		}
		//
		//�ȉ��h�b�y���p�̏���
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
			c.c_altName = $"{EClass.pc.Aka}�u{EClass.pc.c_altName}�v";
			c.bio = EClass.pc.bio;
			c._hobbies = EClass.pc._hobbies;
			c._works = EClass.pc._works;
			c._job = EClass.pc.job;
			c.SetFaith(EClass.pc.faith);
			c.elements.SetTo(85, 1);//�M�|�C���g���P��
			c._tactics = EClass.pc.tactics;
			c.hp = chara.MaxHP;
			c.isCopy = true;
			//
			c.ability.Add(6622, 20, false);//�ψق܂Ȃ����K��
			//��p����X�L���ݒ�
			switch (c.tactics.source.id)
            {
				case ("archer"):
				case ("gunner"):
					c.ability.Add(5003, 80, false);//�ˌ��H
					break;
				case ("warrior"):
				case ("tank"):
				case ("paladin"):
				case ("thief"):
				case ("predator"):
					c.ability.Add(6450, 80, false);//�ːi
					break;
				case ("wizard"):
				case ("warmage"):
				case ("hexr"):
				case ("priest"):
				case ("summoner"):
				//���ӗ̈���݂Ė��@�K��
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
			//�����̐���
			dpEq(ref c);
			return (c);
        }
		//
		//�֐�������K�v����܂�Ȃ���������
		//.Equip�Ŗ�����葕�������Ă邯��EQ_CAT�Ƃ�EQ_ID������̂ق����悩������������Ȃ�
		//�ꊇ�ő����𐶐����鏈�����ǂ����ɂ��肻���ȋC������
		void dpEq(ref Chara c)
		{
			//���C���n���h����̐���
			c.body.Equip(CreFavWp(c), c.body.slotMainHand, false);
			//������ۂ��h��̐���
			switch (EClass.pc.GetFavArmorSkill().id)
			{
				case (120):
					Debug.Log("�y����");
					c.body.Equip(ThingGen.Create(new List<string> { "hat_wizard", "hat_feather" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "cloak_light", "cloak" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "armor_coat", "robe_pope" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "gloves_light", "gloves" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "girdle", "girdle_composite" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "shoes", "boots_" }.RandomItem(), -1, c.LV), null, false);
					break;
				case (121):
					Debug.Log("�������H�i���g�p�j");
					break;
				case (122):
					Debug.Log("�d����");
					c.body.Equip(ThingGen.Create(new List<string> { "helm_heavy", "helm_knight" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "cloak_foreign", "cloak_armored" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "armor_chain", "armor_foreign" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "gloves_plate", "gloves_composite" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "girdle_plate", "girdle_composite" }.RandomItem(), -1, c.LV), null, false);
					c.body.Equip(ThingGen.Create(new List<string> { "boots_armored", "boots_heavy" }.RandomItem(), -1, c.LV), null, false);
					break;
				default:
					Debug.Log("���Ӗh��s��");
					break;
			}
            //�I�t�n���h�̐��������肵�Ȃ�������
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
			//���u����̐��� EQ_CAT���g���Ă݂�
			int favRangedId = EClass.pc.elements.ListElements((Element e) => e.id == 104 || e.id == 105 || e.id == 108 || e.id == 109 , null).FindMax((Element a) => a.Value).id;
            switch (favRangedId)
            {
				case (104)://�|
					c.EQ_CAT("bow");
					c.AddThing(ThingGen.Create("arrow").SetNum(20), true);
					break;
				case (105)://�e
					c.EQ_CAT("gun");
					c.AddThing(ThingGen.Create("bullet").SetNum(20), true);
					break;
				case (108)://����
					c.AddThing("boomerang",c.LV);
					break;
				case (109)://�N���X�{�E
					c.EQ_CAT("crossbow");
					c.AddThing(ThingGen.Create("quarrel").SetNum(20), true);
					break;
				default:
					Debug.Log("���Ӊ��u�s��");
					break;

			}
			//
			Thing CreFavWp(Chara c)
            {
				Thing weapon = ThingGen.Create("dagger", -1, c.LV);
				int favWeapon = EClass.pc.GetFavWeaponSkill().id;
				//Debug.Log(favWeapon);//GetFavWeaponSkill�͉��u�X�L�����擾���Ȃ��@���Ԃ�
				switch (favWeapon)
				{
					case (100)://�i��
						weapon = ThingGen.Create(new List<string> {"martial_claw", "martial_glove" }.RandomItem(), -1, c.LV);
						break;
					case (101)://����
						weapon = ThingGen.Create(new List<string> {"sword", "sword_katana", "sword_claymore" }.RandomItem(), -1, c.LV);
						break; 
					case (102)://��
						weapon = ThingGen.Create(new List<string> {"axe_bardish", "axe_battle", "axe_hand", "axe_machine" }.RandomItem(), -1, c.LV);
						break; 
					case (103)://��
						weapon = ThingGen.Create(new List<string> {"staff_long", "staff"}.RandomItem(), -1, c.LV);
						break;
					case (106)://��
						weapon = ThingGen.Create(new List<string> {"pole_halberd", "pole_trident", "spear"}.RandomItem(), -1, c.LV);
						break;
					case (104)://x�|�@���u���I�΂ꂽ��Z�����������Ƃ�
					case (105)://x�e
					case (108)://����
					case (109)://x�W
					case (107)://�Z��
						weapon = ThingGen.Create(new List<string> { "dagger_hocho", "dagger_ninto", "dagger_pirate", "dagger" }.RandomItem(), -1, c.LV);
						break; 
					case (110)://��
						weapon = ThingGen.Create(new List<string> { "scythe", "scythe_sickle"}.RandomItem(), -1, c.LV);
						break; 
					case (111)://�݊�
						weapon = ThingGen.Create(new List<string> {"blunt_hammer", "blunt_club", "blunt_mace"}.RandomItem(), -1, c.LV);
						break; 
					default:
						//Debug.Log("���ӕ���s���H");
						break;
				}
				return (weapon);
            }
		}
	}
}

/*�����X�L��ID����
100	martial	�i��
101	weaponSword	����
102	weaponAxe	��
103	weaponStaff	��
104	weaponBow	�|
105	weaponGun	�e
106	weaponPolearm	��
107	weaponDagger	�Z��
108	throwing	����
109	weaponCrossbow	�N���X�{�E
110	weaponScythe	��
111	weaponBlunt	�݊�
120	armorLight	�y����
122	armorHeavy	�d����
 */
/*
* ����ID����
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
* �������@��ID�̃���
* 50000���
* 1�C2���ڂ������A��L�������X�g����910�������l(0���A1�X,2���c)
* 3���ڂ���ށA1���A2�f���A3�����A4��A5��A6��ہA7ᏋC�A8�S�A9�C
* 4���ڂ͖��g�p�A��ނ��ǉ����ꂽ�Ƃ��Ƃ��Ɏg�������H
* ���ׂ̌����Ȃ�10��300��50310�ɂȂ�Ƃ���������
* �Ռ�����(eleImpact)�͂��p�r������Ȃ��߁A�g��Ȃ��ق��������
*/