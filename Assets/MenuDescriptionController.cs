using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDescriptionController : MonoBehaviour {
    public ScrollViewController scrollViewPrefab;
    
    public float leftAndRightWidthRatio = 0.3f;
    public float middleWidthRatio = 0.4f;
    
    public float middleTopHeightStartRatio = 0.1f;
    public float middleTopHeightEndRatio = 0.6f;
    
    public float spaceBetweenVerticalParts = 50;
    public float spaceBetweenHorizontalParts = 75;
    public float spaceForTopAndBottom = 50;
    public float spaceForLeftAndRight = 50;

    public RectTransform leftPart;
    public RectTransform middlePart;
    public RectTransform rightPart;

    public string[] leftPartTexts;
    public string[] middlePartTexts;
    public string[] rightPartTexts;

    private float referenceHeight;
    private float referenceWidth;

    void Start() {
        var canvasScaler = middlePart.transform.parent.GetComponent<CanvasScaler>();
        referenceHeight = canvasScaler.referenceResolution.y;
        referenceWidth = canvasScaler.referenceResolution.x;
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
        var eachPartWidth = (referenceWidth - 2 * spaceForLeftAndRight - 2 * spaceBetweenHorizontalParts) * leftAndRightWidthRatio;

        var eachSizePartHeightLeft =
            (referenceHeight - spaceBetweenVerticalParts * (leftPartTexts.Length - 1) - 2 * spaceForTopAndBottom) /
            leftPartTexts.Length;
        
        for (var i = 0; i < leftPartTexts.Length; i++) {
            var scrollView = Instantiate(scrollViewPrefab, leftPart);
            var rectTransform = scrollView.GetComponent<RectTransform>();
            var text = leftPartTexts[i];

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);

            rectTransform.sizeDelta = new Vector2(eachPartWidth, eachSizePartHeightLeft);
            rectTransform.anchoredPosition = new Vector2(
                spaceForLeftAndRight,
                spaceForTopAndBottom + (eachSizePartHeightLeft * i) + (spaceBetweenVerticalParts * i)
            );
            
            scrollView.description.text = text;
        }
        
        var eachSizePartHeightRight =
            (referenceHeight - spaceBetweenVerticalParts * (rightPartTexts.Length - 1) - 2 * spaceForTopAndBottom) /
            rightPartTexts.Length;
        
        for (var i = 0; i < rightPartTexts.Length; i++) {
            var scrollView = Instantiate(scrollViewPrefab, rightPart);
            var rectTransform = scrollView.GetComponent<RectTransform>();
            var text = rightPartTexts[i];
            
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 0);

            rectTransform.sizeDelta = new Vector2(eachPartWidth, eachSizePartHeightRight);
            rectTransform.anchoredPosition = new Vector2(
                - eachPartWidth - spaceForLeftAndRight,
                spaceForTopAndBottom + (eachSizePartHeightRight * i) + (spaceBetweenVerticalParts * i)
            );

            scrollView.description.text = text;
        }
    }

    private void InitializeMiddlePart() {
        var eachPartWidth = (referenceWidth - 2 * spaceForLeftAndRight - 2 * spaceBetweenHorizontalParts) * middleWidthRatio;
        var eachPartHeight = (referenceHeight - 2 * spaceForTopAndBottom - spaceBetweenVerticalParts) / 2;
        eachPartHeight *= (middleTopHeightEndRatio - middleTopHeightStartRatio);
        
        for (var i = 0; i < middlePartTexts.Length; i++) {
            var scrollView = Instantiate(scrollViewPrefab, middlePart);
            var rectTransform = scrollView.GetComponent<RectTransform>();
            var text = middlePartTexts[i];
            
            rectTransform.anchorMin = new Vector2(0.5f, 1);
            rectTransform.anchorMax = new Vector2(0.5f, 1);
            
            rectTransform.sizeDelta = new Vector2(eachPartWidth, eachPartHeight);
            rectTransform.anchoredPosition = new Vector2(
                - eachPartWidth / 2,
                - referenceHeight * middleTopHeightStartRatio
                - eachPartHeight - spaceForTopAndBottom - (eachPartHeight * i) - (spaceBetweenVerticalParts * i)
            );

            scrollView.description.text = text;
        }
    }
}