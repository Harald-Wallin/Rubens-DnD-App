
namespace Rubens_DnD__project;

public partial class NewProfilePage : ContentPage
{
    /*Attribut minimum-värde, genereras av randomgeneratorn*/
    private int baseStrength;
    private int baseDexterity;
    private int baseIntellect;
    private int baseCharisma;
    private int baseSpirit;

    /*Attribut startvärde*/
    private int strength = 0;
    private int dexterity = 0;
    private int intellect = 0;
    private int charisma = 0;
    private int spirit = 0;

    /*Poäng att fördela*/
    private int pointsLeft = 4;

    /*Antal chanser kvar att generera attributer*/
    private int generateClicksLeft = 3;
    private Random random = new Random();

    private MainPage mainPage;

    public NewProfilePage(MainPage mainPage)
    {
        InitializeComponent();
        this.mainPage = mainPage;
        UpdateUI();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    public NewProfilePage()
	{
		InitializeComponent();
        UpdateUI();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void UpdateAttributeLabel(Label label, int current, int baseValue)
    {
        label.Text = current.ToString();

        if (current > baseValue)
            label.TextColor = Colors.LimeGreen;
        else
            label.TextColor = Colors.White; // eller Colors.Black beroende på tema
    }

    private void UpdateUI()
    {
        UpdateAttributeLabel(StrengthLabel, strength, baseStrength);
        UpdateAttributeLabel(DexterityLabel, dexterity, baseDexterity);
        UpdateAttributeLabel(IntellectLabel, intellect, baseIntellect);
        UpdateAttributeLabel(CharismaLabel, charisma, baseCharisma);
        UpdateAttributeLabel(SpiritLabel, spirit, baseSpirit);

        PointsLabel.Text = $"{pointsLeft}/4";
    }


    private void IncreaseAttribute(object sender, EventArgs e)
    {
        if (pointsLeft <= 0) return;

        Button btn = sender as Button;
        if (btn == null) return;

        string attr = btn.CommandParameter.ToString();

        switch (attr)
        {
            case "Strength": 
                strength++;
                break;

            case "Dexterity":
                dexterity++;
                break;

            case "Intellect": 
               intellect++;
                break;

            case "Charisma":
                charisma++;
                break;

            case "Spirit":
                spirit++;
                break;
        }

        pointsLeft--;
        UpdateUI();
    }

    private void DecreaseAttribute(object sender, EventArgs e)
    {
        if (pointsLeft == 4) return;


        Button btn = sender as Button;
        if (btn == null) return;

        string attr = btn.CommandParameter.ToString();

        switch (attr)
        {
            case "Strength":
                if (strength > baseStrength)
                {
                    strength--;
                    pointsLeft ++;
                }break;

            case "Dexterity":
                if (dexterity > baseDexterity)
                {
                    dexterity--;
                    pointsLeft ++;
                }break;

            case "Intellect":
                if (intellect > baseIntellect)
                {
                    intellect--;
                    pointsLeft++;
                }break;

            case "Charisma":
                if (charisma > baseCharisma)
                {
                    charisma--;
                    pointsLeft++;
                }break;

            case "Spirit":
                if (spirit > baseSpirit)
                {
                    spirit--;
                    pointsLeft++;
                }break;
        }

        UpdateUI();
    }

    private void GenerateAttributes(object sender, EventArgs e)
    {
        if (generateClicksLeft <= 0) return;

        baseStrength = random.Next(1, 7);
        baseDexterity = random.Next(1, 7);
        baseIntellect = random.Next(1, 7);
        baseCharisma = random.Next(1, 7);
        baseSpirit = random.Next(1, 7);

        strength = baseStrength;
        dexterity = baseDexterity;
        intellect = baseIntellect;
        charisma = baseCharisma;
        spirit = baseSpirit;


        pointsLeft = 4;

        generateClicksLeft--;
        GenerateCounterLabel.Text = $"{generateClicksLeft}/3";

        UpdateUI();
    }

    private async void PlayClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
           string.IsNullOrWhiteSpace(RaceEntry.Text) ||
           string.IsNullOrWhiteSpace(ClassEntry.Text))
        {
            await this.DisplayAlert("Fel", "Fyll i Namn, Klass & Ras först!", "OK");
            return;
        }

        if (pointsLeft > 0)
        {
            await this.DisplayAlert("Fel", "Fördela alla dina attribut-poäng först!", "OK");
            return;
        }

        Character newCharacter = new Character
        {
            Name = NameEntry.Text,
            Race = RaceEntry.Text,
            Class = ClassEntry.Text,

            Strength = strength,
            Dexterity = dexterity,
            Intellect = intellect,
            Charisma = charisma,
            Spirit = spirit,

            Level = 1,
            Armor = 0,
            //HP = 0,
            WizardLevel = 0
        };

        await this.DisplayAlert(
            $"{NameEntry.Text} är redo för äventyren!\n\n",
            "Karaktärsinfo\n" +
            "~~~~~~~~~~~~~\n" +
            $"{RaceEntry.Text} {ClassEntry.Text}\n" +
            $"Styrka: {strength}\n" +
            $"Smidighet: {dexterity}\n" +
            $"Intelligens: {intellect}\n" +
            $"Karisma: {charisma}\n" +
            $"Själslighet: {spirit}\n",
            "OK"
        );

        mainPage.AddCharacter( newCharacter );
        await Navigation.PushAsync(new CharacterPage(newCharacter));
        await ProfileStorage.SaveProfilesAsync(mainPage.GetCharacters());
    }

    private async void BackClicked(object sender, EventArgs e)
    {
        bool leave = await DisplayAlert(
            "Avbryta?",
            "Profilen kommer inte att sparas.",
            "Ja",
            "Nej");

        if (leave)
            await Shell.Current.GoToAsync("..");
    }



}