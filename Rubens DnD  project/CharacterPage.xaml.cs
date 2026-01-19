namespace Rubens_DnD__project;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.Text.Json;


public partial class CharacterPage : ContentPage
{
    private Character character;
    private Item currentAbilityDetail;
    private bool isCreatingNew = false;
    private bool isAbilityMode = false;



    // Inspirationstatus
    private BoxView[] InspirationBoxes;

    public CharacterPage(Character character)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        this.character = character;
        LoadFullCharacter();
        UpdateHPLabel();
        ClosePopups();

        InventoryListView.ItemsSource = character.Inventory;
        AbilitiesListView.ItemsSource = character.Abilities;

        // Steg 1 – Sätt namn, ras, klass
        NameLabel.Text = character.Name;
        RaceLabel.Text = character.Race;
        ClassLabel.Text = character.Class;

        // Steg 2 – Visa level / stats
        LevelLabel.Text = character.Level.ToString();
        ArmorLabel.Text = character.Armor.ToString();
        HPLabel.Text = $"{character.CurrentHP}/{character.MaxHP}";
        WizardLabel.Text = character.WizardLevel.ToString();

        StrengthLabel.Text = character.Strength.ToString();
        DexterityLabel.Text = character.Dexterity.ToString();
        IntellectLabel.Text = character.Intellect.ToString();
        CharismaLabel.Text = character.Charisma.ToString();
        SpiritLabel.Text = character.Spirit.ToString();

        //-------------Inspirations----------

        InspirationBoxes = new BoxView[] { Inspiration1, Inspiration2, Inspiration3 };

