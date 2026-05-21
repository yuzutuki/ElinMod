using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        {"dummy" ,new yz_SeStruct("dummy",0,yz_ssType.chunk,1,"ダミーデータだよ！重み0だから出ないはずだよ!",0,1) },
        {"e_cultist",new yz_SeStruct("fanatic",100,yz_ssType.chunk,28,"すごい数の信者が集まってきている！ ",-10,0.7f)},
        {"e_komod",new yz_SeStruct("komodoensis",80,yz_ssType.chunk,20,"戦場にコモドドラゴンをはなてっ ",-10,1 )},
        {"e_chicken",new yz_SeStruct("chicken",100,yz_ssType.scatter,40,"復讐を誓うニワトリの群れが現れた…！ ",5,1.2f )},
        {"e_sheep",new yz_SeStruct("sheep",100,yz_ssType.chunk,20,"怒涛の羊たちがあらわれた！！ ",0,1)},
        {"e_giant",new yz_SeStruct("giant",30,yz_ssType.chunk,20,"大地を鳴らす巨人の集団だ…！ ",-5,1)},
        {"e_imouto",new yz_SeStruct("sister",100,yz_ssType.scatter,15,"野生のあなたの妹たちが襲い掛かってきた！ ",0,1)},
        {"e_puti",new yz_SeStruct("putty",100,yz_ssType.chunk,12,"かわいらしいプチの群れだ…。 ",-100,0.1f) },
        {"e_bell",new yz_SeStruct("bell_silver",10,yz_ssType.chunk,8,"ベルの音色が聞こえる… ",-100,0.1f) },
        {"e_zombi",new yz_SeStruct("zombie",100,yz_ssType.scatter,60,"*ヤツらの押し寄せてくる音…* ",-10,0.5f) },
        {"e_kamikaze",new yz_SeStruct("yeek_kamikaze",30,yz_ssType.chunk,24,"キケンな火薬の匂いがする…！ ",-10,0.5f) },
        {"e_tentacle",new yz_SeStruct("tentacleB",30,yz_ssType.scatter,30,"ねちょねちょと蠢く音がする… ",-10,1) },
        {"e_saccu",new yz_SeStruct("nun_mother",80,yz_ssType.scatter,12,"なんだかドキドキする香りが漂ってきた… ",5,1) },
        {"e_kiria",new yz_SeStruct("kiria_fake",50,yz_ssType.chunk,12,"機械少女のレプリカたちが襲ってきた！ ",2,1) },
        {"e_bat",new yz_SeStruct("bat",100,yz_ssType.chunk,30,"生存者を押しつぶそうと大量のコウモリが現れた！ ",-5,1)},
        {"e_kobolt",new yz_SeStruct("kobolt",100,yz_ssType.chunk,30,"生存者を押しつぶそうと大量のコボルトが現れた！ ",-5,1)},
        {"e_ghost",new yz_SeStruct("ghost",100,yz_ssType.chunk,30,"生存者を押しつぶそうと大量のゴーストが現れた！ ",-5,1)},
        {"e_mohikan",new yz_SeStruct("punk",100,yz_ssType.chunk,30,"ヒャッハーな方々が徒党を組んで襲ってきた！ ",-5,0.8f)},
        {"e_toysoldier",new yz_SeStruct("toysoldier",80,yz_ssType.chunk,16,"おもちゃの兵士たちが暴れている！ ",0,1)},
        {"e_yakuza",new yz_SeStruct("cyakuza",60,yz_ssType.chunk,16,"威圧的な見た目のクローン兵だ！コワイ！ ",-5,1)},
        {"e_kimogasa",new yz_SeStruct("kimogasa",60,yz_ssType.scatter,30,"農民のような弱小吸血鬼の集団だ… ",-8,0.8f)},
        //
        {"e_doppel",new yz_SeStruct("doppel",20,yz_ssType.scatter,1,"あなたの影があなたを消しにきた…！ ",0,0) },
        //
        {"l_dogs",new yz_SeStruct("list_dogs",100,yz_ssType.chunk,16,"凶暴な犬の群れだ…！ ",-3,0.8f) },
        {"l_birds",new yz_SeStruct("list_birds",100,yz_ssType.chunk,24,"狂気に駆られた鳥の大群だ！ ",-3,0.8f) },
        {"l_kanis",new yz_SeStruct("list_kanis",100,yz_ssType.chunk,24,"磯の匂いがする…カニの大群だ… ",-5,0.8f) },
        {"l_mercs",new yz_SeStruct("list_mercs",100,yz_ssType.chunk,16,"無法な傭兵団が襲ってきた！ ",0,1) },
        {"l_drush",new yz_SeStruct("list_drush",30,yz_ssType.scatter,16,"様々な竜が集まる…ドラゴン・ラッシュだ！ ",-3,0.9f) },
        //
        {"a_claymore",new yz_SeStruct("silvereye",80,yz_ssType.ally,1," さらに、どこからともなく銀眼の斬殺者が援軍に現れた！ ",10,0) },
        {"a_ojou",new yz_SeStruct("younglady",100,yz_ssType.ally,1," さらに、どこからともなくお嬢様が援軍に現れた！ ",5,0)},
        {"a_sis",new yz_SeStruct("sister",60,yz_ssType.ally,4," さらに、あなたの狂信者が援軍に現れた！ ",0,0)},
        {"a_njslyr",new yz_SeStruct("njslyr",10,yz_ssType.ally,1," さらに、時空の裂け目から赤黒い死神のエントリーだ！ゴウランガ！ ",10,0)},
        {"a_d51",new yz_SeStruct("d51",10,yz_ssType.ally,1," さらに、機械仕掛けの軍犬があなたの味方をするようだ！ ",10,0)},
        {"a_meat",new yz_SeStruct("gwen",10,yz_ssType.ally,1," さらに、無邪気な少女が戦場に迷い込んだ… ",0,0.5f)},
        {"a_nerun",new yz_SeStruct("nerun",40,yz_ssType.ally,1," さらに、世話焼きなガイドが手伝ってくれるようだ ",0,0)},
        {"a_fiama",new yz_SeStruct("fiama",20,yz_ssType.ally,1," さらに、ムーンゲートの歪みからミシリアの影が援軍に現れた！ ",8,0)},
        {"a_ashland",new yz_SeStruct("ashland",20,yz_ssType.ally,1," さらに、ムーンゲートの歪みからミシリアの影が援軍に現れた！ ",10,0)},
        {"a_mesugaki",new yz_SeStruct("adv_gaki",15,yz_ssType.ally,1," さらに、ムーンゲートの歪みから生意気な少女が援軍に現れた！ ",10,0)},
        {"a_kiria",new yz_SeStruct("adv_kiria",15,yz_ssType.ally,1," さらに、ムーンゲートの歪みから機械人形の少女が援軍に現れた！ ",10,0)},
        {"a_ivory",new yz_SeStruct("adv_ivory",15,yz_ssType.ally,1," さらに、ムーンゲートの歪みから象牙塔の使者が援軍に現れた！ ",10,0)},
        {"a_mesherada",new yz_SeStruct("adv_mesherada",15,yz_ssType.ally,1,"　さらに、ムーンゲートの歪みから運命の理解者が援軍に現れた！ ",10,0)},
        {"a_wini",new yz_SeStruct("adv_wini",15,yz_ssType.ally,1," さらに、ムーンゲートの歪みから永遠の旅人が援軍に現れた！ ",10,0)},
        {"a_verna",new yz_SeStruct("adv_verna",15,yz_ssType.ally,1," さらに、ムーンゲートの歪みから翳りの魔女が援軍に現れた！ ",10,0)},
        {"a_eureka",new yz_SeStruct("eureka",15,yz_ssType.ally,1," さらに、ムーンゲートの歪みから戦禍の奏者が援軍に現れた！ ",10,0)},
        //
        {"pt_fiash",new yz_SeStruct("pt_fiash",20,yz_ssType.ally,2," さらに、ムーンゲートの歪みからミシリアの影が援軍に現れた！ ",10,0)},
        {"pt_ketquru",new yz_SeStruct("pt_ketquru",20,yz_ssType.ally,2," さらに、ムーンゲートの歪みから放浪の錬金術師が援軍に現れた！ ",10,0)},
        {"pt_mikomiko",new yz_SeStruct("pt_mikomiko",15,yz_ssType.ally,2," さらに、ムーンゲートの歪みから二人の巫女が援軍に現れた！ ",10,0)},
        {"pt_rodoao",new yz_SeStruct("pt_rodoao",15,yz_ssType.ally,3," さらに、ムーンゲートの歪みから特別な商人？が援軍に現れた！ ",10,0)},
        {"pt_pal",new yz_SeStruct("pt_pal",20,yz_ssType.ally,3," さらに、ムーンゲートの歪みからパルミアの英雄達が援軍に現れた！ ",6,0)},
        {"pt_olv",new yz_SeStruct("pt_olv",20,yz_ssType.ally,2," さらに、ムーンゲートの歪みからオルヴィナの姉妹が援軍に現れた！ ",6,0)}
        //ちぃっ　なんでコモドドラゴンがの人
        //エレア二人
        //トダー？
        //summoning101?
    };
    //援軍用PTデータ
    static public Dictionary<string, List<string>> allyPT = new Dictionary<string, List<string>>
    {
        {"pt_fiash",new List<string>{ "fiama", "ashland" } },
        {"pt_ketquru",new List<string>{ "kettle", "quru" } },
        {"pt_mikomiko",new List<string>{ "miko_nefu", "miko_mifu" } },
        {"pt_rodoao",new List<string>{ "rodwyn", "girl_blue", "merchant_inn_fox2" } },
        {"pt_pal",new List<string>{ "mapMerchant", "conery", "gilbert" } },
        {"pt_olv",new List<string>{ "farris", "theolucia" } }
    };
    //混合部隊の内訳
    static public Dictionary<string, List<string>> mixSpawnList = new Dictionary<string, List<string>>
    {
        {"list_dogs",new List<string>{ "dog", "dog_wild", "hound", "dog_shiba", "dog_shiva", "silverwolf", "kobolt" } },
        {"list_birds",new List<string>{ "chicken", "duck", "goose", "turkey", "caladrius" } },
        {"list_kanis",new List<string>{ "cancer", "hermitcrab", "crab_shiba", "kingcrab" } },
        {"list_mercs",new List<string>{ "merc", "merc_archer", "merc_warrior", "merc_mage" } },
        {"list_drush",new List<string>{ "tyrannosaurus", "mass_monster", "drake", "drake", "dragon", "lizardman", "lizardman", "wyvern", "dodo_rex" } }
    };
    //
    //重みと乱数を見てランダムな項目の取得
    static public yz_SeStruct getEvent(string targetDic=null)
    {
        //処理用の配列
        Dictionary<string, yz_SeStruct> td = new Dictionary<string, yz_SeStruct>();
        //援軍のみ抽出するか除外
        if (targetDic == "Ally")
        {
            td = eventDic.Where(pair => pair.Key.StartsWith("a_") || pair.Key.StartsWith("pt_")).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        else
        {
           td = eventDic.Where(pair => !(pair.Key.StartsWith("a_") || pair.Key.StartsWith("pt_"))).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        //
        //初期化
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
    ally,//プレイヤーの周り（援軍）
    chunk,//固まって出現
    scatter//端にランダムに
}