using System;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace MyThings.Multiplayer
{
    /// <summary>
    /// A Class To Set Up Unity Services
    /// </summary>
    public class UnityServices
    {
        private const int NameLengthMax = 30;


        /// <summary>
        /// Get The Currected Name To Use As Profile
        /// </summary>
        /// <param name="name">The Name</param>
        /// <returns>The Currected Name</returns>
        public static string GetCorrectedName(string name)
        {
            name = name.Trim();
            name = name.Replace(' ', '_');
            StringBuilder sB = new StringBuilder(NameLengthMax);
            for(int i = 0;i<name.Length&&i< NameLengthMax; i++)
            {
                if (Char.IsLetterOrDigit(name[i])||name[i]== '-'||name[i]== '_')
                {
                    sB.Append(name[i]);
                }
            }
            return sB.ToString();
        }
        /// <summary>
        /// Start Unity Services With The Name
        /// </summary>
        /// <param name="Name">The Name to use a sProfile</param>
        public static async Task<bool> StartUnityServices(string Name)
        {
             InitializationOptions options = new();
            options.SetProfile(GetCorrectedName(Name));
            try
            {
                await Unity.Services.Core.UnityServices.InitializeAsync(options);

                AuthenticationService.Instance.SignedIn += () =>
               {
                   Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
               };

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                await AuthenticationService.Instance.UpdatePlayerNameAsync(GetCorrectedName(Name));
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Start unity Services With No Name
        /// </summary>
        /// <returns>Wait For The Task To finish</returns>
        public static async Task StartUnityServices()
        {
            await Unity.Services.Core.UnityServices.InitializeAsync();
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