using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRCode {

    private static Color32[] Encode(string textForEncoding,int width,int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height=height,
                Width=width
            }
        };
        return writer.Write(textForEncoding);
    }


    public static void ShowCode(RawImage raw,string text)
    {
        Texture2D tx = new Texture2D(256,256);
        if (!string.IsNullOrEmpty(text))
        {
            var color32 = Encode(text, tx.width, tx.height);
            tx.SetPixels32(color32);
            tx.Apply();
            //raw.texture = tx;

            //重新赋值一张图，计算大小,避免白色边框过大  
            Texture2D encoded1;
            encoded1 = new Texture2D(190, 190);//创建目标图片大小  
            encoded1.SetPixels(tx.GetPixels(32, 32, 190, 190));
            encoded1.Apply();
            raw.texture = encoded1;
        }

    }
}
