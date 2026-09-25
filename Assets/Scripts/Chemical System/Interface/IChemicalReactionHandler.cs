public interface IChemicalReactionHandler
{
    // 원소에 의해 상태가 변경되면 호출
    public void OnChemicalReaction(ChemicalReaction reaction);
}