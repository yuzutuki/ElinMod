using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//�C�x���g�f�[�^�̍\����
public struct yz_SeStruct//SwarmEventStruct
{
    public string name;//charaCard���Q��
    public int weight;//������
    public string mes;//���b�Z�[�W
    public yz_ssType type;//�o���^�C�v
    public int count;//�o����
    public int lvFixP;//���x���ւ̉��Z���Z�␳
    public float lvFixM;//���x���ւ̏�Z�␳�A0�Ńv���C���[���x��

    public yz_SeStruct(string setName, int setWeight, yz_ssType setType, int setCount, string setMes, int setLvP, float setLvM)
    {
        name = setName;
        weight = setWeight;
        type = setType;
        count = setCount;
        mes = setMes;
        lvFixP = setLvP;
        lvFixM = setLvM;
    }
}
static public class yz_SwarmDictionary
{
    //�d�݃f�[�^
    static public int totalWeight = 0;
    //�C�x���g�̎����f�[�^
    static public Dictionary<string, yz_SeStruct> eventDic = new Dictionary<string, yz_SeStruct> {
        {"dummy" ,new yz_SeStruct("dummy",0,yz_ssType.chunk,1,"�_�~�[�f�[�^����I�d��0������o�Ȃ��͂�����!",0,1) },
        {"e_cultist",new yz_SeStruct("fanatic",100,yz_ssType.chunk,28,"���������̐M�҂��W�܂��Ă��Ă���I ",-10,0.7f)},
        {"e_komod",new yz_SeStruct("komodoensis",80,yz_ssType.chunk,20,"���ɃR���h�h���S�����͂ȂĂ� ",-10,1 )},
        {"e_chicken",new yz_SeStruct("chicken",100,yz_ssType.scatter,40,"���Q�𐾂��j���g���̌Q�ꂪ���ꂽ�c�I ",5,1.2f )},
        {"e_sheep",new yz_SeStruct("sheep",100,yz_ssType.chunk,20,"�{���̗r�����������ꂽ�I�I ",0,1)},
        {"e_giant",new yz_SeStruct("giant",30,yz_ssType.chunk,20,"��n��炷���l�̏W�c���c�I ",-5,1)},
        {"e_imouto",new yz_SeStruct("sister",100,yz_ssType.scatter,15,"�쐶�̂��Ȃ��̖��������P���|�����Ă����I ",0,1)},
        {"e_puti",new yz_SeStruct("putty",100,yz_ssType.chunk,12,"���킢�炵���v�`�̌Q�ꂾ�c�B ",-100,0.1f) },
        {"e_bell",new yz_SeStruct("bell_silver",10,yz_ssType.chunk,8,"�x���̉��F����������c ",-100,0.1f) },
        {"e_zombi",new yz_SeStruct("zombie",100,yz_ssType.scatter,60,"*���c��̉����񂹂Ă��鉹�c* ",-10,0.5f) },
        {"e_kamikaze",new yz_SeStruct("yeek_kamikaze",30,yz_ssType.chunk,24,"�L�P���ȉΖ�̓���������c�I ",-10,0.5f) },
        {"e_tentacle",new yz_SeStruct("tentacleB",30,yz_ssType.scatter,30,"�˂���˂����忂���������c ",-10,1) },
        {"e_saccu",new yz_SeStruct("nun_mother",80,yz_ssType.scatter,12,"�Ȃ񂾂��h�L�h�L���鍁�肪�Y���Ă����c ",5,1) },
        {"e_kiria",new yz_SeStruct("kiria_fake",50,yz_ssType.chunk,12,"�@�B�����̃��v���J�������P���Ă����I ",2,1) },
        {"e_bat",new yz_SeStruct("bat",100,yz_ssType.chunk,30,"�����҂������Ԃ����Ƒ�ʂ̃R�E���������ꂽ�I ",-5,1)},
        {"e_kobolt",new yz_SeStruct("kobolt",100,yz_ssType.chunk,30,"�����҂������Ԃ����Ƒ�ʂ̃R�{���g�����ꂽ�I ",-5,1)},
        {"e_ghost",new yz_SeStruct("ghost",100,yz_ssType.chunk,30,"�����҂������Ԃ����Ƒ�ʂ̃S�[�X�g�����ꂽ�I ",-5,1)},
        {"e_mohikan",new yz_SeStruct("punk",100,yz_ssType.chunk,30,"�q���b�n�[�ȕ��X���k�}��g��ŏP���Ă����I ",-5,0.8f)},
        {"e_toysoldier",new yz_SeStruct("toysoldier",80,yz_ssType.chunk,16,"��������̕��m�������\��Ă���I ",0,1)},
        {"e_yakuza",new yz_SeStruct("cyakuza",60,yz_ssType.chunk,16,"�Ј��I�Ȍ����ڂ̃N���[�������I�R���C�I ",-5,1)},
        {"e_kimogasa",new yz_SeStruct("kimogasa",60,yz_ssType.scatter,30,"�_���̂悤�Ȏ㏬�z���S�̏W�c���c ",-8,0.8f)},
        //
        {"e_doppel",new yz_SeStruct("doppel",20,yz_ssType.scatter,1,"���Ȃ��̉e�����Ȃ��������ɂ����c�I ",0,0) },
        //
        {"l_dogs",new yz_SeStruct("list_dogs",100,yz_ssType.chunk,16,"���\�Ȍ��̌Q�ꂾ�c�I ",-3,0.8f) },
        {"l_birds",new yz_SeStruct("list_birds",100,yz_ssType.chunk,24,"���C�ɋ��ꂽ���̑�Q���I ",-3,0.8f) },
        {"l_kanis",new yz_SeStruct("list_kanis",100,yz_ssType.chunk,24,"��̓���������c�J�j�̑�Q���c ",-5,0.8f) },
        {"l_mercs",new yz_SeStruct("list_mercs",100,yz_ssType.chunk,16,"���@�ȗb���c���P���Ă����I ",0,1) },
        {"l_drush",new yz_SeStruct("list_drush",30,yz_ssType.scatter,16,"�l�X�ȗ����W�܂�c�h���S���E���b�V�����I ",-3,0.9f) },
        //
        {"a_claymore",new yz_SeStruct("silvereye",80,yz_ssType.ally,1," ����ɁA�ǂ�����Ƃ��Ȃ����̎a�E�҂����R�Ɍ��ꂽ�I ",10,0) },
        {"a_ojou",new yz_SeStruct("younglady",100,yz_ssType.ally,1," ����ɁA�ǂ�����Ƃ��Ȃ�����l�����R�Ɍ��ꂽ�I ",5,0)},
        {"a_sis",new yz_SeStruct("sister",60,yz_ssType.ally,4," ����ɁA���Ȃ��̋��M�҂����R�Ɍ��ꂽ�I ",0,0)},
        {"a_njslyr",new yz_SeStruct("njslyr",10,yz_ssType.ally,1," ����ɁA����̗􂯖ڂ���ԍ������_�̃G���g���[���I�S�E�����K�I ",10,0)},
        {"a_d51",new yz_SeStruct("d51",10,yz_ssType.ally,1," ����ɁA�@�B�d�|���̌R�������Ȃ��̖���������悤���I ",10,0)},
        {"a_meat",new yz_SeStruct("gwen",10,yz_ssType.ally,1," ����ɁA���׋C�ȏ��������ɖ������񂾁c ",0,0.5f)},
        {"a_nerun",new yz_SeStruct("nerun",40,yz_ssType.ally,1," ����ɁA���b�Ă��ȃK�C�h����`���Ă����悤�� ",0,0)},
        {"a_fiama",new yz_SeStruct("fiama",20,yz_ssType.ally,1," ����ɁA���[���Q�[�g�̘c�݂���~�V���A�̉e�����R�Ɍ��ꂽ�I ",8,0)},
        {"a_ashland",new yz_SeStruct("ashland",20,yz_ssType.ally,1," ����ɁA���[���Q�[�g�̘c�݂���~�V���A�̉e�����R�Ɍ��ꂽ�I ",10,0)},
        {"a_mesugaki",new yz_SeStruct("adv_gaki",15,yz_ssType.ally,1," ����ɁA���[���Q�[�g�̘c�݂��琶�ӋC�ȏ��������R�Ɍ��ꂽ�I ",10,0)},
        {"a_kiria",new yz_SeStruct("adv_kiria",15,yz_ssType.ally,1," ����ɁA���[���Q�[�g�̘c�݂���@�B�l�`�̏��������R�Ɍ��ꂽ�I ",10,0)},
        {"a_ivory",new yz_SeStruct("adv_ivory",15,yz_ssType.ally,1," ����ɁA���[���Q�[�g�̘c�݂���ۉ哃�̎g�҂����R�Ɍ��ꂽ�I ",10,0)},
        {"a_mesherada",new yz_SeStruct("adv_mesherada",15,yz_ssType.ally,1,"�@����ɁA���[���Q�[�g�̘c�݂���^���̗����҂����R�Ɍ��ꂽ�I ",10,0)},
        {"a_wini",new yz_SeStruct("adv_wini",15,yz_ssType.ally,1," ����ɁA���[���Q�[�g�̘c�݂���i���̗��l�����R�Ɍ��ꂽ�I ",10,0)},
        {"a_verna",new yz_SeStruct("adv_verna",15,yz_ssType.ally,1," ����ɁA���[���Q�[�g�̘c�݂����Ȃ�̖��������R�Ɍ��ꂽ�I ",10,0)},
        {"a_eureka",new yz_SeStruct("eureka",15,yz_ssType.ally,1," ����ɁA���[���Q�[�g�̘c�݂����Ђ̑t�҂����R�Ɍ��ꂽ�I ",10,0)},
        //
        {"pt_fiash",new yz_SeStruct("pt_fiash",20,yz_ssType.ally,2," ����ɁA���[���Q�[�g�̘c�݂���~�V���A�̉e�����R�Ɍ��ꂽ�I ",10,0)},
        {"pt_ketquru",new yz_SeStruct("pt_ketquru",20,yz_ssType.ally,2," ����ɁA���[���Q�[�g�̘c�݂�����Q�̘B���p�t�����R�Ɍ��ꂽ�I ",10,0)},
        {"pt_mikomiko",new yz_SeStruct("pt_mikomiko",15,yz_ssType.ally,2," ����ɁA���[���Q�[�g�̘c�݂����l�̛ޏ������R�Ɍ��ꂽ�I ",10,0)},
        {"pt_rodoao",new yz_SeStruct("pt_rodoao",15,yz_ssType.ally,3," ����ɁA���[���Q�[�g�̘c�݂�����ʂȏ��l�H�����R�Ɍ��ꂽ�I ",10,0)},
        {"pt_pal",new yz_SeStruct("pt_pal",20,yz_ssType.ally,3," ����ɁA���[���Q�[�g�̘c�݂���p���~�A�̉p�Y�B�����R�Ɍ��ꂽ�I ",6,0)},
        {"pt_olv",new yz_SeStruct("pt_olv",20,yz_ssType.ally,2," ����ɁA���[���Q�[�g�̘c�݂���I�����B�i�̎o�������R�Ɍ��ꂽ�I ",6,0)}
        //�������@�Ȃ�ŃR���h�h���S�����̐l
        //�G���A��l
        //�g�_�[�H
        //summoning101?
    };
    //���R�pPT�f�[�^
    static public Dictionary<string, List<string>> allyPT = new Dictionary<string, List<string>>
    {
        {"pt_fiash",new List<string>{ "fiama", "ashland" } },
        {"pt_ketquru",new List<string>{ "kettle", "quru" } },
        {"pt_mikomiko",new List<string>{ "miko_nefu", "miko_mifu" } },
        {"pt_rodoao",new List<string>{ "rodwyn", "girl_blue", "merchant_inn_fox2" } },
        {"pt_pal",new List<string>{ "mapMerchant", "conery", "gilbert" } },
        {"pt_olv",new List<string>{ "farris", "theolucia" } }
    };
    //���������̓���
    static public Dictionary<string, List<string>> mixSpawnList = new Dictionary<string, List<string>>
    {
        {"list_dogs",new List<string>{ "dog", "dog_wild", "hound", "dog_shiba", "dog_shiva", "silverwolf", "kobolt" } },
        {"list_birds",new List<string>{ "chicken", "duck", "goose", "turkey", "caladrius" } },
        {"list_kanis",new List<string>{ "cancer", "hermitcrab", "crab_shiba", "kingcrab" } },
        {"list_mercs",new List<string>{ "merc", "merc_archer", "merc_warrior", "merc_mage" } },
        {"list_drush",new List<string>{ "tyrannosaurus", "mass_monster", "drake", "drake", "dragon", "lizardman", "lizardman", "wyvern", "dodo_rex" } }
    };
    //
    //�d�݂Ɨ��������ă����_���ȍ��ڂ̎擾
    static public yz_SeStruct getEvent(string targetDic=null)
    {
        //�����p�̔z��
        Dictionary<string, yz_SeStruct> td = new Dictionary<string, yz_SeStruct>();
        //���R�̂ݒ��o���邩���O
        if (targetDic == "Ally")
        {
            td = eventDic.Where(pair => pair.Key.StartsWith("a_") || pair.Key.StartsWith("pt_")).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        else
        {
           td = eventDic.Where(pair => !(pair.Key.StartsWith("a_") || pair.Key.StartsWith("pt_"))).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        //
        //������
        totalWeight = 0;
            foreach (yz_SeStruct sc in td.Values)
            {
                totalWeight += sc.weight;
            }
        //
        int r = EClass.rnd(totalWeight);
//        Debug.Log($"total{totalWeight},rnd{r}");
        foreach (KeyValuePair<string, yz_SeStruct> ev in td)
        {
            //Debug.Log($"{ev.Key}:check waight,{r}");
            if (r < ev.Value.weight)
                return (ev.Value);
            r -= ev.Value.weight;
        }
        return (eventDic["dummy"]);
    }
}
public enum yz_ssType//SwarmSpawnType
{
    ally,//�v���C���[�̎���i���R�j
    chunk,//�ł܂��ďo��
    scatter//�[�Ƀ����_����
}