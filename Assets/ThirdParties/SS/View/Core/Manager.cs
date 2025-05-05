using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering.Universal;

namespace SS.View
{
    public class Manager : ManagerDirector
    {
        protected static string m_LoadingSceneName;
        protected static Controller m_LoadingController;

        protected static string m_MaskSceneName;
        protected static Controller m_TuotiralController;

        protected static string m_NoInternetSceneName;
        protected static Controller m_NoInternetController;

        public static string LoadingSceneName
        {
            set
            {
                m_LoadingSceneName = value;
                SceneManager.LoadScene(m_LoadingSceneName, LoadSceneMode.Additive);
            }
            get
            {
                return m_LoadingSceneName;
            }
        }

        public static string MaskSceneName
        {
            set
            {
                m_MaskSceneName = value;
                SceneManager.LoadScene(m_MaskSceneName, LoadSceneMode.Additive);
            }
            get
            {
                return m_MaskSceneName;
            }
        }


        public static string NoInternetSceneName
        {
            set
            {
                m_NoInternetSceneName = value;
                SceneManager.LoadScene(m_NoInternetSceneName, LoadSceneMode.Additive);
            }
            get
            {
                return m_NoInternetSceneName;
            }
        }

        public static bool HasTut() => m_TuotiralController != null;

        public static bool HasLoading() => m_LoadingController != null;

        static Manager()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneAnimationDuration = 0.15f;

            Object = ((GameObject)GameObject.Instantiate(Resources.Load("ManagerObject"))).GetComponent<ManagerObject>();

            Object.gameObject.name = "ManagerObject";

            Application.targetFrameRate = 60;
        }

        #region Loading
        public static void LoadingAnimation(bool active, object data = null)
        {
            if (m_LoadingController != null)
            {
                if (active)
                {
                    (m_LoadingController as DLoadingController).OnShow();
                }
                else
                {
                    (m_LoadingController as DLoadingController).OnHide();
                }
            }
        }

        public static void UpdateTextLoading(string text, float vaule)
        {
          //  (m_LoadingController as DLoadingController).DoVaule(0.5f, vaule, null);
        }
        #endregion

        #region Mask
        public static void ShowMask(PopupMaskData data)
        {
            if (m_TuotiralController != null)
            {
                (m_TuotiralController as PopupMaskController).OnShow(data);
            }
        }

        public static void ShowMask(List<PopupMaskData> datas)
        {
            if (m_TuotiralController != null)
            {
                (m_TuotiralController as PopupMaskController).OnShow(datas);
            }
        }

        public static bool UpdateMask()
        {
            if (m_TuotiralController != null)
            {
                return (m_TuotiralController as PopupMaskController).UpdateStep();
            }
            return false;
        }

        public static void HideMask()
        {
            if (m_TuotiralController != null)
                (m_TuotiralController as PopupMaskController).OnHide();
        }
        #endregion

        #region Internet
        public static void ShowNoInternet()
        {
            m_NoInternetController.OnShown();
        }
        #endregion

        #region Controller
        public static void OnShown(Controller controller)
        {
            if (controller.FullScreen && m_ControllerStack.Count > 1)
            {
                ActivatePreviousController(controller, false);
            }

            controller.OnShown();
            if (controller.Data.onShown != null)
            {
                controller.Data.onShown();
            }

            Object.ShieldOff();
        }

        public static void StartShow(Controller controller)
        {
            HideController(controller, false);
        }

        public static void OnHidden(Controller controller)
        {
            controller.OnHidden();
            if (controller.Data.onHidden != null)
            {
                controller.Data.onHidden();
            }

            Unload();

            if (m_ControllerStack.Count > 0)
            {
                var currentController = m_ControllerStack.Peek();
                currentController.OnReFocus();
            }

            Object.ShieldOff();
        }

        public static void OnFadedIn()
        {
            m_MainController.OnShown();
        }

        public static void OnFadedOut()
        {
            if (m_MainController != null)
            {
                m_MainController.OnHidden();
            }
            LoadrAsync(m_MainSceneName);
        }

        static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Get Controller
            var controller = GetController(scene);

