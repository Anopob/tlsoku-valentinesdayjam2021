﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FlexibleUI : MonoBehaviour
{
    public FlexibleUIData SkinData;

    protected virtual void OnSkinUI()
    {

    }

    public virtual void Awake()
    {
        OnSkinUI();
    }
}
