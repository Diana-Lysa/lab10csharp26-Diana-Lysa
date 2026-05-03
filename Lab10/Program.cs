using System;
using System.Collections.Generic;
using System.Threading;

namespace CityLifeLab10
{
    public class CityEventArgs : EventArgs
    {
        public string Message { get; }
        public DateTime Time { get; }

        public CityEventArgs(string message)
        {
            Message = message;
            Time = DateTime.Now;
        }
    }

    public class City
    {
        public string CityName { get; }

        public event EventHandler<CityEventArgs> EmergencyOccurred;
        public event EventHandler<CityEventArgs> HolidayStarted;

        public City(string name)
        {
            CityName = name;
        }

        public void CreateEmergency(string danger)
        {
            Console.WriteLine($"\n[СИСТЕМА МІСТА]: Увага! Спровоковано подію: {danger}");
            EmergencyOccurred?.Invoke(this, new CityEventArgs(danger));
        }

        public void Celebrate(string holidayName)
        {
            Console.WriteLine($"\n[СИСТЕМА МІСТА]: Сьогодні у нас свято: {holidayName}");
            HolidayStarted?.Invoke(this, new CityEventArgs(holidayName));
        }
    }


    public class Police
    {
        public void OnEmergency(object sender, CityEventArgs e)
        {
            Console.WriteLine($"[ПОЛІЦІЯ]: Виїжджаємо на місце події: {e.Message}. Час: {e.Time:HH:mm:ss}");
        }
    }

    public class CityHall
    {
        public void OnHoliday(object sender, CityEventArgs e)
        {
            Console.WriteLine($"[МЕРІЯ]: Організовуємо офіційне привітання до дня '{e.Message}'!");
        }
    }

    public class Citizen
    {
        public string Name { get; }
        public Citizen(string name) { Name = name; }

        public void ReactToEmergency(object sender, CityEventArgs e)
        {
            Console.WriteLine($"[МЕШКАНЕЦЬ {Name}]: Ой лишенько! Треба бути обережним через {e.Message}.");
        }

        public void ReactToHoliday(object sender, CityEventArgs e)
        {
            Console.WriteLine($"[МЕШКАНЕЦЬ {Name}]: Ура! Йду святкувати {e.Message}!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            City myCity = new City("Київ");

            Police police = new Police();
            CityHall cityHall = new CityHall();
            Citizen diana = new Citizen("Діана");
            Citizen alex = new Citizen("Олексій");

            myCity.EmergencyOccurred += police.OnEmergency;
            myCity.EmergencyOccurred += diana.ReactToEmergency;
            myCity.EmergencyOccurred += alex.ReactToEmergency;

            myCity.HolidayStarted += cityHall.OnHoliday;
            myCity.HolidayStarted += diana.ReactToHoliday;

            myCity.CreateEmergency("Великий затор у центрі міста");
            Thread.Sleep(1500); 

            myCity.Celebrate("День міста");
            Thread.Sleep(1500);

            myCity.CreateEmergency("Відключення електроенергії");

            Console.WriteLine("\nДіана переїхала з міста (відписка від подій)");
            myCity.EmergencyOccurred -= diana.ReactToEmergency;

            myCity.CreateEmergency("Ремонт доріг");

            Console.WriteLine("\nМоделювання завершено. Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}
