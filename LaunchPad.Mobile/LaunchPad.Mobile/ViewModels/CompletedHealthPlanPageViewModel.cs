using FormsControls.Base;
using IIAADataModels.Transfer;
using LaunchPad.Client;
using LaunchPad.Mobile.Helpers;
using LaunchPad.Mobile.Models;
using LaunchPad.Mobile.Services;
using LaunchPad.Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace LaunchPad.Mobile.ViewModels
{
    public class CompletedHealthPlanPageViewModel : ViewModelBase
    {
        public ICommand GoBackCommand => new Command(() => Application.Current.MainPage.Navigation.PopAsync());
        public ICommand HomeCommand => new Command(() => Application.Current.MainPage = new AnimationNavigationPage(new SalonClientsPage()));
        private IToastServices ToastServices => DependencyService.Get<IToastServices>();

        private string _loggedInUserName;
        public string LoggedInUserName
        {
            get => _loggedInUserName;
            set => SetProperty(ref _loggedInUserName, value);
        }
        private string _salonName;
        public string SalonName
        {
            get => _salonName;
            set => SetProperty(ref _salonName, value);
        }
        public static Action<int> BadgeCountAction;
        public static void OnBadgeCountAction(int param)
        {
            if (BadgeCountAction != null)
            {
                BadgeCountAction(param);
            }
        }
        public static Action RemoveScanner;
        public static void OnRemoveScanner()
        {
            RemoveScanner?.Invoke();
        }
        private IDatabaseServices DatabaseServices => DependencyService.Get<IDatabaseServices>();
        private Salon Salon = new Salon();
        private ObservableCollection<CompletedHealthPlan> _completedHealthPlansCollection = new ObservableCollection<CompletedHealthPlan>();
        public ObservableCollection<CompletedHealthPlan> CompletedHealthPlansCollection
        {
            get => _completedHealthPlansCollection;
            set => SetProperty(ref _completedHealthPlansCollection, value);
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        private string _amountPaid = "0.00";
        public string AmountPaid
        {
            get => _amountPaid;
            set => SetProperty(ref _amountPaid, value);
        }

        private string _amountToBePaid = "0.00";
        public string AmountToBePaid
        {
            get => _amountToBePaid;
            set => SetProperty(ref _amountToBePaid, value);
        }

        private string _totalLoyaltyPoints;
        public string TotalLoyaltyPoints
        {
            get => _totalLoyaltyPoints;
            set => SetProperty(ref _totalLoyaltyPoints, value);
        }

        private ObservableCollection<StarRatings> _starRatingsCollection = new ObservableCollection<StarRatings>();
        public ObservableCollection<StarRatings> StarRatingsCollection
        {
            get => _starRatingsCollection;
            set => SetProperty(ref _starRatingsCollection, value);
        }
        private bool _isContentVisible = false;
        public bool IsContentVisible
        {
            get => _isContentVisible;
            set => SetProperty(ref _isContentVisible, value);
        }

        private bool _canPlaceOrder=true;
        public bool CanPlaceOrder
        {
            get => _canPlaceOrder;
            set => SetProperty(ref _canPlaceOrder, value);
        }

        public ICommand PlaceOrderCommand => new Command(() => PlaceOrderAsync());

        public ICommand SignOutCommand => new Command(() =>
        {
            try
            {
                Task.Run(async () =>
                {
                    SecureStorage.RemoveAll();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new AnimationNavigationPage(new SignInPage());
                    });
                });

            }
            catch (Exception)
            {
            }
        });
        public CompletedHealthPlanPageViewModel()
        {
            GetItemsAsync();
            SalonName = App.SalonName;
        }

        private void CloseActivities()
        {
            Device.BeginInvokeOnMainThread(() => ExceptionHandler(() =>
            {
                CompletedHealthPlansCollection.ForEach(a => a.IsScanning = false);

            }));
        }

        private async void GetItemsAsync()
        {
            IsBusy = true;
            await Task.Delay(1000);
            IsContentVisible = true;
            Device.BeginInvokeOnMainThread(() => ExceptionHandler(async () =>
            {
                LoggedInUserName = await SecureStorage.GetAsync("currentUserName");
                var basket = await DatabaseServices.Get<CustomBasket>("basketItems" + Settings.ClientId);
                var salon = await DatabaseServices.Get<Salon>("salon");
                if (basket != null && basket.ItemsCollection.Count > 0)
                {
                    AmountToBePaid = $"{basket.ItemsCollection.Sum(a => a.Variant.Price).ToString("F2")}";
                    BadgeCountAction?.Invoke(basket.ItemsCollection.Count);
                    var healthPlansCompleted = await DatabaseServices.Get<List<HealthPlanToComplete>>("healthPlanCompleted");
                    if (healthPlansCompleted?.Count > 0)
                    {
                        CanPlaceOrder = true;
                    }
                    foreach (var item in basket.Basket.Items)
                    {
                        foreach (var productCategory in salon.ProductCategories)
                        {
                            var products = productCategory.Products.Where(a => a.Id == item.ProductId).ToList();
                            if (products.Count > 0)
                            {
                                foreach (var product in products)
                                {
                                    var completedHealthPlan = new CompletedHealthPlan();

                                    if (healthPlansCompleted.Count > 0 && healthPlansCompleted.Count(a => a.Product.Id == product.Id) > 0)
                                    {
                                        var healthPlan = healthPlansCompleted.First(a => a.Product.Id == product.Id);
                                        completedHealthPlan.HealthPlanToComplete = new HealthPlanToComplete();
                                        completedHealthPlan.HealthPlanToComplete.ProgramName = healthPlan.ProgramName;
                                        completedHealthPlan.HealthPlanToComplete.Product = healthPlan.Product;
                                        completedHealthPlan.HealthPlanToComplete.VariantsList = new ObservableCollection<ProductVariant>();
                                        completedHealthPlan.HealthPlanToComplete.ShouldShowVariant = completedHealthPlan.IsDropdownVisible = healthPlan.Product.Variants?.Count > 0;
                                        foreach (var variant in healthPlan.VariantsList)
                                        {
                                            completedHealthPlan.HealthPlanToComplete.VariantsList.Add(variant);
                                        }
                                        completedHealthPlan.HealthPlanToComplete.SelectedVariant = product.Variants.FirstOrDefault(a => a.Id == healthPlan.SelectedVariant.Id);
                                        completedHealthPlan.IsProductScanned = healthPlan.ProductScanned;
                                        completedHealthPlan.LoyalityPoints = healthPlan.SelectedVariant.LoyaltyPoints;
                                        completedHealthPlan.HealthPlanToComplete.ShouldShowSubVariant = healthPlan.PrescribingOptions?.Count > 0;
                                        if (healthPlan.ShouldShowSubVariant)
                                        {
                                            completedHealthPlan.HealthPlanToComplete.PrescribingOptions = new ObservableCollection<ProductVariantPrescribingOption>();
                                            foreach (var option in healthPlan.SelectedVariant.PrescribingOptions)
                                            {
                                                completedHealthPlan.HealthPlanToComplete.PrescribingOptions.Add(option);
                                            }
                                            completedHealthPlan.HealthPlanToComplete.SelectedOption = completedHealthPlan.HealthPlanToComplete.PrescribingOptions.First(a => a.Title.ToLower() == healthPlan.SelectedOption.Title.ToLower());
                                        }

                                        if (product.ImageUrls?.Count > 0)
                                        {
                                            completedHealthPlan.HealthPlanToComplete.ImageUrl = healthPlan.ImageUrl;
                                        }

                                    }
                                    else
                                    {
                                        if (completedHealthPlan.HealthPlanToComplete == null) completedHealthPlan.HealthPlanToComplete = new HealthPlanToComplete();
                                        completedHealthPlan.HealthPlanToComplete.ProgramName = productCategory.Subtitle;
                                        completedHealthPlan.HealthPlanToComplete.Product = product;
                                        completedHealthPlan.HealthPlanToComplete.VariantsList = new ObservableCollection<ProductVariant>();
                                        completedHealthPlan.HealthPlanToComplete.ShouldShowVariant = completedHealthPlan.IsDropdownVisible = product.Variants?.Count > 0;
                                        foreach (var variant in product.Variants)
                                        {
                                            completedHealthPlan.HealthPlanToComplete.VariantsList.Add(variant);
                                        }
                                        completedHealthPlan.HealthPlanToComplete.SelectedVariant = product.Variants.FirstOrDefault(a => a.Id == item.VariantId);
                                        completedHealthPlan.LoyalityPoints = completedHealthPlan.HealthPlanToComplete.SelectedVariant.LoyaltyPoints;
                                        completedHealthPlan.HealthPlanToComplete.ShouldShowSubVariant = completedHealthPlan.HealthPlanToComplete.SelectedVariant?.PrescribingOptions?.Count > 0;
                                        if (completedHealthPlan.HealthPlanToComplete.ShouldShowSubVariant)
                                        {
                                            completedHealthPlan.HealthPlanToComplete.PrescribingOptions = new ObservableCollection<ProductVariantPrescribingOption>();
                                            foreach (var option in completedHealthPlan.HealthPlanToComplete.SelectedVariant.PrescribingOptions)
                                            {
                                                completedHealthPlan.HealthPlanToComplete.PrescribingOptions.Add(option);
                                            }
                                            completedHealthPlan.HealthPlanToComplete.SelectedOption = completedHealthPlan.HealthPlanToComplete.PrescribingOptions.First(a => a.Title.ToLower() == item.PrescribingOption.ToLower());
                                        }

                                        if (product.ImageUrls?.Count > 0)
                                        {
                                            completedHealthPlan.HealthPlanToComplete.ImageUrl = new Uri(product.ImageUrls.FirstOrDefault());
                                        }
                                    }




                                    completedHealthPlan.CloseOtherScanExceptthisCommand = new Command<Product>((param) => CloseOtherScanExceptthis(param));
                                    completedHealthPlan.ProductScannedCommand = new Command<ZXing.Result>((param) => AddLoyalityPoints(param));
                                    completedHealthPlan.RemoveLoyaltyCommand = new Command<Product>((param) => RemoveLoyalityPoints(param));
                                    CompletedHealthPlansCollection.Add(completedHealthPlan);
                                }
                            }
                        }
                    }
                    var sumOfLoyaltyPoits = CompletedHealthPlansCollection.Where(a => a.IsProductScanned).Sum(a => a.HealthPlanToComplete.SelectedVariant.LoyaltyPoints);

                    TotalLoyaltyPoints = sumOfLoyaltyPoits == 0 ? "" : $"+ {sumOfLoyaltyPoits}";
                    AmountPaid = $"{CompletedHealthPlansCollection.Where(x => x.IsProductScanned).Sum(a => a.HealthPlanToComplete.SelectedVariant.Price).ToString("F2")}";
                    foreach (var item in CompletedHealthPlansCollection.Where(a => a.IsProductScanned))
                    {
                        StarRatingsCollection.Add(new StarRatings
                        {
                            Star = "star.png",
                            WidthRequest = 14,
                            HeightRequest = 14,
                            Margin = new Thickness(0, -2, 0, 0)
                        });
                    }
                    foreach (var item in CompletedHealthPlansCollection.Where(a => !a.IsProductScanned))
                    {
                        StarRatingsCollection.Add(new StarRatings
                        {
                            Star = "icon_white_circle.png",
                            WidthRequest = 8,
                            HeightRequest = 8,
                            Margin = new Thickness(0, 0, 0, 0)
                        });
                    }
                }
            }));
        }

        internal void SaveCurrentState()
        {
            try
            {
                Task.Run(() => ExceptionHandler(async () =>
                {
                    var healthPlansCompleted = await DatabaseServices.Get<List<HealthPlanToComplete>>("healthPlanCompleted");
                    healthPlansCompleted = new List<HealthPlanToComplete>();
                    foreach (var item in CompletedHealthPlansCollection)
                    {
                        healthPlansCompleted.Add(item.HealthPlanToComplete);
                    }

                    await DatabaseServices.InsertData<List<HealthPlanToComplete>>("healthPlanCompleted", healthPlansCompleted);
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void RemoveLoyalityPoints(Product param)
        {
            Device.BeginInvokeOnMainThread(() => ExceptionHandler(async () =>
            {
                if (param != null)
                {
                    var basket = await DatabaseServices.Get<CustomBasket>("basketItems" + Settings.ClientId);
                    if (basket != null && basket.Basket != null && basket.Basket.Items != null)
                    {
                        CompletedHealthPlansCollection.Where(a => !a.IsProductScanned).ForEach(a => a.HealthPlanToComplete.ProductScanned = false);
                        var sumOfLoyaltyPoits = CompletedHealthPlansCollection.Where(a => a.IsProductScanned).Sum(a => a.HealthPlanToComplete.SelectedVariant.LoyaltyPoints);
                        TotalLoyaltyPoints = sumOfLoyaltyPoits == 0 ? "" : $"+ {sumOfLoyaltyPoits}";
                        AmountPaid = $"{CompletedHealthPlansCollection.Where(x => x.IsProductScanned).Sum(a => a.HealthPlanToComplete.SelectedVariant.Price).ToString("F2")}";
                        StarRatingsCollection = new ObservableCollection<StarRatings>();
                        foreach (var item in CompletedHealthPlansCollection.Where(a => a.IsProductScanned))
                        {
                            StarRatingsCollection.Add(new StarRatings
                            {
                                Star = "star.png",
                                WidthRequest = 14,
                                HeightRequest = 14,
                                Margin = new Thickness(0, -2, 0, 0)
                            });
                        }
                        foreach (var item in CompletedHealthPlansCollection.Where(a => !a.IsProductScanned))
                        {
                            StarRatingsCollection.Add(new StarRatings
                            {
                                Star = "icon_white_circle.png",
                                WidthRequest = 8,
                                HeightRequest = 8,
                                Margin = new Thickness(0, 0, 0, 0)
                            });
                        }
                    }
                }

                var healthPlansCompleted = await DatabaseServices.Get<List<HealthPlanToComplete>>("healthPlanCompleted");
                if (healthPlansCompleted != null && healthPlansCompleted.Count > 0)
                {
                    var currentCompletedHealthPlan = CompletedHealthPlansCollection.FirstOrDefault(a => a.HealthPlanToComplete.Product.Id == param.Id);
                    if (currentCompletedHealthPlan != null)
                    {
                        if (healthPlansCompleted.FirstOrDefault(a => a.Product.Id == currentCompletedHealthPlan.HealthPlanToComplete.Product.Id) != null)
                        {
                            healthPlansCompleted.Remove(healthPlansCompleted.FirstOrDefault(a => a.Product.Id == currentCompletedHealthPlan.HealthPlanToComplete.Product.Id));
                        }

                    }
                }

                await DatabaseServices.InsertData<List<HealthPlanToComplete>>("healthPlanCompleted", healthPlansCompleted);
            }));

            Task.Run(() => ExceptionHandler(async () =>
            {
                var healthPlansCompleted = await DatabaseServices.Get<List<HealthPlanToComplete>>("healthPlanCompleted");
                var currentCompletedHealthPlan = CompletedHealthPlansCollection.FirstOrDefault(a => a.HealthPlanToComplete.Product.Id == _selectedProduct.Id);
                if (currentCompletedHealthPlan != null)
                {
                    if (healthPlansCompleted.Count(a => a.Product.Id == currentCompletedHealthPlan.HealthPlanToComplete.Product.Id) > 0)
                    {
                        healthPlansCompleted.Remove(currentCompletedHealthPlan.HealthPlanToComplete);
                    }

                    await DatabaseServices.InsertData<List<HealthPlanToComplete>>("healthPlanCompleted", healthPlansCompleted);
                }

            }));
        }

        private void AddLoyalityPoints(ZXing.Result param)
        {
            Device.BeginInvokeOnMainThread(() => ExceptionHandler(async () =>
            {
                var basket = await DatabaseServices.Get<CustomBasket>("basketItems" + Settings.ClientId);
                if (basket != null && basket.Basket != null && basket.Basket.Items != null)
                {
                    //CompletedHealthPlansCollection.Where(a => a.HealthPlanToComplete.Product.Id == basket.Basket.Items.First(x => x.ProductId == _selectedProduct.Id).ProductId).ForEach(x =>
                    //     {
                    //         var selectedVariant = basket.Basket.Items.First(t => t.ProductId == _selectedProduct.Id).VariantId;
                    //         var variant = _selectedProduct.Variants.First(temp => temp.Id == selectedVariant);
                    //         x.LoyalityPoints = variant.LoyaltyPoints;
                    //     });
                    CompletedHealthPlansCollection.Where(a => a.IsProductScanned).ForEach(a => a.HealthPlanToComplete.ProductScanned = true);
                    TotalLoyaltyPoints = $"+ {CompletedHealthPlansCollection.Where(a => a.IsProductScanned).Sum(a => a.HealthPlanToComplete.SelectedVariant.LoyaltyPoints)}";
                    AmountPaid = $"{CompletedHealthPlansCollection.Where(x => x.IsProductScanned).Sum(a => a.HealthPlanToComplete.SelectedVariant.Price).ToString("F2")}";
                    StarRatingsCollection = new ObservableCollection<StarRatings>();
                    foreach (var item in CompletedHealthPlansCollection.Where(a => a.IsProductScanned))
                    {
                        StarRatingsCollection.Add(new StarRatings
                        {
                            Star = "star.png",
                            WidthRequest = 14,
                            HeightRequest = 14,
                            Margin = new Thickness(0, -2, 0, 0)
                        });
                    }
                    foreach (var item in CompletedHealthPlansCollection.Where(a => !a.IsProductScanned))
                    {
                        StarRatingsCollection.Add(new StarRatings
                        {
                            Star = "icon_white_circle.png",
                            WidthRequest = 8,
                            HeightRequest = 8,
                            Margin = new Thickness(0, 0, 0, 0)
                        });
                    }
                }
                var healthPlansCompleted = await DatabaseServices.Get<List<HealthPlanToComplete>>("healthPlanCompleted");
                if (healthPlansCompleted == null || healthPlansCompleted.Count == 0)
                {
                    healthPlansCompleted = new List<HealthPlanToComplete>();
                    var currentCompletedHealthPlan = CompletedHealthPlansCollection.FirstOrDefault(a => a.HealthPlanToComplete.Product.Id == _selectedProduct.Id);
                    if (currentCompletedHealthPlan != null)
                    {
                        healthPlansCompleted.Add(currentCompletedHealthPlan.HealthPlanToComplete);
                    }
                }
                else
                {
                    var currentCompletedHealthPlan = CompletedHealthPlansCollection.FirstOrDefault(a => a.HealthPlanToComplete.Product.Id == _selectedProduct.Id);
                    if (currentCompletedHealthPlan != null)
                    {
                        if (healthPlansCompleted.FirstOrDefault(a => a.Product.Id == currentCompletedHealthPlan.HealthPlanToComplete.Product.Id) != null)
                        {
                            healthPlansCompleted.Remove(healthPlansCompleted.FirstOrDefault(a => a.Product.Id == currentCompletedHealthPlan.HealthPlanToComplete.Product.Id));
                        }

                        healthPlansCompleted.Add(currentCompletedHealthPlan.HealthPlanToComplete);
                    }
                }
                await DatabaseServices.InsertData<List<HealthPlanToComplete>>("healthPlanCompleted", healthPlansCompleted);
            }));
        }

        private Product _selectedProduct = new Product();
        private void CloseOtherScanExceptthis(Product param)
        {
            _selectedProduct = param;
            Device.BeginInvokeOnMainThread(() => ExceptionHandler(() =>
            {
                CompletedHealthPlansCollection.Where(a => a.HealthPlanToComplete.Product.Id != param.Id).ForEach(x => x.IsScanning = false);
            }));
        }

        private async void PlaceOrderAsync()
        {
            try
            {

                var consumer = await DatabaseServices.Get<Consumer>("current_consumer"+Settings.CurrentTherapistId);
                if (consumer != null && consumer.Id != Guid.Empty)
                {
                    var basket = await DatabaseServices.Get<CustomBasket>("basketItems" + Settings.ClientId);
                    if (basket != null && basket.Basket != null && basket.Basket.Items?.Count > 0)
                    {
                        var currentTherapistJson = await SecureStorage.GetAsync("currentTherapist");
                        var currentTherapist = JsonConvert.DeserializeObject<Therapist>(currentTherapistJson);
                        var saloConsumer = new SalonConsumer
                        {
                            Id = consumer.Id,
                            Firstname = consumer.Firstname,
                            Lastname = consumer.Lastname,
                            Email = consumer.Email,
                            Mobile = consumer.Mobile,
                            TherapistId = currentTherapist.Id,
                            DateOfBirth=consumer.DateOfBirth,
                            CurrentConsultation = new Consultation
                            {
                                Id = Guid.NewGuid(),
                                Basket = new Basket
                                {
                                    Id = basket.Basket.Id,
                                    Items = CompletedHealthPlansCollection.Select(a => new BasketItem
                                    {
                                        ProductId = a.HealthPlanToComplete.Product.Id,
                                        VariantId = a.HealthPlanToComplete.SelectedVariant.Id,
                                        PrescribingOption = a.HealthPlanToComplete.SelectedOption != null? a.HealthPlanToComplete.SelectedOption.Title:""
                                    }).ToList()
                                },
                                HealthPlan = new Basket
                                {
                                    Id = Guid.NewGuid(),
                                    Items = new List<IIAADataModels.Transfer.BasketItem>()
                                }
                            }
                        };

                        var isCompleted = await ApiServices.Client.PostAsync<bool>("salon/consumer/consultation/finalise", saloConsumer);
                        if (isCompleted)
                        {
                            ToastServices.ShowToast("Order has been placed successfully");
                            CanPlaceOrder = false;
                        }
                        else
                        {
                            ToastServices.ShowToast("Order failed");
                        }
                    }

                }
            }
            catch (Exception)
            {
                ToastServices.ShowToast("Something went wrong.Please try again");
            }
        }

        private void ExceptionHandler(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
