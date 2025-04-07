using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int connectionID;
    public ulong playerSteamID;
    public TMP_Text nameText;
    public RawImage avatarImage;
    public TMP_Text playerReadyText;
    public bool ready;

    protected Callback<AvatarImageLoaded_t> AvatarImageLoaded;

    void Start()
    {
        AvatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
    }

    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback){
        if(callback.m_steamID.m_SteamID == playerSteamID){
            avatarImage.texture = GetSteamImageAsTexture(callback.m_iImage);
        }else{
            return;
        }
    }

    private void GetPlayerIcon(){
        int imageID = SteamFriends.GetLargeFriendAvatar(new CSteamID(playerSteamID));
        if(imageID == -1){
            return;
        }
        if(imageID > 0){
            avatarImage.texture = GetSteamImageAsTexture(imageID);
        }
    }

    private Texture2D GetSteamImageAsTexture(int iImage){
        Texture2D texture = null;
        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if(isValid){
            byte[] image = new byte[width * height * 4];
            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));
            if(isValid){
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        return texture;
    }

    public void SetPlayerValues(){
        nameText.text = playerName;
        ChangeReadyStatus();
        GetPlayerIcon();
    }

    public void ChangeReadyStatus(){
        if(ready){
            playerReadyText.text = "Ready";
            playerReadyText.color = Color.green;
        }
        else{
            playerReadyText.text = "Unready";
            playerReadyText.color = Color.red;
        }
    }

    void OnDestroy()
    {
        LobbyController.instance.playerListItems.Remove(this);
        Debug.Log("Destroying PlayerListItem: " + playerName);
    }
}
