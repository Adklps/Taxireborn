
namespace TaxisFeladatOljetekmegFR;

using System.Globalization;

class Program
{
    static List<Taxi> taxis = new List<Taxi>();
    
    static void Main(string[] args)
    {
        Console.WriteLine("segicseg megorulok");
        GetFile();
        HaromDik(); 
        NegyDik();
        OtDik();
        HatDik();
        HetDik();
        NyolcDik();
    }
    
    static void GetFile()
    {
        string[] lines = File.ReadAllLines("../../../fuvar.csv");
        lines = lines.Skip(1).ToArray();
        foreach (string line in lines)
        {
            string[] values = line.Split(';');
            Taxi taxi = new Taxi(
                values[0], 
                DateTime.Parse(values[1]), 
                int.Parse(values[2]), 
                double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.6, 
                double.Parse(values[4].Replace(',', '.'), CultureInfo.InvariantCulture), 
                double.Parse(values[5].Replace(',', '.'), CultureInfo.InvariantCulture), 
                // Valamiert nem akar mukodni csak ha kicserelemxd
                values[6]
                );
            taxis.Add(taxi);
        }
    }

    static void HaromDik()
    {
        int fuvarokSzama = taxis.Count;
        Console.WriteLine($"3. feladat: {fuvarokSzama} fuvar lett teljesítve.");
    }

    static void NegyDik()
    {
        int fuvarokSzama = 0;
        double bevetel = 0;
        foreach (Taxi taxi in taxis)
        {
            if (taxi.TagziSzam == "6185")
            {
                fuvarokSzama++;
                bevetel += taxi.TakgziPrice;
            }
        }
        Console.WriteLine($"4. feladat: {fuvarokSzama} fuvar alatt ${bevetel} jött össze.");
    
    }

    static void OtDik()
    {
        Console.WriteLine("5. feladat: ");
        Dictionary<string, int> fizetesModok = new Dictionary<string, int>();
        foreach (Taxi taxi in taxis)
        {
            if (!fizetesModok.ContainsKey(taxi.VroomPayment))
            {
                fizetesModok.Add(taxi.VroomPayment, 1);
            }
            else
            {
                fizetesModok[taxi.VroomPayment]++;
            }
        }
        foreach (KeyValuePair<string, int> fizetesMod in fizetesModok)
        {
            Console.WriteLine($"\t{fizetesMod.Key}: {fizetesMod.Value} fuvar");
        }
    }
    // a siras kerulget xd
    static void HatDik()
    {
        Console.WriteLine("6. feladat: ");
        double osszTavolsag = 0;
        foreach (Taxi taxi in taxis)
        {
            osszTavolsag += taxi.TaksiTav;
        }
        Console.WriteLine($"\t{Math.Round(osszTavolsag, 2)} km lett összesen tekerve a verdákba.");
    }

    static void HetDik()
    {
       Console.WriteLine("7. feladat: Leghasznosabb mozdony:");

       Taxi leghosszabbFuvar = taxis.OrderByDescending(t=>t.TaksziTime).First();
       
       Console.WriteLine($"\tTaxi azonosító: {leghosszabbFuvar.TagziSzam}");
       Console.WriteLine($"\tFuvar hossza: {leghosszabbFuvar.TaksziTime} volt");
       Console.WriteLine($"\tMegtett távolság: {Math.Round(leghosszabbFuvar.TaksiTav, 2)} km");
       Console.WriteLine($"\tViteldíj: ${leghosszabbFuvar.TakgziPrice}");

    }
    // Itt robbant egy kaves redbull
    static void NyolcDik()
    {
        Console.WriteLine("8. feladat: hibás állományok/adatok feldolgozása");
        StreamWriter sw = new StreamWriter("../../../hibak.txt");
        List<Taxi> hibasTaxis = new List<Taxi>();
        // VALAMIERT ERROR VAN HA LETEZIK A HIBAK.TXT ES ERRE 45 PERC VOLT RAJONNOM
        foreach (Taxi taxi in taxis)
        {
            if (taxi is { TaksziTime: > 0, TakgziPrice: > 0.0, TaksiTav: 0.0 } )
            {
                hibasTaxis.Add(taxi);
            }
        }
        
        List<Taxi> sortedHibasTaxi = hibasTaxis.OrderBy(t=>t.TaxiStart).ToList();
        
        sw.WriteLine("taxi_id;indulas;idotartam;tavolsag;viteldij;borravalo;fizetes_modja");
        foreach (Taxi taxi in sortedHibasTaxi)
        {
            sw.WriteLine(taxi.CsvExp());
        }
    }   
}

class Taxi
{
    public string TagziSzam { get; set; }
    public DateTime TaxiStart { get; set; }
    public int TaksziTime { get; set; }
    public double TaksiTav { get; set; }
    public double TakgziPrice { get; set; }
    public double TaxgiJatt { get; set; }
    public string VroomPayment { get; set; }
    
    public Taxi(
        string TagziSzam, 
        DateTime TaxiStart, 
        int TaksziTime, 
        double TaksiTav,
        double TakgziPrice,
        double TaxgiJatt,
        string VroomPayment
        )
    {
        this.TagziSzam = TagziSzam;
        this.TaxiStart = TaxiStart;
        this.TaksziTime = TaksziTime;
        this.TaksiTav = TaksiTav;
        this.TakgziPrice = TakgziPrice;
        this.TaxgiJatt = TaxgiJatt;
        this.VroomPayment = VroomPayment;
    }

    public string CsvExp()
    {
        return $"{TagziSzam};{TaxiStart};{TaksziTime};{TaksiTav};{TakgziPrice};{TaxgiJatt};{VroomPayment}";
    }
}