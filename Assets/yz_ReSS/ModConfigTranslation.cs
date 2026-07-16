using BepInEx;
using Dungen;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;
using HarmonyLib;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CellType = NPOI.SS.UserModel.CellType;

namespace yz_ReSS
{
    public class ModConfigTranslation:BaseUnityPlugin
    {
        public static bool Regist(ModOptionController ctl,string MainFolder)
        {
            string path = Path.Combine(MainFolder, "ress_Translation.xlsx");
            if (!File.Exists(path))
            {
                Debug.Log($"-=-=ReSS:翻訳ファイルを取得できませんでした。({path})=-=-");
                return false;
            }
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheet("Translation");
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    //ID
                    string key = GetCellString(row, 0);
                    if (key == null || key == "" || key == " ")
                    {
                        //Debug.Log($"ReSS:スキップ列:{i}");
                        continue;
                    }
                    string ja = GetCellString(row, 1);
                    string en = GetCellString(row, 2);
                    string zh = GetCellString(row, 3);
                    Debug.Log($"=-=- RegistTranslation: {key} => ja:{ja},en:{en},zh:{zh}");
                    ctl.SetTranslation(key, en, ja, zh);
                }
            }
            return true;
        }
        static string GetCellString(IRow row, int index)
        {
            var cell = row.GetCell(index);
            //空白だったら空白を返す
            if (cell == null || cell.CellType == CellType.Blank)
            {
                return "";
            }
            //とりあえず全部stringにして返す
            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Formula:
                    return cell.ToString();
                default:
                    return cell.ToString();
            }
        }
    }
}
/*
 * static public Dictionary<string, EffData> LoadExcel(string path)
        {
            Dictionary<string, EffData> dic = new Dictionary<string, EffData>();
            //エラー処理
            if (!File.Exists(path))
            {
                Debug.Log($"-=-=ReSS:Excelファイルを取得できませんでした。({path})=-=-");
                return dic;
            }
            Debug.Log("xlsx読み込みOK");
            //NPOIのなんやかんやでファイル読み込み
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheet("EffectData");
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    EffData data = new EffData();
                    //ID
                    data.id = GetCellString(row, 0);
                    if(data.id == null || data.id == "" || data.id == " ")
                    {
                        //Debug.Log($"ReSS:スキップ列:{i}");
                        continue;
                    }
                    //移動タイプ
                    data.type = YzUtil.ToType(GetCellString(row, 1));
                    //色補正
                    if (GetCellString(row, 3) == null)
                    {
                        data.colorFix = new Color(1, 1, 1);
                    }
                    else
                    {
                        string[] cParts = GetCellString(row, 3).Split(',');
                        if (cParts.Length >= 3 &&
                            float.TryParse(cParts[0], out float r) &&
                            float.TryParse(cParts[1], out float g) &&
                            float.TryParse(cParts[2], out float b))
                        {
                            data.colorFix = new Color(r, g, b);
                        }
                        else
                        {
                            data.colorFix = new Color(1, 1, 1);
                            Debug.Log($"色補正のデータが不正:{data.id}列:{i}");
                        }
                    }
                    //透明度追加補正
                    data.alphaFix = float.TryParse(GetCellString(row, 4), out float a) ? a : 1f;
                    //速度補正
                    data.speedFix = float.TryParse(GetCellString(row, 5), out float s) ? s : 1f;
                    //生存時間
                    data.lifeTime = int.TryParse(GetCellString(row, 7), out int l) ? l : -1;
                    //タグ処理
                    data.lookTarget = GetCellString(row, 2).Contains("LookTarget");
                    data.arrowHold = GetCellString(row, 2).Contains("ArrowHold");
                    data.deviation = GetCellString(row, 2).Contains("Deviation");
                    data.exAction = GetCellString(row, 2).Contains("ExAction");
                    //追加エフェクト処理
                    data.afterEffect = GetCellString(row, 6);
                    //登録
                    if (!dic.ContainsKey(data.id))
                    {
                        dic.Add(data.id, data);
                    }
                    else
                    {
                        Debug.Log($"キーが重複：{data.id},列:{i}");
                    }
                }
            }
            return dic;
        }

        //
        //セル取得用
        static string GetCellString(IRow row, int index)
        {
            var cell = row.GetCell(index);
            //空白だったら空白を返す
            if (cell == null || cell.CellType == CellType.Blank)
            {
                return "";
            }
            //とりあえず全部stringにして返す
            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Formula:
                    return cell.ToString();
                default:
                    return cell.ToString();
            }
        }
*/