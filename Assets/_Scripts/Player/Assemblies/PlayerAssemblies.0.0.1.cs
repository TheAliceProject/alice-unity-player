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
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(DebugModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(SceneGraphModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(AnimationStyleEnum)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(ClockModule)));

            // interop primitives
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Portion)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Position)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Direction)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Orientation)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(VantagePoint)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Angle)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(AxisAlignedBox)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Size)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Scale)));

            // properties
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(DecimalNumberProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(WholeNumberProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(AngleProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(PortionProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(PositionProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(DirectionProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(SizeProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(ScaleProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(OrientationProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(AxisAlignedBoxProperty)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(VantagePointProperty)));
        }
    }
}