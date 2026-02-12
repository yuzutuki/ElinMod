using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yz_sss
{
	public class ConYzPolymorph : ConTransmute
	{
		public override RendererReplacer GetRendererReplacer()
		{
			//return RendererReplacer.CreateFrom("putty_snow", 0);
						return RendererReplacer.CreateFrom("sheep", 0,-1);
		}
	}
}