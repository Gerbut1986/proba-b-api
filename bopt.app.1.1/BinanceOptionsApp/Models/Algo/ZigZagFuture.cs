namespace Models.Algo
{
    using System;
    using Helpers;
    using System.Linq;
    using Helpers.Extensions;
    using System.Collections.Generic;
    using MultiTerminal.Connections.API.Future;
    using MultiTerminal.Connections;
    using System.Collections.Concurrent; 
    using BinanceOptionsApp; 
    using System.Threading.Tasks;

    public class Plot
    {
        public decimal MarketBuy { get; set; } = 0;
        public decimal MarketSell { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }
    public class Zig_Zag
    {
        public string Tip { get; set; } = "";
        public int WaweCount { get; set; } = 0;
        public decimal StartPrice { get; set; } = 0;
        public decimal FinishPrice { get; set; } = 0;
        public long StartTime { get; set; } = 0;
        public long FinishTime { get; set; } = 0;
        public ulong ID_Q_Start { get; set; } = 0;
        public ulong ID_Q_Finish { get; set; } = 0;
        public decimal MaxBuyVol { get; set; } = 0;
        public decimal PriceMaxBuy { get; set; } = 0;
        public decimal MaxSellVol { get; set; } = 0;
        public decimal PriceMaxSell { get; set; } = 0;
        public decimal AvgBuy { get; set; } = 0;
        public decimal AvgSell  { get; set; } = 0;
    }
 

    public class ZigZagFuture
    {
        #region Fields:
           
        //private static decimal Leg = 25; //100*100*10
        //private static readonly decimal SenDist = 200;
        private static readonly decimal ThresholdVol = 1;

        private static decimal MaxASK_Vol = 0;
        private static decimal MinBID_Vol = 0;
        private static decimal MaxASK = 0;
        private static decimal MinBID = 0;
        private static ulong ID_Q_MaxAsk = 0;
        private static ulong ID_Q_MinBid = 0;
        private static ulong TimeMaxASK;
        private static ulong TimeMinBID;
        public static int WaveCounter = 0;
        private static bool WaveDN = false;
        private static bool WaveUP = false;
        //--------------------------------------------+
        private decimal priceStep = 0.01m;
        private decimal StartPrice;
        private decimal FinishPrice;
        private ulong StartTime { get; set; }
        private ulong FinishTime;
        static ulong LastID_Q_MinBid = 0;
        static ulong LastID_Q_MaxAsk = 0;
        static ulong Pre_id = 0;
        string CL = "";
        int Kin = 0;
        static bool Triger = false;
        public static decimal SumAsk = 0, SumBid = 0;
        public static decimal SupportPrice = 0, ResistancePrice = 0;
        public static decimal SupportVol = 0, ResistanceVol = 0;
        public static int MainRankSell = 0, MainRankBuy = 0;
        private static decimal RP = 0, SP = 0, RV = 0, SV = 0;
        private object lockObject = new object();
        private DateTime KinTime;
        //System.DateTime TimeNow;
        private DateTime currentTime;// = System.DateTime.Now;
        private ConcurrentDictionary<decimal, MarketDepthUpdateFuture> MaxAsklistMD = default, MinBidlistMD = default, MDZigDN = default, MDZigUP = default;
        private List<List<string>> MaxAsklistMD_P = default, MinBidlistMD_P = default, MDZigDN_P = default, MDZigUP_P = default;
        
        // Створюємо колекцію моделей MarketData
        List<MarketMaxLevel> MaxDataList = new List<MarketMaxLevel>();
        List<MarketMaxLevel> MaxCurrLegList = new List<MarketMaxLevel>();
        List<MarketMaxLevel> MaxKinList = new List<MarketMaxLevel>();
        decimal MaxLevelBuy = 0, MaxLevelSell = 0, PriceMaxLevelBuy = 0, PriceMaxLevelSell = 0;

        class MarketMaxLevel
        {
            public double MaxMarketBuy { get; set; }
            public double MaxMarketSell { get; set; }
        }
        static void AddMarketLevel(List<MarketMaxLevel> MaxDataList, double maxMarketBuy, double maxMarketSell)
        {
            var marketData = new MarketMaxLevel
            {
                MaxMarketBuy = maxMarketBuy,
                MaxMarketSell = maxMarketSell
            };

            MaxDataList.Add(marketData);
        }

        static double CalculateAverage(List<MarketMaxLevel> dataList, Func<MarketMaxLevel, double> selector)
        {
            int lastThreeCount = Math.Min(dataList.Count, 3);

            if (lastThreeCount == 0)
                return 0;

            double sum = 0;

            for (int i = dataList.Count - lastThreeCount; i < dataList.Count; i++)
            {
                double selectedValue = selector(dataList[i]);
                sum += selectedValue;
            }
            double average = sum / lastThreeCount;
            return average;
        }

        #endregion

        #region Arrays: ZigZagArr, CurrLeg, Kinchik[]:
        public readonly Plot[] ZigZagArr; //100*100*10*3 (for Leg 3000$; step=0.01)
        public static Plot[] CurrLeg = new Plot[300000];
        public static Plot[] Kinchik = new Plot[100000];
        #endregion

        public ZigZagFuture()
        {
            ZigZagArr = new Plot[300000];
            InitZigZagArr();
        }

        #region BuildZigZag func:
        
        public void BuildZigZag(decimal ask, decimal bid, ulong Time, ulong ID_quark, List<AggTradeFuture> tasLst)
        {
            if (MaxASK == 0 || ask > MaxASK)  //  
            {
                MaxASK = ask;
                TimeMaxASK = Time;
                ID_Q_MaxAsk = ID_quark;
                MaxASK_Vol = SumAsk;
                //MainRankSell = BinanceExecution.FunDepthProba2(1);
                RP = ResistancePrice;
                RV = ResistanceVol;
                MaxAsklistMD = BinanceFutureClient.marketDepthList;
                MaxAsklistMD_P = BinanceFutureClient.AsksP;
                var bookTicker = BinanceFutureClient.BookTickerFuture;
                //bookTicker[0].Data.AskQuantity
            }  //new MAX

            if (MinBID == 0 || bid < MinBID)  //  
            {
                MinBID = bid;
                TimeMinBID = Time;
                ID_Q_MinBid = ID_quark;
                MinBID_Vol = SumBid;
                //MainRankBuy = BinanceExecution.FunDepthProba2(2);
                SP = SupportPrice;
                SV = SupportVol;
                MinBidlistMD = BinanceFutureClient.marketDepthList;
            }  //new MIN

            if (TimeMaxASK > TimeMinBID) // UP zig zag
            {
                if (MaxASK - MinBID >= TradeZigZag.Leg && MaxASK - bid >= TradeZigZag.Leg) // UP zig zag
                {
                    WaveUP = true; WaveDN = false;
                    WaveCounter++;
                    StartPrice = MinBID;
                    StartTime = TimeMinBID;
                    FinishPrice = MaxASK;
                    FinishTime = TimeMaxASK;

                    MDZigUP = MaxAsklistMD;
                    ZigZagPlot.Clear();
                    CurrLegPlot.Clear();
                    //InitZigZagArr();
                    //InitCurrLeg();//nulling CurrLeg
                    preID = 0;
                    FillingSmallZZ(ID_Q_MinBid, ID_Q_MaxAsk, StartTime, FinishTime, StartPrice, FinishPrice, "UP", tasLst);
                    MinBID = bid;
                    TimeMinBID = Time;
                    ID_Q_MinBid = ID_quark;
                    MinBID_Vol = SumBid;
                    //MainRankBuy = BinanceExecution.FunDepthProba2(2);
                    SP = SupportPrice;
                    SV = SupportVol;
                }
            }   // UP Zig Zag

            if (WaveUP == true && TimeMaxASK < Time) //for DN cur leg
            {
                if (LastID_Q_MinBid == 0) LastID_Q_MinBid = ID_Q_MinBid;

                if (ID_quark > Pre_id)
                {
                    if (ID_Q_MinBid > LastID_Q_MinBid)
                    {
                      
                        lock (lockObject)
                        {
                            FillingCurrentLeg(ID_Q_MaxAsk, ID_quark, MaxASK, "dn", tasLst);
                        }
                                                
                        CL = GazerCurrLeg("dn");
                        string zzz = CheckZZ();

                        if (CL != "0")
                        {
                            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)Time);
                            string formattedTime = dateTimeOffset.ToString("HH:mm:ss.fff");
                            string symb = TradeZigZag.Symbol; //
                            string FileName = "SpikeFuture" + symb;
                            // --------------
                            
                            if (MaxLevelBuy > MaxLevelSell && ask <= PriceMaxLevelBuy)
                            {
                                if (CL.Contains("Ord=2") && !CL.Contains("shuxerCloseBuy"))
                                {
                                    new _().WriteToTxtFile($"Time: {formattedTime} | Order = 2 | CL: {CL} | Ask: {ask}, " +
                                    $"Bid: {bid}", FileName);
                                    TradeZigZag.Order = 2;
                                }
                            }
                            //----------------------------------------
                            
                            if (CL.Contains("Ord=22"))
                            {
                                new _().WriteToTxtFile($"Time: {formattedTime} | Order = 22 | CL: {CL} | Ask: {ask}, " +
                                $"Bid: {bid}", FileName);
                                TradeZigZag.Order = 22;
                            }
                            else if (CL.Contains("shuxerCloseBuy"))
                            {
                                new _().WriteToTxtFile($"Time: {formattedTime} | Shuxer Close Buy, | CL: {CL} | Ask: {ask}, " +
                                $"Bid: {bid}", FileName);
                                TradeZigZag.Order = 111;
                            }
                            else if (CL.Contains("shuxerCloseSell"))
                            {
                                new _().WriteToTxtFile($"Time: {formattedTime} | Shuxer Close Sell, | CL: {CL} | Ask: {ask}, " +
                                $"Bid: {bid}", FileName);
                                TradeZigZag.Order = 222;
                            }
                            else if (zzz.Contains("ImpulseDN") || zzz.Contains("KlinDN"))
                            {
                                new _().WriteToTxtFile($"Time: {formattedTime} | Order = 22 | zzz: {zzz} | Ask: {ask}, " +
                                $"Bid: {bid}", FileName);
                                TradeZigZag.Order = 22;
                            }

                        }
                       // InitKinchik();//nulling
                       // Knut(ID_Q_MinBid, ID_quark, MinBID, "up", tasLst);
                        //res=CL;
                        Triger = true;
                        LastID_Q_MinBid = ID_Q_MinBid;
                         
                    }
                    else
                    {
                        try
                        {
                            if (ID_quark > ID_Q_MinBid)
                            {
                              //  InitKinchik();//nulling 
                              //  Knut(ID_Q_MinBid, ID_quark, MinBID, "up", tasLst);
                              //  Kin = GazerKinchik("up");
                            }

                            if (Kin == 2 && Triger)
                            {
                                long T = (long)Time;
                                KinTime = DateTimeOffset.FromUnixTimeMilliseconds(T).DateTime;//(long)
                                string formattedTime = KinTime.ToString("HH:mm:ss.fff");
                                currentTime = DateTime.Now;
                                string formattedTimeN = currentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                string KinMsg = $"Time= {formattedTime} Kin= {Kin}  Ask= {ask}  Bid= {bid} curr time: {formattedTimeN}";
                                //string KinMsg = $"Time= {Time}  Kin= {Kin}  Ask= {ask},  Bid= {bid}";
                                new _().WriteToTxtFile(KinMsg, "Kinchik");
                                Triger = false;
                                MakeAggMD("Kin", "UP", MinBID + TradeZigZag.SenseDist, MinBID - TradeZigZag.SenseDist);
                                var P1 = Look4MaxVPrice(1);
                                var P2 = Look4MaxVPrice(2);
                            }
                        }
                        catch (Exception ex)
                        {
                            new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(BuildZigZag)}[ERROR]", true, true);
                        }
                    }
                    Pre_id = ID_quark;
                }
            }  // Curr Leg dn

            if (TimeMaxASK < TimeMinBID) // DN zig zag
            {
                if (MaxASK - MinBID >= TradeZigZag.Leg && ask - MinBID >= TradeZigZag.Leg) // DN zig zag
                {
                    WaveDN = true;
                    WaveUP = false;
                    WaveCounter++;
                    StartPrice = MaxASK;
                    StartTime = TimeMaxASK;
                    FinishPrice = MinBID;
                    FinishTime = TimeMinBID;

                    MDZigDN = MinBidlistMD;
                    ZigZagPlot.Clear();
                    CurrLegPlot.Clear();
                    //InitZigZagArr();
                    //InitCurrLeg();//nulling CurrLeg
                    preID = 0;
                    FillingSmallZZ(ID_Q_MaxAsk, ID_Q_MinBid, StartTime, FinishTime, StartPrice, FinishPrice, "DN", tasLst);
                    MaxASK = ask;
                    TimeMaxASK = Time;
                    ID_Q_MaxAsk = ID_quark;
                    MaxASK_Vol = SumAsk;
                    //MainRankSell = BinanceExecution.FunDepthProba2(1);
                    RP = ResistancePrice;
                    RV = ResistanceVol;
                }
            } // DN Zig Zag

            if (WaveDN == true && TimeMinBID < Time)//for UP cur leg
            {
                if (LastID_Q_MaxAsk == 0) { LastID_Q_MaxAsk = ID_Q_MaxAsk; }

                if (ID_quark > Pre_id)
                {
                    if (ID_Q_MaxAsk > LastID_Q_MaxAsk)
                    {
                        //InitCurrLeg();//nulling 
                        //  int z = 0;
                        //for (ulong i = ID_Q_MinBid; i <= ID_Q_MaxAsk; i++)
                        lock (lockObject)
                        {
                            FillingCurrentLeg(ID_Q_MinBid, ID_quark, MinBID, "up", tasLst);
                        }
                  
                        CL = GazerCurrLeg("up");//
                        string zzz = CheckZZ();

                        if (CL != "0")
                        {
                          DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)Time);
                          string formattedTime = dateTimeOffset.ToString("HH:mm:ss.fff");

                            string symb = TradeZigZag.Symbol; //
                            string FileName = "SpikeFuture" + symb;
                            // --------------

                            if (MaxLevelBuy < MaxLevelSell && bid >= PriceMaxLevelSell)
                            {
                                if (CL.Contains("Ord=1") && !CL.Contains("shuxerCloseSell"))
                                {
                                    new _().WriteToTxtFile($"Time: {formattedTime} | Order = 1 | CL: {CL} | Ask: {ask}, " +
                                    $"Bid: {bid}", FileName);
                                    TradeZigZag.Order = 1;
                                }
                            }
                            //----------------------------------------

                            if (CL.Contains("Ord=11"))
                            {
                                new _().WriteToTxtFile($"Time: {formattedTime} | Order = 11 | CL: {CL} | Ask: {ask}, " +
                                $"Bid: {bid}", FileName);
                                TradeZigZag.Order = 11;
                            }
                            else if (CL.Contains("shuxerCloseBuy"))
                            {
                                new _().WriteToTxtFile($"Time: {formattedTime} | Shuxer Close Buy, | CL: {CL} | Ask: {ask}, " +
                                $"Bid: {bid}", FileName);
                                TradeZigZag.Order = 111;
                            }
                            else if (CL.Contains("shuxerCloseSell"))
                            {
                                new _().WriteToTxtFile($"Time: {formattedTime} | Shuxer Close Sell, | CL: {CL} | Ask: {ask}, " +
                                $"Bid: {bid}", FileName);
                                TradeZigZag.Order = 222;
                            }
                            else if(zzz.Contains("ImpulseUP") || zzz.Contains("KlinUP"))
                            {
                                new _().WriteToTxtFile($"Time: {formattedTime} | Order = 11 | zzz: {zzz} | Ask: {ask}, " +
                                $"Bid: {bid}", FileName);
                                TradeZigZag.Order = 11;
                            }

                        }

                        // InitKinchik();//nulling 
                        // Knut(ID_Q_MaxAsk, ID_quark, MaxASK, "dn", tasLst);
                        Triger = true;
                        LastID_Q_MaxAsk = ID_Q_MaxAsk;
                    }
                    else
                    {

                        try
                        {
                            if (ID_quark > ID_Q_MaxAsk)
                            {
                               // InitKinchik();//nulling 
                                //Knut(ID_Q_MaxAsk, ID_quark, MaxASK, "dn", tasLst); // kinchik dn
                              //  Kin = GazerKinchik("dn");
                            }

                            if (Kin == 1 && Triger)
                            {
                                long T = (long)Time;
                                KinTime = DateTimeOffset.FromUnixTimeMilliseconds(T).DateTime;//(long)
                                currentTime = DateTime.Now;
                                string formattedTimeN = currentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                string formattedTime = KinTime.ToString("HH:mm:ss.fff");
                                string KinMsg = $"Time= {formattedTime} Kin= {Kin}  Ask= {ask}  Bid= {bid} curr time: {formattedTimeN}";

                                //string KinMsg = $"Time= {formattedTime} Kin= {Kin}  Ask= {ask},  Bid= {bid} ";
                                new _().WriteToTxtFile(KinMsg, "Kinchik");
                                Triger = false;
                                MakeAggMD("Kin", "DN", MaxASK + TradeZigZag.SenseDist, MaxASK - TradeZigZag.SenseDist);
                                var P1 = Look4MaxVPrice(1);
                                var P2 = Look4MaxVPrice(2);
                            }
                        }
                        catch (Exception ex)
                        {
                            new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(BuildZigZag)}[ERROR]", true, true);
                        }
                    }
                }
            }     // Curr Leg up
        }
        #endregion
        string CheckZZ()
        {
            string res = "0";
            if (ZZF.Count - 4 >= 0)
            {
                // 5 wave - CurrLeg
                decimal eW4 = ZZF[ZZF.Count - 1].FinishPrice; // Last wave 4
                decimal eW3 = ZZF[ZZF.Count - 2].FinishPrice; //  wave 3
                decimal eW2 = ZZF[ZZF.Count - 3].FinishPrice; //  wave 2
                decimal eW1 = ZZF[ZZF.Count - 4].FinishPrice; //  wave 1
                decimal eW0 = ZZF[ZZF.Count - 4].StartPrice;

                
                if (ZZF[ZZF.Count - 1].Tip == "DN" && ZZF[ZZF.Count - 2].Tip == "UP" && ZZF[ZZF.Count - 3].Tip == "DN"
                     && ZZF[ZZF.Count - 4].Tip == "UP")
                {
                    if (eW2 > eW0 && eW3 > eW1 && eW4 > eW1) res = "ImpulseUP";
                    else if (eW2 > eW0 && eW3 > eW1 && eW4 < eW1 && eW4 > eW2) res = "KlinUP";
                } // Wave UP

                
                else if (ZZF[ZZF.Count - 1].Tip == "UP" && ZZF[ZZF.Count - 2].Tip == "DN" && ZZF[ZZF.Count - 3].Tip == "UP" 
                             && ZZF[ZZF.Count - 4].Tip == "DN")
                {
                    if (eW2 < eW0 && eW3 < eW1 && eW4 < eW1) res = "ImpulseDN";
                    if (eW2 < eW0 && eW3 < eW1 && eW4 > eW1 && eW4 < eW2) res = "KlinDN";

                } // Wave DN
            }

             return res;
        }

        #region FillingSmallZZ func:

        public static Dictionary<decimal, Plot> ZigZagPlot = new Dictionary<decimal, Plot>();
        public List<Zig_Zag> ZZF = new List<Zig_Zag>();

      //  Zig_Zag Z_Z = new Zig_Zag();

        private void FillingSmallZZ(ulong ID_Q_Start, ulong ID_Q_Finish, 
                                     ulong StartTime, ulong FinishTime, decimal StartPr, decimal FinishPr, string tip, List<AggTradeFuture> tasLst)
        {
            if ((tip == "UP" && ID_Q_Finish > ID_Q_Start && ID_Q_Finish < (ulong)tasLst.Count && ID_Q_Start > 0) ||
               (tip == "DN" && ID_Q_Finish > ID_Q_Start && ID_Q_Finish < (ulong)tasLst.Count && ID_Q_Start > 0))
            {
                List<AggTradeFuture> subList = default; 
                
                try
                {
                    var length = (int)(ID_Q_Finish - ID_Q_Start);
                    subList = tasLst.GetRange((int)ID_Q_Start - 1, length);
                }
                catch (Exception ex) { new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", "FillingSmallZZ{tasLst.GetRange()}[ERROR]", true, true); }

                if (subList.Count != 0)
                {
                    int i = 0;
                    foreach (var item in subList)
                    {
                        try
                        {
                            var LastPrice = item.data.Price;
                            var Vol = item.data.Volume;
                            decimal x;
 
                            //==========================================
                            if (item != null)
                            {
                                decimal p = item.data.Price;
                                decimal marketBuy = item.data.MarketBuy;
                                decimal marketSell = item.data.MarketSell;
                                decimal key = p;

                                if (ZigZagPlot.TryGetValue(key, out var plot))
                                {
                                    plot.MarketBuy += marketBuy;
                                    plot.MarketSell += marketSell;
                                }
                                else
                                {
                                    ZigZagPlot.Add(key, new Plot { MarketBuy = marketBuy, MarketSell = marketSell, Price = key });
                                }
                            }
                            //==========================================
 
                        }
                        catch (Exception ex)
                        {
                            new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(FillingSmallZZ)}[ERROR]", true, true);
                        }
                    }

                    //----------------- шукаю мах level ZigZag -----------------------
  
                    // Знаходження максимального значення MaxMarketBuy та відповідної ціни PriceMaxBuy
                    var maxBuyEntry = ZigZagPlot.OrderByDescending(kv => kv.Value.MarketBuy).FirstOrDefault();
                    decimal MaxMarketBuy = maxBuyEntry.Value.MarketBuy;
                    decimal PriceMaxBuy = maxBuyEntry.Key;

                    // Знаходження максимального значення MaxMarketSell та відповідної ціни PriceMaxSell
                    var maxSellEntry = ZigZagPlot.OrderByDescending(kv => kv.Value.MarketSell).FirstOrDefault();
                    decimal MaxMarketSell = maxSellEntry.Value.MarketSell;
                    decimal PriceMaxSell = maxSellEntry.Key;

                    AddMarketLevel(MaxDataList, (double)MaxMarketBuy, (double)MaxMarketSell);
                    var avrgBuy = CalculateAverage(MaxDataList, data => data.MaxMarketBuy);
                    var avrgSell = CalculateAverage(MaxDataList, data => data.MaxMarketSell);

                    if (MaxMarketBuy > MaxLevelBuy) { MaxLevelBuy = MaxMarketBuy; PriceMaxLevelBuy = PriceMaxBuy; }
                    if (MaxMarketSell > MaxLevelSell) { MaxLevelSell = MaxMarketSell; PriceMaxLevelSell = PriceMaxSell; }


                    // ---------------------$$$$$$$-------------------------------------
                    Zig_Zag Z_Z = new Zig_Zag();

                    Z_Z.Tip = tip;
                    Z_Z.WaweCount = WaveCounter;
                    Z_Z.ID_Q_Start = ID_Q_Start;
                    Z_Z.ID_Q_Finish = ID_Q_Finish;
                    Z_Z.StartPrice = StartPr;
                    Z_Z.FinishPrice = FinishPr;
                    Z_Z.StartTime = (long)StartTime;
                    Z_Z.FinishTime = (long)FinishTime;
                    Z_Z.PriceMaxBuy = PriceMaxBuy;
                    Z_Z.MaxBuyVol = MaxMarketBuy;
                    Z_Z.PriceMaxSell = PriceMaxSell;
                    Z_Z.MaxSellVol = MaxMarketSell;
                    Z_Z.AvgBuy = (decimal)avrgBuy;
                    Z_Z.AvgSell = (decimal)avrgSell;

                    ZZF.Add(new Zig_Zag
                    {
                        Tip = Z_Z.Tip,
                        WaweCount = Z_Z.WaweCount,
                        ID_Q_Start = Z_Z.ID_Q_Start,
                        ID_Q_Finish = Z_Z.ID_Q_Finish,
                        StartPrice = Z_Z.StartPrice,
                        FinishPrice = Z_Z.FinishPrice,
                        StartTime = Z_Z.StartTime,
                        FinishTime = Z_Z.FinishTime,
                        PriceMaxBuy = Z_Z.PriceMaxBuy,
                        MaxBuyVol = Z_Z.MaxBuyVol,
                        PriceMaxSell = Z_Z.PriceMaxSell,
                        MaxSellVol = Z_Z.MaxSellVol,
                        AvgBuy = Z_Z.AvgBuy,
                        AvgSell = Z_Z.AvgSell
                    });

                    //-----------------------$$$$$$$-------------------------------------
                    //-------------------------------------------------------------------
                    string symb = TradeZigZag.Symbol;
                    string FileName = "MDLevel2Future" + symb;
                    string logMsg = null;
                    if (tip == "UP") logMsg = $"** UP ****************************************************";
                    if (tip == "DN") logMsg = $"** DN ****************************************************";
                    new _().WriteToTxtFile(logMsg, FileName);
                    new _().WriteToTxtFile($"WaveCounter: {WaveCounter}".ToString(), FileName);
                    var st = long.Parse(StartTime.ToString()).GetFullTime();
                    var ft = long.Parse(FinishTime.ToString()).GetFullTime();
                    logMsg = $"StartTime = {st.Hour}:{st.Minute}:{st.Second}.{st.Millisecond} FinishTime = {ft.Hour}:{ft.Minute}:{ft.Second}.{ft.Millisecond}";
                    new _().WriteToTxtFile(logMsg, FileName);
                    logMsg = $"StartPrice = {StartPrice} FinishPrice = {FinishPrice}";
                    new _().WriteToTxtFile(logMsg, FileName);

                    var startRange = FinishPrice + TradeZigZag.SenseDist;
                    var finishRange = FinishPrice - TradeZigZag.SenseDist;

                    //!!! Stakan print full data

                    if (tip == "UP")
                    {
                        FileName = "MDLevel2Future" + symb;
                        var filteredItems = MDZigUP
                         .Where(pair => pair.Key <= startRange && pair.Key >= finishRange)
                         .ToList();

                        var type1Items = filteredItems
                            .Where(item => item.Value.Type == 1)
                            .OrderByDescending(item => item.Key)
                            .ToList();

                        foreach (var item in type1Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", FileName);
                        }
                        new _().WriteToTxtFile($" - Ask UP Bid - @", FileName);


                        var type2Items = filteredItems
                              .Where(item => item.Value.Type == 2)
                              .OrderByDescending(item => item.Key)
                              .ToList();

                        foreach (var item in type2Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", FileName);
                        }
                    }
                    if (tip == "DN")
                    {
                        FileName = "MDLevel2Future" + symb;
                        var filteredItems = MDZigDN
                     .Where(pair => pair.Key <= startRange && pair.Key >= finishRange)
                         .ToList();

                        var type1Items = filteredItems
                            .Where(item => item.Value.Type == 1)
                            .OrderByDescending(item => item.Key)
                            .ToList();

                        foreach (var item in type1Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", FileName);
                        }
                        new _().WriteToTxtFile($" - Ask DN Bid - @", FileName);
                        var type2Items = filteredItems
                            .Where(item => item.Value.Type == 2)
                            .OrderByDescending(item => item.Key)
                            .ToList();
                        foreach (var item in type2Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", FileName);
                        }
                    }

                    //!!!
                    //!!! Stakan print agregate data
                    MakeAggMD("ZigZag", tip, startRange, finishRange);
                    //!!! Stakan print finish

                    FileName = "SmallZZFuture" + symb;
                    logMsg = "";
                    if (tip == "UP") logMsg = $"** UP *** {WaveCounter} *********************************************";
                    if (tip == "DN") logMsg = $"** DN *** {WaveCounter} *********************************************";
                    new _().WriteToTxtFile(logMsg, FileName);
                    new _().WriteToTxtFile($"WaveCounter: {WaveCounter}".ToString(), FileName);
                    st = long.Parse(StartTime.ToString()).GetFullTime();
                    ft = long.Parse(FinishTime.ToString()).GetFullTime();
                    logMsg = $"StartTime = {st.Hour}:{st.Minute}:{st.Second}.{st.Millisecond} FinishTime = {ft.Hour}:{ft.Minute}:{ft.Second}.{ft.Millisecond}";
                    new _().WriteToTxtFile(logMsg, FileName);
                    logMsg = $"StartPrice = {StartPrice} FinishPrice = {FinishPrice}";
                    new _().WriteToTxtFile(logMsg, FileName);

                    // --- print Max Level ------

                    var logMsgBuilderM = new System.Text.StringBuilder();
                    logMsgBuilderM.AppendLine($"PriceMaxMarketBuy = {PriceMaxBuy} Vol = {MaxMarketBuy}");
                    logMsgBuilderM.AppendLine($"PriceMaxMarketSell = {PriceMaxSell} Vol = {MaxMarketSell}");
                    logMsgBuilderM.AppendLine($"avrgMaxBuy = {avrgBuy} ");
                    logMsgBuilderM.AppendLine($"avrgMaxSell = {avrgSell} ");
                    logMsgBuilderM.AppendLine($"PriceMaxLevelBuy = {PriceMaxLevelBuy} MaxLevelBuy = {MaxLevelBuy} ");
                    logMsgBuilderM.AppendLine($"PriceMaxLevelSell = {PriceMaxLevelSell} MaxLevelSell = {MaxLevelSell} ");

                    logMsg = logMsgBuilderM.ToString();
                    new _().WriteToTxtFile(logMsg, FileName);
                    // -----------------------------------------------------

                    var a = tasLst[(int)ID_Q_Start - 1].data.Ask;
                    var b = tasLst[(int)ID_Q_Start - 1].data.Bid;
                    var a2 = tasLst[(int)ID_Q_Finish - 1].data.Ask;
                    var b2 = tasLst[(int)ID_Q_Finish - 1].data.Bid;

                    logMsg = $"ID_Q_Start: {ID_Q_Start} | ID_Q_Finish: {ID_Q_Finish}, AskS = {a} BidS = {b} AskF = {a2} BidF = {b2}";
                    new _().WriteToTxtFile(logMsg, FileName);

                    //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                    var selectedItems = ZigZagPlot.Where(kv => kv.Value.Price > 0).ToList();

                    if (tip == "UP")
                    {
                        var selectedItemsSorted = selectedItems.OrderBy(kv => kv.Value.Price).ToList();
                        selectedItems = selectedItemsSorted;
                    }
                    else //if (tip == "DN")
                    {
                        var selectedItemsSortedDescending = selectedItems.OrderByDescending(kv => kv.Value.Price).ToList();
                        selectedItems = selectedItemsSortedDescending;
                    }

                    foreach (var kvp in selectedItems)
                    {
                        var logMsgBuilder = new System.Text.StringBuilder();
                        logMsgBuilder.AppendLine($"Price= {kvp.Value.Price}");
                        logMsgBuilder.AppendLine($"MarketBuy = {kvp.Value.MarketBuy}");
                        logMsgBuilder.AppendLine($"MarketSell = {kvp.Value.MarketSell}");

                        logMsg = logMsgBuilder.ToString();
                        new _().WriteToTxtFile(logMsg, FileName);
                    }

                }
            }
            else
            {
                new _().WriteToTxtFile("ID_Q_MaxAsk > ID_Q_MinBid || ID_Q_MinBid > found.Length",
                     "FillingSmallZZ[errors]", true, true);
                string logMsgEr = $"Pomilka ID_Q_Start= {ID_Q_Start}   ID_Q_Finish= {ID_Q_Finish}  Bin= {tasLst.Count}";
                new _().WriteToTxtFile(logMsgEr, "Pomilka");
            }
        }
        #endregion

        #region [FillingCurrentLeg]:
        int cnt = 0; int preID = 0;
        public static Dictionary<decimal, Plot> CurrLegPlot = new Dictionary<decimal, Plot>();
        private void FillingCurrentLeg(ulong ID_Extremum, ulong id, decimal StartPr, string tip, List<AggTradeFuture> tasLst)
        {

            if ((tip == "up" && id >= ID_Extremum && id <= (ulong)tasLst.Count && ID_Extremum > 0) ||
               (tip == "dn" && id >= ID_Extremum && id <= (ulong)tasLst.Count && ID_Extremum > 0))
            {
                lock(lockObject)
                {
                    ++cnt;
                    if(preID == 0) { preID = (int)ID_Extremum; }

                    var length = (int)id - preID;
                    var subList1 = (tasLst.GetRange(preID - 1, length));
                    //var subList1 = BinanceExecution.list.Skip((int)(ID_Extremum - 1)).Take((int)(id - ID_Extremum + 1)).ToArray();
                    if (subList1.Count != 0)
                    {
                        int i = 0; int qq = 0;
                        preID = (int)id;
                        try
                        {
                            foreach (var item in subList1)
                            {
                                qq++;
                              //  var LastPrice = item.data.Price;
                              //  var Vol = item.data.Volume;
                              //  bool BuyerMaker = item.data.IsMarketMaker;
                                /*
                                decimal x;
                                if (tip == "up")
                                {
                                    x = (LastPrice - StartPr) / priceStep;
                                    i = (int)x;
                                }
                                if (tip == "dn")
                                {
                                    x = (StartPr - LastPrice) / priceStep;
                                    i = (int)x;
                                }

                                if (CurrLeg[i] == null) CurrLeg[i] = new Plot();

                                CurrLeg[i].Price = item.data.Price;
                                CurrLeg[i].MarketBuy += item.data.MarketBuy;
                                CurrLeg[i].MarketSell += item.data.MarketSell;
                                */
                                //==========================================
                                if (item != null)
                                {
                                    decimal p = item.data.Price;
                                    decimal marketBuy = item.data.MarketBuy;
                                    decimal marketSell = item.data.MarketSell;
                                    decimal key = p;

                                    if (CurrLegPlot.TryGetValue(key, out var plot))
                                    {
                                        plot.MarketBuy += marketBuy;
                                        plot.MarketSell += marketSell;
                                    }
                                    else
                                    {
                                        CurrLegPlot.Add(key, new Plot { MarketBuy = marketBuy, MarketSell = marketSell, Price = key });
                                    }
                                }
                                //==========================================


                            }

                            var zzz = 0;
                           
                        }
                        catch (Exception ex)
                        {
                            new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(FillingCurrentLeg)}[ERROR]", true, true);
                        }
                    }
                }
            }
            else
            {
                string logMsgEr = $"Pomilka ID_Q_Start= {ID_Extremum}   ID_Q_Finish= {id}  Bin= {tasLst.Count}";
                new _().WriteToTxtFile(logMsgEr, "PomilkaCurrLeg");
            }

        }
        #endregion

        #region [GazerCurrLeg]:

        private string GazerCurrLeg(string tip)
        {
            
            //decimal SumBuy = 0; decimal SumSell = 0;
            string res = "0", ser = "0", ers = "0", rse = "0";

            //----------------- шукаю мах level CurrLeg -----------------------
            // Знаходження максимального значення MaxMarketBuy та відповідної ціни PriceMaxBuy
            var maxBuyEntry = CurrLegPlot.OrderByDescending(kv => kv.Value.MarketBuy).FirstOrDefault();
            decimal MaxCurrBuy = maxBuyEntry.Value.MarketBuy;
            decimal PriceMaxCurrBuy = maxBuyEntry.Key;

            // Знаходження максимального значення MaxMarketSell та відповідної ціни PriceMaxSell
            var maxSellEntry = CurrLegPlot.OrderByDescending(kv => kv.Value.MarketSell).FirstOrDefault();
            decimal MaxCurrSell = maxSellEntry.Value.MarketSell;
            decimal PriceMaxCurrSell = maxSellEntry.Key;

            bool shuxerCloseBuy_OpenSell = false, shuxerCloseSell_OpenBuy = false;
            if(MaxLevelSell > MaxLevelBuy && MaxLevelSell < MaxCurrBuy ) { shuxerCloseSell_OpenBuy = true; shuxerCloseBuy_OpenSell = false; }
            if(MaxLevelSell < MaxLevelBuy && MaxLevelBuy < MaxCurrSell) { shuxerCloseBuy_OpenSell = true; shuxerCloseSell_OpenBuy = false; }

            if(MaxCurrBuy > MaxCurrSell && MaxCurrBuy > MaxLevelSell && MaxCurrBuy > MaxLevelBuy) 
            { shuxerCloseSell_OpenBuy = true; shuxerCloseBuy_OpenSell = false; }

            if (MaxCurrBuy < MaxCurrSell && MaxCurrSell > MaxLevelBuy && MaxCurrSell > MaxLevelSell) 
            { shuxerCloseBuy_OpenSell = true; shuxerCloseSell_OpenBuy = false; }


            if (shuxerCloseSell_OpenBuy) rse = "shuxerCloseSell_OpenBuy ";
            if (shuxerCloseBuy_OpenSell) rse = "shuxerCloseBuy_OpenSell ";
            //----------------------------------------------------------------
            // Сортування за зростанням ціни
            // Вибір останніх п'яти найбільших значень ціни

            var sortedByPrice = CurrLegPlot.OrderBy(kv => kv.Value.Price);
            var SpikeCurrLegUP = sortedByPrice.Skip(Math.Max(0, sortedByPrice.Count() - 5)).Take(5).ToList();

            var descendingSorted = CurrLegPlot.OrderByDescending(kv => kv.Value.Price);
            var SpikeCurrLegDN = descendingSorted.Take(5).ToList();
            var SpikeCurrLeg = (tip == "up") ? SpikeCurrLegUP : SpikeCurrLegDN;
            decimal spikeParametr = TradeZigZag.SpikeParametr;
           
            //----------------------------------------------------------------
            if (SpikeCurrLeg != null && SpikeCurrLeg.Count > 0)
            {
                decimal minPrice = SpikeCurrLeg.Min(p => p.Value.Price);
                decimal maxPrice = SpikeCurrLeg.Max(p => p.Value.Price);
                decimal dlina = maxPrice - minPrice;
                decimal sumMarketBuy = SpikeCurrLeg.Sum(p => p.Value.MarketBuy);
                decimal sumMarketSell = SpikeCurrLeg.Sum(p => p.Value.MarketSell);

                if (tip == "up")
                {
                    if (sumMarketBuy > sumMarketSell * 5)
                    {
                        if (dlina >= 1 * spikeParametr && dlina < 2 * spikeParametr) ser = rse + "spikeUPa";
                        if (dlina >= 2 * spikeParametr && dlina < 5 * spikeParametr) ser = rse + "spikeUPb";
                        if (dlina >= 5 * spikeParametr) ser = rse + "spikeUPc";
                    }
                    else ser = rse;


                }
                else // if(dn)
                {
                    if (sumMarketBuy * 5 < sumMarketSell)
                    {
                        if (dlina >= 1 * spikeParametr && dlina < 2 * spikeParametr) ser = rse + "spikeDNa";
                        if (dlina >= 2 * spikeParametr && dlina < 5 * spikeParametr) ser = rse + "spikeDNb";
                        if (dlina >= 5 * spikeParametr) ser = rse + "spikeDNc";
                    }
                    else ser = rse;

                }

                if (ser != "0")
                {
                    var zzL = ZZF.LastOrDefault();
                    if (zzL != null)
                    {
                        if (zzL.Tip == "UP" && tip == "dn")
                        {
                            // 1 ----- strong ZigZag, norm CurrLeg 
                            if (zzL.AvgBuy > zzL.AvgSell && zzL.AvgBuy < zzL.MaxBuyVol && zzL.MaxSellVol < zzL.MaxBuyVol) ers = " + Ord=2 + Strong ZigZag";

                            // 2 ----- strong CurrLeg, norm ZigZag
                            if (zzL.AvgBuy > zzL.AvgSell && zzL.AvgBuy < MaxCurrBuy && MaxCurrSell < MaxCurrBuy) ers = " + Ord=2 + Strong CurrLeg";

                            if (ers == "0")
                            {
                                if (PriceMaxCurrBuy < PriceMaxCurrSell && MaxCurrSell < MaxCurrBuy && zzL.MaxSellVol < MaxCurrBuy) ers = " + Ord=22";
                            }

                        }

                        if (zzL.Tip == "DN" && tip == "up")
                        {
                            // 1 ----- strong ZigZag, norm CurrLeg 
                            if (zzL.AvgBuy < zzL.AvgSell && zzL.AvgSell < zzL.MaxSellVol && zzL.MaxSellVol > zzL.MaxBuyVol) ers = " + Ord=1 + Strong ZigZag";

                            // 2 ----- strong CurrLeg, norm ZigZag
                            if (zzL.AvgBuy < zzL.AvgSell && zzL.AvgSell < MaxCurrSell && MaxCurrSell > MaxCurrBuy) ers = " + Ord=1 + Strong CurrLeg";

                            if(ers == "0")
                            {
                                if(PriceMaxCurrBuy < PriceMaxCurrSell && MaxCurrSell > MaxCurrBuy && zzL.MaxBuyVol < MaxCurrSell) ers = " + Ord=11";
                            }
                        }

                    }
                }
                else res = ser;

                if (ers != "0")
                {
                    res = ser + ers;
                }
                else res = ser;


                return (res);
            }
            else return res;
        }

        #endregion

        #region Knut:
        private void Knut(ulong ID_Extremum, ulong id, decimal StartPr, string tip, List<AggTradeFuture> tasLst)
        {
            bool subIf = id > ID_Extremum;
            if ((tip == "up" && subIf && id <= (ulong)tasLst.Count && ID_Extremum > 0) ||
                (tip == "dn" && id > ID_Extremum && id <= (ulong)tasLst.Count && ID_Extremum > 0))
            {
                var subList2 = (tasLst.ToArray().SubArray(ID_Extremum - 1, id - 1));
                if (subList2.Length != 0)
                {
                    int i = 0;
                    try
                    {
                        foreach (var item in subList2)
                        {
                            var LastPrice = item.data.Price;
                            var Vol = item.data.Volume;
                            bool BuyerMaker = item.data.IsMarketMaker;
                            decimal x;

                            if (tip == "up")
                            {
                                x = (LastPrice - StartPr) / priceStep;
                                i = (int)x;
                            }
                            else // if (tip == "dn")
                            {
                                x = (StartPr - LastPrice) / priceStep;
                                i = (int)x;
                            }

                            if (Kinchik[i] == null) Kinchik[i] = new Plot();

                            if (i >= 0 && i < 100000)
                            {
                                Kinchik[i].Price = item.data.Price;
                                Kinchik[i].MarketBuy += item.data.MarketBuy;
                                Kinchik[i].MarketSell += item.data.MarketSell;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(Knut)}[ERROR]", true, true);
                    }
                }
            }
            else
            {
                string logMsgEr = $"Pomilka ID_Q_Start= {ID_Extremum}   ID_Q_Finish= {id}  Bin= {tasLst.Count}";
                try
                {
                    new _().WriteToTxtFile(logMsgEr, "PomilkaKnut");
                }
                catch (Exception ex)
                {
                    var dt = DateTime.Now;
                }
            }

        }
        #endregion

        #region GazerKinchik:
        private int GazerKinchik(string tip)
        {
            decimal SumBuy = 0; decimal SumSell = 0;
            int res = 0, ser = 0;
            try
            {
                for (int i = 0; i < 43; i++)
                {
                    //SumBuy += Kinchik[i].LimitBuyAsk;
                    //SumBuy += Kinchik[i].MarketBuyAsk;
                    //SumSell += Kinchik[i].LimitSellBid;
                    //SumSell += Kinchik[i].MarketSellBid;

                    //if (Kinchik[i].LimitBuyAsk >= 1 || Kinchik[i].MarketBuyAsk >= 1)
                    //{
                    //    if (tip == "up") ser = 2;
                    //}

                    //if (Kinchik[i].LimitSellBid >= 1 || Kinchik[i].MarketSellBid >= 1)
                    //{
                    //    if (tip == "dn") ser = 1;
                    //}
                }
            }
            catch (Exception ex)
            {
                new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(GazerKinchik)}[ERROR]", true, true);
            }
            if (ser == 2 && SumBuy > SumSell) res = 2;
            if (ser == 1 && SumBuy < SumSell) res = 1;

            return (res);
        }
        #endregion

        #region Init arrays:
        private void InitZigZagArr()
        {
            //ZigZagArr = new Plot[300000]; // Змінюємо масив на null
            for (var i = 0; i < ZigZagArr.Length; i++)
                ZigZagArr[i] = new Plot();
        }
        private void InitCurrLeg()
        {
            CurrLeg = new Plot[300000]; // Змінюємо масив на null
            for (var i = 0; i < CurrLeg.Length; i++)
                CurrLeg[i] = new Plot();
        }
        private void InitKinchik()
        {
            Kinchik = new Plot[100000]; // Змінюємо масив на null
            for (var i = 0; i < Kinchik.Length; i++)
                Kinchik[i] = new Plot();
        }
        #endregion

        #region Aggregate of Market Depth:
        public class AggregatedBookItem
        {
            public decimal Price { get; set; }
            public decimal AggregatedVolume { get; set; }
        }

        private List<AggregatedBookItem> aggregatedBookType1 = new List<AggregatedBookItem>();
        private List<AggregatedBookItem> aggregatedBookType2 = new List<AggregatedBookItem>();

        private void MakeAggMD(string Event, string type, decimal startRange, decimal finishRange)
        {
            ConcurrentDictionary<decimal, MarketDepthUpdateFuture> MDZigRRR = default;


            if (Event == "ZigZag")
            {
                string logMsg = null;
                if (type == "UP") logMsg = $"** UP ****************************************************";
                if (type == "DN") logMsg = $"** DN ****************************************************";
                new _().WriteToTxtFile(logMsg, "MDLevel2FutureAg");
                new _().WriteToTxtFile($"WaveCounter: {WaveCounter}".ToString(), "MDLevel2FutureAg");
                var st = long.Parse(StartTime.ToString()).GetFullTime();
                var ft = long.Parse(FinishTime.ToString()).GetFullTime();
                logMsg = $"StartTime = {st.Hour}:{st.Minute}:{st.Second}.{st.Millisecond} FinishTime = {ft.Hour}:{ft.Minute}:{ft.Second}.{ft.Millisecond}";
                new _().WriteToTxtFile(logMsg, "MDLevel2FutureAg");
                logMsg = $"StartPrice = {StartPrice} FinishPrice = {FinishPrice}";
                new _().WriteToTxtFile(logMsg, "MDLevel2FutureAg");

                if (type == "UP") MDZigRRR = MDZigUP;
                else if (type == "DN") MDZigRRR = MDZigDN;
            }
            else
            {
                if (type == "UP") MDZigRRR = MinBidlistMD;
                else MDZigRRR = MaxAsklistMD;
            }


            if (MDZigRRR != null)
            {
                // Type 1 Aggregation
                var type1Items = MDZigRRR
                    .Where(item => item.Value.Type == 1 && item.Key <= startRange && item.Key >= finishRange && item.Value.Volume > 0)
                    .OrderByDescending(item => item.Key)
                    .ToList();

                decimal interval = TradeZigZag.Cluster;
                decimal currentRangeStart = type1Items.First().Key;
                decimal BestAsk = type1Items.Last().Key;
                decimal currentRangeEnd = currentRangeStart - interval;
                decimal nextRangeStart;

                while (currentRangeEnd >= BestAsk) //finishRange
                {
                    var sumVolume = type1Items
                        .Where(item => item.Key < currentRangeStart && item.Key >= currentRangeEnd)
                        .Sum(item => item.Value.Volume);

                    if (Event == "ZigZag")
                    {
                        new _().WriteToTxtFile($"Type: 1 Price: {currentRangeEnd}, " +
                            $"Aggregated Volume: {sumVolume}", "MDLevel2FutureAg");
                    }

                    aggregatedBookType1.Add(new AggregatedBookItem
                    {
                        Price = currentRangeEnd,
                        AggregatedVolume = sumVolume
                    });

                    nextRangeStart = currentRangeEnd;
                    currentRangeStart = nextRangeStart;
                    currentRangeEnd = nextRangeStart - interval;
                    if (currentRangeStart > BestAsk && currentRangeEnd < BestAsk) currentRangeEnd = BestAsk;

                }

                // Type 2 Aggregation
                var type2Items = MDZigRRR
                    .Where(item => item.Value.Type == 2 && item.Key <= startRange && item.Key >= finishRange && item.Value.Volume > 0)
                    .OrderByDescending(item => item.Key)
                    .ToList();

                currentRangeStart = type2Items.First().Key;
                currentRangeEnd = currentRangeStart - interval;

                while (currentRangeEnd >= finishRange)
                {
                    var sumVolume = type2Items
                        .Where(item => item.Key < currentRangeStart && item.Key >= currentRangeEnd)
                        .Sum(item => item.Value.Volume);

                    if (Event == "ZigZag")
                    {
                        new _().WriteToTxtFile($"Type: 2 Price: {currentRangeEnd}, " +
                        $"Aggregated Volume: {sumVolume}", "MDLevel2FutureAg");
                    }

                    aggregatedBookType2.Add(new AggregatedBookItem
                    {
                        Price = currentRangeEnd,
                        AggregatedVolume = sumVolume
                    });

                    nextRangeStart = currentRangeEnd;
                    currentRangeStart = nextRangeStart;
                    currentRangeEnd = nextRangeStart - interval;
                }

            }
        }

        private decimal Look4MaxVPrice(int tip)
        {
            decimal res = 0.0m;
            if (tip == 1)
            {
                decimal maxAggregatedVolumePrice = aggregatedBookType1
               .OrderByDescending(item => item.AggregatedVolume)
               .Select(item => item.Price)
               .FirstOrDefault();
                return maxAggregatedVolumePrice;
            }
            else
            {
                decimal maxAggregatedVolumePrice2 = aggregatedBookType2
                .OrderByDescending(item => item.AggregatedVolume)
                .Select(item => item.Price)
                .FirstOrDefault();
                return maxAggregatedVolumePrice2;
            }
            return res;

        }

        #endregion

    

        #region PasportDeal func:
        public void PasportDeal(decimal Ask, decimal Bid)
        {


        }
        #endregion
    }
}
