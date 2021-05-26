using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour {
    [SerializeField]
    TMPro.TMP_Text coinText;

    void Start() {
        coinText.text = "0";
    }

    public void SetCoinCount(int coins) {
        coinText.text = coins.ToString();
    }
}
