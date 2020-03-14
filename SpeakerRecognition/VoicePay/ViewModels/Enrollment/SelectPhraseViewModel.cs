using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;
using SpeakerRecognitionAPI.Models;
using VoicePay.Services;

namespace VoicePay.ViewModels.Enrollment
{
    public class SelectPhraseViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Phrase> Phrases { get; set; }

        public SelectPhraseViewModel()
        {
            IsBusy = true;
            Phrases = new ObservableRangeCollection<Phrase>();
        }

        public async Task GetPhrases()
        {
            IsBusy = true;

            try
            {
                var phrases = await VerificationService.Instance.GetPhrasesAsync();
                if (phrases.Any())
                {
                    Phrases.ReplaceRange(phrases);
                }
                else
                {
                    DisplayAlert("¡Oops!", "Our service is not available right now.", "OK");
                }
            }
            catch
            {
                DisplayAlert("¡Oops!", "Our service is not available right now.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
