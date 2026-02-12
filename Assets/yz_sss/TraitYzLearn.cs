using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    public class TraitYzLearn : Trait
	{
		//äTÇÀTraitBookSkillÇ©ÇÁÇÃÉRÉsÅ[
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

		public override bool IsOnlyUsableByPc
		{
			get
			{
				return true;
			}
		}
		public override int GetValue()
		{
			return this.owner.sourceCard.value;
		}
		public override void OnRead(Chara c)
		{
			if (!c.IsPC)
			{
				return;
			}
			if (this.IsOnlyUsableByPc && !c.IsPC)
			{
				this.owner.SayNothingHappans();
				return;
			}
			this.owner.Say("skillbook_learn", c, this.GetName(), null);
			//èKìæïîï™
			int LearnID = 0;
			Debug.Log($"name:{this.Name},owner:{this.owner.Name},{this.owner.id},{this.GetName()}");
            switch (this.owner.id)
            {
				case "YzLearnAutomap":
					LearnID = 721002;
					break;
				default:
					break;
            }
			if (!c.elements.HasBase(LearnID))
			{
				c.elements.Learn(LearnID, 1);
			}
			else
			{
				c.Say("yz_Learned");
				c.SayNothingHappans();
				return;
			}
			c.Say("spellbookCrumble", this.owner.Duplicate(1), null, null);
			this.owner.ModNum(-1, true);
		}


	}
}