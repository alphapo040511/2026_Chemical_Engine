using UnityEngine;

public class ElementTrigger : MonoBehaviour
{
    public ElementType elementType;

    void OnTriggerEnter(Collider other)
    {
        IChemicalReceiver[] receivers = other.GetComponents<IChemicalReceiver>();
    
        foreach(var receiver in receivers)
        {
            receiver.ApplyElement(elementType);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(elementType != ElementType.Electric) return;     // 전기는 떨어지면 제거

        IChemicalReceiver[] receivers = other.GetComponents<IChemicalReceiver>();
    
        foreach(var receiver in receivers)
        {
            receiver.RemoveElement(elementType);
        }
    }
}
