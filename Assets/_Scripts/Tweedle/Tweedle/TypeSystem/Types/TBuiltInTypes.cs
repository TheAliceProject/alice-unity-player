namespace Alice.Tweedle
{
    /// <summary>
    /// References to built-in types.
    /// </summary>
    static public class TBuiltInTypes
    {
        #region Assembly

        static private readonly TAssembly s_LanguageAssembly;

        static TBuiltInTypes()
        {
            var asm = s_LanguageAssembly = new TAssembly("LanguageTypes", TAssembly.NO_DEPENDENCIES, TAssemblyFlags.CannotUnload);
            s_LanguageAssembly.Add(VOID = new TVoidType(asm));
            s_LanguageAssembly.Add(NULL = new TNullType(asm));

            s_LanguageAssembly.Add(ANY = new TAnyType(asm));

            s_LanguageAssembly.Add(NUMBER = new TAbstractNumberType(asm));
            s_LanguageAssembly.Add(WHOLE_NUMBER = new TWholeNumberType(asm, NUMBER));
            s_LanguageAssembly.Add(DECIMAL_NUMBER = new TDecimalNumberType(asm, NUMBER));

            s_LanguageAssembly.Add(BOOLEAN = new TBooleanType(asm));
            s_LanguageAssembly.Add(TEXT_STRING = new TTextStringType(asm));
            s_LanguageAssembly.Add(TYPE_REF = new TTypeRefType(asm));
        }

        /// <summary>
        /// Returns a shared assembly containing language primitives.
        /// </summary>
        static public TAssembly Assembly()
        {
            return s_LanguageAssembly;
        }

        #endregion // Assembly

        #region Types

        static public readonly TVoidType VOID;
        static public readonly TNullType NULL;
        
        static public readonly TAnyType ANY;
        
        static public readonly TAbstractType NUMBER;
        static public readonly TWholeNumberType WHOLE_NUMBER;
        static public readonly TDecimalNumberType DECIMAL_NUMBER;

        static public readonly TBooleanType BOOLEAN;
        static public readonly TTextStringType TEXT_STRING;

        static public readonly TTypeRefType TYPE_REF;

        #endregion // Types
    }
}