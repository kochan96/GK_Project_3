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
        #endregion

        public RelayCommand GenerateCommand { get; }

        public RelayCommand ChooseImageCommand { get; }

        public DitheringViewModel()
        {
            TryGetBitmap();
            FloydSteinberg = true;
            RValue = 128;
            GValue = 128;
            BValue = 128;
            KAverageValue = 4;
            KPopularValue = 4;

            GenerateCommand = new RelayCommand(x => GenerateMethod(x), x => ImageSource != null);
            ChooseImageCommand = new RelayCommand(x => ChooseImage(x));

            if (ImageSource != null)
            {
                PropagateErrorMethod();
                KAverageMethod();
                KPopularMethod();
            }
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


        void GenerateMethod(object parameter)
        {
            switch (parameter)
            {
                case "0":
                    PropagateErrorMethod();
                    break;

                case "1":
                    KAverageMethod();
                    break;
                case "2":
                    KPopularMethod();
                    break;
                default:
                    PropagateErrorMethod();
                    break;
            }
        }

        private async void PropagateErrorMethod()
        {
            try
            {
                if (FloydSteinberg)
                {
                    byte[] source = ImageSource.ToByteArray();
                    int width = PropagateErrorImageSource.PixelWidth;
                    int height = PropagateErrorImageSource.PixelHeight;
                    byte[] result = await PropagateError.GetDitheredBitmapByteArrayAsync(source, width, height, RValue, GValue, BValue);
                    PropagateErrorImageSource = PropagateErrorImageSource.FromByteArray(result);
                    PropagateErrorText = "Algorytm Propagacji błedów (FloydSteinberg)";
                }
                else if (Stucky)
                {
                    byte[] result = PropagateError.GetDitheredBitmapStucky(ImageSource, RValue, GValue, BValue);
                    PropagateErrorImageSource = PropagateErrorImageSource.FromByteArray(result);
                    PropagateErrorText = "Algorytm Propagacji błedów (Stucky)";

                }
                else if (Burkes)
                {
                    byte[] result = PropagateError.GetDitheredBitmapBurkes(ImageSource, RValue, GValue, BValue);
                    PropagateErrorImageSource = PropagateErrorImageSource.FromByteArray(result);
                    PropagateErrorText = "Algorytm Propagacji błedów (Burkes)";
                }
                OnPropertyChanged(nameof(PropagateErrorText));
            }
            catch (Exception ex)
            {
                AlgorithmErrorMessage(ex.Message);
            }
        }


        private void KAverageMethod()
        {
            try
            {
                byte[] result = KAverage.GetDitheredBitmapKAverage(ImageSource, KAverageValue);
                KAverageImageSource = KAverageImageSource.FromByteArray(result);
                KAverageText = "Algorytm K-średnich";
                OnPropertyChanged(nameof(KAverageText));
            }
            catch (Exception ex)
            {
                AlgorithmErrorMessage(ex.Message);
            }
        }

        private void KPopularMethod()
        {
            try
            {
                byte[] result = KPopular.GetDitheredBitmapKPopular(ImageSource, KPopularValue);
                KPopularImageSource = KPopularImageSource.FromByteArray(result);
                KPopularText = "Algorytm popularnościowy";
                OnPropertyChanged(nameof(KPopularText));
            }
            catch (Exception ex)
            {
                AlgorithmErrorMessage(ex.Message);
            }
        }

        void AlgorithmErrorMessage(string message)
        {
            MessageBox.Show(String.Format("Wystąpił problem podczas algorytmu: {0} ,{1}", Environment.NewLine, message));
        }
    }
}
