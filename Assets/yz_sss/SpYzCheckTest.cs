using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NPOI.SS.UserModel;
using UnityEngine;

namespace yz_sss
{
    internal class SpYzCheckTest : Spell
    {
        int i = 1;
        public override bool Perform()
        {
            //詠唱エフェクト
            CastEffect.Call(CC);
            CC.PlaySound("identify", 1f, true);
            switch (i)
            {
                case (0):
                    TC.Chara.AddCondition<ConParalyze>(100, true);
                    CC.Say("ぱららいず！");
                    break;
                //
                case (1):
                    debug.Log($"Point:{CC.pos.ToString()}" +
                        $",posCenter:{CC.pos.PositionCenter()}" +
                        $",renderCenter:{CC.renderer.PositionCenter()}");
                    if (TP != null)
                    {
                        Vector2 tPos = TP.PositionCenter();
                        debug.Log($"VectorPos:{tPos}");
                    }
                    CC.Say("ここどこ？");
                    break;
                //
                case (2):
                    if (TC != null)
                    {
                        debug.Log($"name:{TC.Name}");
                        /*
                            debug.Log($"ID:{TC.id},{TC?.idMaterial},{TC?.idSkin}");
                            debug.Log($"idEditer?:{TC.Chara?.c_idEditor}");
                            debug.Log($"refCard:{TC.Chara?.c_idRefCard}");
                            debug.Log($"_id:{TC?.c_idRefCard},{TC?.c_idRefCard2}");
                            debug.Log($"Index:{TC.Chara?.refCard?._index},{TC.Chara?.c_indexContainerIcon}");
                        debug.Log($"Sprite:{TC.Chara?.GetImageSprite().name}");
                        debug.Log($"idSprite:{TC.Chara?.sourceCard.idSprite}");
                        debug.Log($"pathSprite:{TC.Chara?.sourceCard.pathSprite}");
                        debug.Log($"sourceIndex:{TC.Chara?.sourceCard._index}");
                        */
                        debug.Log($"source_id:{TC.Chara.source._id}");
                        debug.Log($"source_idRender:{TC.Chara.source.idRenderData}");
                        CC.Say("きみだれ？");
                        //
                        YzUtil.PlayEffect("CheckTest",CC,TC);
                    }
                    break;
                case (3):
                    CC.Say("ゾーンエクスポート");
                    EClass._zone.ExportDialog(null);
                    break;
                case (4):
                    YzWrapper.PlayVanillaEffect("Element/ball_Fire", YzUtil.GetStepTowards(CC.pos, TP), 1f);
                    break;
            }
            return true;
        }
    }
}
/*
    internal class ActCheckTest : Spell
    {
        public override bool Perform()
        {
            CC.PlaySound("identify", 1f, true);
            CC.Say("テスト用の呪文だよ！");
            return true;
        }
    }
 */