using IIAADataModels.Transfer.Survey;
using LaunchPad.Mobile.Helpers;
using LaunchPad.Mobile.Services;
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
    public class ConcernsAndSkinCareSurveyViewModel : ViewModelBase
    {
        public static Action Next;
        public static void OnNext()
        {
            Next?.Invoke();
        }
        private int Counter { get; set; }
        private int MaxCounter { get; set; }
        private IDatabaseServices DatabaseServices => DependencyService.Get<IDatabaseServices>();
        private ObservableCollection<IndexedQuestions> _concernAndSkinCareQuestions = new ObservableCollection<IndexedQuestions>();
        public ObservableCollection<IndexedQuestions> ConcernAndSkinCareQuestions
        {
            get => _concernAndSkinCareQuestions;
            set => SetProperty(ref _concernAndSkinCareQuestions, value);
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

        public ICommand ContinueCommand => new Command<List<SurveySummary>>(async(param) =>
          {
              try
              {
                  if (Counter < MaxCounter)
                  {
                      ConcernAndSkinCareQuestions = new ObservableCollection<IndexedQuestions>();
                      ++Counter;
                      foreach (var survey in App.surveyPageViewModelInstance.SurveyCollection.Where(a => a.Form.Title.ToLower() == "concerns + skin type"))
                      {
                          foreach (var page in survey.Form.Pages.Skip(Counter).Take(1))  //0,1,2,3
                          {
                              var questions = await DatabaseServices.Get<List<CustomFormQuestion>>("survey_page" + page.Id + "_" + survey.Form.Id);

                              ConcernAndSkinCareQuestions.Add(new IndexedQuestions
                              {
                                  SurveyGuid = survey.Form.Id,
                                  PageGuid = page.Id,
                                  Questions = new ObservableCollection<CustomFormQuestion>(questions)
                              });
                          }
                      }
                      ConcernAndSkinCareQuestions[0].IsSelected = true;
                      if (ConcernAndSkinCareQuestions[0].Questions?.Count == 3)
                      {
                          Basis = new FlexBasis(0.333f, true);
                      }
                      else if (ConcernAndSkinCareQuestions[0].Questions?.Count == 2)
                      {
                          Basis = new FlexBasis(0.5f, true);
                      }
                      else if (ConcernAndSkinCareQuestions[0].Questions?.Count == 1)
                      {
                          Basis = new FlexBasis(1f, true);
                      }
                      ConcernAndSkinCareQuestions[0].IsSelected = true;
                     
                  }
                  else
                  {
                      await Task.Run(async () =>
                      {
                          var surveResponse = param.GroupBy(a => a.QuestionGuid).Select(a => new FormResponse
                          {
                              Id = Guid.NewGuid(),
                              Created = DateTime.Now,
                              FormId = SplashPageViewModel.ConcernsFormId,
                              Version = SplashPageViewModel.ConcernsFormVersion,
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
                      Next?.Invoke();
                  }
              }
              catch (Exception)
              {

              }
             
          });
        public ConcernsAndSkinCareSurveyViewModel()
        {
            GetConcernsQuestionsAsync();
        }
        public void GetConcernsQuestionsAsync()
        {
            try
            {
                if (ConcernAndSkinCareQuestions != null || ConcernAndSkinCareQuestions.Count == 0)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                {

                    foreach (var survey in App.surveyPageViewModelInstance.SurveyCollection.Where(a => a.Form.Title.ToLower() == "concerns + skin type"))
                    {
                        foreach (var page in survey.Form.Pages.Take(1))
                        {
                            var questions = await DatabaseServices.Get<List<CustomFormQuestion>>("survey_page" + page.Id + "_" + survey.Form.Id);

                            ConcernAndSkinCareQuestions.Add(new IndexedQuestions
                            {
                                SurveyGuid = survey.Form.Id,
                                PageGuid = page.Id,
                                Questions = new ObservableCollection<CustomFormQuestion>(questions)
                            });
                        }
                        Counter = 0;
                        MaxCounter = survey.Form.Pages.Count - 1;
                    }

                    ConcernAndSkinCareQuestions[0].IsSelected = true;
                  
                });

                    //Task.Run(async () =>
                    //{
                    //    foreach (var survey in App.surveyPageViewModelInstance.SurveyCollection.Where(a => a.Form.Title.ToLower() == "concerns + skin type"))
                    //    {
                    //        foreach (var page in survey.Form.Pages.Skip(1))
                    //        {
                    //            var questions = await DatabaseServices.Get<List<CustomFormQuestion>>("survey_page" + page.Id + "_" + survey.Form.Id);

                    //            ConcernAndSkinCareQuestions.Add(new IndexedQuestions
                    //            {
                    //                SurveyGuid = survey.Form.Id,
                    //                PageGuid = page.Id,
                    //                Questions = new ObservableCollection<CustomFormQuestion>(questions)
                    //            });
                    //        }
                    //    }
                    //    Counter = 0;
                    //    MaxCounter = ConcernAndSkinCareQuestions.Count - 1;
                    //});
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
