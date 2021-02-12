using LaunchPad.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.CustomLayouts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthQuestionsContainerLayout : ContentView
    {
        private List<SurveySummary> SurveySummaries = new List<SurveySummary>();
        private string CurrentActiveResponse { get; set; }
        private string CurrentActiveQuestionId { get; set; }
        private string QuestionGuid { get; set; }
        private string CurrentQuestion { get; set; }
        private string CurrentAnswerText { get; set; }
        private string SubAnswerText { get; set; }
        public HealthQuestionsContainerLayout()
        {
            InitializeComponent();
            this.BindingContext = App.HealthQuestionsSurveyViewModel;
        }

        private void ToggleContainer(object sender, EventArgs e)
        {
            try
            {
                var senderLayout = (Frame)((Grid)sender).Parent;
                var senderLayoutParent = (Grid)senderLayout.Parent;
                var layout = senderLayoutParent.Children[1] as StackLayout;
                var list = BindableLayout.GetItemsSource(layout) as List<CustomFormQuestion>;
                if (list?.Count > 0)
                {
                    (senderLayoutParent.Children[1] as StackLayout).IsVisible = !(senderLayoutParent.Children[1] as StackLayout).IsVisible;
                    if ((senderLayoutParent.Children[1] as StackLayout).IsVisible)
                    {
                        senderLayout.BackgroundColor = Color.Black;
                        (((Grid)sender).Children[0] as Label).TextColor = Color.FromHex("#fff");
                        (senderLayoutParent.Children[2] as BoxView).IsVisible = true;
                        (senderLayoutParent.Children[3] as BoxView).IsVisible = true;
                    }
                    else
                    {
                        (senderLayoutParent.Children[2] as BoxView).IsVisible = false;
                        (senderLayoutParent.Children[3] as BoxView).IsVisible = false;
                    }
                }

                CurrentActiveResponse = (((Grid)sender).Children[0] as Label).Text;

                var containerParent = (StackLayout)((FlexLayout)((StackLayout)(senderLayoutParent.Parent)).Parent).Parent;
                if (containerParent != null)
                {
                    CurrentActiveQuestionId = ((Label)(containerParent.Children[0])).Text;
                    if (string.IsNullOrEmpty(CurrentActiveQuestionId))
                    {
                        CurrentActiveQuestionId = (((StackLayout)((StackLayout)(containerParent.Parent)).Parent)?.Children[0] as Label)?.Text;
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        private void OptionClicked(object sender, EventArgs e)
        {
            try
            {
                var senderLayoutParent = (Button)sender;
                var containerParent = (StackLayout)((FlexLayout)((StackLayout)(senderLayoutParent.Parent)).Parent).Parent;
                if (containerParent != null)
                {
                    CurrentActiveQuestionId = ((Label)(containerParent.Children[0])).Text;
                    if (string.IsNullOrEmpty(CurrentActiveQuestionId))
                    {
                        CurrentActiveQuestionId = (((StackLayout)((StackLayout)(containerParent.Parent)).Parent)?.Children[0] as Label)?.Text;
                    }
                }
                if (((Button)sender).BackgroundColor == Color.FromHex("#fff"))
                {
                    ((Button)sender).BackgroundColor = Color.FromHex("#000");
                    ((Button)sender).TextColor = Color.FromHex("#fff");
                    SurveySummaries.Add(new SurveySummary
                    {
                        QuestionGuid = CurrentActiveQuestionId,
                        QuestionText = CurrentQuestion,
                        AnswerText = (((Button)sender).CommandParameter as Answer)?.ResponseText
                    });
                }
                else
                {
                    ((Button)sender).BackgroundColor = Color.FromHex("#fff");
                    ((Button)sender).TextColor = Color.FromHex("#000");
                    var surveySummary = SurveySummaries.FirstOrDefault(a => a.AnswerText.ToLower() == (((Button)sender).CommandParameter as Answer)?.ResponseText.ToLower());
                    if (surveySummary != null)
                    {
                        SurveySummaries.Remove(surveySummary);
                    }

                }
            }
            catch (Exception)
            {

            }
        }

        private void SubOptionClicked(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).BackgroundColor == Color.FromHex("#000"))
                {
                    ((Button)sender).BackgroundColor = Color.FromHex("#fff");
                    ((Button)sender).TextColor = Color.FromHex("#000");
                }
                else
                {
                    ((Button)sender).BackgroundColor = Color.FromHex("#000");
                    ((Button)sender).TextColor = Color.FromHex("#fff");
                }
                var parent1 = ((Button)sender).Parent as StackLayout;
                var parent2 = parent1.Parent as Grid;
                var parent3 = parent2.Parent as StackLayout;
                var parameter = ((Button)sender).CommandParameter as Answer;
                SubAnswerText = parameter.ResponseText;
                foreach (var child in parent3.Children)
                {
                    var grid = child as Grid;
                    var stack = grid.Children[0] as StackLayout;
                    foreach (var item in stack.Children)
                    {
                        var button = item as Button;
                        if (button.Text?.ToLower() != parameter?.ResponseText?.ToLower())
                        {
                            button.BackgroundColor = Color.FromHex("#fff");
                            button.TextColor = Color.FromHex("#000");
                        }
                    }
                }

                var parent4 = parent3.Parent as StackLayout;
                var paremt5 = parent4.Parent as Grid;
                var child0 = ((Grid)((Frame)paremt5.Children[0]).Content).Children[0] as Label;
                if (child0 != null)
                {
                    CurrentActiveResponse = child0.Text;
                }
                var containerParent = (StackLayout)((FlexLayout)((StackLayout)(paremt5.Parent)).Parent).Parent;
                if (containerParent != null)
                {
                    CurrentActiveQuestionId = ((Label)(containerParent.Children[0])).Text;
                    if (string.IsNullOrEmpty(CurrentActiveQuestionId))
                    {
                        CurrentActiveQuestionId = (((StackLayout)((StackLayout)(containerParent.Parent)).Parent)?.Children[0] as Label)?.Text;
                    }
                }
                var child1 = (Entry)((Grid)((Frame)parent2.Children[1]).Content).Children[0];
                if (child1 != null)
                {
                    child1.Text = string.Empty;
                }
                if (SurveySummaries.Count(a => a.AnswerText.ToLower() == CurrentActiveResponse.ToLower()) == 0)
                {
                    SurveySummaries.Add(new SurveySummary
                    {
                        QuestionGuid=CurrentActiveQuestionId,
                        AnswerText = CurrentActiveResponse,
                        SubAnswerText = SubAnswerText
                    });
                }
                else
                {
                    SurveySummaries.Where(a => a.AnswerText.ToLower() == CurrentActiveResponse.ToLower()).ForEach(x =>
                    {
                        x.SubAnswerText = SubAnswerText;
                    });
                }
            }
            catch (Exception)
            {

            }
        }

        private void Finish(object sender, EventArgs e)
        {
            (this.BindingContext as HealthQuestionsSurveyViewModel)?.SaveAndContinueCommand.Execute(SurveySummaries);
        }

        private void SelectThis(object sender, EventArgs e)
        {
            try
            {
                
                if (((Button)sender).BackgroundColor == Color.FromHex("#000"))
                {
                    ((Button)sender).BackgroundColor = Color.FromHex("#fff");
                    ((Button)sender).TextColor = Color.FromHex("#000");
                    ((Frame)((StackLayout)((Button)sender).Parent).Children[2]).IsVisible = false;
                }
                else
                {
                    ((Button)sender).BackgroundColor = Color.FromHex("#000");
                    ((Button)sender).TextColor = Color.FromHex("#fff");
                    ((Frame)((StackLayout)((Button)sender).Parent).Children[2]).IsVisible = true;
                }
            }
            catch (Exception)
            {
            }


        }

        private void textAreaUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((Editor)sender).Text))
                {
                    CurrentAnswerText = ((Editor)sender).Text;
                    var question = ((Button)((StackLayout)((Frame)((Grid)((Editor)sender).Parent).Parent).Parent).Children[1]).Text;
                    QuestionGuid= ((Label)((StackLayout)((FlexLayout)((StackLayout)((Frame)((Grid)((Editor)sender).Parent).Parent).Parent).Parent).Parent).Children[0]).Text;
                   
                    if (!string.IsNullOrEmpty(question))
                    {
                        var splittedString = question.Split('-');
                        CurrentQuestion = splittedString[0];

                        if (SurveySummaries.Count(a => a.QuestionGuid == QuestionGuid) == 0)
                        {
                            SurveySummaries.Add(new SurveySummary
                            {
                                QuestionGuid=CurrentActiveQuestionId,
                                AnswerText = CurrentQuestion,
                                SubAnswerText = CurrentAnswerText
                            });
                        }
                        else
                        {
                            var surveySummary = SurveySummaries.First(a => a.QuestionGuid == QuestionGuid);
                            var surveySummaryIndex = SurveySummaries.IndexOf(surveySummary);
                            surveySummary.AnswerText = CurrentQuestion;
                            surveySummary.SubAnswerText = CurrentAnswerText;
                            SurveySummaries.Insert(surveySummaryIndex, surveySummary);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        private void entry_unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentActiveResponse))
                {
                    var mainParent = (Grid)((StackLayout)((StackLayout)((Grid)((Frame)((Grid)((Entry)sender).Parent).Parent).Parent).Parent).Parent).Parent;
                    var child0 = ((Grid)((Frame)mainParent.Children[0]).Content).Children[0] as Label;
                    if (child0 != null)
                    {
                        CurrentActiveResponse = child0.Text;
                    }

                    var containerParent = (StackLayout)((FlexLayout)((StackLayout)(mainParent.Parent)).Parent).Parent;
                    if (containerParent != null)
                    {
                        CurrentActiveQuestionId = ((Label)(containerParent.Children[0])).Text;
                        if (string.IsNullOrEmpty(CurrentActiveQuestionId))
                        {
                            CurrentActiveQuestionId = (((StackLayout)((StackLayout)(containerParent.Parent)).Parent)?.Children[0] as Label)?.Text;
                        }
                    }
                }
                if (SurveySummaries.Count(a => a.AnswerText.ToLower() == CurrentActiveResponse.ToLower()) == 0)
                {
                    SurveySummaries.Add(new SurveySummary
                    {
                        AnswerText = CurrentActiveResponse,
                        SubAnswerText = SubAnswerText,
                        ConfigAnswerText = ((Entry)sender).Text
                    });
                }
                else
                {
                    SurveySummaries.Where(a => a.AnswerText.ToLower() == CurrentActiveResponse.ToLower()).ForEach(x =>
                    {
                        x.ConfigAnswerText = ((Entry)sender).Text;
                    });
                }

                CurrentActiveResponse = string.Empty;
                CurrentAnswerText = string.Empty;
                SubAnswerText = string.Empty;

            }
            catch (Exception)
            {

            }
        }

        private void edit_container_clicked(object sender, EventArgs e)
        {
            EditButtonContainer.IsVisible = !EditButtonContainer.IsVisible;
            if (EditButtonContainer.IsVisible)
            {
                ((ImageButton)sender).Source = "icon_three_dots_active";
            }
            else
            {
                ((ImageButton)sender).Source = "icon_three_dots";
            }
        }

        private void EditButtonClicked(object sender, EventArgs e)
        {
            (this.BindingContext as HealthQuestionsSurveyViewModel).EditCommand.Execute(null);
            dotButton.Source = "icon_three_dots";
            EditButtonContainer.IsVisible = false;
        }

        private void UnSelect(object sender, EventArgs e)
        {
            try
            {
                var responseText = (((Grid)sender).Children[0] as Label).Text;
                var senderLayout = (Frame)((Grid)sender).Parent;
                var senderLayoutParent = (Grid)senderLayout.Parent;
                (senderLayoutParent.Children[1] as StackLayout).IsVisible = false;
                senderLayout.BackgroundColor = Color.FromHex("#fff");
                (((Grid)sender).Children[0] as Label).TextColor = Color.FromHex("#000");
                (senderLayoutParent.Children[2] as BoxView).IsVisible = false;
                (senderLayoutParent.Children[3] as BoxView).IsVisible = false;
                var parentlayout = senderLayoutParent.Children[1] as StackLayout;
                var childQuestionContainer = parentlayout.Children[0] as StackLayout;
                foreach (var item in childQuestionContainer.Children)
                {
                    var grid = item as Grid;
                    var gridChildren = grid.Children[0] as StackLayout;
                    foreach (var item1 in gridChildren.Children)
                    {
                        var button = item1 as Button;
                        button.BackgroundColor = Color.FromHex("#fff");
                        button.TextColor = Color.FromHex("#000");
                    }

                    var frameLayout = grid.Children[1] as Frame;
                    var frameLayoutChild = frameLayout.Content as Grid;
                    (frameLayoutChild.Children[0] as Entry).Text = string.Empty;
                }

                var surveySummary = SurveySummaries.FirstOrDefault(a => a.AnswerText.ToLower() == responseText.ToLower());
                if (surveySummary != null)
                {
                    SurveySummaries.Remove(surveySummary);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}