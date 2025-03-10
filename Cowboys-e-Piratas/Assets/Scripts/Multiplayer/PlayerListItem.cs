using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int connectionID;
    public ulong playerSteamID;
    private bool avatarReceived;

    public TMP_Text nameText;
    public RawImage avatarImage;

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
        if(!avatarReceived) GetPlayerIcon();
    }
}
