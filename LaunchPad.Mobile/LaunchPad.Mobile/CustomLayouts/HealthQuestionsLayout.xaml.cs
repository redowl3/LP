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
    public partial class HealthQuestionsLayout : ContentView
    {
        private List<SurveySummary> SurveySummaries = new List<SurveySummary>();
        private string CurrentQuestion { get; set; }
        public HealthQuestionsLayout()
        {
            InitializeComponent();
        }

        private void ToggleContainer(object sender, EventArgs e)
        {
            var layout = (((Grid)((Grid)sender).Parent).Children[1] as StackLayout);
            var list = BindableLayout.GetItemsSource(layout) as List<CustomFormQuestion>;
            if (list?.Count > 0)
            {
                (((Grid)((Grid)sender).Parent).Children[1] as StackLayout).IsVisible = !(((Grid)((Grid)sender).Parent).Children[1] as StackLayout).IsVisible;
                if ((((Grid)((Grid)sender).Parent).Children[1] as StackLayout).IsVisible)
                {
                    ((Grid)sender).BackgroundColor = Color.Black;
                    (((Grid)sender).Children[0] as Label).TextColor = Color.FromHex("#fff");
                }
            }
            var parent1 = ((Grid)sender).Parent as Grid;
            var parent2 = parent1.Parent as Frame;
            var parent3 = parent2.Parent as StackLayout;
            var parent4 = parent3.Parent as FlexLayout;
            CurrentQuestion = QuestiontextLabel.Text;
            var list1 = BindableLayout.GetItemsSource(parent4) as List<Answer>;
            var parameter = (e as TappedEventArgs)?.Parameter as Answer;
            list1.First(a => a.ResponseText.ToLower() == parameter.ResponseText.ToLower()).Selected = true;
            list1.Where(a => a.ResponseText.ToLower() != parameter.ResponseText.ToLower()).ForEach(a => a.Selected = false);
            BindableLayout.SetItemsSource(parent4, list1);
            var childrens = parent4.Children;
            foreach (var item in childrens)
            {
                var child = item as StackLayout;
                var nextChild = child.Children[0] as Frame;
                var nextChild1 = nextChild.Content as Grid;
                var nextChild2 = nextChild1.Children[0] as Grid;
                var nextChild3 = nextChild2.Children[0] as Label;
                if (nextChild3.Text?.ToLower() != parameter.ResponseText.ToLower())
                {
                    nextChild3.TextColor = Color.Black;
                    nextChild2.BackgroundColor = Color.Transparent;
                    (nextChild1.Children[1] as StackLayout).IsVisible = false;
                    foreach (var view in (nextChild1.Children[1] as StackLayout).Children)
                    {
                        var stack = view as StackLayout;
                        foreach (var view1 in stack.Children)
                        {
                            var grid = view1 as Grid;
                            var stack1 = grid.Children[0] as StackLayout;
                            foreach (var view2 in stack1.Children)
                            {
                                var button = view2 as Button;
                                button.BackgroundColor = Color.FromHex("#fff");
                                button.TextColor = Color.FromHex("#000");
                            }
                        }
                    }
                }
            }
        }

        private void OptionClicked(object sender, EventArgs e)
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
            var parent2 = parent1.Parent as FlexLayout;
            var parameter = ((Button)sender).CommandParameter as Answer;
            foreach (var child in parent2.Children)
            {
                var stack = child as StackLayout;
                if (stack.Children.Count>1)
                {
                    var button = stack.Children[1] as Button;
                    if (button.Text?.ToLower() != parameter?.ResponseText?.ToLower())
                    {
                        button.BackgroundColor = Color.FromHex("#fff");
                        button.TextColor = Color.FromHex("#000");
                    }
                }
            }
        }

        private void SubOptionClicked(object sender, EventArgs e)
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
        }
    }
}