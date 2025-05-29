using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering.Universal;
using YNL.Utilities.Extensions;

namespace Directory
{
    public class Manager : ManagerDirector
    {
        protected static string m_LoadingSceneName;
        protected static Controller m_LoadingController;

        protected static string m_MaskSceneName;
        protected static Controller m_MaskController;

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

        public static bool HasMask() => m_MaskController != null;

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
        public static void LoadingAnimation(bool active)
        {
            if (m_LoadingController != null)
            {
                if (active)
                {
                    (m_LoadingController as ILoading).ShowLoading();
                }
                else
                {
                    (m_LoadingController as ILoading).HideLoading();
                }
            }
        }
        #endregion

        #region Mask
        public static void ShowMask(PopupMaskData data)
        {
            if (HasMask())
                (m_MaskController as IMask).ShowMask(data);
        }

        public static void ShowMask(List<PopupMaskData> datas)
        {
            if (HasMask())
                (m_MaskController as IMask).ShowMask(datas);
        }

        public static bool UpdateMaskStep()
        {
            if (HasMask())
                return (m_MaskController as IMask).UpdateMaskStep();
            return false;
        }

        public static void HideMask()
        {
            if (HasMask())
                (m_MaskController as IMask).HideMask();
        }
        #endregion

        #region Internet
        public static void ShowNoInternet()
        {
            (m_NoInternetController as INoInternet).OnShownInternet();
        }
        #endregion

        #region Controller
        public static void OnShown(Controller controller)
        {
            if (controller.FullScreen && m_ControllerStack.Count > 1)
            {
                ActivatePreviousController(controller, false);
            }
            
            controller.SetDelay(Manager.SceneAnimationDuration, () =>
            {
                controller.OnShown();
                if (controller.Data != null && controller.Data.onShown != null)
                {
                    controller.Data.onShown();
                }
            });

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

            if (controller == null)
                return;

            // Loading Scene
            if (controller.SceneName() == MaskSceneName)
            {
                SettingController(ref m_MaskController, 80);
                return;
            }

            if (controller.SceneName() == LoadingSceneName)
            {
                SettingController(ref m_LoadingController, 90);
                return;
            }

            if (controller.SceneName() == NoInternetSceneName)
            {
                SettingController(ref m_NoInternetController, 100);
                return;
            }

            void SettingController(ref Controller controllerRef, int sortingOrder)
            {
                controllerRef = controller;
                controllerRef.HasShield = false;
                controllerRef.FullScreen = false;
                controllerRef.UseCameraUI = true;
                controller.SetupCanvas(sortingOrder);
                controllerRef.gameObject.SetActive(false);
                GameObject.DontDestroyOnLoad(controllerRef.gameObject);
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
            controller.HasShield = data.hasShield;
            controller.SetupCanvas(m_ControllerStack.Count - 1);
            controller.OnActive(data.data);
            controller.CreateShield();
            controller.EventShow();
            // Animation
            if (m_ControllerStack.Count == 1)
            {
                MCamera.SetupBaseAndOverlayCameras(controller.Camera, Object.UICamera);

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
                Manager.LoadingAnimation(true);
                m_LoadingController.StopAllCoroutines();
                m_LoadingController.StartCoroutine(LoadYourAsyncScene(sceneName));
            }
            else
            {
                SceneManager.LoadScene(m_MainSceneName, LoadSceneMode.Single);
            }
        }

        static IEnumerator LoadYourAsyncScene(string sceneName)
        {
            yield return new WaitForSeconds(0.5f);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            while (!asyncLoad.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            Manager.LoadingAnimation(false);
        }
        #endregion
    }
}
