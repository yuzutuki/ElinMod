using System.Collections;
using System.Collections.Generic;
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
        {"dumyy" ,new yz_SeStruct("test_chicken",0,yz_ssType.center,1,"�_�~�[�f�[�^����I�d��0������o�Ȃ��͂�����!",0,1) },
        {"e_cultist",new yz_SeStruct("fanatic",100,yz_ssType.chunk,28,"���������̐M�҂��W�܂��Ă��Ă���I ",-10,0.7f)},
        {"e_komod",new yz_SeStruct("komodoensis",80,yz_ssType.chunk,20,"���ɃR���h�h���S�����͂ȂĂ� ",-10,1 )},
        {"e_chicken",new yz_SeStruct("chicken",100,yz_ssType.scatter,40,"���Q�𐾂��j���g���̌Q�ꂪ���ꂽ�c�I ",5,1.2f )},
        {"e_sheep",new yz_SeStruct("sheep",100,yz_ssType.chunk,20,"�{���̗r�����������ꂽ�I�I ",0,1)},
        {"e_giant",new yz_SeStruct("giant",30,yz_ssType.chunk,20,"��n��炷���l�̏W�c���c�I ",-5,1)},
        {"e_imouto",new yz_SeStruct("sister",100,yz_ssType.chunk,15,"�쐶�̂��Ȃ��̖��������P���|�����Ă����I ",0,1)},
        {"e_puti",new yz_SeStruct("putty",100,yz_ssType.chunk,12,"���킢�炵���v�`�̌Q�ꂾ�c�B ",-100,0.1f) },
        {"e_bell",new yz_SeStruct("bell_silver",10,yz_ssType.chunk,8,"�x���̉��F����������c ",-100,0.1f) },
        {"e_zombi",new yz_SeStruct("zombie",100,yz_ssType.scatter,80,"*���c��̉����񂹂Ă��鉹�c* ",-10,0.5f) },
        {"e_kamikaze",new yz_SeStruct("yeek_kamikaze",30,yz_ssType.chunk,24,"�L�P���ȉΖ�̓���������c�I ",-10,0.5f) },
        {"e_tentacle",new yz_SeStruct("tentacle",30,yz_ssType.scatter,20,"�˂���˂����忂���������c ",-10,1) },
        {"e_saccu",new yz_SeStruct("nun_mother",80,yz_ssType.scatter,12,"�Ȃ񂾂��h�L�h�L���鍁�肪�Y���Ă����c ",5,1) },
        {"e_kiria",new yz_SeStruct("kiria_fake",50,yz_ssType.chunk,12,"�@�B�����̃��v���J�������P���Ă����I ",2,1) },
        //
        {"e_doppel",new yz_SeStruct("doppel",30,yz_ssType.scatter,1,"���Ȃ��̉e�����Ȃ��������ɂ����c�I ",0,0) },
        //
        {"l_dogs",new yz_SeStruct("dogs",100,yz_ssType.chunk,16,"���\�Ȍ��̌Q�ꂾ�c�I ",-3,0.8f) },
        {"l_birds",new yz_SeStruct("birds",100,yz_ssType.chunk,24,"���C�ɋ��ꂽ���̑�Q���I ",-3,0.8f) },
        {"l_kanis",new yz_SeStruct("kanis",100,yz_ssType.chunk,24,"��̓���������c�J�j�̑�Q���c ",-5,0.8f) },
        //
        {"a_claymore",new yz_SeStruct("silvereye",50,yz_ssType.center,1,"�ǂ�����Ƃ��Ȃ����̎a�E�҂����R�Ɍ��ꂽ�I ",10,0) },
        {"a_ojou",new yz_SeStruct("younglady",80,yz_ssType.center,1,"�ǂ�����Ƃ��Ȃ�����l�����R�Ɍ��ꂽ�I ",5,0)},
        {"a_meat",new yz_SeStruct("gwen",20,yz_ssType.center,1,"���׋C�ȏ��������ɖ������񂾁c ",0,0.5f)},
        {"a_nerun",new yz_SeStruct("nerun",30,yz_ssType.center,1,"���b�Ă��ȃK�C�h����`���Ă����悤�� ",0,0)}
    };
    //���������̓���
    static public List<string> eList_dogs = new List<string> { "dog", "dog_wild", "hound", "dog_shiba", "dog_shiva", "silverwolf", "kobolt" };
    static public List<string> eList_birds = new List<string> { "chicken", "duck", "goose", "turkey", "caladrius" };
    static public List<string> eList_kanis = new List<string> { "cancer", "hermitcrab", "crab_shiba", "kingcrab"};
    //
    //�d�݂Ɨ��������ă����_���ȍ��ڂ̎擾
    static public yz_SeStruct getEvent()
    {
        //������
        if (totalWeight == 0)
            foreach (yz_SeStruct sc in eventDic.Values)
            {
                totalWeight += sc.weight;
            }
        //
        int r = EClass.rnd(totalWeight);
//        Debug.Log($"total{totalWeight},rnd{r}");
        foreach (KeyValuePair<string, yz_SeStruct> ev in eventDic)
        {
            if (r < ev.Value.weight&&r>0)
                return (ev.Value);
            r -= ev.Value.weight;
        }
        return (eventDic["e_cultist"]);
    }
}
public enum yz_ssType//SwarmSpawnType
{
    center,//�}�b�v�����i���R�j
    chunk,//�ł܂��ďo��
    scatter//�[�Ƀ����_����
}