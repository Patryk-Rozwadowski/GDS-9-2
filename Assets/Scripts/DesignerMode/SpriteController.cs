using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteController : MonoBehaviour {
    [SerializeField] private Sprite designMode;
    public static bool DesignMode = false;
    private Sprite normalMode;
    private SpriteRenderer spriteRenderer;
    private Image image;

    void Start() {
        TryGetComponent(out spriteRenderer);
        if (TryGetComponent(out image)) {
            normalMode = image.sprite;
        }

        InvokeRepeating(nameof(ChcekMode), 0f, 0.2f);
    }

    private void AssignSprite(Sprite sprite) {
        if (sprite == null) {
            Debug.LogWarning("Missing Sprite!");
            return;
        }

        if (spriteRenderer) {
            if (spriteRenderer.sprite == sprite) {
                return;
            }

            spriteRenderer.sprite = sprite;
        }
        else if (image) {
            if (image.sprite == sprite) {
                return;
            }

            image.sprite = sprite;
            image.SetNativeSize();
        }
        else {
            Debug.LogWarning("Missing Image or SpriteRenderer");
        }
    }

    private void ChcekMode() {
        if (DesignMode) {
            AssignSprite(designMode);
        }
        else {
            AssignSprite(normalMode);
        }
    }
}