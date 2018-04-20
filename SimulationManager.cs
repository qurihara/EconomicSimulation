using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CenterSpace.NMath.Stats;
using System.Diagnostics;

namespace EconomicSimulation
{
    public class UmabanOddsPair : IComparable<UmabanOddsPair>
    {
        public UmabanOddsPair(int umaban, double odds)
        {
            this.umaban = umaban;
            this.odds = odds;
        }
        public int umaban;
        public double odds;

        public int CompareTo(UmabanOddsPair comp)
        {
            if (odds > comp.odds) return 1;
            if (odds < comp.odds) return -1;
            return 0;
        }
    }

    public class SimulationManager
    {
        public static bool OutputExpectedVote = true;
        public static bool BinAnalysis = false;//true;
        public static string binTableForwardFile = "binTableForward.csv";
        public static string binTableBackwardFile = "binTableBackward.csv";

        protected StreamWriter writer = null;
        public SimulationManager()
        {
        }
        public SimulationManager(StreamWriter sw) : this()
        {
            writer = sw;
        }

        protected List<EconomicPlayer> playerList;
        protected List<Race> raceList;

        public virtual double Simulate(int nEconomicPlayer, int nFirstBuyPlayer, int nRaces, Stream resultStream, bool WriteDetailedLog)
        {
            return Simulate(nEconomicPlayer, 0, nFirstBuyPlayer, 0, nRaces, resultStream, WriteDetailedLog);
        }
        public virtual double Simulate(int nEconomicPlayer, int npHitBuyPlayer, int nFirstBuyPlayer, int nRandomBuyPlayer, int nRaces, Stream resultStream, bool WriteDetailedLog)
        {
            return Simulate(nEconomicPlayer, npHitBuyPlayer, nFirstBuyPlayer, nRandomBuyPlayer, 0,0,0, nRaces, resultStream, WriteDetailedLog);
        }
        public virtual double Simulate(int nEconomicPlayer, int npHitBuyPlayer, int nFirstBuyPlayer, int nRandomBuyPlayer, int nSecondBuyPlayer, int nThirdBuyPlayer, int nEconomicFirstBuyPlayer, int nRaces, Stream resultStream, bool WriteDetailedLog)
        {
            return Simulate(nEconomicPlayer, npHitBuyPlayer, nFirstBuyPlayer, nRandomBuyPlayer, nSecondBuyPlayer, nThirdBuyPlayer, nEconomicFirstBuyPlayer,0, nRaces, resultStream, WriteDetailedLog);
        }
        public virtual double Simulate(int nEconomicPlayer, int npHitBuyPlayer, int nFirstBuyPlayer, int nRandomBuyPlayer, int nSecondBuyPlayer, int nThirdBuyPlayer, int nEconomicFirstBuyPlayer, int nEconomicHitterPlayer, int nRaces, Stream resultStream, bool WriteDetailedLog)
        {
            return Simulate(nEconomicPlayer, npHitBuyPlayer, nFirstBuyPlayer, nRandomBuyPlayer, nSecondBuyPlayer, nThirdBuyPlayer, nEconomicFirstBuyPlayer, nEconomicHitterPlayer,0, nRaces, resultStream, WriteDetailedLog);
        }
        public virtual double Simulate(int nEconomicPlayer, int npHitBuyPlayer, int nFirstBuyPlayer, int nRandomBuyPlayer, int nSecondBuyPlayer, int nThirdBuyPlayer, int nEconomicFirstBuyPlayer, int nEconomicHitterPlayer, int nEconomicBetaHitterPlayer, int nRaces, Stream resultStream, bool WriteDetailedLog)
        {
            return Simulate(nEconomicPlayer, npHitBuyPlayer, nFirstBuyPlayer, nRandomBuyPlayer, nSecondBuyPlayer, nThirdBuyPlayer, nEconomicFirstBuyPlayer, nEconomicHitterPlayer, nEconomicBetaHitterPlayer,0, nRaces, resultStream,WriteDetailedLog);
        }
        public virtual double Simulate(int nEconomicPlayer, int npHitBuyPlayer, int nFirstBuyPlayer, int nRandomBuyPlayer, int nSecondBuyPlayer, int nThirdBuyPlayer, int nEconomicFirstBuyPlayer, int nEconomicHitterPlayer, int nEconomicBetaHitterPlayer, int nEconomicOtokuBuyPlayer, int nRaces, Stream resultStream, bool WriteDetailedLog)
        {
            return Simulate(nEconomicPlayer, npHitBuyPlayer, nFirstBuyPlayer, nRandomBuyPlayer, nSecondBuyPlayer, nThirdBuyPlayer, nEconomicFirstBuyPlayer, nEconomicHitterPlayer, nEconomicBetaHitterPlayer, nEconomicOtokuBuyPlayer,0 , nRaces, resultStream, WriteDetailedLog);
        }
        public virtual double Simulate(int nEconomicPlayer, int npHitBuyPlayer, int nFirstBuyPlayer, int nRandomBuyPlayer, int nSecondBuyPlayer, int nThirdBuyPlayer, int nEconomicFirstBuyPlayer, int nEconomicHitterPlayer, int nEconomicBetaHitterPlayer, int nEconomicOtokuBuyPlayer, int nEconomicOtokuBetaBuyPlayer, int nRaces, Stream resultStream, bool WriteDetailedLog)
        {
            return Simulate(nEconomicPlayer, npHitBuyPlayer, nFirstBuyPlayer, nRandomBuyPlayer, nSecondBuyPlayer, nThirdBuyPlayer, nEconomicFirstBuyPlayer, nEconomicHitterPlayer, nEconomicBetaHitterPlayer, nEconomicOtokuBuyPlayer, nEconomicOtokuBetaBuyPlayer, 0, nRaces, resultStream, WriteDetailedLog);
        }
        public virtual double Simulate(int nEconomicPlayer, int npHitBuyPlayer, int nFirstBuyPlayer, int nRandomBuyPlayer, int nSecondBuyPlayer, int nThirdBuyPlayer, int nEconomicFirstBuyPlayer, int nEconomicHitterPlayer, int nEconomicBetaHitterPlayer, int nEconomicOtokuBuyPlayer, int nEconomicOtokuBetaBuyPlayer, int nEconomicAllOtokuBetaBuyPlayer, int nRaces, Stream resultStream, bool WriteDetailedLog)
        {
            return Simulate(nEconomicPlayer, npHitBuyPlayer, nFirstBuyPlayer, nRandomBuyPlayer, nSecondBuyPlayer, nThirdBuyPlayer, nEconomicFirstBuyPlayer, nEconomicHitterPlayer, nEconomicBetaHitterPlayer, nEconomicOtokuBuyPlayer, nEconomicOtokuBetaBuyPlayer, 0 ,0, nRaces, resultStream, WriteDetailedLog);
        }
        public virtual double Simulate(int nEconomicPlayer, int npHitBuyPlayer, int nFirstBuyPlayer, int nRandomBuyPlayer, int nSecondBuyPlayer, int nThirdBuyPlayer, int nEconomicFirstBuyPlayer, int nEconomicHitterPlayer, int nEconomicBetaHitterPlayer, int nEconomicOtokuBuyPlayer, int nEconomicOtokuBetaBuyPlayer, int nEconomicAllOtokuBetaBuyPlayer, int nEconomicAllOtokuBekiBuyPlayer, int nRaces, Stream resultStream, bool WriteDetailedLog)
        {
            StreamWriter sw = writer;
            if (writer == null)
            {
                sw = new StreamWriter(resultStream);
            }

            playerList = new List<EconomicPlayer>();
            int c = 0;
            for (int i = 0; i < nEconomicPlayer; i++) { playerList.Add(new EconomicPlayer(c)); c++; }
            for (int i = 0; i < npHitBuyPlayer; i++) {playerList.Add(new pHitBuyPlayer(c)); c++;}
            for (int i = 0; i < nFirstBuyPlayer; i++) {playerList.Add(new FirstBuyPlayer(c)); c++;}
            for (int i = 0; i < nRandomBuyPlayer; i++) {playerList.Add(new RandomBuyPlayer(c)); c++;}
            for (int i = 0; i < nSecondBuyPlayer; i++) {playerList.Add(new SecondBuyPlayer(c)); c++;}
            for (int i = 0; i < nThirdBuyPlayer; i++) {playerList.Add(new ThirdBuyPlayer(c)); c++;}
            for (int i = 0; i < nEconomicFirstBuyPlayer; i++) {playerList.Add(new EconomicFirstBuyPlayer(c)); c++;}
            for (int i = 0; i < nEconomicHitterPlayer; i++) {playerList.Add(new EconomicHitterPlayer(c)); c++;}
            for (int i = 0; i < nEconomicBetaHitterPlayer; i++) {playerList.Add(new EconomicBetaHitterPlayer(c)); c++;}
            for (int i = 0; i < nEconomicOtokuBuyPlayer; i++) {playerList.Add(new EconomicOtokuBuyPlayer(c)); c++;}
            for (int i = 0; i < nEconomicOtokuBetaBuyPlayer; i++) { playerList.Add(new EconomicOtokuBetaBuyPlayer(c)); c++; }
            for (int i = 0; i < nEconomicAllOtokuBetaBuyPlayer; i++) { playerList.Add(new EconomicAllOtokuBetaBuyPlayer(c)); c++; }
            for (int i = 0; i < nEconomicAllOtokuBekiBuyPlayer; i++) { playerList.Add(new EconomicAllOtokuBekiBuyPlayer(c)); c++; }
            raceList = new List<Race>();
            
            //for (int j = 0; j < nRaces; j++) raceList.Add(new Race());
            //for (int j = 0; j < nRaces; j++) raceList.Add(new pHitRace());
            //for (int j = 0; j < nRaces; j++) raceList.Add(new BetaBuyProbRace());

            for (int j = 0; j < nRaces; j++) raceList.Add(new AnyInvest_pHitStrictRace()); // 160311            

            //for (int j = 0; j < nRaces; j++) raceList.Add(new ExpT3ToyInvest_pHitStrictRace()); // 160222            
            //for (int j = 0; j < nRaces; j++) raceList.Add(new ConstToyInvest_pHitStrictRace()); // 160222            
            //for (int j = 0; j < nRaces; j++) raceList.Add(new ExpTm3ToyInvest_pHitStrictRace()); // 160222        
            //for (int j = 0; j < nRaces; j++) raceList.Add(new FirstAllInvest_pHitStrictRace()); // 160222        
            //for (int j = 0; j < nRaces; j++) raceList.Add(new LastAllInvest_pHitStrictRace(0.65)); // 160222        
    
            //for (int j = 0; j < nRaces; j++) raceList.Add(new ExpT3Invest_pHitStrictRace()); // 160218            
            
            //for (int j = 0; j < nRaces; j++) raceList.Add(new ExpT3InvestRace()); // 150530 
            //for (int j = 0; j < nRaces; j++) raceList.Add(new ExpTm3InvestRace()); // 150605
            //for (int j = 0; j < nRaces; j++) raceList.Add(new ConstInvestRace()); // 150605
            //for (int j = 0; j < nRaces; j++) raceList.Add(new ExpT3InvestMinorRace()); // 150626


            AddedDoubleBinaryPrecise[] adders = new AddedDoubleBinaryPrecise[Race.nHorses];
            for(int i=0;i<adders.Length;i++) adders[i] = new AddedDoubleBinaryPrecise(0d);

            binForwardSumTable = new List<int[]>();
            binBackwardSumTable = new List<int[]>();

            foreach (Race race in raceList)
            {
                double[] di = race.Simulate(playerList, sw,WriteDetailedLog);
                for (int i = 0; i < di.Length; i++) adders[i].Add(di[i]);

                if (SimulationManager.BinAnalysis)
                {
                    SumUpBinTable(race.binTableForward,binForwardSumTable);
                    SumUpBinTable(race.binTableBackward, binBackwardSumTable);
                }

            }

            if (SimulationManager.BinAnalysis)
            {
                OutputBinSumTable(binTableForwardFile, binForwardSumTable);
                OutputBinSumTable(binTableBackwardFile, binBackwardSumTable);
            }


            double ret = 0d;
            if (OutputExpectedVote)
            {
                if (!WriteDetailedLog)
                {
                    for (int i = 0; i < EconomicPlayer.expectedVote.Length; i++)
                    {
                        sw.Write(EconomicPlayer.expectedVote[i].ToString() + ",");
                    }
                    sw.Write(sw.NewLine);
                }
            }
            for (int i = 0; i < adders.Length; i++)
            {
                double v = adders[i].Value;
                if (!WriteDetailedLog)
                {
                    double o = v / (double)nRaces + EconomicPlayer.expectedVote[i];
                    sw.Write(o.ToString() + ",");
                }
                ret += v * v;
            }
            sw.Write(sw.NewLine);

            ret /= (double)Race.nHorses;
            ret /= (double)nRaces;
            //ret = Math.Sqrt(ret);

            if (writer == null)
            {
                sw.Close();
            }
            return ret;
        }

