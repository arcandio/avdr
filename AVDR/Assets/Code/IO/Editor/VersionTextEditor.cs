using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [ExecuteInEditMode]
    public class VersionTextEditor : MonoBehaviour
    {
        /// <summary>
        /// Gets all the version numbers into an array of ints.
        /// </summary>
        /// <returns></returns>
        static int[] GetVersionNumberInts()
        {
            string version = PlayerSettings.bundleVersion;
            string[] split = version.Split('.');

            int major = int.Parse(split[0]);
            int minor = int.Parse(split[1]);
            int patch = int.Parse(split[2]);
            int build = int.Parse(PlayerSettings.iOS.buildNumber);

            return new int[] { major, minor, patch, build };
        }

        /// <summary>
        /// Compiles all the various levels of the version into a single string.
        /// </summary>
        /// <param name="ints"></param>
        /// <returns></returns>
        static string GetVersionNumberString(int[] ints)
        {
            string str = "";
            str += ints[0];
            str += ".";
            str += ints[1];
            str += ".";
            str += ints[2];
            return str;
        }
        /// <summary>
        /// Increments the version number. When new versions are incremented, lower levels are zeroed out.
        /// Be aware: This is Version 4 of BBB. 3 versions preceed it. Continuing to internally label new versions as 1.x.y will only give future and past developers paroxisms of exasperation.
        /// <br />
        /// This method also calls `ResourceCacheGenerator.GenerateResourceCachePrefab()`, so that the AppDataCache is up to date with the new App version.
        /// </summary>
        /// <param name="IncrementDepth">0: Major. 1: Minor. 2: Patch.</param>
        static void IncrementVersionNumber(int incrementDepth)
        {
            int[] ints = GetVersionNumberInts();
            switch (incrementDepth)
            {
                case 0:
                    ints[0] += 1;
                    ints[1] = 0;
                    ints[2] = 0;
                    break;
                case 1:
                    ints[1] += 1;
                    ints[2] = 0;
                    break;
                case 2:
                default:
                    ints[2] += 1;
                    break;
                case 3: 
                    ints[3] += 1;
                    break;
            }
            string version = GetVersionNumberString(ints);
            PlayerSettings.bundleVersion = version;
            PlayerSettings.iOS.buildNumber = ints[3].ToString();
            SetAndroidBundleVersion(ints);
            // ResourceCacheGenerator.GenerateAppData();
        }
        
        /// <summary>
        /// sets the version string for android in the format of 40201
        /// </summary>
        /// <param name="ints"></param>
        static void SetAndroidBundleVersion(int[] ints)
        {
            int versionCode = ints[0] * 10000 + ints[1] * 100 + ints[2];
            PlayerSettings.Android.bundleVersionCode = versionCode;
        }

        #region MENU ITEMS


        [MenuItem("AVDR/Version/Reset Android Bundle Version Number")]
        static void ResetAndroidBundleVersion()
        {
            SetAndroidBundleVersion(GetVersionNumberInts());
        }
        [MenuItem("AVDR/Version/Increment IOS Build Number (x.y.z a++)")]
        static void IncrementIosBuildNumber()
        {
            IncrementVersionNumber(3);
        }
        [MenuItem("AVDR/Version/Increment Patch (x.y.++z)")]
        static void IncrementPatch()
        {
            IncrementVersionNumber(2);
        }
        [MenuItem("AVDR/Version/Increment Minor (x.++y.z)")]
        static void IncrementMinor()
        {
            IncrementVersionNumber(1);
        }
        [MenuItem("AVDR/Version/Increment Major (++x.y.z)")]
        static void IncrementMajor()
        {
            IncrementVersionNumber(0);
        }

        #endregion
    }
}
