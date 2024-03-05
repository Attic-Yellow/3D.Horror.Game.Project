using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneController : MonoBehaviour
{
    private void Update()
    {
        CloseOverlay();
    }

    private void CloseOverlay()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.overlayManager.CheckOnchacterSettingOverlay())
            {
                GameManager.instance.overlayManager.ChacterSettingOverlayController();
            }
            else
            {
                GameManager.instance.overlayManager.OptionOverlayController();
            }
        }
    }
}
