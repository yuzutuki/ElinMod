using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yz_sss
{
    public class ConYzPolymorph : ConTransmute
    {
        //ターン経過処理
        public override void Tick()
        {
            owner.AddCondition("ConSilence", 1000, true);
            owner.AddCondition("ConBlind", 1000, true);
            base.Mod(-1, false);
        }
        public override void OnStart()
        {
            owner.AddCondition("ConSilence", 1000, true);
            owner.AddCondition("ConBlind", 1000, true);
            base.OnStart();
        }
        public override void OnRemoved()
        {
            owner.RemoveCondition<ConSilence>();
            owner.RemoveCondition<ConBlind>();
            base.OnRemoved();
        }

        public override RendererReplacer GetRendererReplacer()
        {
            //return RendererReplacer.CreateFrom("putty_snow", 0);
            return RendererReplacer.CreateFrom("sheep", 0, -1);
        }
    }
}