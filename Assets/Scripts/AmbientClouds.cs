using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientClouds : MonoBehaviour
{
    /*
    public static AmbientClouds sharedInstance;

    public Color outterCloudColor;
    public Color middlerCloudColor;
    public Color centerCloudColor;

    public float moveCloudsEvery = 0.5f;

    private int yAxisModifier;

    [Range(0.02f, 1f)]
    public float perlinNoiseScale = 0.3f;


    private GameManager gManager;

    private Coroutine cloudGeneratorCoroutine;


    public List<Tile> modifiedTiles = new List<Tile>();
    public List<CharController> modifiedCharacters = new List<CharController>();
    public List<EnvironmentObject> modifiedEnvironmentObjects = new List<EnvironmentObject>();

    private List<Tile> tilesToRemove = new List<Tile>();
    private List<CharController> charactersToRemove = new List<CharController>();
    private List<EnvironmentObject> environmentObjectToRemove = new List<EnvironmentObject>();


    bool showClouds = false;


    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        this.gManager = GameManager.sharedInstance;

        this.yAxisModifier = Mathf.FloorToInt(Random.Range(0f, 256/perlinNoiseScale));

        StartCoroutine(CloudMovement());
    }


    public void ShowClouds(bool show)
    {
        if (show)
        {
            if (this.cloudGeneratorCoroutine != null)
            {
                StopCoroutine(this.cloudGeneratorCoroutine);
                this.cloudGeneratorCoroutine = null;
            }
            this.cloudGeneratorCoroutine = StartCoroutine(CloudGenerator());
        }
        else
        {
            if (this.cloudGeneratorCoroutine != null)
            {
                StopCoroutine(this.cloudGeneratorCoroutine);
                this.cloudGeneratorCoroutine = null;

                foreach (Tile tile in this.modifiedTiles)
                {
                    tile.spriteRenderer.color = Color.white;
                }
                this.modifiedTiles.Clear();

                foreach (CharController character in this.modifiedCharacters)
                {
                    character.spriteHandler.ResetToOriginalColor();
                }

                foreach (EnvironmentObject eObject in this.modifiedEnvironmentObjects)
                {
                    eObject.spriteHandler.ResetToOriginalColor();
                }

            }
        }

        this.showClouds = show;
    }


    IEnumerator CloudGenerator()
    {
        while (true)
        {
            foreach (Tile tile in this.gManager.visibleTiles)
            {
                if (tile.tileState == TileState.Enabled)
                {
                    Color cloudColor = GetProceduralCloudColor(tile);

                    foreach (CharController character in this.gManager.visibleCharacters)
                    {
                        if (character.currentTile == tile)
                        {
                            character.spriteHandler.TemporarilyChangeColor(cloudColor);
                            if (!this.modifiedCharacters.Contains(character))
                            {
                                this.modifiedCharacters.Add(character);
                            }
                        }

                    }

                    foreach (EnvironmentObject eObject in this.gManager.visibleEnvironmentObjects)
                    {
                        if(eObject.coordinate == (Vector2Int)tile.coordinates)
                        {
                            eObject.spriteHandler.TemporarilyChangeColor(cloudColor);
                            if (!this.modifiedEnvironmentObjects.Contains(eObject))
                            {
                                this.modifiedEnvironmentObjects.Add(eObject);
                            }
                        }
                    }


                    if (tile.spriteRenderer.color != Color.gray)
                    {
                        tile.AddTemporalTileColor(cloudColor);
                        if (!this.modifiedTiles.Contains(tile))
                        {
                            this.modifiedTiles.Add(tile);
                        }
                    }
                    
                }
                
            }
            

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CloudMovement()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(this.moveCloudsEvery);

            this.yAxisModifier++;

            // El ruido de perlin se repite cada 256 coordenadas
            // Para evitar que el contador del movimiento de las nubes llegue (en algún momento), al tope,
            // se reinicia para evitar que suceda un error
            if (this.yAxisModifier * this.perlinNoiseScale >= 256)
            {
                this.yAxisModifier = 0;
            }

            if (this.showClouds)
            {
                foreach (Tile tile in this.modifiedTiles)
                {
                    if (!this.gManager.visibleTiles.Contains(tile))
                    {
                        this.tilesToRemove.Add(tile);
                        if (this.gManager.mapTileMatrix.GetCharacterAt(tile))
                        {
                            this.charactersToRemove.Add(this.gManager.mapTileMatrix.GetCharacterAt(tile));
                        }
                        if (this.gManager.mapTileMatrix.GetEnvironmentObjectAt(tile))
                        {
                            this.environmentObjectToRemove.Add(this.gManager.mapTileMatrix.GetEnvironmentObjectAt(tile));
                        }
                        
                    }
                }

                foreach (Tile tile in this.tilesToRemove)
                {
                    this.modifiedTiles.Remove(tile);
                }
                this.tilesToRemove.Clear();

                foreach (CharController character in this.charactersToRemove)
                {
                    this.modifiedCharacters.Remove(character);
                }
                this.charactersToRemove.Clear();

                foreach (EnvironmentObject eObject in this.environmentObjectToRemove)
                {
                    this.modifiedEnvironmentObjects.Remove(eObject);
                }
                this.environmentObjectToRemove.Clear();

            }

            


        }
    }

    private Color GetProceduralCloudColor(Tile tile)
    {
    

        int x = tile.coordinates.x;
        int y = tile.coordinates.y + this.yAxisModifier;
        float noise = Mathf.PerlinNoise(x * this.perlinNoiseScale, y * this.perlinNoiseScale);

        if (noise >= 0.85f)
        {
            return this.centerCloudColor;
        }
        else if (noise >= 0.75f)
        {
            return this.middlerCloudColor;
        }
        else if (noise >= 0.6f)
        {
            return this.outterCloudColor;
        }
        else
        {
           return Color.white;
        }
    }*/
    public static AmbientClouds sharedInstance;

    public Color outterCloudColor;
    public Color middlerCloudColor;
    public Color centerCloudColor;

    public Color darkestCloudColor;
    public float cloudColorDegradation = 0.1f;

    public float moveCloudsEvery = 0.5f;

    private int yAxisModifier;

    [Range(0.02f, 1f)]
    public float perlinNoiseScale = 0.3f;


    private GameManager gManager;

    private Coroutine cloudGeneratorCoroutine;


    public List<Tile> modifiedTiles = new List<Tile>();
    public List<CharController> modifiedCharacters = new List<CharController>();
    public List<EnvironmentObject> modifiedEnvironmentObjects = new List<EnvironmentObject>();

    private List<Tile> tilesToRemove = new List<Tile>();
    private List<CharController> charactersToRemove = new List<CharController>();
    private List<EnvironmentObject> environmentObjectToRemove = new List<EnvironmentObject>();

    //Solo se generará una nueva nuve si el script de movimiento ha cambiado
    private bool generateCloud = false;

    bool showClouds = false;


    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        this.gManager = GameManager.sharedInstance;

        this.yAxisModifier = Mathf.FloorToInt(Random.Range(0f, 256 / perlinNoiseScale));

        StartCoroutine(CloudMovement());
    }


    public void ShowClouds(bool show)
    {
        if (show)
        {
            if (this.cloudGeneratorCoroutine != null)
            {
                StopCoroutine(this.cloudGeneratorCoroutine);
                this.cloudGeneratorCoroutine = null;
            }
            this.cloudGeneratorCoroutine = StartCoroutine(CloudGenerator());
        }
        else
        {
            if (this.cloudGeneratorCoroutine != null)
            {
                StopCoroutine(this.cloudGeneratorCoroutine);
                this.cloudGeneratorCoroutine = null;

                foreach (Tile tile in this.modifiedTiles)
                {
                    tile.spriteRenderer.color = Color.white;
                }
                this.modifiedTiles.Clear();

                foreach (CharController character in this.modifiedCharacters)
                {
                    character.spriteHandler.ResetToOriginalColor();
                }

                foreach (EnvironmentObject eObject in this.modifiedEnvironmentObjects)
                {
                    eObject.spriteHandler.ResetToOriginalColor();
                }

            }
        }

        this.showClouds = show;
    }


    IEnumerator CloudGenerator()
    {
        while (true)
        {
            if (this.generateCloud && this.gManager.dayState == DayState.Day)
            {

                foreach (Tile tile in this.gManager.visibleTiles)
                {
                    if (tile.tileState == TileState.Enabled)
                    {
                        Color cloudColor = GetProceduralCloudColor(tile);

                        foreach (CharController character in this.gManager.visibleCharacters)
                        {
                            if (character.currentTile == tile)
                            {
                                character.spriteHandler.TemporarilyChangeColor(cloudColor);
                                if (!this.modifiedCharacters.Contains(character))
                                {
                                    this.modifiedCharacters.Add(character);
                                }
                            }

                        }

                        foreach (EnvironmentObject eObject in this.gManager.visibleEnvironmentObjects)
                        {
                            if (eObject.coordinate == (Vector2Int)tile.coordinates)
                            {
                                eObject.spriteHandler.TemporarilyChangeColor(cloudColor);
                                if (!this.modifiedEnvironmentObjects.Contains(eObject))
                                {
                                    this.modifiedEnvironmentObjects.Add(eObject);
                                }
                            }
                        }


                        if (tile.spriteRenderer.color != Color.gray)
                        {
                            tile.AddTemporalTileColor(cloudColor);
                            if (!this.modifiedTiles.Contains(tile))
                            {
                                this.modifiedTiles.Add(tile);
                            }
                        }

                    }

                }
                this.generateCloud = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CloudMovement()
    {
        while (true)
        {

            yield return new WaitForSeconds(this.moveCloudsEvery);

            this.yAxisModifier++;

            // El ruido de perlin se repite cada 256 coordenadas
            // Para evitar que el contador del movimiento de las nubes llegue (en algún momento), al tope,
            // se reinicia para evitar que suceda un error
            if (this.yAxisModifier * this.perlinNoiseScale >= 256)
            {
                this.yAxisModifier = 0;
            }

            if (this.showClouds)
            {
                foreach (Tile tile in this.modifiedTiles)
                {
                    if (!this.gManager.visibleTiles.Contains(tile))
                    {
                        this.tilesToRemove.Add(tile);
                        if (this.gManager.mapTileMatrix.GetCharacterAt(tile))
                        {
                            this.charactersToRemove.Add(this.gManager.mapTileMatrix.GetCharacterAt(tile));
                        }
                        if (this.gManager.mapTileMatrix.GetEnvironmentObjectAt(tile))
                        {
                            this.environmentObjectToRemove.Add(this.gManager.mapTileMatrix.GetEnvironmentObjectAt(tile));
                        }

                    }
                }

                foreach (Tile tile in this.tilesToRemove)
                {
                    this.modifiedTiles.Remove(tile);
                }
                this.tilesToRemove.Clear();

                foreach (CharController character in this.charactersToRemove)
                {
                    this.modifiedCharacters.Remove(character);
                }
                this.charactersToRemove.Clear();

                foreach (EnvironmentObject eObject in this.environmentObjectToRemove)
                {
                    this.modifiedEnvironmentObjects.Remove(eObject);
                }
                this.environmentObjectToRemove.Clear();

            }

            this.generateCloud = true;


        }
    }

    private Color GetProceduralCloudColor(Tile tile)
    {


        int x = tile.coordinates.x;
        int y = tile.coordinates.y + this.yAxisModifier;
        float noise = Mathf.PerlinNoise(x * this.perlinNoiseScale, y * this.perlinNoiseScale);

        Color auxColor = this.darkestCloudColor;
        
        if (noise >= 0.9f)
        {
            //return this.centerCloudColor;
            return auxColor;
        }
        else if (noise >= 0.8f) 
        {
            //return this.middlerCloudColor;
            return this.GetColor(auxColor, this.cloudColorDegradation);
             
        }
        else if (noise >= 0.7f)
        {
            //return this.outterCloudColor;
            return this.GetColor(auxColor, this.cloudColorDegradation, 1);

        }
        else if (noise >= 0.6f)
        {
            return this.GetColor(auxColor, this.cloudColorDegradation, 2);

        }
        else if (noise >= 0.5)
        {
            return this.GetColor(auxColor, this.cloudColorDegradation, 3);

        }
        else if (noise >= 0.4f)
        {
            return this.GetColor(auxColor, this.cloudColorDegradation, 4);

        }
        else
        {
            return Color.white;
        }
    }

    private Color GetColor(Color baseColor, float degradation, int modificator = 1)
    {
        float auxR;
        float auxG;
        float auxB;
        auxR = Mathf.Clamp(baseColor.r + (degradation * modificator), 0f, 1f);
        auxG = Mathf.Clamp(baseColor.g + (degradation * modificator), 0f, 1f);
        auxB = Mathf.Clamp(baseColor.b + (degradation * modificator), 0f, 1f);
        return new Color(auxR, auxG, auxB);
    }
}
