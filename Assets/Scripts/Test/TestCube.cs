using UnityEngine;


[RequireComponent(typeof(ChemicalObject))]
// 모든 원소에 반응하는 테스트 오브젝트
public class TestCube : MonoBehaviour, IChemicalReactionHandler
{
    ChemicalObject chemicalObject;

    [Header("상태 표현을 위한 오브젝트들")]
    public GameObject fire;
    public GameObject ice;
    public GameObject water;
    public GameObject electric;

    [Header("불이 꺼지는 시간")]
    public float burnTime = 5f;
    private float burnEndTime;

    [Header("물이 마르는 시간")]
    public float wetTime = 10f;
    private float wetEndTime;

    [Header("얼음이 녹는 시간")]
    public float froozeTime = 3f;
    private float froozeEndTime;

    void Awake()
    {
        chemicalObject = GetComponent<ChemicalObject>();
    }

    void Update()
    {
        if(chemicalObject == null) return;

        // 각 원소의 지속 시간이 끝나면 제거 (또는 불 같은 경우 타서 없어져도 돨듯)
        if(chemicalObject.isBurning && burnEndTime <= Time.time)
        {
            chemicalObject.RemoveElement(ElementType.Fire);
        }

        if(chemicalObject.isWet && wetEndTime <= Time.time)
        {
            chemicalObject.RemoveElement(ElementType.Water);
        }

        if(chemicalObject.isFrozen && froozeEndTime <= Time.time)
        {
            chemicalObject.RemoveElement(ElementType.Ice);
        }
    }

    // 원소에 의해 상태가 변경되면 호출
    public void OnChemicalReaction(ChemicalReaction reaction)
    {
        switch(reaction)
        {
            case ChemicalReaction.BurnStarted:
                BurnStarted();
                break;
            case ChemicalReaction.BurnEnded:
                if(fire != null) fire.SetActive(false);
                break;

            case ChemicalReaction.WetStarted:
                WetStarted();
                break;
            case ChemicalReaction.WetEnded:
                if(water != null) water.SetActive(false);
                break;

            case ChemicalReaction.FreezeStarted:
                FroozeStarted();
                break;
            case ChemicalReaction.FreezeEnded:
                if(ice != null) ice.SetActive(false);
                break;

            case ChemicalReaction.ConductStarted:
                if(electric != null) electric.SetActive(true);
                break;
            case ChemicalReaction.ConductEnded:
                if(electric != null) electric.SetActive(false);
                break;

            default:
            break;
        }
    }

    void BurnStarted()
    {
        burnEndTime = Time.time + burnTime;
        
        if(fire != null) fire.SetActive(true);
    }

    void WetStarted()
    {
        wetEndTime = Time.time + wetTime;
        
        if(water != null) water.SetActive(true);
    }

    void FroozeStarted()
    {
        froozeEndTime = Time.time + froozeTime;
        
        if(ice != null) ice.SetActive(true);
    }
}
