
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
namespace yz_ReSS
{
    public static class ColorSetting
    {
        public static Dictionary<string, Color> ElementColor = new Dictionary<string, Color>()
        {
            { "eleMagic",new Color(0.314f, 0.398f, 1.000f, 1.500f) },
            { "eleFire",new Color(0.996f, 0.404f, 0.004f, 1.000f) },
            { "eleCold",new Color(0.175f, 0.338f, 0.873f, 1.000f) },
            { "eleLightning",new Color(0.996f, 0.984f, 0.020f, 0.700f) },

            { "eleDarkness",new Color(0.145f, 0.122f, 0.333f, 1.500f) },
            { "eleNether",new Color(0.175f, 0.058f, 0.081f, 1.200f) },
            { "elePoison",new Color(0.050f, 0.300f, 0.050f, 1.000f) },

            { "eleMind",new Color(1.000f, 0.118f, 0.616f, 0.800f) },
            { "eleNerve",new Color(0.902f, 0.808f, 0.553f, 0.800f) },
            { "eleSound",new Color(1.000f, 0.682f, 0.000f, 0.550f) },

            { "eleChaos",new Color(0.545f, 0.192f, 1.000f, 1.200f) },
            { "eleHoly",new Color(1.000f, 0.980f, 0.753f, 1.000f) },
            { "eleCut",new Color(0.610f, 0.000f, 0.098f, 0.900f) },

            { "eleAcid",new Color(0.666f, 0.855f, 0.228f, 0.700f) },
            { "eleEther",new Color(0.388f, 0.992f, 0.898f, 1.000f) },

            { "_void",new Color(1.000f, 1.000f, 1.000f, 1.000f) },
            { "eleImpact",new Color(1.000f, 1.000f, 1.000f, 1.000f) },
            { "eleVoid",new Color(0.200f, 0.200f, 0.600f, 1.000f) }
        };
        public static Color EleColorFix(Color color, Color baseColor = default, bool isEx = false)
        {
            if (baseColor == default)
            {
                baseColor = Color.white;
            }
            float a = baseColor.a;
            Color result = baseColor;
            float alphaFix = 1.0f;
            if (color != Color.white)
            {
                Color.RGBToHSV(Color.Lerp(baseColor, color, isEx ? 0.5f : 1.0f), out float h, out float s, out float v);
                if (isEx)
                {
                    h += (Random.Range(0f, 0.05f) * (Random.value < 0.5f ? -1f : 1f));
                    s += 0.2f;
                }
                s = s * ModConfig.SatMult.Value;
                v = v * ModConfig.ValMult.Value;
                result = Color.HSVToRGB(h, s, v, true);
                alphaFix = Mathf.Clamp(1.4f - ((color.r * 2 + color.g * 1.8f + color.b * 1.5f) / 5f), 0.1f, 1.2f);
            }
            result.a = a * ModConfig.EffectAlpha.Value * color.a * alphaFix;
            //Debug.Log($"[ReSS] EleColorFix: baseColor={baseColor}, color={color}, result={result}, alphaFix={alphaFix}");
            return result;
        }
    }
    //
}
/*
131,127,255 → #837FFF → RGBA(0.514f, 0.498f, 1.000f, 1.000f)

254,103,1   → #FE6701 → RGBA(0.996f, 0.404f, 0.004f, 1.000f)
19,71,248   → #1347F8 → RGBA(0.075f, 0.278f, 0.973f, 1.000f)
254,251,5   → #FEFB05 → RGBA(0.996f, 0.984f, 0.020f, 1.000f)

37,31,85    → #251F55 → RGBA(0.145f, 0.122f, 0.333f, 1.000f)
70,2,8      → #460208 → RGBA(0.275f, 0.008f, 0.031f, 1.000f)
25,106,0    → #196A00 → RGBA(0.098f, 0.416f, 0.000f, 1.000f)

255,30,157  → #FF1E9D → RGBA(1.000f, 0.118f, 0.616f, 1.000f)
230,206,141 → #E6CE8D → RGBA(0.902f, 0.808f, 0.553f, 1.000f)
255,174,0   → #FFAE00 → RGBA(1.000f, 0.682f, 0.000f, 1.000f)

139,49,255  → #8B31FF → RGBA(0.545f, 0.192f, 1.000f, 1.000f)
255,250,192 → #FFFAC0 → RGBA(1.000f, 0.980f, 0.753f, 1.000f)
181,0,25    → #B50019 → RGBA(0.710f, 0.000f, 0.098f, 1.000f)

198,233,53  → #C6E935 → RGBA(0.776f, 0.914f, 0.208f, 1.000f)
99,253,229  → #63FDE5 → RGBA(0.388f, 0.992f, 0.898f, 1.000f)

[Unity] eleAcid:RGBA(0.282, 0.451, 0.000, 1.000)
[Unity] eleChaos:RGBA(0.914, 0.412, 0.886, 1.000)
[Unity] eleCold:RGBA(0.031, 0.341, 0.706, 1.000)
[Unity] eleCut:RGBA(0.651, 0.000, 0.086, 1.000)
[Unity] eleDarkness:RGBA(0.180, 0.180, 0.180, 1.000)
[Unity] eleEther:RGBA(0.412, 0.925, 1.000, 1.000)
[Unity] eleFire:RGBA(1.000, 0.310, 0.000, 1.000)
[Unity] eleHoly:RGBA(1.000, 1.000, 1.000, 1.000)
[Unity] eleLightning:RGBA(1.000, 0.949, 0.000, 1.000)
[Unity] eleMagic:RGBA(0.439, 0.455, 1.000, 1.000)
[Unity] eleMind:RGBA(1.000, 0.580, 0.871, 1.000)
[Unity] eleNerve:RGBA(0.282, 0.239, 0.000, 1.000)
[Unity] eleNether:RGBA(0.071, 0.059, 0.357, 1.000)
[Unity] elePoison:RGBA(0.000, 0.690, 0.020, 1.000)
[Unity] eleSound:RGBA(0.961, 0.749, 0.431, 1.000)
[Unity] _void:RGBA(1.000, 1.000, 1.000, 1.000)
[Unity] eleImpact:RGBA(1.000, 1.000, 1.000, 1.000)
[Unity] eleVoid:RGBA(0.000, 0.000, 0.000, 1.000)
 */