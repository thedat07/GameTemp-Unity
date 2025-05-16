using DesignPatterns;
using UnityEngine;

namespace Directory
{
    public class ManagerObject : SingletonPersistent<ManagerObject>
    {
        [SerializeField] Camera m_UiCamera;


        public Camera UICamera => m_UiCamera;


        public void PlayEffect()
        {

        }
    }
}