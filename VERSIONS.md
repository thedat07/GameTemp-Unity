# 📦 SDK Versions & Integration Notes

This file tracks the versions and integration details of key SDKs used in the GameTemp Unity project.

---

## ✅ AppLovin MAX SDK

- **Version**: `11.11.3`
- **Integration Tool**: AppLovin Integration Manager
- **Features**:
  - Interstitial Ads
  - Rewarded Video Ads
  - Banner Ads
  - Ad Revenue Tracking (`OnAdRevenuePaidEvent`)
- **Code References**:
  - `AdsManager.cs`
  - `AdRevenueManager.cs`
- **Notes**:
  - Auto-load on close is supported
  - `MaxSdk.SetUserId` used for user tracking

---

## ✅ Firebase SDK

- **Version**:
  - Firebase Core: `11.6.0`
  - Firebase Analytics: `11.6.0`
- **Integration**: `.unitypackage`
- **Features**:
  - Analytics tracking (`LogEvent`)
- **Code References**:
  - `FirebaseEvent.cs`
- **Notes**:
  - Uses wrapper class for easier extension
  - Consider enabling Crashlytics if needed

---

## ✅ Appsflyer SDK

- **Version**: `6.12.0`
- **Integration**: Manual import
- **Features**:
  - Attribution Tracking
  - Conversion Data Handling
- **Code References**:
  - `AppsflyerManager.cs`
- **Notes**:
  - `devKey` and `appId` are configured at runtime
  - Handles `OnConversionDataSuccess` and `OnAppOpenAttribution`

---

## ✅ Facebook SDK

- **Version**: `11.0.0`
- **Integration**: Unity `.unitypackage`
- **Features**:
  - App Activation Tracking
  - Analytics Events (`LogAppEvent`)
- **Code References**:
  - `FacebookManager.cs`
- **Notes**:
  - Facebook SDK is initialized and activated on app start
  - Facebook Login is not currently used, but can be added

---

## 🛠 Unity & Build Info

- **Unity Version**: `2022.3.x LTS`
- **Supported Platforms**:
  - Android (Gradle)
  - iOS (Xcode)

---

## 📌 Update Tips

- AppLovin MAX SDK: check [latest releases](https://dash.applovin.com/documentation/mediation/unity/getting-started/integration#step-1)
- Firebase: use Unity SDK updater or download new `.unitypackage`
- Appsflyer: refer to [official Appsflyer Unity GitHub](https://github.com/AppsFlyerSDK/AppsFlyerUnityPlugin)
- Facebook: use Unity Package or manual `.unitypackage` from Meta

