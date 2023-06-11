using System.Collections;

namespace CV5;

/// <summary>
/// Obojsmerne zreťazený zoznam s hlavou.
/// </summary>
/// <typeparam name="T">Typ, ktorého hodnoty sa budú v zozname uchovávať.</typeparam>
public class DoublyLinkedList<T> : IEnumerable<T>
{
    /// <summary>
    /// Hlava vytvorená pre uľahčenie práce s obojsmerne zreťazeným zoznamom. 
    /// Hlava neuchováva žiadnu hodnotu prvku. Hodnota prvého prvku bude umiestnená 
    /// až za hlavou vo vlastnosti <see cref="DoublyLinkedNode{T}.Next"/>, 
    /// hodnota posledného prvku v zozname bude umiestnená pred hlavou 
    /// vo vlastnosti <see cref="DoublyLinkedNode{T}.Previous"/>. 
    /// Ak žiadny prvok v hlave nebude, hlava bude odkazovať sama na seba.
    /// </summary>
    private readonly DoublyLinkedNode<T> _head;

    /// <summary>
    /// Celkový počet prvkov.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Vytvorí objekt obojsmerne zreťazeného zoznamu.
    /// </summary>
    public DoublyLinkedList()
    {
        Count = 0;

        // Vytvoríme hlavu, ktorá bude odkazovať sama na seba
        _head = new DoublyLinkedNode<T>(default);
        _head.Previous = _head;
        _head.Next = _head;
    }

    /// <summary>
    /// Pridá prvok do zoznamu.
    /// </summary>
    /// <param name="item">Prvok, ktorý sa pridá do zoznamu.</param>
    public void Add(T item)
    {
        var node = new DoublyLinkedNode<T>(item);
        node.Next = _head;
        node.Previous = _head.Previous;

        _head.Previous.Next = node;
        _head.Previous = node;

        Count++;
    }

    /// <summary>
    /// Odstráni prvok zo zoznamu.
    /// </summary>
    /// <param name="item">Prvok, ktorý sa má odstrániť zo zoznamu.</param>
    /// <returns>True, ak sa prvok nájde a úspešne odstráni. Ak neexistuje, vráti False.</returns>
    public bool Remove(T item)
    {
        var node = Find(item);
        if (node is null)
            return false;

        node.Previous.Next = node.Next;
        node.Next.Previous = node.Previous;

        node.Next = null!;
        node.Previous = null!;

        Count--;

        return true;
    }

    /// <summary>
    /// Vymaže celý zoznam.
    /// </summary>
    public void Clear()
    {
        _head.Next = _head;
        _head.Previous = _head;

        Count = 0;
    }

    /// <summary>
    /// Zistí, či zoznam obsahuje prvok.
    /// </summary>
    /// <param name="item">Prvok, ktorého prítomnosť sa má zistiť v zozname.</param>
    /// <returns>True, ak sa prvok nachádza v zozname. Inak vráti false.</returns>
    public bool Contains(T item) => Find(item) is not null;

    /// <summary>
    /// Vyhľadá prvok v zozname a vráti vrchol, ak existuje.
    /// </summary>
    /// <param name="item">Prvok, ktorý sa má vyhľadať v zozname.</param>
    /// <returns>Odkaz na vrchol zoznamu, ktorý obsahuje hľadaný prvok. 
    /// Ak ho nenájde, vráti null hodnotu.</returns>
    private DoublyLinkedNode<T>? Find (T item)
    {
        for (var node = _head.Next; node != _head; node = node.Next)
        {
            if (Equals(node.Data, item))
            {
                return node;
            }
        }

        return null;
    }

    /// <summary>
    /// Skopíruje obsah zoznamu do poľa <paramref name="array"/> od jeho indexu <paramref name="arrayIndex"/>.
    /// </summary>
    /// <param name="array">Pole, ktorého veľkosť musí byť väčšia alebo rovná ako počet prvkov zoznamu.</param>
    /// <param name="arrayIndex">Index do poľa, od ktorého sa začnú prvky zoznamu zapisovať.</param>
    /// <exception cref="ArgumentNullException">Vyhodená, ak je <paramref name="array"/> rovné null hodnote.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Vyhodená, ak je hodnota <paramref name="arrayIndex"/> menšia než 0.</exception>
    /// <exception cref="ArgumentException">Vyhodená, ak je počet prvkov v poli <paramref name="array"/> menší než počet prvkov, ktoré sa majú zapísať zo zoznamu.</exception>
    public void CopyTo(T?[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex is less than zero.");

        if (Count > array.Length - arrayIndex)
            throw new ArgumentException(
                "The number of elements in the source is greater than the available space from index to the end of the destination array.");

        int idx = 0;
        for (var node = _head.Next; node != _head; node = node.Next)
            array[arrayIndex + idx++] = node.Data;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new Enumerator(_head);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }


    /// <summary>
    /// Indexer pre prístup k prvku v zozname podľa indexu.
    /// </summary>
    /// <param name="index">Index, na ktorom sa nachádza prvok v zozname.</param>
    /// <returns>Prvok na danom indexe.</returns>
    /// <exception cref="IndexOutOfRangeException">Vyvolaná, ak je index mimo rozsahu zoznamu.</exception>
    public T? this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();

            int idx = 0;
            for (var node = _head.Next; node != _head; node = node.Next)
                if (index == idx++)
                    return node.Data;

            throw new Exception("Something went wrong. This line should be unreachable.");
        }
    }

    private class Enumerator : IEnumerator<T>
    {
        private DoublyLinkedNode<T?> head;
        private DoublyLinkedNode<T?> current;

        public Enumerator(DoublyLinkedNode<T?> head)
        {
            this.head = head;
            this.current = head;
        }

        public T? Current => current.Data;

        object IEnumerator.Current => Current;

        void IDisposable.Dispose()
        {
        }

        public bool MoveNext()
        {
            current = current.Next;
            return (current != head);
        }

        public void Reset()
        {
            current = head;
        }
    }
}
