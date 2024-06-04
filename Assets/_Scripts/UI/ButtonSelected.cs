using System;
using _Scripts.Decor;
using UnityEngine;
using UnityEngine.UI;
using Type = _Scripts.Decor.Type;

namespace _Scripts.UI
{
    public class ButtonSelected : MonoBehaviour
    {
        private Type _type;
        private Button _button;
        private Button _buttonSelect;
        private DecorButton _buttonDecor;
        private Furniture _furniture;
        private Material _mat;
        private Color _color;
    
        public GameObject furnitureObject;


        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _buttonSelect = transform.GetChild(0).GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(Events);
            _buttonSelect.onClick.AddListener(SelectedEvents);
        }

        private void Events()
        {
            switch (_type)
            {
                case Type.Wall:
                    ChangeWallColor();
                    break;
                case Type.Floor:
                    ChangeFloor();
                    break;
                case Type.Furniture:
                    ChangeFurniture();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SelectedEvents()
        {
            switch (_type)
            {
                case Type.Wall:
                    ChangeWallColor();
                    _buttonDecor.SetBool(true);
                    break;
                case Type.Floor:
                    ChangeFloor();
                    _buttonDecor.SetBool(true);
                    break;
                case Type.Furniture:
                    ChangeFurniture();
                    _buttonDecor.SetBool(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            ButtonGroup.instance.Hide();
        }

        private void ChangeFurniture()
        {
            foreach (Transform child in transform.parent)
            {
                if (child.GetComponent<ButtonSelected>())
                {
                    child.GetComponent<ButtonSelected>().furnitureObject.SetActive(false);
                }
            }
        
            furnitureObject.SetActive(true);
        }

        private void ChangeFloor()
        {
            FloorChange.instance.Change(_mat);
        }

        private void ChangeWallColor()
        {
            MousePainter.instance.ChangePaintColor(_color);
        }

        public void SetWallColor(Wall wall, DecorButton decorButton)
        {
            _buttonDecor = decorButton;

            _button.transform.GetChild(1).GetComponent<Image>().color = wall.Color;
            _color = wall.Color;
        }

        public void SetFloor(Floor floor, DecorButton decorButton)
        {
            _buttonDecor = decorButton;

            _button.transform.GetChild(1).GetComponent<Image>().sprite = floor.Image;
            _mat = floor.Mat;
        }

        public void SetFurniture(Furniture newFur, DecorButton decorButton)
        {
            _buttonDecor = decorButton;
            
            _button.transform.GetChild(1).GetComponent<Image>().sprite = newFur.Image;
            _furniture = newFur;
            furnitureObject = NewFurniture.instance.Furniture.Find(x => x.CompareTag(_furniture.Id));
        }

        public void SetType(Type type)
        {
            _type = type;
        }
    }
}
