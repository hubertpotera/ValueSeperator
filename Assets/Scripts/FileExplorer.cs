using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SFB;

namespace ValueSeperator
{
    public class FileExplorer : MonoBehaviour
    {
        public string Path { get; private set; }

        public static event Action<Texture2D> ImageUpdated;

        public void OpenExplorer()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File","","",false);
            if(paths.Length > 0)
            {
                Path = paths[0];
                StartCoroutine(GetTexture());
            }
        }

        private IEnumerator GetTexture()
        {
            if(Path != null)
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + Path);

                yield return www.SendWebRequest();

                if(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Texture2D tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    if(ImageUpdated != null)
                    {
                        ImageUpdated(tex);
                    }
                }
            }
        }
    }
}
