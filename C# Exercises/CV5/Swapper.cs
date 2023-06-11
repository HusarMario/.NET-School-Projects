namespace CV5;

public class Swapper
{
    /*public static void Swap(ref int number1, ref int number2)
    {
        var temp = number1;
        number1 = number2;
        number2 = temp;
    }

    public static void Swap(ref double number1, ref double number2)
    {
        var temp = number1;
        number1 = number2;
        number2 = temp;
    }

    public static void Swap(ref string str1, ref string str2)
    {
        var temp = str1;
        str1 = str2;
        str2 = temp;
    }*/

    // TODO Úloha 1.2: Pridajte generickú metódu na prehodenie hodnôt a použite ju v programe.

    public static void Swap<T>(ref T variable1, ref T variable2)
    => (variable2, variable1) = (variable1, variable2);
}