        protected List<int[]> binForwardSumTable;
        protected List<int[]> binBackwardSumTable;
        protected void SumUpBinTable(List<int[]> binTable, List<int[]> binSumTable)
        {
            if (binSumTable.Count == 0) //(binSumTable == null)
            {
                //binSumTable = new List<int[]>();
                for (int i = 0; i < binTable.Count; i++)
                {
                    binSumTable.Add(new int[10]);
                    for (int j = 0; j < 10; j++)
                    {
                        binSumTable[i][j] = binTable[i][j];
                    }
                }
            }
            else
            {
                for (int j = 0; j < binTable.Count; j++)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        binSumTable[j][i] += binTable[j][i];
                    }
                }

            }
        }
        protected void OutputBinSumTable(string path, List<int[]> binSumTable)
        {
            using (StreamWriter swo = new StreamWriter(path, false))
            {
                swo.WriteLine(",b1,b2,b3,b4,b5,b6,b7,b8,b9,b10");
                for (int i = 0; i < binSumTable.Count; i++)
                {
                    swo.Write("t" + i.ToString() + ",");
                    for (int j = 0; j < 10; j++)
                    {
                        swo.Write(binSumTable[i][j].ToString() + ",");
                    }
                    swo.Write(swo.NewLine);
                }
            }

        }

    }

    //public class CustomSimulationManager : SimulationManager
    //{
    //    public CustomSimulationManager()
    //    {
    //        playerList = new List<EconomicPlayer>();
    //        raceList = new List<Race>();
    //    }

    //    public void AddPlayer(EconomicPlayer player)
    //    {
    //        playerList.Add(player);
    //    }
    //    public void AddRace(Race race)
    //    {
    //        raceList.Add(race);
    //    }
    //    public virtual double Simulate(int nRaces, Stream resultStream, bool WriteDetailedLog)
    //    {
    //        StreamWriter sw = new StreamWriter(resultStream);

    //        AddedDoubleBinaryPrecise[] adders = new AddedDoubleBinaryPrecise[Race.nHorses];
    //        for (int i = 0; i < adders.Length; i++) adders[i] = new AddedDoubleBinaryPrecise(0d);
    //        foreach (Race race in raceList)
    //        {
    //            double[] di = race.Simulate(playerList, sw, WriteDetailedLog);
    //            for (int i = 0; i < di.Length; i++) adders[i].Add(di[i]);
    //        }

    //        double ret = 0d;
    //        if (!WriteDetailedLog)
    //        {
    //            for (int i = 0; i < EconomicPlayer.expectedVote.Length; i++)
    //            {
    //                sw.Write(EconomicPlayer.expectedVote[i].ToString() + ",");
    //            }
    //            sw.Write(sw.NewLine);
    //        }
    //        for (int i = 0; i < adders.Length; i++)
    //        {
    //            double v = adders[i].Value;
    //            if (!WriteDetailedLog)
    //            {
    //                double o = v / (double)nRaces + EconomicPlayer.expectedVote[i];
    //                sw.Write(o.ToString() + ",");
    //            }
    //            ret += v * v;
    //        }
    //        sw.Write(sw.NewLine);

    //        ret /= (double)Race.nHorses;
    //        ret /= (double)nRaces;
    //        //ret = Math.Sqrt(ret);

    //        sw.Close();
    //        return ret;
    //    }
    //}

    public class Race
    {
        public delegate double GetBuyProbDelegate(int mbr,int id);
        public static int nPlayers = 0;
        public static int nHorses = 16;
        public static int initialMBR = -20;
        public static int initialMBRVariance = 10;
        public static int initialNVoteMax = 100;
        public static int MBRStep = 2;
        public static int lastMBR = -2;
        public List<int> nVoteList;
        public List<double> oddsList;
        public static double tera = 0.25d;

        public static bool Dump1Nin = false;
        public static bool ChangeStartTime = false;

        protected List<UmabanOddsPair> umabanOddsPairList;

        protected static Random rand = new Random(DateTime.Now.Millisecond);

        protected static string binRawFile = "binraw.csv";

        public List<int[]> binTableForward;
        public List<int[]> binTableBackward;

        public Race()
        {
            //odds初期化
            nVoteList = new List<int>();
            oddsList = new List<double>();

            umabanOddsPairList = new List<UmabanOddsPair>();

            //Init();
            //UpdateOdds();
        }

        protected virtual void Init()
        {
            for (int i = 0; i < nHorses; i++)
            {
                int nv = rand.Next(0, initialNVoteMax);
                nVoteList.Add(nv);
                oddsList.Add(0d);
            }
        }

        public double[] Simulate(List<EconomicPlayer> playerList, StreamWriter sw, bool WriteDetailedLog)
        {
            nPlayers = playerList.Count;
            Init();
            UpdateOdds();
            foreach (EconomicPlayer p in playerList) p.Init();
            int nVoted = 0;
            foreach (int nv in nVoteList) nVoted += nv;
            for (int i = 0; i < nVoted; i++)
            {
                EconomicPlayer p = playerList[i];
                p.Init(true);
            }

            int initT = initialMBR;
            if (ChangeStartTime)
            {
                initT = initT - (int)(rand.Next(initialMBRVariance * 2) / 2);

            }
            if (SimulationManager.BinAnalysis)
            {
                if (File.Exists(binRawFile)) File.Delete(binRawFile);
            }
            //for (int t = initT; t < 0; t = t + MBRStep)
            for (int t = initT; t <= lastMBR; t = t + MBRStep)
            {
                foreach (EconomicPlayer player in playerList)
                {
                    player.Simulate(nVoteList, oddsList,t,this.GetBuyProb);
                }
                UpdateOdds();
                if (SimulationManager.BinAnalysis)
                {
                    UpdateBin();
                }

                if (Dump1Nin)
                {
                    using(StreamWriter swN = new StreamWriter("1nin.csv",true)){
                        swN.Write((1d-tera) / oddsList[0] + ",");
                    }
                }

                //DumpVote(sw); // 事前時系列を出すとき
            }

            if (SimulationManager.BinAnalysis)
            {
                SumUpBin(out binTableForward, out binTableBackward);
            }

            if (Dump1Nin)
            {
                using (StreamWriter swN = new StreamWriter("1nin.csv", true))
                {
                    swN.Write(swN.NewLine);
                }
            }
            double[] dif = DumpVote(sw,WriteDetailedLog); // 最終結果だけ出すとき

            return dif;
        }
        protected void UpdateOdds()
        {
            int sum = 0;
            foreach (int nVote in nVoteList) sum += nVote;
            for (int i = 0; i < nVoteList.Count; i++)
            {
                oddsList[i] = (1d - tera) * (double)sum / (double)nVoteList[i];
            }

            //oddsList.Sort();//昇順
            //nVoteList.Sort();
            //nVoteList.Reverse();
        }

        protected double[] DumpVote(StreamWriter sw,bool WriteDetailedLog)
        {
            double[] dif = new double[nHorses];
            int voteSum = 0;
            voteListForDump.Clear();
            oddsListForDump.Clear();
            for (int i = 0; i < nVoteList.Count; i++) {
                voteListForDump.Add(nVoteList[i]);
                oddsListForDump.Add(oddsList[i]);
                voteSum += nVoteList[i];
            }
            //oddsListForDump.Sort(); // この辺sortしていいの？  ここは諸悪の根源だった。ソートしない！！ 160218
            //voteListForDump.Sort();
            //voteListForDump.Reverse();

            for (int i = 0; i < voteListForDump.Count; i++)
            {
                double v = (double)voteListForDump[i] / (double)voteSum;
                double d = v - EconomicPlayer.expectedVote[i];
                dif[i] = d;
                if (WriteDetailedLog)
                {
                    sw.Write(v);
                    sw.Write(",");
                }
            }
            if (WriteDetailedLog)
            {
                sw.Write(",");

                for (int i = 0; i < oddsListForDump.Count; i++)
                {
                    sw.Write(oddsListForDump[i].ToString() + ",");
                }
                sw.Write(",");
                sw.Write(voteSum.ToString());
                sw.Write(sw.NewLine);
            }
            return dif;
        }
        //protected void DumpVote(StreamWriter sw)
        //{
        //    int sum = 0;
        //    foreach (int nVote in nVoteList) sum += nVote;
        //    for (int i = 0; i < nVoteList.Count; i++)
        //    {
        //        sw.Write((double)nVoteList[i]/(double)sum);
        //        if (i < nVoteList.Count - 1) sw.Write(",");
        //    }
        //    sw.Write(sw.NewLine);
        //}

        protected static List<int> voteListForDump = new List<int>();
        protected static List<double> oddsListForDump = new List<double>();

        protected virtual double GetBuyProb(int mbr,int id)
        {
            //todo MBRがゼロに近づくほど大きくなるような確率を返す。　＞＞　todo: y=-1/xや直線を使う。max(mbr)で確率1にする
            return -1d / (double)mbr;
        }

        protected void UpdateBin()
        {
            umabanOddsPairList.Clear();

            for (int i = 0; i < oddsList.Count; i++)
            {
                umabanOddsPairList.Add(new UmabanOddsPair(i, oddsList[i]));
            }
            umabanOddsPairList.Sort(); // odds昇順 == vote降順
            using (StreamWriter swN = new StreamWriter(binRawFile, true))
            {
                //   nHorse/10を基準として、ソート順にパーセンタイルを割り当てる（ビンに入れる）
                double cur = 0;
                double nex = 0;
                int binInd = 0;
                int[] umabanBinList = new int[nHorses];
                while (binInd < 10)
                {
                    nex = cur + (double)(nHorses) / 10d;

                    for (int i = 0; i < oddsList.Count; i++)
                    {
                        if (cur <= i && i < nex)
                        {
                            umabanBinList[umabanOddsPairList[i].umaban] = binInd; // よく考えよ
                        }
                    }                  
                    cur = nex;
                    binInd++;
                }

                for (int i = 0; i < oddsList.Count; i++)
                {
                    swN.Write(umabanBinList[i].ToString() + ",");
                }                  

                swN.Write(swN.NewLine);
            }

        }
        protected void SumUpBin(out List<int[]> binTableForward, out List<int[]> binTableBackward)
        {
            //input:binraw.csv. output:binTableForward, binTableBackward
            int nTime = 0;
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(binRawFile))
            {
                while (!sr.EndOfStream)
                {
                    lines.Add(sr.ReadLine());
                    nTime++;
                }
            }

            binTableForward = new List<int[]>();
            binTableBackward = new List<int[]>();


            int[,] eleTable = new int[nHorses, nTime];
            for (int i = 0; i < nTime; i++)
            {
                string[] ele = lines[i].Split(',');
                for (int j = 0; j < nHorses; j++)
                {
                    eleTable[j, i] = int.Parse(ele[j]);
                }
                binTableForward.Add(new int[10]);
                binTableBackward.Add(new int[10]);
            }

            // forward
            for (int umaban = 0; umaban < nHorses; umaban++)
            {
                int initBin = eleTable[umaban, 0];
                int countContinue = 0;// ゼロのとき、１回も継続しなかった、となる。最大９回継続する。
                for (int t = 1; t < nTime; t++)
                {
                    if (initBin == eleTable[umaban, t])
                    {
                        countContinue++;
                    }
                    else
                    {
                        break;
                    }
                }
                binTableForward[countContinue][initBin]++;
            }

            // backward
            for (int umaban = 0; umaban < nHorses; umaban++)
            {
                int initBin = eleTable[umaban, nTime-1];
                int countContinue = 0;// ゼロのとき、１回も継続しなかった、となる。最大９回継続する。
                for (int t = nTime-2; t >= 0; t--)
                {
                    if (initBin == eleTable[umaban, t])
                    {
                        countContinue++;
                    }
                    else
                    {
                        break;
                    }
                }
                binTableBackward[countContinue][initBin]++;
            }

        }
    }

    public class pHitRace : Race
    {
        protected override void Init()
        {
            int[] nv = GetPHitVote();
            for (int i = 0; i < nHorses; i++)
            {
                nVoteList.Add(nv[i]);
                oddsList.Add(0d);
            }
        }
        protected virtual int[] GetPHitVote()
        {
            int[] nv = new int[nHorses];
            for (int i = 0; i < initialNVoteMax * nHorses; i++)
            {
                double val = rand.NextDouble();
                int horseToBuy = 0;
                double sum = 0;
                for (horseToBuy = 0; horseToBuy < nHorses; horseToBuy++)
                {
                    //sum += pHitBuyPlayer.winRatio[horseToBuy];
                    sum += EconomicPlayer.expectedVote[horseToBuy]; // 160217
                    if (val <= sum) break;
                }
                if (horseToBuy == nHorses) horseToBuy = 0; // added 150605

                nv[horseToBuy]++;
            }
            return nv;
        }
    }


    public class EconomicPlayer
    {
        protected static Random rand = new Random(DateTime.Now.Millisecond);

        protected int id;
        protected bool bought;
        public bool Bought
        {
            get { return bought; }
            //set { bought = value; }
        }

        public EconomicPlayer(int id)
        {
            bought = false;
            this.id = id;
        }

        public void Init()
        {
            bought = false;
        }
        public void Init(bool _bought)
        {
            bought = _bought;
        }
        public virtual void Simulate(List<int> nVote, List<double> odds,int mbr, Race.GetBuyProbDelegate getBuyProb)
        {
            //各プレーヤーは各レースで１度だけ購買活動を行なう。
            if (!bought)
            {
                //買うという判断をするのは、その馬のオッズが予定オッズよりも割高であるとき。予定オッズ：その人気の確定時のオッズの期待値
                //現状では，事前順位から予定オッズを見ているのではなく，競馬新聞のような形で全員が馬の順位をOracle的に知っている設定（だと思う）
                List<int> canBuyList = GetCanBuyList(odds);
                if (canBuyList.Count > 0)
                {
                    //どの馬を買うか、という選択はオッズ割高馬リストから毎分ランダムに１頭だけ決める。
                    int horseToBuy = canBuyList[rand.Next(canBuyList.Count)];
                    //さらに、実際の買う判断を行なう。MBRがゼロに近づくほど確率が高くなる　
                    if (rand.NextDouble() < getBuyProb(mbr,id))
                    {
                        nVote[horseToBuy]++;
                        bought = true;
                    }
                }
            }
        }
        protected List<int> GetCanBuyList(List<double> odds)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < odds.Count; i++)
            {
                if (CanBuy(i, odds))
                {
                    res.Add(i);
                }
            }
            return res;
        }
        protected virtual bool CanBuy(int horseToBuy,List<double> odds)
        {
            return (odds[horseToBuy] > expectedOdds[horseToBuy]);
        }

        //protected static double GetBuyProb(int mbr)
        //{
        //    //todo MBRがゼロに近づくほど大きくなるような確率を返す。　＞＞　todo: y=-1/xや直線を使う。max(mbr)で確率1にする
        //    return -1d/(double)mbr;
        //}

