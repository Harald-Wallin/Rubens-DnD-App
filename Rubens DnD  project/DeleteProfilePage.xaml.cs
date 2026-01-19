using System.Collections.ObjectModel;

namespace Rubens_DnD__project;

public partial class DeleteProfilePage : ContentPage
{

	private Character _profileToDelete;
	private ObservableCollection<Character> _characterList;
    private MainPage _mainPage;    

	public DeleteProfilePage(Character profile, MainPage mainPage)
	{
		InitializeComponent();

		_profileToDelete = profile;
        _mainPage = mainPage;

        TitleLabel.Text = $"Vill du ta bort {_profileToDelete.Name}?";
	}

	private async void CancelClicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync();
	}

    private async void DeleteClicked(object sender, EventArgs e)
    {
        if (ConfirmEntry.Text == "RADERA")
        {
            await _mainPage.RemoveCharacter(_profileToDelete); // ta bort via MainPage
            await Navigation.PopModalAsync();
            await DisplayAlert("Borttagen", $"{_profileToDelete.Name} har raderats.", "OK");
        }
        else
        {
            await DisplayAlert("Fel", "Skriv RADERA för att bekräfta.", "OK");
        }
    }


}