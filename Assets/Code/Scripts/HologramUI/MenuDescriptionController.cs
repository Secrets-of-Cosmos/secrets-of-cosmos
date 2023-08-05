using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuDescriptionController : MonoBehaviour
{
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

    public MenuDescription[] LeftPartTexts;
    public MenuDescription[] MiddlePartTexts;
    public MenuDescription[] RightPartTexts;

    private float refHeight;
    private float refWidth;
    private Vector2 refScale;

    void Start()
    {
        var rectTransform = middlePart.transform.parent.GetComponent<RectTransform>();
        var sizeDelta = rectTransform.sizeDelta;
        refWidth = sizeDelta.x;
        refHeight = sizeDelta.y;
        refScale = rectTransform.localScale;
    }

    public void Show()
    {
        foreach (Transform child in leftPart) Destroy(child.gameObject);
        foreach (Transform child in middlePart) Destroy(child.gameObject);
        foreach (Transform child in rightPart) Destroy(child.gameObject);

        InitializeLeftAndRightParts();
        InitializeMiddlePart();
    }

    public void UpdateTexts()
    {
        foreach (Transform child in leftPart)
        {
            child.GetComponent<ScrollViewController>().header.text = LeftPartTexts[child.GetSiblingIndex()].Header;
            child.GetComponent<ScrollViewController>().description.text = LeftPartTexts[child.GetSiblingIndex()].Text;
        }

        foreach (Transform child in middlePart)
        {
            child.GetComponent<ScrollViewController>().header.text = MiddlePartTexts[child.GetSiblingIndex()].Header;
            child.GetComponent<ScrollViewController>().description.text = MiddlePartTexts[child.GetSiblingIndex()].Text;
        }

        foreach (Transform child in rightPart)
        {
            child.GetComponent<ScrollViewController>().header.text = RightPartTexts[child.GetSiblingIndex()].Header;
            child.GetComponent<ScrollViewController>().description.text = RightPartTexts[child.GetSiblingIndex()].Text;
        }
    }

    public void Destroy()
    {
        foreach (Transform child in leftPart) Destroy(child.gameObject);
        foreach (Transform child in middlePart) Destroy(child.gameObject);
        foreach (Transform child in rightPart) Destroy(child.gameObject);
    }

    private void InitializeLeftAndRightParts()
    {
        var leftAndRight = refWidth * spaceForLeftAndRightRatio;
        var topAndBottom = refHeight * spaceForTopAndBottomRatio;
        var betweenVerticalParts = refHeight * spaceBetweenVerticalPartsRatio;
        var betweenHorizontalParts = refWidth * spaceBetweenHorizontalPartsRatio;

        var eachPartWidth = (refWidth - 2 * leftAndRight - 2 * betweenHorizontalParts) * leftAndRightWidthRatio;
        var eachSizePartHeightLeft = (refHeight - betweenVerticalParts * (LeftPartTexts.Length - 1) - 2 * topAndBottom) / LeftPartTexts.Length;
        var eachSizePartHeightRight = (refHeight - betweenVerticalParts * (RightPartTexts.Length - 1) - 2 * topAndBottom) / RightPartTexts.Length;

        for (var i = 0; i < LeftPartTexts.Length; i++)
        {
            var scrollView = Instantiate(scrollViewPrefab, leftPart);
            var rectTransform = scrollView.GetComponent<RectTransform>();
            var menuDescription = LeftPartTexts[i];

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);

            rectTransform.sizeDelta = new Vector2(eachPartWidth, eachSizePartHeightLeft);
            rectTransform.anchoredPosition = new Vector2(leftAndRight, topAndBottom + (eachSizePartHeightLeft * i) + (betweenVerticalParts * i));

            rectTransform.localScale = refScale * textScale;
            rectTransform.sizeDelta /= refScale * textScale;

            scrollView.header.text = menuDescription.Header;
            scrollView.description.text = menuDescription.Text;
        }

        for (var i = 0; i < RightPartTexts.Length; i++)
        {
            var scrollView = Instantiate(scrollViewPrefab, rightPart);
            var rectTransform = scrollView.GetComponent<RectTransform>();
            var menuDescription = RightPartTexts[i];

            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 0);

            rectTransform.sizeDelta = new Vector2(eachPartWidth, eachSizePartHeightRight);
            rectTransform.anchoredPosition = new Vector2(-eachPartWidth - leftAndRight,
                topAndBottom + (eachSizePartHeightRight * i) + (betweenVerticalParts * i));

            rectTransform.localScale = refScale * textScale;
            rectTransform.sizeDelta /= refScale * textScale;

            scrollView.header.text = menuDescription.Header;
            scrollView.description.text = menuDescription.Text;
        }
    }

    private void InitializeMiddlePart()
    {
        var leftAndRight = refWidth * spaceForLeftAndRightRatio;
        var betweenVerticalParts = refHeight * spaceBetweenVerticalPartsRatio;
        var betweenHorizontalParts = refWidth * spaceBetweenHorizontalPartsRatio;

        var eachPartWidth = (refWidth - 2 * leftAndRight - 2 * betweenHorizontalParts) * middleWidthRatio;
        var eachPartHeight = (refHeight - betweenVerticalParts * (MiddlePartTexts.Length - 1)) / MiddlePartTexts.Length *
                             (middleTopHeightEndRatio - middleTopHeightStartRatio);

        for (var i = 0; i < MiddlePartTexts.Length; i++)
        {
            var scrollView = Instantiate(scrollViewPrefab, middlePart);
            var rectTransform = scrollView.GetComponent<RectTransform>();
            var menuDescription = MiddlePartTexts[i];

            rectTransform.anchorMin = new Vector2(0.5f, 1);
            rectTransform.anchorMax = new Vector2(0.5f, 1);

            rectTransform.sizeDelta = new Vector2(eachPartWidth, eachPartHeight);
            rectTransform.anchoredPosition = new Vector2(-eachPartWidth / 2,
                -refHeight * middleTopHeightStartRatio - eachPartHeight - (eachPartHeight * i) - (betweenVerticalParts * i));

            rectTransform.localScale = refScale * textScale;
            rectTransform.sizeDelta /= refScale * textScale;

            scrollView.header.text = menuDescription.Header;
            scrollView.description.text = menuDescription.Text;
        }
    }

    [System.Serializable]
    public struct MenuDescription
    {
        public string Header;
        public string Text;
    }
}