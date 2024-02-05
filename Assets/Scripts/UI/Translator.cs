
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class Translator : MonoBehaviour
{
    public Translation[] translations = new Translation[] { new Translation(SystemLanguage.English) };

    // static public string selectedLangageName = "English";
    static public SystemLanguage selectedLangage = SystemLanguage.English;

    TMP_Text textObject;

    private void Awake()
    {

        textObject = GetComponent<TMP_Text>();

    }

    private void Start()
    {

        FillEmptyTranslations();

        Translate();

    }

    private void Update()
    {

        Translate();

    }

    public void Translate()
    {

        int languageIndex = FindLanguageIndex();

        textObject.text = translations[languageIndex].text;

    }

    public int FindLanguageIndex()
    {
        return FindLanguageIndex(selectedLangage);
    }

    public int FindLanguageIndex(SystemLanguage language)
    {
        for (int i = 0; i < translations.Length; i++)
        {
            if (translations[i].language == language) { return i; }
        }
        return 0;
    }


    public int FindLanguageIndex(string translationName)
    {
        for (int i = 0; i < translations.Length; i++)
        {
            if (translations[i].name == translationName) { return i; }
        }
        return 0;
    }

    public static int FindLanguageIndex(Translation[] translations)
    {

        for (int i = 0; i < translations.Length; i++)
        {

            if (translations[i].language == Application.systemLanguage) { return i; }

        }

        return 0;

    }

    public void FillEmptyTranslations()
    {

        for (int i = 0; i < translations.Length; i++)
        {

            if (translations[i].text == "") { translations[i].text = textObject.text; }

        }

    }

    [System.Serializable]
    public struct Translation
    {

        public string name;
        public SystemLanguage language;
        [TextArea] public string text;

        public Translation(SystemLanguage language)
        {

            name = language.ToString();

            this.language = language;

            text = "";

        }

        public Translation(Translation translation, string text)
        {
            name = translation.name;
            language = translation.language;
            this.text = text;
        }

    }

}