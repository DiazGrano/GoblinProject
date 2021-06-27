using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private GameManager gManager;
    public List<Tile> visibleTiles = new List<Tile>();
    public List<CharController> visibleCharacters = new List<CharController>();
    public List<EnvironmentObject> visibleEnvironmentObjects = new List<EnvironmentObject>();

    public List<GameObject> modifiedObjects = new List<GameObject>();

    [Tooltip("Every X seconds is a whole day")]
    public float dayTimeScale = 120f;


    public int currentDay;

    [Range(0, 24)]
    public int currentHour;
    [Range(0, 60)]
    public int currentMinute;

    private Coroutine dayCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        this.gManager = GameManager.sharedInstance;
        this.dayCoroutine = StartCoroutine(this.DayTime());
        //StartCoroutine(DayLight());
    }

    IEnumerator DayTime()
    {
        Debug.Log("Corrutina de día iniciada");
        float auxDayTime = ((this.dayTimeScale/24f)/60f);
        while (true)
        {
            if (this.currentMinute >= 60)
            {
                this.currentMinute = 0;
                this.currentHour++;
            }
            if (this.currentHour >= 24)
            {
                this.currentHour = 0;
                this.currentMinute = 0;
                this.currentDay++;
            }
            if (this.currentHour == 8 && this.currentMinute == 0)
            {
                Debug.Log("Día");
                this.SetLight(true);
            }
            if (this.currentHour == 20 && this.currentMinute == 0)
            {
                Debug.Log("Noche");
                this.SetLight(false);
            }
            yield return new WaitForSeconds(auxDayTime);
            this.currentMinute++;
        }


    }
    

    private void SetLight(bool morning)
    {
        if (morning)
        {
            foreach (CharController character in this.visibleCharacters)
            {
                character.spriteHandler.ChangeColorOverTime(0, Color.white);
            }
            foreach (Tile tile in this.visibleTiles)
            {
                tile.spriteHandler.ChangeColorOverTime(0, Color.white);
            }
            foreach (EnvironmentObject eObject in this.visibleEnvironmentObjects)
            {
                eObject.spriteHandler.ChangeColorOverTime(0, Color.white);
            }
            this.gManager.SetDayState(DayState.Day);
        }
        else
        {
            foreach (CharController character in this.visibleCharacters)
            {
                character.spriteHandler.ChangeColorOverTime(0, Color.gray);
            }
            foreach (Tile tile in this.visibleTiles)
            {
                tile.spriteHandler.ChangeColorOverTime(0, Color.gray);
            }
            foreach (EnvironmentObject eObject in this.visibleEnvironmentObjects)
            {
                eObject.spriteHandler.ChangeColorOverTime(0, Color.gray);
            }
            this.gManager.SetDayState(DayState.Night);
        }
    }
    /*
    IEnumerator DayLight()
    {
        while (true)
        {
            if (this.currentHour == 0 && this.currentMinute == 0)
            {
                foreach (CharController character in this.visibleCharacters)
                {
                    character.spriteHandler.ChangeColorOverTime(0, Color.gray);
                }
                foreach (Tile tile in this.visibleTiles)
                {
                    tile.spriteHandler.ChangeColorOverTime(0, Color.gray);
                }
                foreach (EnvironmentObject eObject in this.visibleEnvironmentObjects)
                {
                    eObject.spriteHandler.ChangeColorOverTime(0, Color.gray);
                }
            }
            if (this.currentHour == 12 && this.currentMinute == 0)
            {
                foreach (CharController character in this.visibleCharacters)
                {
                    character.spriteHandler.ChangeColorOverTime(0, Color.white);
                }
                foreach (Tile tile in this.visibleTiles)
                {
                    tile.spriteHandler.ChangeColorOverTime(0, Color.white);
                }
                foreach (EnvironmentObject eObject in this.visibleEnvironmentObjects)
                {
                    eObject.spriteHandler.ChangeColorOverTime(0, Color.white);
                }
            }
            

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }*/


    public void SetVisible(GameObjectType gObjectType, GameObject gObject, bool isVisible = true)
    {
        switch (gObjectType)
        {
            case GameObjectType.Tile:
                Tile tile = this.gManager.GetTileComponent(gObject);
                if (tile)
                {
                    if (isVisible)
                    {
                        if (!this.visibleTiles.Contains(tile))
                        {
                            this.visibleTiles.Add(tile);
                        }
                    }
                    else
                    {
                        this.visibleTiles.Remove(tile);
                    }
                }
                break;
            case GameObjectType.Character:
                CharController charController = this.gManager.GetCharControllerComponent(gObject);
                if (charController)
                {
                    if (isVisible)
                    {
                        if (!this.visibleCharacters.Contains(charController))
                        {
                            this.visibleCharacters.Add(charController);
                        }
                    }
                    else
                    {
                        this.visibleCharacters.Remove(charController);
                    }
                }
                break;
            case GameObjectType.Environment:
                EnvironmentObject environmentObject = this.gManager.GetEnvironmentObjectComponent(gObject);
                if (environmentObject)
                {
                    if (isVisible)
                    {
                        if (!this.visibleEnvironmentObjects.Contains(environmentObject))
                        {
                            this.visibleEnvironmentObjects.Add(environmentObject);
                        }
                    }
                    else
                    {
                        this.visibleEnvironmentObjects.Remove(environmentObject);
                    }
                    
                }
                break;
        }
    }
}
