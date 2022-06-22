
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public static class DataProcess 
{
    public static List<List<float>> datList = new List<List<float>>();
    public static List<List<float>> copyList = new List<List<float>>();
    public static int saveVertCount;
    public static Dictionary<string, List<int>> triangleList = new Dictionary<string, List<int>>();
    private static List<float> maxDatList = new List<float>();
    //make new method that rescales data from scaled data
    public static void ReScaleDat(float min, float max)
    {
        var scaledMin = min / max;
        for (int j = 0; j < datList.Count; j++)
        {
            for (int i = 0; i < datList[j].Count; i++)
            {
                var dat = copyList[j][i];
                var normalizedVal = dat / max;
                datList[j][i] = (normalizedVal < scaledMin) ? scaledMin : (normalizedVal > 1) ? 1 : normalizedVal;
            }
        }
    }
    public static void SortDat(StreamReader streamReader)
    {
        List<float> dat = new List<float>();
        float maxDat_val = 0;
        
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            var datVal = float.Parse(line);
            
            if (datVal > maxDat_val)
            {
                maxDat_val = datVal;
            }

            dat.Add(datVal);
        }
        copyList.Add(new List<float>(dat));
        dat = normalizeDat(dat, maxDat_val);
        datList.Add(new List<float>(dat));
        maxDatList.Add(maxDat_val);
    }

    public static List<float> normalizeDat(List<float> datList, float maxDat_val)
    {
        for (int i = 0; i < datList.Count; i++)
        {
            datList[i] = datList[i] / maxDat_val;
        }
        return datList;
    }

    public static List<string> parseDatList(string[] datfiles)
    {
        List<string> datNames = new List<string>();
        foreach (var path in datfiles)
        {
            string[] subs = path.Split('\\');
            datNames.Add(subs.Last().Split('.')[0]);
        }
        
        return datNames;
    } 
}
