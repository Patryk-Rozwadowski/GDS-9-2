using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActive : MonoBehaviour {
    private void Start() {
    }

    public void SetUnitActive(bool isActive) {
        gameObject.SetActive(isActive);
    }
}