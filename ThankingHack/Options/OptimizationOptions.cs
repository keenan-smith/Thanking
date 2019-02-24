﻿using Thanking.Attributes;

namespace Thanking.Options
{
    public static class OptimizationOptions
    {
        [Save] public static int PacketRefreshRate = 50;
        [Save] public static int InputSamples;
    }
}