using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    

    public void CreateClone(Transform _clonePosition, Vector3 _offset) 
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetUpClone(_clonePosition, cloneDuration, canAttack, _offset);
    }
}
