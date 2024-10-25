namespace Schedule.Models.Database
{
    public class Linker_List : Linker
    {
        public static List<Linker> linkers { get; set; } = new List<Linker>();

        public Linker_List()
        {
            foreach (var date in Date_List.All_Date)
            {
                Linker temp = new Linker();

                temp.Date = date;

                for (int i = 1; i < date.CountPairs() && date.pairs[i] != null; i++)
                {
                    LinkPairsAndData pairsAndData = new LinkPairsAndData();
                    pairsAndData.One_Pair = date.pairs[i];

                    foreach (var Data in Data_List.All_Data)
                    {
                        if (pairsAndData.One_Pair.Pair_Rows[0] == Data.Rows[0]
                            && pairsAndData.One_Pair.Pair_Rows[1] == Data.Rows[1]
                            && pairsAndData.One_Pair.Pair_Rows[2] == Data.Rows[2])
                        {
                            pairsAndData.ManyData.Add(Data);
                        }
                    }

                    temp.DataAndPair.Add(pairsAndData);
                }

                linkers.Add(temp);
            }
        }
    }
}
