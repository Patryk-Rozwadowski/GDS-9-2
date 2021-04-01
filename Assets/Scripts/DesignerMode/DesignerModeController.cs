using UnityEngine;

public class DesignerModeController : MonoBehaviour {
    public void SetDesignerMode() {
        if (SpriteController.DesignMode)
            SpriteController.DesignMode = false;
        else
            SpriteController.DesignMode = true;
    }
}