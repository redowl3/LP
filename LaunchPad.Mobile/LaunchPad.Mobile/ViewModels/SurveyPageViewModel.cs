using FormsControls.Base;
using IIAADataModels.Transfer;
using IIAADataModels.Transfer.Survey;
using LaunchPad.Client;
using LaunchPad.Mobile.Helpers;
using LaunchPad.Mobile.Services;
using LaunchPad.Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace LaunchPad.Mobile.ViewModels
{
    public class SurveyPageViewModel : ViewModelBase
    {
        public static Action<string> AddChildren;
        public static void onAddChildren(string pageName)
        {
            AddChildren?.Invoke(pageName);
        }
        
        public static Action GoBack;
        public static void onGoBack()
        {
            GoBack?.Invoke();
        }

        
        private Guid CurrentSelectedSurveyGuid { get; set; }
        private Guid CurrentSelectedSurveyPageGuid { get; set; }
        private IDatabaseServices DatabaseServices => DependencyService.Get<IDatabaseServices>();

        private int Counter { get; set; }
        private int MaxCounter { get; set; }
        private ObservableCollection<CustomForm> _surveyCollection;
        public ObservableCollection<CustomForm> SurveyCollection
        {
            get => _surveyCollection;
            set => SetProperty(ref _surveyCollection, value);
        }

        private ObservableCollection<CustomPage> _pages;
        public ObservableCollection<CustomPage> Pages
        {
            get => _pages;
            set => SetProperty(ref _pages, value);
        }
        private ObservableCollection<CustomFormQuestion> _questions = new ObservableCollection<CustomFormQuestion>();
        public ObservableCollection<CustomFormQuestion> Questions
        {
            get => _questions;
            set => SetProperty(ref _questions, value);
        }
        private List<IndexModel> _surveyIndexList = new List<IndexModel>();
        public List<IndexModel> SurveyIndexList
        {
            get => _surveyIndexList;
            set => SetProperty(ref _surveyIndexList, value);
        }
        private ObservableCollection<IndexedQuestions> _concernAndSkinCareQuestions = new ObservableCollection<IndexedQuestions>();
        public ObservableCollection<IndexedQuestions> ConcernAndSkinCareQuestions
        {
            get => _concernAndSkinCareQuestions;
            set => SetProperty(ref _concernAndSkinCareQuestions, value);
        }
        private ObservableCollection<IndexedQuestions> _healthQuestionsCollection = new ObservableCollection<IndexedQuestions>();
        public ObservableCollection<IndexedQuestions> HealthQuestionsCollection
        {
            get => _healthQuestionsCollection;
            set => SetProperty(ref _healthQuestionsCollection, value);
        }
        private ObservableCollection<IndexedQuestions> _lifeStylesQuestions = new ObservableCollection<IndexedQuestions>();
        public ObservableCollection<IndexedQuestions> LifeStylesQuestions
        {
            get => _lifeStylesQuestions;
            set => SetProperty(ref _lifeStylesQuestions, value);
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        private bool _isContentLoading = true;
        public bool IsContentLoading
        {
            get => _isContentLoading;
            set => SetProperty(ref _isContentLoading, value);
        }
        private bool _canNext = true;
        public bool CanNext
        {
            get => _canNext;
            set => SetProperty(ref _canNext, value);
        }

        private bool _canFinish = true;
        public bool CanFinish
        {
            get => _canFinish;
            set => SetProperty(ref _canFinish, value);
        }

        private bool _concernPage;
        public bool ConcernPage
        {
            get => _concernPage;
            set => SetProperty(ref _concernPage, value);
        }

        private bool _healthQUestions;
        public bool HealthQuestions
        {
            get => _healthQUestions;
            set => SetProperty(ref _healthQUestions, value);
        }

        private bool _lifeStyles;
        public bool LifeStyles
        {
            get => _lifeStyles;
            set => SetProperty(ref _lifeStyles, value);
        }

        private FlexBasis _basis = new FlexBasis(1f, true);
        public FlexBasis Basis
        {
            get => _basis;
            set => SetProperty(ref _basis, value);
        }
        public ICommand FinishCommand => new Command(() =>
        {
            Application.Current.MainPage = new AnimationNavigationPage(new SalonProductsPage());
        });
        public ICommand GoBackCommand => new Command(() =>
        {
            try
            {
                GoBack?.Invoke();
            }
            catch (Exception)
            {
            }
        });
        public ICommand HomeCommand => new Command(() =>
        {
            try
            {
                Task.Run(() =>
                {
                    SecureStorage.RemoveAll();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new AnimationNavigationPage(new SalonClientsPage());
                    });
                });

            }
            catch (Exception)
            {
            }
        });
        public ICommand SignOutCommand => new Command(() =>
        {
            try
            {
                Task.Run(() =>
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
        public ICommand NextQuestionPageCommand => new Command(() =>
        {
            try
            {
                if (Counter <= MaxCounter)
                {
                    ++Counter;
                    var next = SurveyIndexList[Counter];
                    if (next.PageTitle.ToLower() == "concerns + skin type")
                    {
                        ConcernPage = true;
                        HealthQuestions = false;
                        LifeStyles = false;
                        if (next.PageIndex > 0)
                        {
                            ConcernAndSkinCareQuestions[next.PageIndex - 1].IsSelected = false;
                        }

                        ConcernAndSkinCareQuestions[Counter].IsSelected = true;
                        if (ConcernAndSkinCareQuestions[next.PageIndex].Questions.Count == 3)
                        {
                            Basis = new FlexBasis(0.33f, true);
                        }
                        else if (ConcernAndSkinCareQuestions[next.PageIndex].Questions.Count == 2)
                        {
                            Basis = new FlexBasis(0.5f, true);
                        }
                        else
                        {
                            Basis = new FlexBasis(1f, true);
                        }
                    }
                    else if (next.PageTitle.ToLower() == "health questions")
                    {
                        ConcernPage = false;
                        HealthQuestions = true;
                        LifeStyles = false;
                        if (next.PageIndex > 0)
                        {
                            ConcernAndSkinCareQuestions[next.PageIndex - 1].IsSelected = false;
                        }
                        HealthQuestionsCollection[next.PageIndex].IsSelected = true;
                    }
                    else if (next.PageTitle.ToLower() == "you + your lifestyle")
                    {
                        ConcernPage = false;
                        HealthQuestions = false;
                        LifeStyles = true;
                        if (next.PageIndex > 0)
                        {
                            LifeStylesQuestions[next.PageIndex - 1].IsSelected = false;
                        }
                        if (LifeStylesQuestions[next.PageIndex].Questions.Count == 3)
                        {
                            Basis = new FlexBasis(0.33f, true);
                        }
                        else if (LifeStylesQuestions[next.PageIndex].Questions.Count == 2)
                        {
                            Basis = new FlexBasis(0.5f, true);
                        }
                        else
                        {
                            Basis = new FlexBasis(1f, true);
                        }
                        LifeStylesQuestions[next.PageIndex].IsSelected = true;
                    }
                    else
                    {
                        CanNext = false;
                        CanFinish = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        });
        public SurveyPageViewModel()
        {
            //GetSurveyDataAsync();
            CanNext = true;
            CanFinish = false;
        }
        public async Task GetSurveyDataAsync()
        {
            IsBusy = true;
            try
            {
                if (SurveyCollection == null || SurveyCollection.Count == 0)
                {
                    var salon = await DatabaseServices.Get<Salon>("salon");
                    if (salon == null || salon.Id == Guid.Empty)
                    {
                        salon = await ApiServices.Client.GetAsync<Salon>("salon");
                        await DatabaseServices.InsertData("salon", salon);
                    }

                    if (salon.Surveys?.Count > 0)
                    {
                        SurveyCollection = new ObservableCollection<CustomForm>(salon.Surveys.Where(a => a.Title.ToLower() != "test form").Select(a => new CustomForm
                        {
                            Form = a,
                            SurveySelectedCommand = new Command<CustomForm>((param) =>
                            {
                                SurveyCollection.Where(s => s.Form.Id != param.Form.Id).ForEach(sa => sa.IsSelected = false);
                                SurveySelected(param);
                            })
                        }));

                        //await Task.WhenAll(GetConcernsQuestionsAsync(), GetHealthQuestions(), GetLifeStylesQuestions());

                    }
                }

                Console.WriteLine("App Exited from GetSurveyData Method at : " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            IsContentLoading = false;
        }
        public async Task SelectSurveyAsync()
        {
            try
            {
                Counter = 0;
                MaxCounter = SurveyIndexList.Count - 1;
                if (SurveyCollection.Count > 0)
                {
                    SurveyCollection[0].SelectedCommand.Execute(SurveyCollection[0]);
                }
            }
            catch (Exception)
            {

            }
        }
        public async Task<bool> GetConcernsQuestionsAsync()
        {
            try
            {
                foreach (var survey in SurveyCollection.Where(s => s.Form.Title.ToLower() == "concerns + skin type"))
                {
                    foreach (var page in survey.Form.Pages)
                    {
                        var questions = await DatabaseServices.Get<List<CustomFormQuestion>>("survey_page" + page.Id + "_" + survey.Form.Id);
                        if (questions?.Count > 0)
                        {
                            if (survey.Form.Title.ToLower() == "concerns + skin type")
                            {
                                ConcernAndSkinCareQuestions.Add(new IndexedQuestions
                                {
                                    PageGuid = page.Id,
                                    SurveyGuid = survey.Form.Id,
                                    Questions = new ObservableCollection<CustomFormQuestion>(questions)
                                });

                                SurveyIndexList.Add(new IndexModel
                                {
                                    SurveyId = survey.Form.Id,
                                    PageIndex = ConcernAndSkinCareQuestions.Count - 1,
                                    PageTitle = survey.Form.Title
                                });
                            }
                        }
                    }
                }

                return ConcernAndSkinCareQuestions.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public async Task<bool> GetLifeStylesQuestions()
        {
            try
            {
                foreach (var survey in SurveyCollection.Where(s => s.Form.Title.ToLower() == "you + your lifestyle"))
                {
                    foreach (var page in survey.Form.Pages)
                    {
                        var questions = await DatabaseServices.Get<List<CustomFormQuestion>>("survey_page" + page.Id + "_" + survey.Form.Id);
                        if (questions?.Count > 0)
                        {
                            if (survey.Form.Title.ToLower() == "you + your lifestyle")
                            {
                                LifeStylesQuestions.Add(new IndexedQuestions
                                {
                                    PageGuid = page.Id,
                                    SurveyGuid = survey.Form.Id,
                                    Questions = new ObservableCollection<CustomFormQuestion>(questions)
                                });
                                SurveyIndexList.Add(new IndexModel
                                {
                                    SurveyId = survey.Form.Id,
                                    PageIndex = LifeStylesQuestions.Count - 1,
                                    PageTitle = survey.Form.Title
                                });
                            }
                        }
                    }
                }

                return LifeStylesQuestions.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> GetHealthQuestions()
        {
            try
            {
                foreach (var survey in SurveyCollection.Where(s => s.Form.Title.ToLower() == "health questions"))
                {
                    foreach (var page in survey.Form.Pages)
                    {
                        var questions = await DatabaseServices.Get<List<CustomFormQuestion>>("survey_page" + page.Id + "_" + survey.Form.Id);
                        if (questions?.Count > 0)
                        {
                            if (survey.Form.Title.ToLower() == "health questions")
                            {
                                HealthQuestionsCollection.Add(new IndexedQuestions
                                {
                                    PageGuid = page.Id,
                                    SurveyGuid = survey.Form.Id,
                                    Questions = new ObservableCollection<CustomFormQuestion>(questions)
                                });
                                SurveyIndexList.Add(new IndexModel
                                {
                                    SurveyId = survey.Form.Id,
                                    PageIndex = HealthQuestionsCollection.Count - 1,
                                    PageTitle = survey.Form.Title
                                });
                            }
                        }
                    }
                }
                return HealthQuestionsCollection.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void SurveySelected(CustomForm param)
        {
            try
            {
                Counter = 0;
                if (param != null)
                {
                    CurrentSelectedSurveyGuid = param.Form.Id;
                    ConcernPage = param.Form.Title.ToLower() == "concerns + skin type";
                    HealthQuestions = param.Form.Title.ToLower() == "health questions";
                    LifeStyles = param.Form.Title.ToLower() == "you + your lifestyle";
                  
                    //if (param.Form.Title.ToLower() == "concerns + skin type")
                    //{
                    //    ConcernAndSkinCareQuestions.ForEach(x => x.IsSelected = false);
                    //    ConcernAndSkinCareQuestions[0].IsSelected = true;
                    //    if (ConcernAndSkinCareQuestions[0].Questions.Count == 3)
                    //    {
                    //        Basis = new FlexBasis(0.33f, true);
                    //    }
                    //    else if (ConcernAndSkinCareQuestions[0].Questions.Count == 2)
                    //    {
                    //        Basis = new FlexBasis(0.5f, true);
                    //    }
                    //    else
                    //    {
                    //        Basis = new FlexBasis(1f, true);
                    //    }
                    //    AddChildren?.Invoke("ConcernPage");
                    //}
                    //if (param.Form.Title.ToLower() == "health questions")
                    //{
                    //    HealthQuestions = true;
                    //    HealthQuestionsCollection.ForEach(x => x.IsSelected = false);
                    //    HealthQuestionsCollection[0].IsSelected = true;
                    //    AddChildren?.Invoke("HealthQuestionPage");
                    //}
                    //if (param.Form.Title.ToLower() == "you + your lifestyle")
                    //{
                    //    LifeStylesQuestions.ForEach(x => x.IsSelected = false);
                    //    LifeStylesQuestions[0].IsSelected = true;
                    //    AddChildren?.Invoke("LifeStylePage");
                    //    if (LifeStylesQuestions[0].Questions.Count == 3)
                    //    {
                    //        Basis = new FlexBasis(0.33f, true);
                    //    }
                    //    else if (LifeStylesQuestions[0].Questions.Count == 2)
                    //    {
                    //        Basis = new FlexBasis(0.5f, true);
                    //    }
                    //    else
                    //    {
                    //        Basis = new FlexBasis(1f, true);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    public class CustomForm : ViewModelBase
    {
        public Form Form { get; set; }
        public bool ShouldCenteredText { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public ICommand SelectedCommand => new Command<CustomForm>((param) =>
        {
            IsSelected = true;
            SurveySelectedCommand.Execute(param);
        });

        public ICommand SurveySelectedCommand { get; set; }
    }
    public class CustomPage : ViewModelBase
    {
        public Guid SelectedSurveyId { get; set; }
        public FormPage Page { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public ICommand SelectedCommand => new Command<CustomPage>((param) =>
          {
              IsSelected = true;
              PageSelectedCommand.Execute(param);
          });

        public ICommand PageSelectedCommand { get; set; }
    }

    public class CustomFormQuestion
    {
        public Guid FormId { get; set; }
        public int Version { get; set; }
        public string QuestionGuid { get; set; }
        public bool ConcernPage { get; set; }
        public bool HealthQuestions { get; set; }
        public bool LifeStyles { get; set; }
        public bool Other { get; set; }
        public FlexBasis Basis { get; set; }
        public Guid PageGuid { get; set; }
        public FormQuestionData PageFormQuestionData { get; set; }
        public FormQuestion FormQuestion { get; set; }
        public FormQuestionData FormQuestionData { get; set; }
        public List<CustomFormQuestion> ChildQuestions { get; set; }
        public bool IsTextBox { get; set; }
        public bool IsTextArea { get; set; }
        public bool IsRadio { get; set; }
        public bool IsCheck { get; set; }
        public bool IsCheckWithMultipleYesNo { get; set; }
        public List<CustomFormQuestion> YesNoQuestionsList { get; set; }
        public bool IsYesNo { get; set; }
        public bool IsYesNoWithNoTextArea { get; set; }
        public bool IsGroup { get; set; }
        public bool IsHtml { get; set; }
        public bool IsDropdown { get; set; }
        public bool IsNestedGroup { get; set; }
        public bool IsYesNoWithTextArea { get; set; }
        public bool IsImage { get; set; }
        public LayoutOptions HorizontalOption { get; set; }

        public string ConfigText { get; set; }
        public CustomFormQuestion()
        {
            ChildQuestions = new List<CustomFormQuestion>();
        }
    }

    public class IndexedQuestions : ViewModelBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public Guid PageGuid { get; set; }
        public Guid SurveyGuid { get; set; }
        public ObservableCollection<CustomFormQuestion> Questions { get; set; }

        public IndexedQuestions()
        {
            Questions = new ObservableCollection<CustomFormQuestion>();
        }
    }

    public class IndexModel
    {
        public Guid SurveyId { get; set; }
        public int PageIndex { get; set; }
        public string PageTitle { get; set; }
    }
    public class Answer:ViewModelBase
    {
        public string ResponseText { get; set; }
        public string ResponseValue { get; set; }

        private bool _isSelected;
        public bool Selected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public ICommand SelectCommand => new Command<List<Answer>>((param) =>
          {
              Selected = !Selected;
              param.Where(a => a.ResponseText.ToLower() != ResponseText.ToLower()).ForEach(a => a.Selected = false);
          });
        public string Id { get; set; }
        public List<CustomFormQuestion> SubOptionsList { get; set; }
    }

    public class CustomAnswer:Answer
    {
        public List<CustomFormQuestion> SubOptionsList { get; set; }
        public CustomAnswer()
        {
            SubOptionsList = new List<CustomFormQuestion>();
        }
    }
    public class FormQuestionData
    {
        public string QuestionText { get; set; }
        public string ImageUrl { get; set; }
        public string Condition { get; set; }
        public string conditionvalue { get; set; }
        public string visibility { get; set; }
        public List<Answer> Answers { get; set; }

        public FormQuestionData()
        {
            Answers = new List<Answer>();
        }
    }

    public class SurveySummary
    {
        public string QuestionGuid { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public string SubAnswerText { get; set; }
        public string ConfigAnswerText { get; set; }
    }
}
