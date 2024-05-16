using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int playerScore;
    public float playerHealth;

    // Dodaj dodatkowe pola gracza, które chcesz zapisywać
}

public class SaveLoadManager : MonoBehaviour
{
    // Nazwa klucza do zapisywania danych gracza w PlayerPrefs
    private const string PlayerPrefsKey = "PlayerData";

    // Dane gracza
    private PlayerData playerData;

    void Start()
    {
        LoadPlayerData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }

    void SavePlayerData()
    {
        // Przygotuj dane gracza
        playerData = new PlayerData
        {
            playerScore = 100,          
            playerHealth = 0.8f         
            // Dodaj inne dane gracza, które chcesz zapisywać
        };

        // Zamień dane gracza na JSON
        string playerDataJson = JsonUtility.ToJson(playerData);

        // Zapisz JSON w PlayerPrefs
        PlayerPrefs.SetString(PlayerPrefsKey, playerDataJson);
        PlayerPrefs.Save();

        Debug.Log("Zapisano stan gry gracza.");
    }

    void LoadPlayerData()
    {
        string playerDataJson = PlayerPrefs.GetString(PlayerPrefsKey);

        // Jeśli JSON nie jest pusty, wczytaj dane gracza
        if (!string.IsNullOrEmpty(playerDataJson))
        {
            // Zamień JSON na obiekt PlayerData
            playerData = JsonUtility.FromJson<PlayerData>(playerDataJson);

            // Wykorzystaj wczytane dane do uaktualnienia stanu gry gracza
            UpdatePlayerState();

            Debug.Log("Wczytano stan gry gracza.");
        }
        else
        {
            Debug.Log("Nie znaleziono zapisanego stanu gry gracza.");
        }
    }

    void UpdatePlayerState()
    {
        
        
        
    }
}
