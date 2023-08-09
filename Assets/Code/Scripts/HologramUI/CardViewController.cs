using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardViewController : MonoBehaviour {
    public Text header;
    public Text description;
    public GameObject button;
    public Text buttonLabel;
    public UnityEvent<ClickedButtonInfo> buttonClickedEvent;

    public void ButtonClicked() {
        var buttonInfo = new ClickedButtonInfo {
            buttonName = buttonLabel.text,
            cardHeader = header.text,
            cardDesc = description.text
        };
        buttonClickedEvent.Invoke(buttonInfo);
    }
    
}

[System.Serializable]
public struct ClickedButtonInfo {
    public string cardHeader;
    public string cardDesc;
    public string buttonName;
}
