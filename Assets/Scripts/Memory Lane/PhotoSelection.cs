using UnityEngine;
using UnityEngine.EventSystems;

public class PhotoSelection : MonoBehaviour
{
    public GameObject albumGameObject;
    public GameObject collageGameObject;
    public GameObject broomPhoto;
    public GameObject lampPhoto;
    public GameObject broomLamp;
    public GameObject collageYes;

    public ScreenshotEffect screenshotEffect;

    public bool isCollaged = false;

    private void Awake()
    {
        albumGameObject.SetActive(false);
        collageGameObject.SetActive(false);
    }

    private void Update()
    {
        // Check for the Esc key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAlbumAndCollage();
        }

        if (collageGameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                collageYes.SetActive(true);
            }
        }
    }

    public void OpenAlbum()
    {
        if (!isCollaged)
        {
            if (!screenshotEffect.isTakeBroomPhoto && !screenshotEffect.isTakeLampPhoto)
            {
                albumGameObject.SetActive(true);
                broomPhoto.SetActive(false);
                lampPhoto.SetActive(false);
                broomLamp.SetActive(false);
            }
            else if (screenshotEffect.isTakeBroomPhoto && !screenshotEffect.isTakeLampPhoto)
            {
                albumGameObject.SetActive(true);
                broomPhoto.SetActive(true);
                lampPhoto.SetActive(false);
                broomLamp.SetActive(false);
            }
            else if (!screenshotEffect.isTakeBroomPhoto && screenshotEffect.isTakeLampPhoto)
            {
                albumGameObject.SetActive(true);
                broomPhoto.SetActive(false);
                lampPhoto.SetActive(true);
                broomLamp.SetActive(false);
            }
            else
            {
                albumGameObject.SetActive(true);
                broomPhoto.SetActive(true);
                lampPhoto.SetActive(true);
                broomLamp.SetActive(false);
            }
        }
        else
        {
            albumGameObject.SetActive(true);
            broomPhoto.SetActive(true);
            lampPhoto.SetActive(true);
            broomLamp.SetActive(true);
        }
    }

    public void OpenCollage()
    {
        collageGameObject.SetActive(true);
        collageYes.SetActive(false);
    }

    // Function to close both album and collage
    public void CloseAlbumAndCollage()
    {
        albumGameObject.SetActive(false);
        collageGameObject.SetActive(false);
    }

    public void Collage()
    {
        isCollaged = true;
        CloseAlbumAndCollage();
    }
}