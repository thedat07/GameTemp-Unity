# 📦 SDK Versions & Integration Notes

This file tracks the versions and integration details of key SDKs used in the GameTemp Unity project.

---

## ✅ AppLovin MAX SDK

- **Version**: `8.2.0`
- **Integration Tool**: AppLovin Integration Manager
- **Features**:
  - Interstitial Ads
  - Rewarded Video Ads
  - Banner Ads
  - Ad Revenue Tracking (`OnAdRevenuePaidEvent`)
- **Code References**:
  - `AdsModelView.cs`
  - `AdsModel.cs`
- **Notes**:
  - Auto-load on close is supported
  - `MaxSdk.SetUserId` used for user tracking
---

## ✅ Firebase SDK

- **Version**:
  - Firebase Core: `12.5.0`
  - Firebase Analytics: `12.5.0`
- **Integration**: `.unitypackage`
- **Features**:
  - Analytics tracking (`LogEvent`)
- **Code References**:
  - `FirebaseEvent.cs`, `FirebaseController.cs`
- **Notes**:
  - Uses wrapper class for easier extension
  - Consider enabling Crashlytics if needed

---

## ✅ Appsflyer SDK

- **Version**: `6.15.2`
- **Integration**: Manual import
- **Features**:
  - Attribution Tracking
  - Conversion Data Handling
- **Code References**:
  - `AppsFlyerObjectScript.cs`
- **Notes**:
  - `devKey` and `appId` are configured at runtime
  - Handles `OnConversionDataSuccess` and `OnAppOpenAttribution`

---

## ✅ Facebook SDK

- **Version**: `17.0.0`
- **Integration**: Unity `.unitypackage`
- **Features**:
  - App Activation Tracking
  - Analytics Events (`LogAppEvent`)
- **Code References**:
  - `FacebookController.cs`
- **Notes**:
  - Facebook SDK is initialized and activated on app start
  - Facebook Login is not currently used, but can be added

---

## ✅ DOTween (Tweening Library)

- **Version**: `1.2.765` (hoặc mới nhất)
- **Asset Store**: [DOTween on Unity Asset Store](https://assetstore.unity.com/publishers/19336)
- **Features**:
  - Tweening animations for transforms, UI, sequences
- **Code References**:
  - `DOFade`, `DOMove`, `DOScale`, etc.
- **Notes**:
  - Setup required via `Tools → Demigiant → DOTween Utility Panel`
  - Highly optimized and used for animation sequences in UI and gameplay

---

## ✅ UniRx (Reactive Extensions for Unity)

- **Version**: `6.2.2` (hoặc latest từ GitHub)
- **Source**: [https://github.com/neuecc/UniRx](https://github.com/neuecc/UniRx)
- **Features**:
  - Reactive programming model (`ReactiveProperty`, `Subject`, `Observable.Timer`)
  - Clean separation of logic and UI
- **Notes**:
  - Makes async state and event management cleaner
  - Lightweight and no dependencies

---

## ✅ Mobile Ads v2.0 (Integration Layer)

- **Package**: [Mobile Ads v2.0 - Unity Asset Store](https://assetstore.unity.com/packages/tools/integration/mobile-ads-v2-0-266331)
- **Purpose**:
  - Acts as a wrapper layer for multiple ad networks (Applovin, Unity Ads, AdMob...)
- **Features**:
  - Unified AdsManager
  - Pre-built reward/interstitial/banner logic
- **Code References**:
  - `AdsModelView.cs`
- **Notes**:
  - Makes integration modular and swappable
  - Includes Ad Revenue forwarding and analytics integration hooks

---

## ✅ Easy IAP v2.0 (In-App Purchase System)

- **Package**: [Easy IAP v2.0 on Unity Asset Store](https://assetstore.unity.com/packages/tools/integration/easy-iap-in-app-purchase-v2-0-264594)
- **Purpose**: Simplify and unify IAP across platforms (Google Play, iOS)
- **Features**:
  - Simple purchase API
  - Cross-platform support
  - Receipt validation support
- **Code References**:
  - `ShopModelView.cs`
- **Notes**:
  - Designed to be plug-and-play
  - Can extend with custom listeners for analytics or special logic

---

## 🛠 Unity & Build Info

- **Unity Version**: `2022.3.x LTS`
- **Supported Platforms**:
  - Android (Gradle)
  - iOS (Xcode)

---

## 📌 Update Tips

- AppLovin MAX SDK: check [latest releases](https://dash.applovin.com/documentation/mediation/unity/getting-started/integration#step-1)
- Firebase: use Unity SDK updater or download `.unitypackage`
- Appsflyer: check [AppsFlyer Unity GitHub](https://github.com/AppsFlyerSDK/AppsFlyerUnityPlugin)
- Facebook: use Unity Package Manager or official `.unitypackage`
- DOTween: update via Asset Store or GitHub (Demigiant)
- UniRx: recommend GitHub latest commit if not from UPM

