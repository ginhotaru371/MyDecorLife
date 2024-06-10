using System;
using System.Collections.Generic;
using _Scripts.Decor;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Type = _Scripts.Decor.Type;

namespace _Scripts.UI
{
    public class ButtonGroup : Singleton<ButtonGroup>
    {
        private Button _btn1;
        private Button _btn2;
        private Button _btn3;
        private Button _btnComplete;

        public override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        
            Init();
            Hide();
        }

        private void Init()
        {
            _btn1 = transform.GetChild(0).GetComponent<Button>();
            _btn2 = transform.GetChild(1).GetComponent<Button>();
            _btn3 = transform.GetChild(2).GetComponent<Button>();
            _btnComplete = transform.GetChild(3).GetComponent<Button>();
        }

        private void Start()
        {
            _btnComplete.onClick.AddListener(CompleteSetup);
        }

        private void CompleteSetup()
        {
            if (PaintManager.instance.Paintable())
            {
                MousePainter.instance.PaintComplete();
                PaintManager.instance.Painting(false);
                Hide();
                NewFurniture.instance.SpawnNewFurniture();
                UnSelectedButton();
            }
            else
            {
                NewDecoration.instance.SpawnNewBoxDecoration();
                DecorButtonGroup.instance.Hide();
            }
        }

        public void Show(List<Wall> wall, DecorButton decorButton)
        {
            _btn1.GetComponent<ButtonSelected>().SetWallColor(wall[0], decorButton);
            _btn2.GetComponent<ButtonSelected>().SetWallColor(wall[1], decorButton);
            _btn3.GetComponent<ButtonSelected>().SetWallColor(wall[2], decorButton);
        
            _btn1.GetComponent<ButtonSelected>().SetType(Type.Wall);
            _btn2.GetComponent<ButtonSelected>().SetType(Type.Wall);
            _btn3.GetComponent<ButtonSelected>().SetType(Type.Wall);

            Show();
        }

        public void Show(List<Floor> texture, DecorButton decorButton)
        {
            _btn1.GetComponent<ButtonSelected>().SetFloor(texture[0], decorButton);
            _btn2.GetComponent<ButtonSelected>().SetFloor(texture[1], decorButton);
            _btn3.GetComponent<ButtonSelected>().SetFloor(texture[2], decorButton);
        
            _btn1.GetComponent<ButtonSelected>().SetType(Type.Floor);
            _btn2.GetComponent<ButtonSelected>().SetType(Type.Floor);
            _btn3.GetComponent<ButtonSelected>().SetType(Type.Floor);
        
            Show();
        }

        public void Show(List<Furniture> furnitures, DecorButton decorButton)
        {   
       
            _btn1.GetComponent<ButtonSelected>().SetFurniture(furnitures[0], decorButton);
            _btn2.GetComponent<ButtonSelected>().SetFurniture(furnitures[1], decorButton);
            _btn3.GetComponent<ButtonSelected>().SetFurniture(furnitures[2], decorButton);
        
            _btn1.GetComponent<ButtonSelected>().SetType(Type.Furniture);
            _btn2.GetComponent<ButtonSelected>().SetType(Type.Furniture);
            _btn3.GetComponent<ButtonSelected>().SetType(Type.Furniture);
        
            Show();
        }

        public void Show()
        {
            _btn1.transform.DOScale(1, 0.7f).SetEase(Ease.OutBack).OnPlay(() =>
            {
                _btn1.gameObject.SetActive(true);
                _btn2.transform.DOScale(1, 0.7f).SetDelay(0.1f).SetEase(Ease.OutBack).OnPlay(() =>
                {
                    _btn2.gameObject.SetActive(true);
                    _btn3.transform.DOScale(1, 0.7f).SetDelay(0.2f).SetEase(Ease.OutBack).OnPlay(() =>
                    {
                        _btn3.gameObject.SetActive(true);
                    });
                });
            });
        }

        public void ShowCompleteButton()
        {
            _btnComplete.gameObject.SetActive(true);
            _btnComplete.interactable = true;
        }

        public void HideCompleteButton()
        {
            _btnComplete.gameObject.SetActive(false);
            _btnComplete.interactable = false;
        }

        public void Hide()
        {
            _btn1.gameObject.SetActive(false);
            _btn2.gameObject.SetActive(false);
            _btn3.gameObject.SetActive(false);

            _btn1.transform.localScale = new Vector3(0, 0, 0);
            _btn2.transform.localScale = new Vector3(0, 0, 0);
            _btn3.transform.localScale = new Vector3(0, 0, 0);
        }
        
        
        public void SelectedButton(ButtonSelected btn)
        {
            foreach (Transform child in transform)
            {
                if (child != btn.transform)
                {
                    var newBtn = child.GetComponent<ButtonSelected>();
                    newBtn.Selected(false);
                    btn.Selected(true);
                }
            }
        }

        public void UnSelectedButton()
        {
            foreach (Transform child in transform)
            {
                var newBtn = child.GetComponent<ButtonSelected>();
                newBtn.Selected(false);
            }
        }
    }
}
