using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System.Collections.Generic;
using System;
using Alice.Player.Modules;
using Alice.Player.Primitives;

namespace Alice.Player
{
    static public partial class PlayerAssemblies
    {
        static public readonly string VERSION_0_0_1 = DefineVersion("0.0.1", GenerateVersion_0_0_1);

        static private void GenerateVersion_0_0_1(TAssembly inAssembly)
        {
            inAssembly.Add(TInterop.GenerateType(typeof(DebugModule)));
            inAssembly.Add(TInterop.GenerateType(typeof(SceneGraphModule)));
            inAssembly.Add(TInterop.GenerateType(typeof(AnimationStyleEnum)));
            inAssembly.Add(TInterop.GenerateType(typeof(ClockModule)));

            // interop primitives
            inAssembly.Add(TInterop.GenerateType(typeof(Portion)));
            inAssembly.Add(TInterop.GenerateType(typeof(Position)));
            inAssembly.Add(TInterop.GenerateType(typeof(Direction)));
            inAssembly.Add(TInterop.GenerateType(typeof(Orientation)));
            inAssembly.Add(TInterop.GenerateType(typeof(VantagePoint)));
            inAssembly.Add(TInterop.GenerateType(typeof(Angle)));
            inAssembly.Add(TInterop.GenerateType(typeof(AxisAlignedBox)));
            inAssembly.Add(TInterop.GenerateType(typeof(Size)));
            inAssembly.Add(TInterop.GenerateType(typeof(Scale)));

            // properties
            inAssembly.Add(TInterop.GenerateType(typeof(DecimalNumberProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(WholeNumberProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(AngleProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(PortionProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(PositionProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(DirectionProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(SizeProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(ScaleProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(OrientationProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(AxisAlignedBoxProperty)));
            inAssembly.Add(TInterop.GenerateType(typeof(VantagePointProperty)));
        }
    }
}