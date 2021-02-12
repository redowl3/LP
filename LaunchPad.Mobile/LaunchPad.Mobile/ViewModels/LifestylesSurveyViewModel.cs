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
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace LaunchPad.Mobile.ViewModels
{
    public class LifestylesSurveyViewModel : ViewModelBase
    {
        private int Counter { get; set; }
        private int MaxCounter { get; set; }
        private IDatabaseServices DatabaseServices => DependencyService.Get<IDatabaseServices>();
        private ObservableCollection<IndexedQuestions> _lifeStylesQuestions = new ObservableCollection<IndexedQuestions>();
        public ObservableCollection<IndexedQuestions> LifeStylesQuestions
        {
            get => _lifeStylesQuestions;
            set => SetProperty(ref _lifeStylesQuestions, value);
        }

        private FlexBasis _basis = new FlexBasis(1f, true);
        public FlexBasis Basis
        {
            get => _basis;
            set => SetProperty(ref _basis, value);
        }


        private List<IndexModel> _surveyIndexList = new List<IndexModel>();
        public List<IndexModel> SurveyIndexList
        {
            get => _surveyIndexList;
            set => SetProperty(ref _surveyIndexList, value);
        }
        private bool _isDone;
        public bool IsDone
        {
            get => _isDone;
            set => SetProperty(ref _isDone, value);
        }
        private ObservableCollection<SurveySummary> surveySummaries=new ObservableCollection<SurveySummary>();
        public ObservableCollection<SurveySummary> SurveySummaries
        {
            get => surveySummaries;
            set => SetProperty(ref surveySummaries, value);
        }
        public ICommand NextCommand => new Command<List<SurveySummary>>(async(param) =>
        {
            try
            {
                if (Counter < MaxCounter)
                {
                    LifeStylesQuestions = new ObservableCollection<IndexedQuestions>();
                    ++Counter;
                    foreach (var survey in App.surveyPageViewModelInstance.SurveyCollection.Where(a => a.Form.Title.ToLower() == "you + your lifestyle"))
                    {
                        foreach (var page in survey.Form.Pages.Skip(Counter).Take(1))
                        {
                            var questions = await DatabaseServices.Get<List<CustomFormQuestion>>("survey_page" + page.Id + "_" + survey.Form.Id);

                            LifeStylesQuestions.Add(new IndexedQuestions
                            {
                                SurveyGuid = survey.Form.Id,
                                PageGuid = page.Id,
                                Questions = new ObservableCollection<CustomFormQuestion>(questions)
                            });
                        }
                    }
                    if (LifeStylesQuestions[0].Questions?.Count == 3)
                    {
                        Basis = new FlexBasis(0.333f, true);
                    }
                    else if (LifeStylesQuestions[0].Questions?.Count == 2)
                    {
                        Basis = new FlexBasis(0.5f, true);
                    }
                    else if (LifeStylesQuestions[0].Questions?.Count == 1)
                    {
                        Basis = new FlexBasis(1f, true);
                    }
                    LifeStylesQuestions[0].IsSelected = true;

                   
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        var surveResponse = param.GroupBy(a => a.QuestionGuid).Select(a => new FormResponse
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.Now,
                            FormId = SplashPageViewModel.LifeStyleFormId,
                            Version = SplashPageViewModel.LifestyleFormVersion,
                            Answers = a.Distinct().Select(x => new FormQuestionResponse
                            {
                                QuestionId = new Guid(a.Key),
                                Answer = string.Join("|", param.Where(t => t.QuestionGuid == a.Key).Select(t => string.IsNullOrEmpty(t.SubAnswerText) ? t.AnswerText : string.IsNullOrEmpty(t.ConfigAnswerText) ? t.AnswerText + "-" + t.SubAnswerText : t.AnswerText + "-" + t.SubAnswerText + "-" + t.ConfigAnswerText))
                            }).ToList().Take(1).ToList()
                        });

                        var dbSurveyResponse = await DatabaseServices.Get<List<FormResponse>>("SurveyResponse");
                        dbSurveyResponse.AddRange(surveResponse);
                        await DatabaseServices.InsertData<List<FormResponse>>("SurveyResponse", dbSurveyResponse);
                    });
                    PostSurveyResponseAsync();
                }
            }
            catch (Exception)
            {

            }
        });
        private IToastServices ToastServices => DependencyService.Get<IToastServices>();
        private async void PostSurveyResponseAsync()
        {
            try
            {
                var dbSurveyResponse = await DatabaseServices.Get<List<FormResponse>>("SurveyResponse");
                var consumer = await DatabaseServices.Get<Consumer>("current_consumer" + Settings.CurrentTherapistId);
                if (consumer != null && consumer.Id != Guid.Empty)
                {
                    var list = new List<FormResponse>();
                    foreach (var item in dbSurveyResponse)
                    {
                        if (list.Count(a => a.FormId == item.FormId) == 0)
                        {
                            var formResponse = new FormResponse
                            {
                                Id = Guid.NewGuid(),
                                Created = DateTime.Now,
                                Version = item.Version,
                                FormId = item.FormId
                            };
                            formResponse.Answers = new List<FormQuestionResponse>();
                            formResponse.Answers.AddRange(item.Answers);
                            list.Add(formResponse);
                        }
                        else
                        {
                            list.Where(a => a.FormId == item.FormId).ForEach(x => x.Answers.AddRange(item.Answers));
                        }
                    }

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
                        DateOfBirth = consumer.DateOfBirth,
                        CurrentConsultation = new Consultation
                        {
                            Id = Guid.NewGuid(),
                            SurveyResponses=new List<IIAADataModels.Transfer.Survey.FormResponse>(list)
                        }
                    };

                    var isCompleted = await ApiServices.Client.PostAsync<bool>("salon/consumer/consultation/finalise", saloConsumer);
                    if (isCompleted)
                    {                        
                        await Task.Run(async() =>
                        {
                            await DatabaseServices.InsertData<List<FormResponse>>("survey_response_" + consumer.Id + "_" + currentTherapist.Id, list);
                            App.ConcernsAndSkinCareSurveyViewModel = new ConcernsAndSkinCareSurveyViewModel();
                            App.HealthQuestionsSurveyViewModel = new HealthQuestionsSurveyViewModel();
                            App.LifestylesSurveyViewModel = new LifestylesSurveyViewModel();

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Application.Current.MainPage = new AnimationNavigationPage(new SalonProductsPage());
                            });
                        });
                    }
                    else
                    {
                        ToastServices.ShowToast("Order failed");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public LifestylesSurveyViewModel()
        {
            GetConcernsQuestionsAsync();
        }
        public void GetConcernsQuestionsAsync()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (LifeStylesQuestions != null || LifeStylesQuestions.Count == 0)
                    {
                        foreach (var survey in App.surveyPageViewModelInstance.SurveyCollection.Where(a => a.Form.Title.ToLower() == "you + your lifestyle"))
                        {
                            foreach (var page in survey.Form.Pages.Take(1))
                            {
                                var questions = await DatabaseServices.Get<List<CustomFormQuestion>>("survey_page" + page.Id + "_" + survey.Form.Id);

                                LifeStylesQuestions.Add(new IndexedQuestions
                                {
                                    SurveyGuid = survey.Form.Id,
                                    PageGuid = page.Id,
                                    Questions = new ObservableCollection<CustomFormQuestion>(questions)
                                });
                            }
                            Counter = 0;
                            MaxCounter = survey.Form.Pages.Count - 1;
                        }
                      
                    }

                    LifeStylesQuestions[0].IsSelected = true;
                  
                });
            }
            catch (Exception)
            {

            }
        }
    }
}
