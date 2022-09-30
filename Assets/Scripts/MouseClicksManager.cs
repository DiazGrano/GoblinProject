using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


public enum Click
{
    Left,
    Right,
    Mobile
}
public class MouseClicksManager : MonoBehaviour
{
   // public static MouseClicksManager sharedInstance;

    private GameManager gameManager;
    private Camera cam;
    private Player player;
    private TurnsManager turnsManager;
    private FightsManager fightsManager;
    private SpellsManager spellsManager;
    private SelectedCharacterOptions selectedCharOptions;

    private ResourceCostOptions resourceCostOptions;

    private void Awake()
    {
        //sharedInstance = this;
    }

    private void Start()
    {
        this.gameManager = GameManager.sharedInstance;
        this.GetPlayerInstance();
        this.cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        this.turnsManager = TurnsManager.sharedInstance;
        this.fightsManager = FightsManager.sharedInstance;
        this.spellsManager = SpellsManager.sharedInstance;
        this.selectedCharOptions = SelectedCharacterOptions.sharedInstance;
        
        this.resourceCostOptions = GameManager.sharedInstance.uiManager.resourceCostOptions;
    }

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (IsPointerOverUIObject())
                {
                    return;
                }
                else
                {
                    PointerClicked(Click.Mobile);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                else
                {
                    PointerClicked(Click.Right);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                else
                {
                    PointerClicked(Click.Left);
                }
            }
        }
    }


    private void PointerClicked(Click click)
    {
        this.GetPlayerInstance();


        if (this.selectedCharOptions.characterOptionsOpen())
        {
            this.selectedCharOptions.ForceClose();
        }
        if (click == Click.Left)
        {
            if (this.resourceCostOptions.IsVisible())
            {
                this.resourceCostOptions.ShowSpellCastOptions(0, 0, null, false);
            }
        }
        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        else
        {
            RaycastHit2D hit = Cast2DRayOnMousePosition();
            if (hit)
            {
                this.ColliderClicked(click, hit);
                return;
            }
            else
            {
                if (click == Click.Left || click == Click.Mobile)
                {
                    Tile selectedTile = GameManager.sharedInstance.currentMap.mapMatrix.GetTileAt((Vector2Int)GetMouseCoordinate());
                    if (selectedTile != null)
                    {
                        switch (this.gameManager.gameState)
                        {
                            case GameState.Normal:
                                if (selectedTile.tileState == TileState.Enabled)
                                {
                                    player.MovePlayer(selectedTile);
                                }
                                break;

                            case GameState.SettingFight:
                                if (selectedTile.tileState == TileState.Empty)
                                {
                                    player.MovePlayer(selectedTile);
                                }
                                break;

                            case GameState.Fighting:
                                if (this.turnsManager.IsCharacterTurn(player.characterController))
                                {
                                    if (selectedTile.tileState == TileState.Enabled)
                                    {
                                        if (this.spellsManager.selectedSpell != null)
                                        {
                                            if (this.fightsManager.tilesInSpellRange.Contains(selectedTile))
                                            {
                                                this.resourceCostOptions.ShowSpellCastOptions(CharacterResourceType.ManaPoints, this.spellsManager.selectedSpell.spellData.spellCost, selectedTile);
                                            }
                                            else
                                            {
                                                this.spellsManager.CastSpell(null);
                                            }
                                        }
                                        else
                                        {
                                            List<Tile> path = PathFindingA.sharedInstance.FindPath(selectedTile, this.player.characterController.currentTile, null, false);
                                            if (path != null)
                                            {
                                                if (path.Count - 1 <= player.characterController.characterStats.CharacterResource(CharacterResourceType.MovementPoints))
                                                {
                                                    PathFindingA.sharedInstance.ShowPath(true);
                                                    this.resourceCostOptions.ShowSpellCastOptions(CharacterResourceType.MovementPoints, path.Count - 1, selectedTile);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                
            }
        }
        
    }
    private void ColliderClicked(Click click, RaycastHit2D hit)
    {
        this.GetPlayerInstance();

        CharController selectedCharacter = null;
        switch (hit.collider.tag)
        {
            case "Enemy":
                selectedCharacter = hit.collider.gameObject.GetComponent<CharController>();
                break;
            default:
                Debug.LogWarning("No se encontró una tag conocida para el collider seleccionado (" + hit.collider.gameObject.name + ")");
                return;
        }
        switch (this.gameManager.gameState)
        {
            case GameState.Editor:
                break;

            case GameState.Normal:
                switch (click)
                {
                    case Click.Left:
                        break;
                    case Click.Right:
                        this.selectedCharOptions.ShowSelectedCharacterOptions(selectedCharacter);
                        break;
                    case Click.Mobile:
                        this.selectedCharOptions.ShowSelectedCharacterOptions(selectedCharacter);
                        break;
                }
                break;

            case GameState.SettingFight:
                break;

            case GameState.Fighting:
                switch (click)
                {
                    case Click.Left:
                        if (this.turnsManager.IsCharacterTurn(player.characterController))
                        {
                            if (this.fightsManager.tilesInSpellRange.Contains(selectedCharacter.currentTile))
                            {
                                this.resourceCostOptions.ShowSpellCastOptions(CharacterResourceType.ManaPoints, this.spellsManager.selectedSpell.spellData.spellCost, selectedCharacter.currentTile, true);
                            }
                            else
                            {
                                this.spellsManager.CastSpell(null);
                            }
                        }
                        break;
                    case Click.Right:
                        break;
                    case Click.Mobile:
                        if (this.turnsManager.IsCharacterTurn(player.characterController))
                        {
                            if (this.fightsManager.tilesInSpellRange.Contains(selectedCharacter.currentTile))
                            {
                                this.resourceCostOptions.ShowSpellCastOptions(CharacterResourceType.ManaPoints, this.spellsManager.selectedSpell.spellData.spellCost, selectedCharacter.currentTile, true);
                            }
                            else
                            {
                                this.spellsManager.CastSpell(null);
                            }
                        }
                        break;
                }
                break;

            case GameState.Pause:
                break;
        }
    }

    private RaycastHit2D Cast2DRayOnMousePosition(float maxLength = 1000f)
    {
        Vector2 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = new Ray(mouseWorldPosition, Vector2.zero);

        return Physics2D.Raycast(ray.origin, ray.direction, maxLength);
    }



    private Vector3Int GetMouseCoordinate()
    {
        Vector2 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        return gameManager.currentMap.Floor.WorldToCell(mouseWorldPosition);
    }

    public bool IsPointerOverUIObject()
    {
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || EventSystem.current.currentSelectedGameObject != null)
        {
            return true;
        }

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }



    private void GetPlayerInstance()
    {
        if (this.player != null)
        {
            return;
        }
        if (GameManager.sharedInstance.currentPlayer == null)
        {
            return;
        }
        this.player = GameManager.sharedInstance.currentPlayer;
    }
}
