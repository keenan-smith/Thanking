using Thanking.Attributes;

namespace Thanking.Options
{
    public static class OptimizationOptions
    {
        [Save] public static int PacketRefreshRate = 60;
        [Save] public static int InputSamples;
    }
}