        for (int i = 0; i < InspirationBoxes.Length; i++)
        {
            InspirationBoxes[i].Color = character.InspirationStatus[i] ? Colors.Gold : Colors.Gray;
        }


        
    }

    private void LoadFullCharacter()
    {
        if (!Preferences.ContainsKey($"Character_{character.Name}"))
            return;

        var json = Preferences.Get($"Character_{character.Name}", "");
        if (string.IsNullOrEmpty(json))
            return;

        var loadedCharacter = JsonSerializer.Deserialize<Character>(json);
        if (loadedCharacter == null)
            return;

        // ?? KOPIERA VÄRDEN – byt inte objekt!
        character.Level = loadedCharacter.Level;
        character.Armor = loadedCharacter.Armor;
        character.CurrentHP = loadedCharacter.CurrentHP;
        character.MaxHP = loadedCharacter.MaxHP;
        character.WizardLevel = loadedCharacter.WizardLevel;

        character.Strength = loadedCharacter.Strength;
        character.Dexterity = loadedCharacter.Dexterity;
        character.Intellect = loadedCharacter.Intellect;
        character.Charisma = loadedCharacter.Charisma;
        character.Spirit = loadedCharacter.Spirit;

        character.Notes = loadedCharacter.Notes;

        character.Inventory = loadedCharacter.Inventory ?? new ObservableCollection<Item>();
        character.Abilities = loadedCharacter.Abilities ?? new ObservableCollection<Item>();
        character.InspirationStatus = loadedCharacter.InspirationStatus ?? new bool[3];

    }


    void ShowPopup(VisualElement popup)
    {
        ClosePopups();
        PopupOverlay.IsVisible = true;
        popup.IsVisible = true;
    }

    private Item currentDetailItem;

    private void ShowItemDetailOnTop(Item item)
    {
        currentDetailItem = item;
        ItemDetailName.Text = item.Name;
        ItemDetailDescription.Text = item.Description;
        PopupOverlay.IsVisible = true;
        ItemDetailPopup.IsVisible = true;
    }

    private void CloseItemDetailPopup(object sender, EventArgs e)
    {
        ItemDetailPopup.IsVisible = false;
        currentDetailItem = null;

        if (!InventoryPopup.IsVisible && !AbilitiesPopup.IsVisible && !NotesPopup.IsVisible)
        {
            PopupOverlay.IsVisible = false;
        }
    }

    void ClosePopups()
    {
        PopupOverlay.IsVisible = false;
        InventoryPopup.IsVisible = false;
        AbilitiesPopup.IsVisible = false;
        NotesPopup.IsVisible = false;
    }


    // ---------------- Level ----------------
    private void LevelUpClicked(object sender, EventArgs e)
    {
        character.Level++;
        LevelLabel.Text = character.Level.ToString();
    }

    private void DecreaseLevelClicked(object sender, EventArgs e)
    {
        if (character.Level > 1)
        {
            character.Level--;
            LevelLabel.Text = character.Level.ToString();
        }
    }

    // ---------------- Armor ----------------
    private void IncreaseArmorClicked(object sender, EventArgs e)
    {
        character.Armor++;
        ArmorLabel.Text = character.Armor.ToString();
    }

    private void DecreaseArmorClicked(object sender, EventArgs e)
    {
        if (character.Armor > 0)
        {
            character.Armor--;
            ArmorLabel.Text = character.Armor.ToString();
        }
    }

    // ---------------- HP ----------------
    // Öka current HP
    private void IncreaseCurrentHPClicked(object sender, EventArgs e)
    {
        character.CurrentHP++;
        UpdateHPLabel();
    }

    // Minska current HP
    private void DecreaseCurrentHPClicked(object sender, EventArgs e)
    {
        if (character.CurrentHP > -10)
            character.CurrentHP--;
            UpdateHPLabel();
    }

    // Öka max HP
    private void IncreaseMaxHPClicked(object sender, EventArgs e)
    {
        character.MaxHP++;
        UpdateHPLabel();
    }

    // Minska max HP
    private void DecreaseMaxHPClicked(object sender, EventArgs e)
    {
        if (character.MaxHP > 0)
            character.MaxHP--;
            UpdateHPLabel();
    }

    private void UpdateHPLabel()
    {
        if (character == null || HPLabel == null)
            return;


        var currentHpColor = character.CurrentHP < 0
            ? Colors.Red
            : Colors.White; // eller Black

        HPLabel.FormattedText = new FormattedString
        {
            Spans =
        {
            new Span
            {
                Text = character.CurrentHP.ToString(),
                TextColor = currentHpColor,
                Style = (Style)Application.Current.Resources["HPSpanStyle"]

            },
            new Span
            {
                Text = "/",
                TextColor = Colors.White,
                Style = (Style)Application.Current.Resources["HPSpanStyle"]

            },
            new Span
            {
                Text = character.MaxHP.ToString(),
                TextColor = Colors.White,
                Style = (Style)Application.Current.Resources["HPSpanStyle"]

            }
        }
        };
    }




    // ---------------- Wizard Level ----------------
    private void IncreaseWizardClicked(object sender, EventArgs e)
    {
        character.WizardLevel++;
        WizardLabel.Text = character.WizardLevel.ToString();
    }

    private void DecreaseWizardClicked(object sender, EventArgs e)
    {
        if (character.WizardLevel > 0)
        {
            character.WizardLevel--;
            WizardLabel.Text = character.WizardLevel.ToString();
        }
    }

    // ---------------- Attribut ----------------
    private void IncreaseStrengthClicked(object sender, EventArgs e)
    {
        character.Strength++;
        StrengthLabel.Text = character.Strength.ToString();
    }

    private void DecreaseStrengthClicked(object sender, EventArgs e)
    {
        if (character.Strength > 0)
        {
            character.Strength--;
            StrengthLabel.Text = character.Strength.ToString();
        }
    }

    private void IncreaseDexterityClicked(object sender, EventArgs e)
    {
        character.Dexterity++;
        DexterityLabel.Text = character.Dexterity.ToString();
    }

    private void DecreaseDexterityClicked(object sender, EventArgs e)
    {
        if (character.Dexterity > 0)
        {
            character.Dexterity--;
            DexterityLabel.Text = character.Dexterity.ToString();
        }
    }

    private void IncreaseIntellectClicked(object sender, EventArgs e)
    {
        character.Intellect++;
        IntellectLabel.Text = character.Intellect.ToString();
    }

    private void DecreaseIntellectClicked(object sender, EventArgs e)
    {
        if (character.Intellect > 0)
        {
            character.Intellect--;
            IntellectLabel.Text = character.Intellect.ToString();
        }
    }

    private void IncreaseCharismaClicked(object sender, EventArgs e)
    {
        character.Charisma++;
        CharismaLabel.Text = character.Charisma.ToString();
    }

    private void DecreaseCharismaClicked(object sender, EventArgs e)
    {
        if (character.Charisma > 0)
        {
            character.Charisma--;
            CharismaLabel.Text = character.Charisma.ToString();
        }
    }

    private void IncreaseSpiritClicked(object sender, EventArgs e)
    {
        character.Spirit++;
        SpiritLabel.Text = character.Spirit.ToString();
    }

    private void DecreaseSpiritClicked(object sender, EventArgs e)
    {
        if (character.Spirit > 0)
        {
            character.Spirit--;
            SpiritLabel.Text = character.Spirit.ToString();
        }
    }

    // ---------------- Väskan ----------------
    private void InventoryBtn_Clicked(object sender, EventArgs e)
    {
        // Fyll CollectionView med items
        InventoryListView.ItemsSource = character.Inventory;
        ShowPopup(InventoryPopup);
    }

    private void InventoryBackClicked(object sender, EventArgs e)
    {
        ClosePopups();
    }

    private void InventoryItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as Item;
        if (item == null) return;

        currentDetailItem = item;
        isCreatingNew = false;

        ItemDetailName.Text = item.Name;
        ItemDetailDescription.Text = item.Description;

        ItemDetailName.IsReadOnly = true;
        ItemDetailDescription.IsReadOnly = true;

        SaveDetailButton.IsVisible = false;
        DeleteDetailButton.IsVisible = true;

        PopupOverlay.IsVisible = true;
        ItemDetailPopup.IsVisible = true;

        ((CollectionView)sender).SelectedItem = null;
    }

    private void AddItemClicked(object sender, EventArgs e)
    {
        isCreatingNew = true;
        isAbilityMode = false;  // viktigt!
        currentDetailItem = null;

        ItemDetailName.Text = "";
        ItemDetailDescription.Text = "";

        ItemDetailName.IsReadOnly = false;
        ItemDetailDescription.IsReadOnly = false;


        DeleteDetailButton.IsVisible = false;
        SaveDetailButton.IsVisible = true;
        SaveDetailButton.Text = "Lägg till";

        PopupOverlay.IsVisible = true;
        ItemDetailPopup.IsVisible = true;

    }

    private void InventoryItemTapped(object sender, EventArgs e)
    {
        var label = sender as Label;
        var item = label?.BindingContext as Item;
        if (item == null) return;

        // öppna detalj-popup (som tidigare)
        currentDetailItem = item;
        isCreatingNew = false;
        isAbilityMode = false;

        ItemDetailName.Text = item.Name;
        ItemDetailDescription.Text = item.Description;

        ItemDetailName.IsReadOnly = true;
        ItemDetailDescription.IsReadOnly = true;

        SaveDetailButton.IsVisible = false;
        DeleteDetailButton.IsVisible = true;

        PopupOverlay.IsVisible = true;
        ItemDetailPopup.IsVisible = true;
    }

    //------------Gäller även FÄRDIGHETER-------------
    private async void DeleteItemFromDetailPopup(object sender, EventArgs e)
    {
        Item currentItem = isAbilityMode ? currentAbilityDetail : currentDetailItem;
        if (currentItem == null) return;

        bool confirm = await DisplayAlert(
            "Ta bort",
            $"Vill du verkligen ta bort \"{currentItem.Name}\"?",
            "Ja",
            "Nej");

        if (!confirm) return;

        if (isAbilityMode)
        {
            currentItem = character.Abilities.FirstOrDefault(i => i.Equals(currentItem));
            if (currentItem != null)
            {
                character.Abilities.Remove(currentItem);
                SaveFullCharacter();
            }
        }
        else
        {
            currentItem = character.Inventory.FirstOrDefault(i => i.Equals(currentItem));
            if (currentItem != null)
            {
                character.Inventory.Remove(currentItem);
                SaveFullCharacter();
            }
        }

        // Stäng bara detail-popup, behåll listan öppen
        ItemDetailPopup.IsVisible = false;

        // Behåll overlayen synlig om inventory/abilities är öppen
        if ((isAbilityMode && AbilitiesPopup.IsVisible) || (!isAbilityMode && InventoryPopup.IsVisible))
        {
            PopupOverlay.IsVisible = true; // behåll overlay
        }
        else
        {
            PopupOverlay.IsVisible = false;
        }

        // Nollställ endast detalj-item
        currentAbilityDetail = null;
        currentDetailItem = null;
        isCreatingNew = false;
    }



    // ---------------- FÄRDIGHETER ----------------
    private void AbilitiesBtn_Clicked(object sender, EventArgs e)
    {
        AbilitiesListView.ItemsSource = character.Abilities;
        ShowPopup(AbilitiesPopup);
    }

    private void AbilitiesBackClicked(object sender, EventArgs e)
    {
        ClosePopups();
    }

    private async void AbilitySelected(object sender, SelectionChangedEventArgs e)
    {
        var ability = e.CurrentSelection.FirstOrDefault() as Item;
        var item = character.Inventory.FirstOrDefault(i => i.Name == currentAbilityDetail.Name && i.Description == currentAbilityDetail.Description);
        currentDetailItem = item;

        if (ability == null) return;

        // Visa ability-detaljer på popup
        ShowAbilityDetailOnTop(ability);

        // Deselect item för att kunna klicka igen senare
        ((CollectionView)sender).SelectedItem = null;
    }

    private void AddAbilityClicked(object sender, EventArgs e)
    {
        isCreatingNew = true;
        isAbilityMode = true;

        currentAbilityDetail = null;
        currentDetailItem = null;

        ItemDetailName.Text = "";
        ItemDetailDescription.Text = "";

        ItemDetailName.IsReadOnly = false;
        ItemDetailDescription.IsReadOnly = false;

        SaveDetailButton.IsVisible = true;
        SaveDetailButton.Text = "Lägg till";
        DeleteDetailButton.IsVisible = false;

        PopupOverlay.IsVisible = true;
        ItemDetailPopup.IsVisible = true;
    }


    private void AbilityItemTapped(object sender, EventArgs e)
    {
        var label = sender as Label;
        var ability = label?.BindingContext as Item;
        if (ability == null) return;

        // Sätt alltid både variabler
        currentAbilityDetail = ability;
        currentDetailItem = ability;

        isCreatingNew = false;
        isAbilityMode = true;

        ItemDetailName.Text = ability.Name;
        ItemDetailDescription.Text = ability.Description;

        ItemDetailName.IsReadOnly = true;
        ItemDetailDescription.IsReadOnly = true;

        SaveDetailButton.IsVisible = false;
        DeleteDetailButton.IsVisible = true;

        PopupOverlay.IsVisible = true;
        ItemDetailPopup.IsVisible = true;
    }

    private void ShowAbilityDetailOnTop(Item ability)
    {
        // Sätt alltid båda
        currentAbilityDetail = ability;
        currentDetailItem = ability;

        isCreatingNew = false;
        isAbilityMode = true;

        ItemDetailName.Text = ability.Name;
        ItemDetailDescription.Text = ability.Description;

        ItemDetailName.IsReadOnly = true;
        ItemDetailDescription.IsReadOnly = true;

        SaveDetailButton.IsVisible = false;
        DeleteDetailButton.IsVisible = true;

        PopupOverlay.IsVisible = true;
        ItemDetailPopup.IsVisible = true;
    }

    // ---------------Inventory och Färdigheter SAVE -----------------

    private void SaveItemDetail(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ItemDetailName.Text))
            return;

        var newItem = new Item
        {
            Name = ItemDetailName.Text,
            Description = ItemDetailDescription.Text ?? ""
        };

        if (isAbilityMode)
        {
            // Lägg till ability
            character.Abilities.Add(newItem);
        }
        else
        {
            // Lägg till inventory-item
            character.Inventory.Add(newItem);
        }

        SaveFullCharacter();

        // Stäng bara detalj-popupen
        ItemDetailPopup.IsVisible = false;

        // Behåll overlayen synlig om inventory/abilities fortfarande är öppen
        if ((isAbilityMode && AbilitiesPopup.IsVisible) || (!isAbilityMode && InventoryPopup.IsVisible))
        {
            PopupOverlay.IsVisible = true; // behåll overlay
        }
        else
        {
            PopupOverlay.IsVisible = false;
        }

        // Nollställ detalj-state
        isCreatingNew = false;
        currentAbilityDetail = null;
        currentDetailItem = null;
    }



    // ---------------- NOTES ----------------
    // Öppna Notes-popup
    private void NotesBtn_Clicked(object sender, EventArgs e)
    {
        // Fyll editor med befintliga anteckningar
        NotesEditor.Text = character.Notes;
        ShowPopup(NotesPopup);
    }

    // Back-knapp
    private void NotesBackClicked(object sender, EventArgs e)
    {
        // Spara anteckningarna
        character.Notes = NotesEditor.Text ?? "";
        ClosePopups();
    }



    private void NotesEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        character.Notes = e.NewTextValue;
        SaveFullCharacter(); // 🔥 autosave
    }



    // ---------------- Inspiration ----------------
    private void InspirationTapped(object sender, EventArgs e)
    {
        if (sender is not BoxView box) return;

        // Hämta TapGestureRecognizer
        var tap = box.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;
        if (tap == null) return;

        // Hämta index från CommandParameter
        if (!int.TryParse(tap.CommandParameter?.ToString(), out int index)) return;

        // Byt färg
        bool isGold = box.Color != Colors.Gold;
        box.Color = isGold ? Colors.Gold : Colors.Gray;

        // Spara direkt i character
        if (index >= 0 && index < character.InspirationStatus.Length)
        {
            character.InspirationStatus[index] = isGold;

            // 🔥 Autosave direkt
            SaveFullCharacter();
        }
    }


    // ---------------- Save & Exit ----------------

    private void SaveFullCharacter()
    {
        var json = JsonSerializer.Serialize(character);
        Preferences.Set($"Character_{character.Name}", json);
    }

    private async void ExitClicked(object sender, EventArgs e)
    {
        //SaveCharacter();
        SaveFullCharacter();

        // Uppdatera MainPage
        var mainPage = Application.Current.MainPage.Navigation.NavigationStack.FirstOrDefault(p => p is MainPage) as MainPage;
        mainPage?.UpdateCharacter(character);

        // Navigera till MainPage
        await Application.Current.MainPage.Navigation.PopToRootAsync();
    }
    private string InventoryFile => Path.Combine(FileSystem.AppDataDirectory, $"{character.Name}_inventory.json");

}




