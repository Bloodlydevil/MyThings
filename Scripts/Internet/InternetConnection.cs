using MyThings.ExtendableClass;
using MyThings.Timer;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MyThings.Internet
{
    /// <summary>
    ///  A Connector To Check For The Internet Connection
    /// </summary>
    public class InternetConnection : Singleton_C<InternetConnection>
    {
        private const string WebSite = "https://www.google.com";
        private const float ReConnectionTime = 4;

        private Scheduler scheduler;
        /// <summary>
        /// If Connected To Internet
        /// </summary>
        public bool Connected { get; private set; }

        public event Action OnInternetConnected;
        public event Action<bool> OnInternetPing;
        public event Action OnInternetDisConnected;
        private void Start()
        {
            scheduler = new Scheduler(ReConnectionTime, true);
            StartCoroutine(CheckInternetConnection(WebSite));
        }
        private IEnumerator CheckInternetConnection(string url)
        {
            UnityWebRequest request = new UnityWebRequest(url);
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.Success)
            {
                if (!Connected)
                    OnInternetConnected?.Invoke();
                OnInternetPing?.Invoke(true);
                Connected = true;
            }
            else
            {
                if(Connected)
                    OnInternetDisConnected?.Invoke();
                OnInternetPing?.Invoke(false);
                Connected = false;
            }
            scheduler.Schedule(() => StartCoroutine(CheckInternetConnection(url)));
        }
    }
}