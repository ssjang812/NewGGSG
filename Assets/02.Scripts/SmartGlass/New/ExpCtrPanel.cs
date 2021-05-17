using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpCtrPanel : MonoBehaviour
{
    public static int expNumber;
    public PinchSlider PinchSlider;
    public TextMeshPro ExpNumText;

    private void Start()
    {
        ExpNumText.text = ((int)Math.Round(PinchSlider.SliderValue * 5 + 1)).ToString();
    }

    public void SliderValuetoExpNum()
    {
        int temp;
        temp = (int)Math.Round(PinchSlider.SliderValue * 5 + 1);
        if (temp != expNumber)
        {
            expNumber = temp;
            ExpNumText.text = expNumber.ToString();
        }
    }
}
