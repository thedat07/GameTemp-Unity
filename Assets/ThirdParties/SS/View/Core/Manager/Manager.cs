using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering.Universal;

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

            if (((GameObject)GameObject.Instantiate(Resources.Load("ManagerObject"))).TryGetComponent<ManagerObject>(out ManagerObject o))
            {
                Object = o;
            }

            Object.gameObject.name = "ManagerObject";

            SetTargetFrameRateForMobile();

            void SetTargetFrameRateForMobile()
            {
                int ramMB = SystemInfo.systemMemorySize; // MB

                int targetFrameRate = 30; // mặc định thấp

#if UNITY_ANDROID || UNITY_IOS
                if (ramMB >= 4096) // 4GB trở lên
                {
                    targetFrameRate = 60;
                }
                else if (ramMB >= 2048) // 2GB - 4GB
                {
                    targetFrameRate = 45; // trung bình
                }
                else
                {
                    targetFrameRate = 30; // thấp
                }
#else
                targetFrameRate = 60; // PC, console hoặc editor
#endif

                Application.targetFrameRate = targetFrameRate;
            }
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
            var controller = GetController(scene);

            if (controller != null)
            {

                if (string.Equals(controller.SceneName(), MaskSceneName, System.StringComparison.Ordinal))
                {
                    SettingController(ref m_MaskController, controller, 80);
                    return;
                }
                if (string.Equals(controller.SceneName(), LoadingSceneName, System.StringComparison.Ordinal))
                {
                    SettingController(ref m_LoadingController, controller, 90);
                    return;
                }
                if (string.Equals(controller.SceneName(), NoInternetSceneName, System.StringComparison.Ordinal))
                {
                    SettingController(ref m_NoInternetController, controller, 100);
                    return;
                }

                if (mode == LoadSceneMode.Single)
                {
                    m_ControllerStack.Clear();
                }

                // Xem xét gọi Resources.UnloadUnusedAssets() ở chỗ khác phù hợp hơn
                // Resources.UnloadUnusedAssets();

                if (m_DataQueue.Count == 0)
                {
                    m_DataQueue.Enqueue(new Data(null, scene.name, null, null));
                }

                Data data = null;
                while (m_DataQueue.Count > 0)
                {
                    var peekData = m_DataQueue.Peek();
                    if (string.Equals(peekData.sceneName, scene.name, System.StringComparison.Ordinal))
                    {
                        data = m_DataQueue.Dequeue();
                        break;
                    }
                    else
                    {
                        // Bỏ qua data không đúng scene, tránh dequeue nhiều lần vô ích
                        m_DataQueue.Dequeue();
                    }
                }

                if (data == null)
                {
                    data = new Data(null, scene.name, null, null);
                }
                data.scene = scene;

                m_ControllerStack.Push(controller);

                controller.Data = data;
                controller.HasShield = data.hasShield;

                int stackCount = m_ControllerStack.Count;
                controller.SetupCanvas(stackCount - 1);
                controller.OnActive(data.data);
                controller.CreateShield();
                controller.EventShow();

                if (stackCount == 1)
                {
                    if (controller.Camera != null)
                    {
                        var urpCameraData = controller.Camera.GetUniversalAdditionalCameraData();
                        urpCameraData.renderType = UnityEngine.Rendering.Universal.CameraRenderType.Base;

                        var overlayCameraData = Object.UICamera.GetUniversalAdditionalCameraData();
                        overlayCameraData.renderType = UnityEngine.Rendering.Universal.CameraRenderType.Overlay;

                        urpCameraData.cameraStack.Add(Object.UICamera);
                    }

                    m_MainController = controller;

                    if (string.IsNullOrEmpty(m_MainSceneName))
                    {
                        m_MainSceneName = scene.name;
                    }

                    Object.FadeInScene();
                }
                else
                {
                    controller.Show();
                }
            }
        }

        private static void SettingController(ref Controller controllerRef, Controller controller, int sortingOrder)
        {
            controllerRef = controller;
            controllerRef.HasShield = false;
            controllerRef.FullScreen = false;
            controllerRef.UseCameraUI = true;
            controller.SetupCanvas(sortingOrder);
            controllerRef.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(controllerRef.gameObject);
        }
        #endregion

        #region LoadingScene

        static void LoadrAsync(string sceneName)
        {
            if (HasLoading())
            {
                Manager.LoadingAnimation(true);

                // Dừng hết Coroutine đang chạy trên Object để tránh chạy song song
                Object.StopAllCoroutines();

                // Bắt đầu Coroutine load scene async có cleanup bộ nhớ trước
                Object.StartCoroutine(LoadSceneWithCleanup(sceneName));
            }
            else
            {
                // Trường hợp không có loading, load scene chính theo tên truyền vào
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }

        static IEnumerator LoadSceneWithCleanup(string sceneName)
        {
            // Đợi 0.5s trước để có thể hiện loading animation hoặc chuẩn bị dọn dẹp
            yield return new WaitForSeconds(0.5f);

            // Dọn tài nguyên không còn sử dụng (chờ xong)
            yield return Resources.UnloadUnusedAssets();

            // Có thể gọi GC.Collect nếu muốn (cẩn thận với giật lag)
            // System.GC.Collect();

            // Bắt đầu load scene async với chế độ Single (thay thế scene hiện tại)
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            // Đợi load xong
            while (!asyncLoad.isDone)
            {
                // Có thể theo dõi tiến độ asyncLoad.progress ở đây nếu muốn
                yield return null; // yield return null thay cho WaitForEndOfFrame để mượt hơn
            }

            // Tắt loading animation khi load xong
            Manager.LoadingAnimation(false);
        }
        #endregion
    }
}
