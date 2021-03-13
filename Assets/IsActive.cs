using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActive : MonoBehaviour {
    private void Start() {
        gameObject.SetActive(false);
        Debug.Log($"is Active asdfh");
    }

    public void SetUnitActive(bool isActive) {
        gameObject.SetActive(isActive);
    }
}