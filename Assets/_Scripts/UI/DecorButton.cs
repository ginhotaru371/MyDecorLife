using System;
using System.Collections.Generic;
using _Scripts.Decor;
using UnityEngine;
using UnityEngine.UI;
using Type = _Scripts.Decor.Type;

namespace _Scripts.UI
{
    public class DecorButton : MonoBehaviour
    {
        private Button btn;
        
        public Type type;
        public Level level;

        [SerializeField] private bool _isDone;

        private void Awake()
        {
            btn = gameObject.GetComponent<Button>();
            btn.onClick.AddListener(Show);
        }

        private void Show()
        {
            List<Furniture> newFurnitures = new List<Furniture>();
            switch (type)
            {
                case Type.Wall:
                    DecorButtonGroup.instance.Hide();
                    ButtonGroup.instance.HideCompleteButton();
                    ButtonGroup.instance.Hide();
                    ButtonGroup.instance.Show(level.wallColor, this);
                    break;
                case Type.Floor:
                    DecorButtonGroup.instance.Hide();
                    ButtonGroup.instance.HideCompleteButton();
                    ButtonGroup.instance.Hide();
                    ButtonGroup.instance.Show(level.floorMat, this);
                    break;
                case Type.Wardrobe:
                    
                    foreach (var child in level.furnitures)
                    {
                        if (child.Type == Type.Wardrobe)
                        {
                            newFurnitures.Add(child);
                        }
                    }
                
                    DecorButtonGroup.instance.Hide();
                    ButtonGroup.instance.HideCompleteButton();
                    ButtonGroup.instance.Hide();
                    ButtonGroup.instance.Show(newFurnitures, this);
                    break;
                case Type.Bed:
                    foreach (var child in level.furnitures)
                    {
                        if (child.Type == Type.Bed)
                        {
                            newFurnitures.Add(child);
                        }
                    }
                
                    DecorButtonGroup.instance.Hide();
                    ButtonGroup.instance.HideCompleteButton();
                    ButtonGroup.instance.Hide();
                    ButtonGroup.instance.Show(newFurnitures, this);
                    break;
                case Type.WallShelf:
                    foreach (var child in level.furnitures)
                    {
                        if (child.Type == Type.WallShelf)
                        {
                            newFurnitures.Add(child);
                        }
                    }
                
                    DecorButtonGroup.instance.Hide();
                    ButtonGroup.instance.HideCompleteButton();
                    ButtonGroup.instance.Hide();
                    ButtonGroup.instance.Show(newFurnitures, this);
                    break;
                case Type.Shelf:
                    foreach (var child in level.furnitures)
                    {
                        if (child.Type == Type.Shelf)
                        {
                            newFurnitures.Add(child);
                        }
                    }
                
                    DecorButtonGroup.instance.Hide();
                    ButtonGroup.instance.HideCompleteButton();
                    ButtonGroup.instance.Hide();
                    ButtonGroup.instance.Show(newFurnitures, this);
                    break;
                case Type.BedShelf:
                    foreach (var child in level.furnitures)
                    {
                        if (child.Type == Type.BedShelf)
                        {
                            newFurnitures.Add(child);
                        }
                    }
                
                    DecorButtonGroup.instance.Hide();
                    ButtonGroup.instance.HideCompleteButton();
                    ButtonGroup.instance.Hide();
                    ButtonGroup.instance.Show(newFurnitures, this);
                    break;
                case Type.Carpet:
                    foreach (var child in level.furnitures)
                    {
                        if (child.Type == Type.Carpet)
                        {
                            newFurnitures.Add(child);
                        }
                    }
                
                    DecorButtonGroup.instance.Hide();
                    ButtonGroup.instance.HideCompleteButton();
                    ButtonGroup.instance.Hide();
                    ButtonGroup.instance.Show(newFurnitures, this);
                    break;
                case Type.Sofa:
                    foreach (var child in level.furnitures)
                    {
                        if (child.Type == Type.Sofa)
                        {
                            newFurnitures.Add(child);
                        }
                    }
                
                    DecorButtonGroup.instance.Hide();
                    ButtonGroup.instance.HideCompleteButton();
                    ButtonGroup.instance.Hide();
                    ButtonGroup.instance.Show(newFurnitures, this);
                    break;
                case Type.Window:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetBool(bool done)
        {
            _isDone = done;
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            gameObject.SetActive(true);
            DecorButtonGroup.instance.Check();
        }

        public bool GetBool()
        {
            return _isDone;
        }
    }
}
