using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    internal class FeatYzKemomimi : Feat
    {
        internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
        {
            if (hint)
            {
            //    hints.Add("This is a custom feat"); // 任意（オプション）
            }
            else
            {
                // 属性の変更、ポテンシャルの設定、効果の適用などを行う
                // hint モードではない場合のみ実行する
                eleOwner.ModBase(SKILL.PER, add * 25);
                eleOwner.ModBase(SKILL.resSound, -add * 25);
                // その他
            }
        }
    }
}