//        //確率は確定オッズ平均であって、勝率の逆数ではない。これを逆数とって0.75かけてもpHitにはならないことに注意．avg(0.75/odds)=pHitが正解．
//        protected static double[] expectedOdds = {
//2.2202770404104823,
//4.3329419589026754,
//6.4677523541418411,
//9.4373723878327525,
//13.444834068387518,
//19.130513058764791,
//27.047127377373094,
//37.801628473809892,
//51.989969852374706,
//67.668518541297743,
//86.361626013227706,
//106.3890595576085,
//114.64096862420668,
//137.62398439772585,
//145.12663068978682,
//162.9125983170637
//                                                  };
        protected static double[] expectedOdds = {              
// *.75わすれた．
//2.6643291618819755467869522472284,
//5.4154752621090026860757300060653,
//7.6362109121453934557672482913978,
//10.6801093643198906356801093643198,
//16.0756197151400186477188695624216,
//21.1662609799978833739020002116626,
//30.0003000030000300003000030000300,
//37.5445841937300544396470809085789,
//60.0781015319915890657855211775307,
//80.6646769379688634347019440187142,
//106.4509261230572705982542048115818,
//117.8411501296252651425877916568465,
//161.5508885298869143780290791599353,
//344.7087211306446053085143054119269,
//368.0529996319470003680529996319470,
//127.0002540005080010160020320040640

1.998246871,
4.061606447,
5.727158184,
8.010082023,
12.05671479,
15.87469573,
22.500225,
28.15843815,
45.05857615,
60.4985077,
79.83819459,
88.3808626,
121.1631664,
258.5315408,
276.0397497,
95.2501905,
                                                 };

        public static double[] expectedVote = {
.375329,
.184656,
.130955,
.093632,
.062206,
.047245,
.033333,
.026635,
.016645,
.012397,
.009394,
.008486,
.006190,
.002901,
.002717,
.007874,
                                                 };
        //1	.375329	2.6643291618819755467869522472284
        //2	.184656	5.4154752621090026860757300060653
        //3	.130955	7.6362109121453934557672482913978
        //4	.093632	10.6801093643198906356801093643198
        //5	.062206	16.0756197151400186477188695624216
        //6	.047245	21.1662609799978833739020002116626
        //7	.033333	30.0003000030000300003000030000300
        //8	.026635	37.5445841937300544396470809085789
        //9	.016645	60.0781015319915890657855211775307
        //10	.012397	80.6646769379688634347019440187142
        //11	.009394	106.4509261230572705982542048115818
        //12	.008486	117.8411501296252651425877916568465
        //13	.006190	161.5508885298869143780290791599353
        //14	.002901	344.7087211306446053085143054119269
        //15	.002717	368.0529996319470003680529996319470
        //16	.007874	127.0002540005080010160020320040640


