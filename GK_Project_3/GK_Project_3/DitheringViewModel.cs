using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GK_Project_3
{
    public class DitheringViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        byte rValue;
        public byte RValue
        {
            get { return rValue; }
            set
            {
                if (rValue != value)
                {
                    rValue = value;
                    OnPropertyChanged(nameof(RValue));
                }
            }
        }

        byte gValue;
        public byte GValue
        {
            get { return gValue; }
            set
            {
                if (gValue != value)
                {
                    gValue = value;
                    OnPropertyChanged(nameof(GValue));
                }
            }
        }

        byte bValue;
        public byte BValue
        {
            get { return bValue; }
            set
            {
                if (bValue != value)
                {
                    bValue = value;
                    OnPropertyChanged(nameof(BValue));
                }
            }
        }

        int kAverageValue;
        public int KAverageValue
        {
            get { return kAverageValue; }
            set
            {
                if (kAverageValue != value)
                {
                    kAverageValue = value;
                    OnPropertyChanged(nameof(KAverageValue));
                }
            }
        }

        int kPopularValue;
        public int KPopularValue
        {
            get { return kPopularValue; }
            set
            {
                if (kPopularValue != value)
                {
                    kPopularValue = value;
                    OnPropertyChanged(nameof(KPopularValue));
                }
            }
        }

        WriteableBitmap bitmap;
        public WriteableBitmap ImageSource
        {
            get { return bitmap; }
            set
            {
                if (bitmap != value)
                {
                    bitmap = value;
                    PropagateErrorImageSource = BitmapFactory.New(bitmap.PixelWidth, bitmap.PixelHeight);
                    KAverageImageSource = BitmapFactory.New(bitmap.PixelWidth, bitmap.PixelHeight);
                    KPopularImageSource = BitmapFactory.New(bitmap.PixelWidth, bitmap.PixelHeight);
                    OnPropertyChanged(nameof(ImageSource));
                }
            }
        }

        WriteableBitmap propagateErrorBitmap;
        public WriteableBitmap PropagateErrorImageSource
        {
            get { return propagateErrorBitmap; }
            set
            {
                if (propagateErrorBitmap != value)
                {
                    propagateErrorBitmap = value;
                    OnPropertyChanged(nameof(PropagateErrorImageSource));
                }
            }
        }

        WriteableBitmap kaverageBitmap;
        public WriteableBitmap KAverageImageSource
        {
            get { return kaverageBitmap; }
            set
            {
                if (kaverageBitmap != value)
                {
                    kaverageBitmap = value;
                    OnPropertyChanged(nameof(KAverageImageSource));
                }
            }
        }

        WriteableBitmap kpopularBitmap;
        public WriteableBitmap KPopularImageSource
        {
            get { return kpopularBitmap; }
            set
            {
                if (kpopularBitmap != value)
                {
                    kpopularBitmap = value;
                    OnPropertyChanged(nameof(KPopularImageSource));
                }
            }
        }

        public string PropagateErrorText { get; set; }

        public string KAverageText { get; set; }

        public string KPopularText { get; set; }


        public bool FloydSteinberg { get; set; }

        public bool Burkes { get; set; }

        public bool Stucky { get; set; }

        public bool PropagateErrorRunning { get; private set;  }
        public bool KAverageRunning { get; private set; }
        public bool KPopularRunning { get; private set; }
        #endregion

        public RelayCommand GeneratePropagateError { get; }
        public RelayCommand GenerateKAverage { get; }
        public RelayCommand GenerateKPopular { get; }

        public RelayCommand ChooseImageCommand { get; }

        bool IsAnyRunning()
        {
            return PropagateErrorRunning || KAverageRunning || KPopularRunning;
        }
        public DitheringViewModel()
        {
            TryGetBitmap();
            FloydSteinberg = true;
            RValue = 128;
            GValue = 128;
            BValue = 128;
            KAverageValue = 4;
            KPopularValue = 4;

            GeneratePropagateError = new RelayCommand(x => PropagateErrorMethod(x), x => ImageSource != null && PropagateErrorRunning == false);
            GenerateKAverage = new RelayCommand(x => KAverageMethod(x), x => ImageSource != null && KAverageRunning == false);
            GenerateKPopular = new RelayCommand(x => KPopularMethod(x), x => ImageSource != null && KPopularRunning == false);
            ChooseImageCommand = new RelayCommand(x => ChooseImage(x),x=>IsAnyRunning()==false);

            if (ImageSource != null)
            {
                /* PropagateErrorMethod();
                 KAverageMethod();
                 KPopularMethod();*/
            }

            ///Timers
            ///
            PropagateErrorTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background, PropagateErrorTimer_Tick, Dispatcher.CurrentDispatcher);
            PropagateErrorTimer.Stop();
            KAverageTimer= new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background, KAverageTimer_Tick, Dispatcher.CurrentDispatcher);
            KAverageTimer.Stop();
            KPopularTimer =new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background, KPopularTimer_Tick, Dispatcher.CurrentDispatcher);
            KPopularTimer.Stop();
            PropagateErrorTimeText = String.Empty;
            KPopularTimeText = String.Empty;
            KAverageTimeText = String.Empty;
            ////
        }

        void TryGetBitmap()
        {
            try
            {
                ImageSource = new WriteableBitmap(new BitmapImage(new Uri("pack://application:,,,/Resources/Lenna.png")));
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Nie udało się wczytać domyślnego obrazka: {0} {1}", Environment.NewLine, ex.Message));
            }
        }

        void ChooseImage(object parameter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    BitmapSource source = new BitmapImage(new Uri(ofd.FileName));
                    ImageSource = new WriteableBitmap(source);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Nie udało się dodać obrazka {0} {1}", Environment.NewLine, ex.Message));
                }
            }
        }
        
        #region Propagate Error
        private async void PropagateErrorMethod(object parameter)
        {
            try
            {
                PropagateErrorRunning = true;
                OnPropertyChanged(nameof(PropagateErrorRunning));
                byte[] source = ImageSource.ToByteArray();
                int width = PropagateErrorImageSource.PixelWidth;
                int height = PropagateErrorImageSource.PixelHeight;
                startPropagateError = DateTime.Now;
                PropagateErrorTimer.Start();
                if (FloydSteinberg)
                {
                    byte[] result = await PropagateError.GetDitheredBitmapFloydSteinbergAsync(source, width, height, RValue, GValue, BValue);
                    PropagateErrorImageSource = PropagateErrorImageSource.FromByteArray(result);
                    PropagateErrorText = "Algorytm Propagacji błedów (FloydSteinberg)";
                }
                else if (Stucky)
                {
                    byte[] result = await PropagateError.GetDitheredBitmapStuckyAsync(source, width, height, RValue, GValue, BValue);
                    PropagateErrorImageSource = PropagateErrorImageSource.FromByteArray(result);
                    PropagateErrorText = "Algorytm Propagacji błedów (Stucky)";

                }
                else if (Burkes)
                {
                    byte[] result = await PropagateError.GetDitheredBitmapBurkesAsync(source, width, height, RValue, GValue, BValue);
                    PropagateErrorImageSource = PropagateErrorImageSource.FromByteArray(result);
                    PropagateErrorText = "Algorytm Propagacji błedów (Burkes)";
                }
                PropagateErrorText = PropagateErrorText + ": " + PropagateErrorTimeText;
                OnPropertyChanged(nameof(PropagateErrorText));
            }
            catch (Exception ex)
            {
                AlgorithmErrorMessage(ex.Message);
            }
            PropagateErrorTimeText = String.Empty;
            OnPropertyChanged(nameof(PropagateErrorTimeText));
            PropagateErrorTimer.Stop();
            PropagateErrorRunning = false;
            //Refresh
            OnPropertyChanged(nameof(PropagateErrorRunning));
            GeneratePropagateError.RaiseCanExecuteChanged();
        }

        #endregion

        #region KAverage

        private async void KAverageMethod(object parameter)
        {
            
            try
            {
                KAverageRunning = true;
                OnPropertyChanged(nameof(KAverageRunning));
                byte[] source = ImageSource.ToByteArray();
                int width = ImageSource.PixelWidth;
                int height = ImageSource.PixelHeight;
                startKAverage = DateTime.Now;
                KAverageTimer.Start();

                byte[] result = await KAverage.GetReducedBitmapKAverageAsync(source, width, height, KAverageValue);
                KAverageImageSource = KAverageImageSource.FromByteArray(result);
                KAverageText = "Algorytm K-średnich ("+KAverageTimeText+")";
                OnPropertyChanged(nameof(KAverageText));
            }
            catch (Exception ex)
            {
                AlgorithmErrorMessage(ex.Message);
            }
            KAverageTimeText = String.Empty;
            OnPropertyChanged(nameof(KAverageTimeText));
            KAverageTimer.Stop();
            KAverageRunning = false;
            //Refresh
            OnPropertyChanged(nameof(KAverageRunning));
            GenerateKAverage.RaiseCanExecuteChanged();
        }

        #endregion

        #region KPopular
        private async void KPopularMethod(object parameter)
        {
            try
            {
                KPopularRunning = true;
                OnPropertyChanged(nameof(KPopularRunning));
                byte[] source = ImageSource.ToByteArray();
                int width = ImageSource.PixelWidth;
                int height = ImageSource.PixelHeight;
                startKPopular = DateTime.Now;
                KPopularTimer.Start();

                byte[] result = await KPopular.GetReducedBitmapKPopularAsync(source,width,height, KPopularValue);
                KPopularImageSource = KPopularImageSource.FromByteArray(result);
                KPopularText = "Algorytm popularnościowy (" + KPopularTimeText + ")"; ;
                OnPropertyChanged(nameof(KPopularText));
            }
            catch (Exception ex)
            {
                AlgorithmErrorMessage(ex.Message);
            }
            KPopularTimeText = String.Empty;
            OnPropertyChanged(nameof(KPopularTimeText));
            KPopularTimer.Stop();
            KPopularRunning = false;
            //Refresh
            OnPropertyChanged(nameof(KPopularRunning));
            GenerateKPopular.RaiseCanExecuteChanged();
        }

        #endregion

        void AlgorithmErrorMessage(string message)
        {
            MessageBox.Show(String.Format("Wystąpił problem podczas algorytmu: {0} ,{1}", Environment.NewLine, message));
        }

        #region Timers
        public string KAverageTimeText { get; set; }
        DateTime startKAverage;
        DispatcherTimer KAverageTimer;
        void KAverageTimer_Tick(object sender,EventArgs e)
        {
            KAverageTimeText = (DateTime.Now - startKAverage).ToString(@"dd\.hh\:mm\:ss");
            OnPropertyChanged(nameof(KAverageTimeText));
        }

        public string KPopularTimeText { get; set; }
        DateTime startKPopular;
        DispatcherTimer KPopularTimer;
        void KPopularTimer_Tick(object sender, EventArgs e)
        {
            KPopularTimeText = (DateTime.Now - startKPopular).ToString(@"dd\.hh\:mm\:ss");
            OnPropertyChanged(nameof(KPopularTimeText));
        }

        public string PropagateErrorTimeText { get; set; }
        DateTime startPropagateError;
        DispatcherTimer PropagateErrorTimer;
        void PropagateErrorTimer_Tick(object sender, EventArgs e)
        {
            PropagateErrorTimeText = (DateTime.Now - startPropagateError).ToString(@"dd\.hh\:mm\:ss");
            OnPropertyChanged(nameof(PropagateErrorTimeText));
        }
        #endregion
    }
}
