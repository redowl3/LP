using FormsControls.Base;
using IIAADataModels.Transfer;
using LaunchPad.Client;
using LaunchPad.Mobile.Helpers;
using LaunchPad.Mobile.Models;
using LaunchPad.Mobile.Services;
using LaunchPad.Mobile.Views;
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
    public class UserHealthPlanPageViewModel : ViewModelBase
    {
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
        public static Action CloseDrawer;
        public static void OnCloseDrawer()
        {
            CloseDrawer?.Invoke();
        }
        public static Action<int> BadgeCountAction;
        public static void OnBadgeCountAction(int param)
        {
            if (BadgeCountAction != null)
            {
                BadgeCountAction(param);
            }
        }
        public static Action<bool> ShouldAnimateOut;
        public static void OnShouldAnimateOut(bool param)
        {
            ShouldAnimateOut?.Invoke(param);
        }
        private IDatabaseServices DatabaseServices => DependencyService.Get<IDatabaseServices>();
        private Salon Salon = new Salon();
        private ObservableCollection<HealthPlan> _healthPlanCollection = new ObservableCollection<HealthPlan>();
        public ObservableCollection<HealthPlan> HealthPlanCollection
        {
            get => _healthPlanCollection;
            set => SetProperty(ref _healthPlanCollection, value);
        }
        private int _basketItemsCount;
        public int BasketItemsCount
        {
            get => _basketItemsCount;
            set => SetProperty(ref _basketItemsCount, value);
        }
        private string _subTotal = $"£0.00";
        public string SubTotal
        {
            get => _subTotal;
            set => SetProperty(ref _subTotal, value);
        }
        private bool _shouldDisplayCollection;
        public bool ShouldDisplayCollection
        {
            get => _shouldDisplayCollection;
            set => SetProperty(ref _shouldDisplayCollection, value);
        }
        private ObservableCollection<CustomBasketInfo> _basketItemsCollection;
        public ObservableCollection<CustomBasketInfo> BasketItemsCollection
        {
            get => _basketItemsCollection;
            set => SetProperty(ref _basketItemsCollection, value);
        }
        private bool _isContentVisible = false;
        public bool IsContentVisible
        {
            get => _isContentVisible;
            set => SetProperty(ref _isContentVisible, value);
        }
        public ICommand ContinueCommand => new Command(() => ComplateHelathPLanAsync());
        public ICommand GoBackCommand => new Command(() => Application.Current.MainPage.Navigation.PopAsync());
        public ICommand HomeCommand => new Command(() => Application.Current.MainPage = new AnimationNavigationPage(new SalonClientsPage()));
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

        #region # Constructor #
        public UserHealthPlanPageViewModel()
        {
            GetHealthPlanAsync();
        }

        #endregion

        #region # Methods #
        internal async Task<int> RefreshBadgeCountAsync()
        {
            try
            {
                SalonName = App.SalonName;
                var cartItems = await DatabaseServices.Get<List<Product>>("healthplans"+Settings.ClientId);
                return cartItems.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
                return 0;
            }
        }
        private async void GetHealthPlanAsync()
        {
            try
            {
                await Task.Delay(1000);
                IsContentVisible = true;
                LoggedInUserName = await SecureStorage.GetAsync("currentUserName");
                var healthplans = await DatabaseServices.Get<List<Product>>("healthplans"+Settings.ClientId);
                if (healthplans.Count > 0)
                {
                    decimal sumTotal = 0;
                    if (Salon == null || Salon.Id == Guid.Empty)
                    {
                        Salon = await DatabaseServices.Get<Salon>("salon");
                        if (Salon == null || Salon.Id == Guid.Empty)
                        {
                            Salon = await ApiServices.Client.GetAsync<Salon>("salon");
                            await DatabaseServices.InsertData("salon", Salon);
                        }
                    }

                    var categoryList = Salon.ProductCategories.Select(a => a.Subtitle).ToList();

                    foreach (var productCategory in Salon.ProductCategories)
                    {
                        var healthPlan = new HealthPlan();
                        healthPlan.ProgramName = productCategory.Subtitle;
                        healthPlan.ProductWithLevelTypeList = new ObservableCollection<ProductWithLevelType>();
                        var groupByClassification = productCategory.Products.Where(a => healthplans.Count(x => x.Id == a.Id) > 0).GroupBy(a => a.Classification).ToList();
                        if (groupByClassification.Count > 0)
                        {
                            foreach (var item in groupByClassification)
                            {
                                var temp = new ProductWithLevelType
                                {
                                    Classification = item.Key,
                                    ProgramName = productCategory.Subtitle,
                                    Products = new ObservableCollection<CustomProduct>(item.Select(a => new CustomProduct
                                    {
                                        Product = a,
                                        ShouldShowVariant = a.Variants.Count > 0,
                                        VariantsList = new ObservableCollection<ProductVariant>(a.Variants),
                                        SelectedVariant = a.Variants.FirstOrDefault(),
                                        ImageUrl = new Uri(a.ImageUrls.First()),
                                        RefreshPriceCommand = new Command(() => RefreshPriceAsync()),
                                        PurchaseCommand = new Command<CustomProduct>((param) => AddOrUpdateBasketItemsAsync(healthPlan, param))
                                    }).ToList())
                                };

                                sumTotal = sumTotal + temp.Products.Sum(x => x.SelectedVariant.Price);
                                healthPlan.ProductWithLevelTypeList.Add(temp);
                            }

                            HealthPlanCollection.Add(healthPlan);
                        }

                        //SubTotal = $"£{sumTotal.ToString("F2")}";
                    }
                }


                var basket = await DatabaseServices.Get<CustomBasket>("basketItems"+Settings.ClientId);
                if (basket == null || string.IsNullOrEmpty(basket.Id))
                {
                    BasketItemsCount = 0;
                    SubTotal = $"£0.00";
                    BasketItemsCollection = new ObservableCollection<CustomBasketInfo>();
                    ShouldDisplayCollection = false;
                }
                else
                {
                    BasketItemsCount = basket.Basket.Items.Count;
                    SubTotal = $"£{basket.ItemsCollection.Sum(a => a.Variant.Price).ToString("F2")}";
                    basket.ItemsCollection.ForEach(a => a.RemoveCommand = new Command<CustomBasketInfo>((param) => RemoveItemFromBasketAsync(param)));
                    BasketItemsCollection = new ObservableCollection<CustomBasketInfo>(basket.ItemsCollection);
                    ShouldDisplayCollection = BasketItemsCount > 0;
                }
                // Program name

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "/n/n" + ex.StackTrace);
            }
        }


        private async void AddOrUpdateBasketItemsAsync(HealthPlan healthPlan, CustomProduct param)
        {
            try
            {
                var basket = await DatabaseServices.Get<CustomBasket>("basketItems"+Settings.ClientId);
                if (basket == null || string.IsNullOrEmpty(basket.Id))
                {
                    var basketItem = new BasketItem
                    {
                        ProductId = param.Product.Id,
                        PrescribingOption = param.SelectedOption?.Title,
                        VariantId = param.SelectedVariant.Id
                    };

                    basket = new CustomBasket
                    {
                        Id = Guid.NewGuid().ToString(),
                        Basket = new Basket
                        {
                            Id = Guid.NewGuid(),
                            Items = new List<BasketItem>
                            {
                                basketItem
                            }
                        },
                        ItemsCollection = new List<CustomBasketInfo>
                        {
                            new CustomBasketInfo
                            {
                                ProductId=param.Product.Id,
                                ProgramName = healthPlan.ProgramName,
                                ProductName = param.Product.Name,
                                Price = param.Price,
                                Variant=param.SelectedVariant
                            }
                        }
                    };


                    await DatabaseServices.InsertData("basketItems"+Settings.ClientId, basket);
                    var items = await DatabaseServices.Get<CustomBasket>("basketItems"+Settings.ClientId);
                }
                else
                {
                    if (basket.Basket.Items.Count(a => a.ProductId == param.Product.Id) > 0) return;
                    basket.Basket.Items?.Add(new BasketItem
                    {
                        ProductId = param.Product.Id,
                        PrescribingOption = param.SelectedOption?.Title,
                        VariantId = param.SelectedVariant.Id
                    });
                    basket.ItemsCollection?.Add(new CustomBasketInfo
                    {
                        ProductId = param.Product.Id,
                        ProgramName = healthPlan.ProgramName,
                        ProductName = param.Product.Name,
                        Price = param.Price,
                        Variant = param.SelectedVariant
                    });

                    await DatabaseServices.InsertData("basketItems"+Settings.ClientId, basket);
                }

                BasketItemsCount = basket.Basket.Items.Count;
                SubTotal = $"£{basket.ItemsCollection.Sum(a => a.Variant.Price).ToString("F2")}";
                basket.ItemsCollection.ForEach(a => a.RemoveCommand = new Command<CustomBasketInfo>((parameter) => RemoveItemFromBasketAsync(parameter)));
                BasketItemsCollection = new ObservableCollection<CustomBasketInfo>(basket.ItemsCollection);
                ShouldDisplayCollection = BasketItemsCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "/n/n" + ex.StackTrace);
            }

        }

        private void RefreshPriceAsync()
        {
            try
            {
                decimal sumTotal = 0;
                foreach (var item in HealthPlanCollection)
                {
                    foreach (var temp in item.ProductWithLevelTypeList)
                    {
                        sumTotal = sumTotal + temp.Products.Sum(a => a.SelectedVariant.Price);
                    }
                }

                //SubTotal = $"£{sumTotal.ToString("F2")}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void RemoveItemFromBasketAsync(CustomBasketInfo param)
        {
            try
            {
                ShouldAnimateOut?.Invoke(false);
                BasketItemsCollection.Remove(param);
                BasketItemsCount = BasketItemsCollection.Count;
                SubTotal = $"£{BasketItemsCollection.Sum(a => a.Variant.Price):F2}";
                ShouldDisplayCollection = BasketItemsCollection.Count > 0;
                Task.Run(() => ExceptionHandler(async () =>
                {
                    var basket = await DatabaseServices.Get<CustomBasket>("basketItems"+Settings.ClientId);
                    if (basket != null && basket.ItemsCollection.Count > 0)
                    {
                        basket.ItemsCollection.RemoveAll(a => a.ProductName.ToLower() == param.ProductName?.ToLower() && a.ProgramName.ToLower() == param.ProgramName?.ToLower());
                        basket.Basket.Items?.RemoveAll(a => a.ProductId == param.ProductId);
                        await DatabaseServices.InsertData("basketItems"+Settings.ClientId, basket);
                    }
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
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
        private async void ComplateHelathPLanAsync()
        {
            try
            {
                var basket = await DatabaseServices.Get<CustomBasket>("basketItems"+Settings.ClientId);
                if (basket != null && basket.ItemsCollection.Count > 0)
                {
                    await DatabaseServices.InsertData("CompletedHealthPlanItems", basket);
                    await Application.Current.MainPage.Navigation.PushAsync(new CompletedHealthPlanPage());
                    CompletedHealthPlanPageViewModel.BadgeCountAction?.Invoke(basket.ItemsCollection.Count);
                    CloseDrawer?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        #endregion
    }
}
