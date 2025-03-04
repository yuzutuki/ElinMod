using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_Petrifaction
{
    internal class ActPetrifaction : Spell
    {
        public override bool Perform()
        {
            //pc.Say("yz_PetriMes");
            //debug.Log($"IsPC:{TC.IsPC},IsPCC:{TC.IsPCC},IsParty:{TC.IsPCParty}");
            //�����Ώۏ���
            if(TC.Tiles==null || TC.IsPC || TC.IsPCC ||
                TC == null || TC.HasCondition<ConInvulnerable>() || !TC.ExistsOnMap)
            {
                TC.PlaySound("fizzle", 1f, true);
                CC.Say("yz_PetriFail1");
                return true;
            }
            //��R�����A�ǐ����c
            float power = Act.CC.elements.GetOrCreateElement(base.source.id).GetPower(Act.CC);
            power *= Mathf.Max((CC.LV - (TC.LV * (TC.IsPowerful ? 2 : 1))) * 0.02f +1, 0.01f);//LV��*2���̕␳,�w�x�Ȃ�LV2�{�v�Z
            Debug.Log($"SpellPow:{power},Sqrt:{Mathf.Sqrt((float)power)},clv:{CC.LV},tlv:{TC.LV},par:{(TC.MaxHP * 0.1f) + Mathf.Sqrt((float)power * 0.33f)}");
            if (TC.hp - (power*0.05f) > Mathf.Min((TC.MaxHP*0.1f)+Mathf.Sqrt((float)power * 0.33f),TC.MaxHP*0.20f)||
                (TC.LV*4 > CC.LV && TC.hp > TC.MaxHP*0.50f))
            {
                TC.PlaySound("fizzle", 1f, true);
                CC.Say("yz_PetriFail2",TC.Name);
                return true;
            }
            //
            Thing thing = new Thing();//�����A�C�e���f�[�^�̃��t���b�V��
            int mat = 3;
            switch (this.ID)
            {
                //������
                case ("ActPetrifaction"):
                case ("ActMidasMagic"):
                case ("ActFreezing"):
                case ("ActSiliconization"):
                case ("ActCardJunction"):
                case ("ActTimeRend"):
                    if (this.ID == "ActPetrifaction")
                        mat = 3;
                    if (this.ID == "ActMidasMagic")
                       mat = 12;
                    if (this.ID == "ActFreezing")
                        mat = 61;
                    if (this.ID == "ActSiliconization")
                        mat = 9;
                    if(this.ID== "ActCardJunction")
                        thing = ThingGen.Create("figure3", -1, -1);
                    else if (this.ID == "ActTimeRend")
                        thing = ThingGen.Create("figure", 94, -1);
                    else
                       thing = ThingGen.Create("figure2", mat, -1);
                    thing.MakeFigureFrom(TC.id);//�����f�[�^�Z�b�g
                    if (thing.source._tiles.Length > 1)//���ʌ���ID���炵
                        if (TC.bio.gender == 1)
                            thing.refVal = 1;
                    if (this.ID == "ActSiliconization")//���F�Əd�ʕύX
                    {
                        thing.ChangeWeight(500);
                        thing.Dye(new List<string> { "jelly", "meat", "spidersilk", "ether", "steel", "scale", "aquamarine", "copper" }.RandomItem<string>());
                    }
                    if (this.ID == "ActTimeRend")//�A�C�e�����̕ύX
                        thing.c_altName = $"���Ԓ�~����{TC.Name}";
                    if(thing.id == "figure"||thing.id == "figure2")//�J�[�h�ȊO���Ƌ�Ƃ��Đݒu
                        EClass._zone.AddCard(thing, Act.TP).Install();
                    else
                        EClass._zone.TryAddThing(thing, Act.TP);
                    //Debug.Log($"alt1:{thing.c_altName},alt2:{thing.c_altName2},ExRef:{thing.c_extraNameRef},IDRef:{thing.c_idRefName},name:{thing.Name}");
                    break;
                case ("ActLiquefaction"):
                    //��`�q���o
                    thing = TC.Chara.MakeGene((EClass.rnd(5) == 0) ? new DNA.Type?(DNA.Type.Superior) : null);
                    EClass._zone.TryAddThing(thing, TC.pos, false);
                    //�݂邭
                    thing = ThingGen.Create("milk", -1, -1).SetNum(EClass.rnd(3)+1);
                    thing.MakeRefFrom(TC, null);
                    EClass._zone.TryAddThing(thing, TC.pos, false);
                    break;
            }
            //
            //Debug.Log($"TP:{TP},{TC.pos},TC:{TC.Name},refVal:{TC.refVal},CastID:{this.id},{this.ID},targetGebder:{TC.bio.gender}");
            //Debug.Log($"{TC.uid},{TC.idSkin},{TC.refVal},tMat:{TC.material.id},{TC.material.name}");
            //Debug.Log($"Length:{thing.source._tiles.Length},refVal:{thing.refVal}");
            //�G�t�F�N�g�Ƒ�������
            pc.Say($"yz_{this.ID}", TC.Name);
            //thing.PlayEffect("debuff", true, 0f, default(Vector3));
            CC.PlaySound("identify", 1f, true);
            CC.PlaySound("hit_finish", 1f, true);
            TC.Chara.mana.value = 0;
            TC.hp = 0;
            TC.DamageHP(TC.MaxHP+1, AttackSource.Finish, CC);
            //
            return true;
            
        }
    }
}