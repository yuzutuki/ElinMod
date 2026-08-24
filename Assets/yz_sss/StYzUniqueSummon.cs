using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


namespace yz_sss
{
    internal class StYzUniqueSummon : BaseStance
    {
        //ステータス補正にElementsデータを使用
        public override bool UseElements
        {
            get
            {
                return true;
            }
        }
        //
        public void SetElement(int id, int value)
        {
            this.elements.SetBase(id, value);
        }
        
    }
}