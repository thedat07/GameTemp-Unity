using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using SS.View;
using UnityEngine.Events;

[System.Serializable]
public class ToolInfo
{
    public int clickCountMax = 3;
    public float holdTime = 3.0f; // Thời gian giữ để kích hoạt sự kiện
    public int clickCount = 0;
    public float lastClickTime = 0;
    public float holdTimer = 0;
    public bool isHolding = false;
    public bool isSwiping = false;
    public Vector2 initialMousePosition;
    public float swipeThreshold = 50f; // Khoảng cách vuốt tối thiểu để nhận diện vuốt

    public UnityAction unityAction;

    public void Update()
    {
        // Kiểm tra nếu nhấn chuột
        if (Input.GetMouseButtonDown(0))
        {
            float currentTime = Time.time;

            // Nếu thời gian giữa các lần nhấn chuột quá dài, đặt lại số lần nhấn
            if (currentTime - lastClickTime > 0.5f)
            {
                clickCount = 0;
            }

            // Cập nhật số lần nhấn và thời gian nhấn gần nhất
            clickCount++;
            lastClickTime = currentTime;

            // Kiểm tra nếu đã nhấn 3 lần
            if (clickCount >= clickCountMax)
            {
                initialMousePosition = Input.mousePosition;
                isSwiping = true;
            }
        }

        // Kiểm tra nếu đang vuốt chuột
        if (isSwiping)
        {
            // Nếu chuột đang được giữ và vuốt
            if (Input.GetMouseButton(0))
            {
                Vector2 currentMousePosition = Input.mousePosition;
                float distance = Vector2.Distance(currentMousePosition, initialMousePosition);

                // Nếu vuốt đủ khoảng cách, chuyển sang chế độ giữ
                if (distance >= swipeThreshold)
                {
                    isHolding = true;
                    isSwiping = false;
                }
            }
            else
            {
                // Nếu thả chuột trước khi đủ khoảng cách vuốt, hủy bỏ
                isSwiping = false;
                if (clickCount < clickCountMax)
                {
                    clickCount = 0;
                }
            }
        }

        // Kiểm tra nếu đang giữ chuột
        if (isHolding)
        {
            if (Input.GetMouseButton(0))
            {
                holdTimer += Time.deltaTime;

                // Nếu giữ đủ thời gian, kích hoạt sự kiện
                if (holdTimer >= holdTime)
                {
                    TriggerEvent();
                    isHolding = false;
                    if (clickCount < clickCountMax)
                    {
                        clickCount = 0;
                    }
                }
            }
            else
            {
                // Nếu thả chuột trước khi đủ thời gian, hủy bỏ
                isHolding = false;
                if (clickCount < clickCountMax)
                {
                    clickCount = 0;
                }
            }
        }
    }

    void TriggerEvent()
    {
        unityAction?.Invoke();
    }
}
