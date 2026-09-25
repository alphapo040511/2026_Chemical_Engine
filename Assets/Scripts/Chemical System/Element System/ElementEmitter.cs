using UnityEngine;
using System.Collections.Generic;

// 주변으로 번지는 원소를 번지게 해주는 컴포넌트
public class ElementEmitter : MonoBehaviour
{
    [Header("판정 틱")]
    public float tickRate = 0.1f;
    private float lastTick;

    [Header("불이 번질 확률")]
    public float fireSpreadRate = 0.1f;

    [SerializeField]
    private ElementType element;


    [Header("판정 설정")]
    [SerializeField] private float spreadDistance = 1f;

    private ChemicalObject owner;
    private Collider ownCollider;

    private void Awake()
    {
        owner = GetComponentInParent<ChemicalObject>();
        ownCollider = GetComponent<Collider>();
    }

    public void SetElement(ElementType element)
    {
        if(this.element == element) return;
        this.element = element;
    }

    public void RemoveElement()
    {
        element = ElementType.None;
    }

    void Update()
    {
        if(lastTick + tickRate <= Time.time)
        {
            // 틱 마다 실행
            lastTick = Time.time;
            OnTick();
        }
    }

    void OnTick()
    {
        switch(element)
        {
            case ElementType.Fire:
            if(fireSpreadRate < Random.value)
                EmitElement();
                break;
            case ElementType.Electric:
                EmitElement();
                break;
            default:
            break;
        }
    }

    public void EmitElement()
    {
        if (owner == null || ownCollider == null)
            return;

        // 내 콜라이더 크기를 기준으로 전파 범위 계산
        float radius = ownCollider.bounds.extents.magnitude
                       + spreadDistance;

        Collider[] colliders =
            Physics.OverlapSphere(
                transform.position,
                radius
            );

        // 같은 ChemicalObject가 Collider 여러 개를 가질 수 있으므로
        // 한 번만 처리하기 위해 HashSet 사용
        HashSet<ChemicalObject> targets = new();

        foreach (Collider collider in colliders)
        {
            ChemicalObject target =
                collider.GetComponentInParent<ChemicalObject>();

            if (target == null)
                continue;

            // 자기 자신 제외
            if (target == owner)
                continue;

            targets.Add(target);
        }

        foreach (ChemicalObject target in targets)
        {
            target.ApplyElement(element);
        }
    }
    

    private void OnDrawGizmosSelected()
    {
        Collider collider = GetComponent<Collider>();

        if (collider == null)
            return;

        float radius = collider.bounds.extents.magnitude
                       + spreadDistance;

        Gizmos.DrawWireSphere(
            transform.position,
            radius
        );
    }
}
