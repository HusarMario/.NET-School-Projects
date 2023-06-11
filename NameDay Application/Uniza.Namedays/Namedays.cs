using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Uniza.Namedays
{
    /// <summary>
    /// Štruktúra DayMonth obsahuje informácie o kombinácii dňa a mesiaca.
    /// </summary>
    public readonly record struct DayMonth
    {
        /// <summary>
        /// Vlastnosť Day uchováva integer informáciu počtu dňa v mesiaci.
        /// </summary>
        public int Day {get; init;}
        /// <summary>
        /// Vlastnosť Month uchováva integer informáciu počtu mesiaca v roku.
        /// </summary>
        public int Month {get ; init;}

        /// <summary>
        /// Konštruktor bez parametra vytvára nový DayMonth s oboma vlastnosťami nastevenými na 1.
        /// </summary>
        /// <returns>Novú inštanciu štruktúry DayMonth.</returns>
        public DayMonth()
        {
            Day = 1;
            Month = 1;
        }

        /// <summary>
        /// Konštruktor vytvára nový DayMonth s oboma vlastnosťami nastevenými podľa parametrov.
        /// </summary>
        /// <param name="day">Číslo dňa.</param>
        /// <param name="month">Číslo mesiaca.</param>
        /// <returns>Novú inštanciu štruktúry DayMonth.</returns>
        public DayMonth(int day, int month)
        {
            Day = day;
            Month = month;
        }

        /// <summary>
        /// Funkcia konvertuje DayMonth na DateTime.
        /// </summary>
        /// <returns>Novú inštanciu DateTime.</returns>
        public DateTime ToDateTime()
        {
            return new DateTime(DateTime.Now.Year, Month, Day);
        }
    }

    public readonly record struct Nameday
    {
        /// <summary>
        /// Vlastnosť Name uchováva string informáciu mena.
        /// </summary>
        public string Name {get; init;}
        /// <summary>
        /// Vlastnosť DayMonth uchováva štruktúru DayMonth.
        /// </summary>
        public DayMonth DayMonth { get; init;}

        /// <summary>
        /// Konštruktor bez parametra vytvára nový NameDay s prázdnym menom a novú bezparametrickú inštanciu DayMonth.
        /// </summary>
        /// <returns>Novú inštanciu štruktúry Nameday.</returns>
        public Nameday()
        {
            Name = "\0";
            DayMonth = new DayMonth();
        }

        /// <summary>
        /// Konštruktor bez parametra vytvára nový NameDay s menom a štruktúrou DayMonth podľa parametrov.
        /// </summary>
        /// <param name="name">Meno oslávenca menín.</param>
        /// <param name="dayMonth">Dátum pre meniny.</param>
        /// <returns>Novú inštanciu štruktúry Nameday.</returns>
        public Nameday(string name, DayMonth dayMonth)
        {
            Name = name;
            DayMonth = dayMonth;
        }
    }

    public record class NamedayCalendar : IEnumerable<Nameday>
    {

        private readonly Dictionary<DayMonth, List<Nameday>> namedays;
        /// <summary>
        /// Vlastnosť NameCount uchováva počet mien v kalendári.
        /// </summary>
        public int NameCount { get; private set; }
        /// <summary>
        /// Vlastnosť NameCount uchováva počte dní v kalendári, v ktorom sa oslavujú meniny.
        /// </summary>
        public int DayCount { get; private set; }

        /// <summary>
        /// Konštruktor bez parametra vytvára nový NamedayCalendar so všetkými dňami roka (vrátane 29.februára) bez menín.
        /// Vlastnosti sú nastavené na 0.
        /// </summary>
        /// <returns>Novú inštanciu štruktúry NamedayCalendar.</returns>
        public NamedayCalendar()
        {
            this.namedays = new Dictionary<DayMonth, List<Nameday>>();
            NameCount = 0;
            DayCount = 0;

            DateTime dateTime = new(2024,1,1); //2024 pre 29.februar
            for (int i = 0; i < 366; i++)
            {
                namedays.Add(new DayMonth(dateTime.Day, dateTime.Month), new List<Nameday>());
                dateTime = dateTime.AddDays(1);
            }
        }

        /// <summary>
        /// Indexer vracajúci DayMonth na základe zadaného mena.
        /// </summary>
        /// <param name="index">Meno oslávenca menín.</param>
        /// <returns>Vráti príslušný DayMonth alebo NULL ak žiadny nebol nádjený.</returns>
        public DayMonth? this[string index]
        {
            get 
            {
                foreach (List<Nameday> namedays in namedays.Values)
                {
                    foreach (Nameday nameday in namedays)
                    {
                        if (nameday.Name == index)
                        {
                            return nameday.DayMonth;
                        }
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Indexer vracajúci array stringov obsahujúci mená na základe zadanej DayMonth informácie.
        /// </summary>
        /// <param name="dayMonth">Dátum vo forme DayMonth.</param>
        /// <returns>Vráti array obsahujúci všetky mená v daný dátum (alebo prázdny array).</returns>
        public string[] this[DayMonth dayMonth]
        {
            get
            {
                foreach (DayMonth key in namedays.Keys)
                {
                    if (key.Month.Equals(dayMonth.Month) && key.Day.Equals(dayMonth.Day))
                    {
                        string[] output = new string[namedays[dayMonth].Count];
                        for (int i = 0; i < output.Length; i++)
                        {
                            output[i] = namedays[dayMonth][i].Name;
                        }
                        return output;
                    }
                }
                return Array.Empty<string>();            
            }
        }

        /// <summary>
        /// Indexer vracajúci array stringov obsahujúci mená na základe zadanej DayTime informácie.
        /// </summary>
        /// <param name="date">Dátum vo forme DateTime.</param>
        /// <returns>Vráti array obsahujúci všetky mená v daný dátum (alebo prázdny array).</returns>
        public string[] this[DateTime date]
        {
            get
            {
                return this[new DayMonth(date.Day, date.Month)];
            }
        }

        /// <summary>
        /// Indexer vracajúci array stringov obsahujúci mená na základe zadanej DateOnly informácie.
        /// </summary>
        /// <param name="date">Dátum vo forme DateOnly.</param>
        /// <returns>Vráti array obsahujúci všetky mená v daný dátum (alebo prázdny array).</returns>
        public string[] this[DateOnly date]
        {
            get
            {
                return this[new DayMonth(date.Day, date.Month)];
            }
        }

        /// <summary>
        /// Indexer vracajúci array stringov obsahujúci mená na základe zadanej informácie o dni a mesiaci.
        /// </summary>
        /// <param name="day">Hľadaný deň dátumu.</param>
        /// <param name="month">Hľadaný mesiac dátumu.</param>
        /// <returns>Vráti array obsahujúci všetky mená v daný dátum (alebo prázdny array).</returns>
        public string[] this[int day, int month]
        {
            get
            {
                return this[new DayMonth(day, month)];
            }
        }

        /// <summary>
        /// Funkcia poskytujúca všetky meniny v kalendári.
        /// </summary>
        /// <returns>Vráti IEnumerable<Nameday> obsahujúci všetky meniny spĺňajúce podmienku.</returns>
        public IEnumerable<Nameday> GetNamedays()
        {
            foreach (List<Nameday> namedays in namedays.Values)
            {
                foreach (Nameday nameday in namedays)
                {
                    yield return nameday;
                }
            }
        }

        /// <summary>
        /// Funkcia poskytujúca všetky meniny v kalendári podľa mesiaca.
        /// </summary>
        /// <param name="month">Hľadaný mesiac.</param>
        /// <returns>Vráti IEnumerable<Nameday> obsahujúci všetky meniny spĺňajúce podmienku.</returns>
        public IEnumerable<Nameday> GetNamedays(int month)
        {
            foreach (List<Nameday> namedays in namedays.Values)
            {
                foreach (Nameday nameday in namedays)
                {
                    if (nameday.DayMonth.Month == month)
                    {
                        yield return nameday;
                    }
                }
            }
        }

        /// <summary>
        /// Funkcia poskytujúca všetky meniny v kalendári podľa mena.
        /// </summary>
        /// <param name="name">Hľadané meno.</param>
        /// <returns>Vráti IEnumerable<Nameday> obsahujúci všetky meniny spĺňajúce podmienku.</returns>
        public IEnumerable<Nameday> GetNamedays(string name)
        {
            Regex regex = new(name, RegexOptions.None);
            foreach (List<Nameday> namedays in namedays.Values)
            {
                foreach (Nameday nameday in namedays)
                {
                    if (regex.IsMatch(nameday.Name))
                    {
                        yield return nameday;
                    }
                }
            }
        }

        /// <summary>
        /// Metóda pridáva nové meniny do kalendára, na základe už vytvorených menín.
        /// </summary>
        /// <param name="nameday">Vytvorené meniny.</param>
        public void Add(Nameday nameday)
        {
            namedays[nameday.DayMonth].Add(nameday);
            if (namedays[nameday.DayMonth].Count == 1)
            {
                DayCount++;
            }
            NameCount++;
        }

        /// <summary>
        /// Metóda pridáva nové meniny do kalendára, na základe informácie o dni, mesiaci a pridávaných mien.
        /// </summary>
        /// <param name="day">Deň dátumu.</param>
        /// <param name="month">Mesiac dátumu.</param>
        /// <param name="names">Array obsahujúci všetky mená pre pridanie.</param>
        public void Add(int day, int month, params string[] names)
        {
            Add(new DayMonth(day, month), names);
        }

        /// <summary>
        /// Metóda pridáva nové meniny do kalendára, na základe informácie o dátume a pridávaných mien.
        /// </summary>
        /// <param name="dayMonth">Dátum vo forme DayMonth.</param>
        /// <param name="names">Array obsahujúci všetky mená pre pridanie.</param>
        public void Add(DayMonth dayMonth, params string[] names)
        {
            foreach (string name in names)
            {
                Add(new Nameday(name, dayMonth));
            }
        }

        /// <summary>
        /// Funkcia zisťujúca či sa dané meno nachádza v kalendári.
        /// </summary>
        /// <param name="name">Hľadané meno.</param>
        /// <returns>Vráti True/False informáciu ak sa údaj nájde/nenájde.</returns>
        public bool Contains(string name)
        {
            DayMonth? date = this[name];
            if (date == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Funkcia odstraňujúca dané meno v kalendári.
        /// </summary>
        /// <param name="name">Hľadané meno.</param>
        /// <returns>Vráti True/False informáciu ak sa údaj našiel a odstránil/nenašiel.</returns>
        public bool Remove(string name)
        {
            DayMonth? date = this[name];
            if (date == null)
            {
                return false;
            }

            foreach (Nameday nameday in namedays[date.Value])
            {
                if (nameday.Name.Equals(name))
                {
                    namedays[date.Value].Remove(nameday);
                }
            }
            return true;
        }

        /// <summary>
        /// Metóda vyprázdni celý kalendár.
        /// </summary>
        public void Clear()
        {
            foreach (List<Nameday> namedays in namedays.Values) 
            {
                namedays.Clear();
            }
        }

        /// <summary>
        /// Metóda načítava údaje kalendára.
        /// </summary>
        /// <param name="csvFile">Vstupný csv súbor s meninami.</param>
        public void Load(FileInfo csvFile)
        {
            StreamReader? reader = new(csvFile.FullName);
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line != null)
                {
                    string[] values = line.Split(";");
                    values[0] = values[0].Replace(" ", "");
                    string[]? dateString = values[0].Split(".");

                    for (int i = 1; i < values.Length; i++)
                    {
                        if ((values[i] == "") || (values[i] == " ") || (values[i] == "-") || (values[i] == " -"))
                        {
                            continue;
                        }
                        if (Contains(values[i]))
                        {
                            continue;
                        }
                        Add(int.Parse(dateString[0]), int.Parse(dateString[1]), values[i]);
                    }
                }
                
            }
            reader.Close();
        }

        /// <summary>
        /// Metóda ukladá údaje kalendára.
        /// </summary>
        /// <param name="csvFile">Výstupný csv súbor s meninami.</param>
        public void Save(FileInfo csvFile)
        {
            var writer = new StreamWriter(csvFile.FullName, false, Encoding.UTF8);
            foreach (DayMonth dayMonth in namedays.Keys)
            {
                StringBuilder output = new StringBuilder();
                output.Append("" + dayMonth.Day + ". " + dayMonth.Month + ".");
                for (int i = 0;i < namedays[dayMonth].Count; i++)
                {
                    output.Append(";" + namedays[dayMonth][i].Name);
                }
                writer.WriteLine(output.ToString());
            }
            writer.Close();
        }

        /// <summary>
        /// Vráti enumerátor pre postupné prechádzanie všetkých prvkov v kolekcii Nameday.
        /// </summary>
        /// <returns>Enumerátor pre postupné prechádzanie všetkých prvkov v kolekcii Nameday.</returns>
        public IEnumerator<Nameday> GetEnumerator()
        {
            foreach (List<Nameday> namedays in namedays.Values)
            {
                foreach (Nameday nameday in namedays)
                {
                    yield return nameday;
                }
            }
        }

        /// <summary>
        /// Vráti enumerátor pre postupné prechádzanie všetkých prvkov v kolekcii Nameday.
        /// </summary>
        /// <returns>Enumerátor pre postupné prechádzanie všetkých prvkov v kolekcii Nameday.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        } 
    }
}