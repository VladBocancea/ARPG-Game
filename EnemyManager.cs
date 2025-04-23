using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerUtils;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemy_panel;

    [SerializeField]
    private Button button;

    private int[] numbersOfEnemy = new int[10] { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2 };
    private int[] enemiespool_forest_normal = new int[10] { 1, 0, 0, 0, 0, 1, 2, 2, 2, 2 };
    private EnemyType enemyType; // 0 - tank , 1 - mage, 2 - normal, 3 - elite, 4 - boss 
    private int numberOfEnemy_Spawned;

    private void Awake()
    {
        GenerateNumberOfEnemies();
        GenerateStatus_Enemies(MainGame.levelType, MainGame.GetCurrentLevel());
    }

    /// <summary>
    /// Generate the numbers of enemies in the current stage
    /// </summary>
    private void GenerateNumberOfEnemies()
    {
        UnityEngine.Random.InitState((int)Math.Floor(Time.deltaTime * Guid.NewGuid().GetHashCode()));

        int seed = UnityEngine.Random.Range(0, 10);

        numberOfEnemy_Spawned = numbersOfEnemy[seed];

        for (int i = 0; i < numbersOfEnemy[seed]; i++)
        {
            enemy_panel[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Select from an existing pool what enemie will be created for current stage
    /// </summary>
    /// <returns>Class type</returns>
    private EnemyType GenerateEnemy_Type()
    {
        UnityEngine.Random.InitState((int)Math.Floor(Time.deltaTime * Guid.NewGuid().GetHashCode()));
        int seed = UnityEngine.Random.Range(0, 10);

        switch (enemiespool_forest_normal[seed])
        {
            case 0:
                return EnemyType.TANK;
            case 1:
                return EnemyType.MAGE;
            case 2:
                return EnemyType.NORMAL;
            case 3:
                return EnemyType.ELITE;
            case 4: 
                return EnemyType.BOSS;

            default:
                return EnemyType.NORMAL;
        }
    }

    /// <summary>
    /// Start the generation of the enemy status
    /// </summary>
    /// <param name="levelType"> Location of the stage like forest, desert, etc</param>
    /// <param name="stageLevel"> The stage level </param>
    private void GenerateStatus_Enemies(MainGame.LevelType levelType, int stageLevel)
    {
        switch (levelType)
        {
            case MainGame.LevelType.FOREST:
                for (int i = 0; i < numberOfEnemy_Spawned; i++)
                {
                    string name = "UI/EnemyIconForest/ForestEnemyIcon" + UnityEngine.Random.Range(1, 6).ToString();
                    Sprite icon = Resources.Load<Sprite>(name);
                    var iconEn = enemy_panel[i].transform.GetChild(0).GetComponent<Image>();
                    iconEn.sprite = icon;
                    var e = enemy_panel[i].AddComponent<Enemy>();
                    e.enemy_Type = GenerateEnemy_Type();
                    if(i==0)
                    {
                        e.isSelectedAsTarget = true;
                    }
                    StartCoroutine(e.__Init__());
                }
                break;
            case MainGame.LevelType.DESERT:
                break;
            case MainGame.LevelType.MOUNTAIN:
                break;
            case MainGame.LevelType.CITY:
                break;
            case MainGame.LevelType.SNOW:
                break;
            default:
                break;
        }
    }

    public void RegenerateStatusForStageCompleted()
    {
        GenerateNumberOfEnemies();
        GenerateStatus_Enemies(MainGame.levelType, MainGame.GetLevel());
    }
}
