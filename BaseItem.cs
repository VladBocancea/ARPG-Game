using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PlayerUtils;

public enum RARITY_TAG
{
    Common = 0,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    God
}

public class BaseItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private string nameOf;
    private string rarityText;
    private string levelRequired;
    private int levelOfItem;
    public Image icon;
    private Sprite borderIcon;
    private ARMOR_TAG armor_tag;
    public RARITY_TAG rarity_tag;
    private List<BaseStats> stats;
    private int[] pool = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public Transform parentAfterDrag;

    private static LTDescr delay;

    public void GenerateStatus(int level, Player player)
    {
        levelOfItem = level;

        stats = new List<BaseStats>();

        icon = gameObject.AddComponent<Image>();

        rarity_tag = GenerateRarity_Based_On_Level(level);

        Generate_Item_Tag(player);

        BorderSpritesLoader spriteLoader = GameObject.FindAnyObjectByType<BorderSpritesLoader>();

        switch (rarity_tag)
        {
            case RARITY_TAG.Common:
                //1 stat
                GenerateStatus_Armortag(level);
                //borderIcon.sprite = Resources.Load<Sprite>("UI/BorderItems/CommonBorder");
                borderIcon = spriteLoader.commonBorder;
                break;
            case RARITY_TAG.Uncommon:
                //1 stats
                GenerateStatus_Armortag(level);
                //borderIcon.sprite = Resources.Load<Sprite>("UI/BorderItems/UncomonBorder");
                borderIcon = spriteLoader.uncommonBorder;
                break;
            case RARITY_TAG.Rare:
                //2 stats
                for (int i = 0; i < 2; i++)
                {
                    GenerateStatus_Armortag(level);
                }
                //borderIcon.sprite = Resources.Load<Sprite>("UI/BorderItems/RareBorder");
                borderIcon = spriteLoader.rareBorder;
                break;
            case RARITY_TAG.Epic:
                //2 stats
                for (int i = 0; i < 2; i++)
                {
                    GenerateStatus_Armortag(level);
                }
                //borderIcon.sprite = Resources.Load<Sprite>("UI/BorderItems/EpicBorder");
                borderIcon = spriteLoader.epicBorder;
                break;
            case RARITY_TAG.Legendary:
                //3 stats
                for (int i = 0; i < 3; i++)
                {
                    GenerateStatus_Armortag(level);
                }
                //borderIcon.sprite = Resources.Load<Sprite>("UI/BorderItems/LegendaryBorder");
                borderIcon = spriteLoader.legendaryBorder;
                break;
            case RARITY_TAG.God:
                //3 stats + 1 passive
                for (int i = 0; i < 3; i++)
                {
                    GenerateStatus_Armortag(level);
                }
                //borderIcon.sprite = Resources.Load<Sprite>("UI/BorderItems/GodBorder");
                borderIcon = spriteLoader.godBorder;
                break;
            default:
                break;
        }
    }

    private void GenerateStatus_Armortag(int level)
    {
        BaseStats stat = new BaseStats();
        UnityEngine.Random.InitState((int)Math.Floor(Time.deltaTime * Guid.NewGuid().GetHashCode()));
        int seed = UnityEngine.Random.Range(0, 10);

        if (pool[seed] == 0) //aditive status
        {
            switch (armor_tag)
            {
                case ARMOR_TAG.Head:
                    Generate_Helmet(ref stat, level);
                    break;
                case ARMOR_TAG.Chestplate:
                    Generate_Chestplate(ref stat, level);
                    break;
                case ARMOR_TAG.Leggings:
                    Generate_Leggings(ref stat, level);
                    break;
                case ARMOR_TAG.Boots:
                    Generate_Boots(ref stat, level);
                    break;
                case ARMOR_TAG.Jewel:
                    Generate_Jewel(ref stat, level);
                    break;
                case ARMOR_TAG.Weapon_OneHand:
                    Generate_WeaponOneHand(ref stat, level);
                    break;
                case ARMOR_TAG.Weapon_TwoHand:
                    Generate_WeaponTwoHand(ref stat, level);
                    break;
                case ARMOR_TAG.Weapon_OffHand:
                    Generate_WeaponOffHand(ref stat, level);
                    break;
                case ARMOR_TAG.Bracers:
                    Generate_Bracers(ref stat, level);
                    break;
                default:
                    break;
            }
            stats.Add(stat);
        }
        else //procentual staus
        {
            switch (armor_tag)
            {
                case ARMOR_TAG.Head:

                    break;
                case ARMOR_TAG.Chestplate:

                    break;
                case ARMOR_TAG.Leggings:

                    break;
                case ARMOR_TAG.Boots:

                    break;
                case ARMOR_TAG.Jewel:

                    break;
                case ARMOR_TAG.Weapon_OneHand:

                    break;
                case ARMOR_TAG.Weapon_TwoHand:

                    break;
                case ARMOR_TAG.Weapon_OffHand:

                    break;
                case ARMOR_TAG.Bracers:

                    break;
                default:
                    break;
            }
        }
    }

    //Helmet
    public void Generate_Helmet(ref BaseStats baseStat, int level)
    {
        int seed = UnityEngine.Random.Range(0, 4);
        switch (seed)
        {
            case 0: //health
                baseStat.additiveHealth = GenerateItemStat(level, 2);
                break;
            case 1: //health regen
                baseStat.additiveHealtRegen = GenerateItemStat(level, 0.2);
                break;
            case 2: //armor
                baseStat.additiveArmor = GenerateItemStat(level, 1);
                break;
            case 3: //block chance
                baseStat.additiveArmor = GenerateItemStat(level, 0.8);
                break;
            default:
                break;
        }
    }
    //Chestplate
    public void Generate_Chestplate(ref BaseStats baseStat, int level)
    {
        int seed = UnityEngine.Random.Range(0, 4);
        switch (seed)
        {
            case 0: //health
                baseStat.additiveHealth = GenerateItemStat(level, 2);
                break;
            case 1: //health regen
                baseStat.additiveHealtRegen = GenerateItemStat(level, 0.2);
                break;
            case 2: //armor
                baseStat.additiveArmor = GenerateItemStat(level, 1);
                break;
            case 3: //block chance
                baseStat.additiveArmor = GenerateItemStat(level, 0.8);
                break;
            default:
                break;
        }
    }
    //Leggings
    public void Generate_Leggings(ref BaseStats baseStat, int level)
    {
        int seed = UnityEngine.Random.Range(0, 4);
        switch (seed)
        {
            case 0: //health
                baseStat.additiveHealth = GenerateItemStat(level, 2);
                break;
            case 1: //health regen
                baseStat.additiveHealtRegen = GenerateItemStat(level, 0.2);
                break;
            case 2: //armor
                baseStat.additiveArmor = GenerateItemStat(level, 1);
                break;
            case 3: //block chance
                baseStat.additiveArmor = GenerateItemStat(level, 0.8);
                break;
            default:
                break;
        }
    }
    //Boots
    public void Generate_Boots(ref BaseStats baseStat, int level)
    {
        int seed = UnityEngine.Random.Range(0, 4);
        switch (seed)
        {
            case 0: //health
                baseStat.additiveHealth = GenerateItemStat(level, 2);
                break;
            case 1: //health regen
                baseStat.additiveHealtRegen = GenerateItemStat(level, 0.2);
                break;
            case 2: //armor
                baseStat.additiveArmor = GenerateItemStat(level, 1);
                break;
            case 3: //block chance
                baseStat.additiveArmor = GenerateItemStat(level, 0.8);
                break;
            default:
                break;
        }
    }

    //Bracers
    public void Generate_Jewel(ref BaseStats baseStat, int level)
    {
        int seed = UnityEngine.Random.Range(0, 3);
        switch (seed)
        {
            case 0: //Mana
                baseStat.additiveMana = GenerateItemStat(level, 1);
                break;
            case 1: //Mana regen
                baseStat.additiveManaRegen = GenerateItemStat(level, 0.2);
                break;
            case 2: //Cooldown reduction
                baseStat.additiveCooldownReduction = GenerateItemStat(level, 0.5) / 2;
                break;
        }
    }

    //Jewel
    public void Generate_Bracers(ref BaseStats baseStat, int level)
    {
        int seed = UnityEngine.Random.Range(0, 5);
        switch (seed)
        {
            case 0: //Attack damage
                baseStat.additiveDamage = GenerateItemStat(level, 2);
                break;
            case 1: //Attack speed
                baseStat.additiveAttackSpeed = GenerateItemStat(level, 0.01);
                break;
            case 2: //Magic damage
                baseStat.additiveMagicDamage = GenerateItemStat(level, 2);
                break;
            case 3: //Cooldown reduction
                baseStat.additiveCooldownReduction = GenerateItemStat(level, 0.5) / 2;
                break;
        }
    }

    //Weapon_OneHand
    public void Generate_WeaponOneHand(ref BaseStats baseStat, int level)
    {
        int seed = UnityEngine.Random.Range(0, 6);
        switch (seed)
        {
            case 0: //Attack damage
                baseStat.additiveDamage = GenerateItemStat(level, 2);
                break;
            case 1: //Attack speed
                baseStat.additiveAttackSpeed = GenerateItemStat(level, 0.01);
                break;
            case 2: //Magic damage
                baseStat.additiveMagicDamage = GenerateItemStat(level, 2);
                break;
            case 3: //Critical damage
                baseStat.additiveCriticalDamage = GenerateItemStat(level, 0.5);
                break;
            case 4: //Critical chance
                baseStat.additiveCriticalChance = GenerateItemStat(level, 0.8);
                break;
        }
    }

    //Weapon_TwoHand
    public void Generate_WeaponTwoHand(ref BaseStats baseStat, int level)
    {
        int seed = UnityEngine.Random.Range(0, 6);
        switch (seed)
        {
            case 0: //Attack damage
                baseStat.additiveDamage = GenerateItemStat(level, 4);
                break;
            case 1: //Attack speed
                baseStat.additiveAttackSpeed = GenerateItemStat(level, 0.01);
                break;
            case 2: //Magic damage
                baseStat.additiveMagicDamage = GenerateItemStat(level, 4);
                break;
            case 3: //Critical damage
                baseStat.additiveCriticalDamage = GenerateItemStat(level, 0.5);
                break;
            case 4: //Critical chance
                baseStat.additiveCriticalChance = GenerateItemStat(level, 0.8);
                break;
        }
    }

    //Weapon_OffHand
    public void Generate_WeaponOffHand(ref BaseStats baseStat, int level)
    {
        int seed = UnityEngine.Random.Range(0, 6);
        switch (seed)
        {
            case 0: //Attack damage
                baseStat.additiveDamage = GenerateItemStat(level, 2);
                break;
            case 1: //Attack speed
                baseStat.additiveAttackSpeed = GenerateItemStat(level, 0.01);
                break;
            case 2: //Magic damage
                baseStat.additiveMagicDamage = GenerateItemStat(level, 2);
                break;
            case 3: //Critical damage
                baseStat.additiveCriticalDamage = GenerateItemStat(level, 0.5);
                break;
            case 4: //Critical chance
                baseStat.additiveCriticalChance = GenerateItemStat(level, 0.8);
                break;
        }
    }

    //Base stat table
    //Health --- 2 | Health Regen --- 0.2 | Armor --- 2
    private double GenerateItemStat(int level, double baseStat = 1, double variation = 0.2)
    {
        double[] rarityScaling = { 0.1, 0.15, 0.2, 0.25, 0.35, 0.5 }; // Adjusted for balance

        // Quadratic scaling: Level^2 * Scaling Factor
        double baseValue = baseStat + (level * rarityScaling[(int)rarity_tag]);

        System.Random r = new System.Random();
        r.NextDouble();

        // Apply random variation (between -30% and +30%)
        double randomFactor = 1 + (r.NextDouble() * 2 - 1) * variation;

        // Final stat value
        return Math.Round(baseValue * randomFactor, 2);
    }

    private RARITY_TAG GenerateRarity_Based_On_Level(int level)
    {
        int seed = UnityEngine.Random.Range(0, 10);
        if (level <= 20)
        {
            int[] poolLvl20 = new int[10] { 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 };

            if (poolLvl20[seed] == 0)
            {
                return RARITY_TAG.Common;
            }

            if (poolLvl20[seed] == 1)
            {
                return RARITY_TAG.Uncommon;
            }

        }
        else if (20 < level && level <= 35)
        {
            int[] poolLvl20i35 = new int[10] { 0, 0, 0, 1, 0, 1, 0, 3, 1, 2 };

            if (poolLvl20i35[seed] == 0)
            {
                return RARITY_TAG.Common;
            }

            if (poolLvl20i35[seed] == 1)
            {
                return RARITY_TAG.Uncommon;
            }

            if (poolLvl20i35[seed] == 2)
            {
                return RARITY_TAG.Rare;
            }

            if (poolLvl20i35[seed] == 3)
            {
                return RARITY_TAG.Epic;
            }

        }
        else if (35 < level && level <= 55)
        {
            int[] poolLvl35i55 = new int[10] { 0, 1, 0, 1, 2, 3, 0, 3, 1, 2 };

            if (poolLvl35i55[seed] == 0)
            {
                return RARITY_TAG.Common;
            }

            if (poolLvl35i55[seed] == 1)
            {
                return RARITY_TAG.Uncommon;
            }

            if (poolLvl35i55[seed] == 2)
            {
                return RARITY_TAG.Rare;
            }

            if (poolLvl35i55[seed] == 3)
            {
                return RARITY_TAG.Epic;
            }

        }
        else if (55 < level && level <= 65)
        {
            int[] poolLvl55i65 = new int[10] { 1, 1, 2, 1, 2, 3, 1, 3, 4, 2 };

            if (poolLvl55i65[seed] == 1)
            {
                return RARITY_TAG.Uncommon;
            }

            if (poolLvl55i65[seed] == 2)
            {
                return RARITY_TAG.Rare;
            }

            if (poolLvl55i65[seed] == 3)
            {
                return RARITY_TAG.Epic;
            }

            if (poolLvl55i65[seed] == 4)
            {
                return RARITY_TAG.Legendary;
            }

        }
        else if (65 < level && level < 75)
        {
            int[] poolLvl65i75 = new int[10] { 2, 4, 2, 4, 2, 3, 5, 3, 4, 3 };

            if (poolLvl65i75[seed] == 2)
            {
                return RARITY_TAG.Rare;
            }

            if (poolLvl65i75[seed] == 3)
            {
                return RARITY_TAG.Epic;
            }

            if (poolLvl65i75[seed] == 4)
            {
                return RARITY_TAG.Legendary;
            }

            if (poolLvl65i75[seed] == 5)
            {
                return RARITY_TAG.God;
            }
        }

        return RARITY_TAG.Common;

    }

    /// <summary>
    /// Generate the icon and the item tag (helmet,weapon, helmet etc)
    /// </summary>
    private void Generate_Item_Tag(Player player)
    {
        UnityEngine.Random.InitState((int)Math.Floor(Time.deltaTime * Guid.NewGuid().GetHashCode()));
        int seed = UnityEngine.Random.Range(0, 9);
        int index_util = UnityEngine.Random.Range(0, 2);
        index_util++;
        switch (seed)
        {
            case 0:
                this.armor_tag = ARMOR_TAG.Head;

                switch (player.GetTypeClass())
                {
                    case ClassType.MAGE:

                        icon.sprite = Resources.Load<Sprite>("UI/Item/Mage/MageHelmet" + index_util);

                        break;
                    case ClassType.ARCHER:
                        break;
                    case ClassType.BERSEKER:
                        break;
                    default:
                        break;
                }

                break;
            case 1:
                this.armor_tag = ARMOR_TAG.Chestplate;

                switch (player.GetTypeClass())
                {
                    case ClassType.MAGE:

                        icon.sprite = Resources.Load<Sprite>("UI/Item/Mage/MageChestplate" + index_util);

                        break;
                    case ClassType.ARCHER:
                        break;
                    case ClassType.BERSEKER:
                        break;
                    default:
                        break;
                }

                break;
            case 2:
                this.armor_tag = ARMOR_TAG.Bracers;

                switch (player.GetTypeClass())
                {
                    case ClassType.MAGE:

                        icon.sprite = Resources.Load<Sprite>("UI/item/Mage/MageBracers" + index_util);

                        break;
                    case ClassType.ARCHER:
                        break;
                    case ClassType.BERSEKER:
                        break;
                    default:
                        break;
                }

                break;
            case 3:
                this.armor_tag = ARMOR_TAG.Leggings;

                switch (player.GetTypeClass())
                {
                    case ClassType.MAGE:

                        icon.sprite = Resources.Load<Sprite>("UI/Item/Mage/MageTrousers" + index_util);

                        break;
                    case ClassType.ARCHER:
                        break;
                    case ClassType.BERSEKER:
                        break;
                    default:
                        break;
                }

                break;
            case 4:
                this.armor_tag = ARMOR_TAG.Boots;

                switch (player.GetTypeClass())
                {
                    case ClassType.MAGE:

                        icon.sprite = Resources.Load<Sprite>("UI/Item/Mage/MageSaboti" + index_util);

                        break;
                    case ClassType.ARCHER:
                        break;
                    case ClassType.BERSEKER:
                        break;
                    default:
                        break;
                }

                break;
            case 5:
                this.armor_tag = ARMOR_TAG.Jewel;

                index_util = UnityEngine.Random.Range(0, 6);
                index_util++;
                icon.sprite = Resources.Load<Sprite>("UI/Item/Jewel/Jewel" + index_util);

                break;
            case 6:
                this.armor_tag = ARMOR_TAG.Weapon_OneHand;

                switch (player.GetTypeClass())
                {
                    case ClassType.MAGE:

                        icon.sprite = Resources.Load<Sprite>("UI/Item/Mage/Weapons/MageOneHand" + index_util);

                        break;
                    case ClassType.ARCHER:
                        break;
                    case ClassType.BERSEKER:
                        break;
                    default:
                        break;
                }

                break;
            case 7:
                this.armor_tag = ARMOR_TAG.Weapon_TwoHand;

                switch (player.GetTypeClass())
                {
                    case ClassType.MAGE:

                        icon.sprite = Resources.Load<Sprite>("UI/Item/Mage/Weapons/MageTwoHand" + index_util);

                        break;
                    case ClassType.ARCHER:
                        break;
                    case ClassType.BERSEKER:
                        break;
                    default:
                        break;
                }

                break;
            case 8:
                this.armor_tag = ARMOR_TAG.Weapon_OffHand;

                switch (player.GetTypeClass())
                {
                    case ClassType.MAGE:

                        icon.sprite = Resources.Load<Sprite>("UI/Item/Mage/Weapons/MageOffHand" + index_util);

                        break;
                    case ClassType.ARCHER:
                        break;
                    case ClassType.BERSEKER:
                        break;
                    default:
                        break;
                }

                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        icon.raycastTarget = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        icon.raycastTarget = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(0.8f, () =>
        {
            ToolTipItemSystem.Show(this);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        ToolTipItemSystem.Hide();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

        }
    }

    #region Getters

    public List<BaseStats> GetStats()
    {
        return this.stats;
    }

    public ARMOR_TAG GetArmorTag()
    {
        return this.armor_tag;
    }

    public string GetArmorTag_ToString()
    {
        switch (armor_tag)
        {
            case ARMOR_TAG.Head:
                return "Helmet";
            case ARMOR_TAG.Chestplate:
                return "Chestplate";
            case ARMOR_TAG.Bracers:
                return "Bracers";
            case ARMOR_TAG.Leggings:
                return "Leggings";
            case ARMOR_TAG.Boots:
                return "Boots";
            case ARMOR_TAG.Jewel:
                return "Jewel";
            case ARMOR_TAG.Weapon_OneHand:
                return "Weapon One Hand";
            case ARMOR_TAG.Weapon_TwoHand:
                return "Weapon Two Hand";
            case ARMOR_TAG.Weapon_OffHand:
                return "Weapon Off Hand";
            default:
                break;
        }
        return "";
    }

    public int GetLevel()
    {
        return this.levelOfItem;
    }

    public string GetRarityTag_ToString()
    {
        switch (rarity_tag)
        {
            case RARITY_TAG.Common:
                return "<color=#7B7B7B> Common </color>";
            case RARITY_TAG.Uncommon:
                return "<color=#217B1F> Uncommon </color>";
            case RARITY_TAG.Rare:
                return "<color=#3E4BFF> Rare </color>";
            case RARITY_TAG.Epic:
                return "<color=#79236D> Epic </color>";
            case RARITY_TAG.Legendary:
                return "<color=#D7B732> Legendary </color>";
            case RARITY_TAG.God:
                return "<color=#B50F0A> God </color>";
            default:
                break;
        }

        return "";
    }

    internal Sprite GetSpriteIcon()
    {
        return this.icon.sprite;
    }

    internal Sprite GetSpriteBorder()
    {
        return this.borderIcon;
    }
    #endregion
}
