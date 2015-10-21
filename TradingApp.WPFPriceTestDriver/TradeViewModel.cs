using Trade.PriceService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Trade.WPFPriceTestDriver
{
    /// <summary>
    /// This class defines the ViewModel for the UI of application
    /// This can also be seen as the data context of the application
    /// All the exposed properties are defined in this viewmodel which are then 
    /// binded to the User interface
    /// </summary>
    public class TradeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        DelegateCommand updateFrequencyCommand, startTradingCommand, stopTradingCommand;
        RandomWalkPriceService priceService;
        int frequency = 150;
        Timer _UIRefreshTimer, _clockCycleTimer;
        Dictionary<int, Prices> tradeItems;
        ObservableCollection<InstrumentPrices> tradeItemsCollection;

        /// <summary>
        /// Raises the event for updating the property on UI
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets or sets the TradeItems colelction. This is an observable collection 
        /// which stores a colelction of the all the instruments and their latest values.
        /// Observable collection is used instead of other IEnumerable objects because
        /// it raises the INotify event internally for every item that is changed inside it
        /// </summary>
        public ObservableCollection<InstrumentPrices> TradeItemsCollection
        {
            get { return tradeItemsCollection; }
            set { tradeItemsCollection = value; OnPropertyChanged("TradeItemsCollection"); }
        }

        /// <summary>
        /// Gets or sets the TradeItems dictionary
        /// The only purpose of this dictionary is to help with the searching of Instrument index
        /// Because the dictionary uses hash table to quickly find the object and we need fast searching
        /// because of the frequency with which object values are updated on the UI
        /// </summary>
        public Dictionary<int, Prices> TradeItems
        {
            get { return tradeItems; }
            set
            {
                tradeItems = value;
                OnPropertyChanged("TradeItems");
            }
        }

        /// <summary>
        /// Gets or sets the Update Frequency command
        /// </summary>
        public DelegateCommand UpdateFrequencyCommand
        {
            get { return updateFrequencyCommand; }
            set { updateFrequencyCommand = value; }
        }

        /// <summary>
        /// Get or sets the stop trading delegate command
        /// </summary>
        public DelegateCommand StopTradingCommand
        {
            get { return stopTradingCommand; }
            set { stopTradingCommand = value; }
        }

        /// <summary>
        /// Gets or sets the start trading command
        /// </summary>
        public DelegateCommand StartTradingCommand
        {
            get { return startTradingCommand; }
            set { startTradingCommand = value; }
        }

        /// <summary>
        /// Gets or sets the frequency with which certain columns should be updated on the UI
        /// </summary>
        public int Frequency
        {
            get
            { return frequency; }
            set
            { frequency = value; OnPropertyChanged("Frequency"); }
        }

        /// <summary>
        /// This method defines the constructor and will initialize various delegate commands and collections
        /// </summary>
        public TradeViewModel()
        {
            tradeItemsCollection = new ObservableCollection<InstrumentPrices>();
            InitializeTradeService();
            tradeItems = new Dictionary<int, Prices>();
            updateFrequencyCommand = new DelegateCommand(() => { return true; }, UpdateFrequency);
            startTradingCommand = new DelegateCommand(() => { return CanStart; }, StartTrading);
            stopTradingCommand = new DelegateCommand(() => { return CanStop; }, StopTrading);
            _UIRefreshTimer = new Timer(frequency);
            _clockCycleTimer = new Timer(50);
            _UIRefreshTimer.Elapsed += OnFrequencyTimerHandler;
            _clockCycleTimer.Elapsed += OnCycleTimerHandler;
            _UIRefreshTimer.Start();
            _clockCycleTimer.Start();

        }

        /// <summary>
        /// This block of code executes every frequency interval atleast 150ms for each refresh UI cycle
        /// This frequency interval can be manipulated through GUI. It will delay the refresh of certain columns 
        /// such as Bid/Ask price and volume other columns such as Bid/Ask Quantiy will be refreshed every RWPS cycle 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFrequencyTimerHandler(object sender, ElapsedEventArgs e)
        {
            for (int instruId = 0; instruId < 10; instruId++)
            {
                tradeItemsCollection[instruId].NewVolume = tradeItemsCollection[instruId].Volume;
                tradeItemsCollection[instruId].AskPxAfterFrequencyInterval = tradeItemsCollection[instruId].AskPx;
                tradeItemsCollection[instruId].BidPxAfterFrequencyInterval = tradeItemsCollection[instruId].BidPx;
            }
        }

        /// <summary>
        /// This block of code executes every RWPS cycle i.e. every 50ms
        /// If a price has not been updated since the last update cycle then its background color is reset to neutral (LightGray)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCycleTimerHandler(object sender, ElapsedEventArgs e)
        {
            //set the dirty flag to false for all the instruments
            for (int i = 0; i < 10; i++)
            {
                if (!tradeItemsCollection[i].IsBidPxDirty)
                    tradeItemsCollection[i].BidPxAfterFrequencyInterval = tradeItemsCollection[i].BidPxAfterFrequencyInterval;
                if (!tradeItemsCollection[i].IsAskPxDirty)
                    tradeItemsCollection[i].AskPxAfterFrequencyInterval = tradeItemsCollection[i].AskPxAfterFrequencyInterval;
                tradeItemsCollection[i].IsBidPxDirty = false;
                tradeItemsCollection[i].IsAskPxDirty = false;
            }
        }

        /// <summary>
        /// This method will subsccribe to NewPriceArrived event, this even will be raised 20 times in a second
        /// and hence the handler will also be invoked that many times
        /// </summary>
        void InitializeTradeService()
        {
            InitializeDummyInstruments();
            priceService = new RandomWalkPriceService();
            priceService.NewPricesArrived += PriceUpdateHandler;
        }

        /// <summary>
        /// This boolean property decides whether to allow execution of Stop command
        /// </summary>
        bool CanStop
        {
            get
            {
                return priceService.IsStarted;
            }
        }

        /// <summary>
        /// This boolean property decides whether to allow execute operation on Start command
        /// </summary>
        bool CanStart
        {
            get
            {
                return !priceService.IsStarted;
            }
        }

        /// <summary>
        /// this method initializes the instrument price, quanties with 0 value
        /// </summary>
        private void InitializeDummyInstruments()
        {
            for (int i = 0; i < 10; i++)
            {
                var newPrice = new Prices() { Volume = 0, BidQty = 0, BidPx = 0, AskQty = 0, AskPx = 0 };
                var newInstrument = new InstrumentPrices(i, newPrice);
                tradeItemsCollection.Add(newInstrument);
            }
        }
        private void StopTrading()
        {
            if (priceService != null)
                priceService.Stop();
        }

        /// <summary>
        /// This method starts the timer of our Dummy service so that
        /// new price notifications are recevied 20 times every second
        /// </summary>
        private void StartTrading()
        {
            if (priceService != null)
                priceService.Start();
        }

        /// <summary>
        /// Changes the content of our collection of Instruments
        /// This handler is invoked 20 times a second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="instrumentID"></param>
        /// <param name="prices"></param>
        void PriceUpdateHandler(IPriceService sender, uint instrumentID, IPrices prices)
        {
            int instruId = (int)instrumentID;
            Prices newPrices = (Prices)prices;

            if (tradeItems.ContainsKey(instruId))
            {
                tradeItemsCollection[instruId].AskPx = newPrices.AskPx;
                tradeItemsCollection[instruId].AskQty = newPrices.AskQty;
                tradeItemsCollection[instruId].BidPx = newPrices.BidPx;
                tradeItemsCollection[instruId].BidQty = newPrices.BidQty;
                tradeItemsCollection[instruId].Volume = newPrices.Volume;
                // Also set dirty flag to true
                tradeItemsCollection[instruId].IsBidPxDirty = true;
                tradeItemsCollection[instruId].IsAskPxDirty = true;
            }
            else
            {
                tradeItems.Add((int)instrumentID, (Prices)prices);
            }
        }

        /// <summary>
        /// Resets the timer based on the frequency seleccted by the user
        /// </summary>
        public void UpdateFrequency()
        {
            _UIRefreshTimer.Stop();
            _UIRefreshTimer.Interval = (double)(frequency)>0.0 ? (double)(frequency):1000;
            _UIRefreshTimer.Start();
        }
    }
}
