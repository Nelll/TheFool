using UnityEngine;

public static class Utils
{
    /// <summary>
    /// 각도를 기준으로 원의 둘레 위치를 구하는 메소드
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    public static Vector3 GetPositionFromAngle(float radius, float angle)
    {
        // 3D일 경우 XZ 평면을 사용하기 때문에 y 대신 z 위치로 설정

        // Mathf.Cos(angle)로 x 위치를 Mathf.Sin(angle)로 y 위치를 구합니다.
        // 단위 원이 아닌 radius 반지를을 가지기 때문에 radius를 곱한 값을 x, y 변수에 저장합니다.
        Vector3 position = Vector3.zero;

        angle = DegreeToRadian(angle);

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    /// <summary>
    /// Degree 값을 Radian 값으로 변환
    /// 1도는 "PI/180" radian
    /// angle도는 "PI/180 * angle" radian
    /// </summary>
    /// <param name="angle"></param>
    public static float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180;
    }
}
