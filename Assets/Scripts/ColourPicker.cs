using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ValueSeperator
{
    public class ColourPicker : MonoBehaviour
    {
        private Image img;
        private Camera cmra;
        
        private WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

        private int scrWidth;
        private int scrHeight;

        private void Awake() 
        {
            cmra = Camera.main;
            img = GetComponent<Image>();

            scrWidth = Screen.width;
            scrHeight = Screen.height;
        }

        private void OnRectTransformDimensionsChange()
        {
            scrWidth = Screen.width;
            scrHeight = Screen.height;            
        }

        private void Update()
        {
            StartCoroutine(ReadScreenPixel());
        }


        public IEnumerator ReadScreenPixel()
        {
            yield return frameEnd;

            Texture2D tex = new Texture2D(scrWidth, scrHeight, TextureFormat.RGB24, false);
            RenderTexture rTex = cmra.activeTexture;
            RenderTexture.active = rTex;

            Vector2 mousePos = Input.mousePosition;

            if(mousePos.x > 0 && mousePos.x < scrWidth && mousePos.y > 0 && mousePos.y < scrHeight)
            {
                tex.ReadPixels(new Rect(mousePos, Vector2.one), 0,0);

                img.color = tex.GetPixel(0,0);
            }

            Texture2D.Destroy(tex);
            RenderTexture.ReleaseTemporary(rTex);
        }
    }
}
