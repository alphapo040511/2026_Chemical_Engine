using UnityEngine;

public class ChemicalObject : MonoBehaviour, IChemicalReceiver
{
    [Header("재질 설정")]
    [SerializeField]
    private ChemicalMaterialType materialType = ChemicalMaterialType.None;

    [Header("각 반응에 대한 작동 여부")]
    [SerializeField]
    private bool canBurn;

    [SerializeField]
    private bool canGetWet;

    [SerializeField]
    private bool canFreeze;

    [SerializeField]
    private bool canConductElectricity;

    public bool isBurning {get; private set;}
    public bool isWet {get; private set;}
    public bool isFrozen {get; private set;}
    public bool isConducting {get; private set;}

    // 외부 프로퍼티
    public ChemicalMaterialType MaterialType => materialType;

    public bool CanBurn => canBurn;
    public bool CanGetWet => canGetWet;
    public bool CanFreeze => canFreeze || isWet;            // 얼지 못하는 물체라도 물이 묻으면 언다.
    public bool CanConductElectricity
        => canConductElectricity || isWet;                  // 물에 젖는다면 전기가 통할 수 있다.


    private IChemicalReactionHandler[] receivers;

    private void Awake()
    {
        receivers = GetComponents<IChemicalReactionHandler>();
    }

#region Apply Element
public void ApplyElement(ElementType element)
{
    switch(element)
    {
        case ElementType.Fire:
            OnFire();
            break;
        case ElementType.Water:
            OnWater();
            break;
        case ElementType.Ice:
            OnIce();
            break;
        case ElementType.Electric:
            OnElectric();
            break;
        
        default:
        break;
    }
}

// 불이 붙는 경우 
void OnFire()
{
    if(isWet)   
    {
        // 젖어 있다면 불이 붙지 않고, 물이 증발한다.
        isWet = false;
        NotifyReaction(ChemicalReaction.WetEnded);
        return;
    }

    if(isFrozen)
    {
        // 얼어있다면 녹는다.
        isFrozen = false;
        NotifyReaction(ChemicalReaction.FreezeEnded);
        return;
    }

    if(!canBurn) return;        // 불이 붙지 않는 재질이라면 무시한다.

    isBurning = true;           // 불이 붙는다.
    NotifyReaction(ChemicalReaction.BurnStarted);
}

// 물이 묻는 경우
void OnWater()
{
    if(isBurning)
    {
        // 불이 붙어 있다면 끈다.
        isBurning = false;
        NotifyReaction(ChemicalReaction.BurnEnded);
        return;
    }

    if(!canGetWet) return;      // 젖지 않는다면 무시한다.
    if(isFrozen) return;        // 얼어있다면 물이 묻지 않는다.

    // 물에 젖는다.
    isWet = true;
    NotifyReaction(ChemicalReaction.WetStarted);
}

// 냉기에 닿는 경우
void OnIce()
{
    if(isBurning)
    {
        // 불이 붙어 있다면 불이 꺼진다.
        NotifyReaction(ChemicalReaction.BurnEnded);
        return;
    }

    if(isWet)
    {
        // 젖어있다면 원래 얼 수 없더라도 언다.
        isWet = false;
        NotifyReaction(ChemicalReaction.WetEnded);
        
        isFrozen = true;
        NotifyReaction(ChemicalReaction.FreezeStarted);
    }

    if(!canFreeze) return;  // 얼 수 없다면 무시한다.

    isFrozen = true;
    NotifyReaction(ChemicalReaction.FreezeStarted);   
}

void OnElectric()
{
    if(isBurning)
    {
        // 불이 붙어 있다면 무언가 해주세요.
        
        return;
    }

    if(isWet)
    {
        // 젖어있다면 원래 전기가 통하지 않더라도 통한다.

        // 물을 제거하지 않는다.
        //isWet = false;
        //NotifyReaction(ChemicalReaction.WetEnded);
        
        isConducting = true;
        NotifyReaction(ChemicalReaction.ConductStarted);
    }

    if(!canConductElectricity) return;  // 전기가 통할 수 있는가?

    isConducting = true;
    NotifyReaction(ChemicalReaction.ConductStarted);
}

#endregion

#region Remove Element
public void RemoveElement(ElementType element)
{
  switch(element)
    {
        case ElementType.Fire:
            RemoveFire();
            break;
        case ElementType.Water:
            RemoveWater();
            break;
        case ElementType.Ice:
            RemoveIce();
            break;
        case ElementType.Electric:
            RemoveElectric();
            break;
        
        default:
        break;
    }
}

void RemoveFire()
{
    if(isBurning == false) return;

    isBurning = false;
    NotifyReaction(ChemicalReaction.BurnEnded);
}

void RemoveWater()
{
    if(isWet == false) return;

    isWet = false;
    NotifyReaction(ChemicalReaction.WetEnded);


    // 물 없이 전기가 통할 수 없다면 제거
    if(isConducting == true && CanConductElectricity == false)
    {
        isConducting = false;
        NotifyReaction(ChemicalReaction.ConductEnded);
    }
} 

void RemoveIce()
{
    if(isFrozen == false) return;

    isFrozen = false;
    NotifyReaction(ChemicalReaction.FreezeEnded);
}

void RemoveElectric()
{
    if(isConducting == false) return;

    isConducting = false;
    NotifyReaction(ChemicalReaction.ConductEnded);
}
#endregion

private void NotifyReaction(ChemicalReaction reaction)
{
    foreach (var receiver in receivers)
    {
        receiver.OnChemicalReaction(reaction);
    }
}

#region 재질 변경

    #if UNITY_EDITOR
    private void OnValidate()
    {
        ApplyMaterialPreset();
    }
    #endif

    private void ApplyMaterialPreset()
    {
        switch (materialType)
        {
            case ChemicalMaterialType.None:
                canBurn = false;
                canGetWet = false;
                canFreeze = false;
                canConductElectricity = false;
                break;

            case ChemicalMaterialType.Wood:
                canBurn = true;
                canGetWet = true;
                canFreeze = false;
                canConductElectricity = false;
                break;

            case ChemicalMaterialType.Metal:
                canBurn = false;
                canGetWet = true;
                canFreeze = false;
                canConductElectricity = true;
                break;

            case ChemicalMaterialType.Stone:
                canBurn = false;
                canGetWet = false;
                canFreeze = false;
                canConductElectricity = false;
                break;

            case ChemicalMaterialType.Custom:
            default:
                canBurn = false;
                canGetWet = false;
                canFreeze = false;
                canConductElectricity = false;
                break;
        }
    }
#endregion
}
