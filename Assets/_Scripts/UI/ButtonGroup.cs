using System;
using System.Collections.Generic;
using _Scripts.Decor;
using DG.Tweening;
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
            NewDecoration.instance.SpawnNewBoxDecoration();
            DecorButtonGroup.instance.Hide();
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

        public void Show(List<Floor> mat, DecorButton decorButton)
        {
            _btn1.GetComponent<ButtonSelected>().SetFloor(mat[0], decorButton);
            _btn2.GetComponent<ButtonSelected>().SetFloor(mat[1], decorButton);
            _btn3.GetComponent<ButtonSelected>().SetFloor(mat[2], decorButton);
        
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
        }

        public void HideCompleteButton()
        {
            _btnComplete.gameObject.SetActive(false);
        }

        public void Hide()
        {
            _btn1.gameObject.SetActive(false);
            _btn2.gameObject.SetActive(false);
            _btn3.gameObject.SetActive(false);
        
            _btn1.transform.DOScale(0, 0.3f).OnPlay(() =>
            {
                _btn2.transform.DOScale(0, 0.3f).OnPlay(() =>
                {
                    _btn3.transform.DOScale(0, 0.3f);
                });
            });
        }
    }
}
