using IIAADataModels.Transfer;
using LaunchPad.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace LaunchPad.Mobile.Models
{
    public class CustomProductCategory : ViewModelBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subtitle { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        public ICommand SelectCommand => new Command<string>((param) =>
          {
              IsSelected = true;
              ItemSelectedCommand.Execute(param);
          });
        public ICommand ItemSelectedCommand { get; set; }
    }
    public class CustomProduct : INotifyPropertyChanged
    {
        public Product Product { get; set; }

        public Uri ImageUrl { get; set; }

        public List<CustomProductAdditionalInfo> AdditionalInformations { get; set; }

        public CustomProductAdditionalInfo SelectedAdditionalInfo { get; set; }

        public Command AddPlanCommand { get; set; }
        public Command RemovePlanCommand { get; set; }

        private bool _isProductAdded;
        public bool IsProductAdded
        {
            get => _isProductAdded;
            set => SetProperty(ref _isProductAdded, value);
        }

        private ObservableCollection<ProductVariant> _variantsList;
        public ObservableCollection<ProductVariant> VariantsList
        {
            get => _variantsList;
            set => SetProperty(ref _variantsList, value);
        }

        private ProductVariant _selectedVariant;
        public ProductVariant SelectedVariant
        {
            get => _selectedVariant;
            set
            {
                SetProperty(ref _selectedVariant, value);
                if (SelectedVariant != null)
                {
                    Price = $"£{SelectedVariant.Price.ToString("F2")}";
                    if (SelectedVariant.PrescribingOptions != null && SelectedVariant.PrescribingOptions.Count > 0)
                    {
                        ShouldShowSubVariant = true;
                        PrescribingOptions = new ObservableCollection<ProductVariantPrescribingOption>(SelectedVariant.PrescribingOptions);
                        SelectedOption = PrescribingOptions.FirstOrDefault();
                    }

                    RefreshPriceCommand?.Execute(null);
                }
            }
        }
        private ObservableCollection<ProductVariantPrescribingOption> _prescribingOptions;
        public ObservableCollection<ProductVariantPrescribingOption> PrescribingOptions
        {
            get => _prescribingOptions;
            set => SetProperty(ref _prescribingOptions, value);
        }

        private ProductVariantPrescribingOption _selectedOption;
        public ProductVariantPrescribingOption SelectedOption
        {
            get => _selectedOption;
            set => SetProperty(ref _selectedOption, value);
        }

        private string _price;
        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private bool _shouldShowSubVariant = false;
        public bool ShouldShowSubVariant
        {
            get => _shouldShowSubVariant;
            set => SetProperty(ref _shouldShowSubVariant, value);
        }

        private bool _shouldShowVariant = false;
        public bool ShouldShowVariant
        {
            get => _shouldShowVariant;
            set => SetProperty(ref _shouldShowVariant, value);
        }

        public Command RefreshPriceCommand { get; set; }
        public Command PurchaseCommand { get; set; }
        public Command AddProductToPlanCommand => new Command<CustomProduct>((param) =>
          {
              IsProductAdded = true;
              AddPlanCommand.Execute(param);
          });
        public Command RemoveProductFromPlanCommand => new Command<CustomProduct>((param) =>
          {
              IsProductAdded = false;
              RemovePlanCommand.Execute(param);
          });
        public CustomProduct()
        {
            AdditionalInformations = new List<CustomProductAdditionalInfo>();
            SelectedAdditionalInfo = new CustomProductAdditionalInfo();
            VariantsList = new ObservableCollection<ProductVariant>();
            PrescribingOptions = new ObservableCollection<ProductVariantPrescribingOption>();
        }
        #region # INotifyPropertyChanged Impl #
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value,
                                      [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class CompletedHealthPlan : ViewModelBase
    {
        public HealthPlanToComplete HealthPlanToComplete { get; set; }

        private bool _isDropdownVisible;
        public bool IsDropdownVisible
        {
            get => _isDropdownVisible;
            set => SetProperty(ref _isDropdownVisible, value);
        }

        private int _loyalityPoints;
        public int LoyalityPoints
        {
            get => _loyalityPoints;
            set => SetProperty(ref _loyalityPoints, value);
        }
        private bool _isScanning = false;
        public bool IsScanning
        {
            get => _isScanning;
            set => SetProperty(ref _isScanning, value);
        }

        private bool _isProductScanned = false;
        public bool IsProductScanned
        {
            get => _isProductScanned;
            set => SetProperty(ref _isProductScanned, value);
        }
        public Command ScanCommand => new Command<Product>((param) =>
        {
            IsScanning = true;
            CloseOtherScanExceptthisCommand?.Execute(param);
        });
        public Command ScanResultCommand => new Command<ZXing.Result>((param) =>
        {
            IsScanning = false;
            var result = param.Text;
            IsProductScanned = true;
            IsDropdownVisible = false;
            ProductScannedCommand?.Execute(param);
        });
        public Command CloseOtherScanExceptthisCommand { get; set; }
        public Command ProductScannedCommand { get; set; }

        public ICommand RemoveCommand => new Command<Product>((param) =>
          {
              IsProductScanned = false;
              LoyalityPoints = 0;
              RemoveLoyaltyCommand.Execute(param);
          });
        public Command RemoveLoyaltyCommand { get; set; }
    }
    public class CustomProductAdditionalInfo : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public ProductAdditionalInformation AdditionalInformation { get; set; }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public Color BackgroundColor { get; set; }

        public Command ViewCommand => new Command<CustomProductAdditionalInfo>((param) =>
          {
              IsSelected = !IsSelected;
              ItemSelectedCommand.Execute(param);
          });
        public Command ItemSelectedCommand { get; set; }

        #region # INotifyPropertyChanged Impl #
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value,
                                      [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public CustomProductAdditionalInfo()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class FilterOption : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Option { get; set; }
        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        public Command SelectCommand => new Command<CustomProductAdditionalInfo>((param) =>
        {
            IsSelected = !IsSelected;
            if (IsSelected)
            {
                BackgroundColor = Color.Black;
            }
            else
            {
                BackgroundColor = Color.Transparent;
            }
        });

        #region # INotifyPropertyChanged Impl #
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value,
                                      [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public FilterOption()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class HealthPlan : INotifyPropertyChanged
    {
        public string ProgramName { get; set; }


        private ObservableCollection<ProductWithLevelType> _productWithLevelTypeList;
        public ObservableCollection<ProductWithLevelType> ProductWithLevelTypeList
        {
            get => _productWithLevelTypeList;
            set => SetProperty(ref _productWithLevelTypeList, value);
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public Command ToggleCommand => new Command(() =>
          {
              IsSelected = !IsSelected;
          });
        public HealthPlan()
        {
            ProductWithLevelTypeList = new ObservableCollection<ProductWithLevelType>();
        }

        #region # INotifyPropertyChanged Impl #
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value,
                                      [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class CustomVariant : INotifyPropertyChanged
    {
        private ProductVariant _variant;
        public ProductVariant Variant
        {
            get => _variant;
            set => SetProperty(ref _variant, value);
        }

        public Guid ProductId { get; set; }

        public bool IsSelected { get; set; }

        #region # INotifyPropertyChanged Impl #
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value,
                                      [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class ProductWithLevelType : INotifyPropertyChanged
    {
        public string Classification { get; set; }
        public string ProgramName { get; set; }

        private ObservableCollection<CustomProduct> _products;
        public ObservableCollection<CustomProduct> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ProductWithLevelType()
        {
            Products = new ObservableCollection<CustomProduct>();
        }

        #region # INotifyPropertyChanged Impl #
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value,
                                      [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class CustomBasket
    {
        public string Id { get; set; }
        public Basket Basket { get; set; }
        public List<CustomBasketInfo> ItemsCollection { get; set; }
    }

    public class CustomBasketInfo : INotifyPropertyChanged
    {
        public CustomProduct Product { get; set; }
        public string ProgramName { get; set; }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }

        public string Price { get; set; }

        public ProductVariant Variant { get; set; }

        private bool _shouldRemove;
        public bool ShouldRemove
        {
            get => _shouldRemove;
            set => SetProperty(ref _shouldRemove, value);
        }

        public Command RemoveCommand { get; set; }

        #region # INotifyPropertyChanged Impl #
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value,
                                      [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class StarRatings : INotifyPropertyChanged
    {
        private string _star;
        public string Star
        {
            get => _star;
            set => SetProperty(ref _star, value);
        }
        private double _widthRequest;
        public double WidthRequest
        {
            get => _widthRequest;
            set => SetProperty(ref _widthRequest, value);
        }
        private double _heightRequest;
        public double HeightRequest
        {
            get => _heightRequest;
            set => SetProperty(ref _heightRequest, value);
        }

        private Thickness _margin;
        public Thickness Margin
        {
            get => _margin;
            set => SetProperty(ref _margin, value);
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        #region # INotifyPropertyChanged Impl #
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value,
                                      [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class CustomTherapist
    {
        public Therapist Therapist { get; set; }
        public ICommand SelectCommand { get; set; }
    }

    public class HealthPlanToComplete : ViewModelBase
    {
        public string ProgramName { get; set; }
        public Product Product { get; set; }
        public Uri ImageUrl { get; set; }
        private ObservableCollection<ProductVariant> _variantsList;
        public ObservableCollection<ProductVariant> VariantsList
        {
            get => _variantsList;
            set => SetProperty(ref _variantsList, value);
        }
        private bool _shouldShowSubVariant = false;
        public bool ShouldShowSubVariant
        {
            get => _shouldShowSubVariant;
            set => SetProperty(ref _shouldShowSubVariant, value);
        }

        private bool _shouldShowVariant = false;
        public bool ShouldShowVariant
        {
            get => _shouldShowVariant;
            set => SetProperty(ref _shouldShowVariant, value);
        }
        private ProductVariant _selectedVariant;
        public ProductVariant SelectedVariant
        {
            get => _selectedVariant;
            set => SetProperty(ref _selectedVariant, value);
        }
        private ObservableCollection<ProductVariantPrescribingOption> _prescribingOptions;
        public ObservableCollection<ProductVariantPrescribingOption> PrescribingOptions
        {
            get => _prescribingOptions;
            set => SetProperty(ref _prescribingOptions, value);
        }

        private ProductVariantPrescribingOption _selectedOption;
        public ProductVariantPrescribingOption SelectedOption
        {
            get => _selectedOption;
            set => SetProperty(ref _selectedOption, value);
        }
        private bool _productScanned = false;
        public bool ProductScanned
        {
            get => _productScanned;
            set => SetProperty(ref _productScanned, value);
        }
    }
}
