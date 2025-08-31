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

        public UIContainer(Point screenPosition, Point size = new(), Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, margin, anchor, zOrder)
        {
            elements.CollectionChanged += UpdateFocusableElements;
        }

        private void UpdateFocusableElements(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            focusableElements = elements.ToList().FindAll(e => e.IsFocusable() && e.IsVisible());
        }

        public virtual UIElement AddElement(UIElement element)
        {
            element.parent = this;
            elements.Add(element);
            return element;
        }

        public virtual UIElement RemoveElement(UIElement element)
        {
            element.parent = null;
            elements.Remove(element);
            return element;
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
                focusableElements[currentlySelected].OnConfirmKeyDown();
            }

            if (!Input.IsKeyDown(InputConfig.NavigationSelect) && Input.IsOldKeyDown(InputConfig.NavigationSelect))
            {
                focusableElements[currentlySelected].OnConfirmKeyUp();
            }

            if (Input.OnClick(InputConfig.NavigationBeck))
            {
                onBackButtonPressed.Invoke(this);
            }
        }
    }
}
