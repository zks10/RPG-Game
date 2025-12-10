using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;

    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotKeys = true;
    private bool cloneAttackRelease;

    private int amountOfAttacks;
    private float cloneAttackCooldown;
    private float cloneAttackTimer;
    private bool playerCanDisappear = true;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();
    private HashSet<Transform> enemiesWithHotkey = new HashSet<Transform>();

    public bool playerCanExitState { get; private set; }

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
        {
            playerCanDisappear = false;
            amountOfAttacks += 6;
        }
    }
    private void Update()
    {
        RemoveInvalidTargets();
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            
            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);

        }
    }

    private void ReleaseCloneAttack()
    {
        RemoveInvalidTargets();
        if (targets.Count == 0)
            return;
        // Player should ALWAYS be allowed to release
        cloneAttackRelease = true;
        canCreateHotKeys = false;

        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }

    }

    private void CloneAttackLogic()
    {
        if (!cloneAttackRelease)
            return;

        // Clean destroyed enemies
        targets.RemoveAll(t => t == null);

        // If no enemies, just skip attack
        if (targets.Count == 0)
            return;

        // Not ready for next attack
        if (cloneAttackTimer > 0)
            return;

        if (amountOfAttacks <= 0)
            return;

        cloneAttackTimer = cloneAttackCooldown;

        int randomIndex = Random.Range(0, targets.Count);
        float xOffset = Random.Range(0, 100) > 50 ? 1.3f : -1.3f;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
        }
        else
        {
            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
        }

        amountOfAttacks--;

        if (amountOfAttacks <= 0)
            Invoke("FinishBlackholeAbility", 1);
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackRelease = false;
        enemiesWithHotkey.Clear();
        PlayerManager.instance.player.SetSkillActive(false);

    }
    private void DestroyHotKeys()
    {
        if (createdHotKey.Count <= 0)
            return;
            
        for (int i = 0; i < createdHotKey.Count; i++)
            Destroy(createdHotKey[i]);

        createdHotKey.Clear();
        enemiesWithHotkey.Clear(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null)
            return;

        if (enemiesWithHotkey.Contains(enemy.transform))
            return;

        // Mark that this enemy has a hotkey now
        enemiesWithHotkey.Add(enemy.transform);

        enemy.FreezeTime(true);
        CreateHotKey(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
        
    }
    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
            return;

        if (!canCreateHotKeys)
            return;
        
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_Hotkey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }
    public void AddEnemyToList(Transform _enemyTransform)
    {
        if (_enemyTransform == null) return;

        Enemy enemy = _enemyTransform.GetComponent<Enemy>();
        if (enemy == null || enemy.stats.isDead) return;

        targets.Add(_enemyTransform);
    }


    private void RemoveInvalidTargets()
    {
        targets.RemoveAll(t => t == null);

        if (targets.Count == 0 && cloneAttackRelease)
        {
            // All enemies are gone, finish blackhole
            FinishBlackholeAbility();
        }
    }

}
