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

namespace LaunchPad.Mobile.ViewModels
{
    public class SplashPageViewModel : ViewModelBase
    {
        public static Guid ConcernsFormId { get; set; }
        public static int ConcernsFormVersion { get; set; }
        public static Guid HealthFormId { get; set; }
        public static int HealthFormVersion { get; set; }
        public static Guid LifeStyleFormId { get; set; }
        public static int LifestyleFormVersion { get; set; }
        public static Action<int> UpdateProgress;
        public static void OnUpdateProgress(int param)
        {
            UpdateProgress?.Invoke(param);
        }
        private IDatabaseServices DatabaseServices => DependencyService.Get<IDatabaseServices>();
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        private double _progress = 0.0;
        public double Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        private bool _progressBarVisible;
        public bool ProgressBarVisible
        {
            get => _progressBarVisible;
            set => SetProperty(ref _progressBarVisible, value);
        }


        public SplashPageViewModel()
        {
            GetInitialDataAsync();
        }
        private async void GetInitialDataAsync()
        {
            IsBusy = true;
            await Task.Delay(1000);
            Device.BeginInvokeOnMainThread(() => ExceptionHandler(async () =>
            {
                ProgressBarVisible = true;
                Progress = 0.10;
                var salon = await ApiServices.Client.GetAsync<Salon>("salon");
                Progress = 0.25;
                if (salon != null || salon.Id != Guid.Empty)
                {
                    App.SalonName = salon.Name;
                    Settings.SalonName = salon.Name;
                    var result = await DatabaseServices.InsertData("salon", salon);
                    if (result)
                    {
                        Console.WriteLine("salon stored to local cache");
                    }

                    var concernQuestions = new List<CustomFormQuestion>();
                    var healthQuestions = new List<CustomFormQuestion>();
                    var lifestylesQuestions = new List<CustomFormQuestion>();
                    if (salon.Surveys?.Count > 0)
                    {
                        foreach (var survey in salon.Surveys)
                        {
                            if (survey.Title.ToLower() == "concerns + skin type")
                            {
                                ConcernsFormId = survey.Id;
                                ConcernsFormVersion = survey.Version;
                            }
                            if (survey.Title.ToLower() == "health questions")
                            {
                                HealthFormId = survey.Id;
                                HealthFormVersion = survey.Version;
                            }
                            if (survey.Title.ToLower() == "you + your lifestyle")
                            {
                                LifeStyleFormId = survey.Id;
                                LifestyleFormVersion = survey.Version;
                            }
                            foreach (var page in survey.Pages)
                            {
                                var questions = new List<CustomFormQuestion>(page.Questions.Select(a => new CustomFormQuestion
                                {
                                    FormId = survey.Id,
                                    Version = survey.Version,
                                    QuestionGuid = a.Id.ToString(),
                                    ConcernPage = survey.Title.ToLower() == "concerns + skin type",
                                    HealthQuestions = survey.Title.ToLower() == "health questions",
                                    LifeStyles = survey.Title.ToLower() == "you + your lifestyle",
                                    PageGuid = page.Id,
                                    FormQuestion = a,
                                    FormQuestionData = JsonConvert.DeserializeObject<FormQuestionData>(a.QuestionData.ToString()),
                                    ChildQuestions = a.ChildQuestions.Select(x => new CustomFormQuestion
                                    {
                                        QuestionGuid = x.Id.ToString(),
                                        PageFormQuestionData = JsonConvert.DeserializeObject<FormQuestionData>(a.QuestionData.ToString()),
                                        ConcernPage = survey.Title.ToLower() == "concerns + skin type",
                                        HealthQuestions = survey.Title.ToLower() == "health questions",
                                        LifeStyles = survey.Title.ToLower() == "you + your lifestyle",
                                        FormQuestion = x,
                                        FormQuestionData = JsonConvert.DeserializeObject<FormQuestionData>(x.QuestionData.ToString()),
                                        ChildQuestions = x.ChildQuestions.Select(t => new CustomFormQuestion
                                        {
                                            QuestionGuid = t.Id.ToString(),
                                            ConcernPage = survey.Title.ToLower() == "concerns + skin type",
                                            HealthQuestions = survey.Title.ToLower() == "health questions",
                                            LifeStyles = survey.Title.ToLower() == "you + your lifestyle",
                                            FormQuestion = t,
                                            FormQuestionData = JsonConvert.DeserializeObject<FormQuestionData>(t.QuestionData.ToString()),
                                            IsCheck = t.QuestionType.ToLower() == "check",
                                            IsRadio = t.QuestionType.ToLower() == "radio",
                                            IsTextArea = t.QuestionType.ToLower() == "textarea",
                                            IsTextBox = t.QuestionType.ToLower() == "textbox",
                                            IsYesNo = x.QuestionType.ToLower() == "yesno",
                                            IsYesNoWithNoTextArea = t.QuestionType.ToLower() == "yesno",
                                            IsGroup = t.QuestionType.ToLower() == "group",
                                            IsHtml = t.QuestionType.ToLower() == "html",
                                            IsDropdown = t.QuestionType.ToLower() == "dropdown",
                                            ConfigText = t.Config.Count(config => !string.IsNullOrEmpty(config.Value)) > 0 ? t.Config.First(config => !string.IsNullOrEmpty(config.Value)).Value : ""
                                        }).ToList(),
                                        IsCheck = x.QuestionType.ToLower() == "check",
                                        IsRadio = x.QuestionType.ToLower() == "radio",
                                        IsTextArea = x.QuestionType.ToLower() == "textarea",
                                        IsTextBox = x.QuestionType.ToLower() == "textbox",
                                        IsYesNo = x.QuestionType.ToLower() == "yesno",
                                        IsCheckWithMultipleYesNo = a.ChildQuestions.Count(q => q.QuestionType.ToLower() == "check") > 0 && a.ChildQuestions.Count(q => q.QuestionType.ToLower() == "yesno") >= 1,
                                        IsYesNoWithNoTextArea = a.ChildQuestions.Count(q => q.QuestionType.ToLower() == "yesno") > 0 && a.ChildQuestions.Count(q => q.QuestionType.ToLower() == "textarea") == 0,
                                        IsGroup = x.QuestionType.ToLower() == "group",
                                        IsHtml = x.QuestionType.ToLower() == "html",
                                        IsDropdown = x.QuestionType.ToLower() == "dropdown",
                                        IsYesNoWithTextArea = a.ChildQuestions.Count(q => q.QuestionType.ToLower() == "yesno") > 0 && a.ChildQuestions.Count(q => q.QuestionType.ToLower() == "textarea") > 0,
                                        ConfigText = x.Config.Count(config => !string.IsNullOrEmpty(config.Value)) > 0 ? x.Config.First(config => !string.IsNullOrEmpty(config.Value)).Value : ""
                                    }).ToList(),
                                    IsCheck = a.QuestionType.ToLower() == "check",
                                    IsRadio = a.QuestionType.ToLower() == "radio",
                                    IsTextArea = a.QuestionType.ToLower() == "textarea",
                                    IsTextBox = a.QuestionType.ToLower() == "textbox",
                                    IsYesNoWithNoTextArea = a.QuestionType.ToLower() == "yesno" && a.ChildQuestions.Count(t1 => t1.QuestionType.ToLower() == "textarea") == 0,
                                    IsGroup = a.QuestionType.ToLower() == "group" && a.ChildQuestions?.First().QuestionType.ToLower() != "group",
                                    IsHtml = a.QuestionType.ToLower() == "html",
                                    IsDropdown = a.QuestionType.ToLower() == "dropdown",
                                    IsNestedGroup = a.QuestionType.ToLower() == "group" && a.ChildQuestions?.First().QuestionType.ToLower() == "group",
                                    IsYesNoWithTextArea = a.ChildQuestions.Count(t1 => t1.QuestionType.ToLower() == "yesno") > 0 && a.ChildQuestions.Count(t1 => t1.QuestionType.ToLower() == "textarea") > 0,
                                    IsCheckWithMultipleYesNo = a.ChildQuestions.Count(q => q.QuestionType.ToLower() == "check") > 0 && a.ChildQuestions.Count(q => q.QuestionType.ToLower() == "yesno") >= 1,
                                    YesNoQuestionsList = new List<CustomFormQuestion>(a.ChildQuestions.Where(q => q.QuestionType.ToLower() == "yesno").Select(yn => new CustomFormQuestion
                                    {
                                        FormQuestion = yn,
                                        FormQuestionData = JsonConvert.DeserializeObject<FormQuestionData>(yn.QuestionData.ToString())
                                    })),
                                }));

                                await DatabaseServices.InsertData("survey_page" + page.Id + "_" + survey.Id, questions);
                            }
                        }
                    }
                }

                var consumers = await ApiServices.Client.GetAsync<List<Consumer>>("Salon/Consumers");
                Progress = 0.50;
                if (consumers?.Count > 0)
                {
                    var result = await DatabaseServices.InsertData("consumers", consumers);
                    if (result)
                    {
                        Console.WriteLine("consumers stored to local cache");
                    }

                }

                App.surveyPageViewModelInstance = new SurveyPageViewModel();
                await App.surveyPageViewModelInstance.GetSurveyDataAsync();
                App.ConcernsAndSkinCareSurveyViewModel = new ConcernsAndSkinCareSurveyViewModel();
                App.HealthQuestionsSurveyViewModel = new HealthQuestionsSurveyViewModel();
                App.LifestylesSurveyViewModel = new LifestylesSurveyViewModel();
                await Task.Delay(2000);
                Progress = 0.75;
                IsBusy = false;
                var locationStatus = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();

                if (locationStatus != PermissionStatus.Granted)
                {
                    locationStatus = await Permissions.RequestAsync<Permissions.LocationAlways>();
                }
                var locationInUseStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (locationInUseStatus != PermissionStatus.Granted)
                {
                    locationInUseStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
                var camerastatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (camerastatus != PermissionStatus.Granted)
                {
                    camerastatus = await Permissions.RequestAsync<Permissions.Camera>();
                }

                var flashLightStatus = await Permissions.CheckStatusAsync<Permissions.Flashlight>();
                if (flashLightStatus != PermissionStatus.Granted)
                {
                    await Task.Delay(500);
                    flashLightStatus = await Permissions.RequestAsync<Permissions.Flashlight>();
                }

                Progress = 1;
                await Task.Delay(100);
                Settings.ClientName = "Sarah Smith";
                var currentTherapist = await SecureStorage.GetAsync("currentTherapist");
                if (!string.IsNullOrEmpty(currentTherapist))
                {
                    Application.Current.MainPage = new AnimationNavigationPage(new SalonClientsPage());
                }
                else
                {

                    Application.Current.MainPage = new AnimationNavigationPage(new SignInPage());
                }

            }));
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
