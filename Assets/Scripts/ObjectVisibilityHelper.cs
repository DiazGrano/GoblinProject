﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisibilityHelper : MonoBehaviour
{

    public GameObjectType objectType;
    private GameManager gManager;

    [Header("Solo es necesario asignar el correspondiente")]
    [SerializeField]
    private CharController cController = null;

    [SerializeField]
    private EnvironmentObject eObject = null;
    private void Start()
    {
        this.gManager = GameManager.sharedInstance;
    }

    private void OnBecameVisible()
    {
        switch (objectType)
        {
            case GameObjectType.Tile:
                break;
            case GameObjectType.Character:
                if (this.cController)
                {
                    this.gManager.visibleCharacters.Add(this.cController);
                    GameManager.sharedInstance.environmentManager.SetVisible(objectType, this.cController.gameObject, true);
                }
                break;
            case GameObjectType.Environment:
                if (this.eObject)
                {
                    this.gManager.visibleEnvironmentObjects.Add(this.eObject);
                    GameManager.sharedInstance.environmentManager.SetVisible(objectType, this.eObject.gameObject, true);
                }
                break;
        }
        

    }

    private void OnBecameInvisible()
    {
        switch (objectType)
        {
            case GameObjectType.Tile:
                break;
            case GameObjectType.Character:
                this.gManager.visibleCharacters.Remove(this.cController);
                GameManager.sharedInstance.environmentManager.SetVisible(objectType, this.cController.gameObject, false);
                break;
            case GameObjectType.Environment:
                this.gManager.visibleEnvironmentObjects.Remove(this.eObject);
                GameManager.sharedInstance.environmentManager.SetVisible(objectType, this.eObject.gameObject, false);
                break;
        }
        

    }
}
