using IIAADataModels.Transfer.Survey;
using LaunchPad.Mobile.Helpers;
using LaunchPad.Mobile.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
namespace LaunchPad.Mobile.ViewModels
{
    public class HealthQuestionsSurveyViewModel :ViewModelBase
    {
        public static Action Next;
        public static void OnNext()
        {
            Next?.Invoke();
        }
        private int Counter { get; set; }
        private int MaxCounter { get; set; }
        private IDatabaseServices DatabaseServices => DependencyService.Get<IDatabaseServices>();
        private ObservableCollection<IndexedQuestions> _healthQuestionsCollection = new ObservableCollection<IndexedQuestions>();
        public ObservableCollection<IndexedQuestions> HealthQuestionsCollection
        {
            get => _healthQuestionsCollection;
            set => SetProperty(ref _healthQuestionsCollection, value);
        }

        private FlexBasis _basis = new FlexBasis(1f, true);
        public FlexBasis Basis
        {
            get => _basis;
            set => SetProperty(ref _basis, value);
        }

        private bool _isDone;
        public bool IsDone
        {
            get => _isDone;
            set => SetProperty(ref _isDone, value);
        }

        private List<IndexModel> _surveyIndexList = new List<IndexModel>();
        public List<IndexModel> SurveyIndexList
        {
            get => _surveyIndexList;
            set => SetProperty(ref _surveyIndexList, value);
        }
        public ICommand NextCommand => new Command(() =>
        {
            if (Counter < MaxCounter)
            {
                HealthQuestionsCollection[Counter].IsSelected = false;
                ++Counter;
                HealthQuestionsCollection[Counter].IsSelected = true;
            }
        });

        private ObservableCollection<SurveySummary> surveySummaries;
        public ObservableCollection<SurveySummary> SurveySummaries
        {
            get => surveySummaries;
            set => SetProperty(ref surveySummaries, value);
        }

        private bool _CanContinue = false;
        public bool CanContinue
        {
            get => _CanContinue;
            set => SetProperty(ref _CanContinue, value);
        }
        public ICommand SaveAndContinueCommand => new Command<List<SurveySummary>>((param) =>
        {           
            Task.Run(async () =>
            {
                var surveResponse = param.GroupBy(a => a.QuestionGuid).Select(a => new FormResponse
                {
                    Id=Guid.NewGuid(),
                    Created=DateTime.Now,
                    FormId=SplashPageViewModel.HealthFormId,
                    Version=SplashPageViewModel.HealthFormVersion,
                    Answers=a.Distinct().Select(x=>new FormQuestionResponse
                    {
                        QuestionId=new Guid(a.Key),
                        Answer= string.Join("|", param.Where(t => t.QuestionGuid == a.Key).Select(t => string.IsNullOrEmpty(t.SubAnswerText) ? t.AnswerText : string.IsNullOrEmpty(t.ConfigAnswerText) ? t.AnswerText + "-" + t.SubAnswerText : t.AnswerText + "-" + t.SubAnswerText + "-" + t.ConfigAnswerText))
                    }).ToList().Take(1).ToList()
                });

                await DatabaseServices.InsertData<List<FormResponse>>("SurveyResponse", surveResponse.ToList());
                Device.BeginInvokeOnMainThread(() =>
                {
                    SurveySummaries = new ObservableCollection<SurveySummary>(param);
                    IsDone = true;
                    CanContinue = true;
                });
            });

        });
        public ICommand ContinueCommand => new Command<List<SurveySummary>>(async(param) =>
        {
            Next?.Invoke();
        });
        public ICommand EditCommand => new Command(() =>
        {
            Task.Run(async() =>
            {
                await DatabaseServices.Delete<List<SurveySummary>>("healthsurvey_done_" + Settings.ClientId + "_" + Settings.CurrentTherapistId);
                SecureStorage.Remove("SurveyDone_" + Settings.ClientId + "_" + Settings.CurrentTherapistId);
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsDone = false;
                    CanContinue = false;
                });
            });
          
        });
        public HealthQuestionsSurveyViewModel()
        {
            GetConcernsQuestionsAsync();
        }
        public void GetConcernsQuestionsAsync()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (HealthQuestionsCollection != null || HealthQuestionsCollection.Count == 0)
                    {
                        foreach (var survey in App.surveyPageViewModelInstance.SurveyCollection.Where(a => a.Form.Title.ToLower() == "health questions"))
                        {
                            foreach (var page in survey.Form.Pages)
                            {
                                var questions = await DatabaseServices.Get<List<CustomFormQuestion>>("survey_page" + page.Id + "_" + survey.Form.Id);
                                foreach (var question in questions)
                                {
                                    foreach (var item in question.ChildQuestions.Where(x=>x.ChildQuestions.Where(a=>a.FormQuestion.QuestionType.ToLower()== "conditionalgroup").ToList().Count>0))
                                    {
                                        item.FormQuestionData.Answers.ForEach(a =>
                                        {
                                            var subListData = item.ChildQuestions.FirstOrDefault(x => x.FormQuestionData.conditionvalue.ToLower() == a.ResponseText.ToLower());
                                            if (subListData != null)
                                            {
                                                if (a.SubOptionsList == null) a.SubOptionsList = new List<CustomFormQuestion>();
                                                a.SubOptionsList.Add(new CustomFormQuestion
                                                {
                                                    FormQuestion=subListData.FormQuestion,
                                                    FormQuestionData=subListData.FormQuestionData,
                                                    ChildQuestions=subListData.FormQuestion.ChildQuestions.Select(t=>new CustomFormQuestion
                                                    {
                                                        FormQuestion=t,
                                                        FormQuestionData= JsonConvert.DeserializeObject<FormQuestionData>(t.QuestionData.ToString()),
                                                        IsCheck = t.QuestionType.ToLower() == "check",
                                                        IsRadio = t.QuestionType.ToLower() == "radio",
                                                        IsTextArea = t.QuestionType.ToLower() == "textarea",
                                                        IsTextBox = t.QuestionType.ToLower() == "textbox",
                                                        IsYesNo = t.QuestionType.ToLower() == "yesno",
                                                        IsYesNoWithNoTextArea = t.QuestionType.ToLower() == "yesno",
                                                        IsGroup = t.QuestionType.ToLower() == "group",
                                                        IsHtml = t.QuestionType.ToLower() == "html"
                                                    }).ToList()
                                                });
                                            }
                                        });
                                    }
                                }
                                HealthQuestionsCollection.Add(new IndexedQuestions
                                {
                                    SurveyGuid = survey.Form.Id,
                                    PageGuid = page.Id,
                                    Questions = new ObservableCollection<CustomFormQuestion>(questions)
                                });
                            }
                        }
                    }

                    HealthQuestionsCollection[0].IsSelected = true;
                    Counter = 0;
                    MaxCounter = HealthQuestionsCollection.Count - 1;
                });
            }
            catch (Exception)
            {

            }
        }
    }
}
