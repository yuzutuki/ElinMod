using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//イベントデータの構造体
public struct yz_SeStruct//SwarmEventStruct
{
    public string name;//charaCardを参照
    public int weight;//おもみ
    public string mes;//メッセージ
    public yz_ssType type;//出現タイプ
    public int count;//出現数
    public int lvFixP;//レベルへの加算減算補正
    public float lvFixM;//レベルへの乗算補正、0でプレイヤーレベル

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
    //重みデータ
    static public int totalWeight = 0;
    //イベントの辞書データ
    static public Dictionary<string, yz_SeStruct> eventDic = new Dictionary<string, yz_SeStruct> {
        {"dumyy" ,new yz_SeStruct("test_chicken",0,yz_ssType.center,1,"ダミーデータだよ！重み0だから出ないはずだよ!",0,1) },
        {"e_cultist",new yz_SeStruct("fanatic",100,yz_ssType.chunk,28,"すごい数の信者が集まってきている！ ",-10,0.7f)},
        {"e_komod",new yz_SeStruct("komodoensis",80,yz_ssType.chunk,20,"戦場にコモドドラゴンをはなてっ ",-10,1 )},
        {"e_chicken",new yz_SeStruct("chicken",100,yz_ssType.scatter,40,"復讐を誓うニワトリの群れが現れた…！ ",5,1.2f )},
        {"e_sheep",new yz_SeStruct("sheep",100,yz_ssType.chunk,20,"怒涛の羊たちがあらわれた！！ ",0,1)},
        {"e_giant",new yz_SeStruct("giant",30,yz_ssType.chunk,20,"大地を鳴らす巨人の集団だ…！ ",-5,1)},
        {"e_imouto",new yz_SeStruct("sister",100,yz_ssType.chunk,15,"野生のあなたの妹たちが襲い掛かってきた！ ",0,1)},
        {"e_puti",new yz_SeStruct("putty",100,yz_ssType.chunk,12,"かわいらしいプチの群れだ…。 ",-100,0.1f) },
        {"e_bell",new yz_SeStruct("bell_silver",10,yz_ssType.chunk,8,"ベルの音色が聞こえる… ",-100,0.1f) },
        {"e_zombi",new yz_SeStruct("zombie",100,yz_ssType.scatter,80,"*ヤツらの押し寄せてくる音…* ",-10,0.5f) },
        {"e_kamikaze",new yz_SeStruct("yeek_kamikaze",30,yz_ssType.chunk,24,"キケンな火薬の匂いがする…！ ",-10,0.5f) },
        {"e_tentacle",new yz_SeStruct("tentacle",30,yz_ssType.scatter,20,"ねちょねちょと蠢く音がする… ",-10,1) },
        {"e_saccu",new yz_SeStruct("nun_mother",80,yz_ssType.scatter,12,"なんだかドキドキする香りが漂ってきた… ",5,1) },
        {"e_kiria",new yz_SeStruct("kiria_fake",50,yz_ssType.chunk,12,"機械少女のレプリカたちが襲ってきた！ ",2,1) },
        //
        {"e_doppel",new yz_SeStruct("doppel",30,yz_ssType.scatter,1,"あなたの影があなたを消しにきた…！ ",0,0) },
        //
        {"l_dogs",new yz_SeStruct("dogs",100,yz_ssType.chunk,16,"凶暴な犬の群れだ…！ ",-3,0.8f) },
        {"l_birds",new yz_SeStruct("birds",100,yz_ssType.chunk,24,"狂気に駆られた鳥の大群だ！ ",-3,0.8f) },
        {"l_kanis",new yz_SeStruct("kanis",100,yz_ssType.chunk,24,"磯の匂いがする…カニの大群だ… ",-5,0.8f) },
        //
        {"a_claymore",new yz_SeStruct("silvereye",50,yz_ssType.center,1,"どこからともなく銀眼の斬殺者が援軍に現れた！ ",10,0) },
        {"a_ojou",new yz_SeStruct("younglady",80,yz_ssType.center,1,"どこからともなくお嬢様が援軍に現れた！ ",5,0)},
        {"a_meat",new yz_SeStruct("gwen",20,yz_ssType.center,1,"無邪気な少女が戦場に迷い込んだ… ",0,0.5f)},
        {"a_nerun",new yz_SeStruct("nerun",30,yz_ssType.center,1,"世話焼きなガイドが手伝ってくれるようだ ",0,0)}
    };
    //混合部隊の内訳
    static public List<string> eList_dogs = new List<string> { "dog", "dog_wild", "hound", "dog_shiba", "dog_shiva", "silverwolf", "kobolt" };
    static public List<string> eList_birds = new List<string> { "chicken", "duck", "goose", "turkey", "caladrius" };
    static public List<string> eList_kanis = new List<string> { "cancer", "hermitcrab", "crab_shiba", "kingcrab"};
    //
    //重みと乱数を見てランダムな項目の取得
    static public yz_SeStruct getEvent()
    {
        //初期化
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
    center,//マップ中央（援軍）
    chunk,//固まって出現
    scatter//端にランダムに
}