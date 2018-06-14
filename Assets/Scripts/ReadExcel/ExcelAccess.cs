using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Excel;
//using OfficeOpenXml;
using System.IO;

public class ExcelAccess
{
    public static string ExcelName = "VoiceTable.xlsx";
    public static string ExcelType = "VoiceType.xlsx";
    public static string[] SheetNames = { "Sheet1", "Sheet1", "Sheet1", "Sheet1", "Sheet1", "Sheet1" };


    public  static List<ExcelTableEntity> SelectTables(string tableName)
    {
        DataRowCollection collect = ExcelAccess.ReadExcel(tableName,SheetNames[0]);
        List<ExcelTableEntity> list = new List<ExcelTableEntity>();
        for (int i = 1; i < collect.Count; i++)
        {
            ExcelTableEntity e = new ExcelTableEntity();
            if (collect[i][0].ToString() == "") continue;
            e.ID = collect[i][0].ToString();
            e.Type = collect[i][1].ToString();
            if (tableName == ExcelName)
            {
                e.Time = collect[i][2].ToString();
                e.TimeContent = collect[i][3].ToString();
                e.WinningContent = collect[i][4].ToString();
                e.FailContent = collect[i][5].ToString();
                e.FialContentDrop = collect[i][6].ToString();
                e.WinTime= collect[i][7].ToString();
                e.FailTime = collect[i][8].ToString();
                e.WinningAfter = collect[i][9].ToString();
                e.WinningAfterTime = collect[i][10].ToString();
 
            }
            list.Add(e);
        }
        return list;
    }

    static DataRowCollection ReadExcel(string name, string sheet)
    {
        FileStream fs= File.Open(FilePath(name), FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader ereader=  ExcelReaderFactory.CreateOpenXmlReader(fs);
        DataSet result = ereader.AsDataSet();
        var table = result.Tables[sheet];
        var list = result.Tables[sheet].Columns;
        return result.Tables[sheet].Rows;
    }

    public static string FilePath(string name)
    {
        return Application.dataPath + "/" + name;
    }














}
