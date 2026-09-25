using UnityEngine;

public interface IChemicalReceiver
{
    public void ApplyElement(ElementType element);      // 원소를 추가

    public void RemoveElement(ElementType element);     // 원소를 제거
}
