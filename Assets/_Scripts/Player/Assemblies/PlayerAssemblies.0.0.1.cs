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
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(SystemModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(DebugModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(SceneGraphModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(ClockModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(MouseModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(KeyboardModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(MathModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(EventPolicyModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(TextStyleModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(BubblePositionModule)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(FontTypeModule)));

            // interop primitives
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Portion)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Duration)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Position)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Direction)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Orientation)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(VantagePoint)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Angle)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(AxisAlignedBox)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Size)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Scale)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Paint)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Color)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(ImageSource)));
            inAssembly.Add(TInterop.GenerateType(inAssembly, typeof(Character)));
            

        }
    }
}
