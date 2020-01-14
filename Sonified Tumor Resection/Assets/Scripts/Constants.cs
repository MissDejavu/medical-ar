// constants necessary for calculation, e.g. distance-sound correlation
// for real situation these constants would be derived with imaging scans or set by specialists
// 1.0 equals one meter, 0.01 equals one centimeter
public static class Constants
{
    public const bool DebugLogAll = false;

    // -----------distances & sizes-----------------
    public const float CuttingAreaSize = 0.015f;
    public const float ErrorMarginSize = 0.01f;
    public const float TotalMaxDistance = CuttingAreaSize + 2 * ErrorMarginSize;
    public const float MaxObstacleDistance = 0.005f;

    // -----------sound names-----------------------
    public const string OuterAreaSound = "NoArea";
    public const string OuterErrorMarginSound = "OuterErrorMargin";
    public const string InnerErrorMarginSound = "InnerErrorMargin";
    public const string ResectionAreaSound = "ResectionArea";
    public const string TumorSound = "Tumor";
    public const string BackgroundSound = "Background";
    // -----------sound param+eters-----------------
    public const float MinVolume = 0.25f;
    public const float MaxVolume = 1f;

    public const float MinPitch = 0.5f;
    public const float MaxPitch = 2f;

}
