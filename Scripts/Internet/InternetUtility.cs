using MyThings.Data;
using MyThings.ExtendableClass;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MyThings.Internet
{
    /// <summary>
    /// A Utility Class For Internet Related Things
    /// </summary>
    public class InternetUtility : Singleton_C<InternetUtility>
    {
        private Queue<RequestHolder> Requests = new Queue<RequestHolder>();
        private async void Update()
        {
            if (Requests.Count > 0)
            {
                var holder = Requests.Dequeue();
                var Web = await holder.task;
                holder.callback(Web);
            }
        }

        #region Public

        /// <summary>
        /// Make A Request And Call It When Request Has Finished
        /// </summary>
        /// <param name="URL">The Url Of The REquest</param>
        /// <param name="call">The Function To Call When Request Has Been Finished</param>
        public void MakeBasicRequest(string URL, Action<UnityWebRequest> call)
        {
            Requests.Enqueue(new RequestHolder
            {
                callback = call,
                task = MakeRequest(UnityWebRequest.Get(URL))
            });
        }
        /// <summary>
        /// Make A Request And Call It When Request Has Finished
        /// </summary>
        /// <param name="URL">The Url Of The REquest</param>
        /// <param name="call">The Function To Call When Request Has Been Finished</param>
        public void MakeTextureRequest(string URL, Action<UnityWebRequest> call)
        {
            Requests.Enqueue(new RequestHolder
            {
                callback = call,
                task = MakeRequest(UnityWebRequestTexture.GetTexture(URL))
            });
        }
        /// <summary>
        ///Make A Request And Call It When Request Has Finished
        /// </summary>
        /// <param name="call">The Function To Call When Request Has Been Finished</param>
        /// <param name="Getter">The Request To Send</param>
        public void MakeRequest(Action<UnityWebRequest> call, UnityWebRequest Getter)
        {
            Requests.Enqueue(new RequestHolder
            {
                callback = call,
                task = MakeRequest(Getter)
            });
        }
        /// <summary>
        /// Make A Basic Web Request To The Web
        /// </summary>
        /// <returns>Unity Web Request</returns>
        public async Task<UnityWebRequest> MakeRequest(UnityWebRequest Req)
        {
            await Req.SendWebRequest();

            if (Req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"could Not Connect With The Server");
                return null;
            }
            return Req;
        }

        #endregion

        public static Texture2D GetTexture(UnityWebRequest req)
        {
            Texture2D texture2D = null;
            try
            {
                texture2D= DownloadHandlerTexture.GetContent(req);
            }
            catch
            {
                Debug.LogWarning("Could Not Download Texture");
            }
            return texture2D;
        }
    }
    /// <summary>
    /// A Request Holder To Call It From Main Thread
    /// </summary>
    public struct RequestHolder
    {
        public Task<UnityWebRequest> task;
        public Action<UnityWebRequest> callback;
    }
}