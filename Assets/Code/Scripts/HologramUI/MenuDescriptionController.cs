using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuDescriptionController : MonoBehaviour {
    public ScrollViewController scrollViewPrefab;

    public float textScale = 0.15f;
    
    public float leftAndRightWidthRatio = 0.3f;
    public float middleWidthRatio = 0.4f;

    public float middleTopHeightStartRatio = 0.15f;
    public float middleTopHeightEndRatio = 0.7f;

    public float spaceBetweenVerticalPartsRatio = 0.01f;
    public float spaceBetweenHorizontalPartsRatio = 0.015f;
    public float spaceForTopAndBottomRatio = 0.01f;
    public float spaceForLeftAndRightRatio = 0.01f;

    public RectTransform leftPart;
    public RectTransform middlePart;
    public RectTransform rightPart;

    public string[] leftPartTexts;
    public string[] middlePartTexts;
    public string[] rightPartTexts;

    private float refHeight;
    private float refWidth;
    private Vector2 refScale;

    void Start() {
        var rectTransform = middlePart.transform.parent.GetComponent<RectTransform>();
        var sizeDelta = rectTransform.sizeDelta;
        refWidth = sizeDelta.x;
        refHeight = sizeDelta.y;
        refScale = rectTransform.localScale;
    }

    public void Show() {
        foreach (Transform child in leftPart) Destroy(child.gameObject);
        foreach (Transform child in middlePart) Destroy(child.gameObject);
        foreach (Transform child in rightPart) Destroy(child.gameObject);

        InitializeLeftAndRightParts();
        InitializeMiddlePart();
    }

    public void Destroy() {
        foreach (Transform child in leftPart) Destroy(child.gameObject);
        foreach (Transform child in middlePart) Destroy(child.gameObject);
        foreach (Transform child in rightPart) Destroy(child.gameObject);
    }

    private void InitializeLeftAndRightParts() {
        var leftAndRight = refWidth * spaceForLeftAndRightRatio;
        var topAndBottom = refHeight * spaceForTopAndBottomRatio;
        var betweenVerticalParts = refHeight * spaceBetweenVerticalPartsRatio;
        var betweenHorizontalParts = refWidth * spaceBetweenHorizontalPartsRatio;

        var eachPartWidth = (refWidth - 2 * leftAndRight - 2 * betweenHorizontalParts) * leftAndRightWidthRatio;
        var eachSizePartHeightLeft = (refHeight - betweenVerticalParts * (leftPartTexts.Length - 1) - 2 * topAndBottom) / leftPartTexts.Length;
        var eachSizePartHeightRight = (refHeight - betweenVerticalParts * (rightPartTexts.Length - 1) - 2 * topAndBottom) / rightPartTexts.Length;

        for (var i = 0; i < leftPartTexts.Length; i++) {
            var scrollView = Instantiate(scrollViewPrefab, leftPart);
            var rectTransform = scrollView.GetComponent<RectTransform>();
            var text = leftPartTexts[i];

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);

            rectTransform.sizeDelta = new Vector2(eachPartWidth, eachSizePartHeightLeft);
            rectTransform.anchoredPosition = new Vector2(leftAndRight, topAndBottom + (eachSizePartHeightLeft * i) + (betweenVerticalParts * i));

            rectTransform.localScale = refScale * textScale;
            rectTransform.sizeDelta /= refScale * textScale;
            
            scrollView.description.text = text;
        }

        for (var i = 0; i < rightPartTexts.Length; i++) {
            var scrollView = Instantiate(scrollViewPrefab, rightPart);
            var rectTransform = scrollView.GetComponent<RectTransform>();
            var text = rightPartTexts[i];

            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 0);

            rectTransform.sizeDelta = new Vector2(eachPartWidth, eachSizePartHeightRight);
            rectTransform.anchoredPosition = new Vector2(-eachPartWidth - leftAndRight,
                topAndBottom + (eachSizePartHeightRight * i) + (betweenVerticalParts * i));
            
            rectTransform.localScale = refScale * textScale;
            rectTransform.sizeDelta /= refScale * textScale;

            scrollView.description.text = text;
        }   
    }

    private void InitializeMiddlePart() {
        var leftAndRight = refWidth * spaceForLeftAndRightRatio;
        var betweenVerticalParts = refHeight * spaceBetweenVerticalPartsRatio;
        var betweenHorizontalParts = refWidth * spaceBetweenHorizontalPartsRatio;

        var eachPartWidth = (refWidth - 2 * leftAndRight - 2 * betweenHorizontalParts) * middleWidthRatio;
        var eachPartHeight = (refHeight - betweenVerticalParts * (middlePartTexts.Length - 1)) / middlePartTexts.Length *
                             (middleTopHeightEndRatio - middleTopHeightStartRatio);

        for (var i = 0; i < middlePartTexts.Length; i++) {
            var scrollView = Instantiate(scrollViewPrefab, middlePart);
            var rectTransform = scrollView.GetComponent<RectTransform>();
            var text = middlePartTexts[i];

            rectTransform.anchorMin = new Vector2(0.5f, 1);
            rectTransform.anchorMax = new Vector2(0.5f, 1);

            rectTransform.sizeDelta = new Vector2(eachPartWidth, eachPartHeight);
            rectTransform.anchoredPosition = new Vector2(-eachPartWidth / 2,
                -refHeight * middleTopHeightStartRatio - eachPartHeight - (eachPartHeight * i) - (betweenVerticalParts * i));
            
            rectTransform.localScale = refScale * textScale;
            rectTransform.sizeDelta /= refScale * textScale;

            scrollView.description.text = text;
        }
    }
}