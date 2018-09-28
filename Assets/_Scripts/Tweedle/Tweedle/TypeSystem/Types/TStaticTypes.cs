namespace Alice.Tweedle
{
    /// <summary>
    /// References to built-in types.
    /// </summary>
    static public class TStaticTypes
    {
        #region Assembly

        static private TAssembly s_LanguageAssembly;

        /// <summary>
        /// Returns a shared assembly containing language primitives.
        /// </summary>
        static public TAssembly Assembly()
        {
            if (s_LanguageAssembly == null)
                s_LanguageAssembly = GenerateAssembly();
            return s_LanguageAssembly;
        }

        static private TAssembly GenerateAssembly()
        {
            TAssembly assembly = new TAssembly("LanguageTypes", TAssembly.NO_DEPENDENCIES);
            assembly.Add(VOID);
            assembly.Add(NULL);
            assembly.Add(ANY);
            assembly.Add(NUMBER);
            assembly.Add(WHOLE_NUMBER);
            assembly.Add(DECIMAL_NUMBER);
            assembly.Add(BOOLEAN);
            assembly.Add(TEXT_STRING);
            assembly.Add(TYPE_REF);
            return assembly;
        }

        #endregion // Assembly

        #region Types

        static public readonly TVoidType VOID = new TVoidType();
        static public readonly TNullType NULL = new TNullType();
        
        static public readonly TAnyType ANY = new TAnyType();
        
        static public readonly TAbstractType NUMBER = new TAbstractNumberType();
        static public readonly TWholeNumberType WHOLE_NUMBER = new TWholeNumberType(NUMBER);
        static public readonly TDecimalNumberType DECIMAL_NUMBER = new TDecimalNumberType(NUMBER);

        static public readonly TBooleanType BOOLEAN = new TBooleanType();
        static public readonly TTextStringType TEXT_STRING = new TTextStringType();

        static public readonly TTypeRefType TYPE_REF = new TTypeRefType();

        #endregion // Types
    }
}