//        protected static new double[] expectedOdds = {              // added 150605 一様勝率のときの実験。
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//12,
//                                                 };

//        public static new double[] expectedVote = {
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//0.0625,
//                                                 };
        public static void SetExpectedVoteOdds(double[] exVote, double[] exOdds)
        {
            expectedVote = exVote;
            expectedOdds = exOdds;
            TestExpectedVote(); // debug
        }

        [Conditional("DEBUG")]
        static protected void TestExpectedVote()
        {
            System.Diagnostics.Debug.WriteLine("TestExpectedVote");
            double sum = 0d;
            for (int i = 0; i < Race.nHorses; i++)
            {
                sum += expectedVote[i];
                System.Diagnostics.Debug.WriteLine(i.ToString() + ", " +  expectedVote[i].ToString() + ", " + expectedOdds[i]);
            }
            System.Diagnostics.Debug.WriteLine("Sum of Vote = " + sum.ToString());
        }

    }

    public class EconomicFirstBuyPlayer : EconomicPlayer
    {
        public EconomicFirstBuyPlayer(int id)
            : base(id)
        {
        }
        public override void Simulate(List<int> nVote, List<double> odds, int mbr, Race.GetBuyProbDelegate getBuyProb)
        {
            //各プレーヤーは各レースで１度だけ購買活動を行なう。
            if (!bought)
            {
                //買うという判断をするのは、その馬のオッズが予定オッズよりも割高であるとき。予定オッズ：その人気の確定時のオッズの期待値
                List<int> canBuyList = GetCanBuyList(odds);
                if (canBuyList.Count > 0)
                {
                    //どの馬を買うか、という選択はオッズ割高馬リストの中で最高人気のもの．
                    int horseToBuy = canBuyList[0];
                    //さらに、実際の買う判断を行なう。MBRがゼロに近づくほど確率が高くなる　
                    if (rand.NextDouble() < getBuyProb(mbr,id))
                    {
                        nVote[horseToBuy]++;
                        bought = true;
                    }
                }
            }

        }
    }

    public class FirstBuyPlayer : EconomicPlayer
    {
        public FirstBuyPlayer(int id)
            : base(id)
        {
        }

        public override void Simulate(List<int> nVote, List<double> odds, int mbr, Race.GetBuyProbDelegate getBuyProb)
        {
            //各プレーヤーは各レースで１度だけ購買活動を行なう。
            if (!bought)
            {
                //常に一番強い馬買い
                int horseToBuy = 0;
                //買うという判断をするのは、その馬のオッズが1以上の時．
                if (odds[horseToBuy] >= 1d)
                {
                    //さらに、実際の買う判断を行なう。MBRがゼロに近づくほど確率が高くなる　
                    if (rand.NextDouble() < getBuyProb(mbr,id))
                    {
                        nVote[horseToBuy]++;
                        bought = true;
                    }
                }
            }
        }
    }
    public class SecondBuyPlayer : EconomicPlayer
    {
        public SecondBuyPlayer(int id)
            : base(id)
        {
        }

        public override void Simulate(List<int> nVote, List<double> odds, int mbr, Race.GetBuyProbDelegate getBuyProb)
        {
            //各プレーヤーは各レースで１度だけ購買活動を行なう。
            if (!bought)
            {
                //常に２番人気馬買い
                int horseToBuy = 1;
                //買うという判断をするのは、その馬のオッズが1以上の時．
                if (odds[horseToBuy] >= 1d)
                {
                    //さらに、実際の買う判断を行なう。MBRがゼロに近づくほど確率が高くなる　
                    if (rand.NextDouble() < getBuyProb(mbr,id))
                    {
                        nVote[horseToBuy]++;
                        bought = true;
                    }
                }
            }
        }
    }

    public class ThirdBuyPlayer : EconomicPlayer
    {
        public ThirdBuyPlayer(int id)
            : base(id)
        {
        }

        public override void Simulate(List<int> nVote, List<double> odds, int mbr, Race.GetBuyProbDelegate getBuyProb)
        {
            //各プレーヤーは各レースで１度だけ購買活動を行なう。
            if (!bought)
            {
                //常に３番人気馬買い
                int horseToBuy = 2;
                //買うという判断をするのは、その馬のオッズが1以上の時．
                if (odds[horseToBuy] >= 1d)
                {
                    //さらに、実際の買う判断を行なう。MBRがゼロに近づくほど確率が高くなる　
                    if (rand.NextDouble() < getBuyProb(mbr,id))
                    {
                        nVote[horseToBuy]++;
                        bought = true;
                    }
                }
            }
        }
    }

    public class RandomBuyPlayer : EconomicPlayer
    {
        public RandomBuyPlayer(int id)
            : base(id)
        {
        }

        public override void Simulate(List<int> nVote, List<double> odds, int mbr,Race.GetBuyProbDelegate getBuyProb)
        {
            //各プレーヤーは各レースで１度だけ購買活動を行なう。
            if (!bought)
            {
                //ランダムな馬券を買う
                int horseToBuy = rand.Next(nVote.Count);
                //買うという判断をするのは、その馬のオッズが1以上の時．
                if (odds[horseToBuy] >= 1d)
                {
                    //さらに、実際の買う判断を行なう。MBRがゼロに近づくほど確率が高くなる　
                    if (rand.NextDouble() < getBuyProb(mbr,id))
                    {
                        nVote[horseToBuy]++;
                        bought = true;
                    }
                }
            }
        }
    }

    public class pHitBuyPlayer : EconomicPlayer
    {
        public pHitBuyPlayer(int id)
            : base(id)
        {
        }

        public override void Simulate(List<int> nVote, List<double> odds, int mbr, Race.GetBuyProbDelegate getBuyProb)
        {
            //各プレーヤーは各レースで１度だけ購買活動を行なう。
            if (!bought)
            {
                //
                double val = rand.NextDouble();
                int horseToBuy = 0;
                double sum = 0;
                for (horseToBuy = 0; horseToBuy < nVote.Count; horseToBuy++)
                {
                    sum += winRatio[horseToBuy];
                    if (val <= sum) break;
                }
                //買うという判断をするのは、その馬のオッズが1以上の時．
                if (odds[horseToBuy] >= 1d)
                {
                    //さらに、実際の買う判断を行なう。MBRがゼロに近づくほど確率が高くなる　
                    if (rand.NextDouble() < getBuyProb(mbr,id))
                    {
                        nVote[horseToBuy]++;
                        bought = true;
                    }
                }
            }
        }
//        protected static double[] winRatio = {
//0.375329,
//0.184656,
//0.130955,
//0.093632,
//0.062206,
//0.047245,
//0.033333,
//0.026635,
//0.016645,
//0.012397,
//0.009394,
//0.008486,
//0.00619,
//0.002901,
//0.002717,
//0.007874
//        }; // sum =1.020595
        public static double[] winRatio = {
0.367755084,
0.180929752,
0.128312406,
0.091742562,
0.06095072,
0.046291624,
0.03266036,
0.026097522,
0.016309114,
0.012146836,
0.009204435,
0.008314758,
0.006065089,
0.00284246,
0.002662173,
0.007715107
        }; // sum =1

    }

    //未完
    public class EconomicHitterPlayer : EconomicPlayer
    {
        public EconomicHitterPlayer(int id)
            : base(id)
        {
        }

        public override void Simulate(List<int> nVote, List<double> odds, int mbr, Race.GetBuyProbDelegate getBuyProb)
        {
            //各プレーヤーは各レースで１度だけ購買活動を行なう。
            if (!bought)
            {
                //買うという判断をするのは、その馬のオッズが予定オッズよりも割高であるとき。予定オッズ：その人気の確定時のオッズの期待値
                //現状では，事前順位から予定オッズを見ているのではなく，競馬新聞のような形で全員が馬の順位をOracle的に知っている設定（だと思う）
                List<int> canBuyList = GetCanBuyList(odds);
                if (canBuyList.Count > 0)
                {
                    //どの馬を買うか、という選択はオッズ割高馬リストから毎分ランダムに１頭だけ決める。ただし高人気ほど買いやすい
                    //int horseToBuy = canBuyList[rand.Next(canBuyList.Count)];
                    int horseToBuy = GetHorseToBuy(canBuyList.Count);

                    horseToBuy = canBuyList[horseToBuy];

                    //さらに、実際の買う判断を行なう。MBRがゼロに近づくほど確率が高くなる　
                    if (rand.NextDouble() < getBuyProb(mbr,id))
                    {
                        nVote[horseToBuy]++;
                        bought = true;
                    }
                }
            }
        }
        protected virtual int GetHorseToBuy(int nCand)
        {
            int horseToBuy = 0;
            do
            {
                double val = rand.NextDouble();
                double sum = 0;
                for (horseToBuy = 0; horseToBuy < buyRatio.Length; horseToBuy++)
                {
                    sum += buyRatio[horseToBuy];
                    if (val <= sum) break;
                }
                if (horseToBuy == buyRatio.Length) horseToBuy = 0;

            } while (horseToBuy >= nCand);
            return horseToBuy;
        }
        public static double[] buyRatio = {
0.5,
0.25,
0.125,
0.0625,
0.03125,
0.015625,
0.0078125,
0.00390625,
0.001953125,
0.000976563,
0.000488281,
0.000244141,
0.00012207,
6.10352E-05,
3.05176E-05,
1.52588E-05
        };
    }
    public class EconomicBetaHitterPlayer : EconomicHitterPlayer
    {
        public EconomicBetaHitterPlayer(int id)
            : base(id)
        {
        }

        public static double Alpha // 0< alpha <= 1 で変化することで，EconomicFirstからEcnomic（一様分布）をエミュレートできる
        {
            set{
                dist.Alpha = value;
            }
            get{
                return dist.Alpha;
            }
        }
        public static double Beta // 実質的につかわない．1で固定
        {
            set
            {
                dist.Beta = value;
            }
            get
            {
                return dist.Beta;
            }
        }
        protected static BetaDistribution dist = new CenterSpace.NMath.Stats.BetaDistribution();
        protected override int GetHorseToBuy(int nCand)
        {
            if (nCand <= 1) return 0;

            int horseToBuy = 0;
            //dist.Alpha = Alpha;
            //dist.Beta = Beta;
            double r = rand.NextDouble();
            double val = dist.InverseCDF(r);
            
            double sum = 0;
            for (horseToBuy = 0; horseToBuy < nCand; horseToBuy++)
            {
                sum += 1d / (double)nCand;
                if (val <= sum) break;
            }

            return horseToBuy;
        }
    }

    //未完で使ってない
    public class TrueEconomicBetaHitterPlayer : EconomicBetaHitterPlayer
    {
        public TrueEconomicBetaHitterPlayer(int id)
            : base(id)
        {
        }

        protected override bool CanBuy(int horseToBuy, List<double> odds)
        {
            return (odds[horseToBuy] > expectedOddsWithoutTera[horseToBuy]*(1d - Race.tera));
        }

        protected static double[] expectedOddsWithoutTera = {              
// *.75してないやつ
2.6643291618819755467869522472284,
5.4154752621090026860757300060653,
7.6362109121453934557672482913978,
10.6801093643198906356801093643198,
16.0756197151400186477188695624216,
21.1662609799978833739020002116626,
30.0003000030000300003000030000300,
37.5445841937300544396470809085789,
60.0781015319915890657855211775307,
80.6646769379688634347019440187142,
106.4509261230572705982542048115818,
117.8411501296252651425877916568465,
161.5508885298869143780290791599353,
344.7087211306446053085143054119269,
368.0529996319470003680529996319470,
127.0002540005080010160020320040640
                                                 };

    }

    //未完
    public class BetaBuyProbRace : pHitRace
    {

        public static double Alpha // 実質的につかわない．1で固定
        {
            set
            {
                dist.Alpha = value;
            }
            get
            {
                return dist.Alpha;
            }
        }
        public static double Beta // 0< beta <= 1 で変化させる．1で一様分布
        {
            set
            {
                dist.Beta = value;
            }
            get
            {
                return dist.Beta;
            }
        }
        protected static BetaDistribution dist = new CenterSpace.NMath.Stats.BetaDistribution();

        //mbrは-20くらいから-2までの10区間です． -20, -22, -24, -26, -28がChangeStartTimeで加わる
        protected override double GetBuyProb(int mbr,int id)
        {
            //todo MBRがゼロに近づくほど大きくなるような確率を返す。　＞＞　ベータ関数でやる
            double x1 = 1d - (double)(mbr + Race.MBRStep) / (double) Race.initialMBR;
            double x0 = 1d - (double)mbr / (double)Race.initialMBR;
            double val = dist.CDF(x1) - dist.CDF(x0);
            return val;
            //return -1d / (double)mbr;
        }
    }

    /// <summary>
    /// 050530追加。（１)購入関数が香港実測investのexp(t^3)に比例する。　＞＞　使用禁止！
    /// </summary>
    public class ExpT3InvestRace : pHitRace
    {
        public static new int initialMBR = -20; // 注： static new はうまくいきません。
        public static new int MBRStep = 1; // 注： static new はうまくいきません。
        public static new int initialMBRVariance = 0; // 注： static new はうまくいきません。
        public static new int lastMBR = 0; // 注： static new はうまくいきません。
        //mbrは-20くらいから-2までの10区間です． -20, -22, -24, -26, -28がChangeStartTimeで加わる >> 上記設定により、 -20,-19, ,,, , 0まで。
        protected override double GetBuyProb(int mbr,int id)
        {           
            double idp = (double)id / (double)nPlayers;
//            logとらずフィット
//            A	B	C
//            0.393616816	3.287090348	23.20740297
//            investritsu=A*exp( (mbr/C)^B )		
//            logとってフィット
//            A	B	C
//          0.388343666	2.948029642	23.46624499
//          investritsu=A*exp( (mbr/C)^B )		

            double shifted_mbr = (double)(mbr + 23); 
            //            logとらずフィット
            double A = 0.393616816;
            double B = 3.287090348;
            double C = 23.20740297;
            double investritsu = A * Math.Exp(Math.Pow(shifted_mbr / C,B));
            if (investritsu >= idp) return 1d;
            return 0d;

        }
    }

    /// <summary>
    /// 150626 土谷の分析により「かならず毎回フリップするからMBRの数の偶奇でバイアス方向がかわるかも」と言われたので、MBRの終点を１つ減らしてみた。＞＞結果はほとんどかわらなかった。　＞＞　使用禁止！
    /// </summary>
    public class ExpT3InvestMinorRace : ExpT3InvestRace
    {
        public static new int lastMBR = -1;
    }
    /// <summary>
    /// 検証用に作った、exp(-t^3)に比例するもの 　＞＞　使用禁止！
    /// </summary>
    public class ExpTm3InvestRace : ExpT3InvestRace
    {
        protected override double GetBuyProb(int mbr, int id)
        {
            double idp = (double)id / (double)nPlayers;
            double shifted_mbr = (double)(mbr + 23);
            //            logとらずフィット
            double A = 0.393616816;
            double B = 3.287090348;
            double C = 23.20740297;
            double investritsu = A * Math.Exp(-1d*Math.Pow(shifted_mbr / C, B));
            if (investritsu >= idp) return 1d;
            return 0d;
        }
    }
    /// <summary>
    /// 検証用に作った、単純な線形で投票。 　＞＞　使用禁止！
    /// </summary>
    public class ConstInvestRace : ExpT3InvestRace
    {
        protected override double GetBuyProb(int mbr, int id)
        {
            double idp = (double)id / (double)nPlayers;
            double shifted_mbr = (double)(mbr + 20d);
            double investritsu = (double)shifted_mbr / 21d;
            if (investritsu >= idp) return 1d;
            return 0d;
        }
    }

    public class EconomicOtokuBuyPlayer : EconomicPlayer
    {
        public EconomicOtokuBuyPlayer(int id) : base(id) { }

        protected virtual int GetHorseToBuy(int nCand)
        {
            return 0; // 常に一番お得なもの
        }
        public override void Simulate(List<int> nVote, List<double> odds, int mbr, Race.GetBuyProbDelegate getBuyProb)
        {
            //各プレーヤーは各レースで１度だけ購買活動を行なう。
            if (!bought)
            {
                //買うという判断をするのは、その馬のオッズが予定オッズよりも割高であるとき。予定オッズ：その人気の確定時のオッズの期待値
                //現状では，事前順位から予定オッズを見ているのではなく，競馬新聞のような形で全員が馬の順位をOracle的に知っている設定（だと思う）
                List<int> canBuyList = GetCanBuyList(odds);
                if (canBuyList.Count > 0)
                {
                    //どの馬を買うか、という選択はオッズ割高度合いのもっとも高いもの。
                    int horseToBuy = canBuyList[GetHorseToBuy(canBuyList.Count)];

                    //System.Diagnostics.Debug.WriteLine("@" + horseToBuy.ToString()); //debug

                    //さらに、実際の買う判断を行なう。MBRがゼロに近づくほど確率が高くなる　
                    if (rand.NextDouble() < getBuyProb(mbr, id))
                    {
                        nVote[horseToBuy]++;
                        bought = true;
                    }
                }
            }
        }
        protected new List<int> GetCanBuyList(List<double> odds)
        {
            List<UmabanOddsPair> otokuList = new List<UmabanOddsPair>();
            for (int i = 0; i < odds.Count; i++)
            {
                if (CanBuy(i, odds))
                {
                    otokuList.Add(new UmabanOddsPair(i, (odds[i] - expectedOdds[i]) * expectedVote[i]));
                }
            }
            otokuList.Sort();// asc order
            otokuList.Reverse();// desc order
            List<int> res = new List<int>();
            foreach (UmabanOddsPair p in otokuList) res.Add(p.umaban);
            return res;
        }
    }
    public class EconomicOtokuBetaBuyPlayer : EconomicOtokuBuyPlayer
    {
        public EconomicOtokuBetaBuyPlayer(int id) : base(id) { }

        public static double Alpha // 0< alpha <= 1 で変化することで，EconomicFirstからEcnomic（一様分布）をエミュレートできる
        {
            set
            {
                dist.Alpha = value;
            }
            get
            {
                return dist.Alpha;
            }
        }
        public static double Beta // 実質的につかわない．1で固定
        {
            set
            {
                dist.Beta = value;
            }
            get
            {
                return dist.Beta;
            }
        }
        static EconomicOtokuBetaBuyPlayer()
        {
            dist = new CenterSpace.NMath.Stats.BetaDistribution();
            dist.Alpha = 1d;
            dist.Beta = 1d;
        }
        protected static BetaDistribution dist;// = new CenterSpace.NMath.Stats.BetaDistribution();
        protected override int GetHorseToBuy(int nCand)
        {
            if (nCand <= 1) return 0;

            int horseToBuy = 0;
            //dist.Alpha = Alpha;
            //dist.Beta = Beta;
            double r = rand.NextDouble();
            double val = dist.InverseCDF(r);

            double sum = 0;
            for (horseToBuy = 0; horseToBuy < nCand; horseToBuy++)
            {
                sum += 1d / (double)nCand;
                if (val <= sum) break;
            }

            return horseToBuy;
        }

    }


    // ここから２０１６年

    //後にベータ関数をべきにおきかえるのでつかわなくなるだろう
    public class EconomicAllOtokuBetaBuyPlayer : EconomicOtokuBetaBuyPlayer
    {
        public EconomicAllOtokuBetaBuyPlayer(int id) : base(id) { }

        protected override bool CanBuy(int horseToBuy, List<double> odds)
        {
            return true; // can buy anytime
        }
    }

    public class EconomicAllOtokuBekiBuyPlayer : EconomicOtokuBuyPlayer
    {
        public EconomicAllOtokuBekiBuyPlayer(int id) : base(id) { }

        protected override bool CanBuy(int horseToBuy, List<double> odds)
        {
            return true; // can buy anytime
        }

       public static void SetSetExpectedVoteOdds(double[] eV, double[] eO)
       {
           EconomicPlayer.SetExpectedVoteOdds(eV, eO);
       }
        static EconomicAllOtokuBekiBuyPlayer()
        {
            SetFunc();
        }

        protected static double alpha = 0d;
        public static double Alpha // 0<= alpha で変化することで，Ecnomic（一様分布）からEconomicFirstをエミュレートできる
        {
            set
            {
                alpha = value;
                SetFunc();
            }
            get
            {
                return alpha;
            }
        }

        protected static double[] df;
        protected static double[] cdf;

        protected static void SetFunc()
        {
            if (14 != Race.nHorses)
            {
                System.Diagnostics.Debug.Fail("This class is supposed to be used under nHorses = 14");
            }

            df = new double[Race.nHorses];
            cdf = new double[Race.nHorses];

            double sum = 0d;
            for (int i = 0; i < Race.nHorses; i++)
            {
                double tmp = 1d - (double)i / 14d;
                df[i] = Math.Pow(tmp, alpha);
                sum += df[i];
            }
            double sum2 = 0d;
            for (int i = 0; i < Race.nHorses; i++)
            {
                df[i] = df[i] / sum;
                sum2 += df[i];
                cdf[i] = sum2;
            }
            TestDF(); // debug
        }

        [Conditional("DEBUG")]
        static protected void TestDF()
        {
            System.Diagnostics.Debug.WriteLine("TestDF");
            for (int i = 0; i < Race.nHorses; i++)
            {
                System.Diagnostics.Debug.WriteLine(i.ToString() + ", " + df[i].ToString() + ", "+cdf[i]);
            }
        }

        protected static double Func(int ninki)
        {
            return cdf[ninki];
        }
        protected override int GetHorseToBuy(int nCand)
        {
            if (nCand != Race.nHorses)
            {
                System.Diagnostics.Debug.Fail("This class is supposed to be used under nCand = nHorses");
            }
            //if (nCand <= 1) return 0;

            int horseToBuy = 0;
            double r = rand.NextDouble();
            for (horseToBuy = 0; horseToBuy < nCand; horseToBuy++)
            {
                if (r <= cdf[horseToBuy]) break;
            }

            return horseToBuy;
        }

    }


    /// <summary>
    /// 060218追加 initialMBRでちゃんと投票数の投票が想定勝率相似で済んでいる。
    /// </summary>
    public abstract class pHitStrictRace : pHitRace
    {
        static pHitStrictRace()
        {
            initialMBR = -20;
            MBRStep = 1;
            initialMBRVariance = 0;
            lastMBR = 0;            
        }
        //public static new int initialMBR = 0;//-20;
        //public static new int MBRStep = 1;
        //public static new int initialMBRVariance = 0;
        //public static new int lastMBR = 0;
        //mbrは-20くらいから-2までの10区間です． -20, -22, -24, -26, -28がChangeStartTimeで加わる >> 上記設定により、 -20,-19, ,,, , 0まで。
        protected override double GetBuyProb(int mbr, int id)
        {
            double idp = (double)id / (double)nPlayers;
            double investritsu = GetInvestRitsu(mbr);
            if (investritsu >= idp) return 1d;
            return 0d;
        }

        protected abstract double GetInvestRitsu(int mbr);

        protected override int[] GetPHitVote()
        {
            int[] nv = new int[nHorses];
            for (int i = 0; i < GetInvestRitsu(initialMBR) * nPlayers; i++)
            {
                double val = rand.NextDouble();
                int horseToBuy = 0;
                double sum = 0;
                for (horseToBuy = 0; horseToBuy < nHorses; horseToBuy++)
                {
                    sum += EconomicPlayer.expectedVote[horseToBuy]; // 160217
                    if (val <= sum) break;
                }
                if (horseToBuy == nHorses) horseToBuy = 0; // added 150605

                nv[horseToBuy]++;
            }
            return nv;
        }
    }

    /// <summary>
    /// 060218追加。購入関数が香港実測investのexp(t^3)に比例する。 + initialMBRでちゃんと投票数の投票が想定勝率相似で済んでいる。
    /// </summary>
    public class ExpT3Invest_pHitStrictRace : pHitStrictRace
    {
        protected override double GetInvestRitsu(int mbr)
        {
            //            logとらずフィット
            //            A	B	C
            //            0.393616816	3.287090348	23.20740297
            //            investritsu=A*exp( (mbr/C)^B )		
            //            logとってフィット
            //            A	B	C
            //          0.388343666	2.948029642	23.46624499
            //          investritsu=A*exp( (mbr/C)^B )		

            double shifted_mbr = (double)(mbr + 23);
            //            logとらずフィット
            double A = 0.393616816;
            double B = 3.287090348;
            double C = 23.20740297;
            double investritsu = A * Math.Exp(Math.Pow(shifted_mbr / C, B));
            return investritsu;
        }
        
    }

    /// <summary>
    /// 060222追加。購入関数にトイモデルを採用。exp(t^3)のもの。（いままではexp(t^3)といいつつexp(t^3.327)とかだった。
    /// f(t)=0.35*exp(1.3122776556233473*10^-4*t^3)    (t=0,1,2,…,20)
    /// </summary>
    public class ExpT3ToyInvest_pHitStrictRace : pHitStrictRace
    {
        protected override double GetInvestRitsu(int mbr)
        {
            double shifted_mbr = (double)(mbr + 20);
            double A = 0.35d;
            double B = 3d;
            double C = 1.3122776556233473d * 0.0001d;
            double investritsu = A * Math.Exp(Math.Pow(shifted_mbr, B)*C);
            return investritsu;
        }
    }

    /// <summary>
    /// 060222追加。購入関数にトイモデルを採用。exp(-t^3)のもの。（いままではexp(t^3)といいつつexp(t^3.327)とかだった。
    /// f(t)=1.35-0.35*exp(1.3122776556233473*10^-4*(20-t)^3)  (t=0,1,2,…,20)
    /// </summary>
    public class ExpTm3ToyInvest_pHitStrictRace : pHitStrictRace
    {
        protected override double GetInvestRitsu(int mbr)
        {
            double shifted_mbr = (double)(mbr + 20);
            double A = -0.35d;
            double B = 3d;
            double C = 1.3122776556233473d * 0.0001d;
            double D = 1.35d;
            double investritsu = D + A * Math.Exp(Math.Pow(20d-shifted_mbr, B) * C);
            return investritsu;
        }

    }

    /// <summary>
    /// 060222追加。購入関数にトイモデルを採用。一定ペース投票のもの。
    /// f(t)=0.0325*t+0.35    (t=0,1,2,…,20)
    /// </summary>
    public class ConstToyInvest_pHitStrictRace : pHitStrictRace
    {
        protected override double GetInvestRitsu(int mbr)
        {
            double shifted_mbr = (double)(mbr + 20);
            double investritsu = 0.0325d * shifted_mbr + 0.35d;
            return investritsu;
        }

    }

    /// <summary>
    /// 060222追加。検証用。最初の時刻にすべて投票、すなわち予定勝率と相似。
    /// </summary>
    public class FirstAllInvest_pHitStrictRace : pHitStrictRace
    {
        protected override double GetInvestRitsu(int mbr)
        {
            return 1d;
        }

    }

    /// <summary>
    /// 060222追加。購入関数にトイモデルを採用。最初に０．３５かけて、あとは最終時刻にすべて。
    /// </summary>
    public class LastAllInvest_pHitStrictRace : pHitStrictRace
    {
        protected double firstInvestRatio = 0.35d;
        public LastAllInvest_pHitStrictRace()
        {
            firstInvestRatio = 0.35d;
        }
        public LastAllInvest_pHitStrictRace(double firstInvestRatio)
        {
            this.firstInvestRatio = firstInvestRatio;
        }
        protected override double GetInvestRitsu(int mbr)
        {
            double ret = firstInvestRatio;// 0.35d;
            if (mbr == lastMBR)
            {
                ret = 1d;
            }
            return ret;

        }

    }

    /// <summary>
    /// 060311追加。20分点ですべての値を別枠で与える。
    /// </summary>
    public class AnyInvest_pHitStrictRace : pHitStrictRace
    {
        protected static double[] investRatio = {
                                                    1
                                                };
        public static double[] InvestRatio
        {
            set
            {
                investRatio = value;
            }
        }

        protected override double GetInvestRitsu(int mbr)
        {
            int shifted_mbr = mbr + 20;
            return investRatio[shifted_mbr];
        }

    }

}
