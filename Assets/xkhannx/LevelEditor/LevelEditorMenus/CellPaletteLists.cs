using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPaletteLists : MonoBehaviour
{
    [SerializeField] Transform buttonParent;
    [SerializeField] RectTransform paletteWindow;
    
    int curPalette = 0;
    int maxInd = 0;
    List<CellBrushButton> brushButtons = new List<CellBrushButton>();

    void Start()
    {
        Vector2 listRect = paletteWindow.GetComponent<RectTransform>().sizeDelta;
        listRect.y = 120;
        paletteWindow.GetComponent<RectTransform>().sizeDelta = listRect;

        foreach (Transform child in buttonParent)
        {
            if (child.GetComponent<CellBrushButton>() != null)
            {
                brushButtons.Add(child.GetComponent<CellBrushButton>());
                if (maxInd < brushButtons[brushButtons.Count - 1].paletteId)
                {
                    maxInd = brushButtons[brushButtons.Count - 1].paletteId;
                }
            }
        }

        EnablePalette();
    }
    public void SwitchPalette(bool next)
    {
        curPalette += next ? 1 : -1;
        curPalette = Mathf.Clamp(curPalette, 0, maxInd);
        EnablePalette();
    }

    void EnablePalette()
    {
        foreach (var child in brushButtons)
        {
            child.gameObject.SetActive(child.paletteId == curPalette);
        }
    }
}
