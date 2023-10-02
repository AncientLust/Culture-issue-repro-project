using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using System.Globalization;

namespace Culture_issue_repro_project
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            PopulateLocale();
        }

        private void PopulateLocale()
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                localePicker.Items.Add(culture.Name);
            }
        }

        private async void OnListenClicked(object sender, EventArgs e)
        {
            // Language selection
            if (localePicker.SelectedIndex == -1)
            {
                await Toast.Make("Please select a locale first.").Show(CancellationToken.None);
                return;
            }

            string selectedLocale = localePicker.Items[localePicker.SelectedIndex];

            // Permission check
            var isGranted = await SpeechToText.RequestPermissions(CancellationToken.None);
            if (!isGranted)
            {
                await Toast.Make("Permission not granted").Show(CancellationToken.None);
                return;
            }

            // Voice recognition
            var recognitionResult = await SpeechToText.ListenAsync(
                CultureInfo.GetCultureInfo(selectedLocale),
                new Progress<string>(),
                CancellationToken.None);

            if (recognitionResult.IsSuccessful)
            {
                await Toast.Make(recognitionResult.Text).Show(CancellationToken.None);
            }
            else
            {
                await Toast.Make(recognitionResult.Exception?.Message ?? "Unable to recognize speech").Show(CancellationToken.None);
            }
        }
    }
}