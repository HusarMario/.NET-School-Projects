namespace CV5;

class Program
{
    static void Main(string[] args)
    {
        SwapExample();
        DoublyLinkedListExample();
    }

    private static void SwapExample()
    {
        var a = 10;
        var b = 20;
        Swapper.Swap(ref a, ref b);
        // V premenných by mali byť tieto hodnoty:
        // a = 20, b = 10
        Console.WriteLine($"{a} {b}");
 
        var d1 = Math.PI;
        var d2 = Math.E;
        Swapper.Swap(ref d1, ref d2);
        // V premenných by mali byť tieto hodnoty:
        // d1 = Math.E, d2 = Math.PI
        Console.WriteLine($"{d1} {d2}");

        var hello = "Ahoj";
        var world = "svet";
        Swapper.Swap(ref hello,ref world);
        // V premenných by mali byť tieto hodnoty:
        // hello = "svet", world = "Ahoj"
        Console.WriteLine($"{hello} {world}");

        // Úloha 1.1: Opravte chyby, aby prehodenie hodnôt fungovalo a bol nasledujúci požadovaný výstup: 
        // 20 10
        // 2,718281828459045 3,141592653589793
        // svet Ahoj
    }

    private static void DoublyLinkedListExample()
    {
        /*var list = new DoublyLinkedList<string>();
        list.Add("biela");
        list.Add("modra");
        list.Add("cervena");
        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(list[i]);
        }*/

        var list = new DoublyLinkedList<string>();
        list.Add("biela");
        list.Add("modra");
        list.Add("cervena");

        var numbers = new DoublyLinkedList<int>();
        for (int i = 1; i < 6; i++)
           numbers.Add(i);

        // Prechod cez indexer:
        //for (int i = 0; i < numbers.Count; i++)
        //    Console.WriteLine(numbers[i]);

        // Prechod cez enumerátor:
        var enumerator = numbers.GetEnumerator();
        //while (enumerator.MoveNext())
        //{
        //    var number = enumerator.Current;
        //   Console.WriteLine(number);
        //}

        // Prechod cez foreach - s využitím enumerátora:
        foreach (var number in numbers)
            Console.WriteLine(number);
    }
}
