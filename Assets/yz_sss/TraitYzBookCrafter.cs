using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yz_sss
{
    public class TraitYzBookCrafter : TraitCrafter
    {
        //クラフトのID
        public override string IdSource
        {
            get
            {
                return "YzBookCrafter";
            }
        }

        //選択時のコマンドテキスト
        public override string CrafterTitle
        {
            get
            {
                return "yz_invBookCrafter";
            }
        }
        //作成時のアニメ
        public override AnimeID IdAnimeProgress
        {
            get
            {
                return AnimeID.Shiver;
            }
        }
        //効果音
        public override string idSoundProgress
        {
            get
            {
                return "craft_scratch";
            }
        }

        public override int GetDuration(AI_UseCrafter ai, int costSp)
        {
            return base.GetSource(ai).time;
        }
    }

}