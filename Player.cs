using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerUtils;


public class Player : MonoBehaviour
{

    #region Name of the character
    protected string playerName;
    #endregion

    #region Player Character Image
    protected Sprite playerIcon;
    #endregion

    #region Money
    protected long money;
    #endregion

    #region Experience
    protected long experienceNeeded;
    protected long currentEXP;
    #endregion

    #region Lvl
    protected int Level = 1;
    #endregion

    #region Health
    protected float health;
    protected float healthRegen;

    protected float currentHP;
    #endregion

    #region Mana
    protected float mana;
    protected float manaRegen;

    protected float currentMana;
    #endregion

    #region Armor
    protected int armor;
    protected float blockChance;
    #endregion

    #region MagicResist
    protected int magicRezist;
    #endregion

    #region MagicDamage
    protected float magicDamage;
    #endregion

    #region AttackDamage
    protected float attackDamage;
    #endregion

    #region AttackSpeed
    protected float attackSpeed;
    #endregion

    #region CriticalDamage
    protected int criticalDamage;
    protected int criticalChance;
    protected bool[] criticalChance_pool = new bool[100];
    #endregion

    #region CooldownReduction
    protected int cooldownReduction;
    #endregion

    #region Inventory
    protected Dictionary<ARMOR_TAG, GameObject> equippedItems;
    protected List<GameObject> inventory;
    #endregion

    #region Skill Wheel
    [SerializeField]
    protected BaseSkill[] skillWheel = new BaseSkill[7];

    #endregion

    #region ClassType
    private ClassType classType;
    #endregion

    #region Utils
    private bool isFirstTimeInit = true;
    #endregion

    #region Data
    private GameObject playerPanel;
    private Queue<GameObject> blocksToHaveEffect;
    private bool nextAttackIsBlocked = false;
    private Transform contentTransform_DebuffsBuffsZone;

    private HealthBar healthBar;
    private ManaBar manaBar;
    private ExperienceBar experienceBar;

    private TextMeshProUGUI healthBarTxt;
    private TextMeshProUGUI manaBarTxt;
    private TextMeshProUGUI experienceBarTxt;

    private bool init_dataForMainGame = true;
    #endregion

