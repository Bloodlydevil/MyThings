using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace MyThings.Multiplayer
{
    /// <summary>
    /// A Class To Set Up Unity Services
    /// </summary>
    public class N_UnityServices
    {
        /// <summary>
        /// Get The Currected Name To Use As Profile
        /// </summary>
        /// <param name="name">The Name</param>
        /// <returns>The Currected Name</returns>
        public static string GetCorrectedName(string name)
        {
            name = name.Trim();
            name = name.Replace(' ', '_');
            return name;
        }
        /// <summary>
        /// Start Unity Services With The Name
        /// </summary>
        /// <param name="Name">The Name to use a sProfile</param>
        public static async void StartUnityServices(string Name)
        {
             InitializationOptions options = new();
            options.SetProfile(GetCorrectedName(Name));
            await UnityServices.InitializeAsync(options);
             AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            };
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
        /// <summary>
        /// Start unity Services With No Name
        /// </summary>
        /// <returns>Wait For The Task To finish</returns>
        public static async Task StartUnityServices()
        {
            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            };
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}