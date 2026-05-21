using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace yz_sss
{
    public class TraitYzRandomSpellbook : TraitSpellbook
    {
        public override TraitBaseSpellbook.Type BookType
        {
            get
            {
                return TraitBaseSpellbook.Type.Spell;
            }
        }

        public override SourceElement.Row source
        {
            get
            {
                return EClass.sources.elements.map.TryGetValue(this.owner.refVal, 0);
            }
        }
        public override void OnCreate(int lv)
        {
            Debug.Log($"ref:{this.owner.refVal}");
            string name = this.owner.id;
            List<int> idList = new List<int>();
            foreach (int id in YzSSS.SpellIDList)
            {
                EClass.sources.elements.map.TryGetValue(id, out SourceElement.Row row);
                Debug.Log($"id:{id}, category:{row.categorySub}, alias:{row.alias}, tag:{row.tag.ToString()}");
                switch (name)
                {
                    case "YzRandomSpellbookA":
                        //
                        if (row.categorySub == "attack")
                            if (row.tag != null && !row.tag.Contains("specialAbility"))
                            {
                                idList.Add(id);
                            }
                        break;
                    case "YzRandomSpellbookU":
                        //
                        if (row.categorySub == "util")
                            if (row.tag != null && !row.tag.Contains("specialAbility"))
                            {
                                if (row.alias.Contains("Summon"))
                                {
                                    continue;
                                }
                                idList.Add(id);
                            }
                        break;
                    case "YzRandomSpellbookS":
                        //
                        if (row.alias.Contains("Summon"))
                            if (row.tag != null && !row.tag.Contains("specialAbility"))
                            {
                                idList.Add(id);
                            }
                        break;
                    case "YzRandomSpellbookSP":
                        //
                        if (row.tag != null && row.tag.Contains("specialAbility"))
                        {
                            idList.Add(id);
                        }
                        break;
                    case "YzRandomSpellbook":
                    default:
                        idList.Add(id);
                        break;
                }
            }
            if (idList.Count == 0)
            {
                idList.Add(720039);
            }
            if(name== "YzRandomSpellbookNew")
            {
                if(idList.Count > 6)
                {
                    idList.RemoveRange(0, idList.Count - 6);
                }
            }
            /*
            List<int> idList = new List<int>(YzSSS.SpellIDList);
            switch (name)
            {
                case "YzRandomSpellbookA":
                    //アタックだけ抽出
            foreach(int i in idList)
            {
                EClass.sources.elements.map.TryGetValue(i, out SourceElement.Row row);
            }
                    break;
                case "YzRandomSpellbook":
                default:
                    break;
            }
            */
            int choiceID = idList.RandomItem();
            this.owner.refVal = choiceID;
            this.owner.c_charges = 6 + EClass.rnd(15);
        }
        public override string GetName()
        {
            //Debug.Log($"booktype:{this.BookType.ToString()}");
            return "spellbook_".lang(this.source.GetName().ToLower(), null, null, null, null);
        }
    }

}