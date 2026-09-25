
public enum ChemicalReaction
{
    None,

    // 상태 시작
    BurnStarted,       // 불이 붙음
    WetStarted,        // 젖음
    FreezeStarted,     // 얼어붙음
    ConductStarted,    // 전기가 통하기 시작함

    // 상태 종료
    BurnEnded,         // 불이 꺼짐
    WetEnded,          // 젖은 상태가 사라짐
    FreezeEnded,       // 얼음이 녹음
    ConductEnded,      // 전기가 끊김

    // 특수 반응
    WaterEvaporated,   // 물이 증발함
}
