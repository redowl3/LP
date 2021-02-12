using LaunchPad.Mobile.Models;
using LaunchPad.Mobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaunchPad.Mobile.CustomLayouts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductLayoutView : ContentView
    {
        private CustomProductAdditionalInfo OldParam;
        public ProductLayoutView()
        {
            InitializeComponent();
        }
        private void benefits_close_clicked(object sender, TappedEventArgs e)
        {
            //BenefitsDetailsGridView.IsVisible = false;
            //BenefitBoxView.BackgroundColor = Color.Gray;
            DetailStack.IsVisible = !DetailStack.IsVisible;
        }

        private void TapGestureRecognizer_Tappedbenefits(object sender, EventArgs e)
        {
            //DirectionDetailsGridView.IsVisible = false;
            //KeyDetailsGridView.IsVisible = false;
            //DirectionBoxView.BackgroundColor = KeyBoxView.BackgroundColor = Color.Gray;
            //BenefitsDetailsGridView.IsVisible = !BenefitsDetailsGridView.IsVisible;
            //if (BenefitsDetailsGridView.IsVisible)
            //{
            //    BenefitBoxView.BackgroundColor = Color.White;
            //}
            //else
            //{
            //    BenefitBoxView.BackgroundColor = Color.Gray;
            //}
        }

        private void TapGestureRecognizer_Tappeddirections(object sender, EventArgs e)
        {
            //BenefitsDetailsGridView.IsVisible = false;
            //KeyDetailsGridView.IsVisible = false;
            //BenefitBoxView.BackgroundColor = KeyBoxView.BackgroundColor = Color.Gray;
            //DirectionDetailsGridView.IsVisible = !DirectionDetailsGridView.IsVisible;
            //if (DirectionDetailsGridView.IsVisible)
            //{
            //    DirectionBoxView.BackgroundColor = Color.White;
            //}
            //else
            //{
            //    DirectionBoxView.BackgroundColor = Color.Gray;
            //}
        }

        private void TapGestureRecognizer_TappedKey(object sender, EventArgs e)
        {
            //BenefitsDetailsGridView.IsVisible = false;
            //DirectionDetailsGridView.IsVisible = false;
            //BenefitBoxView.BackgroundColor = DirectionBoxView.BackgroundColor = Color.Gray;
            //KeyDetailsGridView.IsVisible = !KeyDetailsGridView.IsVisible;
            //if (KeyDetailsGridView.IsVisible)
            //{
            //    KeyBoxView.BackgroundColor = Color.White;
            //}
            //else
            //{
            //    KeyBoxView.BackgroundColor = Color.Gray;
            //}


        }

        private void itemTapped(object sender, EventArgs e)
        {
            DetailStack.IsVisible = true;
            var param = ((e as TappedEventArgs)?.Parameter as CustomProductAdditionalInfo);
            if (OldParam!=null && OldParam.Id == param.Id)
            {
                DetailStack.IsVisible = false;
                OldParam = null;
            }
            else
            {
                DetailLabel.Text = param.AdditionalInformation.Detail;
                OldParam = param;
            }
        }

        private void CloseStack(object sender, EventArgs e)
        {
            DetailStack.IsVisible = false;
        }
    }
}