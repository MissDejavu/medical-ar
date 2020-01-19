// constants necessary for calculation, e.g. distance-sound correlation
// for real situation these constants would be derived with imaging scans or set by specialists
// 1.0 equals one meter, 0.01 equals one centimeter
public static class Constants
{
    public const bool DebugLogAll = false;

    // -----------distances & sizes-----------------
    public const float CuttingAreaSize = 0.05f;
    public const float ErrorMarginSize = 0.15f;
    public const float TotalMaxDistance = CuttingAreaSize + 2 * ErrorMarginSize; //total max distance from the tumor
    public const float MaxObstacleDistance = 0.15f; //lower than the max distance to blood vessel triggers a sonification

    // -----------sound names-----------------------
    public const string OuterAreaSound = "NoArea";
    public const string MarginsSound = "Margins";
    public const string TumorSound = "Tumor";
    public const string BackgroundSound = "Background";
    // -----------sound param+eters-----------------
    public const float MinVolume = 0.1f;
    public const float MaxVolume = 2f;

    public const float MinPitch = 0.5f;
    public const float MaxPitch = 1.5f;

    public const float MinFrequency = 2000.0f;
    public const float MaxFrequency = 10.0f;
    public const float MeanFrequency = 100.0f;
}
