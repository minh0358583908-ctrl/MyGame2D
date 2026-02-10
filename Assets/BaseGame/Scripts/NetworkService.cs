using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace BaseGame
{
    public enum WebRequestMethod { Get, Post, Put, Delete }

    [Serializable]
    public class WebRequestModel
    {
        public string url;
        public WebRequestMethod method;
        public int timeout;
        public string reqData;
        public UnityWebRequest.Result result = UnityWebRequest.Result.InProgress;
        public long rspCode = 0;
        public string rspData = "";

        public WebRequestModel(string url, WebRequestMethod method, int timeout = 3, string reqData = "")
        {
            this.url = url;
            this.method = method;
            this.timeout = timeout;
            this.reqData = reqData;
        }
    }

    public static class NetworkService
    {
        private const string UrlGoogle204 = "https://www.google.com/generate_204";

        public static async Task<bool> IsActive()
        {
            var webReqModel = new WebRequestModel(UrlGoogle204, WebRequestMethod.Get, 1);
            var result = await SendWebRequest(webReqModel);
            return result.result == UnityWebRequest.Result.Success;
        }

        public static async Task<WebRequestModel> SendWebRequest(WebRequestModel webReqModel)
        {
            UnityWebRequest webRequest = null;
            switch (webReqModel.method)
            {
                case WebRequestMethod.Get:
                    webRequest = UnityWebRequest.Get(webReqModel.url);
                    break;
                case WebRequestMethod.Post:
                    webRequest = new UnityWebRequest(webReqModel.url, "POST");
                    webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(webReqModel.reqData));
                    webRequest.downloadHandler = new DownloadHandlerBuffer();
                    webRequest.SetRequestHeader("Content-Type", "application/json");
                    break;
                case WebRequestMethod.Put:
                    webRequest = new UnityWebRequest(webReqModel.url, "PUT");
                    webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(webReqModel.reqData));
                    webRequest.downloadHandler = new DownloadHandlerBuffer();
                    webRequest.SetRequestHeader("Content-Type", "application/json");
                    break;
                case WebRequestMethod.Delete:
                    webRequest = UnityWebRequest.Delete(webReqModel.url);
                    break;
                default:
                    webReqModel.result = UnityWebRequest.Result.ProtocolError;
                    return webReqModel;
            }

            webRequest.timeout = webReqModel.timeout;
            // await webRequest.SendWebRequest();

            webReqModel.result = webRequest.result;
            webReqModel.rspCode = webRequest.responseCode;
            if (webRequest.result == UnityWebRequest.Result.Success)
                webReqModel.rspData = webRequest.downloadHandler.text;

            return webReqModel;
        }
    }
}