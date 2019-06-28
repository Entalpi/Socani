using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Monetization;

public class BannerAd : MonoBehaviour {
  public string gameId = "5e11bab8-3eef-421f-9679-391270e942d1";
  public string placementId = "AdBanner";
  public bool testMode = true;

  void Start() {
    Monetization.Initialize(gameId, testMode);
    Advertisement.Initialize(gameId, testMode);
    StartCoroutine(ShowBannerWhenReady());

    if (Advertisement.isSupported) {
      Debug.Log("Advertisement is supported.");
    }
  }

  IEnumerator ShowBannerWhenReady() {
    while (!Advertisement.IsReady(placementId)) {
      yield return new WaitForSeconds(0.5f);
    }
    Advertisement.Banner.Show(placementId);
    Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
  }
}