            // Loading Scene
            if (controller.SceneName() == LoadingSceneName)
            {
                controller.SetupCanvas(90);
                m_LoadingController = controller;
                m_LoadingController.gameObject.SetActive(false);
                GameObject.DontDestroyOnLoad(m_LoadingController.gameObject);
                return;
            }

            if (controller.SceneName() == NoInternetSceneName)
            {
                controller.SetupCanvas(100);
                m_NoInternetController = controller;
                m_NoInternetController.gameObject.SetActive(false);
                GameObject.DontDestroyOnLoad(m_NoInternetController.gameObject);
                return;
            }

            if (controller.SceneName() == MaskSceneName)
            {
                controller.SetupCanvas(80);
                m_TuotiralController = controller;
                m_TuotiralController.gameObject.SetActive(false);
                GameObject.DontDestroyOnLoad(m_TuotiralController.gameObject);
                return;
            }

            // Single Mode automatically destroy all scenes, so we have to clear the stack.
            if (mode == LoadSceneMode.Single)
            {
                m_ControllerStack.Clear();
            }

            // Unload resources and collect GC.
            Resources.UnloadUnusedAssets();
            // System.GC.Collect();

            // Get Data
            if (m_DataQueue.Count == 0)
            {
                m_DataQueue.Enqueue(new Data(null, scene.name, null, null));
            }

            Data data = m_DataQueue.Dequeue();
            while (data.sceneName != scene.name && m_DataQueue.Count > 0)
            {
                data = m_DataQueue.Dequeue();
            }

            if (data == null)
            {
                data = new Data(null, scene.name, null, null);
            }

            data.scene = scene;

            // Push the current scene to the stack.
            m_ControllerStack.Push(controller);

            // Setup controller
            controller.Data = data;
            controller.hasShield = data.hasShield;
            controller.SetupCanvas(m_ControllerStack.Count - 1);
            controller.OnActive(data.data);
            controller.CreateShield();
            controller.EventShow();

            // Animation
            if (m_ControllerStack.Count == 1)
            {
                // Own Camera
                if (controller.Camera != null)
                {
                    // Thiết lập Camera chính là Base
                    var urpCameraData = controller.Camera.GetUniversalAdditionalCameraData();
                    urpCameraData.renderType = UnityEngine.Rendering.Universal.CameraRenderType.Base;
                    //   urpCameraData.cameraStack.Clear(); 

                    // Đảm bảo Camera phụ là Overlay
                    var overlayCameraData = Object.UICamera.GetUniversalAdditionalCameraData();
                    overlayCameraData.renderType = UnityEngine.Rendering.Universal.CameraRenderType.Overlay;

                    // Thêm Camera phụ vào Stack của Camera chính
                    urpCameraData.cameraStack.Add(Object.UICamera);
                }

                // Main Scene
                m_MainController = controller;
                if (string.IsNullOrEmpty(m_MainSceneName))
                {
                    m_MainSceneName = scene.name;
                }

                // Fade
                Object.FadeInScene();
            }
            else
            {
                // Popup Scene
                controller.Show();
            }
        }
        #endregion

        #region LoadingScene

        static void LoadrAsync(string sceneName)
        {
            if (HasLoading())
            {
                // if (sceneName == HomeGameController.HOMEGAME_SCENE_NAME)
                //     GameManager.Instance.GetAdsPresenter().ShowInterstitial(null, "");

                Manager.LoadingAnimation(true);
                (m_LoadingController as DLoadingController).sceneName = sceneName;
                Load();
            }
            else
            {
                SceneManager.LoadScene(m_MainSceneName, LoadSceneMode.Single);
            }
        }

        static void Load()
        {
            m_LoadingController.StopAllCoroutines();
            m_LoadingController.StartCoroutine(LoadYourAsyncScene((m_LoadingController as DLoadingController).sceneName));
        }

        static IEnumerator LoadYourAsyncScene(string sceneName)
        {
            Manager.UpdateTextLoading("UnZip", 0.8f);

            yield return new WaitForSeconds(0.6f);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!asyncLoad.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        #endregion
    }
}
