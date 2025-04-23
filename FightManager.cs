using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class FightManager : MonoBehaviour
{
    private static Player player;
    private static Vector2 player_position;
    private static List<Enemy> targets;
    private static bool isLoadedForFirstTime = true;

    public static IEnumerator Fight(BaseSkill skillUsed)
    {
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(0.2f);

        OnStartFight();

        //-----------------------------------------------------------PLAYER TURN----------------------------------------------------------------

        switch (skillUsed.GetTargetTAG())
        {
            case PlayerUtils.SKILL_TARGET_TAG.One:

                foreach (Enemy enemy in targets)
                {
                    if (enemy.isSelectedAsTarget)
                    {
                        GameObject skillCopy = Instantiate(skillUsed.gameObject, skillUsed.transform.parent);
                        skillCopy.transform.SetAsLastSibling();
                        LTDescr end = LeanTween.move(skillCopy, enemy.transform.GetChild(0).transform, 0.8f).setEaseOutCubic();
                        yield return new WaitForSeconds(0.2f);
                        end.setOnComplete(() =>
                            {
                                DestroyImmediate(skillCopy);
                                enemy.TakeDamage(player, skillUsed);
                            });
                        break;
                        
                    }
                }

                break;
            case PlayerUtils.SKILL_TARGET_TAG.Two:

                CheckNumberOfTargets(2);

                foreach (Enemy enemy in targets)
                {
                    if (enemy.isSelectedAsTarget)
                    {
                        GameObject skillCopy = Instantiate(skillUsed.gameObject, skillUsed.transform.parent);
                        skillCopy.transform.SetAsLastSibling();
                        LTDescr end = LeanTween.move(skillCopy, enemy.transform.GetChild(0).transform, 0.8f).setEaseOutCubic();
                        yield return new WaitForSeconds(0.2f);
                        end.setOnComplete(() =>
                        {
                            DestroyImmediate(skillCopy);
                            enemy.TakeDamage(player, skillUsed);
                        });

                    }
                }


                break;
            case PlayerUtils.SKILL_TARGET_TAG.Three:

                CheckNumberOfTargets(3);

                foreach (Enemy enemy in targets)
                {
                    if (enemy.isSelectedAsTarget)
                    {
                        GameObject skillCopy = Instantiate(skillUsed.gameObject, skillUsed.transform.parent);
                        skillCopy.transform.SetAsLastSibling();
                        LTDescr end = LeanTween.move(skillCopy, enemy.transform.GetChild(0).transform, 0.8f).setEaseOutCubic();
                        yield return new WaitForSeconds(0.2f);
                        end.setOnComplete(() =>
                        {
                            DestroyImmediate(skillCopy);
                            enemy.TakeDamage(player, skillUsed);
                        });
                    }
                }

                break;
            case PlayerUtils.SKILL_TARGET_TAG.ALL:

                foreach (Enemy enemy in targets)
                {
                    GameObject skillCopy = Instantiate(skillUsed.gameObject, skillUsed.transform.parent);
                    skillCopy.transform.SetAsLastSibling();
                    LTDescr end = LeanTween.move(skillCopy, enemy.transform.GetChild(0).transform, 0.8f).setEaseOutCubic();
                    yield return new WaitForSeconds(0.2f);
                    end.setOnComplete(() =>
                    {
                        DestroyImmediate(skillCopy);
                        enemy.TakeDamage(player, skillUsed);
                    });
                }

                break;
            case PlayerUtils.SKILL_TARGET_TAG.Self:
                GameObject skillCopy_self = Instantiate(skillUsed.gameObject, skillUsed.transform.parent);
                skillCopy_self.transform.SetAsLastSibling();
                LTDescr end_self = LeanTween.move(skillCopy_self, player_position, 0.8f).setEaseOutCubic();
                yield return new WaitForSeconds(0.2f);
                end_self.setOnComplete(() =>
                {
                    player.TakeDamage(skillUsed);
                    DestroyImmediate(skillCopy_self);
                });
                break;
            default:
                break;
        }

        TickPass_FightManager();

        //-------------------------------------------------------- ^ --PLAYER TURN-- ^ -------------------------------------------------------------

        yield return new WaitForSeconds(1f);

        //-------------------------------------------------------------ENEMIES TURN-----------------------------------------------------------------

        foreach (Enemy enemy_turn in targets)
        {

            BaseSkill enemySkill = enemy_turn.GetFirstSkill();
            if (enemySkill == null || enemySkill.GetTypeAbility() == PlayerUtils.AbilityType.BLOCK)
            {
                enemy_turn.DeleteAbilitiesAfterUse_OnAttack();
                continue;
            }
            GameObject skillCopy = Instantiate(enemySkill.gameObject, enemySkill.transform.parent);
            skillCopy.transform.SetAsLastSibling();
            LTDescr end = LeanTween.move(skillCopy, player_position, 0.8f).setEaseOutCubic();
            yield return new WaitForSeconds(0.2f);
            end.setOnComplete(() =>
            {
                DestroyImmediate(skillCopy);
                player.TakeDamage(enemy_turn,enemySkill);
            });
            enemy_turn.DeleteAbilitiesAfterUse_OnAttack();

            TickPass_FightManager();

        }

        //Clean up
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < targets.Count; i++)
        {
            if (i != 0)
            {
                targets[i].isSelectedAsTarget = false;
            }
            else
            {
                targets[i].isSelectedAsTarget = true;
            }
            targets[i].Redo_AbilitiesAfterDeletion();
        }

        Cursor.lockState = CursorLockMode.None;

        //-------------------------------------------------------- ^ --ENEMIES TURN-- ^ -------------------------------------------------------------
    }

    private static void CheckNumberOfTargets(int spellTargets)
    {
        //More skill targets than actual targets
        if (spellTargets > targets.Count)
        {
            foreach (Enemy enemy in targets)
            {
                enemy.isSelectedAsTarget = true;
            }
            return;
        }
        //---------------------------------------

        //Number of selected enemies
        int selectedNumberOfEnemies = 0;
        foreach (Enemy enemy_Target in targets)
        {
            if (enemy_Target.isSelectedAsTarget)
            {
                selectedNumberOfEnemies++;
            }
        }
        //----------------------------------------

        //  Logic for when the number of enemies are
        // less than the actually targets required by
        // spell
        while (selectedNumberOfEnemies < spellTargets)
        {
            UnityEngine.Random.InitState((int)Math.Floor(Time.deltaTime * Guid.NewGuid().GetHashCode()));
            int selection = UnityEngine.Random.Range(0, targets.Count);

            if (!targets[selection].isSelectedAsTarget)
            {
                selectedNumberOfEnemies++;
                targets[selection].isSelectedAsTarget = true;
            }
        }
        //----------------------------------------
    }

    private static void OnStartFight()
    {
        if(isLoadedForFirstTime)
        {
            On_Fight_Preparation();
            player_position = GameObject.FindGameObjectWithTag("Player").transform.position;
            isLoadedForFirstTime = false;
        }
    }

    public static void On_Fight_Preparation()
    {
        //Player info
        player = FindAnyObjectByType<Player>();

        //Targets info
        targets = new List<Enemy>();
        if (GameObject.Find("EnemyPanel1") != null)
        {
            targets.Add(GameObject.Find("EnemyPanel1").GetComponent<Enemy>());
        }

        if (GameObject.Find("EnemyPanel2") != null)
        {
            targets.Add(GameObject.Find("EnemyPanel2").GetComponent<Enemy>());
        }

        if (GameObject.Find("EnemyPanel3") != null)
        {
            targets.Add(GameObject.Find("EnemyPanel3").GetComponent<Enemy>());
        }

        targets.First().isSelectedAsTarget = true;

    }

    public static void Call_WhenEnemieDie(GUID guid_Enemy)
    {
        foreach (Enemy e in targets)
        {
            if(e.GetGUID() == guid_Enemy)
            {
                targets.Remove(e);
                Destroy(e.gameObject);
                return;
            }
        }
    }

    public static void TickPass_FightManager()
    {
        player.TickPass_Player();
        foreach (Enemy e in targets)
        {
            e.TickPass_Enemy();
        }
    }

    private void Awake()
    {

    }
    private void Update()
    {

    }
}
