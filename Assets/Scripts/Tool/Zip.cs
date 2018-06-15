using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System;
using XLua;

[Hotfix]
public sealed class Zip
{

    public static void CreateZip(string sourceFilePath, string zipFilePath)
    {
        ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFilePath));
        zipStream.SetLevel(6);// 压缩质量和压缩速度的平衡点
        CreateZipFiles(sourceFilePath, zipStream);
        zipStream.Finish();
        zipStream.Close();
    }
    private static void CreateZipFiles(string sourcePath, ZipOutputStream zipstream)
    {
        Crc32 crc = new Crc32();
        string[] filesArray = Directory.GetFileSystemEntries(sourcePath);
        foreach (var item in filesArray)
        {  
            if (Directory.Exists(item))
            {
                CreateZipFiles(item, zipstream);
            }
            else
            {
                FileStream fileStream = File.OpenRead(item);
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                string tempFile = item.Substring(item.LastIndexOf("\\")+1);
                ZipEntry entry = new ZipEntry(tempFile);
                entry.DateTime = DateTime.Now;
                entry.Size = fileStream.Length;
                entry.CompressionMethod = CompressionMethod.Stored;//要加不然会报错
                fileStream.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                zipstream.PutNextEntry(entry);
                zipstream.Write(buffer, 0, buffer.Length);
            }
        }
    }

    public static IEnumerator UnZip(string file, string outPath, Action finish)
    {
        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);
        ZipInputStream s = new ZipInputStream(File.OpenRead(file));
        ZipEntry entry = null;
        while ((entry = s.GetNextEntry()) != null)
        {
            #region  按照压缩前的文件夹解压
            //string fileName = Path.GetFileName(entry.Name);
            //string filePath = Path.Combine(outPath, entry.Name);
            // string directoryName = Path.GetDirectoryName(filePath);
            //if (!string.IsNullOrEmpty(directoryName))
            //{
            //    Directory.CreateDirectory(directoryName);
            //}
            #endregion

            //统一解压到一个文件夹下
            string fileName = Path.GetFileName(entry.Name);
            string filePath = Path.Combine(outPath, fileName);

            if (!string.IsNullOrEmpty(fileName))
            {
                try
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    FileStream streamWriter = File.Create(filePath);
                    int size = 2048;
                    byte[] data = new byte[size];
                    while (size > 0)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                    }
                    streamWriter.Close();
                    streamWriter.Dispose();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        Debug.Log("解压完成");
        File.Delete(file);
        s.Close();
        s.Dispose();
        if (finish != null)
            finish();
        yield break;
    }
}
