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
    public UnityEvent buttonClickedEvent;
    public ClickedButtonInfo buttonInfo;

    public void ButtonClicked() {
        buttonInfo.buttonName = buttonLabel.text;
        buttonInfo.cardHeader = header.text;
        buttonInfo.cardDesc = description.text;
        buttonClickedEvent.Invoke();
    }
    
    [System.Serializable]
    public class ClickedButtonInfo {
        public string cardHeader;
        public string cardDesc;
        public string buttonName;
    }
}
