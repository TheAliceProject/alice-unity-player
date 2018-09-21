namespace Alice.Tweedle
{
    /// <summary>
    /// References to built-in types.
    /// </summary>
    static public class TStaticTypes
    {
        static public readonly TVoidType VOID = new TVoidType();
        static public readonly TNullType NULL = new TNullType();
        
        static public readonly TAnyType ANY = new TAnyType();
        
        static public readonly TAbstractType NUMBER = new TAbstractNumberType();
        static public readonly TWholeNumberType WHOLE_NUMBER = new TWholeNumberType(NUMBER);
        static public readonly TDecimalNumberType DECIMAL_NUMBER = new TDecimalNumberType(NUMBER);

        static public readonly TBooleanType BOOLEAN = new TBooleanType();
        static public readonly TTextStringType TEXT_STRING = new TTextStringType();

        static public readonly TTypeRefType TYPE_REF = new TTypeRefType();

        static public readonly TType[] ALL_PRIMITIVE_TYPES = {
            VOID, NULL, NUMBER, WHOLE_NUMBER, DECIMAL_NUMBER, BOOLEAN, TEXT_STRING, TYPE_REF
        };
    }
}