    public static Player instance;

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2 && init_dataForMainGame)
        {
            blocksToHaveEffect = new Queue<GameObject>();
            contentTransform_DebuffsBuffsZone = GameObject.FindGameObjectWithTag("BDZonePlayer").transform;
            playerPanel = GameObject.FindGameObjectWithTag("Player");
            init_dataForMainGame = false;

            healthBar = GameObject.FindGameObjectWithTag("HealtBarPlayer").GetComponent<HealthBar>();
            manaBar = GameObject.FindGameObjectWithTag("ManaBarPlayer").GetComponent<ManaBar>();
            experienceBar = GameObject.FindGameObjectWithTag("ExperienceBarPLayer").GetComponent<ExperienceBar>();

            healthBar.SetMaxHealth((int)health);
            healthBar.SetHealth((int)health);

            manaBar.SetMaxMana((int)mana);
            manaBar.SetMana((int)mana);

            experienceBar.SetMaxExperience((int)experienceNeeded);
            experienceBar.SetExperience((int)experienceNeeded);

            healthBarTxt = GameObject.FindGameObjectWithTag("HealthBarTextPlayer").GetComponent<TextMeshProUGUI>();
            manaBarTxt = GameObject.FindGameObjectWithTag("ManaBarTextPlayer").GetComponent<TextMeshProUGUI>();
            experienceBarTxt = GameObject.FindGameObjectWithTag("ExperienceBarTextPlayer").GetComponent<TextMeshProUGUI>();

            healthBarTxt.text = health + " / " + health;
            manaBarTxt.text = mana + " / " + mana;
            experienceBarTxt.text = currentEXP + " / " + experienceNeeded;

            currentEXP = experienceNeeded;
            currentHP = health;
            currentMana = mana;

        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            init_dataForMainGame = true;
        }
    }

    private void Awake()
    {
        inventory = new List<GameObject>();
        inventory.Capacity = 60;
        //------- !!! Delete after testing in main game scene !!! -------
        //Critical chance pool generator (move to a separate method)
        for (int i = 0; i < 100; i++)
        {
            if (criticalChance == 0)
            {
                criticalChance_pool[i] = false;
            }
            else
            {
                if (i < criticalChance)
                {
                    criticalChance_pool[i] = true;
                }
                else
                {
                    criticalChance_pool[i] = false;
                }
            }
        }

        experienceNeeded = (int)Mathf.Ceil((float)(1f / 2f) * Mathf.Pow(Level + 1, 3) + Mathf.Pow(Level + 1, 2) + 100);

        //---------------------------------------------------------------

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Initiliaze the status based on the class type that has been selected
    /// </summary>
    /// <param name="_playerName"></param>
    /// <param name="_playerIcon"></param>
    /// <param name="_money"></param>
    /// <param name="_experience"></param>
    /// <param name="_health"></param>
    /// <param name="_healtRegen"></param>
    /// <param name="_mana"></param>
    /// <param name="_manaRegen"></param>
    /// <param name="_armor"></param>
    /// <param name="_blockChance"></param>
    /// <param name="_magicResist"></param>
    /// <param name="_magicDamage"></param>
    /// <param name="_attackDamage"></param>
    /// <param name="_attackSpeed"></param>
    /// <param name="_criticalDamage"></param>
    /// <param name="_criticalChance"></param>
    public void CreateStatus(string _playerName,
        Sprite _playerIcon,
        long _money,
        long _experience,
        float _health,
        float _healtRegen,
        float _mana,
        float _manaRegen,
        int _armor,
        float _blockChance,
        int _magicResist,
        int _magicDamage,
        int _attackDamage,
        float _attackSpeed,
        int _criticalDamage,
        int _criticalChance,
        ClassType cl_type
        )
    {
        this.playerName = _playerName;
        this.playerIcon = _playerIcon;
        this.money = _money;
        this.currentEXP = _experience;
        this.health = _health;
        this.healthRegen = _healtRegen;
        this.mana = _mana;
        this.manaRegen = _manaRegen;
        this.armor = _armor;
        this.blockChance = _blockChance;
        this.magicRezist = _magicResist;
        //this.magicDamage = _magicDamage;
        this.magicDamage = 45;
        this.attackDamage = _attackDamage;
        this.attackSpeed = _attackSpeed;
        this.criticalDamage = _criticalDamage;
        this.criticalChance = _criticalChance;
        this.classType = cl_type;
        this.cooldownReduction = 0;
        this.money = 5;
    }

    public void LevelUp(int expGiven)
    {
        if (expGiven + currentEXP > experienceNeeded)
        {
            currentEXP = (expGiven + currentEXP) - experienceNeeded;
            Level++;
            if (Level < 75)
            {
                experienceNeeded = (int)Mathf.Ceil((float)(1f / 2f) * Mathf.Pow(Level + 1, 3) + Mathf.Pow(Level + 1, 2) + 100);
                experienceBar.SetMaxExperience((int)experienceNeeded);
            }
        }
        else
        {
            currentEXP += expGiven;
        }

        experienceBar.SetExperience((int)currentEXP);
        experienceBarTxt.text = currentEXP +  " / " + experienceNeeded; 
    }

    public void UseMana(float manaRemove)
    {
        currentMana -= manaRemove;
        manaBar.SetMana((int)currentMana);
        manaBarTxt.text = currentMana + " / " + mana;
    }

    #region Damage Handler
    /// <summary>
    /// Calculate the damage to deal to enemies 
    /// </summary>
    /// <returns>A structure that will contains information about what type of damage to deal and the amount</returns>
    public DamageStruct GiveDamage()
    {
        UnityEngine.Random.InitState((int)Math.Floor(Time.deltaTime * Guid.NewGuid().GetHashCode()));

        int seed = UnityEngine.Random.Range(0, 100);
        if (classType == ClassType.MAGE)
        {
            if (criticalChance_pool[seed])
            {

                return new DamageStruct((int)(attackSpeed * (magicDamage * 0.8f) * (criticalDamage / 100f)), IncomingDamageType.MAGIC);
            }
            else
            {
                return new DamageStruct((int)(attackSpeed * (magicDamage * 0.8f)), IncomingDamageType.MAGIC);
            }
        }
        else
        {
            if (criticalChance_pool[seed])
            {
                return new DamageStruct((int)(attackSpeed * attackDamage * (criticalDamage / 100f)), IncomingDamageType.NORMAL);
            }
            else
            {
                return new DamageStruct((int)(attackSpeed * attackDamage), IncomingDamageType.NORMAL);
            }
        }
    }

    /// <summary>
    /// Used for self skills like block ability, shields, infusions, etc.
    /// </summary>
    /// <param name="skillUsed"></param>
    public void TakeDamage(BaseSkill skillUsed)
    {
        switch (skillUsed.GetTypeAbility())
        {
            case AbilityType.ABILITY:

                break;
            case AbilityType.ATTACK:
                break;

            case AbilityType.BLOCK:
                nextAttackIsBlocked = true;

                GameObject _temp = GameObject.Instantiate(skillUsed.gameObject, contentTransform_DebuffsBuffsZone);
                blocksToHaveEffect.Enqueue(_temp);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Used for when the enemy is attacking the player
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="skillUsed"></param>
    public void TakeDamage(Enemy enemy, BaseSkill skillUsed)
    {

        switch (skillUsed.GetTypeAbility())
        {
            case AbilityType.ABILITY:

                break;
            case AbilityType.ATTACK:
                TakeDamage_WithoutBlock(enemy.GiveDamage());
                break;
            case AbilityType.BLOCK:
                break;
            default:
                break;
        }
    }

    public void TakeDamage_Block(DamageStruct damage_ToReceive)
    {
        nextAttackIsBlocked = false;

        DestroyImmediate(contentTransform_DebuffsBuffsZone.transform.GetChild(0).gameObject);
        blocksToHaveEffect = new Queue<GameObject>();

        switch (damage_ToReceive.damageType)
        {
            case IncomingDamageType.TRUE_DAMAGE:

                currentHP -= damage_ToReceive.damage;

                break;
            case IncomingDamageType.MAGIC:

                if (magicRezist - damage_ToReceive.damage < 0)
                {
                    currentHP -= Math.Abs(magicRezist - damage_ToReceive.damage);
                }

                break;
            case IncomingDamageType.NORMAL:

                if (armor - damage_ToReceive.damage < 0)
                {
                    currentHP -= Math.Abs(armor - damage_ToReceive.damage);

                    //StartCoroutine(ShakeEffect_OnDamage());
                }

                break;
        }

        //healthBar.SetHealth(currentHealthPoints);
        //hpText.text = currentHealthPoints + " / " + healthPoints;
        healthBarTxt.text = currentHP + " / " + health;
        healthBar.SetHealth((int)currentHP);
    }

    public void TakeDamage_WithoutBlock(DamageStruct damage_ToReceive)
    {
        if (nextAttackIsBlocked)
        {
            TakeDamage_Block(damage_ToReceive);
            return;
        }

        switch (damage_ToReceive.damageType)
        {
            case IncomingDamageType.TRUE_DAMAGE:

                currentHP -= damage_ToReceive.damage;

                break;
            case IncomingDamageType.MAGIC:

                if (magicRezist - damage_ToReceive.damage < 0)
                {
                    currentHP -= (int)Math.Abs((0.3 * magicRezist) - damage_ToReceive.damage);
                }

                break;
            case IncomingDamageType.NORMAL:

                if (armor - damage_ToReceive.damage < 0)
                {
                    currentHP -= (int)Math.Abs((0.3 * armor) - damage_ToReceive.damage);

                    //StartCoroutine(ShakeEffect_OnDamage());
                }

                break;
        }

        //healthBar.SetHealth(currentHealthPoints);
        //hpText.text = currentHealthPoints + " / " + healthPoints;
        healthBarTxt.text = currentHP + " / " + health;
        healthBar.SetHealth((int)currentHP);
    }
    #endregion

    #region Inventory
    #endregion

    #region Update Status when equipped items

    public void AddStatusFromItems(BaseItem item)
    {
        //Additive
        foreach (BaseStats status in item.GetStats())
        {
            this.armor += (int)status.additiveArmor;
            this.health += (float)status.additiveHealth;
            this.healthRegen += (float)status.additiveHealtRegen;
            this.mana += (float)status.additiveMana;
            this.manaRegen += (float)status.additiveManaRegen;
            //this.blockChance = ;
            this.magicRezist += (int)status.additiveMagicRezist;
            this.magicDamage += (float)status.additiveMagicDamage;
            this.attackDamage += (float)status.additiveDamage;
            this.attackSpeed += (float)status.additiveAttackSpeed;
            this.criticalDamage += (int)status.additiveCriticalDamage;
            this.criticalChance += (int)status.additiveCriticalChance;
            this.cooldownReduction += (int)status.additiveCooldownReduction;
        }
    }

    public void RemoveStatusFromItems(BaseItem item)
    {
        foreach (BaseStats status in item.GetStats())
        {
            this.armor -= (int)status.additiveArmor;
            this.health -= (float)status.additiveHealth;
            this.healthRegen -= (float)status.additiveHealtRegen;
            this.mana -= (float)status.additiveMana;
            this.manaRegen -= (float)status.additiveManaRegen;
            //this.blockChance = ;
            this.magicRezist -= (int)status.additiveMagicRezist;
            this.magicDamage -= (float)status.additiveMagicDamage;
            this.attackDamage -= (float)status.additiveDamage;
            this.attackSpeed -= (float)status.additiveAttackSpeed;
            this.criticalDamage -= (int)status.additiveCriticalDamage;
            this.criticalChance -= (int)status.additiveCriticalChance;
            this.cooldownReduction -= (int)status.additiveCooldownReduction;
        }
    }

    #endregion

    public void TickPass_Player()
    {
        if (currentMana + manaRegen < mana)
        {
            currentMana += manaRegen;
        }

        if (currentHP + healthRegen < health)
        {
            currentHP += healthRegen;
        }

        healthBarTxt.text = currentHP + " / " + health;
        manaBarTxt.text = currentMana + " / " + mana;

        healthBar.SetHealth((int)currentHP);

        manaBar.SetMana((int)currentMana);

    }

    //Getter and setters

    public void AddMoney(long moneyToAdd)
    {
        money += moneyToAdd;
    }

    public void RemoveMoney(long moneyToRemove)
    {
        money -= moneyToRemove;
    }
    public void AddItemToInventory(GameObject item)
    {
        inventory.Add(item);
        item.transform.SetParent(this.transform);
    }

    public GameObject GetItemInventory(int index)
    {
        if (index >= inventory.Count)
        {
            return null;
        }

        if (inventory[index] != null)
        {
            return inventory[index];
        }

        return null;
    }

    public ClassType GetTypeClass()
    {
        return this.classType;
    }

    public int GetCurrentLevel()
    {
        return this.Level;
    }

    public Sprite GetPlayerIcon()
    {
        return this.playerIcon;
    }

    public string GetPlayerName()
    {
        return this.playerName;
    }

    public float GetHP()
    {
        return this.health;
    }

    public float GetMana()
    {
        return this.mana;
    }

    public float GetAttack()
    {
        return this.attackDamage;
    }

    public float GetMagicDamage()
    {
        return this.magicDamage;
    }

    public float GetAttackSpeed()
    {
        return this.attackSpeed;
    }

    public float GetArmor()
    {
        return this.armor;
    }

    public long GetMoney()
    {
        return this.money;
    }

    public BaseSkill[] GetAllSkills()
    {
        return skillWheel;
    }
    public void SetSkillWheel(BaseSkill[] skills)
    {
        skillWheel = skills;
    }
    public void SetSkillWheel(BaseSkill skill, int index)
    {
        if (skill == null)
        {
            skillWheel[index] = null;
            return;
        }
        skillWheel[index] = Instantiate(skill, this.transform);
    }

    public void SetForced_init_dataForMainGame()
    {
        init_dataForMainGame = true;
    }


}
