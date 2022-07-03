using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ValueSeperator
{
    public class HudHadler : MonoBehaviour
    {
        public Slider ValueSlider1;
        public Slider ValueSlider2;
        public Slider OpacitySlider;
        public Slider BGDarknessSlider;

        public RawImage MainImage;
        public RawImage LowValueImage;
        public RawImage MidValueImage;
        public RawImage HighValueImage;
        public RawImage ZoomedImage;
        public RawImage[] Backgrounds;

        public GameObject ImagesObject;
        public GameObject ImageObject;

        private int _zoomedImageIdx = 0;

        private Texture2D _texture;
        private int _width;
        private int _height;

        private float _lowerValue;
        private float _higherValue;
        private float _opacityMultiplier;

        private void Start()
        {
            FileExplorer.ImageUpdated += LoadTexture;
            OnValueSliderMoved();
            OnOpacitySliderMoved();
            OnBGSliderMoved();
        }

        private void OnApplicationQuit() 
        {
            FileExplorer.ImageUpdated -= LoadTexture;
        }

        private void LoadTexture(Texture2D tex)
        {
            _texture = tex;
            _width = tex.width;
            _height = tex.height;
            UpdateImages();
        } 

        public void OnValueSliderMoved()
        {
            float v1 = ValueSlider1.value;
            float v2 = ValueSlider2.value;

            if(v1 > v2)
            {
                _higherValue = v1;
                _lowerValue = v2;
            }
            else
            {
                _higherValue = v2;
                _lowerValue = v1;
            }

            UpdateImages();
        }

        public void OnOpacitySliderMoved()
        {
            _opacityMultiplier = OpacitySlider.value;
            UpdateImages();
        }

        public void OnBGSliderMoved()
        {
            float val = 1-BGDarknessSlider.value;
            Color colour = new Color(val,val,val);
            for (int i = 0; i < Backgrounds.Length; i++)
            {
                Backgrounds[i].color = colour;
            }
        }

        private void UpdateImages()
        {
            if(_texture == null)
            {
                return;
            }

            Vector2 sizeDelta;
            if(_width > _height)
            {
                sizeDelta = new Vector2(200f, 200f * _height/_width);
            }
            else
            {
                sizeDelta = new Vector2(200f * _width/_height, 200f);
            }
            MainImage.rectTransform.sizeDelta = sizeDelta;
            LowValueImage.rectTransform.sizeDelta = sizeDelta;
            MidValueImage.rectTransform.sizeDelta = sizeDelta;
            HighValueImage.rectTransform.sizeDelta = sizeDelta;
            ZoomedImage.rectTransform.sizeDelta = 2*sizeDelta;
            for (int i = 0; i < Backgrounds.Length-1; i++)
            {
                Backgrounds[i].rectTransform.sizeDelta = sizeDelta;
            }
            Backgrounds[Backgrounds.Length-1].rectTransform.sizeDelta = 2*sizeDelta;

            MainImage.texture = _texture;
            LowValueImage.texture = ComputeTextureInValueRange.Compute(_texture, 0f, _lowerValue, _opacityMultiplier);
            MidValueImage.texture = ComputeTextureInValueRange.Compute(_texture, _lowerValue, _higherValue, _opacityMultiplier);
            HighValueImage.texture = ComputeTextureInValueRange.Compute(_texture, _higherValue, 1f, _opacityMultiplier);
            
            switch (_zoomedImageIdx)
            {
                case 0:
                    ZoomedImage.texture = ComputeTextureInValueRange.Compute(_texture, 0f, _lowerValue, _opacityMultiplier);
                    break;
                case 1:
                    ZoomedImage.texture = ComputeTextureInValueRange.Compute(_texture, _lowerValue, _higherValue, _opacityMultiplier);
                    break;
                case 2:
                    ZoomedImage.texture = ComputeTextureInValueRange.Compute(_texture, _higherValue, 1f, _opacityMultiplier);
                    break;
            }
        }

        public void ZoomIn(int img)
        {
            ImageObject.SetActive(true);
            ImagesObject.SetActive(false);
            _zoomedImageIdx = img;
            UpdateImages();
        }

        public void ZoomOut()
        {
            ImageObject.SetActive(false);
            ImagesObject.SetActive(true);
        }
    }
}
