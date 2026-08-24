using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    public class TraitYzMagicalTicket : Trait
    {
        public override bool CanRead(Chara c)
		{
			return !c.isBlind;
		}

		public override int GetActDuration(Chara c)
		{
			return 1;
		}

		public virtual bool IsPlan
		{
			get
			{
				return false;
			}
        }
        public override bool IsNoShop
        {
            get
            {
                return true;
            }
        }
        public override bool IsOnlyUsableByPc
		{
			get
			{
				return true;
			}
		}
		public override void OnRead(Chara c)
		{
			if (!c.IsPC)
			{
				return;
			}
            //CS$<>8__locals1.cc.PlaySound("return_cast", 1f, true);
            if (EClass.player.returnInfo == null)
            {
                //魔法屋の存在チェックと取得

                Zone zone = EClass.game.world.FindZone("yz_sssMainZone");
                if (zone.Region == null)
                {
                    Debug.Log("== Error? : Missing StrangeSpellShop in mainworld.  ==");
                    Debug.Log("== Recreating zone... == ");
                    zone.Destroy();
                    SpatialGen.Create("yz_sssMainZone", EClass.game.world, true, -99999, -99999, 0);
                    Zone NewZone = EClass.game.world.FindZone("yz_sssMainZone");
                    Debug.Log($"ZoneID: {NewZone.id},Region: {NewZone?.Region?.id}, Position: {NewZone.mapX}, {NewZone.mapY},{NewZone.x}, {NewZone.y}");
                    zone = NewZone;
                }
                int toUid = zone.uid;
                //
                EClass.player.returnInfo = new Player.ReturnInfo
                    {
                        turns = 10,
                        uidDest = toUid
                    };
                
                Msg.Say("returnBegin");
            }
            else
            {
                EClass.player.returnInfo = null;
                Msg.Say("returnAbort");
            }
            //
            this.owner.ModNum(-1, true);
		}


	}
}