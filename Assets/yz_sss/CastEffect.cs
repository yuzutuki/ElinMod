using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yz_sss
{
    public class CastEffect : MonoBehaviour
    {
        static public void Call(Card CC, int castType = 0)
        {
            switch (castType)
            {
                case 0:
                default:
                    YzUtil.PlayEffect("CastA", CC, CC);
                    break;
            }
        }
    }
}