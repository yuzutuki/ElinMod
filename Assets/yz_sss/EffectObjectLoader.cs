using BepInEx;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace yz_sss
{
    //ShinySpellの読み込み部分を改良
    //取得したオブジェクト名からスクリプトアタッチなどを決める
    //*CodeRabbit*とか使ってみるべきかも
    public class EffData
    {
        public string id;
        public EffType type;
        public string tag;
        public Color colorFix;
        public float alphaFix;
        public float speedFix;
        public bool lookTarget;
        public bool arrowHold;
        public bool deviation;
        public string afterEffect;
        //空コンストラクタ
        public EffData() { }
        //コピーコンストラクタ
        public EffData(EffData o)
        {
            id = o.id;
            type = o.type;
            tag = o.tag;
            colorFix = o.colorFix;
            alphaFix = o.alphaFix;
            speedFix = o.speedFix;
            lookTarget = o.lookTarget;
            arrowHold = o.arrowHold;
            deviation = o.deviation;
            afterEffect = o.afterEffect;
        }
        //クローン
        public EffData Clone()
        {
            return new EffData(this);
        }
        //数値チェック
        public void CheckEffectData()
        {
            Debug.Log(
            $"[EffData]\n" +
            $"  id:         {id}\n" +
            $"  type:       {type}\n" +
            $"  tag:        {tag}\n" +
            $"  colorFix:   {colorFix}\n" +
            $"  alphaFix:   {alphaFix}\n" +
            $"  speedFix:   {speedFix}\n" +
            $"  lookTarget: {lookTarget}\n" +
            $"  arrowHold: {arrowHold}\n" +
            $"  afterEffect: {afterEffect}"
          );
        }
    }
    public class EffectObjectLoader : MonoBehaviour
    {

        //
        static public void LoadEffectAsset(string assetPath)
        {
            //エクセルファイル読み込み
            YzSSS.effDic = LoadExcel(Path.Combine(assetPath, "Asset", "sss_Data.xlsx"));
            //読み込みテスト
            if (YzSSS.effDic.TryGetValue("CheckTest", out EffData loadtest))
            {
                Debug.Log($"読み込みテスト:{loadtest.id},{loadtest.type},{loadtest.tag},{loadtest.colorFix}");
            }
            //アセットバンドル読み込み
            AssetBundle ab;
            string path = Path.Combine(assetPath, "Asset", "sss_Particle");
            Debug.Log($"path:{path}");
            //Debug.Log("LoadEffectAsset...");
            ab = AssetBundle.LoadFromFile(path);

            if (ab == null)
            {
                Debug.Log($"-==-エラーデス!！ アセットの読み込みに失敗,Path{path}-==-");
                return;
            }
            Debug.Log(ab.name);
            // すべてのアセット名を取得
            string[] assetNames = ab.GetAllAssetNames();

            foreach (string name in assetNames)
            {
                GameObject go = ab.LoadAsset<GameObject>(name);
                if (go != null)
                {
                    //Debug.Log($"-=loaded: {go.name}");
                    //スクリプトアタッチ
                    go.AddComponent<EffectObjectController>();
                    YzSSS.objDic.Add(go.name, go);
                    Debug.Log($"---importParticle: {go.name}---");
                    //
                }
            }
        }
        static public Dictionary<string, EffData> LoadExcel(string path)
        {
            Dictionary<string, EffData> dic = new Dictionary<string, EffData>();
            //エラー処理
            if (!File.Exists(path))
            {
                Debug.Log($"-=-=エラーデス！Excelファイルを取得できませんでした。({path})=-=-");
                return dic;
            }
            Debug.Log("xlsx読み込みOK");
            //NPOIのなんやかんやでファイル読み込み
            //using内でやると処理が終わったときにファイルを閉じてメモリにやさしい　らしい。
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
                    //タグ処理
                    data.lookTarget = GetCellString(row, 2).Contains("LookTarget");
                    data.arrowHold = GetCellString(row, 2).Contains("ArrowHold");
                    data.deviation = GetCellString(row, 2).Contains("Deviation");
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
    }
}