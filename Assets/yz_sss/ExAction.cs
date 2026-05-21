using Cwl.Helper;
using Cwl.Helper.Unity;
using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace yz_sss
{
    //追加アクション用ステータス
    public readonly struct ExActionData
    {
        public Spell SP { get; }
        public Zone CastZone { get; }
        public Chara CC { get; }
        public Chara TC { get; }
        public Point TP { get; }
        public float SubFloat { get; }
        public ExActionData(Spell sp, Zone castZone, Chara cc, Point tp, Chara tc = null, float subFloat = 0f)
        {
            SP = sp;
            CastZone = castZone;
            CC = cc;
            TP = tp.Copy();
            SubFloat = subFloat;
            TC = tc;
        }
    }
    internal interface IExAction
    {
        void ExAction(ExActionData data);
    }
}