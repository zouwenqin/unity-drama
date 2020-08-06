using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QRCodeTest : MonoBehaviour
{
    public RawImage cameraTexture;
    public RawImage QRCode;

    private WebCamTexture webCamTexture;

    Color32[] data;
    BarcodeReader barcodeReader;
    BarcodeWriter barcodeWriter;

    float interval = 0f;
    private void Start()
    {
        //打开了摄像头
        WebCamDevice[] devices = WebCamTexture.devices;
        string deviceName = devices[0].name;
        webCamTexture = new WebCamTexture(deviceName, 400, 300);
        cameraTexture.texture = webCamTexture;
        webCamTexture.Play();

        barcodeReader = new BarcodeReader();


    }

    private void Update()
    {
        interval += Time.deltaTime;
        if(interval >= 3f)
        {
            ScanQRCode();
            interval = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //在这种写法里，只能填入256
            ShowQRCode("火影",256, 256);
            
        }
    }

    /// <summary>
    /// 调摄像头扫描二维码
    /// </summary>
    void ScanQRCode()
    {
        data =  webCamTexture.GetPixels32();

        var result = barcodeReader.Decode(data, webCamTexture.width, webCamTexture.height);
        if(result != null)
        {

        } 

    }

    void ShowQRCode(string str, int width, int height)
    {
        //定义texture2D并填充
        Texture2D t = new Texture2D(width, height);
        t.SetPixels32(GeneQRCode(str, width, height));
        t.Apply();

        QRCode.texture = t;

    }

    Color32[] GeneQRCode(string formatStr, int width, int height)
    {
        ZXing.QrCode.QrCodeEncodingOptions options = new ZXing.QrCode.QrCodeEncodingOptions();
        options.CharacterSet = "UTF-8";// 设置字符编码，否则中文不能显示
        options.Width = width;
        options.Height = height;
        options.Margin = 5; //二维码离边缘的空白
        
        barcodeWriter = new BarcodeWriter {Format = ZXing.BarcodeFormat.QR_CODE,Options = options };

        
        barcodeWriter.Write(formatStr);

        return barcodeWriter.Write(formatStr);
    }
}
