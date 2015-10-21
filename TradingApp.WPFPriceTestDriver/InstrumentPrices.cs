using Trade.PriceService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trade.WPFPriceTestDriver
{
    public class InstrumentPrices : IPrices, INotifyPropertyChanged
    {
        Prices prc;
        int index;
        uint newVolume = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        private double _bidPxAfterFrequencyInterval;
        private double _askPxAfterFrequencyInterval;
        private bool isBidPxDirty, isAskPxDirty;

        public InstrumentPrices(int index, Prices prices)
        {
            this.index = index;
            if (this.prc == null)
                this.prc = new Prices();
            this.prc.AskPx = prices.AskPx;
            this.prc.AskQty = prices.AskQty;
            this.prc.BidPx = prices.BidPx;
            this.prc.BidQty = prices.BidQty;
            this.prc.Volume = prices.Volume;
        }

        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                RaisePropertyChanged("Index");
            }
        }

        public double BidPx
        {
            get
            {
                return prc.BidPx;
            }
            set
            {
                prc.BidPx = value;
                RaisePropertyChanged("BidPx");
            }
        }

        public double BidPxAfterFrequencyInterval
        {
            get
            {
                return _bidPxAfterFrequencyInterval;
            }
            set
            {
                _bidPxAfterFrequencyInterval = value;
                RaisePropertyChanged("BidPxAfterFrequencyInterval");
            }
        }

        public uint BidQty
        {
            get
            {
                return prc.BidQty;
            }
            set
            {
                prc.BidQty = value;
                RaisePropertyChanged("BidQty");
            }
        }

        public double AskPx
        {
            get
            {
                return prc.AskPx;
            }
            set
            {
                prc.AskPx = value;
                RaisePropertyChanged("AskPx");
            }
        }
        public double AskPxAfterFrequencyInterval
        {
            get
            {
                return _askPxAfterFrequencyInterval;
            }
            set
            {
                _askPxAfterFrequencyInterval = value;
                RaisePropertyChanged("AskPxAfterFrequencyInterval");
            }
        }

        /// <summary>
        /// This flag will decide whether the instrument's color be changed to neutral color or not
        /// If the value of an instrument is updated in this cycle then this flag should be set to true
        /// For instrument's whose IsDirty flag is false(i.e. there value didn,t change), color would be changed to neutral 
        /// </summary>
        public bool IsBidPxDirty
        {
            get { return isBidPxDirty; }
            set { isBidPxDirty = value; }
        }
        public bool IsAskPxDirty
        {
            get { return isAskPxDirty; }
            set { isAskPxDirty = value; }
        }
        public uint AskQty
        {
            get
            {
                return prc.AskQty;
            }
            set
            {
                prc.AskQty = value;
                RaisePropertyChanged("AskQty");
            }
        }

        public uint Volume
        {
            get
            {
                return prc.Volume;
            }
            set
            {
                prc.Volume = value;
                RaisePropertyChanged("Volume");
            }
        }
        /// <summary>
        /// This property defines the volumes accumulated by an Instrument since last cycle(Minimum Frequency is 150ms)
        /// </summary>
        public uint NewVolume
        {
            get
            {
                return newVolume;
            }
            set
            {
                newVolume = value;
                RaisePropertyChanged("NewVolume");
            }
        }
        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        
    }
}
