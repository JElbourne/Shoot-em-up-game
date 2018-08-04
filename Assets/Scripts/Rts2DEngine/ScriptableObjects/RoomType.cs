using UnityEngine;

[CreateAssetMenu(fileName = "New Room Type", menuName = "Map/Room Type")]
public class RoomType : ScriptableObject {

    new public string name = "Empty Cavern";
    public Texture2D roomPattern;
    public GameObject floorTile;
    public GameObject wallTile;
    public GameObject specialItem;
    public int numberOfBatterySpawners = 1;
    public int numberOfMinerals;
    public bool spawnsEnemies;
    // TODO enemy spawner
}
