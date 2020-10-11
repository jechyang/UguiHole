using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class HoleMgr : MonoBehaviour,ICanvasRaycastFilter
{
    private readonly List<string> _holeNumbersKeywords = new List<string>()
    {
        "HOLE_ONE",
        "HOLE_TWO",
        "HOLE_THREE",
        "HOLE_FOUR"
    };
    private RectTransform _rectTransform;
    public RectTransform rectTransform => _rectTransform ?? (_rectTransform = transform as RectTransform);

    private Material _material;
    public List<Image> HoleImages = new List<Image>();
    private void Start()
    {
        _material = new Material(Shader.Find("ShaderBooks/HoleShader"));
        GetComponent<Image>().material = _material;
        RefreshHole();
    }

    private void RefreshHole()
    {
        if (HoleImages.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _holeNumbersKeywords.Count; i++)
        {
            var holeNumKeyWord = _holeNumbersKeywords[i];
            if (i == HoleImages.Count - 1)
            {
                _material.EnableKeyword(holeNumKeyWord);
            }
            else
            {
                _material.DisableKeyword(holeNumKeyWord);
            }
        }

        for (int i = 0; i < HoleImages.Count; i++)
        {
            var image = HoleImages[i];
            var texture = image.mainTexture;
            Vector3[] imgCorners = new Vector3[4];
            image.rectTransform.GetWorldCorners(imgCorners);
            Rect imgRect = new Rect(imgCorners[0].x, imgCorners[0].y, imgCorners[2].x - imgCorners[0].x,
                imgCorners[2].y - imgCorners[0].y);
            Vector3[] maskCorners = new Vector3[4];
        
            rectTransform.GetWorldCorners(maskCorners);
            Rect maskRect = new Rect(maskCorners[0].x, maskCorners[0].y, maskCorners[2].x - maskCorners[0].x,
                maskCorners[2].y - maskCorners[0].y);
            var x = (imgRect.xMin - maskRect.xMin) / (maskRect.width);
            var y = (imgRect.yMin - maskRect.yMin) / (maskRect.height);
            var targetX = x + (imgRect.width / maskRect.width);
            var targetY = y + (imgRect.height / maskRect.height);
            _material.SetTexture($"_HoleTex{i+1}",texture);
            var targetRect = new Vector4(x, y, targetX, targetY);
            _material.SetVector($"_HoleRect{i+1}",targetRect);
            var sprite = image.sprite;
            var uv = (sprite != null) ? DataUtility.GetInnerUV(sprite) : Vector4.zero;
            _material.SetVector($"_HoleUv{i+1}",uv);
        }
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (HoleImages.Count == 0)
        {
            return true;
        }

        foreach (Image image in HoleImages)
        {
            var worldPoint = eventCamera.ScreenToWorldPoint(sp);
            Vector3[] imgCorners = new Vector3[4];
            image.rectTransform.GetWorldCorners(imgCorners);
            Rect imgRect = new Rect(imgCorners[0].x, imgCorners[0].y, imgCorners[2].x - imgCorners[0].x,
                imgCorners[2].y - imgCorners[0].y);
            var result = !imgRect.Contains(worldPoint);
            if (!result)
            {
                return false;
            }
        }

        return true;
    }
}
