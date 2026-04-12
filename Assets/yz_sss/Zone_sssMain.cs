using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace yz_sss {
	//マップパラメータ
	
	public class Zone_sssMain : Zone_Civilized
	{
		public override ZoneTransition.EnterState RegionEnterState
		{
			get
			{
				return ZoneTransition.EnterState.Return;
			}
		}
		public override string pathExport
		{
			get
			{
				return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Maps/" + this.idExport + ".z");
			}
		}
	}
	
}