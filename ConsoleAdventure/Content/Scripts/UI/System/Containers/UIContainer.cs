using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System.Containers
{
    public abstract class UIContainer : UIElement
    {
        internal ObservableCollection<UIElement> elements = new ObservableCollection<UIElement>();
        internal List<UIElement> focusableElements = new List<UIElement>();

        internal int currentlySelected;

        internal bool isFocused;

        public Action<UIContainer> onBackButtonPressed = new Action<UIContainer>((UIContainer e) => {});

        public UIContainer(Point screenPosition, Point size = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, anchor, zOrder)
        {
            elements.CollectionChanged += UpdateFocusableElements;
        }

        private void UpdateFocusableElements(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            focusableElements = elements.ToList().FindAll(e => e.IsFocusable() && e.IsVisible());
        }

        public virtual UIElement AddElement(UIElement element)
        {
            element.SetParent(this);
            elements.Add(element);
            return element;
        }

        public virtual UIElement RemoveElement(UIElement element)
        {
            element.SetParent(null);
            elements.Remove(element);
            return element;
        }

        public List<UIElement> GetElementsByName(string name)
        {
            return elements.Where((UIElement element) => { return element.GetName() == name; }).ToList();
        }

        public UIElement GetFirstElementWithName(string name)
        {
            return GetElementsByName(name).FirstOrDefault(defaultValue: null);
        }

        public UIElement GetChild(int num)
        {
            return elements[num];
        }

        public List<UIElement> GetChilds()
        {
            return elements.ToList();
        }

        public virtual List<UIElement> ClearChilds()
        {
            List<UIElement> list = elements.ToList();
            elements.Clear();
            return list;
        }

        public override void Update()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].IsVisible())
                {
                    elements[i].Update();
                }
            }

            if (!isFocused) return;
            if (Input.IsKeyDown(InputConfig.NavigationSelect) && !Input.IsOldKeyDown(InputConfig.NavigationSelect))
            {
                if (currentlySelected > focusableElements.Count-1) return;
                focusableElements[currentlySelected].OnConfirmKeyDown();
            }

            if (!Input.IsKeyDown(InputConfig.NavigationSelect) && Input.IsOldKeyDown(InputConfig.NavigationSelect))
            {
                if (currentlySelected > focusableElements.Count-1) return;
                focusableElements[currentlySelected].OnConfirmKeyUp();
            }

            if (Input.OnClick(InputConfig.NavigationBeck))
            {
                onBackButtonPressed.Invoke(this);
            }
        }
    }
}
